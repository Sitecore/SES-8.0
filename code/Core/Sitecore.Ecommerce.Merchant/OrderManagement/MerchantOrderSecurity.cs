// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MerchantOrderSecurity.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the merchant order security class.
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

namespace Sitecore.Ecommerce.Merchant.OrderManagement
{
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Sitecore.Security.Accounts;

  /// <summary>
  /// Defines the merchant order security class.
  /// </summary>
  public class MerchantOrderSecurity
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
    /// Gets or sets the state configuration.
    /// </summary>
    /// <value>The state configuration.</value>
    [CanBeNull]
    public CoreOrderStateConfiguration OrderStateConfiguration { get; set; } 

    /// <summary>
    /// Determines whether this instance can process the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns><c>true</c> if this instance can process the specified order; otherwise, <c>false</c>.</returns>
    public virtual bool CanProcess([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      State state = order.State;
      Assert.IsNotNull(state, "Order state cannot be null.");

      return state.Code != OrderStateCode.Cancelled && state.Code != OrderStateCode.Closed && state.Code != OrderStateCode.Suspicious;
    }

    /// <summary>
    /// Determines whether this instance [can edit order lines] the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    ///   <c>true</c> if this instance [can edit order lines] the specified order; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool CanEditOrderLines([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      State state = order.State;
      Assert.IsNotNull(state, "Order state cannot be null.");

      return this.CanProcess(order) && !((state.Code == OrderStateCode.InProcess) && 
                                          (state.Substates.Any(s => (s.Code == OrderStateCode.InProcessShippedInFull) && s.Active) || 
                                            state.Substates.Any(s => (s.Code == OrderStateCode.InProcessCapturedInFull) && s.Active)));
    }

    /// <summary>
    /// Determines whether this instance can cancel the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns><c>true</c> if this instance can cancell the specified order; otherwise, <c>false</c>.</returns>
    public virtual bool CanCancel([NotNull]Order order)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.IsNotNull(this.User, "Unable to determine whether this order can be cancelled. User cannot be null.");
      Assert.IsNotNull(this.OrderStateConfiguration, "Unable to determine whether this order can be cancelled. OrderStateConfiguration cannot be null.");

      return this.OrderStateConfiguration.GetFollowingStates(order.State).Any(state => state.Code == OrderStateCode.Cancelled) && this.UserInAdministerRole();
    }

    /// <summary>
    /// Determines whether this instance can reopen the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns><c>true</c> if this instance can reopen the specified order; otherwise, <c>false</c>.</returns>
    public virtual bool CanReopen([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.IsNotNull(this.User, "Unable to determine whether this order can be reopened. User cannot be null.");

      State state = order.State;
      Assert.IsNotNull(state, "Order state cannot be null.");

      return ((state.Code == OrderStateCode.Closed || state.Code == OrderStateCode.Cancelled) && (this.User.IsAdministrator || this.User.IsInRole(OrderManagerRole.OrderManagerAdministrators))) ||
        (state.Code == OrderStateCode.Suspicious && this.UserInAdministerRole());
    }

    /// <summary>
    /// Determines whether this instance can change current order state to any previous state.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns><c>true</c> if this instance can revert the specified order to previous state; otherwise, <c>false</c>.</returns>
    public virtual bool CanRevert([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.IsNotNull(this.User, "Unable to determine whether this order can be reopened. User cannot be null.");

      return this.UserInAdministerRole();
    }

    /// <summary>
    /// Determines whether current user can create new order.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if current user can create new order; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool CanCreate()
    {
      Assert.IsNotNull(this.User, "Unable to determine whether current can create new order. User cannot be null.");

      return this.UserInAdministerRole();
    }

    /// <summary>
    /// Is the user in administer role.
    /// </summary>
    /// <returns>Boolean value.</returns>
    private bool UserInAdministerRole()
    {
      Assert.IsNotNull(this.User, "User cannot be null.");
      return this.User.IsAdministrator || this.User.IsInRole(OrderManagerRole.OrderManagerAdministrators) || this.User.IsInRole(OrderManagerRole.OrderManagerProcessing);
    }
  }
}
