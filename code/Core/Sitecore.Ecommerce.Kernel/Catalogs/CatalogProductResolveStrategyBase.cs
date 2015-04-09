// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatalogProductResolveStrategyBase.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The Catalog Product Resolve Strategy Base.
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

namespace Sitecore.Ecommerce.Catalogs
{
  using System.Collections.Generic;
  using DomainModel.Catalogs;
  using DomainModel.Products;
  using Microsoft.Practices.Unity;
  using Sitecore.Data.Items;

  /// <summary>
  /// The Catalog Product Resolve Strategy Base.
  /// </summary>
  public abstract class CatalogProductResolveStrategyBase : ICatalogProductResolveStrategy
  {
    /// <summary>
    /// Gets or sets the product repository.
    /// </summary>
    /// <value>
    /// The product repository.
    /// </value>
    [NotNull]
    [Dependency]
    public IProductRepository ProductRepository { get; set; }

    #region Implementation of ICatalogProductResolveStrategy

    /// <summary>
    /// Gets products collection from the product catalog.
    /// </summary>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <typeparam name="T">The arguments type.</typeparam>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The products collection.</returns>
    public abstract IEnumerable<TProduct> GetCatalogProducts<TProduct, T>(T arguments) where TProduct : IProductRepositoryItem;

    #endregion

    /// <summary>
    /// Gets the catalog product items.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <returns>The catalog product items.</returns>
    public abstract IEnumerable<Item> GetCatalogProductItems(Item catalogItem);
  }
}