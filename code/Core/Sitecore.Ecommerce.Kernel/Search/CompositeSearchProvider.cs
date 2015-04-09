﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeSearchProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the CompositeSearchProvider type.
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
  using System.Linq;
  using Diagnostics;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// Defines the composite search provider class.
  /// </summary>
  public class CompositeSearchProvider : DatabaseSearchProvider
  {
    /// <summary>
    /// Gets or sets the primary search provider.
    /// </summary>
    /// <value>The primary search provider.</value>
    public ISearchProvider PrimarySearchProvider { get; set; }

    /// <summary>
    /// Gets or sets the secondary search provider.
    /// </summary>
    /// <value>The secondary search provider.</value>
    public ISearchProvider SecondarySearchProvider { get; set; }

    /// <summary>
    /// Searches the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>Returns collection of items</returns>
    [Obsolete("Use Search method with database parameter instead.")]
    public override IEnumerable<Item> Search(Query query)
    {
      Assert.ArgumentNotNull(query, "query");

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
      Assert.IsNotNull(query, "query");
      Assert.IsNotNull(database, "database");

      IEnumerable<Item> result = Enumerable.Empty<Item>();

      if (this.PrimarySearchProvider != null)
      {
        result = this.PrimarySearchProvider.Search(query, database);
      }

      if (((result == null) || (!result.Any())) && (this.SecondarySearchProvider != null))
      {
        result = this.SecondarySearchProvider.Search(query, database);
      }

      return result ?? Enumerable.Empty<Item>();
    }
  }
}
