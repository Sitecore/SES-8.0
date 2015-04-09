// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderListModel.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order list model class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Models
{
  using System;

  /// <summary>
  /// Defines the order list model class.
  /// </summary>
  public class OrderListModel
  {
    /// <summary>
    /// Gets or sets the order ID.
    /// </summary>
    /// <value>
    /// The order ID.
    /// </value>
    public string ID { get; set; }

    /// <summary>
    /// Gets or sets the order date.
    /// </summary>
    /// <value>
    /// The order date.
    /// </value>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the order time.
    /// </summary>
    /// <value>
    /// The order time.
    /// </value>
    public TimeSpan IssueTime { get; set; }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    /// <value>
    /// The state.
    /// </value>
    public string State { get; set; }

    /// <summary>
    /// Gets or sets the name of the customer.
    /// </summary>
    /// <value>
    /// The name of the customer.
    /// </value>
    public string CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the total amount.
    /// </summary>
    /// <value>
    /// The total amount.
    /// </value>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    /// <value>
    /// The currency.
    /// </value>
    public string Currency { get; set; }

    /// <summary>
    /// Gets or sets the sub states.
    /// </summary>
    /// <value>
    /// The sub states.
    /// </value>
    public string SubStates { get; set; }
  }
}