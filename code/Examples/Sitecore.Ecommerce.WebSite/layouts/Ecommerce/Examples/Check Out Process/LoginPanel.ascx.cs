// -------------------------------------------------------------------------------------------
// <copyright file="LoginPanel.ascx.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess
{
  using System;
  using System.Web.Security;
  using System.Web.UI;
  using Analytics.Components;
  using Catalogs;
  using DomainModel.Configurations;
  using DomainModel.Users;
  using Links;
  using Sitecore.Globalization;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using Sitecore.Web;
  using Unity;
  using Utils;

  /// <summary>
  /// Login panel
  /// </summary>
  public partial class LoginPanel : UserControl
  {
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    protected string UserName { get; set; }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      var membershipUser = Membership.GetUser();
      if (membershipUser != null)
      {
        this.UserName = membershipUser.UserName.Replace(Sitecore.Context.Domain.Name + @"\", string.Empty);
      }

      var isLoggedIn = MainUtil.IsLoggedIn();
      this.liStatusNotLoggedIn.Visible = !isLoggedIn;
      this.liStatusLoggedIn.Visible = isLoggedIn;
      this.liMypage.Visible = isLoggedIn;
      this.btnLogIn.Visible = !isLoggedIn;
      this.btnLogOut.Visible = isLoggedIn;

      GeneralSettings generalSettings = Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>();
      this.btnLogIn.HRef = ItemUtil.GetItemUrl(generalSettings.MainLoginLink, true);

      this.lblLogedInAs.Text = string.Format(Translate.Text(Sitecore.Ecommerce.Examples.Texts.YouAreLoggedInAs), this.UserName);
    }

    /// <summary>
    /// Handles the OnClick event of the btnLogOut control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnLogOut_OnClick(object sender, EventArgs e)
    {
      ICustomerManager<CustomerInfo> customerManager = Sitecore.Ecommerce.Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
      AnalyticsUtil.AuthentificationUserLoggedOut(customerManager.CurrentUser.NickName);

      AuthenticationManager.Logout();

      customerManager.ResetCurrentUser();

      var url = string.Empty;

      if (Sitecore.Context.Item.Security.CanRead(User.Current))
      {
        var qs = WebUtil.GetQueryString();

        var itemUrl = Sitecore.Context.ClientData.GetValue("itempath") as string;
        if (string.IsNullOrEmpty(itemUrl))
        {
          url = LinkManager.GetItemUrl(Sitecore.Context.Item);
        }
        else
        {
          VirtualProductResolver virtualProductResolver = Sitecore.Ecommerce.Context.Entity.Resolve<VirtualProductResolver>();
          var folderItem = virtualProductResolver.ProductCatalogItem;
          if (folderItem != null)
          {
            url = virtualProductResolver.GetVirtualProductUrl(folderItem, Sitecore.Context.Item);
          }
        }

        qs = qs.TrimStart('?');
        qs = (qs != string.Empty) ? "?" + qs : string.Empty;
        url = string.Concat(url, qs);
      }
      else
      {
        url = "/";
      }

      Response.Redirect(url);
    }
  }
}