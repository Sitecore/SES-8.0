// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BbsPaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The BBS payment provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
// Copyright 2015 Sitecore Corporation A/S
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
// except in compliance with the License. You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the 
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
// either express or implied. See the License for the specific language governing permissions 
// and limitations under the License.
// -------------------------------------------------------------------------------------------

namespace Sitecore.Ecommerce.Payments.Bbs
{
  using System;
  using System.Collections.Specialized;
  using System.Web;
  using Diagnostics;
  using DomainModel.Payments;
  using Prices;
  using Services;

  /// <summary>
  /// The BBS payment provider.
  /// </summary>
  public class BbsPaymentProvider : OnlinePaymentProvider, IReservable
  {
    /// <summary>
    /// Determines the authorization request.
    /// </summary>
    protected readonly string ReservableTransactionType = "auth";

    /// <summary>
    /// Begins the payment.
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment arguments.</param>
    public override void Invoke([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs)
    {
      Assert.IsNotNull(paymentSystem, "Payment method is null");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");
      Assert.IsNotNull(paymentArgs.ShoppingCart, "Shopping cart is null");

      PaymentSettingsReader configuration = new PaymentSettingsReader(paymentSystem);

      string merchantId = paymentSystem.Username;
      string token = paymentSystem.Password;
      string amount = paymentArgs.ShoppingCart.Totals.TotalPriceIncVat.ToCents();
      string currencyCode = this.Currency(paymentArgs.ShoppingCart.Currency.Code);
      string orderNumber = paymentArgs.ShoppingCart.OrderNumber;
      string redirectUrl = paymentArgs.PaymentUrls.ReturnPageUrl;
      string paymentSystemCode = paymentSystem.Code;
      string webServicePlatform = configuration.GetSetting("webServicePlatform");
      string language = configuration.GetSetting("language");

      RegisterRequest request = new RegisterRequest
      {
        Order = new Order
        {
          Amount = amount,
          OrderNumber = orderNumber,
          CurrencyCode = currencyCode
        },
        Terminal = new Terminal
        {
          Language = language,
          RedirectUrl = redirectUrl
        },
        Environment = new Services.Environment
        {
          WebServicePlatform = webServicePlatform
        }
      };

      Netaxept client = new Netaxept();

      try
      {
        RegisterResponse response = client.Register(merchantId, token, request);
        NameValueCollection data = new NameValueCollection
      {
        { "MerchantID", merchantId }, 
        { "TransactionID", response.TransactionId }
      };

        ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
        transactionDataProvider.SaveStartValues(orderNumber, amount, currencyCode, paymentSystemCode);

        this.PostData(paymentSystem.PaymentUrl, data);
      }
      catch (Exception exception)
      {
        Log.Error(exception.Message, this);
        HttpContext.Current.Session["paymentErrorMessage"] = exception.Message;
        HttpContext.Current.Response.Redirect(paymentArgs.PaymentUrls.FailurePageUrl, false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
      }
    }

    /// <summary>
    /// Processes the callback in terms of the HttpRequest and extracts either hidden form fields, or querystring parameters
    /// that is returned from the payment provider.
    /// Determines the payment status and saves that indication for later, could be in session, in db, or other storage.
    /// This information is important and used in the GetPaymentStatus().
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="paymentArgs">The payment args.</param>
    public override void ProcessCallback([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");
      Assert.IsNotNull(paymentArgs.ShoppingCart, "Shopping cart is null");

      this.PaymentStatus = PaymentStatus.Failure;

      HttpRequest request = HttpContext.Current.Request;
      PaymentSettingsReader configuration = new PaymentSettingsReader(paymentSystem);
      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      string operation = configuration.GetSetting("operation");

      string transactionId = request.QueryString["transactionId"];
      string responseCode = request.QueryString["responseCode"];

      if (string.Compare(responseCode, "OK", StringComparison.OrdinalIgnoreCase) == 0)
      {
        string merchantId = paymentSystem.Username;
        string token = paymentSystem.Password;
        string orderNumber = paymentArgs.ShoppingCart.OrderNumber;
        decimal amount = paymentArgs.ShoppingCart.Totals.TotalPriceIncVat;
        string currency = this.Currency(paymentArgs.ShoppingCart.Currency.Code);

        Netaxept client = new Netaxept();
        ProcessRequest processRequest = new ProcessRequest
        {
          Operation = operation,
          TransactionId = transactionId
        };
        try
        {
          ProcessResponse processResponse = client.Process(merchantId, token, processRequest);
          if (string.Compare(processResponse.ResponseCode, "OK", StringComparison.OrdinalIgnoreCase) == 0)
          {
            this.PaymentStatus = PaymentStatus.Succeeded;
            transactionDataProvider.SaveCallBackValues(paymentArgs.ShoppingCart.OrderNumber, this.PaymentStatus.ToString(), transactionId, amount.ToString(), currency, string.Empty, string.Empty, string.Empty, string.Empty);

            if (string.Compare(operation, this.ReservableTransactionType, StringComparison.OrdinalIgnoreCase) == 0)
            {
              ReservationTicket reservationTicket = new ReservationTicket
              {
                Amount = amount,
                AuthorizationCode = processResponse.AuthorizationId,
                InvoiceNumber = orderNumber,
                TransactionNumber = transactionId
              };
              transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, PaymentConstants.ReservationTicket, reservationTicket);
            }
          }
        }
        catch (Exception exception)
        {
          Log.Error(exception.Message, exception, this);
        }
      }
      else if (string.Compare(responseCode, "CANCEL", StringComparison.OrdinalIgnoreCase) == 0)
      {
        this.PaymentStatus = PaymentStatus.Canceled;
      }

      if (this.PaymentStatus != PaymentStatus.Succeeded)
      {
        transactionDataProvider.SavePersistentValue(paymentArgs.ShoppingCart.OrderNumber, TransactionConstants.PaymentStatus, this.PaymentStatus.ToString());
      }
    }

    /// <summary>
    /// Captures the payment
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="paymentArgs">The payment args.</param>
    /// <param name="reservationTicket">The reservation ticket.</param>
    /// <param name="amount">The amount.</param>
    public void Capture([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs, [NotNull] ReservationTicket reservationTicket, decimal amount)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");
      Assert.ArgumentNotNull(reservationTicket, "reservationTicket");

      Assert.ArgumentCondition((amount > 0), "amount", "An amount should be greater than zero");
      Assert.ArgumentCondition((amount <= reservationTicket.Amount), "amount", "An amount should not be greater than the original reserved one");

      const string Operation = "CAPTURE";
      string orderNumber = reservationTicket.InvoiceNumber;
      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();

      try
      {
        ProcessResponse processResponse = this.Process(paymentSystem, reservationTicket, amount, Operation);
        transactionDataProvider.SavePersistentValue(orderNumber, processResponse.ResponseCode == "OK" ? PaymentConstants.CaptureSuccess : processResponse.ResponseText);
      }
      catch (Exception exception)
      {
        Log.Warn(exception.Message, exception, this);
        transactionDataProvider.SavePersistentValue(orderNumber, exception.Message);
      }
    }

    /// <summary>
    /// Cancels the payment reservation
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="paymentArgs">The payment args.</param>
    /// <param name="reservationTicket">The reservation ticket.</param>
    public void CancelReservation([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs, [NotNull] ReservationTicket reservationTicket)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");
      Assert.ArgumentNotNull(reservationTicket, "reservationTicket");

      const string Operation = "ANNUL";
      string orderNumber = reservationTicket.InvoiceNumber;
      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();

      try
      {
        ProcessResponse processResponse = this.Process(paymentSystem, reservationTicket, reservationTicket.Amount, Operation);
        transactionDataProvider.SavePersistentValue(orderNumber, processResponse.ResponseCode == "OK" ? PaymentConstants.CancelSuccess : processResponse.ResponseText);
      }
      catch (Exception exception)
      {
        Log.Warn(exception.Message, exception, this);
        transactionDataProvider.SavePersistentValue(orderNumber, exception.Message);
      }
    }

    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="reservationTicket">The reservation ticket.</param>
    /// <param name="operation">The operation.</param>
    /// <returns>The <see cref="ProcessResponse"/>.</returns>
    [NotNull]
    protected ProcessResponse Process([NotNull] PaymentSystem paymentSystem, [NotNull] ReservationTicket reservationTicket, [NotNull] string operation)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(reservationTicket, "reservationTicket");
      Assert.ArgumentNotNull(operation, "operation");

      return this.Process(paymentSystem, reservationTicket, reservationTicket.Amount, operation);
    }

    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="reservationTicket">The reservation ticket.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="operation">The operation.</param>
    /// <returns>The <see cref="ProcessResponse"/>.</returns>
    [NotNull]
    protected ProcessResponse Process([NotNull] PaymentSystem paymentSystem, [NotNull] ReservationTicket reservationTicket, decimal amount, [NotNull] string operation)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(reservationTicket, "reservationTicket");
      Assert.ArgumentNotNull(operation, "operation");

      string merchantId = paymentSystem.Username;
      string token = paymentSystem.Password;
      string transactionId = reservationTicket.TransactionNumber;
      string transactionAmount = amount.ToCents();

      Netaxept client = new Netaxept();
      ProcessRequest processRequest = new ProcessRequest
      {
        Operation = operation,
        TransactionId = transactionId,
        TransactionAmount = transactionAmount
      };

      return client.Process(merchantId, token, processRequest);
    }

