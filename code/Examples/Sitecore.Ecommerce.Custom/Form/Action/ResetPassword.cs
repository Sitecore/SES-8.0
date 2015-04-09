// -------------------------------------------------------------------------------------------
// <copyright file="ResetPassword.cs" company="Sitecore Corporation">
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
  using System.Web.Security;
  using DomainModel.Mails;
  using DomainModel.Users;
  using Forms.Actions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Form.Core.Client.Data.Submit;
  using Sitecore.Form.Core.Pipelines.RenderForm;
  using Sitecore.Form.Submit;
  using Unity;

  /// <summary>
  /// The Reset Password form.
  /// </summary>
  public class ResetPassword : ILoad, ISaveAction
  {
    #region Constants

    /// <summary>
    /// The mail template name reset your password.
    /// </summary>
    private static readonly string MailTemplateNameResetYourPassword = "Reset your password";

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
    /// The user submitts an email to reset the password
    /// for the user account.
    /// The current password is encrypted in the email
    /// in order to be able to change the password.
    /// </summary>
    /// <param name="formid"> The formid. </param>
    /// <param name="fields"> The fields. </param>
    /// <param name="data">The data.</param>
    public void Execute(ID formid, AdaptedResultList fields, params object[] data)
    {
      NameValueCollection form = new NameValueCollection();
      ActionHelper.FillFormData(form, fields, null);

      MembershipUser membershipUser = Membership.GetUser(Sitecore.Context.Domain.Name + @"\" + form["UserName"]);
      if (membershipUser != null)
      {
        string newPassword = Membership.GeneratePassword(6, 0);
        string oldPwd = membershipUser.ResetPassword();
        newPassword = newPassword.Replace("|", string.Empty);
        membershipUser.ChangePassword(oldPwd, newPassword);

        ICustomerManager<CustomerInfo> customerProvider = Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
        CustomerInfo customerInfo = customerProvider.GetCustomerInfo(form["UserName"]);
        string email = customerInfo.Email;

        if (!Utils.MainUtil.IsValidEmailAddress(email))
        {
          email = customerInfo.Email;
        }

        string key = form["UserName"] + "|" + newPassword;
        string encryptKey = Crypto.EncryptTripleDES(key, "5dfkjek5");
        encryptKey = Uri.EscapeDataString(encryptKey);
        Item newPasswordLink = Utils.ItemUtil.GetNavigationLinkItem("New Password");
        string url = Utils.ItemUtil.GetNavigationLinkPath(newPasswordLink) + "?key=" + encryptKey;

        var param = new
        {
          Recipient = email,
          URL = url
        };
        IMail mailProvider = Context.Entity.Resolve<IMail>();
        mailProvider.SendMail(MailTemplateNameResetYourPassword, param, string.Empty);
      }

      // No action is performed because of the security policies.
    }

    #endregion
  }
}