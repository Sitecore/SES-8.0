// -------------------------------------------------------------------------------------------
// <copyright file="SearchResultController.cs" company="Sitecore Corporation">
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
  using Catalogs;
  using Links;
  using Lucene.Net.Documents;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Search;

  /// <summary>
  /// Search result contoller
  /// </summary>
  public class SearchResultController
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchResultController"/> class.
    /// </summary>
    public SearchResultController()
    {
      this.ProductResolver = Context.Entity.Resolve<VirtualProductResolver>();
    }

    /// <summary>
    /// Gets or sets the product resolver.
    /// </summary>
    /// <value>The product resolver.</value>
    protected VirtualProductResolver ProductResolver { get; set; }

    /// <summary>
    /// Gets the search result.
    /// </summary>
    /// <param name="searchHits">The search hits.</param>
    /// <returns>Returns collection of search results</returns>
    public virtual IEnumerable<SearchResult> GetSearchResult(SearchHits searchHits)
    {
      List<SearchResult> result = new List<SearchResult>();
      foreach (SearchHit hit in searchHits.Slice(0))
      {
        SearchResult sr = new SearchResult();
        Item parentItem = null;
        Item resultItem = null;

        if (hit.Document.GetField(BuiltinFields.CatalogItemUri) != null)
        {
          string catalogUri = this.GetFieldValue(hit.Document, BuiltinFields.CatalogItemUri);
          if (!string.IsNullOrEmpty(catalogUri))
          {
            parentItem = Database.GetItem(new ItemUri(catalogUri));
            resultItem = Database.GetItem(new ItemUri(hit.Url));
            if (resultItem != null && parentItem != null)
            {
              sr.ResultItem.ItemLink = this.ProductResolver.GetVirtualProductUrl(parentItem, resultItem);
            }
          }
        }
        else
        {
          resultItem = Database.GetItem(new ItemUri(hit.Url));
          if (resultItem != null)
          {
            ////remove items without visualization from search result.
            if (resultItem.Visualization != null && resultItem.Visualization.Layout == null)
            {
              continue;
            }

            sr.ResultItem.ItemLink = LinkManager.GetItemUrl(resultItem);
            parentItem = resultItem.Parent;
          }
        }

        if (parentItem != null && resultItem != null)
        {
          sr.ResultItem.ItemTitle = this.GetItemTitle(resultItem);
          sr.ResultItem.ItemUri = hit.Url;

          sr.ParentItem.ItemLink = LinkManager.GetItemUrl(parentItem);
          sr.ParentItem.ItemTitle = this.GetItemTitle(parentItem);
          sr.ParentItem.ItemUri = parentItem.Uri.ToString();
        }

        result.Add(sr);
      }

      return result;
    }

    /// <summary>
    /// Gets the field value.
    /// </summary>
    /// <param name="doc">The document.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns>Returns field value from document.</returns>
    protected virtual string GetFieldValue(Document doc, string fieldName)
    {
      return StringUtil.GetString(new[] { doc.Get(fieldName) });
    }

    /// <summary>
    /// Gets the item title.
    /// </summary>
    /// <param name="item">The item to prceed.</param>
    /// <returns>Returns the item tetle field value or item name.</returns>
    protected virtual string GetItemTitle(Item item)
    {
      return !string.IsNullOrEmpty(item["Title"]) ? item["Title"] : item.Name;
    }
  }
}