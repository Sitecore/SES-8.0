// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DibsPaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//    The DIBS payment provider.
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

namespace Sitecore.Ecommerce.Payments.Dibs
{
  using System;
  using System.Collections.Specialized;
  using System.Net;
  using System.Text;
  using System.Web;
  using Diagnostics;
  using DomainModel.Payments;
  using Prices;

  /// <summary>
  /// The DIBS payment provider.
  /// </summary>
  public class DibsPaymentProvider : OnlinePaymentProviderBase<PaymentSystem>, IReservable
  {
    /// <summary>
    /// Custom Sitecore settings for Dibs provider.
    /// </summary>
    protected readonly string[] CustomSettings = { "key1", "key2", "captureUrl", "cancelReservationUrl" };

    /// <summary>
    /// Begins the payment.
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment arguments.</param>
    public override void Invoke([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");

      PaymentSettingsReader configuration = new PaymentSettingsReader(paymentSystem);

      string merchantId = paymentSystem.Username;
      string ordernumber = paymentArgs.ShoppingCart.OrderNumber;
      string amount = paymentArgs.ShoppingCart.Totals.TotalPriceIncVat.ToCents();
      string currency = this.Currency(paymentArgs.ShoppingCart.Currency.Code);
      string key1 = configuration.GetSetting("key1");
      string key2 = configuration.GetSetting("key2");
      string concatanatedString = string.Format("merchant={0}&orderid={1}&currency={2}&amount={3}", merchantId, ordernumber, currency, amount);

      string acceptUrl = paymentArgs.PaymentUrls.ReturnPageUrl;
      string cancelUrl = acceptUrl + PaymentConstants.CancelQueryString;

      NameValueCollection data = new NameValueCollection
      {
        { "merchant", paymentSystem.Username },
        { "amount", amount },
        { "currency", currency },
        { "orderid", ordernumber },
        { "accepturl", acceptUrl },
        { "cancelurl", cancelUrl },
        { "ip", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] },
        { "lang", this.Language(Sitecore.Context.Language.Name) },
        { "ordertext", paymentArgs.Description },
        { "delivery1.Navn", paymentArgs.ShoppingCart.CustomerInfo.BillingAddress.Name },
        { "delivery2.Adresse", paymentArgs.ShoppingCart.CustomerInfo.BillingAddress.Address },
        { "delivery3.Kommentar", string.Empty },
        { "md5key", this.CalculateMd5Key(key1, key2, concatanatedString) },
      };

      data.Add(configuration.GetProviderSettings(this.CustomSettings));

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      transactionDataProvider.SaveStartValues(ordernumber, amount, currency, paymentSystem.Code);

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
      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      PaymentSettingsReader configuration = new PaymentSettingsReader(paymentSystem);
      bool capturenow = !string.IsNullOrEmpty(configuration.GetSetting("capturenow"));

      if (request[PaymentConstants.ActionKey] != PaymentConstants.CancelResponse)
      {
        string transactionNumber = request["transact"];
        string amount = request["amount"];
        string merchant = request["merchant"];
        string orderid = request["orderid"];
        string authkey = request["authkey"];
        string cardType = request["paytype"] ?? string.Empty;

        if (this.CallBackIsvalid(paymentSystem, paymentArgs, transactionNumber, amount, merchant, orderid))
        {
          this.PaymentStatus = PaymentStatus.Succeeded;
          transactionDataProvider.SaveCallBackValues(orderid, this.PaymentStatus.ToString(), transactionNumber, amount, string.Empty, string.Empty, string.Empty, string.Empty, cardType);

          if (!capturenow)
          {
            ReservationTicket reservationTicket = new ReservationTicket
            {
              InvoiceNumber = orderid,
              AuthorizationCode = authkey,
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

      PaymentSettingsReader configuration = new PaymentSettingsReader(paymentSystem);

      string key1 = configuration.GetSetting("key1");
      string key2 = configuration.GetSetting("key2");
      string url = configuration.GetSetting("captureUrl");

      string merchant = paymentSystem.Username;
      string password = paymentSystem.Password;

      string orderId = reservationTicket.InvoiceNumber;
      string transact = reservationTicket.TransactionNumber;
      string amountInput = amount.ToCents();
      const string Textreply = "yes";
      const string Fullreply = "yes";

      string forHash = string.Format("merchant={0}&orderid={1}&transact={2}&amount={3}", merchant, orderId, transact, amountInput);
      string md5Key = this.CalculateMd5Key(key1, key2, forHash);

      NameValueCollection data = new NameValueCollection
      {
        { "merchant", merchant },
        { "orderid", orderId },
        { "transact", transact },
        { "amount", amountInput },
        { "md5key", md5Key },
        { "textreply", Textreply },
        { "fullreply", Fullreply },
      };

      NameValueCollection result = new NameValueCollection();
      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();

      transactionDataProvider.SavePersistentValue(orderId, this.SendRequest(merchant, password, url, data, ref result) ? PaymentConstants.CaptureSuccess : string.Format("UnCaptured. Reason={0}", result["reason"]));
    }

    /// <summary>
    /// Cancels the payment
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="paymentArgs">The payment args.</param>
    /// <param name="reservationTicket">The reservation ticket.</param>
    public void CancelReservation([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs, [NotNull] ReservationTicket reservationTicket)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");
      Assert.ArgumentNotNull(reservationTicket, "reservationTicket");

      PaymentSettingsReader configuration = new PaymentSettingsReader(paymentSystem);

      string key1 = configuration.GetSetting("key1");
      string key2 = configuration.GetSetting("key2");
      string url = configuration.GetSetting("cancelReservationUrl");

      string merchant = paymentSystem.Username;
      string password = paymentSystem.Password;

      string orderId = reservationTicket.InvoiceNumber;
      string transact = reservationTicket.TransactionNumber;
      const string Textreply = "yes";
      const string Fullreply = "yes";

      string forHash = string.Format("merchant={0}&orderid={1}&transact={2}", merchant, orderId, transact);
      string md5Key = this.CalculateMd5Key(key1, key2, forHash);

      NameValueCollection data = new NameValueCollection
      {
        { "merchant", merchant },
        { "orderid", orderId },
        { "transact", transact },
        { "md5key", md5Key },
        { "textreply", Textreply },
        { "fullreply", Fullreply },
      };

      NameValueCollection result = new NameValueCollection();
      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      transactionDataProvider.SavePersistentValue(orderId, this.SendRequest(merchant, password, url, data, ref result) ? PaymentConstants.CancelSuccess : string.Format("UnCanceled. Reason={0}", result["reason"]));
    }

    /// <summary>Performs request to the payment provider </summary>
    /// <param name="merchant">The merchant.</param>
    /// <param name="password">The password.</param>
    /// <param name="url">The url of the function</param>
    /// <param name="data">The input data.</param>
    /// <param name="result">The result collection of the data from the provider.</param>
    /// <returns>Boolean result of the operation</returns>
    protected bool SendRequest(string merchant, string password, string url, NameValueCollection data, ref NameValueCollection result)
    {
      bool operationResult = false;
      using (WebClient client = new WebClient())
      {
        client.Credentials = new NetworkCredential(merchant, password);
        try
        {
          byte[] bytes = client.UploadValues(url, "POST", data);
          string httpBody = Encoding.UTF8.GetString(bytes);
          result = this.SplitQueryString(httpBody);
          string status;
          if ((status = result["status"]) != null)
          {
            if (status.ToLower() == "accepted")
            {
              operationResult = true;
            }
          }
        }
        catch (Exception exception)
        {
          Log.Error(exception.Message, this);
        }
      }

      return operationResult;
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

        // Australian Dollar (AUD)
        case "aud":
          return "036";

        // Canadian Dollar (CAD)
        case "cad":
          return "124";

        // Icelandic Kroner (ISK)
        case "isk":
          return "352";

        // Japanese Yen (JPY)
        case "jpy":
          return "392";

        // New Zealand Dollar (NZD)
        case "nzd":
          return "578";

        // Norwegian Kroner (NOK)
        case "nok":
          return "756";

        // Swiss Franc (CHF)
        case "chf":
          return "949";

        // Turkish Lire (TRY)
        case "try":
          return "949";
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
        case "sv":
          return "sv";
        case "no":
          return "no";
        case "en":
          return "en";
        case "nl":
          return "nl";
        case "de":
          return "de";
        case "fr":
          return "fr";
        case "fi":
          return "fi";
        case "es":
          return "es";
        case "it":
          return "it";
        case "fo":
          return "fo";
        case "pl":
          return "pl";
        default:
          return null;
      }
    }

    /// <summary>
    /// Calculates the MD5 key.
    /// </summary>
    /// <param name="key1">The key1.</param>
    /// <param name="key2">The key2.</param>
    /// <param name="concanatedString">The concanated string.</param>
    /// <returns>
    /// the MD5 key.
    /// </returns>
    protected string CalculateMd5Key([NotNull] string key1, [NotNull] string key2, [NotNull] string concanatedString)
    {
      Assert.ArgumentNotNull(key1, "key1");
      Assert.ArgumentNotNull(key2, "key2");
      Assert.ArgumentNotNull(concanatedString, "concanatedString");

      if (string.IsNullOrEmpty(key1) || string.IsNullOrEmpty(key2))
      {
        return string.Empty;
      }

      string composedString = key1 + concanatedString;
      string firstLevelEncryptedString = Crypto.GetMD5Hash(composedString);
      return Crypto.GetMD5Hash(key2 + firstLevelEncryptedString);
    }

    /// <summary>Calls the back isvalid.</summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment Args.</param>
    /// <param name="transactionNumber">The transaction number.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="merchant">The merchant.</param>
    /// <param name="orderId">The order id.</param>
    /// <returns>The back isvalid result.</returns>
    protected bool CallBackIsvalid(PaymentSystem paymentSystem, PaymentArgs paymentArgs, string transactionNumber, string amount, string merchant, string orderId)
    {
      Assert.IsNotNull(paymentArgs.ShoppingCart, "Shopping cart is null");
      bool result = false;
      string originalTotalAmount = Context.Entity.Resolve<ITransactionData>().GetPersistentValue(paymentArgs.ShoppingCart.OrderNumber, TransactionConstants.TotalAmount) as string;
      string originalMerchantId = paymentSystem.Username;
      string originalOrderId = paymentArgs.ShoppingCart.OrderNumber;

      if (!string.IsNullOrEmpty(transactionNumber) && !string.IsNullOrEmpty(amount) && !string.IsNullOrEmpty(merchant) && !string.IsNullOrEmpty(orderId))
      {
        result = (!string.IsNullOrEmpty(originalTotalAmount) && amount.Contains(originalTotalAmount)) && merchant.Contains(originalMerchantId) &&
             orderId.Contains(originalOrderId) && !string.IsNullOrEmpty(transactionNumber);
      }

      return result;
    }

    /// <summary>
    /// Splits query string
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    /// Collection of values
    /// </returns>
    [NotNull]
    protected NameValueCollection SplitQueryString([NotNull] string input)
    {
      Assert.ArgumentNotNull(input, "input");

      NameValueCollection values = new NameValueCollection();
      string[] arrayOuter = input.Split(new[] { '&' });
      for (int i = 0; i < arrayOuter.Length; i++)
      {
        string[] arrayInner = arrayOuter[i].Split(new[] { '=' });
        if (arrayInner.Length == 2)
        {
          values.Add(arrayInner[0], arrayInner[1]);
        }
        else
        {
          values.Add(i.ToString(), arrayOuter[i]);
        }
      }

      return values;
    }
  }
}