// -------------------------------------------------------------------------------------------
// <copyright file="PageMain.aspx.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.Demo
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Web.Security;
  using System.Web.UI;
  using Analytics.Components;
  using Catalogs;
  using Classes;
  using Diagnostics;
  using DomainModel.Configurations;
  using DomainModel.Users;
  using Examples;
  using Globalization;
  using Links;
  using SecurityModel;
  using Sitecore.Data.Items;
  using Sitecore.Security.Authentication;
  using Sitecore.Web;

  /// <summary>
  /// The main page code behind.
  /// </summary>
  public partial class PageMain : Page
  {
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    protected string UserName { get; set; }

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      if (this.IsPostBack)
      {
        AddToShoppingCartFromPostback(this.Request.Form);
      }

      if (Sitecore.Context.Item != null)
      {
        this.scID.Attributes["content"] = Sitecore.Context.Item.ID.ToString();
      }

      if (Sitecore.Context.Item != null &&
          Sitecore.Context.Item.Template != null &&
          Sitecore.Context.Item.Template.Name.Equals("Home"))
      {
        this.pageContainer.Attributes.Add("class", "home_page");
      }
    }

    /// <summary>
    /// Adds to shopping cart from postback.
    /// </summary>
    /// <param name="form">The form.</param>
    private static void AddToShoppingCartFromPostback(NameValueCollection form)
    {
      // firefox workaround
      var btns = new List<string>();

      foreach (var key in form.AllKeys)
      {
        if (!key.StartsWith("btn"))
        {
          continue;
        }

        var id = key.Replace(".x", string.Empty).Replace(".y", string.Empty);
        if (!btns.Contains(id))
        {
          btns.Add(id);
        }
      }

      // firefox workaround siden img knappers postes 3 ganger
      foreach (var id in btns)
      {
        var arr = id.Split('_');
        var command = string.Empty;
        var code = string.Empty;
        if (arr.Length == 3)
        {
          command = arr[1];
          code = arr[2];
        }

        // qty
        var qty = form["quantity"];
        if (string.IsNullOrEmpty(qty))
        {
          qty = "1";
        }

        if (!string.IsNullOrEmpty(qty) && !string.IsNullOrEmpty(code))
        {
          switch (command)
          {
            case "add":
              ShoppingCartWebHelper.AddToShoppingCart(code, qty);
              break;
            case "del":
              ShoppingCartWebHelper.DeleteFromShoppingCart(code);
              break;
            case "delpl":
              ShoppingCartWebHelper.DeleteProductLineFromShoppingCart(code);
              break;
          }
        }
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      // Login info
      var membershipUser = Membership.GetUser();
      if (membershipUser != null)
      {
        this.UserName = membershipUser.UserName.Replace(Sitecore.Context.Domain.Name + @"\", string.Empty);
      }

      var isLoggedIn = Utils.MainUtil.IsLoggedIn();
      this.liStatusNotLoggedIn.Visible = !isLoggedIn;
      this.liStatusLoggedIn.Visible = isLoggedIn;
      this.liMypage.Visible = isLoggedIn;
      this.btnLogIn.Visible = !isLoggedIn;
      this.btnLogOut.Visible = isLoggedIn;

      this.lblLogedInAs.Text = string.Format(Translate.Text(Sitecore.Ecommerce.Examples.Texts.YouAreLoggedInAs), this.UserName);
    }

    /// <summary>
    /// Votes the clicked.
    /// </summary>
    /// <returns></returns>
    internal static bool VoteClicked()
    {
      return NicamHelper.SafeRequest("VoteButton").Equals("VoteButton");
    }

    /// <summary>
    /// Handles the OnClick event of the btnLogIn control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnLogIn_OnClick(object sender, EventArgs e)
    {
      AnalyticsUtil.AuthentificationClickedLoginLink();

      GeneralSettings generalSettings = Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>();
      this.Response.Redirect(Utils.ItemUtil.GetItemUrl(generalSettings.MainLoginLink, true));
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

      try
      {
        if (Sitecore.Context.Item.Security.CanRead(Sitecore.Security.Accounts.User.Current))
        {
          var qs = WebUtil.GetQueryString();

          Item catalogItem = null;

          using (new SecurityDisabler())
          {
            catalogItem = Sitecore.Ecommerce.Context.Entity.Resolve<VirtualProductResolver>().ProductCatalogItem;
          }

          if (catalogItem == null)
          {
            url = LinkManager.GetItemUrl(Sitecore.Context.Item);
          }
          else
          {
            url = Sitecore.Ecommerce.Context.Entity.Resolve<VirtualProductResolver>().GetVirtualProductUrl(catalogItem, Sitecore.Context.Item);
          }

          qs = qs.TrimStart('?');
          qs = (qs != string.Empty) ? "?" + qs : string.Empty;
          url = string.Concat(url, qs);
        }
        else
        {
          url = "/";
        }
      }
      catch (Exception err)
      {
        Log.Warn(err.Message, err);
      }

      this.Response.Redirect(url);
    }
  }
}