// -------------------------------------------------------------------------------------------
// <copyright file="SiteUtils.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using IO;
  using Sitecore.Data.Items;
  using Sites;

  /// <summary>
  /// Site utils
  /// </summary>
  public class SiteUtils
  {
    /// <summary>
    /// Site root path
    /// </summary>
    private static readonly string RootPathAttribute = "rootPath";

    /// <summary>
    /// Site Start item path
    /// </summary>
    private static readonly string StartItemAttributte = "startItem";

    /// <summary>
    /// Ecommerce Site Settings
    /// </summary>
    private static readonly string EcommerceSiteSettingsAttribute = "EcommerceSiteSettings";

    /// <summary>
    /// Ecxlude sites 
    /// </summary>
    private static readonly string ExcludeSites = "|shell|login|testing|admin|modules_shell|modules_website|scheduler|system|publisher|";

    /// <summary>
    /// Site name attribute
    /// </summary>
    private static readonly string SiteNameAttribute = "name";

    /// <summary>
    /// Gets the site root.
    /// </summary>
    /// <param name="name">The site name.</param>
    /// <returns>return site root path</returns>
    public static string GetSiteRoot(string name)
    {
      return GetSiteRoot(SiteManager.GetSite(name));
    }

    /// <summary>
    /// Gets the site root.
    /// </summary>
    /// <param name="site">The site to proceed</param>
    /// <returns>returns site root</returns>
    public static string GetSiteRoot(Site site)
    {
      return FileUtil.MakePath(site.Properties[RootPathAttribute], site.Properties[StartItemAttributte]);
    }

    /// <summary>
    /// Gets the site by item.
    /// </summary>
    /// <param name="item">The item to proceed</param>
    /// <returns>Returns site name by item</returns>
    public static string GetSiteByItem(Item item)
    {
      Assert.IsNotNull(item, "item");

      return GetSiteByItemPath(item.Paths.FullPath, SiteManager.GetSites());
    }

    /// <summary>
    /// Gets the site by item path.
    /// </summary>
    /// <param name="itemFullPath">The item full path.</param>
    /// <returns>Returns site name by item path</returns>
    public static string GetSiteByItemPath(string itemFullPath)
    {
      return GetSiteByItemPath(itemFullPath, SiteManager.GetSites());
    }
    
    /// <summary>
    /// Gets the site by item path.
    /// </summary>
    /// <param name="itemFullPath">The item full path.</param>
    /// <param name="sites">The sites.</param>
    /// <returns>Returns item site name</returns>
    public static string GetSiteByItemPath(string itemFullPath, IEnumerable<Site> sites)
    {
      Dictionary<string, int> matchedSites = new Dictionary<string, int>();
      foreach (Site site in sites)
      {
        string siteName = site.Properties[SiteNameAttribute];

        if (!string.IsNullOrEmpty(site.Properties[EcommerceSiteSettingsAttribute]) && !ExcludeSites.Contains(string.Format("|{0}|", siteName)))
        {
          string siteRoot = GetSiteRoot(site);
          if (itemFullPath.StartsWith(siteRoot, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(siteRoot))
          {
            string[] siteRootItems = siteRoot.Split('/');
            string[] itemPathItems = itemFullPath.Split('/');
            int lastRootItemIndex = siteRootItems.Length - 1;
            if (string.Compare(siteRootItems[lastRootItemIndex], itemPathItems[lastRootItemIndex], StringComparison.InvariantCultureIgnoreCase) == 0)
            {
              matchedSites.Add(siteName, lastRootItemIndex);
            }
          }
        }
      }

      string result = (from item in matchedSites
                       where item.Value == matchedSites.Max(p => p.Value)
                       select item.Key).FirstOrDefault();
      return result;
    }
  }
}