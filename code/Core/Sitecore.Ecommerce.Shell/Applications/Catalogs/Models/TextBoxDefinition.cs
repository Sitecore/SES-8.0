// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxDefinition.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Represents a search field wich should be assigned to an item field. Configured on catalog settings level.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models
{
  using Data;

  /// <summary>
  /// Represents a search field wich should be assigned to an item field. Configured on catalog settings level.
  /// </summary>
  public class TextBoxDefinition
  {
    /// <summary>
    /// Gets or sets the field name to search.
    /// </summary>
    /// <value>The field name to search.</value>
    [Entity(FieldName = "Field")]
    public string Field { get; set; }

    /// <summary>
    /// Gets or sets the title to show at a search text box.
    /// </summary>
    /// <value>The title.</value>
    [Entity(FieldName = "Title")]
    public string Title { get; set; }
  }
}