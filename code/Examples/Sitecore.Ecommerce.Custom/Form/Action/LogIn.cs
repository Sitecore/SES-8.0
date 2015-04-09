// -------------------------------------------------------------------------------------------
// <copyright file="LogIn.cs" company="Sitecore Corporation">
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
  using System.Web;
  using Analytics.Components;
  using DomainModel.Users;
  using Ecommerce;
  using Exceptions;
  using Forms;
  using Forms.Actions;
  using Globalization;
  using Links;
  using Sitecore.Data;
  using Sitecore.Form.Core.Client.Data.Submit;
  using Sitecore.Form.Core.Client.Submit;
  using Sitecore.Form.Core.Pipelines.RenderForm;
  using Sitecore.Form.Submit;
  using Sitecore.Security.Authentication;
  using Utils;

  /// <summary>
  /// The Log In form.
  /// </summary>
  public class LogIn : CreateItem, ILoad
  {
    #region Public methods

    /// <summary>
    /// The form load event handler.
    /// </summary>
    /// <param name="isPostback">Gets a value that indicates whether the form is being rendered for the first time or is being loaded in response to a postback.</param>
    /// <param name="args">The Render Form arguments.</param>
    public void Load(bool isPostback, RenderFormArgs args)
    {
      HtmlFormModifier form = new HtmlFormModifier(args);

      if (!MainUtil.IsLoggedIn())
      {
        form.ChangeFormIntroduction(string.Empty);
      }
      else
      {
        ICustomerManager<CustomerInfo> customerManager = Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
        string name = customerManager.CurrentUser.BillingAddress.Name;
        form.ChangeFormTitle(string.Format("{0} {1}.", Translate.Text(Examples.Texts.Welcome), name));
        form.HideSectionByField("Returning customer", "UserName");
        //Note: hiding of submit form button :)
        form.HideElement("div", "class", "scfSubmitButtonBorder");
      }
    }

    /// <summary>
    /// Executes the specified formid.
    /// </summary>
    /// <param name="formid">The formid.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="data">The data.</param>
    public override void Execute(ID formid, AdaptedResultList fields, object[] data)
    {
      AnalyticsUtil.AuthentificationClickedLoginButton();

      base.Execute(formid, fields, data);
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// Redirects to the login menu if the user exists.
    /// Otherwise returns an error message / exception.
    /// </summary>
    /// <param name="formid"> The formid. </param>
    /// <param name="fields"> The fields. </param>
    /// <exception cref="ValidatorException">Username or password was wrong. Please try again.</exception>
    protected override void CreateItemByFields(ID formid, AdaptedResultList fields)
    {
      AnalyticsUtil.AuthentificationClickedLoginButton();

      NameValueCollection form = new NameValueCollection();
      ActionHelper.FillFormData(form, fields, null);

      ICustomerManager<CustomerInfo> customerManager = Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
      
      if (Sitecore.Context.User.IsAuthenticated)
      {
        AuthenticationManager.Logout();
        customerManager.ResetCurrentUser();
      }
               
      string fullNickName = Sitecore.Context.Domain.GetFullName(form["UserName"]);
      bool loginResult = customerManager. LogInCustomer(fullNickName, form["Password"]);

      if (loginResult)
      {
        AnalyticsUtil.AuthentificationUserLoginSucceeded(fullNickName);

        try
        {
          string loginPath = ItemUtil.GetNavigationLinkPath("Login");
          if (!string.IsNullOrEmpty(loginPath))
          {
            HttpContext.Current.Response.Redirect(loginPath);
          }
          else
          {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
              HttpContext.Current.Response.Redirect(HttpContext.Current.Request.UrlReferrer.AbsolutePath);
            }
          }
        }
        catch (NavigateLinkNotFoundException)
        {
          HttpContext.Current.Response.Redirect(LinkManager.GetItemUrl(Sitecore.Context.Item));
        }
      }
      else
      {
        AnalyticsUtil.AuthentificationUserLoginFailed(fullNickName);
        throw new ValidatorException("Username or password was wrong. Please try again.");
      }
    }

    #endregion
  }
}