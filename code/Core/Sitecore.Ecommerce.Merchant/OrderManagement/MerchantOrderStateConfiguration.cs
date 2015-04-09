// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MerchantOrderStateConfiguration.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the merchant order state configuration class.
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
  /// Defines the merchant order state configuration class.
  /// </summary>
  public class MerchantOrderStateConfiguration
  {
    /// <summary>
    /// Gets or sets the state manager.
    /// </summary>
    /// <value>
    /// The state manager.
    /// </value>
    [CanBeNull]
    public CoreOrderStateConfiguration StateManager { get; set; }

    /// <summary>
    /// Gets or sets the order security.
    /// </summary>
    /// <value>
    /// The order security.
    /// </value>
    [CanBeNull]
    public MerchantOrderSecurity OrderSecurity { get; set; }

    /// <summary>
    /// Gets the states.
    /// </summary>
    /// <returns>
    /// The states.
    /// </returns>
    [NotNull]
    public virtual IQueryable<State> GetStates()
    {
      Assert.IsNotNull(this.StateManager, "Unable to get states. StateManager cannot be null.");

      return this.StateManager.GetStates();
    }

    /// <summary>
    /// Gets the available states.
    /// </summary>
    /// <param name="order">
    /// The order.
    /// </param>
    /// <returns>
    /// The available states.
    /// </returns>
    [NotNull]
    public virtual IQueryable<State> GetAvailableStates([NotNull]Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      State state = order.State;
      Assert.IsNotNull(state, "Order state cannot be null.");

      State currentState = this.GetStates().Single(s => s.Code == state.Code);
      IQueryable<State> result = this.GetFollowingStates(state).Union(Enumerable.Repeat(currentState, 1));

      Assert.IsNotNull(this.OrderSecurity, "Unable to get states. OrderSecurity cannot be null.");

      if (this.OrderSecurity.CanRevert(order))
      {
        result = result.Union(this.GetPreviousStates(state));
      }

      if (!this.OrderSecurity.CanCancel(order))
      {
        result = result.Where(r => r.Code != OrderStateCode.Cancelled);
      }

      if (this.OrderSecurity.CanReopen(order))
      {
        var reopenStates = this.GetStates().Where(s => s.Code == OrderStateCode.New || s.Code == OrderStateCode.InProcess || s.Code == OrderStateCode.Open);
        result = result.Union(reopenStates);
      }

      return result;
    }

    /// <summary>
    /// Gets the admissible substates.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>
    /// The admissible substates.
    /// </returns>
    [NotNull]
    public virtual IQueryable<Substate> GetAdmissibleSubstates([NotNull] State state)
    {
      Assert.ArgumentNotNull(state, "state");
      Assert.IsNotNull(this.StateManager, "Unable to get admissible sub-states. StateManager cannot be null.");

      SubstateCombinationSet substateCombinationSet = this.StateManager.GetSubstateCombinations(state);

      return (new StateValidator()).GetAdmissibleSubstates(state, substateCombinationSet).AsQueryable();
    }

    /// <summary>
    /// Gets the following states.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>
    /// The following states.
    /// </returns>
    [NotNull]
    public virtual IQueryable<State> GetFollowingStates([NotNull] State state)
    {
      Assert.IsNotNull(state, "state");
      Assert.IsNotNull(this.StateManager, "Unable to get following states. StateManager cannot be null.");

      return this.StateManager.GetFollowingStates(state);
    }

    /// <summary>
    /// Gets the previous states.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>
    /// The previous states.
    /// </returns>
    [NotNull]
    public virtual IQueryable<State> GetPreviousStates([NotNull] State state)
    {
      Assert.IsNotNull(state, "state");
      Assert.IsNotNull(this.StateManager, "Unable to get previous states. StateManager cannot be null.");

      return this.StateManager.GetPreviousStates(state);
    }

    /// <summary>
    /// Determines whether the specified state is valid.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>
    ///   <c>true</c> if the specified state is valid; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool IsValid([NotNull] State state)
    {
      Assert.IsNotNull(state, "state");
      Assert.IsNotNull(this.StateManager, "Unable to check whether state is valid. StateManager cannot be null.");

      return this.StateManager.IsValid(state);
    }
  }
}