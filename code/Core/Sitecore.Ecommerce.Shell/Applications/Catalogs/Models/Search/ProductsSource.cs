// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductsSource.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the products source class.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models.Search
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using DomainModel.Products;
  using Ecommerce.Search;
  using Products;
  using Sitecore.Web.UI.Grids;

  /// <summary>
  /// Defines the products source class.
  /// </summary>
  public class ProductsSource : CatalogBaseSource
  {
    /// <summary>
    /// The Products.
    /// </summary>
    private IEnumerable<ProductBaseData> products;

    /// <summary>
    /// Product repository
    /// </summary>
    private IProductRepository productRepository;

    /// <summary>
    /// Gets the product repository.
    /// </summary>
    /// <value>The product repository.</value>
    [NotNull]
    public virtual IProductRepository ProductRepository
    {
      get
      {
        if (this.productRepository != null)
        {
          return this.productRepository;
        }

        this.productRepository = Context.Entity.Resolve<IProductRepository>();
        if (this.Database != null && this.productRepository is ProductRepository)
        {
          ((ProductRepository)this.productRepository).Database = this.Database;
        }

        return this.productRepository;
      }
    }

    /// <summary>
    /// Filters the specified filters.
    /// </summary>
    /// <param name="filters">The filters.</param>
    public override void Filter([NotNull] IList<GridFilter> filters)
    {
      Assert.ArgumentNotNull(filters, "filters");
    }

    /// <summary>
    /// Gets the entry count.
    /// </summary>
    /// <returns>Returns entries count.</returns>
    public override int GetEntryCount()
    {
      return this.ProductRepository.GetCount<IProductRepositoryItem, Query>(this.GetQuery());
    }

    /// <summary>
    /// Gets the entries in content language.
    /// </summary>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns>
    /// The entries in content language.
    /// </returns>
    protected override IEnumerable<List<string>> GetContentLanguageEntries(int pageIndex, int pageSize)
    {
      IEnumerable<ProductBaseData> productsList = this.products == null ? this.ProductRepository.Get<ProductBaseData, Query>(this.GetQuery(), pageIndex, pageSize).ToList() : this.GetDataFromRepository().Skip(pageIndex * pageSize).Take(pageSize).ToList();
      return new EntityResultDataConverter<ProductBaseData>().Convert(productsList, this.SearchOptions.GridColumns).Rows;
    }

    /// <summary>
    /// Gets data ro sort.
    /// </summary>
    /// <param name="elementType">The element type.</param>
    /// <returns>
    /// query able list of data.
    /// </returns>
    [AllowNull("*")]
    protected override IQueryable GetSource(out Type elementType)
    {
      var firstElement = this.GetDataFromRepository().FirstOrDefault();
      if (firstElement == null)
      {
        elementType = null;
        return null;
      }

      elementType = firstElement.GetType();
      IQueryable tempSource = this.products.AsQueryable();
      return this.CastSource(elementType, tempSource);
    }

    /// <summary>
    /// Updates the data.
    /// </summary>
    /// <param name="source">The source.</param>
    protected override void UpdateSourceData([NotNull] IQueryable source)
    {
      Assert.ArgumentNotNull(source, "source");

      this.products = source.OfType<ProductBaseData>().AsEnumerable();
    }

    /// <summary>
    /// Gets data from repository.
    /// </summary>
    /// <returns>The enumeration of the ProductBaseData.</returns>
    [NotNull]
    protected IEnumerable<ProductBaseData> GetDataFromRepository()
    {
      return this.products ?? (this.products = this.ProductRepository.Get<ProductBaseData, Query>(this.GetQuery()));
    }
  }
}