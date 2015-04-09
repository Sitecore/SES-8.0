// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchOptions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SearchOption type.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models.Search
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using Text;

  /// <summary>
  /// Defines the SearchOption type.
  /// </summary>
  public class SearchOptions
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchOptions"/> class.
    /// </summary>
    public SearchOptions()
    {
      this.Checklists = new NameValueCollection();
      this.GridColumns = new List<GridColumn>();
      this.SearchFields = new NameValueCollection();
      this.Templates = new ListString();
    }

    /// <summary>
    /// Gets or sets the search root.
    /// </summary>
    /// <value>The search root.</value>
    public virtual string SearchRoot { get; set; }

    /// <summary>
    /// Gets or sets the search fields which search provider uses for search.
    /// These fields are specified by catalog settings.
    /// </summary>
    /// <value>The search fields.</value>
    public NameValueCollection SearchFields { get; set; }

    /// <summary>
    /// Gets or sets the search checklists which search provider uses for search.
    /// These fields are specified by catalog settings.
    /// </summary>
    /// <value>The checklists.</value>
    public NameValueCollection Checklists { get; set; }

    /// <summary>
    /// Gets or sets the grid columns.
    /// </summary>
    /// <value>The grid columns.</value>
    public List<GridColumn> GridColumns { get; set; }

    /// <summary>
    /// Gets or sets the parameters.
    /// </summary>
    /// <value>The parameters.</value>
    public ListString Templates { get; set; }
  }
}