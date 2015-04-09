// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridCommandEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines event arguments for a grid row events.
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
  using System.Collections.Specialized;
  
  /// <summary>
  /// Defines event arguments for a grid row events.
  /// </summary>
  public class GridCommandEventArgs : EventArgs
  {
    /// <summary>
    /// Gets or sets the row ID.
    /// </summary>
    /// <value>The row ID.</value>
    public string RowID { get; set; }

    /// <summary>
    /// Gets or sets the rows ID.
    /// </summary>
    /// <value>The rows ID.</value>
    public StringCollection RowsID { get; set; }
  }
}