// -------------------------------------------------------------------------------------------
// <copyright file="SearchContext.cs" company="Sitecore Corporation">
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
  using Lucene.Net.Index;
  using Lucene.Net.Search;
  using Sitecore.Search;

  /// <summary>
  /// Search context
  /// </summary>
  public class SearchContext : ISearchContext
  {
    /// <summary>
    /// Decorates the specified query adding context information.
    /// </summary>
    /// <param name="query">The source query.</param>
    /// <returns>The decorated query.</returns>
    public Lucene.Net.Search.Query Decorate(Lucene.Net.Search.Query query)
    {
      BooleanQuery result = new BooleanQuery(true);
      result.Add(query, Occur.MUST);
      this.AddDecorations(result);
      return result;
    }

    /// <summary>
    /// Adds the decorations.
    /// </summary>
    /// <param name="query">The query.</param>
    protected virtual void AddDecorations(BooleanQuery query)
    {
      query.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.Language, Sitecore.Context.Language.Name)), Occur.MUST); 
    }
  }
}