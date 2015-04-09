// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfOrderRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order provider class.
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

namespace Sitecore.Ecommerce.Data.OrderManagement
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.Entity;
  using System.Data.Entity.Validation;
  using System.Linq;
  using System.Transactions;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the order provider class.
  /// </summary>
  public class EfOrderRepository : Repository<Order>, IDisposable
  {
    /// <summary>
    /// The database context.
    /// </summary>
    private IOrdersContext databaseContext;

    /// <summary>
    /// Is set to true when instance is disposed.
    /// </summary>
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="EfOrderRepository" /> class.
    /// </summary>
    /// <param name="shopContext">The shop context.</param>
    public EfOrderRepository(ShopContext shopContext)
    {
      Assert.ArgumentNotNull(shopContext, "shopContext");
      Assert.IsNotNullOrEmpty(shopContext.OrdersDatabaseName, "shopContext.OrdersDatabaseName");

      this.databaseContext = new OrderModel(shopContext.OrdersDatabaseName);
    }

    /// <summary>
    /// Gets or sets the database context.
    /// </summary>
    /// <value>
    /// The database context.
    /// </value>
    [NotNull]
    public IOrdersContext DatabaseContext
    {
      get
      {
        return this.databaseContext;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.databaseContext = value;
      }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (this.isDisposed)
      {
        return;
      }

      if (disposing)
      {
        // Managed stuff should be handled here
      }

      this.isDisposed = true;
    }

    /// <summary>
    /// Gets the specified expression.
    /// </summary>
    /// <returns>
    /// The I enumerable.
    /// </returns>
    protected override IQueryable<Order> GetAll()
    {
      return this.DatabaseContext.Orders.OrderByDescending(o => o.IssueDate);
    }

    /// <summary>
    /// Gets the persisted entity.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>The persisted entity.</returns>
    protected override Order GetPersistedEntity(Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      if (string.IsNullOrEmpty(order.OrderId))
      {
        return null;
      }

      return this.GetAll().Where(o => o.OrderId == order.OrderId).AsNoTracking().SingleOrDefault();
    }

    /// <summary>
    /// Saves the specified orders.
    /// </summary>
    /// <param name="orders">The orders.</param>
    /// <exception cref="ProviderException"><c>ProviderException</c>.</exception>
    /// <exception cref="SaveOrdersException"><c>OrdersSaveException</c>.</exception>
    protected override void PerformSaving(IEnumerable<Order> orders)
    {
      Debug.ArgumentNotNull(orders, "orders");

      var orderList = orders.ToList();

      foreach (Order order in orderList)
      {
        EntityState entityState = order.ID == Guid.Empty ? EntityState.Added : EntityState.Modified;
        if (order.ID == Guid.Empty)
        {
          order.ID = Guid.NewGuid();
        }

        this.DatabaseContext.Entry(order, entityState);
      }

      try
      {
        this.DatabaseContext.SaveChanges();
      }
      catch (Exception e)
      {
        foreach (Order order in orderList)
        {
          this.DatabaseContext.Entry(order, EntityState.Unchanged);
        }

        if (e is DbEntityValidationException)
        {
          throw new ProviderException((DbEntityValidationException)e);
        }

        throw new SaveOrdersException(e);
      }
    }

    /// <summary>
    /// Deletes the specified entry.
    /// </summary>
    /// <param name="orders">The orders.</param>
    protected override void Delete(IEnumerable<Order> orders)
    {
      Debug.ArgumentNotNull(orders, "orders");

      using (TransactionScope scope = new TransactionScope())
      {
        IDbSet<Order> set = this.DatabaseContext.Orders;

        foreach (Order entity in orders)
        {
          if (!this.DatabaseContext.IsAttached(entity))
          {
            set.Attach(entity);
          }

          this.DatabaseContext.CascadeDelete(entity);
        }

        this.DatabaseContext.SaveChanges();

        scope.Complete();
      }
    }
  }
}