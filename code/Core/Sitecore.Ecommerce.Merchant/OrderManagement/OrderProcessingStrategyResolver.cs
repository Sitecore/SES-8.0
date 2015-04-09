// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderProcessingStrategyResolver.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderProcessingStrategyResolver type.
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
  using System.Collections.Generic;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the strategy resolver class.
  /// </summary>
  public abstract class OrderProcessingStrategyResolver
  {
    /// <summary>
    /// Gets the strategy.
    /// </summary>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="newOrder">The new order.</param>
    /// <returns>The strategy.</returns>
    public abstract IEnumerable<OrderProcessingStrategy> GetStrategies(Order oldOrder, Order newOrder);
  }
}