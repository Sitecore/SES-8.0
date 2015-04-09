// -------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartSummary.ascx.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess
{
  using System;
  using System.Web.UI;
  using DomainModel.Configurations;
  using Globalization;
  using DomainModel.Currencies;

  /// <summary>
  /// The shopping cart summary.
  /// </summary>
  public partial class ShoppingCartSummary : UserControl
  {
    /// <summary>
    /// shopping cart
    /// </summary>
    private DomainModel.Prices.Totals totals;

    /// <summary>
    /// Currency.
    /// </summary>
    private Currency currency;

    /// <summary>
    /// Display price including VAT
    /// </summary>
    private bool displayPriceIncVat = true;

    /// <summary>
    /// Display title.
    /// </summary>
    private bool displayTitle = true;

    /// <summary>
    /// Gets the general settings.
    /// </summary>
    /// <value>The general settings.</value>
    public GeneralSettings GeneralSettings
    {
      get
      {
        return Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>();
      }
    }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    /// <value>The currency.</value>
    public virtual Currency Currency
    {
      get
      {
        if (this.currency == null)
        {
          DomainModel.Carts.ShoppingCart shoppingCart = Sitecore.Ecommerce.Context.Entity.GetInstance<DomainModel.Carts.ShoppingCart>();
          this.currency = shoppingCart.Currency;
        }

        return this.currency;
      }

      set { this.currency = value; }
    }

    /// <summary>
    /// Gets the ShoppingCart settings.
    /// </summary>
    public ShoppingCartSettings ShoppingCartSettings
    {
      get
      {
        return Sitecore.Ecommerce.Context.Entity.GetConfiguration<ShoppingCartSettings>();
      }
    }

    /// <summary>
    /// Gets or sets the data items.
    /// </summary>
    /// <value>The data items.</value>
    public DomainModel.Prices.Totals Totals
    {
      get
      {
        if (this.totals == null)
        {
          DomainModel.Carts.ShoppingCart shoppingCart = Sitecore.Ecommerce.Context.Entity.GetInstance<DomainModel.Carts.ShoppingCart>();
          this.totals = shoppingCart.Totals;
        }

        return this.totals;
      }

      set
      {
        this.totals = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [display price inc vat].
    /// </summary>
    /// <value><c>true</c> if [display price inc vat]; otherwise, <c>false</c>.</value>
    public bool DisplayPriceIncVat
    {
      get
      {
        return this.displayPriceIncVat;
      }

      set
      {
        this.displayPriceIncVat = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [display VAT].
    /// </summary>
    /// <value><c>true</c> if [display VAT]; otherwise, <c>false</c>.</value>
    public bool DisplayVAT { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [display title].
    /// </summary>
    /// <value><c>true</c> if [display title]; otherwise, <c>false</c>.</value>
    public bool DisplayTitle
    {
      get
      {
        return this.displayTitle;
      }

      set
      {
        this.displayTitle = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [dispay price exl vat].
    /// </summary>
    /// <value><c>true</c> if [dispay price exl vat]; otherwise, <c>false</c>.</value>
    public bool DispayPriceExlVat { get; set; }

    #region Protected methods

    /// <summary>
    /// The page_ load.
    /// </summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e"> The event arguments. </param>
    protected void Page_Load(object sender, EventArgs e)
    {
      if (this.DisplayTitle)
      {
        this.lblFormTitle.Text = Translate.Text(Sitecore.Ecommerce.Examples.Texts.CartSummary);
      }

      this.priceExclVAT.Visible = this.DispayPriceExlVat;
      this.priceInclVat.Visible = this.DisplayPriceIncVat;
      this.VAT.Visible = this.DisplayVAT;
      this.DataBind();
    }

    /// <summary>
    /// Formats the price.
    /// </summary>
    /// <param name="price">The price.</param>
    /// <returns>Returns formated price</returns>
    protected virtual string FormatPrice(decimal price)
    {
      return Utils.MainUtil.FormatPrice(price, this.GeneralSettings.DisplayCurrencyOnPrices, this.ShoppingCartSettings.PriceFormatString, this.Currency.Code);
    }

    #endregion
  }
}