    /// <summary>
    /// Currencies the specified currency code.
    /// </summary>
    /// <param name="currencyCode">The currency code.</param>
    /// <returns>
    /// The specified currency code.
    /// </returns>
    [NotNull]
    protected override string Currency([NotNull] string currencyCode)
    {
      Assert.ArgumentNotNull(currencyCode, "currencyCode");

      switch (currencyCode.ToLower())
      {
        case "dkk":
          return "DKK";

        // Euro (EUR)
        case "euro":
          return "EUR";

        // US Dollar $ (USD)
        case "usd":
          return "USD";

        // English Pound £ (GBP)
        case "gbp":
          return "GBP";

        // Swedish Kroner (SEK)
        case "sek":
          return "SEK";

        // Australian Dollar (AUD)
        case "aud":
          return "AND";

        // Norwegian Kroner (NOK)
        case "nok":
          return "NOK";

        default:
          return currencyCode;
      }
    }

    /// <summary>
    /// Languages the specified language code.
    /// </summary>
    /// <param name="languageCode">The language code.</param>
    /// <returns>
    /// The specified currency code.
    /// </returns>
    [CanBeNull]
    protected virtual string Language([NotNull] string languageCode)
    {
      Assert.ArgumentNotNull(languageCode, "languageCode");

      switch (languageCode.ToLower())
      {
        // English
        case "en":
          return "en_GB";

        // Norwegian
        case "no":
          return "no_NO";

        // Swedish
        case "sv":
          return "sv_SV";

        default:
          return null;
      }
    }
  }
}