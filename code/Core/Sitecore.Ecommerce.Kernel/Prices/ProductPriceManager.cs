// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductPriceManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Product price manager. Calculates and determines which price to use according to prixe matrix.
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

namespace Sitecore.Ecommerce.Prices
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using Data.Fields;
  using Diagnostics;
  using DomainModel.Addresses;
  using DomainModel.Configurations;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using DomainModel.Prices;
  using DomainModel.Products;
  using DomainModel.Users;
  using PriceMatrix;
  using Products;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Utils;

  /// <summary>
  /// Product price manager. Calculates and determines which price to use according to price matrix.
  /// </summary>
  public class ProductPriceManager : IProductPriceManager
  {
    /// <summary>
    /// The shop context.
    /// </summary>
    private readonly ShopContext shopContext;

    /// <summary>
    /// The price calculator factory.
    /// </summary>
    private readonly PriceCalculatorFactory priceCalculatorFactory;

    /// <summary>
    /// The product repository database.
    /// </summary>
    private Database database;

    /// <summary>
    /// The general settings.
    /// </summary>
    private GeneralSettings generalSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductPriceManager" /> class.
    /// </summary>
    /// <param name="priceCalculatorFactory">The price calculator factory.</param>
    /// <param name="shopContext">The shop context.</param>
    public ProductPriceManager(PriceCalculatorFactory priceCalculatorFactory, ShopContext shopContext)
    {
      this.priceCalculatorFactory = priceCalculatorFactory;
      this.shopContext = shopContext;
    }

    /// <summary>
    /// Gets the product repository database.
    /// </summary>
    /// <value>The database.</value>
    [NotNull]
    private Database Database
    {
      get
      {
        if (this.database == null)
        {
          Assert.IsNotNull(this.shopContext, "Unable to resolve database. Context shop cannot be null.");
          Assert.IsNotNull(this.shopContext.InnerSite, "Unable to resolve database. Inner site cannot be null.");

          this.database = this.shopContext.InnerSite.ContentDatabase;
          Assert.IsNotNull(this.database, "Unable to resolve database.");
        }

        return this.database;
      }
    }

    /// <summary>
    /// Gets the business catalog settings.
    /// </summary>
    /// <value>The business catalog settings.</value>
    [NotNull]
    private GeneralSettings GeneralSettings
    {
      get
      {
        if (this.generalSettings == null)
        {
          Assert.IsNotNull(this.shopContext, "Unable to resolve general settings. Context shop cannot be null.");

          this.generalSettings = this.shopContext.GeneralSettings;
          Assert.IsNotNull(this.generalSettings, this.GetType(), "General settings not found.");
        }

        return this.generalSettings;
      }
    }

    #region Implementation of IProductPriceManager

    /// <summary>
    /// Gets the product totals.
    /// </summary>
    /// <typeparam name="TTotals">The type of the totals.</typeparam>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <typeparam name="TCurrency">The type of the currency.</typeparam>
    /// <param name="product">The product.</param>
    /// <param name="currency">The currency.</param>
    /// <returns>The product prices list.</returns>
    public virtual TTotals GetProductTotals<TTotals, TProduct, TCurrency>(TProduct product, TCurrency currency)
      where TProduct : ProductBaseData
      where TTotals : DomainModel.Prices.Totals
      where TCurrency : Currency
    {
      Assert.ArgumentNotNull(product, "product");
      Assert.ArgumentNotNull(currency, "currency");

      return this.GetProductTotals<TTotals, TProduct, TCurrency>(product, currency, 1);
    }

    /// <summary>
    /// Gets the product totals.
    /// </summary>
    /// <typeparam name="TTotals">The type of the totals.</typeparam>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <typeparam name="TCurrency">The type of the currency.</typeparam>
    /// <param name="product">The product.</param>
    /// <param name="currency">The currency.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns>The product prices list.</returns>
    public virtual TTotals GetProductTotals<TTotals, TProduct, TCurrency>(TProduct product, TCurrency currency, uint quantity)
      where TProduct : ProductBaseData
      where TTotals : DomainModel.Prices.Totals
      where TCurrency : Currency
    {
      Assert.ArgumentNotNull(product, "product");
      Assert.ArgumentNotNull(currency, "currency");

      TTotals productTotals = Context.Entity.Resolve<TTotals>();

      if (quantity < 1)
      {
        return productTotals;
      }

      IDictionary<string, decimal> priceMatrix = new Dictionary<string, decimal>
      {
        { "NormalPrice", this.GetPriceMatrixPrice(product, "Normalprice") },
        { "MemberPrice", this.GetPriceMatrixPrice(product, "Memberprice") }
      };

      decimal vat = decimal.Zero;
      if (product is Product)
      {
        vat = this.GetVat(product as Product);
      }

      DomainModel.Prices.Totals totals = this.GetProductTotals(priceMatrix, vat, quantity);

      ICurrencyConverter<TTotals, TCurrency> currencyConverter = Context.Entity.Resolve<ICurrencyConverter<TTotals, TCurrency>>();
      return currencyConverter.Convert(totals as TTotals, currency);
    }

    /// <summary>
    /// Gets the value-added tax.
    /// </summary>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <param name="product">The product.</param>
    /// <param name="vatRegion">The vat region.</param>
    /// <returns>
    /// The value-added tax.
    /// </returns>
    public virtual decimal GetVat<TProduct>(TProduct product, VatRegion vatRegion = null) where TProduct : Product
    {
      Assert.ArgumentNotNull(product, "product");

      if (string.IsNullOrEmpty(product.VatType))
      {
        Log.Warn(string.Format("Value-added tax field of product '{0}' is null or empty", product.Title), this);
        return decimal.Zero;
      }

      Item vatTypeItem = this.Database.GetItem(product.VatType);
      if (vatTypeItem == null)
      {
        Log.Warn("Value-added tax item is null", this);
        return decimal.Zero;
      }

      Item vatValueItem = null;

      if (vatRegion is IEntity)
      {
        vatValueItem = vatTypeItem.Axes.SelectSingleItem(string.Format("./*[@VAT Region ='{0}']", (vatRegion as IEntity).Alias));
      }

      if (vatValueItem == null)
      {
        vatValueItem = vatTypeItem.Axes.SelectSingleItem(string.Format("./*[@VAT Region ='{0}']", this.GeneralSettings.DefaultVatRegion));
      }

      string vatValue = vatValueItem != null ? vatValueItem["VAT Value"] : "0";

      decimal vat;
      decimal.TryParse(vatValue, NumberStyles.Float, CultureInfo.InvariantCulture, out vat);

      return vat;
    }

    /// <summary>
    /// Gets the product totals.
    /// </summary>
    /// <param name="priceMatrix">The price matrix.</param>
    /// <param name="vat">The vat.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns>
    /// The product totals.
    /// </returns>
    protected virtual DomainModel.Prices.Totals GetProductTotals(IDictionary<string, decimal> priceMatrix, decimal vat, uint quantity)
    {
      Assert.IsNotNull(this.priceCalculatorFactory, "Price Calculation Factory cannot be null.");

      PriceCalculator calculator = this.priceCalculatorFactory.CreateCalculator();

      DomainModel.Prices.Totals totals = calculator.Calculate(priceMatrix, vat, quantity);

      return totals;
    }

    #endregion

    /// <summary>
    /// Gets the price matrix price.
    /// </summary>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <param name="product">The product.</param>
    /// <param name="priceMatrixName">Name of the price matrix.</param>
    /// <returns>The price matrix price.</returns>
    protected virtual decimal GetPriceMatrixPrice<TProduct>(TProduct product, string priceMatrixName) where TProduct : ProductBaseData
    {
      Assert.ArgumentNotNull(product, "product");

      string field = "Price";
      Item productItem = ProductRepositoryUtil.GetRepositoryItem(product, this.Database);

      if (productItem == null)
      {
        return decimal.Zero;
      }

      PriceField priceField;
      try
      {
        priceField = productItem.Fields[field];
      }
      catch (Exception exception)
      {
        Log.Error(exception.Message, exception, this);
        return decimal.Zero;
      }

      if (priceField == null)
      {
        return decimal.Zero;
      }

      string sitename = "Shop";
      string query = string.Format("./{0}/{1}", sitename, priceMatrixName);
      IPriceMatrixItem priceMatrixItem = priceField.PriceMatrix.SelectSingleItem(query);
      CategoryItem categoryItem = priceMatrixItem as CategoryItem;

      if (categoryItem == null)
      {
        return decimal.Zero;
      }

      string price = categoryItem.Amount;

      decimal priceValue;
      return !decimal.TryParse(price, NumberStyles.Float, CultureInfo.InvariantCulture, out priceValue) ? 0 : priceValue;
    }
  }
}