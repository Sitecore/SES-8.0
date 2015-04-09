// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductResolver.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The product resolver processor uses <see cref="Sitecore.Ecommerce.Catalogs.VirtualProductResolver"/> to determine the product to display by page URL.
//   When the product is found the processor set the product item to the context item and rewrite context page filepath to show the virtual product presentation.
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

namespace Sitecore.Ecommerce.Pipelines.HttpRequest
{
  using System;
  using Catalogs;
  using Diagnostics;
  using Globalization;
  using IO;
  using Microsoft.Practices.Unity;
  using SecurityModel;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Ecommerce.Unity;
  using Sitecore.Pipelines.HttpRequest;
  using Sites;
  using Text;

  /// <summary>
  /// The product resolver processor uses <see cref="Sitecore.Ecommerce.Catalogs.VirtualProductResolver"/> to determine the product to display by page URL.
  /// When the product is found the processor set the product item to the context item and rewrite context page filepath to show the virtual product presentation.
  /// </summary>
  public class ProductResolver : HttpRequestProcessor
  {
    #region Overrides of HttpRequestProcessor

    /// <summary>
    /// Processes the specified args.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public override void Process(HttpRequestArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (Sitecore.Context.Item != null || Sitecore.Context.Database == null || Sitecore.Context.Database.Name == Constants.CoreDatabaseName || string.IsNullOrEmpty(args.Url.ItemPath) || args.Context.Request == null)
      {
        return;
      }

      VirtualProductResolver virtualProductResolver = Context.Entity.Resolve<VirtualProductResolver>();

      virtualProductResolver.ProductPresentationItem = default(Item);
      virtualProductResolver.ProductCatalogItem = default(Item);

      UrlString url = new UrlString
      {
        HostName = args.Context.Request.Url.Host,
        Path = args.Context.Request.RawUrl.Split(new[] { '?' })[0]
      };

      Item productItem;
      Item presentationItem;
      Item productCatalogItem = null;
      using (new SecurityDisabler())
      {
        productItem = virtualProductResolver.GetProductItem(url.ToString());
        presentationItem = virtualProductResolver.ProductPresentationItem;

        if (productItem == null || presentationItem == null)
        {
          productCatalogItem = this.GetParentItemFromItemPath(args.Url.ItemPath);

          if (productCatalogItem == null)
          {
            return;
          }

          virtualProductResolver.ProductCatalogItem = productCatalogItem;

          // Get layout from the product presentation item.
          presentationItem = virtualProductResolver.GetProductPresentationItem(productCatalogItem);
          if (presentationItem == null)
          {
            return;
          }

          virtualProductResolver.ProductPresentationItem = presentationItem;

          // Resolve product item
          string key = virtualProductResolver.GetDisplayProductsModeKey(productCatalogItem);

          var message = string.Format("ProductUrlProcessor is not defined with the name: {0}. Check Unity.config file.", key);
          Assert.IsTrue(Context.Entity.HasRegistration(typeof(ProductUrlProcessor), key), message);

          ProductUrlProcessor productUrlProcessor = Context.Entity.Resolve<ProductUrlProcessor>(key);

          string nameValue = args.Url.ItemPath.Substring(args.Url.ItemPath.LastIndexOf("/") + 1);
          productItem = productUrlProcessor.ResolveProductItem(nameValue);

          if (productItem == null)
          {
            return;
          }
        }
      }

      if (productCatalogItem == null)
      {
        using (new SecurityDisabler())
        {
          productCatalogItem = virtualProductResolver.ProductCatalogItem;
        }
      }

      productCatalogItem = args.ApplySecurity(productCatalogItem);
      productItem = args.ApplySecurity(productItem);

      if (productCatalogItem == null || productItem == null)
      {
        return;
      }

      Sitecore.Context.Page.FilePath = presentationItem.Visualization.Layout.FilePath;
      Sitecore.Context.Item = productItem;
    }

    #endregion

    /// <summary>
    /// Gets the parent item from item path.
    /// </summary>
    /// <param name="path">The item path.</param>
    /// <returns>The the parent item from item path.</returns>
    protected virtual Item GetParentItemFromItemPath(string path)
    {
      int indexOfSlash = path.LastIndexOf("/");
      path = path.Substring(0, indexOfSlash);

      Item item = Sitecore.Context.Database.GetItem(path);
      if (item == null)
      {
        string path1 = MainUtil.DecodeName(path);
        item = Sitecore.Context.Database.GetItem(path1);
      }

      SiteContext site = Sitecore.Context.Site;
      string str2 = (site != null) ? site.RootPath : string.Empty;
      if (item == null)
      {
        string path1 = FileUtil.MakePath(str2, path, '/');
        item = Sitecore.Context.Database.GetItem(path1);
      }

      if (item == null)
      {
        string path1 = MainUtil.DecodeName(FileUtil.MakePath(str2, path, '/'));
        item = Sitecore.Context.Database.GetItem(path1);
      }

      if (item == null)
      {
        item = this.ResolveUsingDisplayName(path);
      }

      return item;
    }

    /// <summary>
    /// Resolves the display name of the using.
    /// </summary>
    /// <param name="itemPath">The item path.</param>
    /// <returns>The resolved item.</returns>
    protected virtual Item ResolveUsingDisplayName(string itemPath)
    {
      Assert.ArgumentNotNull(itemPath, "UrlItemPath");
      using (new SecurityDisabler())
      {
        if (string.IsNullOrEmpty(itemPath) || (itemPath[0] != '/'))
        {
          return null;
        }

        int index = itemPath.IndexOf('/', 1);
        if (index < 0)
        {
          return null;
        }

        Item root = ItemManager.GetItem(itemPath.Substring(0, index), Language.Current, Sitecore.Data.Version.Latest, Sitecore.Context.Database, SecurityCheck.Disable);
        if (root == null)
        {
          return null;
        }

        string path = MainUtil.DecodeName(itemPath.Substring(index));

        Item child = root;
        foreach (string itemName in path.Split(new[] { '/' }))
        {
          if (itemName.Length == 0)
          {
            continue;
          }

          child = this.GetChildFromNameOrDisplayName(child, itemName);
          if (child == null)
          {
            return null;
          }
        }

        return child;
      }
    }

    /// <summary>
    /// Gets the child with a given name or display name
    /// </summary>
    /// <param name="item">The parent item.</param>
    /// <param name="itemName">Name or display name of the item.</param>
    /// <returns>The child item.</returns>
    protected virtual Item GetChildFromNameOrDisplayName(Item item, string itemName)
    {
      foreach (Item child in item.Children)
      {
        if (child.DisplayName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
        {
          return child;
        }

        if (child.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
        {
          return child;
        }
      }

      return null;
    }
  }
}