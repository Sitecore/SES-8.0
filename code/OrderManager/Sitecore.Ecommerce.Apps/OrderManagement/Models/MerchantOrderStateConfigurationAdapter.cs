// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MerchantOrderStateConfigurationAdapter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the MerchantOrderStateConfigurationAdapter type.
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
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;

  /// <summary>
  /// Defines the merchant order state configuration adapter class.
  /// </summary>
  public class MerchantOrderStateConfigurationAdapter
  {
    /// <summary>
    /// Stores the reference to merchant order state configuration.
    /// </summary>
    private readonly MerchantOrderStateConfiguration merchantOrderStateConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="MerchantOrderStateConfigurationAdapter"/> class.
    /// </summary>
    /// <param name="merchantOrderStateConfiguration">The merchant order state configuration.</param>
    public MerchantOrderStateConfigurationAdapter([NotNull] MerchantOrderStateConfiguration merchantOrderStateConfiguration)
    {
      Assert.ArgumentNotNull(merchantOrderStateConfiguration, "merchantOrderStateConfiguration");

      this.merchantOrderStateConfiguration = merchantOrderStateConfiguration;
    }

    /// <summary>
    /// Gets the states.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>The states.</returns>
    public IQueryable<StateModel> GetStates([CanBeNull] Order order)
    {
      IQueryable<State> states = this.merchantOrderStateConfiguration.GetStates();

      if (order == null)
      {
        return states.Select(state => new StateModel(state) { Enabled = true });
      }

      IQueryable<State> availableStates = this.merchantOrderStateConfiguration.GetAvailableStates(order).Union(new[] { order.State });
      return states.Select(state => new StateModel(state) { Enabled = availableStates.Any(availableState => availableState.Code == state.Code) });
    }
  }
}