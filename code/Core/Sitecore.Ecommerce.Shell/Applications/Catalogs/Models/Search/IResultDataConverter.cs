// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResultDataConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Converts result data to format appropriate for a view.
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

  /// <summary>
  /// Converts result data to format appropriate for a view.
  /// </summary>
  /// <typeparam name="T">The result data format.</typeparam>
  public interface IResultDataConverter<T>
  {
    /// <summary>
    /// Converts the specified initial result.
    /// </summary>
    /// <param name="initialResult">The initial result.</param>
    /// <param name="columns">The columns.</param>
    /// <returns>
    /// The data converted to the GridData format.
    /// </returns>
    GridData Convert(T initialResult, List<GridColumn> columns);
  }
}