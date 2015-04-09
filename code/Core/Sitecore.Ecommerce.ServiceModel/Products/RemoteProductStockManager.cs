// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoteProductStockManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The remote product stock manager. Provides product stock management through WCF service.
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

namespace Sitecore.Ecommerce.ServiceModel.Products
{
  using System;
  using System.Linq.Expressions;
  using Diagnostics;
  using DomainModel.Products;
  using ProductStockService;
  using Services;

  /// <summary>
  /// The remote product stock manager. Provides product stock management through WCF service.
  /// </summary>
  public class RemoteProductStockManager : IProductStockManager
  {
    /// <summary>
    /// Defines ServiceClientArgs factory.
    /// </summary>
    private readonly ServiceClientArgsFactory serviceClientArgsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteProductStockManager"/> class.
    /// </summary>
    /// <param name="serviceClientArgsFactory">The service client args factory.</param>
    public RemoteProductStockManager(ServiceClientArgsFactory serviceClientArgsFactory)
    {
      Assert.ArgumentNotNull(serviceClientArgsFactory, "serviceClientArgsFactory");
      this.serviceClientArgsFactory = serviceClientArgsFactory;
    }

    /// <summary>
    /// Gets the product stock.
    /// </summary>
    /// <param name="stockInfo">The product stock info. Contains info required to get product stock value.
    /// By default it's product code.</param>
    /// <returns>The product stock.</returns>
    public ProductStock GetStock(ProductStockInfo stockInfo)
    {
      ServiceClientArgs args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (ProductStockServiceClient client = new ProductStockServiceClient())
      {
        ProductStock stock = client.Get(stockInfo, args);

        return stock;
      }
    }

    /// <summary>
    /// Updates the product stock.
    /// </summary>
    /// <param name="stockInfo">The stock info.</param>
    /// <param name="newAmount">The new amount.</param>
    public void Update(ProductStockInfo stockInfo, long newAmount)
    {
      ServiceClientArgs args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (ProductStockServiceClient client = new ProductStockServiceClient())
      {
        client.Update(stockInfo.ProductCode, newAmount, args);
      }
    }

    /// <summary>
    /// Updates the specified stock info.
    /// </summary>
    /// <param name="stockInfo">The stock info.</param>
    /// <param name="expression">The expression.</param>
    public void Update(ProductStockInfo stockInfo, Expression<Func<long, long>> expression)
    {
      Func<long, long> func = expression.Compile();
      ServiceClientArgs args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (ProductStockServiceClient client = new ProductStockServiceClient())
      {
        ProductStock stock = client.Get(stockInfo, args);
        long newStock = func(stock.Stock);

        client.Update(stockInfo.ProductCode, newStock, args);
      }
    }
  }
}