// -------------------------------------------------------------------------------------------
// <copyright file="PasswordValidator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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
  using DomainModel.Users;
  using Sitecore.Form.Core.Validators;
  using Sitecore.Security.Authentication;

  /// <summary>
  /// Validator class that validates the current user towards a password provided.
  /// </summary>
  public class PasswordValidator : FormCustomValidator
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordValidator"/> class.
    /// </summary>
    public PasswordValidator()
    {
      this.ServerValidate += OnValidate;
    }

    /// <summary>
    /// Validates wheether the current user and password fits together.
    /// </summary>
    /// <param name="source">
    /// The source.
    /// </param>
    /// <param name="args">
    /// The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.
    /// </param>
    private static void OnValidate(object source, ServerValidateEventArgs args)
    {
      ICustomerManager<CustomerInfo> customerManager = Ecommerce.Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
      string username = customerManager.CurrentUser.NickName;

      var provider = new FormsAuthenticationProvider();
      var helper = new AuthenticationHelper(provider);
      args.IsValid = helper.ValidateUser(username, args.Value);
    }
  }
}