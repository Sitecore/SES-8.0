// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCart.ascx.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The shopping cart user control.
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
  using System.Linq;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using Analytics.Components;
  using CheckOuts;
  using DomainModel.Carts;
  using DomainModel.CheckOuts;
  using DomainModel.Products;
  using Globalization;
  using Links;
  using Sitecore.Ecommerce.Examples;
  using Text;
  using Utils;

  /// <summary>
  /// The shopping cart user control.
  /// </summary>
  public partial class ShoppingCart : UserControl
  {
    /// <summary>
    /// Continue Shopping label.
    /// </summary>
    private const string ContinueShopping = "Continue Shopping";

    /// <summary>
    /// Gets the shopping cart.
    /// </summary>
    /// <value>The current shopping cart.</value>
    protected virtual DomainModel.Carts.ShoppingCart Cart
    {
      get { return Sitecore.Ecommerce.Context.Entity.GetInstance<DomainModel.Carts.ShoppingCart>(); }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      this.UcProductsListView.repProductsList.ItemCommand += this.UcProductsListView_ItemCommand;
    }

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        AnalyticsUtil.ShoppingCartViewed();

        this.btnEmptyShoppingCart.Text = Translate.Text(Texts.EmptyCart);
        this.btnUpdate.Text = Translate.Text(Texts.UpdateCart);
        this.btnContinueShopping.Text = Translate.Text(Texts.ContinueShopping);
      }

      if (this.Cart.ShoppingCartLines.Count == 0)
      {
        this.litStatus.Text = Translate.Text(Texts.TheShoppingCartIsEmpty);
        this.UcProductsListView.Visible = false;
        this.btnUpdateContainer.Visible = false;
        this.btnProceedToCheckoutContainer.Visible = false;
        this.btnEmptyContainer.Visible = false;
        this.tocArea.Visible = false;
        this.summary.Visible = false;
        this.delivery.Visible = false;
      }

      this.UcProductsListView.Currency = this.Cart.Currency;
      this.summary.Currency = this.Cart.Currency;
      this.delivery.Currency = this.Cart.Currency;

      this.btnProceedToCheckout.Text = Translate.Text(Texts.ProceedToCheckout);
      this.btnProceedToCheckout.ToolTip = Translate.Text(Texts.PleaseReadAndAcceptTheTermsAndConditionsBeforeContinuing);
      this.btnProceedToCheckout.Attributes["disabled"] = Cart.ShoppingCartLines.Count <= 0 || !this.termsOfConditions.Checked ? "disabled" : string.Empty;
    }

    /// <summary>
    /// Handles the Clicked event of the ProceedToCheckout control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The arguments.</param>
    protected void ProceedToCheckoutClicked(object sender, EventArgs args)
    {
      string checkoutPath = ItemUtil.GetNavigationLinkPath("CheckOut");
      if (!string.IsNullOrEmpty(checkoutPath))
      {
        Response.Redirect(checkoutPath);
      }
    }

    /// <summary>
    /// Handles the Click event of the btnEmptyShoppingCart control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnEmptyShoppingCart_Click(object sender, EventArgs e)
    {
      uint numberOfProducts = 0;
      ListString shoppingCartContent = new ListString();
      foreach (ShoppingCartLine line in this.Cart.ShoppingCartLines)
      {
        shoppingCartContent.Add(line.Product.Code);
        numberOfProducts += line.Quantity;
      }

      AnalyticsUtil.ShoppingCartEmptied(shoppingCartContent.ToString(), numberOfProducts);

      this.Cart.ShoppingCartLines.Clear();

      ICheckOut checkOut = Sitecore.Ecommerce.Context.Entity.Resolve<ICheckOut>();
      if (checkOut is CheckOut)
      {
        ((CheckOut)checkOut).ResetCheckOut();
      }

      ItemUtil.RedirectToNavigationLink(ContinueShopping, false);
    }

    /// <summary>
    /// Handles the Click event of the btnContinueShopping control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnContinueShopping_Click(object sender, EventArgs e)
    {
      AnalyticsUtil.ShoppingCartContinueShopping();

      ItemUtil.RedirectToNavigationLink(ContinueShopping, false);
    }

    /// <summary>
    /// Handles the Click event of the btnUpdate control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
      AnalyticsUtil.ShoppingCartUpdated();

      this.UpdateShoppingCart();
    }

    /// <summary>
    /// The update shopping cart.
    /// </summary>
    protected void UpdateShoppingCart()
    {
      foreach (var product in this.UcProductsListView.GetProducts())
      {
        string productCode = product.Key;
        uint quant = product.Value;

        ProductLine existingProductLine = this.Cart.ShoppingCartLines.FirstOrDefault(p => p.Product.Code.Equals(productCode));

        if (existingProductLine == null)
        {
          return;
        }

        IShoppingCartManager shoppingCartManager = Sitecore.Ecommerce.Context.Entity.Resolve<IShoppingCartManager>();
        shoppingCartManager.UpdateProductQuantity(productCode, quant);

        AnalyticsUtil.ShoppingCartItemUpdated(productCode, existingProductLine.Product.Title, quant);
      }

      Sitecore.Ecommerce.Context.Entity.SetInstance(this.Cart);

      this.UpdateTotals(this.Cart);
      this.UpdateProductLines(this.Cart);
    }

    /// <summary>
    /// Handles the ItemCommand event of the UcProductsListView control.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterCommandEventArgs"/> instance containing the event data.</param>
    protected void UcProductsListView_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
      if (e.CommandName != "Delete" || string.IsNullOrEmpty((string)e.CommandArgument))
      {
        return;
      }

      ListString listString = new ListString((string)e.CommandArgument);
      string productCode = listString[0];

      ProductLine existingProductLine = this.Cart.ShoppingCartLines.FirstOrDefault(p => p.Product.Code.Equals(productCode));

      if (existingProductLine != null)
      {
        AnalyticsUtil.ShoppingCartItemRemoved(productCode, existingProductLine.Product.Title, existingProductLine.Quantity);
      }

      IShoppingCartManager shoppingCartManager = Sitecore.Ecommerce.Context.Entity.Resolve<IShoppingCartManager>();
      shoppingCartManager.RemoveProductLine(productCode);

      if (this.Cart.ShoppingCartLines.Count == 0)
      {
        this.Response.Redirect(LinkManager.GetItemUrl(Sitecore.Context.Item));
      }

      Sitecore.Ecommerce.Context.Entity.SetInstance(this.Cart);

      this.UpdateTotals(this.Cart);
      this.UpdateProductLines(this.Cart);
    }

    /// <summary>
    /// Updates the totals.
    /// </summary>
    /// <param name="shoppingCart">The shoppingCart.</param>
    protected void UpdateTotals(DomainModel.Carts.ShoppingCart shoppingCart)
    {
      this.summary.Totals = shoppingCart.Totals;
      this.summary.Currency = shoppingCart.Currency;
      this.summary.DataBind();
    }

    /// <summary>
    /// Updates the product lines.
    /// </summary>
    /// <param name="shoppingCart">The shoppingCart.</param>
    protected void UpdateProductLines(DomainModel.Carts.ShoppingCart shoppingCart)
    {
      this.UcProductsListView.ProductLines = shoppingCart.ShoppingCartLines;
      this.UcProductsListView.Currency = shoppingCart.Currency;
      this.UcProductsListView.DataBind();
    }
  }
}