// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICreateOrderActionView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The OrderManagerActionsView interface.
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
  /// <summary>
  /// The ICreateOrderActionView interface.
  /// </summary>
  public interface ICreateOrderActionView
  {
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="ICreateOrderActionView" /> is enabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if visible; otherwise, <c>false</c>.
    /// </value>
    bool Enabled { get; set; }

    /// <summary>
    /// Redirects to order details.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    void RedirectToOrderDetails(string orderId);
  }
}
