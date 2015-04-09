// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryBuilderWrapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the QueryBiulderWrapper type.
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
  using Sitecore.Ecommerce.Search;

  /// <summary>
  /// Wraps a query builder in order to provide additional query configuration
  /// </summary>
  public class QueryBuilderWrapper
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryBuilderWrapper"/> class.
    /// </summary>    
    public QueryBuilderWrapper()
    {
      this.InnerBuilder = new Query();
    }

    /// <summary>
    /// Gets or sets the search root.
    /// </summary>
    /// <value>The search root.</value>
    public virtual string SearchRoot
    {
      get { return this.InnerBuilder.SearchRoot; }
      set { this.InnerBuilder.SearchRoot = value; }
    }

    /// <summary>
    /// Gets or sets the inner builder.
    /// </summary>
    /// <value>The inner builder.</value>
    public Query InnerBuilder { get; set; }

    /// <summary>
    /// Gets the result query.
    /// </summary>
    /// <returns>Returns Query</returns>
    public virtual Query GetResultQuery()
    {
      return this.InnerBuilder;
    }
  }
}