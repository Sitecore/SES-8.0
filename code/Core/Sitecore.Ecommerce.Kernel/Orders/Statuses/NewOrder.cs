// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewOrder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The New state.
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

namespace Sitecore.Ecommerce.Orders.Statuses
{
  using System;

  using DomainModel.Orders;
  using Products;
  using Sitecore.Ecommerce.DomainModel.Products;
  using Utils;

  /// <summary>
  /// The New state.
  /// </summary>
  [Obsolete]
  public class NewOrder : OrderStatusBase
  {
    /// <summary>
    /// Processes the specified order.
    /// </summary>
    /// <typeparam name="T">The order type.</typeparam>
    /// <param name="order">The order instance.</param>
    protected override void Process<T>(T order)
    {
      if (order.OrderLines.IsNullOrEmpty())
      {
        return;
      }

      foreach (OrderLine indexOrderLine in order.OrderLines)
      {
        if (!(indexOrderLine.Product is Product))
        {
          continue;
        }

        IProductStockManager stockManager = Context.Entity.Resolve<IProductStockManager>();
        ProductStockInfo stockInfo = new ProductStockInfo { ProductCode = indexOrderLine.Product.Code };
        OrderLine line = indexOrderLine;

        stockManager.Update(stockInfo, s => s - line.Quantity);
      }
    }
  }
}