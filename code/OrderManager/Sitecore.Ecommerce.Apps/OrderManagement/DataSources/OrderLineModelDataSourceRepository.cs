// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineModelDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineModelDataSourceRepository type.
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
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;
  using Models;

  /// <summary>
  /// Defines the order line model data source repository class.
  /// </summary>
  public class OrderLineModelDataSourceRepository : DataSourceRepository<OrderLineModel>
  {
    /// <summary>
    /// The merchant order manager
    /// </summary>
    private MerchantOrderManager orderManager;

    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>
    /// The order manager.
    /// </value>
    [NotNull]
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
    /// Selects the specified raw query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>
    /// The I enumerable.
    /// </returns>
    [NotNull]
    public override IEnumerable<OrderLineModel> SelectEntities([NotNull] string rawQuery)
    {
      Assert.ArgumentNotNull(rawQuery, "rawQuery");
      Assert.IsNotNull(this.OrderManager, "Unable to get OrderLines. OrderManager cannot be null.");

      var lines = this.OrderManager.GetOrder(rawQuery).OrderLines.Select(this.GetOrderLineModel);

      return lines;
    }

    /// <summary>
    /// Gets the specified raw query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>
    /// The order line model.
    /// </returns>
    [CanBeNull]
    public virtual OrderLineModel Get([NotNull] string rawQuery)
    {
      Assert.ArgumentNotNull(rawQuery, "rawQuery");

      long alias = long.Parse(rawQuery);

      Order order = this.OrderManager.GetOrders().FirstOrDefault(o => o.OrderLines.Any(ol => ol.Alias == alias));

      OrderLineModel orderLine = null;

      if (order != null)
      {
        orderLine = order.OrderLines.Where(ol => ol.Alias == alias).Select(this.GetOrderLineModel).FirstOrDefault();
      }

      return orderLine;
    }

    /// <summary>
    /// Gets the order line model.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <returns>
    /// The order line model.
    /// </returns>
    [NotNull]
    private OrderLineModel GetOrderLineModel([NotNull]OrderLine line)
    {
      Assert.ArgumentNotNull(line, "line");

      OrderLineModel model = new OrderLineModel
      {
        Alias = line.Alias,
        Description = line.LineItem.Item.Description,
        ProductCode = line.LineItem.Item.Code,
        ProductName = line.LineItem.Item.Name,
        Quantity = (long)line.LineItem.Quantity,
        UnitPrice = line.LineItem.Price.PriceAmount.Value,
        UnitPriceFormatted = line.LineItem.Price.PriceAmount.Value.ToString("N")
      };

      return model;
    }
  }
}