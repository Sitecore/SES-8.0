// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Consts.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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

namespace Sitecore.Ecommerce
{
  /// <summary>
  /// The static readonlys.
  /// </summary>
  public static class Consts
  {
    #region Consts

    /// <summary>
    /// The languages root path.
    /// </summary>
    private static readonly string LANGUAGES_ROOT_PATH = "/sitecore/system/Languages";

    /// <summary>
    /// The menu title fieldname.
    /// </summary>
    private static readonly string MENU_TITLE_FIELDNAME = "Menu Title";

    /// <summary>
    /// The review folder name.
    /// </summary>
    private static readonly string REVIEW_FOLDER_NAME = "Reviews";

    /// <summary>
    /// The review folder template name.
    /// </summary>
    private static readonly string REVIEW_FOLDER_TEMPLATE_NAME = "ReviewFolder";

    /// <summary>
    /// The review item template name.
    /// </summary>
    private static readonly string REVIEW_ITEM_TEMPLATE_NAME = "ProductReview";

    /// <summary>
    /// The search result page.
    /// </summary>
    private static readonly string SEARCH_RESULT_PAGE = "/Functions/SearchResult.aspx";

    /// <summary>
    /// The spot links root.
    /// </summary>
    private static readonly string SPOT_LINKS_ROOT = "/sitecore/content/Globals/Spot links";

    /// <summary>
    /// The templates path.
    /// </summary>
    private static readonly string TEMPLATES_PATH = "User Defined/Nicam";

    /// <summary>
    /// The templates root path.
    /// </summary>
    private static readonly string TEMPLATES_ROOT_PATH = "User Defined/Nicam/Products";

    #endregion Consts

    #region Properties

    /// <summary>
    /// The extranet role.
    /// </summary>
    public static string ExtranetRole
    {
      get
      {
        return Sitecore.Context.Domain.Name + @"\users";
      }
    }

    /// <summary>
    /// The languages root path.
    /// </summary>
    public static string LanguagesRootPath
    {
      get
      {
        return LANGUAGES_ROOT_PATH;
      }
    }

    /// <summary>
    /// The menu title fieldname.
    /// </summary>
    public static string MenuTitleFieldname
    {
      get
      {
        return MENU_TITLE_FIELDNAME;
      }
    }

    /// <summary>
    /// The review folder name.
    /// </summary>
    public static string ReviewFolderName
    {
      get
      {
        return REVIEW_FOLDER_NAME;
      }
    }

    /// <summary>
    /// The review folder template name.
    /// </summary>
    public static string ReviewFolderTemplateName
    {
      get
      {
        return REVIEW_FOLDER_TEMPLATE_NAME;
      }
    }

    /// <summary>
    /// The review item template name.
    /// </summary>
    public static string ReviewItemTemplateName
    {
      get
      {
        return REVIEW_ITEM_TEMPLATE_NAME;
      }
    }

    /// <summary>
    /// The search result page.
    /// </summary>
    public static string SearchResultPage
    {
      get
      {
        return SEARCH_RESULT_PAGE;
      }
    }

    /// <summary>
    /// The spot links root.
    /// </summary>
    public static string SpotLinksRoot
    {
      get
      {
        return SPOT_LINKS_ROOT;
      }
    }

    /// <summary>
    /// The templates path.
    /// </summary>
    public static string TemplatesPath
    {
      get
      {
        return TEMPLATES_PATH;
      }
    }

    /// <summary>
    /// The templates root path.
    /// </summary>
    public static string TemplatesRootPath
    {
      get
      {
        return TEMPLATES_ROOT_PATH;
      }
    }

    #endregion Properties
  }
}