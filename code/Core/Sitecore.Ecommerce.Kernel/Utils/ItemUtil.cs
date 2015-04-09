// -------------------------------------------------------------------------------------------
// <copyright file="ItemUtil.cs" company="Sitecore Corporation">
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
  using System.Web;
  using Globalization;
  using Layouts;
  using Links;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;

  /// <summary>
  /// Util class for eCommerce module
  /// </summary>
  public static class ItemUtil
  {
    /// <summary> Gets the first ascendant or self with value from item. </summary>
    /// <param name="field"> The field name </param>
    /// <param name="item"> The item to proceed </param>
    /// <returns>returns value from item </returns>
    public static string GetFirstAscendantOrSelfWithValueFromItem(string field, Item item)
    {
      while (item != null && item[field] == string.Empty)
      {
        item = item.Parent;
      }

      return item != null ? item[field] : string.Empty;
    }

    /// <summary> Gets the title or dictionary entry if title is empty </summary>
    /// <param name="item"> The item. </param>
    /// <param name="isEditModeEnabled"> if set to <c>true</c> [is edit mode enabled]. </param>
    /// <returns>returns title or dictionary entry </returns>
    public static string GetTitleOrDictionaryEntry(Item item, bool isEditModeEnabled)
    {
      string title = item["Title"];
      return title;
    }

    /// <summary> Gets a navigation link item wich is descendant of current sitecore item </summary>
    /// <param name="key"> The key to proceed </param>
    /// <returns>returns navigation link item</returns>
    public static Item GetNavigationLinkItem(string key)
    {
      return GetNavigationLinkItem(key, Sitecore.Context.Item);
    }

    /// <summary> Gets the navigation link item wich is descendant of selected sitecore item </summary>
    /// <param name="key"> The key to prcceed </param>
    /// <param name="item"> The selected sitecore item. </param>
    /// <returns>returns item </returns>
    public static Item GetNavigationLinkItem(string key, Item item)
    {
      Item navigationLinkitem = item.Axes.SelectSingleItem(string.Format("./*[@@templatename='Navigation Links']/*[@key='{0}']", key));
      return navigationLinkitem;
    }

    /// <summary>
    /// Gets the navigation link title.
    /// </summary>
    /// <param name="navigationLinkItem">
    /// The navigation link item.
    /// </param>
    /// <returns>
    /// </returns>
    public static string GetNavigationLinkTitle(Item navigationLinkItem)
    {
      return GetNavigationLinkTitle(navigationLinkItem, false);
    }

    /// <summary> Gets the navigation link title. </summary>
    /// <param name="key"> The key. </param>
    /// <param name="isEditModeEnabled"> if set to <c>true</c> [is edit mode enabled]. </param>
    /// <returns>returns navigation link title </returns>
    public static string GetNavigationLinkTitle(string key, bool isEditModeEnabled)
    {
      Item navigationLinkItem = GetNavigationLinkItem(key);
      if (navigationLinkItem != null)
      {
        return GetNavigationLinkTitle(navigationLinkItem, isEditModeEnabled);
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets the navigation link title.
    /// </summary>
    /// <param name="navigationLinkItem">
    /// The navigation link item.
    /// </param>
    /// <param name="isEditModeEnabled">
    /// if set to <c>true</c> [is edit mode enabled].
    /// </param>
    /// <returns>
    /// </returns>
    public static string GetNavigationLinkTitle(Item navigationLinkItem, bool isEditModeEnabled)
    {
      return GetTitleOrDictionaryEntry(navigationLinkItem, isEditModeEnabled);
    }

    /// <summary>
    /// Gets the navigation link path.
    /// </summary>
    /// <param name="key">
    /// The key.
    /// </param>
    /// <param name="includeQuery">
    /// if set to <c>true</c> [include query].
    /// </param>
    /// <returns>
    /// </returns>
    public static string GetNavigationLinkPath(string key, bool includeQuery)
    {
      Item navigationLinkItem = GetNavigationLinkItem(key);

      if (navigationLinkItem != null)
      {
        return GetNavigationLinkPath(navigationLinkItem, includeQuery);
      }

      return null;
    }

    /// <summary>
    /// Gets the navigation link path.
    /// </summary>
    /// <param name="navigationLinkItem">
    /// The navigation link item.
    /// </param>
    /// <param name="includeQuery">
    /// if set to <c>true</c> [include query].
    /// </param>
    /// <returns>
    /// </returns>
    public static string GetNavigationLinkPath(Item navigationLinkItem, bool includeQuery)
    {
      LinkField generalLink = navigationLinkItem.Fields["Link"];
      if (generalLink != null)
      {
        if (generalLink.TargetItem != null)
        {
          if (includeQuery)
          {
            return LinkManager.GetItemUrl(generalLink.TargetItem) + HttpContext.Current.Request.Url.Query;
          }

          return LinkManager.GetItemUrl(generalLink.TargetItem);
        }
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets the navigation link path.
    /// </summary>
    /// <param name="key">The key of the link.</param>
    /// <returns>the navigation link path.</returns>
    public static string GetNavigationLinkPath(string key)
    {
      return GetNavigationLinkPath(key, true);
    }

    /// <summary>
    /// Gets the navigation link path.
    /// </summary>
    /// <param name="navigationLinkItem">The navigation link item.</param>
    /// <returns>the navigation link path.</returns>
    public static string GetNavigationLinkPath(Item navigationLinkItem)
    {
      return GetNavigationLinkPath(navigationLinkItem, true);
    }

    /// <summary>
    /// Gets the link to an item from item ID
    /// </summary>
    /// <param name="itemID">
    /// The item Id.
    /// </param>
    /// <param name="addQueryString">
    /// Whether add the query string.
    /// </param>
    /// <returns>
    /// The item url for website.
    /// </returns>
    public static string GetItemUrl(string itemID, bool addQueryString)
    {
      if (!string.IsNullOrEmpty(itemID))
      {
        Item item = Sitecore.Context.Database.GetItem(itemID);
        if (item != null)
        {
          string itemUrl = LinkManager.GetItemUrl(item);
          return addQueryString ? string.Format("{0}{1}", itemUrl, HttpContext.Current.Request.Url.Query) : itemUrl;
        }
      }

      return String.Empty;
    }

    /// <summary>
    /// Gets the item full URL.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="addQueryString">if set to <c>true</c> [add query string].</param>
    /// <returns>The item full url</returns>
    public static string GetItemFullUrl(string itemID, bool addQueryString)
    {
      if (!string.IsNullOrEmpty(itemID))
      {
        Item item = Sitecore.Context.Database.GetItem(itemID);
        if (item != null)
        {
          string itemUrl = item.Paths.FullPath;
          return addQueryString ? string.Format("{0}{1}", itemUrl, HttpContext.Current.Request.Url.Query) : itemUrl;
        }
      }

      return String.Empty;
    }

    /// <summary>
    /// Chekcs if current sitecore item has print layout
    /// </summary>
    /// <returns>The result.</returns>
    public static bool CurrentItemHasPrintLayout()
    {
      Item item = Sitecore.Context.Item;
      if (item == null)
      {
        return false;
      }

      Item print = Sitecore.Context.Device.InnerItem.Axes.SelectSingleItem("../*[@@name='Print']");
      if (print != null)
      {
        var device = new DeviceItem(print);
        RenderingReference[] renderings = item.Visualization.GetRenderings(device, false);
        return renderings != null && renderings.Length != 0;
      }

      return false;
    }

    /// <summary>
    /// Redirects to navigation link.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="includeQuery">if set to <c>true</c> [include query].</param>
    public static void RedirectToNavigationLink(string key, bool includeQuery)
    {
      string path = GetNavigationLinkPath(key, includeQuery);
      if (!string.IsNullOrEmpty(path) && HttpContext.Current != null)
      {
        HttpContext.Current.Response.Redirect(path);
      }
    }
  }
}