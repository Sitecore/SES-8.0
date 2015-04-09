// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoteOrderManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The Remote Order Manager. Provides order management through the WCF service.
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

namespace Sitecore.Ecommerce.ServiceModel.Orders
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Data.Convertors;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Orders;
  using OrderService;
  using Search;
  
  /// <summary>
  /// The Remote Order Manager. Provides order management through the WCF service.
  /// </summary>
  [Obsolete]
  public class RemoteOrderManager : IOrderManager<Order>
  {
    /// <summary>
    /// Defines ServiceClientArgs factory.
    /// </summary>
    private readonly ServiceClientArgsFactory serviceClientArgsFactory;

    /// <summary>
    /// The order convertor.
    /// </summary>
    private readonly Convertor<Order> orderConvertor;

    /// <summary>
    /// The shopping cart convertor.
    /// </summary>
    private readonly Convertor<ShoppingCart> shoppingCartConvertor;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteOrderManager" /> class.
    /// </summary>
    /// <param name="serviceClientArgsFactory">The service client args factory.</param>
    /// <param name="orderConvertor">The order convertor.</param>
    /// <param name="shoppingCartConvertor">The shopping cart convertor.</param>
    public RemoteOrderManager(ServiceClientArgsFactory serviceClientArgsFactory, Convertor<Order> orderConvertor, Convertor<ShoppingCart> shoppingCartConvertor)
    {
      Assert.ArgumentNotNull(serviceClientArgsFactory, "serviceClientArgsFactory");
      Assert.ArgumentNotNull(orderConvertor, "orderConvertor");
      Assert.ArgumentNotNull(shoppingCartConvertor, "shoppingCartConvertor");

      this.serviceClientArgsFactory = serviceClientArgsFactory;
      this.orderConvertor = orderConvertor;
      this.shoppingCartConvertor = shoppingCartConvertor;
    }

    /// <summary>
    /// Generates the order number.
    /// </summary>
    /// <returns>The order number.</returns>
    public virtual string GenerateOrderNumber()
    {
      var args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (var client = new OrderServiceClient())
      {
        return client.GenerateOrderNumber(args);
      }
    }

    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <typeparam name="TShoppingCart">The type of the shopping cart.</typeparam>
    /// <param name="shoppingCart">The shopping cart.</param>
    /// <returns>The order.</returns>
    public virtual Order CreateOrder<TShoppingCart>(TShoppingCart shoppingCart) where TShoppingCart : ShoppingCart
    {
      Assert.IsNotNull(this.orderConvertor, "Unable to create the order. OrderConvertor cannot be null.");
      Assert.IsNotNull(this.shoppingCartConvertor, "Unable to create the order. ShoppingCartConvertor cannot be null.");

      var shoppingCartTable = this.shoppingCartConvertor.DomainModelToDTO(shoppingCart);
      var args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (var client = new OrderServiceClient())
      {
        var data = client.Create(shoppingCartTable, args);

        return this.orderConvertor.DTOToDomainModel(data).FirstOrDefault();
      }
    }

    /// <summary>
    /// Gets an order by order number.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <returns>The order.</returns>
    public virtual Order GetOrder(string orderNumber)
    {
      Assert.IsNotNull(this.orderConvertor, "Unable to create the order. OrderConvertor cannot be null.");

      var args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (var client = new OrderServiceClient())
      {
        var data = client.Get(orderNumber, args);

        return this.orderConvertor.DTOToDomainModel(data).FirstOrDefault();
      }
    }

    /// <summary>
    /// Gets the orders by search query.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <returns>The order collection.</returns>
    /// <exception cref="ArgumentException">Query type expected.</exception>
    public virtual IEnumerable<Order> GetOrders<TQuery>(TQuery query)
    {
      Assert.IsNotNull(this.orderConvertor, "Unable to create the order. OrderConvertor cannot be null.");

      if (!(query is Query))
      {
        throw new ArgumentException("Query type expected.");
      }

      var args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (var client = new OrderServiceClient())
      {
        var data = client.GetByQuery(query as Query, args);

        return this.orderConvertor.DTOToDomainModel(data);
      }
    }

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns>The orders.</returns>
    public IEnumerable<Order> GetOrders<TQuery>(TQuery query, int pageIndex, int pageSize)
    {
      Assert.IsNotNull(this.orderConvertor, "Unable to create the order. OrderConvertor cannot be null.");

      var args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (var client = new OrderServiceClient())
      {
        var data = client.GetRangeByQuery(query as Query, pageIndex, pageSize, args);

        return this.orderConvertor.DTOToDomainModel(data);
      }
    }

    /// <summary>
    /// Gets the orders count.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <returns>Returns orders count.</returns>
    public int GetOrdersCount<TQuery>(TQuery query)
    {
      var args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (var client = new OrderServiceClient())
      {
        return client.GetCount(query as Query, args);
      }
    }

    /// <summary>
    /// Saves the order.
    /// </summary>
    /// <param name="order">The order.</param>
    public virtual void SaveOrder(Order order)
    {
      Assert.IsNotNull(this.orderConvertor, "Unable to create the order. OrderConvertor cannot be null.");

      var orderData = this.orderConvertor.DomainModelToDTO(order);
      var args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (var client = new OrderServiceClient())
      {
        client.Save(orderData, args);
      }
    }
  }
}