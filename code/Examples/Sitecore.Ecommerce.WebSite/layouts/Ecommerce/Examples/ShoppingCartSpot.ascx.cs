// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartSpot.ascx.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The ShoppingCartSpot user control.
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.UserControls
{
  using System;
  using System.Web.UI;
  using Analytics.Components;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Configurations;
  using DomainModel.Products;
  using Sitecore.Globalization;
  using Utils;

  /// <summary>
  /// The ShoppingCartSpot user control.
  /// </summary>
  public partial class ShoppingCartSpot : UserControl
  {
    /// <summary>
    /// Gets the settings.
    /// </summary>
    /// <value>The settings.</value>
    protected ShoppingCartSpotSettings Settings
    {
      get { return Sitecore.Ecommerce.Context.Entity.GetConfiguration<ShoppingCartSpotSettings>(); }
    }

    /// <summary>
    /// Gets the general settings.
    /// </summary>
    /// <value>The general settings.</value>
    protected GeneralSettings GeneralSettings
    {
      get { return Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>(); }
    }

    /// <summary>
    /// Gets a value indicating whether [show navigation links].
    /// </summary>
    /// <value><c>true</c> if [show navigation links]; otherwise, <c>false</c>.</value>
    protected bool IsItemsInShoppingCart
    {
      get { return this.ShoppingCart.ShoppingCartLines.Count > 0; }
    }

    /// <summary>
    /// Gets the lit amount in ShoppingCart status line.
    /// </summary>
    /// <returns>The amount in ShoppingCart status text.</returns>
    protected string AmountInShoppingCartStatusText
    {
      get
      {
        int itemsInShoppingCart = this.ShoppingCart.ShoppingCartLines.Count;

        return itemsInShoppingCart == 0
                 ? Translate.Text(Sitecore.Ecommerce.Examples.Texts.TheShoppingCartIsEmpty)
                 : string.Format("{0} {1}", itemsInShoppingCart, Translate.Text(Sitecore.Ecommerce.Examples.Texts.ItemInShoppingCart));
      }
    }

    /// <summary>
    /// Gets a value indicating whether [show total sum in shopping cart].
    /// </summary>
    /// <value>
    /// <c>true</c> if [show total sum in shopping cart]; otherwise, <c>false</c>.
    /// </value>
    protected bool ShowTotalSumInShoppingCart
    {
      get
      {
        bool visible = this.Settings.ShowTotal;
        if (!visible || this.ShoppingCart.ShoppingCartLines == null || this.ShoppingCart.ShoppingCartLines.Count <= 0)
        {
          visible = false;
        }

        return visible;
      }
    }

    /// <summary>
    /// Gets the lit total sum.
    /// </summary>
    /// <returns>
    /// The total sum.
    /// </returns>
    protected string TotalSum
    {
      get
      {
        if (this.ShoppingCart.ShoppingCartLines.Count > 0)
        {
          decimal price = this.Settings.ShowTotalIncVat
                            ? this.ShoppingCart.Totals.PriceIncVat
                            : this.ShoppingCart.Totals.PriceExVat;

          return MainUtil.FormatPrice(price, GeneralSettings.DisplayCurrencyOnPrices, this.Settings.PriceFormatString);
        }

        return null;
      }
    }

    /// <summary>
    /// Gets the total price inc vat.
    /// </summary>
    /// <value>The total price inc vat.</value>
    protected string TotalPriceIncVat
    {
      get
      {
        try
        {
          return MainUtil.FormatPrice(this.ShoppingCart.Totals.TotalPriceIncVat);
        }
        catch (Exception exception)
        {
          Log.Error(exception.Message, exception);
          return "-";
        }
      }
    }

    /// <summary>
    /// Gets a value indicating whether [show amount in shopping cart status line].
    /// </summary>
    /// <value>
    /// <c>true</c> if [show amount in shopping cart status line]; otherwise, <c>false</c>.
    /// </value>
    protected bool ShowAmountInShoppingCartStatusLine
    {
      get { return this.ShoppingCart.ShoppingCartLines.Count == 0 || this.Settings.ShowAmountInShoppingCartStatusLine; }
    }

    /// <summary>
    /// Gets a value indicating whether [show shopping cart items].
    /// </summary>
    /// <value>
    /// <c>true</c> if [show shopping cart items]; otherwise, <c>false</c>.
    /// </value>
    protected bool ShowShoppingCartItems
    {
      get { return this.ShoppingCart.ShoppingCartLines.Count > 0 && this.Settings.ShowShoppingCartItemLines; }
    }

    /// <summary>
    /// Gets the shopping cart instance.
    /// </summary>
    protected ShoppingCart ShoppingCart
    {
      get { return Sitecore.Ecommerce.Context.Entity.GetInstance<ShoppingCart>(); }
    }

    /// <summary>
    /// The page load event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      this.repShoppingCartList.DataSource = this.ShoppingCart.ShoppingCartLines;
      DataBind();

      bool alwaysShowShoppingCart = this.Settings.AlwaysShowShoppingCart;
      if (this.ShoppingCart.ShoppingCartLines.Count == 0)
      {
        Visible = alwaysShowShoppingCart;
      }
    }

    /// <summary>
    /// Gets the price text.
    /// </summary>
    /// <param name="dataItem">The data item.</param>
    /// <returns>The ShoppingCart line item price.</returns>
    protected string ShoppingCartLineItemPrice(object dataItem)
    {
      ProductLine productLine = dataItem as ProductLine;
      if (productLine == null)
      {
        Log.Warn("Product line is null.", this);
        return "-";
      }

      return this.Settings.ShowPriceIncVAT
               ?
                 MainUtil.FormatPrice(productLine.Totals.PriceIncVat, false, this.Settings.PriceFormatString)
               :
                 MainUtil.FormatPrice(productLine.Totals.PriceExVat, false, this.Settings.PriceFormatString);
    }

    /// <summary>
    /// Gets the lit total price text.
    /// </summary>
    /// <param name="dataItem">The data item.</param>
    /// <returns>The ShoppingCart line total price.</returns>
    protected string ShoppingCartLineTotalPrice(object dataItem)
    {
      ProductLine productLine = dataItem as ProductLine;
      if (productLine == null)
      {
        Log.Warn("Product line is null.", this);
        return "-";
      }

      return this.Settings.ShowPriceIncVAT
               ?
                 MainUtil.FormatPrice(productLine.Totals.TotalPriceIncVat, false, this.Settings.PriceFormatString)
               :
                 MainUtil.FormatPrice(productLine.Totals.TotalPriceExVat, false, this.Settings.PriceFormatString);
    }

    /// <summary>
    /// Gets the Friendly Url
    /// </summary>
    /// <param name="dataItem">The data item.</param>
    /// <returns>Shopping cart item friendly url.</returns>
    protected string ShoppingCartLineFriendlyUrl(object dataItem)
    {
      ShoppingCartLine shoppingCartLine = dataItem as ShoppingCartLine;
      if (shoppingCartLine == null)
      {
        Log.Warn("Product line is null.", this);
        return "-";
      }

      return AnalyticsUtil.AddFollowListToQueryString(shoppingCartLine.FriendlyUrl, "ShoppingCartSpot");
    }
  }
}