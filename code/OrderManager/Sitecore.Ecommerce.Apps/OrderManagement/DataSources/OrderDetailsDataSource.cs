// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderDataSource.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderDataSource type. The class is used to instantiate MerchantOrderManager
//   in a proper way using Unity Container.
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
  using Sitecore.Ecommerce.Merchant.OrderManagement;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the OrderDataSource type. The class is used to instantiate MerchantOrderManager
  /// in a proper way using Unity Container.
  /// </summary>
  public class OrderDetailsDataSource
  {
    /// <summary>
    /// The merchant order manager.
    /// </summary>
    private readonly MerchantOrderManager merchantOrderManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderDetailsDataSource" /> class.
    /// </summary>
    public OrderDetailsDataSource()
    {
      this.merchantOrderManager = Context.Entity.Resolve<MerchantOrderManager>();
    }

    /// <summary>
    /// Gets the order by ID.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <returns>The order.</returns>
    [CanBeNull]
    public Order GetOrder([NotNull] string orderId)
    {
      return this.merchantOrderManager.GetOrder(orderId);
    }
  }
}