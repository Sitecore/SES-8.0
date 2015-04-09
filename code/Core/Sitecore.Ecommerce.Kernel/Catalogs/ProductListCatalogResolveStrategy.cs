// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductListCatalogResolveStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The query catalog product resolve strategy.
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
  using Data;
  using Diagnostics;
  using Microsoft.Practices.Unity;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Exceptions;
  using Text;

  /// <summary>
  /// The query catalog product resolve strategy.
  /// </summary>
  public class ProductListCatalogResolveStrategy : CatalogProductResolveStrategyBase
  {
    /// <summary>
    /// The field name.
    /// </summary>
    private readonly string filedName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductListCatalogResolveStrategy"/> class.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    public ProductListCatalogResolveStrategy(string fieldName)
    {
      Assert.ArgumentNotNullOrEmpty(fieldName, "fieldName");
      this.filedName = fieldName;
    }

    /// <summary>
    /// Gets or sets the data mapper.
    /// </summary>
    /// <value>The data mapper.</value>
    [NotNull]
    [Dependency]
    public IDataMapper DataMapper { get; set; }

    #region Implementation of ICatalogProductResolveStrategy

    /// <summary>
    /// Gets products collection from the product catalog.
    /// </summary>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <typeparam name="T">The catalog item type.</typeparam>
    /// <param name="catalogItem">The catalog item.</param>
    /// <returns>
    /// The products collection.
    /// </returns>
    /// <exception cref="Sitecore.Exceptions.ItemNullException">Catalog item is null.</exception>
    /// <exception cref="ItemNotFoundException">Product catalog item was not found.</exception>
    /// <exception cref="ItemNullException">Catalog item is null.</exception>
    public override IEnumerable<TProduct> GetCatalogProducts<TProduct, T>(T catalogItem)
    {
      Assert.ArgumentNotNull(catalogItem, "catalogItem");

      Item catalog = catalogItem as Item;
      if (catalog == null)
      {
        throw new ItemNullException("Catalog item is null.");
      }

      foreach (Item productItem in this.GetCatalogProductItems(catalog))
      {
        TProduct product = this.DataMapper.GetEntity<TProduct>(productItem);
        yield return product;
      }
    }

    #endregion

    /// <summary>
    /// Gets the catalog product items.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <returns>The catalog product items.</returns>
    public override IEnumerable<Item> GetCatalogProductItems(Item catalogItem)
    {
      Assert.ArgumentNotNull(catalogItem, "catalogItem");

      ListString productIds = new ListString(catalogItem[this.filedName]);

      foreach (string productId in productIds)
      {
        if (string.IsNullOrEmpty(productId) || !ID.IsID(productId))
        {
          continue;
        }

        Item productItem = catalogItem.Database.GetItem(productId);
        if (productItem == null)
        {
          continue;
        }

        yield return productItem;
      }
    }
  }
}