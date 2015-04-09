// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopConfiguration.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the shop configuration class.
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
  using Configurations;
  using Data;
  using DomainModel.Data;
  using IO;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sites;

  /// <summary>
  /// Defines the shop configuration class.
  /// </summary>
  public class ShopConfiguration
  {
    /// <summary>
    /// The siteContext settings key.
    /// </summary>
    public const string EcommerceSiteSettingsKey = "EcommerceSiteSettings";

    /// <summary>
    /// The data mapper.
    /// </summary>
    private readonly IDataMapper dataMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShopConfiguration" /> class.
    /// </summary>
    /// <param name="dataMapper">The data mapper.</param>
    public ShopConfiguration(IDataMapper dataMapper)
    {
      Assert.ArgumentNotNull(dataMapper, "dataMapper");

      this.dataMapper = dataMapper;
    }

    /// <summary>
    /// Gets the busines catalog settings.
    /// </summary>
    /// <param name="siteContext">The siteContext context.</param>
    /// <param name="database">The database.</param>
    /// <returns>
    /// The busines catalog settings.
    /// </returns>
    public virtual DomainModel.Configurations.BusinessCatalogSettings GetBusinesCatalogSettings([NotNull] SiteContext siteContext, Database database)
    {
      return this.GetSettingItem<BusinessCatalogSettings>(siteContext, database, "Business Catalog");
    }

    /// <summary>
    /// Gets the general settings.
    /// </summary>
    /// <param name="siteContext">The siteContext context.</param>
    /// <param name="database">The database.</param>
    /// <returns>
    /// The general settings.
    /// </returns>
    public virtual DomainModel.Configurations.GeneralSettings GetGeneralSettings([NotNull] SiteContext siteContext, Database database)
    {
      return this.GetSettingItem<GeneralSettings>(siteContext, database, "General");
    }

    /// <summary>
    /// Gets the setting item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="siteContext">The shop context.</param>
    /// <param name="database">The database.</param>
    /// <param name="settingName">Name of the setting.</param>
    /// <returns>
    /// The setting item.
    /// </returns>
    [CanBeNull]
    private T GetSettingItem<T>([NotNull] SiteContext siteContext, [NotNull] Database database, [NotNull] string settingName) where T : IEntity, new()
    {
      string settingsPath = FileUtil.MakePath(siteContext.StartPath, siteContext.Properties[EcommerceSiteSettingsKey]);
      string businessCatalogPath = FileUtil.MakePath(settingsPath, settingName);

      Item item = database.GetItem(businessCatalogPath);

      if (item == null)
      {
        return default(T);
      }

      return this.dataMapper.GetEntity<T>(item);
    }
  }
}