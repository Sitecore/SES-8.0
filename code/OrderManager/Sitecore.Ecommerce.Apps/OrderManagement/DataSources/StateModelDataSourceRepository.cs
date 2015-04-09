// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateModelDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//  Defines the state model data source repository class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.DataSources
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Web;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;
  using Models;

  /// <summary>
  /// Defines the state model data source repository class.
  /// </summary>
  public class StateModelDataSourceRepository : DataSourceRepository<StateModel>
  {
    /// <summary>
    /// The order manager instance.
    /// </summary>
    private MerchantOrderManager orderManager;

    /// <summary>
    /// The state manager instance.
    /// </summary>
    private MerchantOrderStateConfiguration stateManager;

    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>
    /// The order manager.
    /// </value>
    [CanBeNull]
    public MerchantOrderManager OrderManager
    {
      get { return this.orderManager ?? (this.orderManager = Context.Entity.Resolve<MerchantOrderManager>()); }
      set { this.orderManager = value; }
    }

    /// <summary>
    /// Gets or sets the state manager.
    /// </summary>
    /// <value>
    /// The state manager.
    /// </value>
    [CanBeNull]
    public MerchantOrderStateConfiguration StateManager
    {
      get { return this.stateManager ?? (this.stateManager = Context.Entity.Resolve<MerchantOrderStateConfiguration>()); }
      set { this.stateManager = value; }
    }

    /// <summary>
    /// Gets the available following statuses.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <returns>
    /// The available following statuses.
    /// </returns>
    [NotNull]
    public IEnumerable<StateModel> GetStatuses([NotNull] string orderNumber)
    {
      Assert.ArgumentNotNull(orderNumber, "orderNumber");

      Assert.IsNotNull(this.OrderManager, "Unable to get the order. Order manager cannot be null.");

      Order order = this.OrderManager.GetOrder(orderNumber);

      Assert.IsNotNull(this.StateManager, "Unable to get the order state. State configuration cannot be null.");

      IEnumerable<State> availableStates = this.StateManager.GetAvailableStates(order);

      return this.StateManager.GetStates().Select(s => this.Convert(s, availableStates.Count(a => a.Code == s.Code) > 0));
    }

    /// <summary>
    /// Converts the specified state.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    /// <returns>
    /// The state list model.
    /// </returns>
    [NotNull]
    public virtual StateModel Convert([NotNull] State state, bool enabled)
    {
      Assert.ArgumentNotNull(state, "state");

      return new StateModel(state) { Enabled = enabled };
    }

    /// <summary>
    /// Gets the available following statuses by order query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>
    /// The available following statuses by order query.
    /// </returns>
    [NotNull]
    public override IEnumerable<StateModel> SelectEntities([NotNull] string rawQuery)
    {
      Assert.ArgumentNotNull(rawQuery, "rawQuery");

      return this.GetStatuses(HttpUtility.ParseQueryString(rawQuery)["Query"]);
    }
  }
}