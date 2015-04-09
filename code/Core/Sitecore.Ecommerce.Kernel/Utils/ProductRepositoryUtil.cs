// -------------------------------------------------------------------------------------------
// <copyright file="ProductRepositoryUtil.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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

namespace Sitecore.Ecommerce.Utils
{
  using System.Linq;
  using Data;
  using Diagnostics;
  using DomainModel.Data;
  using DomainModel.Products;
  using Search;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// The product utility.
  /// </summary>
  public static class ProductRepositoryUtil
  {
    /// <summary>
    /// The entity helper instance.
    /// </summary>
    private static readonly EntityHelper EntityHelper;

    /// <summary>
    /// Initializes static members of the <see cref="ProductRepositoryUtil"/> class.
    /// </summary>
    static ProductRepositoryUtil()
    {
      EntityHelper = Context.Entity.Resolve<EntityHelper>();
    }

    /// <summary>
    /// Gets the product item.
    /// </summary>
    /// <typeparam name="T">The repository item type.</typeparam>
    /// <param name="product">The product.</param>
    /// <param name="database">The database.</param>
    /// <returns>
    /// The product item.
    /// </returns>
    public static Item GetRepositoryItem<T>(T product, Database database = null) where T : IProductRepositoryItem
    {
      Assert.ArgumentNotNull(product, "product");

      ShopContext shop = Context.Entity.Resolve<ShopContext>();

      database = database ?? shop.Database;

      if (product is IEntity && !string.IsNullOrEmpty(((IEntity)product).Alias))
      {
        string location = ((IEntity)product).Alias;

        Assert.IsNotNull(database, "Unable to get repository item. Database cannot be null.");

        Item item = database.GetItem(location);
        if (item != null)
        {
          return item;
        }
      }

      Assert.IsNotNull(shop.BusinessCatalogSettings, "Business Catalog settings not found.");

      if (string.IsNullOrEmpty(shop.BusinessCatalogSettings.ProductsLink))
      {
        return default(Item);
      }

      Item productsRoot = database.GetItem(shop.BusinessCatalogSettings.ProductsLink);
      Assert.IsNotNull(productsRoot, "Product repository root item is null");

      string key = EntityHelper.GetField<T>(i => i.Code);
      if (string.IsNullOrEmpty(key))
      {
        Log.Warn(string.Concat("Field name is undefined. Type: ", typeof(T).ToString(), ". Property: 'Code'."), null);

        key = "Code";
      }

      Query query = new Query { SearchRoot = productsRoot.ID.ToString() };
      query.AppendField(key, product.Code, MatchVariant.Exactly);

      ISearchProvider searchProvider = Context.Entity.Resolve<ISearchProvider>();
      return searchProvider.Search(query).FirstOrDefault();
    }

    /// <summary>
    /// Determines whether [is product item] [the specified item].
    /// </summary>
    /// <param name="item">The product item.</param>
    /// <param name="templateId">The template id.</param>
    /// <returns>
    /// <c>true</c> if [is product item] [the specified item]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBasedOnTemplate(TemplateItem item, ID templateId)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(templateId, "templateId");

      if (item.ID.Equals(templateId))
      {
        return true;
      }

      if (item.BaseTemplates.IsNullOrEmpty())
      {
        return false;
      }

      if (item.BaseTemplates.Any(bt => bt.ID.Equals(templateId)))
      {
        return true;
      }

      if (item.BaseTemplates.Any(bt => bt.ID.Equals(item.ID)))
      {
        return false;
      }

      foreach (TemplateItem templateItem in item.BaseTemplates)
      {
        if (IsBasedOnTemplate(templateItem, templateId))
        {
          return true;
        }
      }

      return false;
    }
  }
}