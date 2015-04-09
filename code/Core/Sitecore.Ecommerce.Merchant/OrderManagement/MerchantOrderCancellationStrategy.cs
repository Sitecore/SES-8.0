// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MerchantOrderCancellationStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the MerchantOrderCancellationStrategy type.
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

  /// <summary>
  /// Defines the merchant order cancellation strategy class.
  /// </summary>
  public class MerchantOrderCancellationStrategy : ProcessingStrategy
  {
    /// <summary>
    /// Gets or sets StateManager.
    /// </summary>
    public virtual CoreOrderStateConfiguration StateManager { get; set; }

    /// <summary>
    /// Gets or sets the reason substate code.
    /// </summary>
    /// <value>The reason substate code.</value>
    public string ReasonSubstateCode { get; set; }

    /// <summary>
    /// Gets or sets the order security.
    /// </summary>
    /// <value>
    /// The order security.
    /// </value>
    [CanBeNull]
    public MerchantOrderSecurity OrderSecurity { get; set; }

    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="order">The order.</param>
    public override void Process(Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      Assert.IsNotNull(this.StateManager, "Unable to cancel the order. The inner state manager is not set.");
      Assert.IsNotNull(this.OrderSecurity, "Unable to cancel the order. OrderSecurity cannot be null.");

      Assert.IsTrue(this.OrderSecurity.CanCancel(order), "Unable to cancel the order. Cancellation denied.");

      State cancelledState = this.StateManager.GetFollowingStates(order.State).SingleOrDefault(state => state.Code == OrderStateCode.Cancelled);
      Assert.IsNotNull(cancelledState, "Unable to cancel the order. Cancellation not allowed for the current state.");

      order.State = cancelledState;

      Substate substate = order.State.Substates.SingleOrDefault(s => s.Code == this.ReasonSubstateCode);
      Assert.IsNotNull(substate, "Unable to cancel the order. Cancellation reason code not found.");

      substate.Active = true;
    }
  }
}