// -------------------------------------------------------------------------------------------
// <copyright file="FormUtil.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
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

namespace Sitecore.Ecommerce.Forms
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Web.Security;
  using System.Web.UI.WebControls;
  using Sitecore.Data.Items;
  using Sitecore.Ecommerce.DomainModel.Configurations;

  /// <summary>
  /// Utility class for forms.
  /// </summary>
  public static class FormUtil
  {
    /// <summary>
    /// The ecommerce settings countries code
    /// </summary>
    public const string EcommerceSettingsCountriesCode = "Code";

    /// <summary>
    /// The ecommerce settings countries default code.
    /// </summary>
    public const string EcommerceSettingsCountriesDefaultCode = "NO";

    /// <summary>
    /// The ecommerce settings countries default name.
    /// </summary>
    public const string EcommerceSettingsCountriesDefaultName = "Norway";
    
    /// <summary>
    /// The ecommerce settings countries title
    /// </summary>
    public const string EcommerceSettingsCountriesTitle = "Title";

    /// <summary>
    /// Gets the countries registered in the solution.
    /// </summary>
    /// <value>The countries.</value>
    private static IEnumerable<Item> Countries
    {
      get
      {
        var countries = new List<Item>();
        var businessCatalogSettings = Context.Entity.GetConfiguration<BusinessCatalogSettings>();
        var countirsItem = Sitecore.Context.Database.GetItem(businessCatalogSettings.CountriesLink);
        var items = countirsItem.Children.ToArray();

        // Array.Sort (items);
        foreach (var item in items)
        {
          countries.Add(item);
        }

        var countriesSorted = from c in countries
                                                   orderby c.Name
                                                   select c;

        return countriesSorted;
      }
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="username">
    /// The user´s username.
    /// </param>
    public static void DeleteSitecoreUser(string username)
    {
      try
      {
        Membership.DeleteUser(username);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
      }
    }

    /// <summary>
    /// Fills a given <see cref="ListControl"/> with the countries registered in the solution.
    /// </summary>
    /// <param name="listControl">
    /// The list control.
    /// </param>
    public static void FillCountryDropdown(ListControl listControl)
    {
      foreach (var country in Countries)
      {
        listControl.Items.Add(new ListItem(country[EcommerceSettingsCountriesTitle], country.ID.ToString()));
      }

      listControl.SelectedValue = EcommerceSettingsCountriesDefaultCode;
    }

    /// <summary>
    /// Gets the country code for a given country name.
    /// </summary>
    /// <param name="countryName">
    /// Name of the country.
    /// </param>
    /// <returns>
    /// the country code
    /// </returns>
    public static string GetCountryCode(string countryName)
    {
      foreach (var item in Countries)
      {
        if (item[EcommerceSettingsCountriesTitle] == countryName)
        {
          return item[EcommerceSettingsCountriesCode];
        }
      }

      return EcommerceSettingsCountriesDefaultCode;
    }

    /// <summary>
    /// Gets the name of the country for a given country code.
    /// </summary>
    /// <param name="countryCode">
    /// The country code.
    /// </param>
    /// <returns>
    /// The country name.
    /// </returns>
    public static string GetCountryName(string countryCode)
    {
      foreach (var item in Countries)
      {
        if (item[EcommerceSettingsCountriesCode] == countryCode)
        {
          return item[EcommerceSettingsCountriesTitle];
        }
      }

      return EcommerceSettingsCountriesDefaultName;
    }
  }
}