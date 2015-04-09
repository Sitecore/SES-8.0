// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines event arguments for textbox events.
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

  /// <summary>
  /// Defines event arguments for textbox events.
  /// </summary>
  public class TextBoxEventArgs : EventArgs
  {
    /// <summary>
    /// Gets or sets the text box definition.
    /// </summary>
    /// <value>The text box definition.</value>
    public TextBoxDefinition TextBoxDefinition { get; set; }

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>The text.</value>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this text box is wide.
    /// </summary>
    /// <value><c>true</c> if this textbox is wide; otherwise, <c>false</c>.</value>
    public bool IsWide { get; set; }
  }
}