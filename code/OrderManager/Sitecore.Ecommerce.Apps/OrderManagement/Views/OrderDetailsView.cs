// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderDetailsView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order details view class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Views
{
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the order details view class.
  /// </summary>
  public class OrderDetailsView : IOrderDetailsView
  {
    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>The order.</value>
    public Order Order { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is read only.
    /// </summary>
    /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
    public bool IsReadOnly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance can reopen order.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance can reopen order; otherwise, <c>false</c>.
    /// </value>
    public bool CanReopenOrder { get; set; }
  }
}