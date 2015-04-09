// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductsListView.ascx.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The shopping cart view.
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess
{
  using System;
  using System.Collections.Generic;
  using System.Web.UI;
  using System.Web.UI.HtmlControls;
  using System.Web.UI.WebControls;
  using Analytics.Components;
  using DomainModel.Carts;
  using DomainModel.Configurations;
  using DomainModel.Orders;
  using DomainModel.Products;
  using Examples.CheckOutProcess;
  using Utils;
  using DomainModel.Currencies;

  /// <summary>
  /// The shopping cart view.
  /// </summary>
  public partial class ProductsListView : UserControl
  {
    /// <summary>
    /// Data source.
    /// </summary>
    private object productLines;

    #region Public properties

    /// <summary>
    /// Gets the general settings.
    /// </summary>
    /// <value>The general settings.</value>
    public static GeneralSettings GeneralSettings
    {
      get
      {
        return Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>();
      }
    }

    /// <summary>
    /// Gets the ShoppingCart settings.
    /// </summary>
    public static ShoppingCartSettings ShoppingCartSettings
    {
      get
      {
        return Sitecore.Ecommerce.Context.Entity.GetConfiguration<ShoppingCartSettings>();
      }
    }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    /// <value>The currency.</value>
    public Currency Currency { get; set; }

    /// <summary>
    /// Gets or sets DisplayMode.
    /// </summary>
    public OrderDisplayMode DisplayMode { get; set; }

    /// <summary>
    /// Gets or sets the data source.
    /// </summary>
    /// <value>The data source.</value>
    public object ProductLines
    {
      get
      {
        if (this.productLines == null)
        {
          DomainModel.Carts.ShoppingCart shoppingCart = Sitecore.Ecommerce.Context.Entity.GetInstance<DomainModel.Carts.ShoppingCart>();
          this.productLines = shoppingCart.ShoppingCartLines;
        }

        return this.productLines;
      }

      set
      {
        this.productLines = value;
      }
    }

    /// <summary>
    /// Gets the products.
    /// </summary>
    /// <returns>Returns collection pairs of product code and quantity</returns>
    public IEnumerable<KeyValuePair<string, uint>> GetProducts()
    {
      foreach (RepeaterItem item in this.repProductsList.Items)
      {
        TextBox txtQuantity = item.FindControl("txtQuantity") as TextBox;
        LinkButton btnDelete = item.FindControl("btnDelete") as LinkButton;

        if (btnDelete == null || txtQuantity == null)
        {
          continue;
        }

        string quantity = txtQuantity.Text;
        string productCode = btnDelete.CommandArgument;
        string[] productArgsArray = productCode.Split('|');

        if (productArgsArray.Length > 1)
        {
          productCode = productArgsArray[0];
        }

        uint quant;
        if (!uint.TryParse(quantity, out quant))
        {
          continue;
        }

        yield return new KeyValuePair<string, uint>(productCode, quant);
      }
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">
    /// The source of the event. 
    /// </param>
    /// <param name="e">
    /// The <see cref="System.EventArgs"/> instance containing the event data. 
    /// </param>
    protected void Page_Load(object sender, EventArgs e)
    {
      this.repProductsList.DataSource = this.ProductLines;
      if (!this.IsPostBack)
      {
        this.DataBind();
      }
    }

    /// <summary>
    /// Handles the ItemDataBound event of the repProductsList control.
    /// </summary>
    /// <param name="sender">
    /// The source of the event.
    /// </param>
    /// <param name="e">
    /// The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.
    /// </param>    
    protected void repShoppingCartList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
      // Initialize OrderDisplayMode - start
      Control liHeader = e.Item.FindControl("liHeader");
      Control divImage = e.Item.FindControl("divImage");
      Control divNumber = e.Item.FindControl("divNumber");
      HtmlGenericControl divText = e.Item.FindControl("divText") as HtmlGenericControl;
      Control tdCommands = e.Item.FindControl("tdCommands");
      Control divCountEdit = e.Item.FindControl("divCountEdit");
      Control divCountDisplay = e.Item.FindControl("divCountDisplay");
      Literal litPrice = e.Item.FindControl("litPrice") as Literal;
      Literal litTotalPrice = e.Item.FindControl("litTotalPrice") as Literal;
      LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
      ProductLine productLine = e.Item.DataItem as ProductLine;
      const string attributeClass = "class";
      const string classColImageText = "colImageText";

      switch (this.DisplayMode)
      {
        case OrderDisplayMode.OrderConfirmation:
          {
            if (liHeader != null)
            {
              liHeader.Visible = false;
            }

            if (tdCommands != null)
            {
              tdCommands.Visible = false;
            }

            if (divImage != null)
            {
              divImage.Visible = false;
            }

            if (divNumber != null)
            {
              if (productLine != null)
              {
                if (!string.IsNullOrEmpty(productLine.Product.Code))
                {
                  divNumber.Visible = true;
                }
                else
                {
                  if (divText != null)
                  {
                    divText.Attributes.Add(attributeClass, classColImageText);
                  }
                }

                if (litPrice != null && litTotalPrice != null)
                {
                  if (ShoppingCartSettings.ShowPriceIncVAT)
                  {
                    litPrice.Text = this.FormatPrice(productLine.Totals.PriceIncVat);
                    litTotalPrice.Text = this.FormatPrice(productLine.Totals.TotalPriceIncVat);
                  }
                  else
                  {
                    litPrice.Text = this.FormatPrice(productLine.Totals.PriceExVat);
                    litTotalPrice.Text = this.FormatPrice(productLine.Totals.TotalPriceExVat);
                  }
                }
              }
            }

            if (divCountEdit != null)
            {
              divCountEdit.Visible = false;
            }

            break;
          }

        case OrderDisplayMode.ShoppingCart:
          {
            if (!ShoppingCartSettings.ShowImage)
            {
              if (divImage != null)
              {
                divImage.Visible = false;
              }

              if (divNumber != null)
              {
                if (productLine != null)
                {
                  if (!string.IsNullOrEmpty(productLine.Product.Code))
                  {
                    divNumber.Visible = true;
                  }
                  else
                  {
                    if (divText != null)
                    {
                      divText.Attributes.Add(attributeClass, classColImageText);
                    }
                  }
                }
              }
            }

            if (productLine != null)
            {
              if (btnDelete != null)
              {
                btnDelete.CommandArgument = productLine.Product.Code;
              }

              if (litPrice != null && litTotalPrice != null)
              {
                if (ShoppingCartSettings.ShowPriceIncVAT)
                {
                  litPrice.Text = this.FormatPrice(productLine.Totals.PriceIncVat);
                  litTotalPrice.Text = this.FormatPrice(productLine.Totals.TotalPriceIncVat);
                }
                else
                {
                  litPrice.Text = this.FormatPrice(productLine.Totals.PriceExVat);
                  litTotalPrice.Text = this.FormatPrice(productLine.Totals.TotalPriceExVat);
                }
              }
            }

            if (divCountDisplay != null)
            {
              divCountDisplay.Visible = false;
            }

            break;
          }
      }
    }

    /// <summary>
    /// Gets the Friendly Url
    /// </summary>
    /// <param name="dataItem">
    /// The data item.
    /// </param>
    /// <returns>
    /// Shopping cart item friendly url.
    /// </returns>
    protected string ShoppingCartLineFriendlyUrl(object dataItem)
    {
      string friendlyUrl;
      if (this.DisplayMode == OrderDisplayMode.ShoppingCart)
      {
        ShoppingCartLine shoppingCartItem = (ShoppingCartLine)dataItem;
        friendlyUrl = shoppingCartItem.FriendlyUrl;
      }
      else
      {
        OrderLine orderLine = (OrderLine)dataItem;
        friendlyUrl = orderLine.FriendlyUrl;
      }

      return AnalyticsUtil.AddFollowListToQueryString(friendlyUrl, this.DisplayMode.ToString());
    }

    /// <summary>
    /// Formats the price.
    /// </summary>
    /// <param name="price">The price.</param>
    /// <returns>Returns formated price.</returns>
    protected virtual string FormatPrice(object price)
    {
      return MainUtil.FormatPrice(price, GeneralSettings.DisplayCurrencyOnPrices, ShoppingCartSettings.PriceFormatString, this.Currency.Code);
    }

    #endregion
  }
}