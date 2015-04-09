// -------------------------------------------------------------------------------------------
// <copyright file="PayExPaymentProvider.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Payments.PayEx
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Security.Cryptography;
  using System.Text;
  using System.Web;
  using System.Xml;
  using Diagnostics;
  using DomainModel.Payments;
  using Prices;
  using Services;
  using Utils;

  /// <summary>
  /// The PayEx payment provider
  /// </summary>
  public class PayExPaymentProvider : OnlinePaymentProvider, IReservable
  {
    /// <summary>
    /// Specifies the authorization transaction.
    /// </summary>
    protected readonly string ReservableTransactionType = "authorization";

    /// <summary>
    /// Specifies the provider exception - canceling of the order.
    /// </summary>
    protected const string OperationCanceledError = "OperationCancelledbyCustomer";

    /// <summary>
    /// Begins the payment.
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment Args.</param>
    public override void Invoke([NotNull] PaymentSystem paymentSystem, [NotNull] PaymentArgs paymentArgs)
    {
      Assert.IsNotNull(paymentSystem, "Payment system is null");
      Assert.ArgumentNotNull(paymentArgs, "paymentArgs");
      Assert.IsNotNull(paymentArgs.ShoppingCart, "Shopping cart is null");

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      PaymentSettingsReader configuration = new PaymentSettingsReader(paymentSystem);

      long accountNumber = TypeUtil.TryParse<long>(paymentSystem.Username, 0);
      string purchaseOperation = configuration.GetSetting("purchaseOperation");
      int price = int.Parse(paymentArgs.ShoppingCart.Totals.TotalPriceIncVat.ToCents());
      int shippingPrice = int.Parse(paymentArgs.ShoppingCart.ShippingPrice.ToCents());
      string priceArgList = string.Empty;

      string currency = this.Currency(paymentArgs.ShoppingCart.Currency.Code);
      const int Vat = 0;
      string orderID = paymentArgs.ShoppingCart.OrderNumber;
      string productNumber = paymentArgs.ShoppingCart.ShoppingCartLines.Aggregate(string.Empty, (accumulator, next) => string.Format("{0}, {1}", accumulator, next.Product.Code)).Trim().TrimStart(',');
      string description = paymentArgs.Description;
      string clientIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
      string clientIdentifier = configuration.GetSetting("clientIdentifier");
      string additionalValues = configuration.GetSetting("additionalValues");
      string externalID = configuration.GetSetting("externalID");
      string returnUrl = paymentArgs.PaymentUrls.ReturnPageUrl;
      string cancelUrl = returnUrl + PaymentConstants.CancelQueryString;
      string view = configuration.GetSetting("view");
      string agreementRef = configuration.GetSetting("agreementRef");
      string clientLanguage = this.Language(Sitecore.Context.Language.Name);
      string encryptionKey = paymentSystem.Password;

      ArrayList param = new ArrayList
      {
        accountNumber.ToString(), 
        purchaseOperation, 
        price, 
        priceArgList, 
        currency, 
        Vat, 
        orderID, 
        productNumber, 
        description, 
        clientIpAddress, 
        clientIdentifier, 
        additionalValues, 
        externalID, 
        returnUrl, 
        view, 
        agreementRef, 
        cancelUrl, 
        clientLanguage, 
        encryptionKey
      };
      string hash = this.CreateMD5Hash(param);

      PxOrder order = new PxOrder();
      string xmlReturn = order.Initialize7(accountNumber, purchaseOperation, price, priceArgList, currency, Vat, orderID, productNumber, description, clientIpAddress, clientIdentifier, additionalValues, externalID, returnUrl, view, agreementRef, cancelUrl, clientLanguage, hash);

      string redirectUrl = this.ParseRes(xmlReturn, "/payex/redirectUrl");
      string orderRef = this.ParseRes(xmlReturn, "/payex/orderRef");
      int itemNumber = 1;
      string mainErrorDescription = this.ParseRes(xmlReturn, "/payex/status/description");
      List<string> errorCodes = new List<string>
      {
        this.ParseRes(xmlReturn, "/payex/status/errorCode"), 
        mainErrorDescription, 
        this.PrepareOrderLine(
          order, 
          accountNumber, 
          orderRef, 
          itemNumber++.ToString(), 
          description, 
          paymentArgs.ShoppingCart.ShoppingCartLines.Count, 
          price - shippingPrice, 
          int.Parse(paymentArgs.ShoppingCart.Totals.TotalVat.ToCents()), 
          int.Parse(paymentArgs.ShoppingCart.Totals.VAT.ToCents()) * 100, 
          encryptionKey), 
        this.PrepareOrderLine(
          order, 
          accountNumber, 
          orderRef, 
          itemNumber++.ToString(), 
          "Shipping price", 
          1, 
          shippingPrice, 
          0, 
          0, 
          encryptionKey)
      };

      if (errorCodes.TrueForAll(x => x == "OK"))
      {
        transactionDataProvider.SaveStartValues(orderID, price.ToString(), currency, paymentSystem.Code);
      }
      else
      {
        transactionDataProvider.SavePersistentValue(orderID, TransactionConstants.PaymentStatus, PaymentStatus.Failure.ToString());
        redirectUrl = paymentArgs.PaymentUrls.FailurePageUrl;
        HttpContext.Current.Session["paymentErrorMessage"] = mainErrorDescription;
      }

      HttpContext.Current.Response.Redirect(redirectUrl, false);
      HttpContext.Current.ApplicationInstance.CompleteRequest();
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

      if (request.QueryString[PaymentConstants.ActionKey] != PaymentConstants.CancelResponse)
      {
        string orderRef = request["orderRef"];
        long accountNumber = TypeUtil.TryParse<long>(paymentSystem.Username, 0);
        string secretKey = paymentSystem.Password;

        if (!string.IsNullOrEmpty(orderRef) && accountNumber > 0)
        {
          ArrayList param = new ArrayList
          {
            accountNumber,
            orderRef,
            secretKey
          };
          string hash = this.CreateMD5Hash(param);
          try
          {
            this.CompleteCallback(paymentSystem, orderRef, accountNumber, hash, transactionDataProvider);
          }
          catch (Exception exception)
          {
            Log.Warn(exception.Message, exception, this);
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

      long accountNumber = TypeUtil.TryParse<long>(paymentSystem.Username, 0);
      int transactionNumber = int.Parse(reservationTicket.TransactionNumber);
      int amountInput = int.Parse(amount.ToCents());

      string encryptionKey = paymentSystem.Password;

      ArrayList param = new ArrayList
      {
        accountNumber,
        transactionNumber,
        amountInput,
        encryptionKey
      };
      string hash = this.CreateMD5Hash(param);

      PxOrder order = new PxOrder();
      string xmlReturn = order.Capture2(accountNumber, transactionNumber, amountInput, hash);
      string errorCode = this.ParseRes(xmlReturn, "/payex/status/errorCode");
      string description = this.ParseRes(xmlReturn, "/payex/status/description");

      if (errorCode == "OK" && description == "OK")
      {
        transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, PaymentConstants.CaptureSuccess);
      }
      else
      {
        transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, string.Format("errorCode={0} description={1}", errorCode, description));
      }
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

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();

      long accountNumber = TypeUtil.TryParse<long>(paymentSystem.Username, 0);
      int transactionNumber = int.Parse(reservationTicket.TransactionNumber);
      string encryptionKey = paymentSystem.Password;

      ArrayList param = new ArrayList
      {
        accountNumber,
        transactionNumber,
        encryptionKey
      };
      string hash = this.CreateMD5Hash(param);

      PxOrder order = new PxOrder();
      string xmlReturn = order.Cancel2(accountNumber, transactionNumber, hash);
      string errorCode = this.ParseRes(xmlReturn, "/payex/status/errorCode");
      string description = this.ParseRes(xmlReturn, "/payex/status/description");

      if (errorCode == "OK" && description == "OK")
      {
        transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, PaymentConstants.CancelSuccess);
      }
      else
      {
        transactionDataProvider.SavePersistentValue(reservationTicket.InvoiceNumber, string.Format("errorCode={0} description={1}", errorCode, description));
      }
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
    /// The specified language code.
    /// </returns>
    [CanBeNull]
    protected virtual string Language([NotNull] string languageCode)
    {
      Assert.ArgumentNotNull(languageCode, "languageCode");

      switch (languageCode.ToLower())
      {
        // Norwegian
        case "no":
          return "nb-NO";

        // English
        case "en":
          return "en-US";

        // Swedish
        case "sv":
          return "sv-SE";

        // Danish
        case "dk":
          return "da-DK";

        // Finnish
        case "fi":
          return "fi-FI";

        // French
        case "fr":
          return "fr-FR";

        // Deutsch
        case "de":
          return "de-DE";

        // Polish
        case "pl":
          return "pl-PL";

        // Czech
        case "cz":
          return "cs-CZ";

        // Hungarian
        case "hu":
          return "hu-HU";

        // Spanish
        case "es":
          return "es-ES";
        default:
          return null;
      }
    }

    /// <summary>
    /// Parses the result.
    /// </summary>
    /// <param name="xmlText">The XML text.</param>
    /// <param name="node">The context node.</param>
    /// <returns>
    /// The parsed result.
    /// </returns>
    [NotNull]
    protected virtual string ParseRes([NotNull] string xmlText, [NotNull] string node)
    {
      Assert.ArgumentNotNull(xmlText, "xmlText");
      Assert.ArgumentNotNull(node, "node");

      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlText);
        XmlNode xmlNode = xmlDocument.SelectSingleNode(node);
        if (xmlNode != null)
        {
          return xmlNode.InnerText;
        }
        return string.Empty;
      }
      catch
      {
        return string.Empty;
      }
    }

    /// <summary>
    /// Creates the Md5 hash.
    /// </summary>
    /// <param name="param">The param.</param>
    /// <returns>
    /// The Md5 hash.
    /// </returns>
    [NotNull]
    protected virtual string CreateMD5Hash([NotNull] ArrayList param)
    {
      Assert.ArgumentNotNull(param, "param");

      string parameters = string.Empty;

      IEnumerator enumerator = param.GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (enumerator.Current != null)
        {
          parameters += enumerator.Current.ToString();
        }
      }

      return this.CreateHash(parameters);
    }

    /// <summary>
    /// Creates the hash.
    /// </summary>
    /// <param name="stringToHash">The string to hash.</param>
    /// <returns>
    /// The ctreated  hash.
    /// </returns>
    [NotNull]
    protected virtual string CreateHash([NotNull] string stringToHash)
    {
      Assert.ArgumentNotNull(stringToHash, "stringToHash");

      MD5 hash = new MD5CryptoServiceProvider();

      byte[] data = hash.ComputeHash(Encoding.Default.GetBytes(stringToHash));

      StringBuilder dataString = new StringBuilder();

      for (int i = 0; i < data.Length; i++)
      {
        dataString.Append(data[i].ToString("x2"));
      }

      return dataString.ToString();
    }

    /// <summary>
    /// Prepare order line
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="accountNumber">The account number.</param>
    /// <param name="orderRef">The order ref.</param>
    /// <param name="itemNumber">The item number.</param>
    /// <param name="itemDescription1">The item description 1.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="orderLinePrice">The order line price.</param>
    /// <param name="orderLineVatPrice">The order line vat price.</param>
    /// <param name="orderLineVatPercent">The order line vat percent.</param>
    /// <param name="encryptionKey">The encryption key.</param>
    /// <returns>
    /// Error code.
    /// </returns>
    [NotNull]
    protected virtual string PrepareOrderLine([NotNull] PxOrder order, long accountNumber, [NotNull] string orderRef, [NotNull] string itemNumber, [NotNull] string itemDescription1, int quantity, int orderLinePrice, int orderLineVatPrice, int orderLineVatPercent, [NotNull] string encryptionKey)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(orderRef, "orderRef");
      Assert.ArgumentNotNull(itemNumber, "itemNumber");
      Assert.ArgumentNotNull(itemDescription1, "itemDescription1");
      Assert.ArgumentNotNull(encryptionKey, "encryptionKey");

      ArrayList orderLineParams = new ArrayList
      {
        accountNumber,
        orderRef,
        itemNumber,
        itemDescription1,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        quantity,
        orderLinePrice,
        orderLineVatPrice,
        orderLineVatPercent,
        encryptionKey
      };
      string orderLineHash = this.CreateMD5Hash(orderLineParams);
      string xmlReturnOrderLine = order.AddSingleOrderLine(
        accountNumber,
        orderRef,
        itemNumber,
        itemDescription1,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        quantity,
        orderLinePrice,
        orderLineVatPrice,
        orderLineVatPercent,
        orderLineHash);

      return this.ParseRes(xmlReturnOrderLine, "/payex/status/errorCode");
    }

    /// <summary>
    /// Performs the completion of the conversation with PayPal.
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="orderRef">The order ref.</param>
    /// <param name="accountNumber">The account number.</param>
    /// <param name="hash">The hash.</param>
    /// <param name="transactionDataProvider">The transaction data provider.</param>
    protected void CompleteCallback(PaymentSystem paymentSystem, string orderRef, long accountNumber, string hash, ITransactionData transactionDataProvider)
    {
      PaymentSettingsReader configuration = new PaymentSettingsReader(paymentSystem);
      string purchaseOperation = configuration.GetSetting("purchaseOperation");
      PaymentStatus paymentStatusResult = PaymentStatus.Failure;

      PxOrder payexOrder = new PxOrder();
      string xmlReturn = payexOrder.Complete(accountNumber, orderRef, hash);

      string transactionNumber = this.ParseRes(xmlReturn, "/payex/transactionNumber");
      string orderNumber = this.ParseRes(xmlReturn, "/payex/orderId");
      string transactionAmount = this.ParseRes(xmlReturn, "/payex/amount");
      string errorCode = this.ParseRes(xmlReturn, "/payex/status/errorCode");
      int transactionStatus = int.Parse(this.ParseRes(xmlReturn, "/payex/transactionStatus"));

      if (errorCode == "OK")
      {
        switch (transactionStatus)
        {
          case (int)TransactionStatus.Sale:
          case (int)TransactionStatus.Authorize:
          {
            paymentStatusResult = PaymentStatus.Succeeded;
            transactionDataProvider.SaveCallBackValues(orderNumber, paymentStatusResult.ToString(), transactionNumber, transactionAmount, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            if (string.Compare(purchaseOperation, this.ReservableTransactionType, StringComparison.OrdinalIgnoreCase) == 0)
            {
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

            break;
          }

          default:
          {
            string transactionErrorCode = this.ParseRes(xmlReturn, "/payex/errorDetails/transactionErrorCode");
            if (transactionErrorCode == OperationCanceledError)
            {
              paymentStatusResult = PaymentStatus.Canceled;
            }

            break;
          }
        }
      }

      this.PaymentStatus = paymentStatusResult;
    }
  }
}