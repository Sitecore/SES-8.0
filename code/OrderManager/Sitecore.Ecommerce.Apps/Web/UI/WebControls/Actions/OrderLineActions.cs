// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineActions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineActions type.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls.Actions
{
  /// <summary>
  /// Enum of the order line actions.
  /// </summary>
  public enum OrderLineActions
  {
    /// <summary>
    ///  Marker of the adding of the new order line.
    /// </summary>
    Add = 0,

    /// <summary>
    ///  Marker of the editing of the existing order line.
    /// </summary>
    Edit = 1
  }
}