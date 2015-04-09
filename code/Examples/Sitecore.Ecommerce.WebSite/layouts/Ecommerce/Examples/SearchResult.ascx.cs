// -------------------------------------------------------------------------------------------
// <copyright file="SearchResult.ascx.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.layouts.Ecommerce
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using Analytics.Components;
  using Classes;
  using DomainModel.Configurations;
  using Lucene.Net.Index;
  using Lucene.Net.Search;
  using Sitecore.Data;
  using Sitecore.Ecommerce.Search;

  /// <summary>
  /// Search result class
  /// </summary>
  public partial class SearchResult : UserControl
  {
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        if (NicamHelper.SafeRequest("search") != string.Empty)
        {
          var searchWords = NicamHelper.UrlDecode(NicamHelper.SafeRequest("search"));

          IEnumerable<Sitecore.Ecommerce.Search.SearchResult> resultHits = new LuceneSearcher(this.BuildQuery(searchWords), "web").Search();

          List<Sitecore.Ecommerce.Search.SearchResult> results = new List<Sitecore.Ecommerce.Search.SearchResult>();
          foreach (Sitecore.Ecommerce.Search.SearchResult resultHit in resultHits)
          {
            if (resultHit == null || resultHit.ResultItem == null || resultHit.ResultItem.ItemUri == null)
            {
              continue;
            }

            if (!results.Any(r => r.ResultItem.ItemLink == resultHit.ResultItem.ItemLink) && ItemUri.Parse(resultHit.ResultItem.ItemUri).Language == Sitecore.Context.Language && ItemUri.Parse(resultHit.ParentItem.ItemUri).Language == Sitecore.Context.Language)
            {
              results.Add(resultHit);
            }
          }

          foreach (Sitecore.Ecommerce.Search.SearchResult resultHit in results)
          {
            TreeNode pnode = this.linksTree.Nodes.Cast<TreeNode>().FirstOrDefault(t => t.NavigateUrl == resultHit.ParentItem.ItemLink);

            TreeNode node = new TreeNode(resultHit.ResultItem.ItemTitle)
            {
              NavigateUrl = resultHit.ResultItem.ItemLink,
              Value = resultHit.ResultItem.ItemUri
            };

            if (pnode == null)
            {
              pnode = new TreeNode(resultHit.ParentItem.ItemTitle)
                        {
                          NavigateUrl = resultHit.ParentItem.ItemLink,
                          Value = resultHit.ParentItem.ItemUri
                        };
              pnode.ChildNodes.Add(node);
              this.linksTree.Nodes.Add(pnode);
            }
            else
            {
              pnode.ChildNodes.Add(node);
            }
          }

          this.bodySearch.Value = searchWords;
          AnalyticsUtil.Search(searchWords, resultHits.Count());
        }
      }
      else
      {
        if (SearchAgainClicked() && this.bodySearch.Value != string.Empty)
        {
          var url = NicamHelper.RedirectUrl(Consts.SearchResultPage, "search", this.bodySearch.Value);
          //// Search Result Page via QS only. It is necessary for Print version of page
          Response.Redirect(url);
        }
      }
    }

    /// <summary>
    /// Builds the query.
    /// </summary>
    /// <param name="searchWords">The search words.</param>
    /// <returns>Return the lucene search query.</returns>
    protected virtual Lucene.Net.Search.Query BuildQuery(string searchWords)
    {
      BusinessCatalogSettings businessCatalogSettings = Sitecore.Ecommerce.Context.Entity.GetConfiguration<BusinessCatalogSettings>();
      string productRepository = businessCatalogSettings.ProductsLink;
      BooleanQuery query = new BooleanQuery();
      query.Add(new BooleanClause(new PrefixQuery(new Term(Sitecore.Search.BuiltinFields.Content, searchWords.ToLower())), Occur.MUST));
      query.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.Language, Sitecore.Context.Language.Name)), Occur.MUST);

      ////we should avoid search hits from product repository.
      if (!string.IsNullOrEmpty(productRepository))
      {
        productRepository = ShortID.Encode(new ID(productRepository)).ToLowerInvariant();
        query.Add(new BooleanClause(new PrefixQuery(new Term(Sitecore.Search.BuiltinFields.Path, productRepository)), Occur.MUST_NOT));
      }

      return query;
    }

    /// <summary>
    /// Searches the again clicked.
    /// </summary>
    /// <returns>Value indicated where search button was clicked again</returns>
    private static bool SearchAgainClicked()
    {
      return NicamHelper.SafeRequest("SearchAgain").Equals("SearchAgain");
    }
  }
}