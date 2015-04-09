// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountryDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the countries data source repository class.
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
  using System.Collections.Generic;
  using Diagnostics;
  using DomainModel.Addresses;
  using DomainModel.Configurations;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// Defines the countries data source repository class.
  /// </summary>
  public class CountryDataSourceRepository : DataSourceRepository<Country>
  {
    /// <summary>
    /// The shop context.
    /// </summary>
    private ShopContext shopContext;

    /// <summary>
    /// Gets or sets the shop context.
    /// </summary>
    /// <value>
    /// The shop context.
    /// </value>
    [CanBeNull]
    public ShopContext ShopContext
    {
      get
      {
        if (this.shopContext == null && Context.Entity != null)
        {
          this.shopContext = (ShopContext)Context.Entity.Resolve(typeof(ShopContext), null);
        }

        return this.shopContext;
      }

      set
      {
        this.shopContext = value;
      }
    }

    /// <summary>
    /// Gets the countries.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>The list of countries.</returns>
    [NotNull]
    public override IEnumerable<Country> SelectEntities([CanBeNull] string rawQuery)
    {
      Item countryRoot = this.GetCountryRootItem();

      yield return new Country { Code = string.Empty, Title = string.Empty };

      if (countryRoot != null)
      {
        foreach (Item countryItem in countryRoot.Children)
        {
          yield return new Country { Code = countryItem["Code"], Title = countryItem["Title"] };
        }
      }
    }

    /// <summary>
    /// Gets the country root item.
    /// </summary>
    /// <returns>The country root item.</returns>
    protected virtual Item GetCountryRootItem()
    {
      Assert.IsNotNull(this.ShopContext, "ShopContext cannot be null.");

      BusinessCatalogSettings businessCatalogSettings = this.ShopContext.BusinessCatalogSettings;
      Assert.IsNotNull(businessCatalogSettings, "Unable to read countries. Business catalog settings not found.");
      Assert.IsNotNullOrEmpty(businessCatalogSettings.CountriesLink, "Unable to read countries. Countries link not found.");

      Database database = this.ShopContext.Database;
      Assert.IsNotNull(database, "Unable to read countries. Database not found.");

      return database.GetItem(businessCatalogSettings.CountriesLink);
    }
  }
}