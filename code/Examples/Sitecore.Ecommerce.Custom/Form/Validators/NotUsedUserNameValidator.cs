// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotUsedUserNameValidator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the NotUsedUserNameValidator class.
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
  using System.Web.Security;
  using System.Web.UI.WebControls;
  using Sitecore.Form.Core.Validators;

  /// <summary>
  /// Validates if the username provided is in use.
  /// </summary>
  public class NotUsedUserNameValidator : FormCustomValidator
  {
    // private const string ExtranetPrefix = @"extranet\";

    /// <summary>
    /// Initate the Not Used User Name Validator
    /// </summary>
    public NotUsedUserNameValidator()
    {
      this.ServerValidate += this.OnValidate;
    }

    /// <summary>
    /// Username is not in use before.
    /// </summary>
    /// <param name="source">
    /// The source.
    /// </param>
    /// <param name="args">
    /// The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.
    /// </param>
    private void OnValidate(object source, ServerValidateEventArgs args)
    {
      args.IsValid = Membership.GetUser(Sitecore.Context.Domain.Name + @"\" + args.Value) == null;
    }
  }
}