// -------------------------------------------------------------------------------------------
// <copyright file="Product.ascx.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.layouts.Ecommerce
{
  using System;
  using System.Web;
  using System.Web.UI;
  using Utils;

  /// <summary>
  /// The product detailed inforamtion page.
  /// </summary>
  public partial class Product : UserControl
  {
    /// <summary>
    /// Gets the name of the get product tab.
    /// </summary>
    /// <value>The name of the get product tab.</value>
    private static string GetProductTabName
    {
      get
      {
        return HttpContext.Current != null && HttpContext.Current.Session["EcProductTabName"] != null
                       ? HttpContext.Current.Session["EcProductTabName"].ToString()
                       : "Specifications";
      }
    }

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        // clears EcProductTabName on first page load
        Session["EcProductTabName"] = null;

        // redirect to default variant if current product has a default variant selected
        if (Sitecore.Context.Item != null && !string.IsNullOrEmpty(Sitecore.Context.Item["Default variant"]))
        {
          var item = Sitecore.Context.Database.GetItem(Sitecore.Context.Item["Default variant"]);
          if (item != null)
          {
            var url = Links.LinkManager.GetItemUrl(item);
            Response.Redirect(url);
          }
        }
      }

      // gets tabname from session if avalible
      var tabName = (Session["EcProductTabName"] == null) ? "Specifications" : Session["EcProductTabName"].ToString();

      // loads xsl rendering into TabContent div
      this.tabContent.InnerHtml = MainUtil.LoadRendering("Ecommerce/Examples/ProductTab" + tabName);
    }

    /// <summary>
    /// Gets the product tab CSS class.
    /// </summary>
    /// <param name="tabName">Name of the tab.</param>
    /// <returns>The product tab CSS class.</returns>
    protected string GetProductTabCSSClass(string tabName)
    {
      return tabName.Equals(GetProductTabName) ? "current" : string.Empty;
    }
  }
}