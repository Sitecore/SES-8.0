// -------------------------------------------------------------------------------------------
// <copyright file="BusinessCatalogUtil.cs" company="Sitecore Corporation">
//  Copyright (c) Sitecore Corporation 1999-2015 
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
  using Data;
  using Diagnostics;
  using DomainModel.Configurations;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Unity;

  /// <summary>
  /// </summary>
  public static class BusinessCatalogUtil
  {
    /// <summary>
    /// </summary>
    public const string BUSINESS_UNIT = "Company Master Data Link";

    /// <summary>
    /// </summary>
    public const string COUNTRIES = "Countries Link";

    /// <summary>
    /// Gets value from a field in an option item within the selected configuration folder in buisness catalog
    /// </summary>
    /// <param name="name">
    /// The options item name 
    /// </param>
    /// <param name="section">
    /// The section.
    /// </param>
    /// <param name="fieldName">
    /// Name of the field to retrive value from on option item
    /// </param>
    /// <returns>
    /// </returns>
    public static string GetOptionFromName(string name, string section, string fieldName)
    {
      string query = string.Format(".//*[@@name='{0}']", name);
      Item optionItem = GetOptionItemFromQuery(section, query);
      return optionItem != null ? optionItem[fieldName] : null;
    }

    /// <summary>
    /// Gets the option item from query.
    /// </summary>
    /// <param name="section">
    /// The section.
    /// </param>
    /// <param name="query">
    /// The query.
    /// </param>
    /// <returns>
    /// </returns>
    private static Item GetOptionItemFromQuery(string section, string query)
    {
      BusinessCatalogSettings businessCatalogSettings = Context.Entity.GetConfiguration<BusinessCatalogSettings>();
      EntityHelper entityHelper = Context.Entity.Resolve<EntityHelper>();
      string sectionValue = entityHelper.GetPropertyValueByField<string, BusinessCatalogSettings>(businessCatalogSettings, section);

      Assert.IsNotNullOrEmpty(sectionValue, "Item path is null");

      Item catalogItem = Sitecore.Context.Database.GetItem(sectionValue);

      Assert.IsNotNull(catalogItem, "Could not find BusinessCatalog/" + section);
      Item optionItem = catalogItem.Axes.SelectSingleItem(query);
      
      return optionItem;
    }

    /// <summary>
    /// Gets the title from a sitecore item.
    /// </summary>
    /// <param name="sitecoreId">
    /// The sitecore ID.
    /// </param>
    /// <returns>
    /// </returns>
    private static string GetTitleFromItem(ID sitecoreId)
    {
      Item item = Sitecore.Context.Database.GetItem(sitecoreId);
      return item != null ? item["Title"] : null;
    }

    /// <summary>
    /// Gets the title from referenced item or value if value is not a valid Sitecore ID pointing at an item with Title field
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// </returns>
    public static string GetTitleFromReferencedItemOrValue(string value)
    {
      string title = string.Empty;
      if (ID.IsID(value))
      {
        title = GetTitleFromItem(ID.Parse(value));
      }

      if (string.IsNullOrEmpty(title))
      {
        // The value is not a Sitecore ID, then use this as default.
        title = value;
      }

      return title;
    }
  }
}