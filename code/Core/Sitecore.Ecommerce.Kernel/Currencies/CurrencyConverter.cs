// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrencyConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the CurrencyConverter type.
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

namespace Sitecore.Ecommerce.Currencies
{
  using System;
  using System.Globalization;
  using System.Linq;
  using Data;
  using Diagnostics;
  using DomainModel.Configurations;
  using DomainModel.Currencies;
  using DomainModel.Prices;
  using Globalization;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  
  /// <summary>
  /// Converts totals from one currency to another one.
  /// </summary>
  /// <typeparam name="TTotals">The type of the totals.</typeparam>
  /// <typeparam name="TCurrency">The type of the currency.</typeparam>
  public class CurrencyConverter<TTotals, TCurrency> : ICurrencyConverter<TTotals, TCurrency>
    where TTotals : Totals
    where TCurrency : DomainModel.Currencies.Currency
  {
    /// <summary>
    /// Gets or sets the shop context.
    /// </summary>
    /// <value>
    /// The shop context.
    /// </value>
    [CanBeNull]
    public ShopContext ShopContext { get; set; }

    /// <summary>
    /// Converts the specified input totals.
    /// </summary>
    /// <param name="inputTotals">The input totals.</param>
    /// <param name="outputCurrency">The output currency.</param>
    /// <returns>The output totals.</returns>
    public virtual TTotals Convert(TTotals inputTotals, TCurrency outputCurrency)
    {
      Assert.ArgumentNotNull(inputTotals, "Input totals");
      Assert.ArgumentNotNull(outputCurrency, "Output currency");

      Assert.IsNotNull(this.ShopContext, "Unable to convert currency. ShopContext should be set.");
      GeneralSettings generalSettings = this.ShopContext.GeneralSettings;

      Assert.IsNotNull(generalSettings, "Unable to convert currency. General Settings should be set.");

      if (!ID.IsID(generalSettings.MasterCurrency))
      {
        Log.Warn("Master currency item Id is invalid.", this);
        return inputTotals;
      }

      Item item = ItemManager.GetItem(generalSettings.MasterCurrency, Language.Current, Sitecore.Data.Version.Latest, Sitecore.Context.Database);
      if (item == null)
      {
        Log.Error("Master currency item was not found.", this);
        return inputTotals;
      }

      IDataMapper dataMapper = Context.Entity.Resolve<IDataMapper>();
      TCurrency masterCurrency = dataMapper.GetEntity<TCurrency>(item);
      if (masterCurrency == null)
      {
        Log.Error("Master currency was not defined.", this);
        return inputTotals;
      }

      if (masterCurrency.Code.Equals(outputCurrency.Code, StringComparison.OrdinalIgnoreCase))
      {
        return inputTotals;
      }

      BusinessCatalogSettings businessCatalogSettings = this.ShopContext.BusinessCatalogSettings;
      Assert.IsNotNull(businessCatalogSettings, "Unable to convert currency. Business Catalog Settings should be set.");

      Item currencyMatrixRootItem = Sitecore.Context.Database.GetItem(businessCatalogSettings.CurrencyMatrixLink);
      if (currencyMatrixRootItem == null)
      {
        Log.Error("Currency matrix root item was not found.", this);
        return inputTotals;
      }

      Item currenciesRootItem = Sitecore.Context.Database.GetItem(businessCatalogSettings.CurrenciesLink);
      if (currenciesRootItem == null)
      {
        Log.Error("Currencies root item was not found.", this);
        return inputTotals;
      }

      string currencyQuery = string.Format(".//*[@Code='{0}']", outputCurrency.Code);
      Item outputCurrencyItem = currenciesRootItem.Axes.SelectSingleItem(currencyQuery);
      if (outputCurrencyItem == null)
      {
        Log.Error("Output currency item was not found.", this);
        return inputTotals;
      }

      string query = string.Format(".//*[@Currency Link='{0}']/*[@Currency Link='{1}']", generalSettings.MasterCurrency, outputCurrencyItem.ID);
      Item exchangeRateItem = currencyMatrixRootItem.Axes.SelectSingleItem(query);

      if (exchangeRateItem == null)
      {
        Log.Error("Exchange rate item was not found.", this);
        return inputTotals;
      }

      Assert.IsNotNullOrEmpty(exchangeRateItem["Exchange Rate"], string.Concat("Exchange rate was no set for currencies. Master currency: ", masterCurrency.Code, ", Display currency: ", outputCurrency.Code));

      decimal exchangeRate;
      if (!decimal.TryParse(exchangeRateItem["Exchange Rate"], NumberStyles.Currency, CultureInfo.InvariantCulture, out exchangeRate))
      {
        Log.Error(string.Concat("Can not convert the exchange rate. Master currency: ", masterCurrency.Code, ", Display currency: ", outputCurrency.Code), this);
        exchangeRate = 1;
      }

      return this.Convert(inputTotals, exchangeRate);
    }

    /// <summary>
    /// Converts the specified input totals.
    /// </summary>
    /// <param name="inputTotals">The input totals.</param>
    /// <param name="relativeExchangeRate">The relative exchange rate.</param>
    /// <returns>The output totals.</returns>
    /// <exception cref="Exception">Exchange rate is incorrect</exception>
    public virtual TTotals Convert(TTotals inputTotals, decimal relativeExchangeRate)
    {
      Assert.IsNotNull(inputTotals, "Input totals is null");
      if (relativeExchangeRate <= 0)
      {
        Log.Warn("Exchange rate is incorrect", this);
        return inputTotals;
      }

      TTotals totals = Context.Entity.Resolve<TTotals>();

      inputTotals.ToList().ForEach(p => totals[p.Key] = p.Value * relativeExchangeRate);
      totals.VAT = inputTotals.VAT;

      return totals;
    }
  }
}