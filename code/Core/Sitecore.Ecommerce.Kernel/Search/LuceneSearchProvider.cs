// -------------------------------------------------------------------------------------------
// <copyright file="LuceneSearchProvider.cs" company="Sitecore Corporation">
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
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using Lucene.Net.Index;
  using Lucene.Net.Search;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Search;

  /// <summary>
  /// Lucene Search Provider
  /// </summary>
  public class LuceneSearchProvider : DatabaseSearchProvider
  {
    /// <summary>
    /// Gets or sets the name of the index.
    /// </summary>
    /// <value>The name of the index.</value>
    public virtual string IndexName { get; set; }

    /// <summary>
    /// Provide search with the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <returns>The search result.</returns>
    [Obsolete("Use Search method with database parameter instead.")]
    public override IEnumerable<Item> Search(Query query)
    {
      Assert.ArgumentNotNull(query, "query");

      System.Diagnostics.Debugger.Launch();

      return this.Search(query, this.Database);
    }

    /// <summary>
    /// Searches the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="database">The database.</param>
    /// <returns>Returns collection of items</returns>
    public override IEnumerable<Item> Search(Query query, Database database)
    {
      Assert.ArgumentNotNull(query, "query");
      Assert.ArgumentNotNull(database, "database");

      string indexName = string.IsNullOrEmpty(this.IndexName) ? "products" : this.IndexName;

      IEnumerable<Item> items;

      LuceneQueryBuilder builder = new LuceneQueryBuilder(query, this.Database);
      BooleanQuery luceneQuery = builder.BuildResultQuery();
      using (IndexSearchContext context = SearchManager.GetIndex(indexName).CreateSearchContext())
      {
        this.AddDecorations(luceneQuery, database.Name);
        PreparedQuery pq = new PreparedQuery(luceneQuery);
        ////Sitecore Query parser does not allow to use IDs in field values.
        SearchHits hits = context.Search(pq, int.MaxValue);
        items = this.GetSearchResultItems(hits);
      }

      return items;
    }

    /// <summary>
    /// Gets the search result items.
    /// </summary>
    /// <param name="hits">The search hits.</param>
    /// <returns>Returns a list of items</returns>
    protected virtual IEnumerable<Item> GetSearchResultItems(SearchHits hits)
    {
      return hits.FetchResults(0, 0).Select<SearchResult, Item>(res => res.GetObject<Item>());
    }

    /// <summary>
    /// Adds the decorations.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="databaseName">Name of the database.</param>
    protected virtual void AddDecorations(BooleanQuery query, string databaseName)
    {
      Assert.ArgumentNotNull(query, "query");
      Assert.ArgumentNotNull(databaseName, "databaseName");

      query.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.Language, Sitecore.Context.Language.Name.ToLower())), Occur.MUST);
      query.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.Database, databaseName.ToLower())), Occur.MUST);
      query.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.LatestVersion, "1")), Occur.MUST);
    }
  }
}