// -------------------------------------------------------------------------------------------
// <copyright file="OnlinePaymentProviderBase.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Payments
{
  using System;
  using System.Collections.Specialized;
  using System.Text;
  using System.Web;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Payments;

  /// <summary>
  /// Abstract class that must be inhterited from when implementing a new Payment Provider
  /// </summary>
  /// <typeparam name="T">The payment method type.</typeparam>
  [Obsolete("Use OnlinePaymentProvider instead.")]
  public abstract class OnlinePaymentProviderBase<T> : PaymentProviderBase<T>, IOnlinePaymentProvider<T> where T : DomainModel.Payments.PaymentSystem
  {
    #region Fields

    /// <summary>
    /// The payment return page url.
    /// </summary>
    private string paymentReturnPageUrl = "Payment Return Page Url";

    /// <summary>
    /// The payment cancel url.
    /// </summary>
    private string paymentCancelUrl = "Payment Cancel Url";

    /// <summary>
    /// The shopping cart.
    /// </summary>
    private ShoppingCart shoppingCart;

    #endregion

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <value>The description.</value>
    [Obsolete]
    public virtual string Description
    {
      get
      {
        if (this.ShoppingCart != null)
        {
          StringBuilder description = new StringBuilder();

          foreach (ShoppingCartLine shoppingCartLine in this.ShoppingCart.ShoppingCartLines)
          {
            description.Append(shoppingCartLine.Product.Title);
            description.Append(", ");
          }

          return description.ToString().Trim().TrimEnd(',');
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets or sets the payment return page URL.
    /// </summary>
    /// <value>The payment return page URL.</value>
    [Obsolete]
    public virtual string ReturnPageUrl
    {
      get
      {
        return this.ExtractPaymentLinkUrl(this.paymentReturnPageUrl);
      }

      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          this.paymentReturnPageUrl = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets the payment cancel page URL.
    /// </summary>
    /// <value>The payment cancel page URL.</value>
    [Obsolete]
    public virtual string CancelPageUrl
    {
      get
      {
        return this.ExtractPaymentLinkUrl(this.paymentCancelUrl);
      }

      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          this.paymentCancelUrl = value;
        }
      }
    }

    /// <summary>
    /// Gets the product numbers.
    /// </summary>
    /// <value>The product numbers.</value>
    [Obsolete]
    protected virtual string ProductNumbers
    {
      get
      {
        if (this.ShoppingCart != null)
        {
          StringBuilder productNumbers = new StringBuilder();

          foreach (ShoppingCartLine shoppingCartLine in this.ShoppingCart.ShoppingCartLines)
          {
            productNumbers.Append(shoppingCartLine.Product.Code);
            productNumbers.Append(", ");
          }

          return productNumbers.ToString().Trim().TrimEnd(',');
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the shopping cart.
    /// </summary>
    /// <value>The shopping cart.</value>
    [Obsolete("Use the ShoppingCart property of the PaymentArgs instance")]
    protected virtual ShoppingCart ShoppingCart
    {
      get
      {
        if (this.shoppingCart == null)
        {
          this.shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
        }

        return this.shoppingCart;
      }
    }

    /// <summary>
    /// Initializes the payment.
    /// </summary>
    [Obsolete]
    public override void InvokePayment()
    {
      PaymentUrlResolver paymentUrlResolver = new PaymentUrlResolver();
      PaymentArgs paymentArgs = new PaymentArgs
      {
        PaymentUrls = paymentUrlResolver.Resolve(),
        ShoppingCart = this.ShoppingCart,
        Description = this.Description
      };

      this.Invoke(this.PaymentSystem, paymentArgs);
    }

    /// <summary>
    /// Processes the callback in terms of the HttpRequest and extracts either hidden form fields, or querystring parameters
    /// that is returned from the payment provider.
    /// Determines the payment status and saves that indication for later, could be in session, in db, or other storage.
    /// This information is important and used in the GetPaymentStatus().
    /// </summary>
    /// <param name="request">The HttpRequest that holds all posted variables</param>
    [Obsolete("Use ProcessPaymentCallback(PaymentSystem, PaymentArgs, HttpRequest) instead")]
    public virtual void ProcessPaymentCallBack([NotNull] HttpRequest request)
    {
      Assert.ArgumentNotNull(request, "request");

      Assert.IsNotNull(this.ShoppingCart, "Shopping cart is null");

      PaymentUrlResolver paymentUrlResolver = new PaymentUrlResolver();
      PaymentArgs paymentArgs = new PaymentArgs
    {
        PaymentUrls = paymentUrlResolver.Resolve(),
        ShoppingCart = this.ShoppingCart,
        Description = this.Description
      };

      this.ProcessCallback(this.PaymentSystem, paymentArgs);
    }

    /// <summary>
    /// Cancels the order / deletes saved data
    /// </summary>
    [Obsolete]
    public virtual void CancelPayment()
    {
    }

    /// <summary>
    /// Finalizes the order. This function needs to be used after the payment
    /// was done for the order.
    /// </summary>
    /// <returns>
    /// The transaction in which the order creation resulted
    /// </returns>
    [Obsolete]
    public virtual string FinalizePayment()
    {
      Assert.IsNotNull(this.ShoppingCart, "Shopping cart is null");

      if (string.IsNullOrEmpty(this.ShoppingCart.OrderNumber))
      {
        ITransactionData transactionData = Context.Entity.Resolve<ITransactionData>();
        string transactionNumber = transactionData.GetPersistentValue(this.ShoppingCart.OrderNumber, TransactionConstants.TransactionNumber) as string;

        if (!string.IsNullOrEmpty(transactionNumber))
        {
          return transactionNumber;
        }
      }

      return string.Empty;
    }
      }
}