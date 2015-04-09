// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellShopResolver.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the shell shop resolver class.
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

namespace Sitecore.Ecommerce.Shell.Pipelines.HttpRequest
{
  using System;
  using System.Linq;
  using System.Web;
  using Microsoft.Practices.Unity;
  using Sitecore.Data;
  using Sitecore.Pipelines;
  using Sitecore.Sites;

  /// <summary>
  /// Defines the shell shop resolver class.
  /// </summary>
  public class ShellShopResolver
  {
    /// <summary>
    ///  The HttpContextBase instance.
    /// </summary>
    private HttpContextBase httpContext;

    /// <summary>
    /// The shop context factory.
    /// </summary>
    private ShopContextFactory shopContextFactory;

    /// <summary>
    /// The shop configuration.
    /// </summary>
    private ShopConfiguration shopConfiguration;

    /// <summary>
    /// The shop context switcher.
    /// </summary>
    private ShopContextSwitcher shopContextSwitcher;

    /// <summary>
    /// Gets or sets the HTTP context.
    /// </summary>
    /// <value>The HTTP context.</value>
    [NotNull]
    public HttpContextBase HttpContext
    {
      get
      {
        if (this.httpContext == null)
        {
          HttpContext context = System.Web.HttpContext.Current;
          this.httpContext = new HttpContextWrapper(context);
        }

        return this.httpContext;
      }

      set
      {
        this.httpContext = value;
      }
    }

    /// <summary>
    /// Gets or sets the shop context factory.
    /// </summary>
    /// <value>
    /// The shop context factory.
    /// </value>
    [NotNull]
    public ShopContextFactory ShopContextFactory
    {
      get { return this.shopContextFactory ?? (this.shopContextFactory = Context.Entity.Resolve<ShopContextFactory>()); }
      set { this.shopContextFactory = value; }
    }

    /// <summary>
    /// Gets or sets the shop configuration.
    /// </summary>
    /// <value>
    /// The shop configuration.
    /// </value>
    [NotNull]
    public ShopConfiguration ShopConfiguration
    {
      get { return this.shopConfiguration ?? (this.shopConfiguration = Context.Entity.Resolve<ShopConfiguration>()); }
      set { this.shopConfiguration = value; }
    }

    /// <summary>
    /// Gets or sets the name of the shell site.
    /// </summary>
    /// <value>
    /// The name of the shell site.
    /// </value>
    [NotNull]
    public string ShellSiteName { get; set; }

    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public void Process(PipelineArgs args)
    {
      this.ResetHttpContext();

      SiteContext site = Sitecore.Context.Site;
      if (site == null || string.Compare(this.ShellSiteName, site.Name, StringComparison.OrdinalIgnoreCase) != 0)
      {
        return;
      }

      var queryString = this.HttpContext.Request.QueryString;

      string itemId = queryString["id"] ?? queryString["sc_itemid"];
      if (string.IsNullOrEmpty(itemId) || !ID.IsID(itemId))
      {
        return;
      }

      string databaseOverrideName = queryString["database"] ?? queryString["db"] ?? queryString["sc_database"];
      Database databaseOverride = null;

      if (!string.IsNullOrEmpty(databaseOverrideName))
      {
        databaseOverride = Database.GetDatabase(databaseOverrideName);
      }

      var shop = this.ShopContextFactory.GetWebShops().FirstOrDefault(sh => this.CheckIfCurrentShopContext(sh, databaseOverride, ID.Parse(itemId)));
      if (shop == null)
      {
        return;
      }

      shop.GeneralSettings = this.ShopConfiguration.GetGeneralSettings(shop.InnerSite, shop.Database);
      shop.BusinessCatalogSettings = this.ShopConfiguration.GetBusinesCatalogSettings(shop.InnerSite, shop.Database);

      // INFO: looks like using factories here is an overdesign. Simple instantiation is used instead.
      this.shopContextSwitcher = new ShopContextSwitcher(shop);
    }

    /// <summary>
    /// Checks if current shop context.
    /// </summary>
    /// <param name="shopContext">The shop context.</param>
    /// <param name="database">The database.</param>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>True if current shop context, otherwise false.</returns>
    private bool CheckIfCurrentShopContext(ShopContext shopContext, Database database, ID itemId)
    {
      if (database != null)
      {
        shopContext.Database = database;
      }

      return this.CheckIfCurrentSiteContextByHomePath(shopContext, itemId)
        || this.CheckIfCurrentSiteContextByContentPath(shopContext, itemId);
    }

    /// <summary>
    /// Checks if current site context by home path.
    /// </summary>
    /// <param name="shopContext">The shop context.</param>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>True if current shop context, otherwise false.</returns>
    private bool CheckIfCurrentSiteContextByHomePath(ShopContext shopContext, ID itemId)
    {
      return this.CheckIfItemIsDescendantOfSiteContextRoot(shopContext, itemId, shopContext.InnerSite.RootPath + shopContext.InnerSite.StartItem);
    }

    /// <summary>
    /// Checks if current site context by content path.
    /// </summary>
    /// <param name="shopContext">The shop context.</param>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>True if current shop context, otherwise false.</returns>
    private bool CheckIfCurrentSiteContextByContentPath(ShopContext shopContext, ID itemId)
    {
      return this.CheckIfItemIsDescendantOfSiteContextRoot(shopContext, itemId, shopContext.InnerSite.RootPath);
    }

    /// <summary>
    /// Checks if item is descendant of site context root.
    /// </summary>
    /// <param name="shopContext">The shop context.</param>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="rootPath">The root path.</param>
    /// <returns>True if current shop context, otherwise false.</returns>
    private bool CheckIfItemIsDescendantOfSiteContextRoot(ShopContext shopContext, ID itemId, string rootPath)
    {
      if (shopContext.Database == null)
      {
        return false;
      }

      var item = shopContext.Database.GetItem(itemId);
      var rootItem = shopContext.Database.GetItem(rootPath);
      return item != null && rootItem != null && (rootItem.Axes.IsAncestorOf(item) || rootItem.ID == itemId);
    }

    /// <summary>
    /// Resets the HTTP context.
    /// </summary>
    private void ResetHttpContext()
    {
      if (System.Web.HttpContext.Current != null)
      {
        if (this.HttpContext.Request.RawUrl != System.Web.HttpContext.Current.Request.RawUrl)
        {
          this.httpContext = null;
        }
      }
    }
  }
}