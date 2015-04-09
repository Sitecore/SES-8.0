// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckOutPaymentReturnPage.ascx.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The checkout payment return page.
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess
{
  using System;
  using System.Web;
  using System.Web.UI;
  using CheckOuts;
  using Diagnostics;
  using DomainModel.CheckOuts;
  using DomainModel.Orders;
  using DomainModel.Payments;
  using Payments;
  using Sitecore.Form.Core.Client.Submit;
  using Sitecore.Pipelines;
  using Text;
  using Utils;

  /// <summary>
  /// The checkout payment return page.
  /// </summary>
  public partial class CheckOutPaymentReturnPage : UserControl
  {
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <exception cref="ValidatorException">Order could not be created.</exception>
    protected void Page_Load(object sender, EventArgs e)
    {
      DomainModel.Carts.ShoppingCart shoppingCart = Sitecore.Ecommerce.Context.Entity.GetInstance<DomainModel.Carts.ShoppingCart>();
      if (string.IsNullOrEmpty(shoppingCart.PaymentSystem.Code))
      {
        return;
      }

      PaymentUrlResolver paymentUrlResolver = new PaymentUrlResolver();
      PaymentArgs paymentArgs = new PaymentArgs
      {
        PaymentUrls = paymentUrlResolver.Resolve(),
        ShoppingCart = shoppingCart
      };

      ITransactionData transactionData = Sitecore.Ecommerce.Context.Entity.Resolve<ITransactionData>();

      PaymentProvider paymentProvider = Sitecore.Ecommerce.Context.Entity.Resolve<PaymentProvider>(shoppingCart.PaymentSystem.Code);
      DomainModel.Payments.PaymentSystem paymentSystem = shoppingCart.PaymentSystem;

      try
      {
        paymentProvider.ProcessCallback(paymentSystem, paymentArgs);
      }
      catch (Exception exception)
      {
        IOrderManager<Order> orderManager = Sitecore.Ecommerce.Context.Entity.Resolve<IOrderManager<Order>>();
        shoppingCart.OrderNumber = orderManager.GenerateOrderNumber();
        transactionData.DeletePersistentValue(shoppingCart.OrderNumber);

        HttpContext.Current.Session["paymentErrorMessage"] = exception.Message;
        this.Response.Redirect(paymentArgs.PaymentUrls.FailurePageUrl);

        return;
      }

      switch (paymentProvider.PaymentStatus)
      {
        case PaymentStatus.Succeeded:
          {
            IOrderManager<Order> orderManager = Sitecore.Ecommerce.Context.Entity.Resolve<IOrderManager<Order>>();
            Order order = orderManager.CreateOrder(shoppingCart);

            if (order != null)
            {
              // Redirect to order confirmation page
              string orderId = shoppingCart.OrderNumber;
              ICheckOut checkOut = Sitecore.Ecommerce.Context.Entity.Resolve<ICheckOut>();
              if (checkOut is CheckOut)
              {
                ((CheckOut)checkOut).ResetCheckOut();
              }

              if (MainUtil.IsLoggedIn())
              {
                orderId = string.Format("orderid={0}", orderId);
              }
              else
              {
                string encryptKey = Crypto.EncryptTripleDES(orderId, "5dfkjek5");
                orderId = string.Format("key={0}", Uri.EscapeDataString(encryptKey));
              }

              this.StartOrderCreatedPipeline(order.OrderNumber);

              if (!string.IsNullOrEmpty(orderId))
              {
                UrlString url = new UrlString(paymentArgs.PaymentUrls.SuccessPageUrl);
                url.Append(new UrlString(orderId));
                this.Response.Redirect(url.ToString());
              }
            }
            else
            {
              IOrderManager<Order> orderProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IOrderManager<Order>>();
              shoppingCart.OrderNumber = orderProvider.GenerateOrderNumber();
              transactionData.DeletePersistentValue(shoppingCart.OrderNumber);

              HttpContext.Current.Session["paymentErrorMessage"] = "Order could not be created.";
              this.Response.Redirect(paymentArgs.PaymentUrls.FailurePageUrl);
            }

            break;
          }

        case PaymentStatus.Canceled:
          {
            IOrderManager<Order> orderProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IOrderManager<Order>>();
            shoppingCart.OrderNumber = orderProvider.GenerateOrderNumber();
            transactionData.DeletePersistentValue(shoppingCart.OrderNumber);

            HttpContext.Current.Session["paymentErrorMessage"] = "Payment has been aborted by user.";
            this.Response.Redirect(paymentArgs.PaymentUrls.CancelPageUrl);

            break;
          }

        case PaymentStatus.Failure:
          {
            IOrderManager<Order> orderProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IOrderManager<Order>>();
            shoppingCart.OrderNumber = orderProvider.GenerateOrderNumber();
            transactionData.DeletePersistentValue(shoppingCart.OrderNumber);

            this.Response.Redirect(paymentArgs.PaymentUrls.FailurePageUrl);

            break;
          }
      }
    }

    /// <summary>
    /// Starts the order create pipeline.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    protected virtual void StartOrderCreatedPipeline([NotNull] string orderNumber)
    {
      Assert.ArgumentNotNull(orderNumber, "orderNumber");

      PipelineArgs pipelineArgs = new PipelineArgs();
      pipelineArgs.CustomData.Add("orderNumber", orderNumber);

      CorePipeline.Run("orderCreated", pipelineArgs);
    }
  }
}