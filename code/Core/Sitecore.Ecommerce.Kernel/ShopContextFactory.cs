// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopContextFactory.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the shop context factory class.
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

namespace Sitecore.Ecommerce
{
  using System.Collections.Generic;
  using System.Linq;

  using Sites;

  /// <summary>
  /// Defines the shop context factory class.
  /// </summary>
  public class ShopContextFactory
  {
    /// <summary>
    /// The ecommerce site settings key.
    /// </summary>
    public const string EcommerceSiteSettingsKey = "EcommerceSiteSettings";

    /// <summary>
    /// The site context factory.
    /// </summary>
    private readonly SiteContextFactoryWrapper siteContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShopContextFactory"/> class.
    /// </summary>
    public ShopContextFactory()
      : this(new SiteContextFactoryWrapper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShopContextFactory"/> class.
    /// </summary>
    /// <param name="siteContextFactory">The site context factory.</param>
    public ShopContextFactory(SiteContextFactoryWrapper siteContextFactory)
    {
      this.siteContextFactory = siteContextFactory;
    }

    /// <summary>
    /// Gets the web shops.
    /// </summary>
    /// <returns>
    /// The web shops.
    /// </returns>
    public virtual IEnumerable<ShopContext> GetWebShops()
    {
      return this.siteContextFactory.Sites.Where(si => si.Properties[EcommerceSiteSettingsKey] != null).Select(si => new ShopContext(new SiteContext(si)));
    }
  }
}