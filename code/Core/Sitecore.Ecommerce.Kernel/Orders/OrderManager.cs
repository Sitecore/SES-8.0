// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderProvider class. Provides order management functionality such as creating, updating and selecting orders.
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

namespace Sitecore.Ecommerce.Orders
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Configuration;
  using Data;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Configurations;
  using DomainModel.Data;
  using DomainModel.Orders;
  using DomainModel.Payments;
  using Payments;
  using Search;
  using SecurityModel;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Exceptions;
  using Statuses;
  using Utils;

  /// <summary>
  /// Defines the OrderProvider class. Provides order management functionality such as creating, updating and selecting orders.
  /// </summary>
  /// <typeparam name="T">The order type</typeparam>
  [Obsolete]
  public class OrderManager<T> : IOrderManager<T> where T : DomainModel.Orders.Order
  {
    /// <summary>
    /// The entity helper instance.
    /// </summary>
    private readonly EntityHelper entityHelper;
    
    /// <summary>
    /// The order display template Id.                 
    /// </summary>
    private readonly ID orderItemTempalteId = new ID(Settings.GetSetting("Ecommerce.Order.OrderItemTempalteId"));

    /// <summary>
    /// The order line template Id.                 
    /// </summary>
    private readonly TemplateID orderLineItemTempalteId = new TemplateID(new ID(Settings.GetSetting("Ecommerce.Order.OrderLineItemTempalteId")));

    /// <summary>
    /// The search provider.
    /// </summary>
    private ISearchProvider searchProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderManager&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="searchProvider">The search provider.</param>
    /// <param name="orderIdGenerator">The order id generator.</param>
    public OrderManager(ISearchProvider searchProvider, OrderIDGenerator orderIdGenerator)
    {
      this.entityHelper = Context.Entity.Resolve<EntityHelper>();
      this.searchProvider = searchProvider;
      this.OrderIDGenerator = orderIdGenerator;

      BusinessCatalogSettings settings = Context.Entity.GetConfiguration<BusinessCatalogSettings>();
      Assert.IsNotNull(settings, typeof(BusinessCatalogSettings), "Unable initialize BusinessCatalogSettings.");
      Assert.IsNotNullOrEmpty(settings.OrdersLink, "Unable initialize Orders root item.");

      this.Database = Sitecore.Context.ContentDatabase;
      this.ItemId = settings.OrdersLink;
    }

    /// <summary>
    /// Gets or sets the order number generator.
    /// </summary>
    /// <value>
    /// The order number generator.
    /// </value>
    public OrderIDGenerator OrderIDGenerator { get; set; }

    /// <summary>
    /// Gets or sets the database.
    /// </summary>
    /// <value>The database.</value>
    protected Database Database { get; set; }

    /// <summary>
    /// Gets or sets the item id.
    /// </summary>
    /// <value>The item id.</value>
    protected string ItemId { get; set; }

    /// <summary>
    /// Gets or sets the search provider.
    /// </summary>
    /// <value>The search provider.</value>
    protected virtual ISearchProvider SearchProvider
    {
      get { return this.searchProvider; }
      set { this.searchProvider = value; }
    }

    /// <summary>
    /// Gets the orders source item.
    /// </summary>
    /// <value>The orders item.</value>
    private Item OrdersItem
    {
      get
      {
        Assert.IsNotNull(this.Database, "Orders database not found.");

        return this.Database.GetItem(this.ItemId);
      }
    }

    #region Implementation of the IOrderManager

    /// <summary>
    /// Gets the order number.
    /// </summary>
    /// <returns>
    /// If ordernumber exists, returns ordernumber, else returns null.
    /// </returns>
    public virtual string GenerateOrderNumber()
    {
      return this.OrderIDGenerator.Generate();
    }

    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <returns>The order.</returns>
    public virtual T GetOrder(string orderNumber)
    {
      Query query = new Query();
      query.Add(new FieldQuery("OrderNumber", orderNumber, MatchVariant.Exactly));

      return this.GetOrders(query).FirstOrDefault();
    }

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="searchQuery">The search query.</param>
    /// <returns>The order collection</returns>
    public virtual IEnumerable<T> GetOrders<TQuery>(TQuery searchQuery)
    {
      return this.GetOrdersItems(searchQuery).Select<Item, T>(this.GetOrderFromItem);
    }

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns>The orders.</returns>
    public virtual IEnumerable<T> GetOrders<TQuery>(TQuery query, int pageIndex, int pageSize)
    {
      return this.GetOrdersItems(query).Skip(pageIndex * pageSize).Take(pageSize).Select<Item, T>(this.GetOrderFromItem);
    }

    /// <summary>
    /// Gets the orders count.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <returns>Return selected orders count.</returns>
    public virtual int GetOrdersCount<TQuery>(TQuery query)
    {
      return this.GetOrdersItems(query).Count();
    }

    /// <summary>
    /// Gets the orders items.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="searchQuery">The search query.</param>
    /// <returns>Returns orders items.</returns>
    protected virtual IEnumerable<Item> GetOrdersItems<TQuery>(TQuery searchQuery)
    {
      Assert.ArgumentNotNull(searchQuery, "Query");
      Assert.IsTrue(searchQuery is Query, "Query type is invalid");

      Query query = searchQuery as Query;

      if (string.IsNullOrEmpty(query.SearchRoot))
      {
        Item orderRoot = this.OrdersItem;
        Assert.IsNotNull(orderRoot, "Orders root item is null");

        query.SearchRoot = orderRoot.ID.ToString();
      }

      AttributeQuery templateId = new AttributeQuery("TemplateId", this.orderItemTempalteId.ToString(), MatchVariant.Exactly);
      if (!query.Contains(templateId))
      {
        if (!query.IsEmpty())
        {
          query.AppendCondition(QueryCondition.And);
        }

        query.Add(templateId);
      }

      return this.SearchProvider.Search(query, this.OrdersItem.Database).OrderByDescending(itm => itm["OrderNumber"]);
    }

    /// <summary>
    /// Creates the order.
    /// </summary>
    /// <typeparam name="TShoppingCart">The type of the shopping cart.</typeparam>
    /// <param name="shoppingCart">The shopping cart.</param>
    /// <returns>The order.</returns>
    /// <exception cref="ConfigurationException">The Order/Default Status For Order Registration setting did't contain a valid Status item.</exception>
    /// <exception cref="Exception"><c>Exception</c>.</exception>
    public virtual T CreateOrder<TShoppingCart>(TShoppingCart shoppingCart) where TShoppingCart : ShoppingCart
    {
      Assert.IsNotNull(shoppingCart, "Shopping Cart is null");

      Events.Event.RaiseEvent("order:creating", this);

      TemplateItem orderTemplateItem = this.Database.GetTemplate(this.orderItemTempalteId);
      Assert.IsNotNull(orderTemplateItem, "Order item template is null");

      T order = Context.Entity.Resolve<T>();
      this.entityHelper.CopyPropertiesValues(shoppingCart, ref order);
      foreach (ShoppingCartLine line in shoppingCart.ShoppingCartLines)
      {
        DomainModel.Orders.OrderLine orderLine = Context.Entity.Resolve<DomainModel.Orders.OrderLine>();

        orderLine.Product = line.Product;
        orderLine.Totals = line.Totals;
        orderLine.Quantity = line.Quantity;
        orderLine.FriendlyUrl = line.FriendlyUrl;

        order.OrderLines.Add(orderLine);
      }

      // NOTE: Save transaction number.
      ITransactionData transactionData = Context.Entity.Resolve<ITransactionData>();
      string transactionNumber = TypeUtil.TryParse(transactionData.GetPersistentValue(shoppingCart.OrderNumber, TransactionConstants.TransactionNumber), string.Empty);
      if (!string.IsNullOrEmpty(transactionNumber))
      {
        order.TransactionNumber = transactionNumber;
      }

      order.OrderDate = DateTime.Now;
      order.Status = Context.Entity.Resolve<NewOrder>();
      order.ProcessStatus();

      Item orderItem;

      using (new SecurityDisabler())
      {
        orderItem = this.OrdersItem.Add(shoppingCart.OrderNumber, orderTemplateItem);
        Assert.IsNotNull(orderItem, "Failed to create to order item");

        if (order is IEntity)
        {
          ((IEntity)order).Alias = orderItem.ID.ToString();
        }
      }

      try
      {
        this.SaveOrder(orderItem, order);
      }
      catch
      {
        using (new SecurityDisabler())
        {
          orderItem.Delete();
        }

        throw;
      }

      Events.Event.RaiseEvent("order:created", this, order);

      return order;
    }

    /// <summary>
    /// Saves the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <exception cref="ConfigurationException">The Order id is invalid.</exception>
    public virtual void SaveOrder(T order)
    {
      Assert.ArgumentNotNull(order, "order");

      Item orderItem = this.GetOrderItem(order);

      Events.Event.RaiseEvent("order:saving", this, this.GetOrder(order.OrderNumber), order);
      this.SaveOrder(orderItem, order);
      Events.Event.RaiseEvent("order:saved", this, order);
    }

    #endregion

    /// <summary>
    /// Saves the order.
    /// </summary>
    /// <param name="orderItem">The order item.</param>
    /// <param name="order">The order.</param>
    protected virtual void SaveOrder(Item orderItem, T order)
    {
      Assert.ArgumentNotNull(orderItem, "orderItem");
      Assert.ArgumentNotNull(order, "order");

      IDataMapper dataMapper = Context.Entity.Resolve<IDataMapper>();
      dataMapper.SaveEntity(order, orderItem, "OrderMappingRule");

      using (new SecurityDisabler())
      {
        foreach (DomainModel.Orders.OrderLine orderLine in order.OrderLines)
        {

          Query query = new Query();
          query.SearchRoot = orderItem.ID.ToString();
          query.AppendAttribute("templateid", this.orderLineItemTempalteId.ToString(), MatchVariant.Exactly);
          query.AppendCondition(QueryCondition.And);
          query.AppendField("id", orderLine.Product.Code, MatchVariant.Exactly);
          Item orderLineItem = this.SearchProvider.Search(query, this.Database).FirstOrDefault();
          if (orderLineItem == null)
          {
            orderLineItem = orderItem.Add(orderLine.Product.Code, this.orderLineItemTempalteId);
          }

          dataMapper.SaveEntity(orderLine, orderLineItem, "OrderLineMappingRule");
        }
      }
    }

    /// <summary>
    /// Gets the order from item.
    /// </summary>
    /// <param name="orderItem">The order item.</param>
    /// <returns>The order from item.</returns>
    protected virtual T GetOrderFromItem(Item orderItem)
    {
      if (orderItem == null)
      {
        return Context.Entity.Resolve<T>();
      }

      IDataMapper dataMapper = Context.Entity.Resolve<IDataMapper>();
      T order = dataMapper.GetEntity<T>(orderItem, "OrderMappingRule");

      order.OrderLines = new List<DomainModel.Orders.OrderLine>();

      foreach (Item orderLineItem in orderItem.Children)
      {
        DomainModel.Orders.OrderLine orderLine = dataMapper.GetEntity<DomainModel.Orders.OrderLine>(orderLineItem, "OrderLineMappingRule");
        orderLine.Id = orderLineItem.ID.ToString();

        Assert.IsNotNull(orderLine.Product, "There is no products in the orderline");

        order.OrderLines.Add(orderLine);
      }

      return order;
    }

    /// <summary>
    /// Gets the order item.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>The order item.</returns>
    /// <exception cref="ConfigurationException">The Order id is null or empty.</exception>
    /// <exception cref="InvalidOperationException">The order number is null or empty.</exception>
    protected virtual Item GetOrderItem(T order)
    {
      if (order is IEntity)
      {
        IEntity entity = order as IEntity;
        if (!string.IsNullOrEmpty(entity.Alias) || ID.IsID(entity.Alias))
        {
          Item item = this.Database.GetItem(entity.Alias);
          if (item != null)
          {
            return item;
          }
        }
      }

      if (string.IsNullOrEmpty(order.OrderNumber))
      {
        Log.Warn("The order number is null or empty.", this);
        throw new InvalidOperationException("The order number is null or empty.");
      }

      string orderItemTemplateId = string.IsNullOrEmpty(this.entityHelper.GetTemplate(typeof(T)))
                                     ? this.orderItemTempalteId.ToString()
                                     : this.entityHelper.GetTemplate(typeof(T));

      string number = this.entityHelper.GetField<T>(i => i.OrderNumber);
      if (string.IsNullOrEmpty(number))
      {
        Log.Warn(string.Concat("Field name is undefined. Type: ", typeof(T).ToString(), ". Property: 'OrderNumber'."), this);

        number = "OrderNumber";
      }

      Query query = new Query { SearchRoot = this.OrdersItem.ID.ToString() };
      query.AppendAttribute("TemplateId", orderItemTemplateId, MatchVariant.Exactly);
      query.AppendCondition(QueryCondition.And);
      query.AppendField(number, order.OrderNumber, MatchVariant.Exactly);

      return this.SearchProvider.Search(query, this.Database).FirstOrDefault();
    }
  }
}