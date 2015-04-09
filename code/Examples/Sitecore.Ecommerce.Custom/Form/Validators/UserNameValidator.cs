// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserNameValidator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the UserNameValidator class.
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

namespace Sitecore.Ecommerce.Form.Validators
{
  using System.Web.UI.WebControls;
  using Sitecore.Form.Core.Validators;

  /// <summary>
  /// </summary>
  public class UserNameValidator : FormCustomValidator
  {
    /// <summary>
    /// Initate User Name Validator.
    /// </summary>
    public UserNameValidator()
    {
      this.ServerValidate += OnValidate;
    }

    /// <summary>
    /// Called when [validate].
    /// </summary>
    /// <param name="source">
    /// The source.
    /// </param>
    /// <param name="args">
    /// The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.
    /// </param>
    private static void OnValidate(object source, ServerValidateEventArgs args)
    {
      args.IsValid = true;
    }
  }
}