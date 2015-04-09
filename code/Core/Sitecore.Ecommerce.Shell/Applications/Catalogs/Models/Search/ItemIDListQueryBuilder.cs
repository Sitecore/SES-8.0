// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemIDListQueryBuilder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Builds a search result query from item id list. Uses inner query builder provider.
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

using Sitecore.Ecommerce.Search;

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models.Search
{
  using System.Collections.Generic;
  using Sitecore.Data;

  /// <summary>
  /// Builds a search result query from item id list. Uses inner query builder provider.
  /// </summary>
  public class ItemIDListQueryBuilder : QueryBuilderWrapper
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemIDListQueryBuilder"/> class.
    /// </summary>    
    /// <param name="idList">The id list.</param>
    public ItemIDListQueryBuilder(IEnumerable<string> idList)
    {
      var ids = new List<string>(idList);
      for (var i = 0; i < ids.Count; i++)
      {
        if (!ID.IsID(ids[i]))
        {
          continue;
        }

        this.InnerBuilder.AppendAttribute("id", ids[i], MatchVariant.Exactly);
        if (i < ids.Count - 1 && ID.IsID(ids[i + 1]))
        {
          this.InnerBuilder.AppendCondition(QueryCondition.Or);
        }
      }
    }
  }
}