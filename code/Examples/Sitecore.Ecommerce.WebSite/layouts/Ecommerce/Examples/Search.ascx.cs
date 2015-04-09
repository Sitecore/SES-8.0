// -------------------------------------------------------------------------------------------
// <copyright file="Search.ascx.cs" company="Sitecore Corporation">
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
  using System.Web.UI;
  using Classes;
  using Globalization;

  public partial class Search : UserControl
  {
    private readonly string keywords = Translate.Text(Sitecore.Ecommerce.Examples.Texts.Keywords);

    private const string onblur = "javascript: if (document.getElementById('{0}').value=='{1}' || document.getElementById('{0}').value=='' ) {2} document.getElementById('{0}').value='{1}'; {3}";

    private const string onfocus = "javascript: if (document.getElementById('{0}').value=='{1}') {2}  document.getElementById('{0}').value=''; {3}";

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        BindJavaScript();
      }
      if (SearchClicked())
      {
        SimpleSearch();
      }
    }

    private void SimpleSearch()
    {
      var searchKeywords = headerSearch.Value;
      if (searchKeywords != keywords)
      {
        var url = NicamHelper.RedirectUrl(Consts.SearchResultPage, "search", searchKeywords);
        // Search Result Page
        Response.Redirect(url);
      }
    }

    private static bool SearchClicked()
    {
      return NicamHelper.SafeRequest("SearchButton").Equals("SearchButton");
    }

    private void BindJavaScript()
    {
      headerSearch.Value = keywords;
      headerSearch.Attributes["onfocus"] = string.Format(onfocus, headerSearch.ClientID, keywords, "{", "}");
      headerSearch.Attributes["onblur"] = string.Format(onblur, headerSearch.ClientID, keywords, "{", "}");
    }
  }
}