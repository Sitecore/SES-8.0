// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextSwitcherDataSourceBase.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ContextSwitcherDataSourceBase type.
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
  /// The context switcher data source base.
  /// </summary>
  public abstract class ContextSwitcherDataSourceBase
  {
    /// <summary>
    /// The shop context factory.
    /// </summary>
    private readonly ShopContextFactory shopContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContextSwitcherDataSourceBase" /> class.
    /// </summary>
    /// <param name="shopContextFactory">The shop context factory.</param>
    protected ContextSwitcherDataSourceBase(ShopContextFactory shopContextFactory)
    {
      this.shopContextFactory = shopContextFactory;
    }

    /// <summary>
    /// Gets the shop context factory.
    /// </summary>
    /// <value>
    /// The shop context factory.
    /// </value>
    public ShopContextFactory ShopContextFactory
    {
      get { return this.shopContextFactory; }
    }

    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    /// <value>The source.</value>
    public string Source { get; set; }

    /// <summary>
    /// The select.
    /// </summary>
    /// <returns>
    /// The <see cref="ContextItemCollection"/>.
    /// </returns>
    public abstract ContextItemCollection Select();
  }
}