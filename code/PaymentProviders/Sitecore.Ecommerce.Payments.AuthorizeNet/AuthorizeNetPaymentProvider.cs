// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorizeNetPaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The Authorize.Net payment provider.
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

namespace Sitecore.Ecommerce.Payments.AuthorizeNet
{
  using System;
  using System.Collections.Specialized;
  using System.Globalization;
  using System.Security.Cryptography;
  using System.Text;
  using System.Web;
  using Diagnostics;
  using DomainModel.Payments;
  using Prices;
  using Utils;
  using CaptureRequest = global::AuthorizeNet.CaptureRequest;
  using Gateway = global::AuthorizeNet.Gateway;
  using VoidRequest = global::AuthorizeNet.VoidRequest;

  /// <summary>
  /// The Authorize.Net payment provider.
  /// </summary>
  public class AuthorizeNetPaymentProvider : OnlinePaymentProviderBase<PaymentSystem>, IReservable
  {
    /// <summary>
    /// Determines the authorization request.
    /// </summary>
    protected readonly string ReservableTransactionType = "auth_only";

    /// <summary>
    /// Begins the payment.
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment arguments.</param>
    public override void Invoke([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");

      base.Invoke(paymentSystem, paymentArgs);

      string sequence = new Random().Next(0, 1000).ToString();
      string timeStamp = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
      string currency = this.Currency(paymentArgs.ShoppingCart.Currency.Code);

      string fingerprint = this.HmacMD5(paymentSystem.Password, paymentSystem.Username + "^" + sequence + "^" + timeStamp + "^" + paymentArgs.ShoppingCart.Totals.TotalPriceIncVat.RoundToCents() + "^" + currency);


      NameValueCollection data = new NameValueCollection
      {
        { "x_login", paymentSystem.Username },
        { "x_invoice_num", paymentArgs.ShoppingCart.OrderNumber },
        { "x_po_num", paymentArgs.ShoppingCart.OrderNumber },
        { "x_receipt_link_url", paymentArgs.PaymentUrls.ReturnPageUrl },
        { "x_amount", paymentArgs.ShoppingCart.Totals.TotalPriceIncVat.RoundToCents() },
        { "x_tax", paymentArgs.ShoppingCart.Totals.TotalVat.RoundToCents() },
        { "x_currency_code", currency },
        { "x_description", paymentArgs.Description },
        { "x_fp_sequence", sequence },
        { "x_fp_timestamp", timeStamp },
        { "x_customer_ip", HttpContext.Current.Request.UserHostAddress },
        { "x_fp_hash", fingerprint },
        { "x_first_name", paymentArgs.ShoppingCart.CustomerInfo.BillingAddress.Name },
        { "x_last_name", paymentArgs.ShoppingCart.CustomerInfo.BillingAddress.Name2 },
        { "x_address", paymentArgs.ShoppingCart.CustomerInfo.BillingAddress.Address },
        { "x_city", paymentArgs.ShoppingCart.CustomerInfo.BillingAddress.City },
        { "x_state", paymentArgs.ShoppingCart.CustomerInfo.BillingAddress.State },
        { "x_zip", paymentArgs.ShoppingCart.CustomerInfo.BillingAddress.Zip },
        { "x_country", paymentArgs.ShoppingCart.CustomerInfo.BillingAddress.Country.Title },
        { "x_ship_to_first_name", paymentArgs.ShoppingCart.CustomerInfo.ShippingAddress.Name },
        { "x_ship_to_last_name", paymentArgs.ShoppingCart.CustomerInfo.ShippingAddress.Name2 },
        { "x_ship_to_address", paymentArgs.ShoppingCart.CustomerInfo.ShippingAddress.Address },
        { "x_ship_to_city", paymentArgs.ShoppingCart.CustomerInfo.ShippingAddress.City },
        { "x_ship_to_state", paymentArgs.ShoppingCart.CustomerInfo.ShippingAddress.State },
        { "x_ship_to_zip", paymentArgs.ShoppingCart.CustomerInfo.ShippingAddress.Zip },
        { "x_ship_to_country", paymentArgs.ShoppingCart.CustomerInfo.ShippingAddress.Country.Title },
        { "x_phone", paymentArgs.ShoppingCart.CustomerInfo.Phone },
        { "x_fax", paymentArgs.ShoppingCart.CustomerInfo.Fax },
        { "x_email", paymentArgs.ShoppingCart.CustomerInfo.Email },
        { "x_header_email_receipt", paymentArgs.ShoppingCart.CustomerInfo.Email },
      };

      PaymentSettingsReader paymentSettingsReader = new PaymentSettingsReader(paymentSystem);
      data.Add(paymentSettingsReader.GetProviderSettings());

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      transactionDataProvider.SaveStartValues(paymentArgs.ShoppingCart.OrderNumber, paymentArgs.ShoppingCart.Totals.TotalPriceIncVat.RoundToCents(), currency, paymentSystem.Code);

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
      string transactionType = configuration.GetSetting("x_type");

      if (request.QueryString[PaymentConstants.ActionKey] != PaymentConstants.CancelResponse)
      {
        string transactionId = request.Form["x_split_tender_id"] ?? request.Form["x_trans_id"];
        string invoiceNumber = request.Form["x_invoice_num"];
        string authorizationCode = request.Form["x_auth_code"];
        string totalPrice = request.Form["x_amount"];
        string responseCode = request.Form["x_response_code"];
        string responseReasonCode = request.Form["x_response_reason_code"];
        string responseReasonText = request.Form["x_response_reason_text"];
        string method = request.Form["x_method"];
        string hash = request.Form["x_MD5_Hash"];

        string hashMD5 = Crypto.GetMD5Hash(paymentSystem.Password + paymentSystem.Username + transactionId + totalPrice);

        if (!string.IsNullOrEmpty(hash) && !string.IsNullOrEmpty(hashMD5) && string.Equals(hashMD5, hash, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(responseCode) && responseCode.Equals("1"))
        {
          this.PaymentStatus = PaymentStatus.Succeeded;
          transactionDataProvider.SaveCallBackValues(paymentArgs.ShoppingCart.OrderNumber, PaymentStatus.Succeeded.ToString(), transactionId, totalPrice, string.Empty, responseCode, responseReasonCode, responseReasonText, method);

          if (string.Compare(transactionType, this.ReservableTransactionType, StringComparison.OrdinalIgnoreCase) == 0)
          {
            ReservationTicket reservationTicket = new ReservationTicket
            {
              InvoiceNumber = invoiceNumber,
              AuthorizationCode = authorizationCode,
              TransactionNumber = transactionId,
              Amount = TypeUtil.TryParse(totalPrice, decimal.Zero)
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
    /// Cancels the payment reservation.
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment arguments.</param>
    /// <param name="reservationTicket">The reservation ticket.</param>
    public void CancelReservation([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs, [NotNull] ReservationTicket reservationTicket)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");
      Assert.ArgumentNotNull(reservationTicket, "reservationTicket");

      PaymentSettingsReader configuration = new PaymentSettingsReader(paymentSystem);
      bool isTest;
      Boolean.TryParse(configuration.GetSetting("x_test_request"), out isTest);
      var request = new VoidRequest(reservationTicket.TransactionNumber);
      var gate = new Gateway(paymentSystem.Username, paymentSystem.Password, isTest);

      var response = gate.Send(request);

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, response.Approved ? PaymentConstants.CancelSuccess : response.Message);
    }

    /// <summary>
    /// Captures the payment
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
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
      bool isTest;
      Boolean.TryParse(configuration.GetSetting("x_test_request"), out isTest);
      var request = new CaptureRequest(amount, reservationTicket.TransactionNumber, reservationTicket.AuthorizationCode);
      var gate = new Gateway(paymentSystem.Username, paymentSystem.Password, isTest);

      var response = gate.Send(request);

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, response.Approved ? PaymentConstants.CaptureSuccess : response.Message);
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
        // Danish kroner (DKK)
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

        // Norwegian Kroner (SEK)
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
    /// the specified language code.
    /// </returns>
    [CanBeNull]
    protected virtual string Language([NotNull] string languageCode)
    {
      Assert.ArgumentNotNull(languageCode, "languageCode");

      switch (languageCode.ToLower())
      {
        case "sv":
          return "se";
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
        case "it":
          return "it";
        case "fo":
          return "fo";
        case "pl":
          return "pl";
        case "da":
          return "da";
        default:
          return null;
      }
    }

    /// <summary>
    /// HMAs the c_ M d5.
    /// </summary>
    /// <param name="key">The string key.</param>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The MD5 code.
    /// </returns>
    [NotNull]
    protected virtual string HmacMD5([NotNull] string key, [NotNull] string value)
    {
      // The first two lines take the input values and convert them from strings to Byte arrays
      Assert.ArgumentNotNull(key, "key");
      Assert.ArgumentNotNull(value, "value");

      byte[] encKey = (new ASCIIEncoding()).GetBytes(key);
      byte[] encData = (new ASCIIEncoding()).GetBytes(value);

      // create a HMACMD5 object with the key set
      HMACMD5 myhmacMD5 = new HMACMD5(encKey);

      // calculate the hash (returns a byte array)
      byte[] hash = myhmacMD5.ComputeHash(encData);

      // loop through the byte array and add append each piece to a string to obtain a hash string
      string fingerprint = string.Empty;
      for (int i = 0; i < hash.Length; i++)
      {
        fingerprint += hash[i].ToString("x").PadLeft(2, '0');
      }

      return fingerprint;
    }
  }
}