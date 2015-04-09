// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductPriceService.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the product price service class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement
{
  using System;
  using Diagnostics;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using DomainModel.Prices;
  using DomainModel.Products;
  using Exceptions;

  /// <summary>
  /// Defines the product price service class.
  /// </summary>
  public class ProductPriceService
  {
    /// <summary>
    /// The product price manager instance.
    /// </summary>
    private IProductPriceManager productPriceManager;

    /// <summary>
    /// The currency provider.
    /// </summary>
    private IEntityProvider<Currency> currencyProvider;

    /// <summary>
    /// Gets or sets the product price manager.
    /// </summary>
    /// <value>
    /// The product price manager.
    /// </value>
    [NotNull]
    public IProductPriceManager ProductPriceManager
    {
      get
      {
        return this.productPriceManager ?? (this.productPriceManager = Context.Entity.Resolve<IProductPriceManager>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.productPriceManager = value;
      }
    }


    /// <summary>
    /// Gets or sets the currency provider.
    /// </summary>
    /// <value>The currency provider.</value>
    [NotNull]
    public virtual IEntityProvider<Currency> CurrencyProvider
    {
      get
      {
        return this.currencyProvider ?? (this.currencyProvider = Context.Entity.Resolve<IEntityProvider<Currency>>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.currencyProvider = value;
      }
    }

    /// <summary>
    /// Gets the price.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <param name="priceCode">The price code.</param>
    /// <returns>
    /// The price.
    /// </returns>
    [CanBeNull]
    public virtual decimal? GetPrice([NotNull] ProductBaseData product, [NotNull] string priceCode)
    {
      Assert.ArgumentNotNull(product, "product");
      Assert.ArgumentNotNull(priceCode, "priceCode");

      var currency = this.CurrencyProvider.Get(priceCode);

      decimal? price = null;

      if (currency != null)
      {
        try
        {
          Totals totals = this.ProductPriceManager.GetProductTotals<Totals, ProductBaseData, Currency>(product, currency);
          price = Math.Round(totals.PriceExVat, 2, MidpointRounding.AwayFromZero);
        }
        catch (CurrencyConversionException)
        {
          price = null;
        }
      }

      return price;
    }
  }
}