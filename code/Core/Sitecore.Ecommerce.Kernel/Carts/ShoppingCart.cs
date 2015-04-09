// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCart.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ShoppingCart type.
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

namespace Sitecore.Ecommerce.Carts
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Data;
  using Diagnostics;
  using DomainModel.Configurations;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using DomainModel.Payments;
  using DomainModel.Prices;
  using DomainModel.Products;
  using DomainModel.Shippings;
  using DomainModel.Users;
  using Globalization;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Utils;
  using Validators.Interception;

  /// <summary>
  /// Defines the ShoppingCart type.
  /// </summary>
  [Serializable]
  public class ShoppingCart : DomainModel.Carts.ShoppingCart, IEntity
  {
    /// <summary>
    /// The shopping cart totals.
    /// </summary>
    private Totals totals;

    /// <summary>
    /// The currency.
    /// </summary>
    private Currency currency;

    /// <summary>
    /// Gets or sets the order number.
    /// </summary>
    /// <value>The order number.</value>
    [Entity(FieldName = "OrderNumber")]
    public override string OrderNumber { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the product lines.
    /// </summary>
    /// <value>The product lines.</value>
    public override IList<DomainModel.Carts.ShoppingCartLine> ShoppingCartLines { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the customer info.
    /// </summary>
    /// <value>The customer info.</value>
    [Entity(FieldName = "CustomerInfo")]
    public override CustomerInfo CustomerInfo { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the shipping price.
    /// </summary>
    /// <value>The shipping price.</value>
    [Entity(FieldName = "Shipping Price")]
    public override decimal ShippingPrice { get; set; }

    /// <summary>
    /// Gets or sets the totals.
    /// </summary>
    /// <value>The totals.</value>
    [Entity(FieldName = "Totals")]
    public override Totals Totals
    {
      get
      {
        this.totals = Context.Entity.Resolve<Totals>();

        if (this.ShoppingCartLines.IsNullOrEmpty())
        {
          return this.totals;
        }

        foreach (ProductLine line in this.ShoppingCartLines)
        {
          IProductPriceManager productPriceManager = Context.Entity.Resolve<IProductPriceManager>();
          Totals lineTotals = productPriceManager.GetProductTotals<Totals, ProductBaseData, Currency>(line.Product, this.Currency, line.Quantity);

          if (lineTotals.Any(p => p.Value > 0))
          {
            line.Totals = lineTotals;
          }

          line.Totals.ToList().ForEach(p => this.totals[p.Key] += p.Value);
        }

        if (this.ShoppingCartLines.Count > 0)
        {
          this.totals.VAT = this.ShoppingCartLines.First().Totals.VAT;
        }

        this.totals.TotalPriceExVat += this.ShippingPrice;
        this.totals.TotalPriceIncVat += this.ShippingPrice;

        this.totals.PriceExVat = this.totals.TotalPriceExVat;
        this.totals.MemberPrice = this.totals.TotalPriceExVat;
        this.totals.PriceIncVat = this.totals.TotalPriceIncVat;

        return this.totals;
      }

      [NotNullValue]
      set
      {
        this.totals = value;
      }
    }

    /// <summary>
    /// Gets or sets the payment method.
    /// </summary>
    /// <value>The payment method.</value>
    [Entity(FieldName = "PaymentSystem")]
    public override PaymentSystem PaymentSystem { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the shipping provider.
    /// </summary>
    /// <value>The shipping provider.</value>
    [Entity(FieldName = "ShippingProvider")]
    public override ShippingProvider ShippingProvider { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the notification option.
    /// </summary>
    /// <value>The notification option.</value>
    [Entity(FieldName = "NotificationOption")]
    public override NotificationOption NotificationOption { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the notification option value.
    /// </summary>
    /// <value>The notification option value.</value>
    [Entity(FieldName = "NotificationOptionValue")]
    public override string NotificationOptionValue { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    /// <value>The currency.</value>
    [Entity(FieldName = "Currency")]
    public override Currency Currency
    {
      get
      {
        if (this.currency != null && !string.IsNullOrEmpty(this.currency.Code))
        {
          return this.currency;
        }

        GeneralSettings generalSettings = Context.Entity.GetConfiguration<GeneralSettings>();
        if (generalSettings == null)
        {
          return this.currency;
        }

        if (this.currency is IEntity && !string.IsNullOrEmpty(generalSettings.DisplayCurrency) && generalSettings.DisplayCurrency.Equals(((IEntity)this.currency).Alias))
        {
          return this.currency;
        }

        if (this.currency == null || string.IsNullOrEmpty(this.currency.Code))
        {
          Item item = null;

          string currencyLink = generalSettings.DisplayCurrency;
          if (!string.IsNullOrEmpty(currencyLink))
          {
            item = ItemManager.GetItem(currencyLink, Language.Current, Sitecore.Data.Version.Latest, Sitecore.Context.Database);
          }

          if (item == null)
          {
            Log.Warn("Display Currency item was not found.", this);

            currencyLink = generalSettings.MasterCurrency;
            if (!string.IsNullOrEmpty(currencyLink))
            {
              item = ItemManager.GetItem(currencyLink, Language.Current, Sitecore.Data.Version.Latest, Sitecore.Context.Database);
            }
          }

          if (item == null)
          {
            Log.Error("Master Currency item was not found.", this);

            return this.currency;
          }

          IDataMapper dataMapper = Context.Entity.Resolve<IDataMapper>();
          Currency resultCurrency = dataMapper.GetEntity<Currency>(item);

          if (resultCurrency != null)
          {
            this.currency = resultCurrency;
          }
        }

        return this.currency;
      }

      [NotNullValue]
      set
      {
        this.currency = value;
      }
    }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual string Alias { get; [NotNullValue] set; }

    #endregion
  }
}