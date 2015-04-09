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

namespace Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;  
  using System.Web.UI;
  using Examples;

  /// <summary>
  /// The main page code behind.
  /// </summary>
  public partial class PageMain : Page
  {
    /// <summary>
    /// The lock object.
    /// </summary>
    private static readonly object Lock = new object();

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
      if (IsPostBack)
      {
        AddToShoppingCartFromPostback(Request.Form);
      }
      //set value of scID mettatag
      if (Sitecore.Context.Item != null)
      {
        scID.Attributes["content"] = Sitecore.Context.Item.ID.ToString();
      }

      if (Sitecore.Context.Item != null &&
          Sitecore.Context.Item.Template != null &&
          Sitecore.Context.Item.Template.Name.Equals("Home"))
      {
        pageContainer.Attributes.Add("class", "home_page");
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
  }
}