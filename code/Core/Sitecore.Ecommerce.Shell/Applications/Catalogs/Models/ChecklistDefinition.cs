// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChecklistDefinition.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the checklist section in a catalog to provide additional search result filtering.
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
  using System.Collections.ObjectModel;

  /// <summary>
  /// Defines the checklist section in a catalog to provide additional search result filtering.
  /// </summary>
  public class ChecklistDefinition
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ChecklistDefinition"/> class.
    /// </summary>
    public ChecklistDefinition()
    {
      this.Checkboxes = new Collection<ChecklistItem>();
    }

    /// <summary>
    /// Gets or sets the checklist header.
    /// </summary>
    /// <value>The checklist header.</value>
    public string Header { get; set; }

    /// <summary>
    /// Gets or sets the field name for search filters.
    /// </summary>
    /// <value>The field name.</value>
    public string Field { get; set; }

    /// <summary>
    /// Gets or sets the list of the checkboxes for current checklist.
    /// </summary>
    /// <value>The checkboxes.</value>
    public Collection<ChecklistItem> Checkboxes { get; set; }
  }
}