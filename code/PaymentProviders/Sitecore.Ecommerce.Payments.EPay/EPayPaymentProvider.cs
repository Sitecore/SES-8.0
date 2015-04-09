// -------------------------------------------------------------------------------------------
// <copyright file="EPayPaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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

namespace Sitecore.Ecommerce.Payments.EPay
{
  using System;
  using System.Collections.Specialized;
  using System.Web;
  using Diagnostics;
  using DomainModel.Payments;
  using Prices;
  using Services;

  /// <summary>
  /// The EPay payment provider.
  /// </summary>
  public class EPayPaymentProvider : OnlinePaymentProvider, IReservable
  {
    /// <summary>
    /// Identifies the reservable behavior of the payment provider.
    /// </summary>
    protected readonly string ReservableBehavior = "0";

    /// <summary>
    /// Begins the payment.
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment Args.</param>
    public override void Invoke([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs)
    {
      Assert.IsNotNull(paymentSystem, "Payment method is null");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");
      Assert.IsNotNull(paymentArgs.ShoppingCart, "Shopping cart is null");

      PaymentSettingsReader configuration = new PaymentSettingsReader(paymentSystem);

      string amount = paymentArgs.ShoppingCart.Totals.TotalPriceIncVat.ToCents();
      string currency = this.Currency(paymentArgs.ShoppingCart.Currency.Code);
      string orderid = paymentArgs.ShoppingCart.OrderNumber;
      string password = paymentSystem.Password;
      string encryptedString = Crypto.GetMD5Hash(String.Concat(currency, amount, orderid, password));

      string merchantnumber = paymentSystem.Username;
      string acceptUrl = paymentArgs.PaymentUrls.ReturnPageUrl;
      string declineUrl = acceptUrl + PaymentConstants.CancelQueryString;
      string language = this.Language(Sitecore.Context.Language.Name);

      NameValueCollection data = new NameValueCollection
      {
        { "merchantnumber", merchantnumber },
        { "amount", amount },
        { "currency", currency },
        { "orderid", orderid },
        { "accepturl", acceptUrl },
        { "declineurl", declineUrl },
        { "language", language },
        { "ordretext", paymentArgs.Description },
        { "description", paymentArgs.Description },
        { "MD5Key", encryptedString },
      };

      data.Add(configuration.GetProviderSettings());

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      transactionDataProvider.SaveStartValues(orderid, amount, currency, paymentSystem.Code);
      transactionDataProvider.SavePersistentValue(orderid, TransactionConstants.TotalAmount, amount);

      this.PostData(paymentSystem.PaymentUrl, data);
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
      string instantcapture = configuration.GetSetting("instantcapture");

      if (request.QueryString[PaymentConstants.ActionKey] != PaymentConstants.CancelResponse)
      {
        string transactionNumber = request.QueryString["tid"];
        string cardid = request.QueryString["cardid"];
        string currency = request.QueryString["cur"];
        string orderid = request.QueryString["orderid"];
        string amount = request.QueryString["amount"];
        string hashString = request.QueryString["eKey"];
        string date = request.QueryString["date"];

        if (!this.CallBackIsvalid(paymentSystem, paymentArgs, currency, transactionNumber, amount, orderid, hashString, date))
        {
          Log.Error("Callback parameters are invalid.", this);
        }
        else
        {
          this.PaymentStatus = PaymentStatus.Succeeded;
          transactionDataProvider.SaveCallBackValues(orderid, this.PaymentStatus.ToString(), transactionNumber, amount, currency, string.Empty, string.Empty, string.Empty, cardid);

          if (string.Compare(instantcapture, this.ReservableBehavior, StringComparison.OrdinalIgnoreCase) == 0)
          {
            ReservationTicket reservationTicket = new ReservationTicket
            {
              InvoiceNumber = orderid,
              AuthorizationCode = hashString,
              TransactionNumber = transactionNumber,
              Amount = amount.FromCents()
            };
            transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, PaymentConstants.ReservationTicket, reservationTicket);
          }
        }
      }
      else
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

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();

      Payment payment = new Payment();
      int pbsResponse = 0;
      int epayresponse = 0;
      bool captureResult = payment.capture(int.Parse(paymentSystem.Username), long.Parse(reservationTicket.TransactionNumber), int.Parse(amount.ToCents()), string.Empty, paymentSystem.Password, ref pbsResponse, ref epayresponse);
      transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, captureResult ? PaymentConstants.CaptureSuccess : string.Format("pbsResponse={0} epayresponse={1}", pbsResponse, epayresponse));
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

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();

      Payment payment = new Payment();
      int pbsResponse = 0;
      bool deleteResult = payment.delete(int.Parse(paymentSystem.Username), long.Parse(reservationTicket.TransactionNumber), string.Empty, paymentSystem.Password, ref pbsResponse);
      transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, deleteResult ? PaymentConstants.CancelSuccess : string.Format("pbsResponse={0}", pbsResponse));
    }

    /// <summary>
    /// Currencies the specified currency code.
    /// </summary>
    /// <param name="currencyCode">The currency code.</param>
    /// <returns>
    /// The specified currency code.
    /// </returns>
    [CanBeNull]
    protected override string Currency([NotNull] string currencyCode)
    {
      Assert.ArgumentNotNull(currencyCode, "currencyCode");

      switch (currencyCode.ToLower())
      {
        // Danish kroner (DKK)
        case "dkk":
          return "208";

        // Euro (EUR)
        case "euro":
          return "978";

        // US Dollar $ (USD)
        case "usd":
          return "840";

        // English Pound £ (GBP)
        case "gbp":
          return "826";

        // Swedish Kroner (SEK)
        case "sek":
          return "752";

        // Norwegian Kroner (SEK)
        case "nok":
          return "756";
        default:
          return currencyCode;
      }
    }

    /// <summary>
    /// Languages the specified language code.
    /// </summary>
    /// <param name="languageCode">The language code.</param>
    /// <returns>
    /// The specified language code.
    /// </returns>
    [CanBeNull]
    protected virtual string Language([NotNull] string languageCode)
    {
      Assert.ArgumentNotNull(languageCode, "languageCode");

      switch (languageCode.ToLower())
      {
        case "dk":
          return "1";
        case "sv":
          return "3";
        case "no":
          return "4";
        case "en":
          return "2";
        case "is":
          return "6";
        case "gr":
          return "5";
        default:
          return null;
      }
    }

    /// <summary>
    /// Calls the back isvalid.
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment Args.</param>
    /// <param name="currency">The currency.</param>
    /// <param name="transactionNumber">The transaction number.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="hashString">The hash string.</param>
    /// <param name="date">The current date.</param>
    /// <returns>
    /// the back is valid.
    /// </returns>
    protected bool CallBackIsvalid(PaymentSystem paymentSystem, PaymentArgs paymentArgs, string currency, string transactionNumber, string amount, string orderId, string hashString, string date)
    {
      if (string.IsNullOrEmpty(currency) || string.IsNullOrEmpty(transactionNumber) || string.IsNullOrEmpty(amount) || string.IsNullOrEmpty(orderId))
      {
        return false;
      }

      string currentData = DateTime.Now.ToString("yyyyMMdd");
      return orderId.Contains(paymentArgs.ShoppingCart.OrderNumber) && currentData.Equals(date);
    }
  }
}