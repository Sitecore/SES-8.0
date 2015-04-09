// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleOrderListModelDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the single order list model data source repository class.
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
  using Diagnostics;
  using Models;

  /// <summary>
  /// Defines the single order list model data source repository class.
  /// </summary>
  public class SingleOrderListModelDataSourceRepository : OrderListModelDataSourceRepository
  {
    /// <summary>
    /// Selects the specified raw query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>
    /// The I enumerable.
    /// </returns>
    [NotNull]
    public override IEnumerable<OrderListModel> SelectEntities([NotNull] string rawQuery)
    {
      Assert.ArgumentNotNull(rawQuery, "rawQuery");
      Assert.ArgumentNotNullOrEmpty(rawQuery, "rawQuery");

      IEnumerable<OrderListModel> orders = this.GetOrders().AsQueryable().Where(o => o.OrderId == rawQuery).Select(GetOrderListModel);

      return orders;
    }
  }
}