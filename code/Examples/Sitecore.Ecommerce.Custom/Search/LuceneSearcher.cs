// -------------------------------------------------------------------------------------------
// <copyright file="LuceneSearcher.cs" company="Sitecore Corporation">
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
  using Diagnostics;
  using Lucene.Net.Search;
  using Sitecore.Search;

  /// <summary>
  /// Lucene searcher
  /// </summary>
  public class LuceneSearcher
  {
    /// <summary>
    /// Lucene query for searching
    /// </summary>
    private Lucene.Net.Search.Query query;

    /// <summary>
    /// Name of index for search
    /// </summary>
    private string indexName;

    /// <summary>
    /// Initializes a new instance of the <see cref="LuceneSearcher"/> class.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="indexName">Name of the index.</param>
    public LuceneSearcher(Lucene.Net.Search.Query query, string indexName)
    {
      Assert.ArgumentNotNull(query, "query");
      Assert.ArgumentNotNullOrEmpty(indexName, "indexName");

      this.query = query;
      this.indexName = indexName;
    }

    /// <summary>
    /// Searches this instance.
    /// </summary>
    /// <returns>Returns the search results</returns>
    public virtual IEnumerable<SearchResult> Search()
    {
      using (IndexSearchContext context = SearchManager.GetIndex(this.indexName).CreateSearchContext())
      {
        SearchResultController controller = new SearchResultController();        
        return controller.GetSearchResult(context.Search(new PreparedQuery(this.query), int.MaxValue)); 
      }
    }
  }
}