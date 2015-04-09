// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentContextSwitcherDataSource.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines ContentContextSwitcherDataSource type.
// </summary>
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.DataSources
{
  using System;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines ContentContextSwitcherDataSource type.
  /// </summary>
  public class ContentContextSwitcherDataSource : ContextSwitcherDataSourceBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentContextSwitcherDataSource" /> class.
    /// </summary>
    /// <param name="shopContextFactory">The shop context factory.</param>
    public ContentContextSwitcherDataSource(ShopContextFactory shopContextFactory)
      : base(shopContextFactory)
    {
    }

    /// <summary>
    /// Gets the web shops.
    /// </summary>
    /// <returns>
    /// The web shops.
    /// </returns>
    public override ContextItemCollection Select()
    {
      var result = new ContextItemCollection();

      if (string.IsNullOrEmpty(this.Source))
      {
        return result;
      }

      var itemUri = ItemUri.Parse(this.Source);
      var rootItem = Database.GetItem(itemUri);

      var webShops = this.ShopContextFactory.GetWebShops().Select(ws => ws.InnerSite.Name);
      foreach (Item item in rootItem.Children.Where(c => webShops.Any(ws => string.Compare(ws, c["Name"], StringComparison.OrdinalIgnoreCase) == 0)))
      {
        result.Add(new ContextItem { Name = item["Name"], Title = item["Title"], Tooltip = item["Tooltip"], Icon = item["Icon"] });
      }

      return result;
    }
  }
}