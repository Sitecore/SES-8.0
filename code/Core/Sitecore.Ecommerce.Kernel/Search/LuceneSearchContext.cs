// -------------------------------------------------------------------------------------------
// <copyright file="LuceneSearchContext.cs" company="Sitecore Corporation">
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
  using Diagnostics;
  using Lucene.Net.Index;
  using Lucene.Net.Search;
  using Shell;
  using Shell.Data;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Search;
  using Sitecore.Security.Accounts;

  /// <summary>
  /// Search Context
  /// </summary>
  public class LuceneSearchContext : SearchContext
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LuceneSearchContext"/> class.
    /// </summary>
    /// <param name="user">The current user</param>
    public LuceneSearchContext(User user) : base(user)
    {      
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LuceneSearchContext"/> class.
    /// </summary>
    /// <param name="user">The current user.</param>
    /// <param name="root">The search root.</param>
    public LuceneSearchContext(User user, Item root) : base(user, root)
    {      
    }

    /// <summary>
    /// Adds the decorations to the BooleanQuery object. Override in derived class to change the way context affects queries.
    /// </summary>
    /// <param name="result">The result.</param>
    protected override void AddDecorations(BooleanQuery result)
    {
      Assert.ArgumentNotNull(result, "result");
      User user = this.User;
      if (user != null)
      {
        result.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.Creator, user.Name)), Occur.SHOULD);
        result.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.Editor, user.Name)), Occur.SHOULD);
      }

      Item item = this.Item;
      if (item != null)
      {
        result.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.Path, ShortID.Encode(item.ID).ToLowerInvariant())), Occur.MUST);
        result.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.Database, item.Database.Name)), Occur.MUST);
        // result.Add(new TermQuery(new Term(BuiltinFields.Language, item.Language.ToString())), Occur.MUST);
      }

      if (!this.IgnoreContentEditorOptions)
      {
        if (!UserOptions.View.ShowHiddenItems)
        {
          result.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.Hidden, "1")), Occur.MUST_NOT);
        }

        if (!UserOptions.View.ShowEntireTree && (item != null))
        {
          Item item2 = item.Database.GetItem(RootSections.GetSection(item));
          if (item2 != null)
          {
            result.Add(new TermQuery(new Term(Sitecore.Search.BuiltinFields.Path, ShortID.Encode(item2.ID).ToLowerInvariant())), Occur.MUST);
          }
        }
      }
    }
  }
}