// -------------------------------------------------------------------------------------------
// <copyright file="ChangePassword.cs" company="Sitecore Corporation">
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
  using System.Collections.Specialized;
  using System.Web.Security;
  using DomainModel.Mails;
  using DomainModel.Users;
  using Sitecore.Data;
  using Sitecore.Form.Core.Client.Data.Submit;
  using Sitecore.Form.Core.Client.Submit;  
  using Sitecore.Form.Core.Pipelines.RenderForm;
  using Sitecore.Form.Submit;
  using Sitecore.Security.Authentication;
  using Unity;
  using Utils;

  /// <summary>
  /// The change password.
  /// </summary>
  public class ChangePassword : ISaveAction
  {
    #region Constants

    /// <summary>
    /// The mail template name password changed.
    /// </summary>
    private const string MailTemplateNamePasswordChanged = "Your password has been changed";

    #endregion

    #region Public methods

    /// <summary> Submitts the new password for the associated sitecore membership account </summary>
    /// <param name="formid">The formid.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="data">The data.</param>
    /// <exception cref="ValidatorException">The password information provided is incorrect.</exception>
    public void Execute(ID formid, AdaptedResultList fields, params object[] data)
    {
      NameValueCollection form = new NameValueCollection();
      ActionHelper.FillFormData(form, fields, null);
      
      if (!string.IsNullOrEmpty(form["CreatePassword"]))
      {
        ICustomerManager<CustomerInfo> customerManager = Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
        string customerId = customerManager.CurrentUser.NickName;

        MembershipUser membershipUser = Membership.GetUser(customerId);

        // Checks that the user information is correct for the user who want's to change password
        if (AuthenticationManager.Login(customerId, form["OldPassword"]) && !string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(form["OldPassword"]))
        {
          // We can continue if the information is correct
          if (membershipUser != null)
          {
            string email = customerManager.CurrentUser.Email;

            if (!MainUtil.IsValidEmailAddress(email))
            {
              email = membershipUser.Email;
            }

            if (membershipUser.ChangePassword(form["OldPassword"], form["CreatePassword"]))
            {
              var param = new { Recipient = email };

              IMail mailProvider = Context.Entity.Resolve<IMail>();
              mailProvider.SendMail(MailTemplateNamePasswordChanged, param, string.Empty);
            }
          }
        }
        else
        {
          throw new ValidatorException("The password information provided is incorrect.");
        }
      }
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// The on load. </summary>
    /// <param name="isPostback">Is PostBack </param>
    /// <param name="args">Render form args </param>
    protected void OnLoad(bool isPostback, RenderFormArgs args)
    {
    }

    #endregion
  }
}