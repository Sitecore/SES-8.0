// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MerchantShopResolver.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the merchant shop resolver class.
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

namespace Sitecore.Ecommerce.Apps.Pipelines.HttpRequest
{
  using System;

  using Diagnostics;

  using Ecommerce.OrderManagement.Orders;

  using Logging;

  using Merchant.OrderManagement;

  using Microsoft.Practices.Unity;
  using Sitecore.Ecommerce.Apps.Security;
  using Sitecore.Pipelines;
  using Sites;

  /// <summary>
  /// Defines the merchant shop resolver class.
  /// </summary>
  public class MerchantShopResolver
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MerchantShopResolver"/> class.
    /// </summary>
    public MerchantShopResolver()
    {
      this.ShopConfiguration = Context.Entity.Resolve<ShopConfiguration>();
      this.SiteContextFactory = Context.Entity.Resolve<SiteContextFactoryWrapper>();
    }

    /// <summary>
    /// Gets or sets the name of the merchant site.
    /// </summary>
    /// <value>
    /// The name of the merchant site.
    /// </value>
    [NotNull]
    public string MerchantSiteName { get; set; }

    /// <summary>
    /// Gets or sets the name of the shop context.
    /// </summary>
    /// <value>
    /// The name of the shop context.
    /// </value>
    [NotNull]
    public string ShopContextName { get; set; }

    /// <summary>
    /// Gets or sets the shop configuration.
    /// </summary>
    /// <value>
    /// The shop configuration.
    /// </value>
    [NotNull]
    public ShopConfiguration ShopConfiguration { get; set; }

    /// <summary>
    /// Gets or sets the site context factory.
    /// </summary>
    /// <value>
    /// The site context factory.
    /// </value>
    [NotNull]
    public SiteContextFactoryWrapper SiteContextFactory { get; set; }

    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The args.</param>
    public virtual void Process([NotNull] PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      SiteContext contextSite = Sitecore.Context.Site;
      if (contextSite == null || string.Compare(this.MerchantSiteName, Sitecore.Context.Site.Name, StringComparison.OrdinalIgnoreCase) != 0)
      {
        return;
      }

      string storedSiteName = Sitecore.Context.User.Profile.GetSelectedShopContext();

      SiteContext site = null;
      if (!string.IsNullOrEmpty(storedSiteName))
      {
        site = this.SiteContextFactory.GetSiteContext(storedSiteName);
      }

      if (site == null && !string.IsNullOrEmpty(this.ShopContextName))
      {
        site = this.SiteContextFactory.GetSiteContext(this.ShopContextName);
        if (site != null)
        {
          Sitecore.Context.User.Profile.SetDefaultShopContext(this.ShopContextName);
          Sitecore.Context.User.Profile.Save();
        }
      }

      if (string.IsNullOrEmpty(this.ShopContextName))
      {
        Sitecore.Context.User.Profile.SetDefaultShopContext(string.Empty);
        Sitecore.Context.User.Profile.Save();
      }

      if (site != null)
      {
        var database = site.ContentDatabase;
        ShopContext shop = new ShopContext(site)
        {
          Database = database,
          BusinessCatalogSettings = this.ShopConfiguration.GetBusinesCatalogSettings(site, database),
          GeneralSettings = this.ShopConfiguration.GetGeneralSettings(site, database)
        };

        Context.Entity.RegisterInstance(typeof(ShopContext), null, shop, new HierarchicalLifetimeManager());
      }
      else
      {
        Context.Entity.RegisterInstance(typeof(LoggingProvider), null, new EmptyLoggingProvider(), new HierarchicalLifetimeManager());
        Context.Entity.RegisterInstance(typeof(Data.Repository<Order>), null, new EmptyOrderRepository(), new HierarchicalLifetimeManager());
      }
    }
  }
}