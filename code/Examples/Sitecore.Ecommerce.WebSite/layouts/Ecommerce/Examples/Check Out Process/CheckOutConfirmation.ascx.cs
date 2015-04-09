// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckOutConfirmation.ascx.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Checkout confirmation page control
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
  using System.Web.UI;
  using DomainModel.Orders;
  using DomainModel.Users;
  using Globalization;
  using Sitecore.Ecommerce.Examples;
  using Sitecore.Security.Accounts;
  using Utils;

  /// <summary>
  ///   Checkout confirmation page control.
  /// </summary>
  public partial class CheckOutConfirmation : UserControl
  {
    /// <summary>
    /// The navigation link next title.
    /// </summary>
    private static readonly string NavigatationLinkShop = "Continue Shopping";

    /// <summary>
    /// Current order.
    /// </summary>
    private Order currentOrder;

    /// <summary>
    /// Gets the current order.
    /// </summary>
    /// <returns>Returns Order.</returns>
    protected virtual Order CurrentOrder
    {
      get
      {
        if (this.currentOrder == null && !string.IsNullOrEmpty(this.GetOrderId()))
        {
          IOrderManager<Order> orderManager = Sitecore.Ecommerce.Context.Entity.Resolve<IOrderManager<Order>>();
          this.currentOrder = orderManager.GetOrder(this.GetOrderId());
        }

        return this.currentOrder;
      }
    }

    /// <summary>
    /// Gets a value indicating whether [create account visibility].
    /// </summary>
    /// <value>
    /// <c>true</c> if [create account visibility]; otherwise, <c>false</c>.
    /// </value>
    private bool CreateAccountVisibility
    {
      get
      {
        bool userExist = true;
        if (this.CurrentOrder != null)
        {
          userExist = User.Exists(string.Format("{0}\\{1}", Sitecore.Context.Domain.Name, this.CurrentOrder.CustomerInfo.Email));
        }

        return !Sitecore.Context.User.IsAuthenticated && !this.IsPrintDevice() && !userExist;
      }
    }

    /// <summary>
    /// Gets the order id.
    /// </summary>
    /// <returns>Returns order ID.</returns>
    protected virtual string GetOrderId()
    {
      string key = string.IsNullOrEmpty(this.Request.QueryString["key"]) ? string.Empty : Uri.UnescapeDataString(this.Request.QueryString["key"]);
      string orderId = string.IsNullOrEmpty(this.Request.QueryString["orderid"]) ? string.Empty : Uri.UnescapeDataString(this.Request.QueryString["orderid"]);

      if (!string.IsNullOrEmpty(key))
      {
        string encryptKey = Uri.UnescapeDataString(key);
        if (!string.IsNullOrEmpty(encryptKey))
        {
          orderId = Crypto.DecryptTripleDES(encryptKey, "5dfkjek5");
        }
      }

      return orderId;
    }

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      this.ShowNavigationButtons();

      Order order = this.CurrentOrder;
      string orderId = this.GetOrderId();

      ICustomerManager<CustomerInfo> customerManager = Sitecore.Ecommerce.Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();

      if (order != null)
      {
        if (customerManager.CurrentUser != null)
        {
          if (customerManager.CurrentUser.CustomerId != order.CustomerInfo.CustomerId)
          {
            this.Response.Redirect("/");
          }
        }

        this.ShowStatusMessage(Sitecore.Context.Item["Short description"]);
        this.ShowFormTitle(Sitecore.Context.Item["Title"]);
        this.ShoppingCartAndOrderView.DataEntity = order;
        this.CreateAcccount.Visible = this.CreateAccountVisibility;
      }
      else
      {
        this.HideOrderAndDisplayError(string.Format(Translate.Text(Texts.OrderWithOrderIdCouldNotBeFound), orderId));
        this.CreateAcccount.Visible = false;
        this.btnPrintOrder.Visible = false;
      }
    }

    /// <summary> navigate to shop </summary>
    /// <param name="sender">Object sender</param>
    /// <param name="args">Event arguments</param>
    protected void BtnGotoShop_Click(object sender, EventArgs args)
    {
      ItemUtil.RedirectToNavigationLink(NavigatationLinkShop, false);
    }

    /// <summary>
    /// Hides the order and display error.
    /// </summary>
    /// <param name="errormessage">The errormessage.</param>
    private void HideOrderAndDisplayError(string errormessage)
    {
      this.litStatusMessage.Text = errormessage;
      this.ShoppingCartAndOrderView.Visible = false;
    }

    /// <summary>
    /// Shows the status message.
    /// </summary>
    /// <param name="message">The message.</param>
    private void ShowStatusMessage(string message)
    {
      this.litStatusMessage.Text = message;
    }

    /// <summary>
    /// Shows the form title.
    /// </summary>
    /// <param name="message">The message.</param>
    private void ShowFormTitle(string message)
    {
      this.litTitle.Text = message;
    }

    /// <summary> Dispay navigation buttons </summary>
    private void ShowNavigationButtons()
    {
      this.btnGotoShop.Visible = !this.IsPrintDevice();
      this.btnPrintOrder.Visible = !this.IsPrintDevice();
      this.btnGotoShop.Text = ItemUtil.GetNavigationLinkTitle(NavigatationLinkShop, true);
      this.btnPrintOrder.Value = Translate.Text(Texts.PrintThisPage);
    }

    /// <summary>
    /// Determines whether [is print device].
    /// </summary>
    /// <returns>
    /// <c>true</c> if [is print device]; otherwise, <c>false</c>.
    /// </returns>
    private bool IsPrintDevice()
    {
      return string.Compare(Sitecore.Context.Device.Name, "print", true) == 0;
    }
  }
}