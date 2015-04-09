// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductStockManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Represents the simple product stock manager. Determines the product stock value by product info.
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

namespace Sitecore.Ecommerce.Products
{
  using System;
  using System.Linq.Expressions;
  using System.Threading;
  using DomainModel.Products;
  using Sitecore.Configuration;

  /// <summary>
  /// Represents the product stock manager. Determines the product stock value by product info.
  /// </summary>
  public class ProductStockManager : IProductStockManager
  {
    /// <summary>
    /// The maximal number of concurrent stock requests.
    /// </summary>
    private const string MaxNumberOfConcurrentStockRequests = "Ecommerce.Stock.MaxConcurrentRequests";

    /// <summary>
    /// The collection of reader writer locks.
    /// </summary>
    private static readonly ReaderWriterLockSlim[] Locks;

    /// <summary>
    /// Gets or sets the product repository.
    /// </summary>
    /// <value>The product repository.</value>
    private readonly IProductRepository productRepository;

    /// <summary>
    /// Initializes the <see cref="ProductStockManager"/> class.
    /// </summary>
    static ProductStockManager()
    {
      Locks = new ReaderWriterLockSlim[Settings.GetIntSetting(MaxNumberOfConcurrentStockRequests, Environment.ProcessorCount)];

      for (int index = 0; index < Locks.Length; ++index)
      {
        Locks[index] = new ReaderWriterLockSlim();
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductStockManager" /> class.
    /// </summary>
    /// <param name="productRepository">The product repository.</param>
    public ProductStockManager(IProductRepository productRepository)
    {
      if (productRepository is ProductRepository)
      {
        var tempRepository = productRepository as ProductRepository;
        if (tempRepository.ShopContext != null)
        {
          tempRepository.Database = tempRepository.ShopContext.InnerSite.ContentDatabase;
        }
      }

      this.productRepository = productRepository;
    }

    /// <summary>
    /// Gets the product stock.
    /// </summary>
    /// <param name="stockInfo">The product stock info. Contains info required to get product stock value.
    /// By default it's product code.</param>
    /// <returns>The product stock.</returns>
    public virtual DomainModel.Products.ProductStock GetStock(ProductStockInfo stockInfo)
    {
      ReaderWriterLockSlim readerWriterLock = GetLock(stockInfo);

      readerWriterLock.EnterReadLock();
      try
      {
        DomainModel.Products.ProductStock stock = this.productRepository.Get<DomainModel.Products.ProductStock>(stockInfo.ProductCode);
        return stock;
      }
      finally
      {
        readerWriterLock.ExitReadLock();
      }
    }

    /// <summary>
    /// Updates the product stock.
    /// </summary>
    /// <param name="stockInfo">The stock info.</param>
    /// <param name="newAmount">The new amount.</param>
    public virtual void Update(ProductStockInfo stockInfo, long newAmount)
    {
      this.Update(stockInfo, a => newAmount);
    }

    /// <summary>
    /// Updates the specified stock info.
    /// </summary>
    /// <param name="stockInfo">The stock info.</param>
    /// <param name="expression">The expression.</param>
    public void Update(ProductStockInfo stockInfo, Expression<Func<long, long>> expression)
    {
      ReaderWriterLockSlim readerWriterLock = GetLock(stockInfo);

      readerWriterLock.EnterWriteLock();
      try
      {
        DomainModel.Products.ProductStock stock = this.productRepository.Get<DomainModel.Products.ProductStock>(stockInfo.ProductCode);
        Func<long, long> func = expression.Compile();
        stock.Stock = func(stock.Stock);

        this.productRepository.Update(stock);
      }
      finally
      {
        readerWriterLock.ExitWriteLock();
      }
    }

    /// <summary>
    /// Gets the lock.
    /// </summary>
    /// <param name="stockInfo">The product stock info.</param>
    /// <returns>
    /// The lock.
    /// </returns>
    private static ReaderWriterLockSlim GetLock(ProductStockInfo stockInfo)
    {
      return Locks[(stockInfo.ProductCode.GetHashCode() & int.MaxValue) % Locks.Length];
    }
  }
}