// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopContextFactoryContextSwitcherDataSource.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The ShopContextFactoryContextSwitcherDataSource type.
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
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// The context items provider.
  /// </summary>
  public class ShopContextFactoryContextSwitcherDataSource : ContextSwitcherDataSourceBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ShopContextFactoryContextSwitcherDataSource" /> class.
    /// </summary>
    /// <param name="shopContextFactory">The shop context factory.</param>
    public ShopContextFactoryContextSwitcherDataSource(ShopContextFactory shopContextFactory)
      : base(shopContextFactory)
    {
    }

    /// <summary>
    /// Gets the context items.
    /// </summary>
    /// <returns>The collection of the context items.</returns>
    public override ContextItemCollection Select()
    {
      var result = new ContextItemCollection();
      foreach (ShopContext shopContext in this.ShopContextFactory.GetWebShops())
      {
        result.Add(
          new ContextItem
            {
              Name = shopContext.InnerSite.Name,
              Title = shopContext.InnerSite.BrowserTitle,
              Tooltip = shopContext.InnerSite.BrowserTitle
            });
      }

      return result;
    }
  }
}