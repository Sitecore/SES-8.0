// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnlinePaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the online payment provider class.
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

namespace Sitecore.Ecommerce.Payments
{
  using System;
  using System.Collections.Specialized;
  using System.Web;
  using Diagnostics;
  using DomainModel.Payments;

  /// <summary>
  /// Defines the online payment provider class.
  /// </summary>
  public abstract class OnlinePaymentProvider : PaymentProvider
  {
    /// <summary>
    /// Initializes the payment.</summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment arguments.</param>
    public override void Invoke(DomainModel.Payments.PaymentSystem paymentSystem, PaymentArgs paymentArgs)
    {
      Assert.IsNotNull(HttpContext.Current, "Http context is null");

      Assert.IsNotNull(paymentSystem, "Payment system is null");
      Assert.IsNotNull(paymentArgs.ShoppingCart, "Shopping cart is null");
      Assert.IsNotNull(paymentArgs.ShoppingCart.Totals, "Shopping cart totals were not set");
      Assert.IsNotNull(paymentArgs.ShoppingCart.Currency, "Shopping cart currency was not set");
      Assert.IsNotNull(paymentArgs.ShoppingCart.CustomerInfo, "Customer information was not set");
      Assert.IsNotNull(paymentArgs.ShoppingCart.CustomerInfo.ShippingAddress, "Customer shipping address was not set");
      Assert.IsNotNull(paymentArgs.ShoppingCart.CustomerInfo.BillingAddress, "Customer billing address was not set");
      Assert.IsNotNull(paymentArgs.Description, "Description is empty");

      Assert.IsNotNullOrEmpty(paymentSystem.Code, "Payment system code was not set");
      Assert.IsNotNullOrEmpty(paymentSystem.Username, "Payment system user name was not set");
      Assert.IsNotNullOrEmpty(paymentSystem.Password, "Payment system password was not set");
      Assert.IsNotNullOrEmpty(paymentArgs.ShoppingCart.OrderNumber, "Order number was not set");
      Assert.IsNotNullOrEmpty(paymentArgs.PaymentUrls.ReturnPageUrl, "Return page was not set");
      Assert.IsNotNullOrEmpty(paymentSystem.PaymentUrl, "Payment url was not set");

      Assert.IsTrue(paymentArgs.ShoppingCart.Totals.TotalPriceIncVat > 0, "Price is empty");
    }

    /// <summary>
    /// Posts the data.
    /// </summary>
    /// <param name="url">The target URL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <exception cref="Exception"><c>Exception</c>.</exception>
    protected virtual void PostData(string url, NameValueCollection parameters)
    {
      Assert.IsNotNullOrEmpty(url, "Payment URL is null or empty");
      Assert.IsNotNull(parameters, "Input parameters are null");

      HttpContext.Current.Response.Clear();

      HttpContext.Current.Response.Write("<html><head>");
      HttpContext.Current.Response.Write("</head><body onload=\"document.formName.submit()\">");
      HttpContext.Current.Response.Write(string.Format("<form name=\"formName\" method=\"post\" action=\"{0}\" >", url));
      foreach (string key in parameters)
      {
        HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", key, parameters[key]));
      }

      HttpContext.Current.Response.Write("</form>");
      HttpContext.Current.Response.Write("</body></html>");

      try
      {
        HttpContext.Current.ApplicationInstance.CompleteRequest();
      }
      catch (Exception exception)
      {
        Log.Error(exception.Message, exception, this);
        throw;
      }
    }

    /// <summary>
    /// Converts the currency.
    ///  </summary>
    /// <param name="currencyCode">The currency code.</param>
    /// <returns>New currency code.</returns>
    protected virtual string Currency(string currencyCode)
    {
      return currencyCode;
    }

    /// <summary>
    /// Constants to use within the whole set of the payment providers.
    /// </summary>
    protected static class PaymentConstants
    {
      /// <summary>
      /// Specifies the value that is saved in the session to present successfull capture transaction.
      /// </summary>
      public static readonly string CaptureSuccess = "Captured";

      /// <summary>
      /// Specifies the value that is saved in the session to present successfull capture transaction.
      /// </summary>
      public static readonly string CancelSuccess = "Canceled";

      /// <summary>
      /// Specifies the value that is saved in the session to present successfull capture transaction.
      /// </summary>
      public static readonly string ReservationTicket = "ReservationTicket";

      /// <summary>
      /// Value to fill the authorization code when it is empty.
      /// </summary>
      public static readonly string EmptyAuthorizationCode = "None";

      /// <summary>
      /// Specifies the canceling of the operation.
      /// </summary>
      public static readonly string CancelResponse = "cancel";

      /// <summary>
      /// Denetermines the special custom query string key.
      /// </summary>
      public static readonly string ActionKey = "scemaction";

      /// <summary>
      /// Query string to redirect to the cancel page.
      /// </summary>
      public static readonly string CancelQueryString = string.Format("?{0}={1}", ActionKey, CancelResponse);
    }
  }
}
