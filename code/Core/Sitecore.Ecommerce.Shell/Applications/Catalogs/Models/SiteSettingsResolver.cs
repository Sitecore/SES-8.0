// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteSettingsResolver.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Resolve Business Catalog settings for a site using any site item URI.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models
{
  using Data;
  using Diagnostics;
  using DomainModel.Configurations;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sites;
  using Utils;

  /// <summary>
  /// Resolve Business Catalog settings for a site using any site item URI.
  /// </summary>
  public class SiteSettingsResolver
  {
    /// <summary>
    /// Gets the site settings by a site item URI.
    /// </summary>
    /// <param name="siteItemUri">The site item URI.</param>
    /// <returns>
    /// The site settings.
    /// </returns>
    [NotNull]
    public virtual BusinessCatalogSettings GetSiteSettings([NotNull] ItemUri siteItemUri)
    {
      Assert.ArgumentNotNull(siteItemUri, "siteItemUri");

      Item contextItem = Database.GetItem(siteItemUri);
      Assert.IsNotNull(contextItem, "Unable to resolve catalog's context item.");

      string siteName = SiteUtils.GetSiteByItem(contextItem);
      Assert.IsNotNullOrEmpty(siteName, "Unable to resolve catalog's website context.");

      SiteContext site = SiteContextFactory.GetSiteContext(siteName);
      Assert.IsNotNull(site, "Unable to resolve catalog's website context.");

      BusinessCatalogSettings settings;
      Database database = Configuration.Factory.GetDatabase(siteItemUri.DatabaseName);
      using (new SiteContextSwitcher(site))
      {
        using (new SiteIndependentDatabaseSwitcher(database))
        {
          settings = Context.Entity.GetConfiguration<BusinessCatalogSettings>();
        }
      }

      return settings;
    }
  }
}