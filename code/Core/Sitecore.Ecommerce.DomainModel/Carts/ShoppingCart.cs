// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCart.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ShoppingCart type.
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

namespace Sitecore.Ecommerce.DomainModel.Carts
{
  using System;
  using System.Collections.Generic;
  using Currencies;
  using Payments;
  using Prices;
  using Shippings;
  using Users;

  /// <summary>
  /// Defines the ShoppingCart type.
  /// </summary>
  [Serializable]
  public class ShoppingCart
  {
    /// <summary>
    /// Gets or sets the order number.
    /// </summary>
    /// <value>The order number.</value>
    public virtual string OrderNumber { get; set; }

    /// <summary>
    /// Gets or sets the product lines.
    /// </summary>
    /// <value>The product lines.</value>
    public virtual IList<ShoppingCartLine> ShoppingCartLines { get; set; }

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
  }
}