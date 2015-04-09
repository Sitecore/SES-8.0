// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChecklistEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines event arguments for checklist events.
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
  using System;
  using Text;

  /// <summary>
  /// Defines event arguments for checklist events.
  /// </summary>
  public class ChecklistEventArgs : EventArgs
  {
    /// <summary>
    /// Gets or sets the checklist definition.
    /// </summary>
    /// <value>The checklist definition.</value>
    public ChecklistDefinition ChecklistDefinition { get; set; }

    /// <summary>
    /// Gets or sets the checked values.
    /// </summary>
    /// <value>The checked values.</value>
    public ListString CheckedValues { get; set; }
  }
}