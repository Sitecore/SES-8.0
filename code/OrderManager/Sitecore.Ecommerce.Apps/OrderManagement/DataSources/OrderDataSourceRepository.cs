// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderDataSourceRepository type.
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
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;

  /// <summary>
  /// Defines the order data source repository class.
  /// </summary>
  /// <typeparam name="T">The repository item.</typeparam>
  public abstract class OrderDataSourceRepository<T> : DataSourceRepository<T>
  {
    /// <summary>
    /// The order manager.
    /// </summary>
    private MerchantOrderManager orderManager;

    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>
    /// The order manager.
    /// </value>
    [CanBeNull]
    public MerchantOrderManager OrderManager
    {
      get
      {
        return this.orderManager ?? (this.orderManager = Context.Entity.Resolve<MerchantOrderManager>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        this.orderManager = value;
      }
    }

    /// <summary>
    /// Returns the collection of the whole orders.
    /// </summary>
    /// <returns>
    /// Collection of the orders.
    /// </returns>
    [NotNull]
    protected virtual IEnumerable<Order> GetOrders()
    {
      Assert.IsNotNull(this.OrderManager, "Unable to get the orders. OrderManager cannot be null.");

      return this.OrderManager.GetOrders();
    }
  }
}