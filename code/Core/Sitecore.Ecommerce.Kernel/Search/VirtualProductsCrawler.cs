// -------------------------------------------------------------------------------------------
// <copyright file="VirtualProductsCrawler.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Search
{
  using System.Collections.Generic;
  using System.Linq;
  using Catalogs;
  using Configuration;
  using Data;
  using Diagnostics;
  using DomainModel.Catalogs;
  using Globalization;
  using Lucene.Net.Documents;
  using Lucene.Net.Index;
  using Lucene.Net.Search;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Search;
  using Sites;
  using Utils;

  /// <summary>
  /// Virtual products database crawler
  /// </summary>
  public class VirtualProductsCrawler : DatabaseCrawler
  {
    /// <summary>
    /// Product catalog template ID
    /// </summary>
    private static readonly string productCatalogTemplateId = "{BD5B78E1-107E-4572-90DD-3F11F8A7534E}";

    /// <summary>
    /// Indexes the virtual products.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <param name="productItems">The product items.</param>
    /// <param name="context">The context.</param>
    protected virtual void AddVirtualProducts(Item catalogItem, IEnumerable<Item> productItems, IndexUpdateContext context)
    {
      foreach (Item itm in productItems)
      {
        foreach (Language language in itm.Languages)
        {
          Item latestVersion = itm.Database.GetItem(itm.ID, language, Version.Latest);
          if (latestVersion != null)
          {
            foreach (Item version in latestVersion.Versions.GetVersions(false))
            {
              this.AddVirtualProduct(catalogItem, version, latestVersion, context);
            }
          }
        }
      }
    }

    /// <summary>
    /// Adds the virtual product.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <param name="version">The version.</param>
    /// <param name="latestVersion">The latest version.</param>
    /// <param name="context">The context.</param>
    protected virtual void AddVirtualProduct(Item catalogItem, Item version, Item latestVersion, IndexUpdateContext context)
    {
      this.IndexVersion(version, latestVersion, context, catalogItem);
    }

    /// <summary>
    /// Adds the item to the index.
    /// </summary>
    /// <param name="item">The Sitecore item to index.</param>
    /// <param name="latestVersion">The latest version.</param>
    /// <param name="context">The context.</param>
    protected override void IndexVersion(Item item, Item latestVersion, IndexUpdateContext context)
    {
      this.IndexVersion(item, latestVersion, context, null);

      if (this.IsCatalogItem(item))
      {
        string siteName = SiteUtils.GetSiteByItem(item);
        if (!string.IsNullOrEmpty(siteName))
        {
          using (new SiteContextSwitcher(Factory.GetSite(siteName)))
          {
            using (new SiteIndependentDatabaseSwitcher(item.Database))
            {
              this.AddVirtualProducts(item, this.GetVirtualProductsForIndexing(item), context);
            }
          }
        }
      }
    }

    /// <summary>
    /// Indexes the version.
    /// </summary>
    /// <param name="item">The item to proceed.</param>
    /// <param name="latestVersion">The latest version.</param>
    /// <param name="context">The context.</param>
    /// <param name="catalogItem">The catalog item.</param>
    protected virtual void IndexVersion(Item item, Item latestVersion, IndexUpdateContext context, Item catalogItem)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(latestVersion, "latestVersion");
      Assert.ArgumentNotNull(context, "context");

      Document document = new Document();
      this.AddVersionIdentifiers(item, latestVersion, document);
      this.AddAllFields(document, item, true);
      this.AddSpecialFields(document, item);
      this.AdjustBoost(document, item);

      if (catalogItem != null)
      {
        this.AddVirtualProductIdentifiers(document, item, catalogItem);
      }

      context.AddDocument(document);
    }

    /// <summary>
    /// Deletes the specific version information from the index.
    /// </summary>
    /// <param name="id">The item id.</param>
    /// <param name="language">The language.</param>
    /// <param name="version">The version.</param>
    /// <param name="context">The context.</param>
    protected override void DeleteVersion(ID id, string language, string version, IndexDeleteContext context)
    {
      base.DeleteVersion(id, language, version, context);
      ItemUri versionUri = new ItemUri(id, Language.Parse(language), Version.Parse(version), Factory.GetDatabase(this.Database));
      Item versionItem = Sitecore.Data.Database.GetItem(versionUri);
      if (versionItem != null && this.IsCatalogItem(versionItem))
      {
        context.DeleteDocuments(context.Search(new PreparedQuery(this.GetVirtualProductsQuery(versionItem)), int.MaxValue).Ids);
      }
    }

    /// <summary>
    /// Gets the virtual products query.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <returns>Returns virtual products query.</returns>
    protected virtual Lucene.Net.Search.Query GetVirtualProductsQuery(Item catalogItem)
    {
      BooleanQuery query = new BooleanQuery();
      query.Add(new TermQuery(new Term(BuiltinFields.CatalogItemUri, catalogItem.Uri.ToString())), Occur.MUST);
      this.AddMatchCriteria(query);
      return query;
    }

    /// <summary>
    /// Adds the virtual product identifiers.
    /// </summary>
    /// <param name="document">The document.</param>
    /// <param name="productItem">The product item.</param>
    /// <param name="catalogItem">The catalog item.</param>
    protected virtual void AddVirtualProductIdentifiers(Document document, Item productItem, Item catalogItem)
    {
      document.Add(CreateDataField(BuiltinFields.CatalogItemUri, catalogItem.Uri.ToString()));
      ////replacing product path to catalog item child path
      Field pathField = document.GetField(Sitecore.Search.BuiltinFields.Path);
      string productId = _shortenGuid.Replace(productItem.ID.ToString(), string.Empty).ToLowerInvariant();
      string path = string.Format("{0} {1}", this.GetItemPath(catalogItem), productId);
      pathField.SetValue(path);
    }

    /// <summary>
    /// Gets the virtual products.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <returns>Returns list of virtual products</returns>
    protected virtual IEnumerable<Item> GetVirtualProductsForIndexing(Item catalogItem)
    {
      string selectedMethod = catalogItem["Product Selection Method"];
      if (string.IsNullOrEmpty(selectedMethod) || !ID.IsID(selectedMethod))
      {
        return new List<Item>();
      }

      Item selectionMethodItem = catalogItem.Database.GetItem(selectedMethod);
      if (selectionMethodItem != null)
      {
        string selectionMethodName = selectionMethodItem["Code"];
        var catalogProductResolveStrategy = Context.Entity.Resolve<ICatalogProductResolveStrategy>(selectionMethodName);
        var catalogProductResolveStrategyBase = catalogProductResolveStrategy as CatalogProductResolveStrategyBase;

        if (catalogProductResolveStrategyBase != null)
        {
          return catalogProductResolveStrategyBase.GetCatalogProductItems(catalogItem);
        }
      }
      else
      {
        Log.Warn(string.Format("Product Selection Method was noe found in item {0}", catalogItem.ID), this);
      }

      return new List<Item>();
    }

    /// <summary>
    /// Determines whether [is catalog item] [the specified item].
    /// </summary>
    /// <param name="item">The item to proceed.</param>
    /// <returns>
    /// <c>true</c> if [is catalog item] [the specified item]; otherwise, <c>false</c>.
    /// </returns>
    protected virtual bool IsCatalogItem(Item item)
    {
      if (item.TemplateID.ToString() == productCatalogTemplateId)
      {
        return true;
      }

      return item.Template.BaseTemplates.Any(tp => tp.ID.ToString() == productCatalogTemplateId);
    }
  }
}