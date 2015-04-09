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

namespace Sitecore.Ecommerce.Orders
{
  using System;
  using System.Collections.Generic;
  using Data;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using DomainModel.Orders;
  using DomainModel.Payments;
  using DomainModel.Prices;
  using DomainModel.Shippings;
  using DomainModel.Users;
  using Validators.Interception;

  /// <summary>
  /// The order implementation.
  /// </summary>
  [Entity(TemplateId = "{2769D69F-E217-4C0A-A41F-2083EC165218}")]
  [Obsolete("Use \"Sitecore.Ecommerce.OrderManagement.Orders.Order, Sitecore.Ecommerce.Core\" class instead.")]
  public class Order : DomainModel.Orders.Order, IEntity
  {
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
      : base(status)
    {
    }

    /// <summary>
    /// Gets or sets the order status.
    /// </summary>
    /// <value>The order status.</value>
    /// <exception cref="Exception"><c>Exception</c>.</exception>
    [Entity(FieldName = "Status")]
    public override OrderStatus Status { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the order date.
    /// </summary>
    /// <value>The order date.</value>
    [Entity(FieldName = "OrderDate")]
    public override DateTime OrderDate { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the tracking number.
    /// </summary>
    /// <value>The tracking number.</value>
    [Entity(FieldName = "TrackingNumber")]
    public override string TrackingNumber { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the transaction number.
    /// </summary>
    /// <value>The transaction number.</value>
    [Entity(FieldName = "TransactionNumber")]
    public override string TransactionNumber { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the discount code.
    /// </summary>
    /// <value>The discount code.</value>
    [Entity(FieldName = "DiscountCode")]
    public override string DiscountCode { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    /// <value>The comment.</value>
    [Entity(FieldName = "Comment")]
    public override string Comment { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the order number.
    /// </summary>
    /// <value>The order number.</value>
    [Entity(FieldName = "OrderNumber")]
    public override string OrderNumber { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the product lines.
    /// </summary>
    /// <value>The product lines.</value>
    [Entity(FieldName = "ProductLines")]
    public override IList<DomainModel.Orders.OrderLine> OrderLines { get; set; }

    /// <summary>
    /// Gets or sets the customer info.
    /// </summary>
    /// <value>The customer info.</value>
    [Entity(FieldName = "CustomerInfo")]
    public override CustomerInfo CustomerInfo { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the totals.
    /// </summary>
    /// <value>The totals.</value>
    [Entity(FieldName = "Totals")]
    public override Totals Totals { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the payment method.
    /// </summary>
    /// <value>The payment method.</value>
    [Entity(FieldName = "PaymentSystem")]
    public override PaymentSystem PaymentSystem { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the shipping provider.
    /// </summary>
    /// <value>The shipping provider.</value>
    [Entity(FieldName = "ShippingProvider")]
    public override ShippingProvider ShippingProvider { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the shipping price.
    /// </summary>
    /// <value>The shipping price.</value>
    [Entity(FieldName = "Shipping Price")]
    public override decimal ShippingPrice { get; set; }

    /// <summary>
    /// Gets or sets the notification option.
    /// </summary>
    /// <value>The notification option.</value>
    [Entity(FieldName = "NotificationOption")]
    public override NotificationOption NotificationOption { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the notification option value.
    /// </summary>
    /// <value>The notification option value.</value>
    [Entity(FieldName = "NotificationOptionValue")]
    public override string NotificationOptionValue { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    /// <value>The currency.</value>
    [Entity(FieldName = "Currency")]
    public override Currency Currency { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual string Alias { get; [NotNullValue] set; }
  }
}