// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LuceneIndexRebuilder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Lucene index rebuilder.
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
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using Sitecore.Search;
  using StringExtensions;

  /// <summary>
  /// Lucene index rebuilder.
  /// </summary>
  public class LuceneIndexRebuilder
  {
    /// <summary>
    /// Indexes names.
    /// </summary>
    private readonly ArrayList indexes = new ArrayList();

    /// <summary>
    /// Gets the indexes.
    /// </summary>
    /// <value>The indexes.</value>
    public ArrayList Indexes
    {
      get { return this.indexes; }
    }

    /// <summary>
    /// Rebuilds search indexes.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public void Rebuild([NotNull] object sender, [NotNull] EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      foreach (string indexName in from object t in this.Indexes select t as string)
      {
        try
        {
          Index index = SearchManager.GetIndex(indexName);
          if (index != null)
          {
            Log.Info("Rebuilding index \"{0}\".".FormatWith(indexName), this);

            index.Rebuild();
          }
        }
        catch (KeyNotFoundException ex)
        {
          Log.Warn("Unable to rebuild index \"{0}\". The index is not found.".FormatWith(indexName), ex, this);
        }
        catch (Exception ex)
        {
          Log.Error(ex.Message, ex, this);
        }
      }
    }
  }
}