// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductStockManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Represents the product stock manager. Determines the product stock value by product info.
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

namespace Sitecore.Ecommerce.DomainModel.Products
{
  using System;
  using System.Linq.Expressions;

  /// <summary>
  /// Represents the product stock manager. Determines the product stock value by product info.
  /// </summary>
  public interface IProductStockManager
  {
    /// <summary>
    /// Gets the product stock.
    /// </summary>
    /// <param name="stockInfo">The product stock info. Contains info required to get product stock value.
    /// By default it's product code.</param>
    /// <returns>The product stock.</returns>
    ProductStock GetStock(ProductStockInfo stockInfo);

    /// <summary>
    /// Updates the product stock.
    /// </summary>
    /// <param name="stockInfo">The stock info.</param>
    /// <param name="newAmount">The new amount.</param>
    void Update(ProductStockInfo stockInfo, long newAmount);

    /// <summary>
    /// Updates the specified stock info.
    /// </summary>
    /// <param name="stockInfo">The stock info.</param>
    /// <param name="expression">The expression.</param>
    void Update(ProductStockInfo stockInfo, Expression<Func<long, long>> expression);
  }
}