// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisitorOrderCancelationStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the visitor order cancelation strategy class.
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

  /// <summary>
  /// Defines the visitor order cancelation strategy class.
  /// </summary>
  public class VisitorOrderCancelationStrategy : ProcessingStrategy
  {
    /// <summary>
    /// Gets or sets StateManager.
    /// </summary>
    public virtual CoreOrderStateConfiguration StateManager { get; set; }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    public override void Process([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      Assert.IsNotNull(this.StateManager, "Unable to cancel the order. The inner state manager is not set.");

      State cancelledState = this.StateManager.GetFollowingStates(order.State).SingleOrDefault(state => state.Code == OrderStateCode.Cancelled);

      Assert.IsNotNull(cancelledState, "Unable to cancel the order in current state.");

      order.State = cancelledState;
      order.State.Substates.Single(ss => ss.Code == "Customer").Active = true;
    }
  }
}