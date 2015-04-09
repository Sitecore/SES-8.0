// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuickPayPaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The QuickPay payment provider.
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

namespace Sitecore.Ecommerce.Payments.QuickPay
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.IO;
  using System.Linq;
  using System.Net;
  using System.Security.Cryptography;
  using System.Text;
  using System.Web;
  using System.Xml;
  using Diagnostics;
  using DomainModel.Payments;
  using Prices;

  /// <summary>
  /// The QuickPay payment provider.
  /// </summary>
  public class QuickPayPaymentProvider : OnlinePaymentProvider, IReservable
  {
    /// <summary>
    /// Determines the authorization request.
    /// </summary>
    protected readonly string ReservableBehavior = "0";

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

      string protocol = configuration.GetSetting("protocol");
      string msgtype = configuration.GetSetting("msgtype");
      string merchant = paymentSystem.Username;
      string language = this.Language(Sitecore.Context.Language.Name);
      string ordernumber = paymentArgs.ShoppingCart.OrderNumber;
      string amount = paymentArgs.ShoppingCart.Totals.TotalPriceIncVat.ToCents();
      string callbackurl = paymentArgs.PaymentUrls.ReturnPageUrl;
      string currency = this.Currency(paymentArgs.ShoppingCart.Currency.Code);
      string continueurl = paymentArgs.PaymentUrls.ReturnPageUrl;
      string cancelurl = continueurl + PaymentConstants.CancelQueryString;
      string description = paymentArgs.Description;
      string ipaddress = HttpContext.Current.Request.UserHostAddress;
      string testmode = configuration.GetSetting("testmode");
      string autocapture = configuration.GetSetting("autocapture");
      string secret = paymentSystem.Password;

      string tohash = string.Concat(protocol, msgtype, merchant, language, ordernumber, amount, currency, continueurl, cancelurl, callbackurl, autocapture, description, ipaddress, testmode, secret);
      string hash = this.GetMD5Hash(tohash);

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      transactionDataProvider.SaveStartValues(ordernumber, amount, currency, paymentSystem.Code);

      NameValueCollection data = new NameValueCollection
      {
        { "protocol", protocol },
        { "msgtype", msgtype },
        { "merchant", merchant },
        { "language", language },
        { "ordernumber", ordernumber },
        { "amount", amount },
        { "currency", currency },
        { "continueurl", continueurl },
        { "cancelurl", cancelurl },
        { "callbackurl", callbackurl },
        { "autocapture", autocapture },
        { "description", description },
        { "ipaddress", ipaddress },
        { "testmode", testmode },
        { "md5check", hash },
      };

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
      string autocapture = configuration.GetSetting("autocapture");

      if (request.QueryString[PaymentConstants.ActionKey] != PaymentConstants.CancelResponse)
      {
        string xmlReturn = this.CheckStatus(paymentSystem, paymentArgs);
        if (!string.IsNullOrEmpty(xmlReturn))
        {
          var quickPayResponse = this.ParseResult(xmlReturn);
          string secret = paymentSystem.Password;
          string tohash = string.Join(string.Empty, quickPayResponse.Where(x => x.Key != "md5check").Select(x => x.Value).ToArray()) + secret;

          string md5Check = this.GetMD5Hash(tohash);
          if (md5Check.Equals(quickPayResponse["md5check"]) && quickPayResponse["qpstat"].Equals("000"))
          {
            this.PaymentStatus = PaymentStatus.Succeeded;
            transactionDataProvider.SaveCallBackValues(paymentArgs.ShoppingCart.OrderNumber, this.PaymentStatus.ToString(), quickPayResponse["transaction"], quickPayResponse["amount"], quickPayResponse["currency"], string.Empty, string.Empty, string.Empty, quickPayResponse["cardtype"]);
            if (string.Compare(autocapture, this.ReservableBehavior, StringComparison.OrdinalIgnoreCase) == 0)
            {
              string transactionAmount = transactionDataProvider.GetPersistentValue(paymentArgs.ShoppingCart.OrderNumber, TransactionConstants.TotalAmount) as string;
              string orderNumber = paymentArgs.ShoppingCart.OrderNumber;
              string transactionNumber = transactionDataProvider.GetPersistentValue(paymentArgs.ShoppingCart.OrderNumber, TransactionConstants.TransactionNumber) as string;

              decimal amount = transactionAmount.FromCents();
              ReservationTicket reservationTicket = new ReservationTicket
              {
                InvoiceNumber = orderNumber,
                AuthorizationCode = PaymentConstants.EmptyAuthorizationCode,
                TransactionNumber = transactionNumber,
                Amount = amount
              };
              transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, PaymentConstants.ReservationTicket, reservationTicket);
            }
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

    #region IReservablePaymentProvider Members

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

      var configuration = new PaymentSettingsReader(paymentSystem);
      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();

      string protocolInput = configuration.GetSetting("protocol");
      const string MsgtypeInput = "capture";
      string merchantInput = paymentSystem.Username;
      string amountInput = amount.ToCents();
      string transaction = reservationTicket.TransactionNumber;
      string secretInput = paymentSystem.Password;
      string apiUrl = configuration.GetSetting("apiURL");

      string tohashInput = string.Concat(protocolInput, MsgtypeInput, merchantInput, amountInput, transaction, secretInput);
      string hashInput = this.GetMD5Hash(tohashInput);
      string requestString = string.Format(
        "protocol={0}&msgtype={1}&merchant={2}&amount={3}&transaction={4}&md5check={5}",
        protocolInput,
        MsgtypeInput,
        paymentSystem.Username,
        amountInput,
        transaction,
        hashInput);

      string message = "UnCaptured: ";
      string information = string.Empty;
      string xmlReturn = this.SendRequest(apiUrl, requestString, ref information);
      if (!string.IsNullOrEmpty(xmlReturn))
      {
        var quickPayResponse = this.ParseResult(xmlReturn);
        string secret = paymentSystem.Password;
        string tohash = string.Join(string.Empty, quickPayResponse.Where(x => x.Key != "md5check").Select(x => x.Value).ToArray()) + secret;
        string md5Check = this.GetMD5Hash(tohash);

        if (md5Check.Equals(quickPayResponse["md5check"]) && quickPayResponse["qpstat"].Equals("000"))
        {
          transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, PaymentConstants.CaptureSuccess);
          return;
        }

        message += string.Format("qpstat={0}", quickPayResponse["qpstat"]);
      }
      else
      {
        message += information;
      }

      transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, message);
    }

    /// <summary>
    /// Cancels payment reservation
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="paymentArgs">The payment args.</param>
    /// <param name="reservationTicket">The reservation ticket.</param>
    public void CancelReservation([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs, [NotNull] ReservationTicket reservationTicket)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");
      Assert.ArgumentNotNull(reservationTicket, "reservationTicket");

      var configuration = new PaymentSettingsReader(paymentSystem);
      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();

      string protocolInput = configuration.GetSetting("protocol");
      const string MsgtypeInput = "cancel";
      string merchantInput = paymentSystem.Username;
      string transaction = reservationTicket.TransactionNumber;
      string secretInput = paymentSystem.Password;
      string apiUrl = configuration.GetSetting("apiURL");

      string tohashInput = string.Concat(protocolInput, MsgtypeInput, merchantInput, transaction, secretInput);
      string hashInput = this.GetMD5Hash(tohashInput);

      string requestString = string.Format(
        "protocol={0}&msgtype={1}&merchant={2}&transaction={3}&md5check={4}",
        protocolInput,
        MsgtypeInput,
        paymentSystem.Username,
        transaction,
        hashInput);


      string message = "UnCanceled: ";
      string information = string.Empty;
      string xmlReturn = this.SendRequest(apiUrl, requestString, ref information);
      if (!string.IsNullOrEmpty(xmlReturn))
      {
        var quickPayResponse = this.ParseResult(xmlReturn);
        string secret = paymentSystem.Password;
        string tohash = string.Join(string.Empty, quickPayResponse.Where(x => x.Key != "md5check").Select(x => x.Value).ToArray()) + secret;
        string md5Check = this.GetMD5Hash(tohash);

        if (md5Check.Equals(quickPayResponse["md5check"]) && quickPayResponse["qpstat"].Equals("000"))
        {
          transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, PaymentConstants.CancelSuccess);
          return;
        }

        message += string.Format("qpstat={0}", quickPayResponse["qpstat"]);
      }
      else
      {
        message += information;
      }

      transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, message);
    }

    #endregion

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
    /// The specified language code.
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
    /// Gets the MD5 hash.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>The MD5 hash.</returns>
    protected virtual string GetMD5Hash(string input)
    {
      MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
      byte[] bs = Encoding.UTF8.GetBytes(input);
      bs = x.ComputeHash(bs);
      StringBuilder s = new StringBuilder();
      foreach (byte b in bs)
      {
        s.Append(b.ToString("x2").ToLower());
      }

      string outstr = s.ToString();
      return outstr;
    }

    /// <summary>
    /// Checks status of the transaction
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment Args.</param>
    /// <returns>
    /// Checks the status of the transaction.
    /// </returns>
    [NotNull]
    protected virtual string CheckStatus([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs)
    {
      Assert.ArgumentNotNull(paymentSystem, "paymentSystem");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");

      var configuration = new PaymentSettingsReader(paymentSystem);
      string protocolInput = configuration.GetSetting("protocol");
      const string MsgtypeInput = "status";
      string merchantInput = paymentSystem.Username;
      string ordernumberInput = paymentArgs.ShoppingCart.OrderNumber;
      string secretInput = paymentSystem.Password;
      string apiUrl = configuration.GetSetting("apiURL");

      string tohashInput = string.Concat(protocolInput, MsgtypeInput, merchantInput, ordernumberInput, secretInput);
      string hashInput = this.GetMD5Hash(tohashInput);
      string requestString = string.Format(
        "protocol={0}&msgtype={1}&merchant={2}&ordernumber={3}&md5check={4}",
        protocolInput,
        MsgtypeInput,
        paymentSystem.Username,
        ordernumberInput,
        hashInput);

      string information = string.Empty;
      return this.SendRequest(apiUrl, requestString, ref information);
    }

    /// <summary>
    /// Sends request to the QuickPay service.
    /// </summary>
    /// <param name="apiUrl">The api url.</param>
    /// <param name="requestString">The request string.</param>
    /// <param name="information">The information.</param>
    /// <returns>Service response</returns>
    [NotNull]
    protected string SendRequest(string apiUrl, string requestString, ref string information)
    {
      string xmlReturn = string.Empty;
      try
      {
        var request = WebRequest.Create(apiUrl);
        var httpContent = Encoding.UTF8.GetBytes(requestString);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = httpContent.Length;

        using (var stream = request.GetRequestStream())
        {
          stream.Write(httpContent, 0, httpContent.Length);
        }

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        if (response != null)
        {
          StreamReader reader = new StreamReader(response.GetResponseStream());
          xmlReturn = reader.ReadToEnd();
        }
      }
      catch (Exception exception)
      {
        Log.Error(information = exception.Message, this);
      }

      return xmlReturn;
    }

    /// <summary>
    ///  Parses QuickPay response
    /// </summary>
    /// <param name="xmlReturn">The xml return.</param>
    /// <returns>
    ///  Dictionary that is created from the response 
    /// </returns>
    protected IDictionary<string, string> ParseResult(string xmlReturn)
    {
      var xml = new XmlDocument();
      xml.LoadXml(xmlReturn);

      // Get root node 'response'
      var root = xml["response"];
      if (root != null)
      {
        return new List<string>
        {
          "msgtype",
          "ordernumber",
          "amount",
          "currency",
          "time",
          "state",
          "qpstat",
          "qpstatmsg",
          "chstat",
          "chstatmsg",
          "merchant",
          "merchantemail",
          "transaction",
          "cardtype",
          "cardnumber",
          "md5check"
        }

        .ToDictionary(x => x, x => root[x] == null ? null : root[x].InnerText);
      }

      return null;
    }
  }
}