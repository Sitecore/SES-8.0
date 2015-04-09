// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductLineModelDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the product line model data source repository class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.DataSources
{
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;
  using Diagnostics;
  using DomainModel.Products;
  using Merchant.OrderManagement;
  using Models;
  using Search;
  using Sitecore.Utils;

  /// <summary>
  /// Defines the product line model data source repository class.
  /// </summary>
  public class ProductLineModelDataSourceRepository : DataSourceRepository<ProductLineModel>
  {
    /// <summary>
    /// The product repository.
    /// </summary>
    private IProductRepository productRepository;

    /// <summary>
    /// The product stopck mananger.
    /// </summary>
    private IProductStockManager productStockManager;

    /// <summary>
    /// The merchant order manager.
    /// </summary>
    private MerchantOrderManager orderManager;

    /// <summary>
    /// The currencyCode.
    /// </summary>
    private string currencyCode;

    /// <summary>
    /// The price service.
    /// </summary>
    private ProductPriceService priceService;

    /// <summary>
    /// Gets or sets the product repository.
    /// </summary>
    /// <value>
    /// The product repository.
    /// </value>
    [NotNull]
    public IProductRepository ProductRepository
    {
      get
      {
        return this.productRepository ?? (this.productRepository = Context.Entity.Resolve<IProductRepository>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.productRepository = value;
      }
    }

    /// <summary>
    /// Gets or sets the product stock manager.
    /// </summary>
    /// <value>
    /// The product stock manager.
    /// </value>
    [NotNull]
    public IProductStockManager ProductStockManager
    {
      get
      {
        return this.productStockManager ?? (this.productStockManager = Context.Entity.Resolve<IProductStockManager>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.productStockManager = value;
      }
    }

    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>
    /// The order manager.
    /// </value>
    [NotNull]
    public MerchantOrderManager OrderManager
    {
      get
      {
        return this.orderManager ?? (this.orderManager = Context.Entity.Resolve<MerchantOrderManager>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.orderManager = value;
      }
    }

    /// <summary>
    /// Gets or sets the price service.
    /// </summary>
    /// <value>
    /// The price service.
    /// </value>
    [NotNull]
    public ProductPriceService PriceService
    {
      get
      {
        return this.priceService ?? (this.priceService = new ProductPriceService());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.priceService = value;
      }
    }

    /// <summary>
    /// Selects the specified raw query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>
    /// The I enumerable.
    /// </returns>
    [NotNull]
    public override IEnumerable<ProductLineModel> SelectEntities([CanBeNull] string rawQuery)
    {
      if ((rawQuery != null) && (rawQuery.IndexOf('[') >= 0) && (rawQuery.LastIndexOf(']') >= 0))
      {
        rawQuery = rawQuery.Substring(rawQuery.IndexOf('[') + 1, rawQuery.LastIndexOf(']') - rawQuery.IndexOf('[') - 1);
      }

      Query query = !string.IsNullOrEmpty(rawQuery) ? new EcommerceQueryParser().Parse(Sitecore.Data.Query.QueryParser.ParsePredicate(rawQuery)) : new Query();

      return this.ProductRepository.Get<ProductBaseData, Query>(query).Select(this.GetProductLineModel).OrderBy(p => p.Code);
    }

    /// <summary>
    /// Selects the specified raw query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <param name="orderId">The order id.</param>
    /// <returns>
    /// The data table.
    /// </returns>
    [NotNull]
    public virtual DataTable Select([CanBeNull] string rawQuery, [NotNull] string orderId)
    {
      Assert.ArgumentNotNull(orderId, "orderId");

      var order = this.OrderManager.GetOrder(orderId);

      Assert.IsNotNull(order, "Unable to get product line. Order cannot be null.");

      this.currencyCode = order.PricingCurrencyCode;

      return this.Select(rawQuery);
    }

    /// <summary>
    /// Converts to data table.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns>
    /// The to data table.
    /// </returns>
    [NotNull]
    protected override DataTable ConvertToDataTable(IEnumerable<ProductLineModel> entities)
    {
      DataTable result = base.ConvertToDataTable(entities);

      result.Columns["Code"].ColumnName = "Product Code";

      return result;
    }

    /// <summary>
    /// Gets the product line model.
    /// </summary>
    /// <param name="productBaseData">The product base data.</param>
    /// <returns>
    /// The product line model.
    /// </returns>
    [NotNull]
    private ProductLineModel GetProductLineModel([NotNull] ProductBaseData productBaseData)
    {
      Assert.ArgumentNotNull(productBaseData, "productBaseData");

      long stock = this.ProductStockManager.GetStock(new ProductStockInfo
      {
        ProductCode = productBaseData.Code
      }).Stock;

      var price = this.PriceService.GetPrice(productBaseData, this.currencyCode);

      return new ProductLineModel
      {
        Code = productBaseData.Code,
        Name = productBaseData.Name,
        Stock = stock,
        Price = price
      };
    }
  }
}