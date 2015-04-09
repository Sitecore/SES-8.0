// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderService.svc.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The order service implementation.
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

namespace Sitecore.Ecommerce.Services
{
  using System.Collections.Generic;
  using System.Data;
  using System.ServiceModel;
  using System.ServiceModel.Activation;
  using Data.Convertors;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Orders;
  using Search;
  using Sites;
  using Visitor.OrderManagement;
  using Visitor.OrderManagement.Transient;

  /// <summary>
  /// The order service implementation.
  /// </summary>
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  public class OrderService : IOrderService
  {
    /// <summary>
    /// The DTO order convertor.
    /// </summary>
    private Convertor<Order> orderConvertor;

    /// <summary>
    /// The DTO shopping cart converrtor.
    /// </summary>
    private Convertor<ShoppingCart> cartConvertor;

    /// <summary>
    /// Gets or sets the order convertor.
    /// </summary>
    /// <value>
    /// The order convertor.
    /// </value>
    public Convertor<Order> OrderConvertor
    {
      get
      {
        return this.orderConvertor ?? (this.orderConvertor = Context.Entity.Resolve<Convertor<Order>>());
      }
      set
      {
        this.orderConvertor = value;
      }
    }

    /// <summary>
    /// Gets or sets the shopping cart convertor.
    /// </summary>
    /// <value>
    /// The shopping cart convertor.
    /// </value>
    public Convertor<ShoppingCart> ShoppingCartConvertor
    {
      get
      {
        return this.cartConvertor ?? (this.cartConvertor = Context.Entity.Resolve<Convertor<ShoppingCart>>());
      }
      set
      {
        this.cartConvertor = value;
      }
    }

    /// <summary>
    /// Generates the order number.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The order number.</returns>
    public virtual string GenerateOrderNumber(ServiceClientArgs args)
    {
      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        IOrderManager<Order> orderManager = Context.Entity.Resolve<IOrderManager<Order>>();

        return orderManager.GenerateOrderNumber();
      }
    }

    /// <summary>
    /// Creates the order.
    /// </summary>
    /// <param name="shoppingCartData">The shopping cart data.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The order.</returns>
    public virtual DataTable Create(DataTable shoppingCartData, ServiceClientArgs args)
    {
      Assert.ArgumentNotNull(shoppingCartData, "shoppingCartData");

      Assert.IsNotNull(shoppingCartData.Rows, "shoppingCartDataRow");
      Assert.IsTrue(shoppingCartData.Rows.Count == 1, "Shopping Cart data expected.");

      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        ShoppingCart shoppingCart = Context.Entity.Resolve<ShoppingCart>();

        this.ShoppingCartConvertor.DTOToDomainModel(shoppingCartData.Rows[0], ref shoppingCart);

        IOrderManager<Order> orderManager = Context.Entity.Resolve<IOrderManager<Order>>();
        Order order = orderManager.CreateOrder(shoppingCart);

        return this.OrderConvertor.DomainModelToDTO(order);
      }
    }

    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The string.</returns>
    public virtual DataTable Get(string orderNumber, ServiceClientArgs args)
    {
      Assert.ArgumentNotNullOrEmpty(orderNumber, "orderNumber");

      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        IOrderManager<Order> orderManager = Context.Entity.Resolve<IOrderManager<Order>>();
        Utils.SetCustomerId(args, orderManager);

        Order order = orderManager.GetOrder(orderNumber);

        return this.OrderConvertor.DomainModelToDTO(order);
      }
    }

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The orders.</returns>
    public virtual DataTable GetByQuery(Query query, ServiceClientArgs args)
    {
      Assert.ArgumentNotNull(query, "query");

      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        IOrderManager<Order> orderManager = Context.Entity.Resolve<IOrderManager<Order>>();
        Utils.SetCustomerId(args, orderManager);

        IEnumerable<Order> orders = orderManager.GetOrders(query);

        return this.OrderConvertor.DomainModelToDTO(orders);
      }
    }

    /// <summary>
    /// Gets the by query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The orders.</returns>
    public DataTable GetRangeByQuery(Query query, int pageIndex, int pageSize, ServiceClientArgs args)
    {
      Assert.ArgumentNotNull(query, "query");

      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        IOrderManager<Order> orderManager = Context.Entity.Resolve<IOrderManager<Order>>();
        Utils.SetCustomerId(args, orderManager);

        IEnumerable<Order> orders = orderManager.GetOrders(query, pageIndex, pageSize);

        return this.OrderConvertor.DomainModelToDTO(orders);
      }
    }

    /// <summary>
    /// Gets the orders count.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The orders count.</returns>
    public int GetCount(Query query, ServiceClientArgs args)
    {
      Assert.ArgumentNotNull(query, "query");

      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        IOrderManager<Order> orderManager = Context.Entity.Resolve<IOrderManager<Order>>();
        Utils.SetCustomerId(args, orderManager);

        return orderManager.GetOrdersCount(query);
      }
    }

    /// <summary>
    /// Saves the specified order.
    /// </summary>
    /// <param name="orderData">The order data.</param>
    /// <param name="args">The arguments.</param>
    public virtual void Save(DataTable orderData, ServiceClientArgs args)
    {
      Assert.ArgumentNotNull(orderData, "orderData");

      Assert.IsNotNull(orderData.Rows, "orderDataRow");
      Assert.IsTrue(orderData.Rows.Count == 1, "Order data expected.");

      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        IOrderManager<Order> orderManager = Context.Entity.Resolve<IOrderManager<Order>>();

        Order order = Context.Entity.Resolve<Order>();
        this.OrderConvertor.DTOToDomainModel(orderData.Rows[0], ref order);

        orderManager.SaveOrder(order);
      }
    }
  }
}