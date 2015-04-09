// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrdersSource.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the orders source class.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models.Search
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using DomainModel.Orders;
  using Sitecore.Web.UI.Grids;

  /// <summary>
  /// Defines the orders source class.
  /// </summary>
  public class OrdersSource : CatalogBaseSource
  {
    /// <summary>
    /// Gets or sets Order mapping rules.
    /// </summary>
    private IEnumerable<Order> orders;

    /// <summary>
    /// Order Manager
    /// </summary>
    private IOrderManager<Order> orderManager;

    /// <summary>
    /// Gets the order manager.
    /// </summary>
    /// <value>The order manager.</value>
    [NotNull]
    protected virtual IOrderManager<Order> OrderManager
    {
      get { return this.orderManager ?? (this.orderManager = Context.Entity.Resolve<IOrderManager<Order>>()); }
    }

    /// <summary>
    /// Filters the specified filters.
    /// </summary>
    /// <param name="filters">The filters.</param>
    public override void Filter([NotNull] IList<GridFilter> filters)
    {
      Assert.ArgumentNotNull(filters, "filters");
    }

    /// <summary>
    /// Gets the entry count.
    /// </summary>
    /// <returns>Returns entry count</returns>
    public override int GetEntryCount()
    {
      return this.OrderManager.GetOrdersCount(this.GetQuery());
    }

    /// <summary>
    /// Gets the entries in content language.
    /// </summary>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns>
    /// The entries in content language.
    /// </returns>
    protected override IEnumerable<List<string>> GetContentLanguageEntries(int pageIndex, int pageSize)
    {
      IEnumerable<Order> ordersList = this.orders == null ? this.OrderManager.GetOrders(this.GetQuery(), pageIndex, pageSize).ToList() : this.GetOrders().Skip(pageIndex * pageSize).Take(pageSize).ToList();

      IList<List<string>> gridData = new EntityResultDataConverter<Order>().Convert(ordersList, this.SearchOptions.GridColumns).Rows;
      return gridData;
    }

    /// <summary>
    /// Gets data ro sort.
    /// </summary>
    /// <param name="elementType">The element type.</param>
    /// <returns>
    /// query able list of data.
    /// </returns>
    [AllowNull("*")]
    protected override IQueryable GetSource(out Type elementType)
    {
      var firstElement = this.GetOrders().FirstOrDefault();
      if (firstElement == null)
      {
        elementType = null;
        return null;
      }

      elementType = firstElement.GetType();
      IQueryable tempSource = this.orders.AsQueryable();

      return this.CastSource(elementType, tempSource);
    }

    /// <summary>
    /// Updates the data.
    /// </summary>
    /// <param name="source">The source.</param>
    protected override void UpdateSourceData([NotNull] IQueryable source)
    {
      Assert.ArgumentNotNull(source, "source");

      this.orders = source.OfType<Order>().AsEnumerable();
    }

    /// <summary>
    /// Gets the orders from the storage.
    /// </summary>
    /// <returns>Enumeration of the orders.</returns>
    [NotNull]
    protected virtual IEnumerable<Order> GetOrders()
    {
      return this.orders ?? (this.orders = this.OrderManager.GetOrders(this.GetQuery()));
    }
  }
}