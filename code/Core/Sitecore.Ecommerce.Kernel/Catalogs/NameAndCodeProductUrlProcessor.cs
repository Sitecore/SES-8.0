// -------------------------------------------------------------------------------------------
// <copyright file="NameAndCodeProductUrlProcessor.cs" company="Sitecore Corporation">
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
  using System.Web;
  using Diagnostics;
  using DomainModel.Configurations;
  using Links;
  using Search;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Text;
  using Utils;

  /// <summary>
  /// The name product resolver.
  /// </summary>
  public class NameAndCodeProductUrlProcessor : ProductUrlProcessor
  {
    #region Overrides of ProductUrlBuilder

    /// <summary>
    /// Initializes a new instance of the <see cref="NameAndCodeProductUrlProcessor"/> class.
    /// </summary>
    /// <param name="searchProvider">The search provider.</param>
    public NameAndCodeProductUrlProcessor(ISearchProvider searchProvider) : base(searchProvider)
    {
    }

    /// <summary>
    /// Resolves the product item.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The product item.</returns>
    public override Item ResolveProductItem(params string[] arguments)
    {
      Assert.ArgumentNotNull(arguments, "arguments");

      string[] nameAndCode = arguments[0].Split('_');
      string name = nameAndCode[0];
      string code = nameAndCode[1];

      Assert.IsNotNullOrEmpty(name, "name");
      Assert.IsNotNullOrEmpty(code, "code");

      Item productFolderItem = Sitecore.Context.Database.GetItem(Context.Entity.GetConfiguration<BusinessCatalogSettings>().ProductsLink);
      Assert.IsNotNull(productFolderItem, "Products root item cannot be null.");

      Query query = new Query { SearchRoot = productFolderItem.ID.ToString() };
      query.AppendField("Product Code", code, MatchVariant.Exactly);
      query.AppendCondition(QueryCondition.And);
      query.AppendAttribute("key", name, MatchVariant.Exactly);

      foreach (Item item in this.SearchProvider.Search(query, Sitecore.Context.Database))
      {
        if (ProductRepositoryUtil.IsBasedOnTemplate(item.Template, new ID(this.ProductTemplateId)))
        {
          return item;
        }
      }

      return default(Item);
    }

    /// <summary>
    /// Builds the product URL.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <param name="productItem">The product item.</param>
    /// <returns>The product URL.</returns>
    public override UrlString GetProductUrl(Item catalogItem, Item productItem)
    {
      Assert.ArgumentNotNull(catalogItem, "catalogItem");
      Assert.ArgumentNotNull(productItem, "productItem");

      UrlOptions urlOptions = new UrlOptions
                                {
                                  SiteResolving = true,
                                  ShortenUrls = false,
                                  UseDisplayName = false,
                                  AlwaysIncludeServerUrl = false,
                                  AddAspxExtension = false,
                                  EncodeNames = false,
                                  LanguageEmbedding = LanguageEmbedding.Always,
                                };
      UrlString url = new UrlString(LinkManager.GetItemUrl(catalogItem, urlOptions));
      
      string productUrl = IO.FileUtil.MakePath(url.Path, string.Concat(HttpUtility.UrlPathEncode(productItem.Name), "_", HttpUtility.UrlPathEncode(productItem["Product Code"])), '/');
      url.Path = string.Concat(productUrl, ".aspx");
      
      return url;
    }

    #endregion
  }
}