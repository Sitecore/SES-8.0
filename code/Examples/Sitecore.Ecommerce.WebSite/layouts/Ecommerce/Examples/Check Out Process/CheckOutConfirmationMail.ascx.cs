// -------------------------------------------------------------------------------------------
// <copyright file="CheckOutConfirmationMail.ascx.cs" company="Sitecore Corporation">
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
  using DomainModel.Orders;
  using DomainModel.Users;
  using Globalization;

  /// <summary>
  /// </summary>
  public partial class CheckOutConfirmationMail : UserControl
  {
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      string key = string.IsNullOrEmpty(this.Request.QueryString["key"]) ? string.Empty : Uri.UnescapeDataString(this.Request.QueryString["key"]);
      string orderId = string.IsNullOrEmpty(this.Request.QueryString["orderid"]) ? string.Empty : Uri.UnescapeDataString(this.Request.QueryString["orderid"]);

      // var orderId = "";
      bool displayOrderConfirmation = false;

      if (!string.IsNullOrEmpty(key))
      {
        IOrderManager<Order> orderProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IOrderManager<Order>>();

        string encryptKey = Uri.UnescapeDataString(key);
        if (!string.IsNullOrEmpty(encryptKey))
        {
          orderId = Crypto.DecryptTripleDES(encryptKey, "5dfkjek5");
        }

        Order order = orderProvider.GetOrder(orderId);
        if (order == null)
        {
          this.HideOrderAndDisplayError(string.Format(Translate.Text(Sitecore.Ecommerce.Examples.Texts.OrderWithOrderIdCouldNotBeFound, true), orderId));
        }
        else
        {
          this.ShoppingCartAndOrderView.DataEntity = order;
          displayOrderConfirmation = true;
        }
      }
      else if (!string.IsNullOrEmpty(orderId))
      {
        ICustomerManager<CustomerInfo> customerManager = Sitecore.Ecommerce.Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
        IOrderManager<Order> orderProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IOrderManager<Order>>();
        Order order = orderProvider.GetOrder(orderId);
        if (order != null)
        {
          if (customerManager.CurrentUser != null)
          {
            if (customerManager.CurrentUser.CustomerId != order.CustomerInfo.CustomerId)
            {
              this.Response.Redirect("/");
            }
            else
            {
              this.ShoppingCartAndOrderView.DataEntity = order;
              displayOrderConfirmation = true;
            }
          }
        }
      }

      // If the order shouldn't be displayed show errormessage.
      if (!displayOrderConfirmation)
      {
        this.HideOrderAndDisplayError(string.Format(Translate.Text(Sitecore.Ecommerce.Examples.Texts.OrderWithOrderIdCouldNotBeFound), orderId));
      }
      else
      {
        this.ShowStatusMessage(Sitecore.Context.Item["Short description"]);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="errormessage">
    /// </param>
    private void HideOrderAndDisplayError(string errormessage)
    {
      this.litStatusMessage.Text = errormessage;
      this.ShoppingCartAndOrderView.Visible = false;
    }

    /// <summary>
    /// </summary>
    /// <param name="message">
    /// </param>
    private void ShowStatusMessage(string message)
    {
      this.litStatusMessage.Text = message;
    }
  }
}