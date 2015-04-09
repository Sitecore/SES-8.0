// -------------------------------------------------------------------------------------------
// <copyright file="NewPassword.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Form.Action
{
  using System;
  using System.Collections.Specialized;
  using System.Web;
  using System.Web.Security;
  using DomainModel.Mails;
  using DomainModel.Users;
  using Forms.Actions;
  using Sitecore.Data;
  using Sitecore.Form.Core.Client.Data.Submit;
  using Sitecore.Form.Core.Pipelines.RenderForm;
  using Sitecore.Form.Submit;
  using Sitecore.Security.Authentication;

  /// <summary>
  /// The New Password form.
  /// </summary>
  public class NewPassword : ILoad, ISaveAction
  {
    #region Constants

    /// <summary>
    /// The encrypt key.
    /// </summary>
    private const string EncryptKey = "5dfkjek5";

    /// <summary>
    /// The mail template name password reset.
    /// </summary>
    private const string MailTemplateNamePasswordReset = "Your password has been reset";

    #endregion

    #region Public methods

    /// <summary>
    /// The form load event handler.
    /// </summary>
    /// <param name="isPostback">Gets a value that indicates whether the form is being rendered for the first time or is being loaded in response to a postback.</param>
    /// <param name="args">The render form arguments.</param>
    public void Load(bool isPostback, RenderFormArgs args)
    {
    }

    /// <summary>
    /// Submits an email to the user who wants to reset the password on the user account.
    /// </summary>
    /// <param name="formid">The formid.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="data">The data.</param>
    public void Execute(ID formid, AdaptedResultList fields, params object[] data)
    {
      NameValueCollection form = new NameValueCollection();
      ActionHelper.FillFormData(form, fields, null);

      string encryptKey = HttpContext.Current.Request.QueryString["key"];
      encryptKey = Uri.UnescapeDataString(encryptKey);

      if (string.IsNullOrEmpty(encryptKey))
      {
        return;
      }

      string decryptKey = Crypto.DecryptTripleDES(encryptKey, EncryptKey);

      string[] values = decryptKey.Split('|');
      string username = values[0];
      string password = values[1];

      string userName = string.Format("{0}\\{1}", Sitecore.Context.Domain.Name, username);
      MembershipUser membershipUser = Membership.GetUser(userName);

      // Checks that the user information is correct for the user who want's to change password
      if (!AuthenticationManager.Login(userName, password) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
      {
        return;
      }

      // We can continue if the information is correct
      if (membershipUser == null)
      {
        return;
      }

      string newPassword = form["Password"];

      ICustomerManager<CustomerInfo> customerProvider = Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
      CustomerInfo customerInfo = customerProvider.GetCustomerInfo(username);
      string email = customerInfo.Email;

      if (!membershipUser.ChangePassword(password, newPassword))
      {
        return;
      }

      IMail mailProvider = Context.Entity.Resolve<IMail>();
      mailProvider.SendMail(MailTemplateNamePasswordReset, new { Recipient = email }, string.Empty);
    }

    #endregion
  }
}