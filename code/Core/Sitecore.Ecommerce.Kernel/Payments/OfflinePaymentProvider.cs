// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfflinePaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The offline payment provider.
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
  using System.Web;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Orders;
  using DomainModel.Payments;
  using Sitecore.Ecommerce.Prices;
  using Sitecore.Pipelines;

  /// <summary>
  /// The offline payment provider.
  /// </summary>
  public class OfflinePaymentProvider : PaymentProviderBase<DomainModel.Payments.PaymentSystem>, IOfflinePaymentProvider<DomainModel.Payments.PaymentSystem>
  {
    /// <summary>
    /// Invokes the payment.
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="paymentArgs">The payment args.</param>
    /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
    public override void Invoke(DomainModel.Payments.PaymentSystem paymentSystem, PaymentArgs paymentArgs)
    {
      string ordernumber = paymentArgs.ShoppingCart.OrderNumber;
      string amount = paymentArgs.ShoppingCart.Totals.TotalPriceIncVat.ToCents();
      string currency = paymentArgs.ShoppingCart.Currency.Code;

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      transactionDataProvider.SaveStartValues(ordernumber, amount, currency, paymentSystem.Code);

      HttpContext.Current.Response.Redirect(paymentArgs.PaymentUrls.ReturnPageUrl);
      }

    /// <summary>
    /// Processes the callback in terms of the HttpRequest and extracts either hidden form fields, or querystring parameters
    /// that is returned from the payment provider.
    /// Determines the payment status and saves that indication for later, could be in session, in db, or other storage.
    /// This information is important and used in the PaymentStatus.
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="paymentArgs">The payment arguments.</param>
    /// <param name="request">The request.</param>
    /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
    public override void ProcessCallback(DomainModel.Payments.PaymentSystem paymentSystem, PaymentArgs paymentArgs)
    {
      this.PaymentStatus = PaymentStatus.Succeeded;
    }

    /// <summary>
    /// Processes the payment.
    /// </summary>
    /// <typeparam name="T">The shopping cart type.</typeparam>
    /// <param name="shoppingCart">The shopping cart.</param>
    [Obsolete]
    public virtual void ProcessPayment<T>(T shoppingCart) where T : ShoppingCart
    {
      IOrderManager<Order> orderProvider = Context.Entity.Resolve<IOrderManager<Order>>();
      Order order = orderProvider.CreateOrder(shoppingCart);

      if (order == null)
      {
        Log.Error("Order could not be created.", this);
        this.PaymentStatus = PaymentStatus.Failure;
        return;
      }

      this.PaymentStatus = PaymentStatus.Succeeded;

      this.StartOrderCreatedPipeline(order);
    }

    /// <summary>
    /// Starts the order create pipeline.
    /// </summary>
    /// <param name="order">The order.</param>
    [Obsolete]
    protected virtual void StartOrderCreatedPipeline(Order order)
    {
      PipelineArgs pipelineArgs = new PipelineArgs();
      pipelineArgs.CustomData.Add("order", order);

      CorePipeline.Run("orderCreated", pipelineArgs);
    }
  }
}