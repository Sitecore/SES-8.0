// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StrictCurrencyConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the strict currency converter class.
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
  using Diagnostics;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using DomainModel.Prices;
  using Exceptions;
  using Sitecore.Data.Items;

  /// <summary>
  /// Defines the strict currency converter class.
  /// </summary>
  public class StrictCurrencyConverter : ICurrencyConverter<Totals, DomainModel.Currencies.Currency>
  {
    /// <summary>
    /// Gets or sets the currency mapper.
    /// </summary>
    /// <value>The currency mapper.</value>
    private readonly EntityMapper<Item, IEntity> currencyMapper;

    /// <summary>
    /// The inner currency converter.
    /// </summary>
    private readonly ICurrencyConverter<Totals, DomainModel.Currencies.Currency> innerCurrencyConverter;

    /// <summary>
    /// The shop context.
    /// </summary>
    private readonly ShopContext shopContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="StrictCurrencyConverter"/> class.
    /// </summary>
    /// <param name="innerCurrencyConverter">The inner currency converter.</param>
    /// <param name="shopContext">The shop context.</param>
    /// <param name="currencyMapper">The currency mapper.</param>
    public StrictCurrencyConverter([NotNull] ICurrencyConverter<Totals, DomainModel.Currencies.Currency> innerCurrencyConverter, [NotNull] ShopContext shopContext, [NotNull] EntityMapper<Item, IEntity> currencyMapper)
    {
      Assert.ArgumentNotNull(innerCurrencyConverter, "innerCurrencyConverter");
      Assert.ArgumentNotNull(shopContext, "shopContext");
      Assert.ArgumentNotNull(currencyMapper, "currencyMapper");

      this.currencyMapper = currencyMapper;
      this.innerCurrencyConverter = innerCurrencyConverter;
      this.shopContext = shopContext;
    }

    /// <summary>
    /// Converts the specified input totals.
    /// </summary>
    /// <param name="inputTotals">The input totals.</param>
    /// <param name="outputCurrency">The output currency.</param>
    /// <returns>
    /// The T totals.
    /// </returns>
    /// <exception cref="CurrencyConversionException">Throws exception if unable convert to specified currency.</exception>
    [NotNull]
    public Totals Convert([NotNull] Totals inputTotals, [NotNull] DomainModel.Currencies.Currency outputCurrency)
    {
      Assert.ArgumentNotNull(inputTotals, "inputTotals");
      Assert.ArgumentNotNull(outputCurrency, "outputCurrency");

      DomainModel.Currencies.Currency masterCurrency = this.GetMasterCurrency();

      if ((masterCurrency != null) && (masterCurrency.Code == outputCurrency.Code))
      {
        return inputTotals;
      }

      Totals totals = this.innerCurrencyConverter.Convert(inputTotals, outputCurrency);

      if (ReferenceEquals(inputTotals, totals))
      {
        throw new CurrencyConversionException();
      }

      return totals;
    }

    /// <summary>
    /// Converts the specified input totals.
    /// </summary>
    /// <param name="inputTotals">The input totals.</param>
    /// <param name="relativeExchangeRate">The relative exchange rate.</param>
    /// <returns>
    /// The T totals.
    /// </returns>
    /// <exception cref="CurrencyConversionException">Throws exception if unable convert to specified currency.</exception>
    [NotNull]
    public Totals Convert([NotNull] Totals inputTotals, decimal relativeExchangeRate)
    {
      Assert.ArgumentNotNull(inputTotals, "inputTotals");

      Totals totals = this.innerCurrencyConverter.Convert(inputTotals, relativeExchangeRate);

      if (ReferenceEquals(inputTotals, totals))
      {
        throw new CurrencyConversionException();
      }

      return totals;
    }

    /// <summary>
    /// Gets the master currency.
    /// </summary>
    /// <returns>The master currency.</returns>
    protected DomainModel.Currencies.Currency GetMasterCurrency()
    {
      return this.shopContext.GetMasterCurrency(this.currencyMapper);
    }
  }
}