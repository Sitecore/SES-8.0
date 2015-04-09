// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisitorOrderSecurity.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the visitor order security class.
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

namespace Sitecore.Ecommerce.Visitor.OrderManagement
{
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Sitecore.Security.Accounts;

  /// <summary>
  /// Defines the visitor order security class.
  /// </summary>
  public class VisitorOrderSecurity
  {
    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    /// <value>
    /// The user.
    /// </value>
    [CanBeNull]
    public User User { get; set; }

    /// <summary>
    /// Gets or sets the order state configuration.
    /// </summary>
    /// <value>The order state configuration.</value>
    [CanBeNull]
    public CoreOrderStateConfiguration OrderStateConfiguration { get; set; }

    /// <summary>
    /// Determines whether this instance can cancel the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// <c>true</c> if this instance can cancell the specified order; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool CanCancel(Order order)
    {
      Assert.IsNotNull(this.User, "Unable to determine whether this order can be cancelled. User cannot be null.");
      Assert.IsNotNull(this.OrderStateConfiguration, "Unable to determine whether this order can be cancelled. OrderStateConfiguration cannot be null.");

      return this.OrderStateConfiguration.GetFollowingStates(order.State).Any(state => state.Code == OrderStateCode.Cancelled);
    }
  }
}
