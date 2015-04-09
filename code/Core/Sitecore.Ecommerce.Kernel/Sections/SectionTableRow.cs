// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SectionTableRow.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SectionTableRow class.
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

namespace Sitecore.Ecommerce.Sections
{
  /// <summary>
  /// </summary>
  public class SectionTableRow
  {
    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    /// <value>The name of the field.</value>
    public string FieldName { get; set; }

    /// <summary>
    /// Gets or sets the field name2.
    /// </summary>
    /// <value>The field name2.</value>
    public string FieldName2 { get; set; }

    /// <summary>
    /// Gets or sets the label.
    /// </summary>
    /// <value>The label.</value>
    public string Label { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show label column].
    /// </summary>
    /// <value><c>true</c> if [show label column]; otherwise, <c>false</c>.</value>
    public bool ShowLabelColumn { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show label].
    /// </summary>
    /// <value><c>true</c> if [show label]; otherwise, <c>false</c>.</value>
    public bool ShowLabel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [hide field].
    /// </summary>
    /// <value><c>true</c> if [hide field]; otherwise, <c>false</c>.</value>
    public bool HideField { get; set; }
  }
}