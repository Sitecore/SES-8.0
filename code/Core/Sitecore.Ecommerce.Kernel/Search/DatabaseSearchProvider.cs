// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseSearchProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the DatabaseSearchProvider type.
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

namespace Sitecore.Ecommerce.Search
{
  using System;
  using System.Collections.Generic;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sites;

  /// <summary>
  /// Defines the DatabaseSearchProvider type.
  /// </summary>
  public abstract class DatabaseSearchProvider : ISearchProvider
  {
    /// <summary>
    /// Gets the database.
    /// </summary>
    /// <value>The database.</value>
    [Obsolete("Use Search method with database parameter instead.")]
    public virtual Database Database
    {
      get
      {
        SiteContext site = Sitecore.Context.Site;
        Database content = Sitecore.Context.ContentDatabase;

        if (site != null && (site.Name == "modules_shell" || site.Name == "shell") && content != null)
        {
          return content;
        }

        return Sitecore.Context.Database;
      }
    }

    /// <summary>
    /// Searches the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>Returns collection of items</returns>
    [Obsolete("Use Search method with database parameter instead.")]
    public abstract IEnumerable<Item> Search(Query query);

    /// <summary>
    /// Searches the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="database">The database.</param>
    /// <returns>Returns collection of items</returns>
    public abstract IEnumerable<Item> Search(Query query, Database database);
  }
}