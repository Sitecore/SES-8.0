// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILoad.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Describes ILoad interface.
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

namespace Sitecore.Ecommerce.Forms.Actions
{
  using Sitecore.Form.Core.Pipelines.RenderForm;

  /// <summary>
  /// ILoad interface.
  /// </summary>
  public interface ILoad
  {
    /// <summary>
    /// The form load event handler.
    /// </summary>
    /// <param name="isPostback">Gets a value that indicates whether the form is being rendered for the first time or is being loaded in response to a postback.</param>
    /// <param name="args">The render form arguments.</param>
    void Load(bool isPostback, RenderFormArgs args);
  }
}