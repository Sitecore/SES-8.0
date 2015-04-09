// -------------------------------------------------------------------------------------------
// <copyright file="Order.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.DomainModel.Orders
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Currencies;
  using Payments;
  using Prices;
  using Shippings;
  using Users;

  /// <summary>
  /// The order abstract class.
  /// </summary>
  [Obsolete("Use \"Sitecore.Ecommerce.OrderManagement.Orders.Order, Sitecore.Ecommerce.Core\" class instead.")]
  public class Order
  {
    /// <summary>
    /// Current order status.
    /// </summary>
    private OrderStatus currentOrderStatus;

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class.
    /// </summary>
    public Order()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="status">The status.</param>
    public Order(OrderStatus status)
    {
      this.currentOrderStatus = status;
    }

    /// <summary>
    /// Gets or sets the order status.
    /// </summary>
    /// <value>The order status.</value>
    /// <exception cref="Exception"><c>Exception</c>.</exception>
    public virtual OrderStatus Status
    {
      get
      {
        return this.currentOrderStatus;
      }

      set
      {
        if ((this.Status != null) && (this.Status.GetType() != typeof(OrderStatus)))
        {
          Type newStatusType = value.GetType();

          if (!this.Status.GetAvailableFollowingStatuses().Any(p => newStatusType.IsAssignableFrom(p.GetType())))
          {
            throw new Exception(string.Concat("Status: '", newStatusType.ToString(), "' is not in the list of available following statuses of: '", this.Status.GetType(), "'."));
          }
        }

        this.currentOrderStatus = value;
      }
    }

    /// <summary>
    /// Gets or sets the order date.
    /// </summary>
    /// <value>The order date.</value>
    public virtual DateTime OrderDate { get; set; }

    /// <summary>
    /// Gets or sets the tracking number.
    /// </summary>
    /// <value>The tracking number.</value>
    public virtual string TrackingNumber { get; set; }

    /// <summary>
    /// Gets or sets the transaction number.
    /// </summary>
    /// <value>The transaction number.</value>
    public virtual string TransactionNumber { get; set; }

    /// <summary>
    /// Gets or sets the discount code.
    /// </summary>
    /// <value>The discount code.</value>
    public virtual string DiscountCode { get; set; }

    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    /// <value>The comment.</value>
    public virtual string Comment { get; set; }

    /// <summary>
    /// Gets or sets the order number.
    /// </summary>
    /// <value>The order number.</value>
    public virtual string OrderNumber { get; set; }

    /// <summary>
    /// Gets or sets the product lines.
    /// </summary>
    /// <value>The product lines.</value>
    public virtual IList<OrderLine> OrderLines { get; set; }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    /// <value>The currency.</value>
    public virtual Currency Currency { get; set; }

    /// <summary>
    /// Gets or sets the customer info.
    /// </summary>
    /// <value>The customer info.</value>
    public virtual CustomerInfo CustomerInfo { get; set; }

    /// <summary>
    /// Gets or sets the notification option.
    /// </summary>
    /// <value>The notification option.</value>
    public virtual NotificationOption NotificationOption { get; set; }

    /// <summary>
    /// Gets or sets the notification option value.
    /// </summary>
    /// <value>The notification option value.</value>
    public virtual string NotificationOptionValue { get; set; }

    /// <summary>
    /// Gets or sets the payment method.
    /// </summary>
    /// <value>The payment method.</value>
    public virtual PaymentSystem PaymentSystem { get; set; }

    /// <summary>
    /// Gets or sets the shipping provider.
    /// </summary>
    /// <value>The shipping provider.</value>
    public virtual ShippingProvider ShippingProvider { get; set; }

    /// <summary>
    /// Gets or sets the shipping price.
    /// </summary>
    /// <value>The shipping price.</value>
    public virtual decimal ShippingPrice { get; set; }

    /// <summary>
    /// Gets or sets the totals.
    /// </summary>
    /// <value>The totals.</value>
    public virtual Totals Totals { get; set; }

    /// <summary>
    /// Gets or sets the authorization code.
    /// </summary>
    public virtual string AuthorizationCode { get; set; }

    /// <summary>
    /// Processes the status.
    /// </summary>
    public virtual void ProcessStatus()
    {
      MethodInfo processStatusMethodInfo = this.Status.GetType().GetMethod("Process", BindingFlags.NonPublic | BindingFlags.Instance);
      Type[] argumentTypes = { this.GetType() };
      MethodInfo genericProcessStatusMethodInfo = processStatusMethodInfo.MakeGenericMethod(argumentTypes);
      genericProcessStatusMethodInfo.Invoke(this.Status, new[] { this });
    }
  }
}