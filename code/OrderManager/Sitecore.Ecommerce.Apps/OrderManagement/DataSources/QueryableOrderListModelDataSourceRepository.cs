// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryableOrderListModelDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the queryable order list model data source repository class.
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
  using System.Data;
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Models;

  /// <summary>
  /// Defines the queryable order list model data source repository class.
  /// </summary>
  public class QueryableOrderListModelDataSourceRepository : OrderListModelDataSourceRepository
  {
    /// <summary>
    /// The predefined filter.
    /// </summary>
    private PredefinedFilter filter = new PredefinedFilter();

    /// <summary>
    /// The SpeakExpressionParser.
    /// </summary>
    private SpeakExpressionParser parser = new SpeakExpressionParser();

    /// <summary>
    /// Gets or sets the filter.
    /// </summary>
    /// <value>The filter.</value>
    [NotNull]
    public PredefinedFilter Filter
    {
      get 
      { 
        return this.filter; 
      }
      
      set 
      {
        Assert.ArgumentNotNull(value, "value");

        this.filter = value; 
      }
    }

    /// <summary>
    /// Gets or sets the parser.
    /// </summary>
    /// <value>The parser.</value>
    [NotNull]
    public SpeakExpressionParser Parser
    {
      get 
      { 
        return this.parser; 
      }

      set 
      {
        Assert.ArgumentNotNull(value, "value");

        this.parser = value; 
      }
    }

    /// <summary>
    /// Selects the orders.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>The orders.</returns>
    [NotNull]
    public override IEnumerable<OrderListModel> SelectEntities([CanBeNull] string rawQuery)
    {
      IEnumerable<Order> result = GetOrders();

      if (!string.IsNullOrEmpty(rawQuery))
      {
        result = this.Filter.ApplyFilter(result, rawQuery);
      }

      return result.Select(this.GetOrderListModel);
    }

    /// <summary>
    /// Selects the orders.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <param name="expression">The expression.</param>
    /// <returns>
    /// The orders.
    /// </returns>
    [NotNull]
    public DataTable Select([CanBeNull] string rawQuery, [CanBeNull]string expression)
    {
      var query = this.Parser.Parse("o", rawQuery, expression);
      return this.Select(query);
    }
  }
}