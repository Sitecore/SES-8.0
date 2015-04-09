// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransientOrderManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the transient order manager class.
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

namespace Sitecore.Ecommerce.Visitor.OrderManagement.Transient
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Security;
  using Data;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Orders;
  using DomainModel.Payments;
  using DomainModel.Users;
  using Ecommerce.OrderManagement;
  using Orders.Statuses;
  using Payments;
  using Utils;
  using OldOrder = DomainModel.Orders.Order;

  /// <summary>
  /// Defines the transient order manager class.
  /// </summary>
  public class TransientOrderManager : IOrderManager<OldOrder>, IUserAware
  {
    #region Fields

    /// <summary>
    /// Gets or sets the order converter.
    /// </summary>
    /// <value>
    /// The order converter.
    /// </value>
    [NotNull]
    internal TransientOrderConverter orderConverter;

    /// <summary>
    /// Gets or sets the EntityHelper instance.
    /// </summary>
    [NotNull]
    internal EntityHelper entityHelper;

    /// <summary>
    /// Gets or sets the inner repository.
    /// </summary>
    /// <value>
    /// The inner repository.
    /// </value>
    [NotNull]
    private readonly VisitorOrderManager innerRepository;

    /// <summary>
    /// Gets or sets the order number generator.
    /// </summary>
    /// <value>
    /// The order number generator.
    /// </value>
    [NotNull]
    private readonly OrderIDGenerator orderIDGenerator;

    /// <summary>
    /// Gets or sets CustomerManager.
    /// </summary>
    [NotNull]
    private readonly ICustomerManager<CustomerInfo> customerManager;

    /// <summary>
    /// Gets or sets transactionData.
    /// </summary>
    [NotNull]
    private readonly ITransactionData transactionData;

    /// <summary>
    /// Gets or sets the shop context.
    /// </summary>
    /// <value>The shop context.</value>
    [NotNull]
    private readonly ShopContext shopContext;

    /// <summary>
    /// Default start path item.
    /// </summary>
    private Sitecore.Data.Items.Item defaultStartPathItem;

    /// <summary>
    /// Customer id.
    /// </summary>
    private string customerId;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransientOrderManager" /> class.
    /// </summary>
    /// <param name="innerRepository">The inner repository.</param>
    /// <param name="orderConverter">The order converter.</param>
    /// <param name="orderIdGenerator">The order id generator.</param>
    /// <param name="customerManager">The customer manager.</param>
    /// <param name="entityHelper">The entity helper.</param>
    /// <param name="transactionData">The transaction data.</param>
    /// <param name="shopContext">The shop context.</param>
    public TransientOrderManager(VisitorOrderManager innerRepository, TransientOrderConverter orderConverter, OrderIDGenerator orderIdGenerator, ICustomerManager<CustomerInfo> customerManager, EntityHelper entityHelper, ITransactionData transactionData, ShopContext shopContext)
    {
      this.innerRepository = innerRepository;
      this.orderConverter = orderConverter;
      this.orderIDGenerator = orderIdGenerator;
      this.customerManager = customerManager;
      this.entityHelper = entityHelper;
      this.transactionData = transactionData;
      this.shopContext = shopContext;
    }

    /// <summary>
    /// Gets or sets the default start path item.
    /// </summary>
    /// <value>
    /// The default start path item.
    /// </value>
    [NotNull]
    public Sitecore.Data.Items.Item DefaultStartPathItem
    {
      get
      {
        return this.defaultStartPathItem ??
          (this.defaultStartPathItem = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath));
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        this.defaultStartPathItem = value;
      }
    }

    /// <summary>
    /// Gets or sets the customer id.
    /// </summary>
    /// <value>
    /// The customer id.
    /// </value>
    [NotNull]
    public virtual string CustomerId
    {
      get
      {
        if (this.customerId == null)
        {
          Assert.IsNotNull(this.customerManager, "CustomerManager must be set.");
          Assert.IsNotNull(this.customerManager.CurrentUser, "CustomerManager.CurrentUser must be set.");
          this.customerId = this.customerManager.CurrentUser.CustomerId;
        }

        return this.customerId;
      }
      
      set
      {
        this.customerId = value;
      }
    }

    #endregion

    #region Get orders

    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <returns>
    /// The order.
    /// </returns>
    [CanBeNull]
    public virtual OldOrder GetOrder([NotNull] string orderNumber)
    {
      Assert.ArgumentNotNullOrEmpty(orderNumber, "orderNumber");

      this.InitializeInnerRepository();

      var order = this.innerRepository.GetAll().FirstOrDefault(o => o.OrderId == orderNumber);
      Assert.IsNotNull(order, "order cannot be null.");

      if (order.BuyerCustomerParty.SupplierAssignedAccountID == this.CustomerId)
      {
        return this.Convert(order);
      }

      throw new SecurityException(Texts.YouDoNotHaveTheNecessaryPermissionsToEditThisOrderItWasCreatedByAnotherCustomer);
    }

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <returns>
    /// The orders.
    /// </returns>
    [NotNull]
    public virtual IEnumerable<OldOrder> GetOrders<TQuery>(TQuery query)
    {
      return this.GetAllOrdersByUser().Select(this.Convert);
    }

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns>Collection of the orders.</returns>
    [NotNull]
    public virtual IEnumerable<OldOrder> GetOrders<TQuery>(TQuery query, int pageIndex, int pageSize)
    {
      return this.GetOrders(query).Skip(pageIndex * pageSize).Take(pageSize);
    }

    /// <summary>
    /// Gets the orders count.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <returns>Returns selected orders count.</returns>
    public virtual int GetOrdersCount<TQuery>(TQuery query)
    {
      return this.GetAllOrdersByUser().Count();
    }

    #endregion

    #region Other methods from the IOrderManager

    /// <summary>
    /// Creates the order.
    /// </summary>
    /// <typeparam name="TShoppingCart">The type of the shopping cart.</typeparam>
    /// <param name="shoppingCart">The shopping cart.</param>
    /// <returns>
    /// The order.
    /// </returns>
    [NotNull]
    public virtual OldOrder CreateOrder<TShoppingCart>([NotNull] TShoppingCart shoppingCart) where TShoppingCart : ShoppingCart
    {
      Assert.ArgumentNotNull(shoppingCart, "shoppingCart");

      Assert.IsNotNull(this.transactionData, "TransactionData must be set.");
      Assert.IsNotNull(this.shopContext, "Unable to create the order. Shop context is not set.");

      this.InitializeInnerRepository();

      OldOrder order = Context.Entity.Resolve<OldOrder>();
      this.Map(shoppingCart, order);

      string transactionNumber = TypeUtil.TryParse(this.transactionData.GetPersistentValue(shoppingCart.OrderNumber, TransactionConstants.TransactionNumber), string.Empty);
      if (!string.IsNullOrEmpty(transactionNumber))
      {
        order.TransactionNumber = transactionNumber;
      }

      order.OrderDate = DateTime.Now;
      order.Status = Context.Entity.Resolve<NewOrder>();
      order.ProcessStatus();

      string cardType = this.transactionData.GetPersistentValue(order.OrderNumber, TransactionConstants.CardType) as string;
      order.CustomerInfo.CustomProperties[TransactionConstants.CardType] = cardType;

      Ecommerce.OrderManagement.Orders.Order newOrder = this.Convert(order);

      ReservationTicket sourceTicket = this.transactionData.GetPersistentValue(order.OrderNumber, "ReservationTicket") as ReservationTicket;
      if (sourceTicket != null)
      {
        Ecommerce.OrderManagement.Orders.ReservationTicket destinationTicket = new Ecommerce.OrderManagement.Orders.ReservationTicket();
        this.Map(sourceTicket, destinationTicket);
        newOrder.ReservationTicket = destinationTicket;
      }

      newOrder.ShopContext = this.shopContext.InnerSite.Name;

      this.innerRepository.Create(newOrder);

      return order;
    }

    /// <summary>
    /// Generates the order number.
    /// </summary>
    /// <returns>The order number.</returns>
    [NotNull]
    public virtual string GenerateOrderNumber()
    {
      return this.orderIDGenerator.Generate();
    }

    /// <summary>
    /// Saves the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
    public virtual void SaveOrder([NotNull] OldOrder order)
    {
      Assert.ArgumentNotNull(order, "order");
      throw new NotImplementedException();
    }

    #endregion

    #region Get orders - inner logic

    /// <summary>
    /// Gets all new orders.
    /// </summary>
    /// <returns>Collection of new orders.</returns>
    [NotNull]
    protected IEnumerable<Ecommerce.OrderManagement.Orders.Order> GetAllOrders()
    {
      this.InitializeInnerRepository();

      return this.innerRepository.GetAll();
    }

    /// <summary>
    /// Gets all new orders.
    /// </summary>
    /// <returns>Collection of new orders.</returns>
    [NotNull]
    protected IEnumerable<Ecommerce.OrderManagement.Orders.Order> GetAllOrdersByUser()
    {
      return this.GetAllOrders().Where(o => o.BuyerCustomerParty.SupplierAssignedAccountID == this.CustomerId);
    }

    #endregion

    #region Mappings

    /// <summary>
    /// Maps the specified source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    protected virtual void Map([NotNull] ShoppingCart source, [NotNull] OldOrder destination)
    {
      Assert.ArgumentNotNull(source, "source");
      Assert.ArgumentNotNull(destination, "destination");

      Assert.IsNotNull(this.entityHelper, "EntityHelper must be set.");

      this.entityHelper.CopyPropertiesValues(source, ref destination);
      foreach (ShoppingCartLine line in source.ShoppingCartLines)
      {
        DomainModel.Orders.OrderLine orderLine = Context.Entity.Resolve<DomainModel.Orders.OrderLine>();
        this.Map(line, orderLine);
        destination.OrderLines.Add(orderLine);
      }
    }

    /// <summary>
    /// Maps the specified source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    protected virtual void Map([NotNull] ShoppingCartLine source, [NotNull] DomainModel.Orders.OrderLine destination)
    {
      Assert.ArgumentNotNull(source, "source");
      Assert.ArgumentNotNull(destination, "destination");

      destination.Product = source.Product;
      destination.Totals = source.Totals;
      destination.Quantity = source.Quantity;
      destination.FriendlyUrl = source.FriendlyUrl;
      destination.ImageUrl = source.ImageUrl;
    }

    /// <summary>
    /// Maps the specified source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    protected virtual void Map([NotNull] DomainModel.Payments.ReservationTicket source, [NotNull] Ecommerce.OrderManagement.Orders.ReservationTicket destination)
    {
      Assert.ArgumentNotNull(source, "source");
      Assert.ArgumentNotNull(destination, "destination");

      destination.Amount = source.Amount;
      destination.AuthorizationCode = source.AuthorizationCode;
      destination.InvoiceNumber = source.InvoiceNumber;
      destination.TransactionNumber = source.TransactionNumber;
    }

    /// <summary>
    /// Maps the specified source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>
    /// The order.
    /// </returns>
    [NotNull]
    protected virtual Ecommerce.OrderManagement.Orders.Order Convert([NotNull] OldOrder source)
    {
      Assert.ArgumentNotNull(source, "source");

      Assert.IsNotNull(this.orderConverter, "OrderConverter must be set.");

      Ecommerce.OrderManagement.Orders.Order destination = this.orderConverter.Convert(source);

      return destination;
    }

    /// <summary>
    /// Converts the order in new format to order in old format.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>
    /// The old order.
    /// </returns>
    [NotNull]
    protected virtual OldOrder Convert([NotNull] Ecommerce.OrderManagement.Orders.Order source)
    {
      Assert.ArgumentNotNull(source, "source");
      Assert.IsNotNull(this.orderConverter, "OrderConverter must be set.");

      OldOrder destination = this.orderConverter.Convert(source);

      return destination;
    }

    #endregion

    #region Resolvings

    /// <summary>
    /// Initializes the inner repository.
    /// </summary>
    protected virtual void InitializeInnerRepository()
    {
      Assert.IsNotNull(this.innerRepository, "Unable to get orders. InnerRepository must be set.");
      if (this.innerRepository is IUserAware)
      {
        ((IUserAware)this.innerRepository).CustomerId = this.CustomerId;
      }
    }

    #endregion
  }
}