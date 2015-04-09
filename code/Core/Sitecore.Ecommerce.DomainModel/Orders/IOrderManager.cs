// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrderManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The order manager.
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

namespace Sitecore.Ecommerce.DomainModel.Orders
{
  using System;
  using System.Collections.Generic;
  using Carts;

  /// <summary>
  /// The order manager.
  /// </summary>
  /// <typeparam name="TOrder">The type of the order.</typeparam>
  [Obsolete]
  public interface IOrderManager<TOrder> where TOrder : Order
  {
    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <returns>The order.</returns>
    TOrder GetOrder(string orderNumber);

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <returns>The order collection</returns>
    IEnumerable<TOrder> GetOrders<TQuery>(TQuery query);

    /// <summary>
    /// Creates the order.
    /// </summary>
    /// <typeparam name="TShoppingCart">The type of the shopping cart.</typeparam>
    /// <param name="shoppingCart">The shopping cart.</param>
    /// <returns>The order.</returns>
    TOrder CreateOrder<TShoppingCart>(TShoppingCart shoppingCart) where TShoppingCart : ShoppingCart;

    /// <summary>
    /// Saves the order.
    /// </summary>
    /// <param name="order">The order.</param>
    void SaveOrder(TOrder order);

    /// <summary>
    /// Generates the order number.
    /// </summary>
    /// <returns>The order number.</returns>
    string GenerateOrderNumber();

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns></returns>
    IEnumerable<TOrder> GetOrders<TQuery>(TQuery query, int pageIndex, int pageSize);

    /// <summary>
    /// Gets the orders count.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <returns>Returns selected orders count.</returns>
    int GetOrdersCount<TQuery>(TQuery query);
  }
}