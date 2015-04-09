// -------------------------------------------------------------------------------------------
// <copyright file="ProductUrlProcessor.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Catalogs
{
  using Search;
  using Sitecore.Data.Items;
  using Text;

  /// <summary>
  /// The product resolver abstract class.
  /// </summary>
  public abstract class ProductUrlProcessor
  {
    /// <summary>
    /// The search provider.
    /// </summary>
    private readonly ISearchProvider searchProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductUrlProcessor"/> class.
    /// </summary>
    /// <param name="searchProvider">The search provider.</param>
    public ProductUrlProcessor(ISearchProvider searchProvider)
    {
      this.searchProvider = searchProvider;
    }

    /// <summary>
    /// Gets the search provider.
    /// </summary>
    /// <value>The search provider.</value>
    protected virtual ISearchProvider SearchProvider
    {
      get
      {
        return this.searchProvider;
      }
    }

    /// <summary>
    /// Gets the product template id.
    /// </summary>
    /// <value>The product template id.</value>
    protected virtual string ProductTemplateId
    {
      get
      {
        return Configuration.Settings.GetSetting("Ecommerce.Product.BaseTemplateId");
      }
    }

    /// <summary>
    /// Resolves the product item.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The product item.</returns>
    public abstract Item ResolveProductItem(params string[] arguments);

    /// <summary>
    /// Builds the product URL.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <param name="productItem">The product item.</param>
    /// <returns>The product URL.</returns>
    public abstract UrlString GetProductUrl(Item catalogItem, Item productItem);
  }
}