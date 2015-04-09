// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualProductResolver.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The virtual product resolver.
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
  using System.Collections;
  using Diagnostics;
  using Links;
  using Microsoft.Practices.Unity;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Ecommerce.Unity;
  using Utils;

  /// <summary>
  /// The virtual product resolver.
  /// </summary>
  public class VirtualProductResolver
  {
    /// <summary>
    /// The product URL collection.
    /// </summary>
    private static Hashtable productsUrlsCollection = Hashtable.Synchronized(new Hashtable());

    /// <summary>
    /// The productBase temlate Id.
    /// </summary>
    private readonly ID productBaseTemplateId = new ID(Configuration.Settings.GetSetting("Ecommerce.Product.BaseTemplateId"));

    /// <summary>
    /// The parameters.
    /// </summary>
    protected VirtualProductResolverArgs parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="VirtualProductResolver"/> class.
    /// </summary>
    /// <param name="resolverArgs">The resolver args.</param>
    public VirtualProductResolver(VirtualProductResolverArgs resolverArgs)
    {
      Assert.ArgumentNotNull(resolverArgs, "resolverArgs");

      this.parameters = resolverArgs;
    }

    /// <summary>
    /// Gets or sets the product catalog item.
    /// </summary>
    /// <value>The product catalog item.</value>
    public virtual Item ProductCatalogItem
    {
      get
      {
        if (System.Web.HttpContext.Current == null)
        {
          return default(Item);
        }

        string productCatalogItemUri = System.Web.HttpContext.Current.Items["CatalogItemUri"] as string;
        return string.IsNullOrEmpty(productCatalogItemUri) ? Sitecore.Context.Item : Database.GetItem(new ItemUri(productCatalogItemUri));
      }

      set
      {
        if (System.Web.HttpContext.Current != null)
        {
          string uri = value != null ? value.Uri.ToString() : string.Empty;
          System.Web.HttpContext.Current.Items["CatalogItemUri"] = uri;
        }
      }
    }

    /// <summary>
    /// Gets or sets the prodcut presentation item.
    /// </summary>
    /// <value>The prodcut presentation item.</value> 
    public virtual Item ProductPresentationItem
    {
      get
      {
        if (System.Web.HttpContext.Current == null)
        {
          return default(Item);
        }

        string presentationItemUri = System.Web.HttpContext.Current.Items["PresentationItemUri"] as string;
        return string.IsNullOrEmpty(presentationItemUri) ? default(Item) : Database.GetItem(new ItemUri(presentationItemUri));
      }

      set
      {
        if (System.Web.HttpContext.Current != null)
        {
          string uri = value != null ? value.Uri.ToString() : string.Empty;
          System.Web.HttpContext.Current.Items["PresentationItemUri"] = uri;
        }
      }
    }

    /// <summary>
    /// Gets or sets the product URL collection.
    /// </summary>
    /// <value>The product URL collection.</value>
    public virtual Hashtable ProductsUrlsCollection
    {
      get
      {
        return productsUrlsCollection;
      }

      set
      {
        if (!value.IsSynchronized)
        {
          value = Hashtable.Synchronized(value);
        }

        productsUrlsCollection = value;
      }
    }

    /// <summary>
    /// Gets the product item by URL.
    /// </summary>
    /// <param name="url">The full URL.</param>
    /// <returns>The product item by URL.</returns>
    public virtual Item GetProductItem(string url)
    {
      Assert.ArgumentNotNullOrEmpty(url, "url");

      object value;

      lock (this.ProductsUrlsCollection.SyncRoot)
      {
        value = this.ProductsUrlsCollection[System.Web.HttpUtility.UrlDecode(url)];
      }

      if (value == null)
      {
        Log.Debug("Unable to parse product url '{0}'.".FormatWith(url), this);

        this.ProductCatalogItem = default(Item);
        this.ProductPresentationItem = default(Item);

        return default(Item);
      }

      ProductUriLine productUriLine = (ProductUriLine)value;

      Item productItem = Database.GetItem(new ItemUri(productUriLine.ProductItemUri));

      this.ProductPresentationItem = Database.GetItem(new ItemUri(productUriLine.ProductPresentationItemUri));
      this.ProductCatalogItem = Database.GetItem(new ItemUri(productUriLine.ProductCatalogItemUri));

      return productItem;
    }

    /// <summary>
    /// Gets the virtual product URL.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <param name="productItem">The product item.</param>
    /// <returns>The virtual product URL.</returns>
    public virtual string GetVirtualProductUrl(Item catalogItem, Item productItem)
    {
      Assert.ArgumentNotNull(catalogItem, "catalogItem");
      Assert.ArgumentNotNull(productItem, "productItem");

      if (catalogItem.Template != null && ProductRepositoryUtil.IsBasedOnTemplate(catalogItem.Template, this.productBaseTemplateId))
      {
        if (this.ProductCatalogItem != default(Item) && !ProductRepositoryUtil.IsBasedOnTemplate(this.ProductCatalogItem.Template, this.productBaseTemplateId))
        {
          catalogItem = this.ProductCatalogItem;
        }
        else
        {
          lock (this.ProductsUrlsCollection.SyncRoot)
          {
            foreach (DictionaryEntry dictionaryEntry in this.ProductsUrlsCollection)
            {
              var uriLine = (ProductUriLine)dictionaryEntry.Value;
              if (string.Compare(uriLine.ProductItemUri, catalogItem.Uri.ToString(), true) == 0)
              {
                catalogItem = Database.GetItem(new ItemUri(uriLine.ProductCatalogItemUri));
              }
            }
          }
        }
      }

      string key = this.GetDisplayProductsModeKey(catalogItem);

      // NOTE: Check whether items are presented in the url collection
      lock (this.ProductsUrlsCollection.SyncRoot)
      {
        foreach (DictionaryEntry entry in this.ProductsUrlsCollection)
        {
          ProductUriLine line = (ProductUriLine)entry.Value;
          if (line.ProductItemUri == productItem.Uri.ToString() && line.ProductCatalogItemUri == catalogItem.Uri.ToString() && line.DisplayProductsMode == key)
          {
            return System.Web.HttpUtility.UrlPathEncode(entry.Key as string);
          }
        }
      }

      ProductUrlProcessor productUrlBuilder;

      if (Context.Entity.HasRegistration(typeof(ProductUrlProcessor), key))
      {
        productUrlBuilder = Context.Entity.Resolve<ProductUrlProcessor>(key);
      }
      else
      {
        Log.Warn(string.Format("ProductUrlProcessor is not defined with the name: {0}. Check Unity.config file.", key), this);
        return LinkManager.GetItemUrl(productItem);
      }

      string url = productUrlBuilder.GetProductUrl(catalogItem, productItem).ToString();

      Item productPresentationItem = this.GetProductPresentationItem(catalogItem);
      Assert.IsNotNull(productPresentationItem, "Product presentation item is null");

      ProductUriLine productUriLine = new ProductUriLine
                                        {
                                          ProductItemUri = productItem.Uri.ToString(),
                                          ProductCatalogItemUri = catalogItem.Uri.ToString(),
                                          ProductPresentationItemUri = productPresentationItem.Uri.ToString(),
                                          DisplayProductsMode = key,
                                        };

      string result = System.Web.HttpUtility.UrlDecode(url);

      lock (this.ProductsUrlsCollection.SyncRoot)
      {
        this.ProductsUrlsCollection[result] = productUriLine;
      }

      return System.Web.HttpUtility.UrlPathEncode(url);
    }

    /// <summary>
    /// Gets the product presentation item.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <returns>The product presentation item.</returns>
    public virtual Item GetProductPresentationItem(Item catalogItem)
    {
      string productPresentationItemId = this.GetFirstAscendantOrSelfWithValueFromItem(this.parameters.ProductDetailPresentationStorageField, catalogItem);

      if (string.IsNullOrEmpty(productPresentationItemId) || !ID.IsID(productPresentationItemId))
      {
        return default(Item);
      }

      return catalogItem.Database.GetItem(productPresentationItemId);
    }

    /// <summary>
    /// Gets the display products mode key.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <returns>The display products mode key.</returns>
    public virtual string GetDisplayProductsModeKey(Item catalogItem)
    {
      Assert.ArgumentNotNull(catalogItem, "catalogItem");

      string displayModeId = this.GetFirstAscendantOrSelfWithValueFromItem(this.parameters.DisplayProductsModeField, catalogItem);
      if (string.IsNullOrEmpty(displayModeId))
      {
        return null;
      }

      Assert.IsTrue(ID.IsID(displayModeId), "Display Products Mode value is invalid item reference.");

      Item displayProductsModeItem = Sitecore.Context.Database.GetItem(displayModeId); // Item in folder: "/sitecore/system/Modules/Ecommerce/System/Display Product Modes"
      Assert.IsNotNull(displayProductsModeItem, "Display Products Mode item is null or not found.");
      string key = displayProductsModeItem[this.parameters.DisplayProductsModeKeyField];
      Assert.IsNotNullOrEmpty(key, "Display Products Mode Key is invalid.");

      return key;
    }

    /// <summary>
    /// Gets the first ascendant or self with value from item.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="item">The current item.</param>
    /// <returns>The first ascendant or self with value from item.</returns>
    protected virtual string GetFirstAscendantOrSelfWithValueFromItem(string field, Item item)
    {
      while (item != null && item[field] == string.Empty)
      {
        item = item.Parent;
      }

      return item != null ? item[field] : string.Empty;
    }
  }
}