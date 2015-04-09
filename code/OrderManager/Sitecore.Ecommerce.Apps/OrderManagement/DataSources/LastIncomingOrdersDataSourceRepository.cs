// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LastIncomingOrdersDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the last incoming orders data source repository class.
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
  using Ecommerce.OrderManagement;
  using Models;

  /// <summary>
  /// Defines the last new orders data source repository class.
  /// </summary>
  public class LastIncomingOrdersDataSourceRepository : OrderListModelDataSourceRepository
  {
    /// <summary>
    /// Returns the collection of the last orders that are in "New" state.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>
    /// Collection of the orders.
    /// </returns>
    [NotNull]
    public override IEnumerable<OrderListModel> SelectEntities([CanBeNull] string rawQuery)
    {
      return this.GetOrders().AsQueryable()
        .Where(o => o.State.Code == OrderStateCode.Open)
        .OrderByDescending(o => o.IssueDate)
        .Take(6)
        .Select(GetOrderListModel);
    }
  }
}