// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStatusCommand.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Order Status command.
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

namespace Sitecore.Ecommerce.Shell.Applications.OrderCatalog.Models
{
  using Diagnostics;

  /// <summary>
  /// Order Status command.
  /// </summary>
  public class OrderStatusCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStatusCommand"/> class.
    /// </summary>
    /// <param name="orderStatusCode">The order status code.</param>
    /// <param name="icon">The command icon.</param>
    /// <param name="title">The display name.</param>
    public OrderStatusCommand(string orderStatusCode, string icon, string title)
    {
      Assert.ArgumentNotNullOrEmpty(orderStatusCode, "orderStatusCode");
      Assert.ArgumentNotNullOrEmpty(icon, "icon");

      this.OrderStatusCode = orderStatusCode;
      this.Icon = icon;
      this.Title = title;
    }

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    /// <value>The display name.</value>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the icon.
    /// </summary>
    /// <value>The command icon.</value>
    public string Icon { get; set; }

    /// <summary>
    /// Gets or sets the order status code.
    /// </summary>
    /// <value>The order status code.</value>
    public string OrderStatusCode { get; set; }
  }
}