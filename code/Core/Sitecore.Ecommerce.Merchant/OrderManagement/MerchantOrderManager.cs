// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MerchantOrderManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the merchant order manager class.
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

namespace Sitecore.Ecommerce.Merchant.OrderManagement
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using Data;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the merchant order manager class.
  /// </summary>
  public class MerchantOrderManager
  {
    /// <summary>
    /// The core order manager.
    /// </summary>
    private Repository<Order> orderRepository;

    /// <summary>
    /// The old entity.
    /// </summary>
    private Order oldEntity;

    /// <summary>
    /// Occurs when order is about to be saved.
    /// </summary>
    public event EventHandler<SaveEntityEventArgs<Order>> OrderSaving;

    /// <summary>
    /// Occurs when order is saved.
    /// </summary>
    public event EventHandler<SaveEntityEventArgs<Order>> OrderSaved;

    /// <summary>
    /// Gets or sets the core order manager.
    /// </summary>
    /// <value>
    /// The core order manager.
    /// </value>
    [CanBeNull]
    public virtual Repository<Order> CoreOrderManager
    {
      get
      {
        if (this.orderRepository == null && Context.Entity != null)
        {
          this.orderRepository = Context.Entity.Resolve<Repository<Order>>();
        }

        return this.orderRepository;
      }

      set
      {
        this.orderRepository = value;
      }
    }

    /// <summary>
    /// Gets or sets the state configuration.
    /// </summary>
    /// <value>The state configuration.</value>
    [NotNull]
    public virtual CoreOrderStateConfiguration StateConfiguration { get; set; }

    /// <summary>
    /// Gets or sets the calculation strategy.
    /// </summary>
    /// <value>
    /// The calculation strategy.
    /// </value>
    [NotNull]
    public virtual IOrderCalculationStrategy CalculationStrategy { get; set; }

    /// <summary>
    /// Gets or sets the order processor.
    /// </summary>
    /// <value>
    /// The order processor.
    /// </value>
    [CanBeNull]
    public virtual MerchantOrderProcessor OrderProcessor { get; set; }

    /// <summary>
    /// Gets or sets the strategy resolver.
    /// </summary>
    /// <value>
    /// The strategy resolver.
    /// </value>
    [CanBeNull]
    public virtual OrderProcessingStrategyResolver StrategyResolver { get; set; }

    /// <summary>
    /// Gets or sets the shop context factory.
    /// </summary>
    /// <value>
    /// The shop context factory.
    /// </value>
    public virtual ShopContext ShopContext { get; set; }

    /// <summary>
    /// Gets the core order manager not null.
    /// </summary>
    [NotNull]
    private Repository<Order> CoreOrderManagerNotNull
    {
      get
      {
        Assert.IsNotNull(this.CoreOrderManager, "CoreOrderManager cannot be null.");

        return this.CoreOrderManager;
      }
    }

    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <returns>The order.</returns>
    [CanBeNull]
    public virtual Order GetOrder([NotNull] string orderId)
    {
      Assert.ArgumentNotNull(orderId, "orderId");

      var result = this.CoreOrderManagerNotNull.GetAll().SingleOrDefault(o => o.OrderId == orderId);

      if (result != null && this.ShopContext != null && (result.ShopContext != this.ShopContext.InnerSite.Name))
      {
        result = null;
      }

      return result;
    }

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <returns>The orders.</returns>
    [NotNull]
    public virtual IQueryable<Order> GetOrders()
    {
      var result = this.CoreOrderManagerNotNull.GetAll();

      if (this.ShopContext != null)
      {
        var shopContextName = this.ShopContext.InnerSite.Name;
        if (!string.IsNullOrEmpty(shopContextName))
        {
          return result.Where(o => o.ShopContext == shopContextName);
        }
      }

      return Enumerable.Empty<Order>().AsQueryable();
    }

    /// <summary>
    /// Saves the order.
    /// </summary>
    /// <param name="order">The order.</param>
    public virtual void Save([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      this.CoreOrderManagerNotNull.EntitySaving += this.OnOrderSaving;
      this.CoreOrderManagerNotNull.EntitySaved += this.OnOrderSaved;
      try
      {
        this.PerformOrderSaving(order);
      }
      finally
      {
        this.CoreOrderManagerNotNull.EntitySaving -= this.OnOrderSaving;
        this.CoreOrderManagerNotNull.EntitySaved -= this.OnOrderSaved;
      }
    }

    /// <summary>
    /// Deletes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    public virtual void DeleteOrder([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      this.CoreOrderManagerNotNull.Delete(new[] { order });
    }

    /// <summary>
    /// Deletes the orders.
    /// </summary>
    /// <param name="expression">The expression.</param>
    public virtual void DeleteOrders([NotNull] Expression<Func<Order, bool>> expression)
    {
      Assert.ArgumentNotNull(expression, "expression");

      IQueryable<Order> orders = this.orderRepository.GetAll().Where(expression);
      this.CoreOrderManagerNotNull.Delete(orders);
    }

    /// <summary>
    /// Performs the order saving.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <exception cref="InvalidStateConfigurationException">Throws InvalidStateConfigurationException if order state is invalid.</exception>
    protected virtual void PerformOrderSaving([NotNull] Order order)
    {
      Debug.ArgumentNotNull(order, "order");

      if (!this.StateConfiguration.IsValid(order.State))
      {
        throw new InvalidStateConfigurationException();
      }

      this.CalculationStrategy.ApplyCalculations(order);

      this.CoreOrderManagerNotNull.Save(new[] { order });
    }

    /// <summary>
    /// Called when the order has saving.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see><cref>EntitySaveEventArgs{T}.Ecommerce.OrderManagement.Orders.Order&amp;gt;</cref></see> instance containing the event data.</param>
    protected virtual void OnOrderSaving([NotNull] object sender, [NotNull] SaveEntityEventArgs<Order> e)
    {
      Debug.ArgumentNotNull(sender, "sender");
      Debug.ArgumentNotNull(e, "e");

      if (this.StrategyResolver == null && Context.Entity != null)
      {
        this.StrategyResolver = Context.Entity.Resolve<OrderProcessingStrategyResolver>();
      }

      Assert.IsNotNull(this.StrategyResolver, "Unable to save the order. Strategy Resolver cannot be null.");

      IEnumerable<OrderProcessingStrategy> strategies = this.StrategyResolver.GetStrategies(e.OldEntity, e.NewEntity);
      Dictionary<string, object> parameters = new Dictionary<string, object> { { "oldOrder", e.OldEntity } };

      Assert.IsNotNull(this.OrderProcessor, "Unable to save the order. Order Processor cannot be null.");
      foreach (OrderProcessingStrategy strategy in strategies)
      {
        this.OrderProcessor.OrderProcessingStrategy = strategy;
        this.OrderProcessor.ProcessOrder(e.NewEntity, parameters);
      }

      this.oldEntity = e.OldEntity;

      var orderSaving = this.OrderSaving;
      if (orderSaving != null)
      {
        orderSaving(this, e);
      }
    }

    /// <summary>
    /// Called when the order has saved.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see><cref>Sitecore.Ecommerce.Data.EntityEventArgs&amp;lt;Sitecore.Ecommerce.OrderManagement.Orders.Order&amp;gt;</cref></see> instance containing the event data.</param>
    protected virtual void OnOrderSaved([NotNull] object sender, [NotNull] EntityEventArgs<Order> e)
    {
      Debug.ArgumentNotNull(sender, "sender");
      Debug.ArgumentNotNull(e, "e");

      var orderSaved = this.OrderSaved;
      if (orderSaved != null)
      {
        var args = new SaveEntityEventArgs<Order>(this.oldEntity, e.Entity);
        orderSaved(this, args);
      }
    }
  }
}