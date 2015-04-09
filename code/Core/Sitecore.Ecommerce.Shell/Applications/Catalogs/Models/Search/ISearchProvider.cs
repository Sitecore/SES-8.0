// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Describes search provider contract
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
  /// <summary>
  /// The search provider.
  /// </summary>
  public interface ISearchProvider
  {
    /// <summary>
    /// Provide search with the specified query.
    /// </summary>
    /// <param name="options">The search options.</param>
    /// <returns>The search result.</returns>
    GridData Search(SearchOptions options);
  }
}