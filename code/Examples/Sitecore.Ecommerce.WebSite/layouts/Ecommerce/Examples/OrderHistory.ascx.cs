// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderHistory.ascx.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Order history user control.
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

namespace Sitecore.Ecommerce.layouts.Ecommerce
{
  using System;
  using System.Linq;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using Globalization;
  using OrderManagement;
  using OrderManagement.Orders;
  using Visitor.OrderManagement;

  /// <summary>
  /// Order history user control.
  /// </summary>
  public partial class OrderHistory : UserControl
  {
    /// <summary>
    /// Cancelled state.
    /// </summary>
    private const string CancelledState = "Cancelled";

    /// <summary>
    ///   VisitorOrderManager instance.
    /// </summary>
    private VisitorOrderManager orderRepository;

    /// <summary>
    ///  VisitorOrderProcessorBase instance.
    /// </summary>
    private VisitorOrderProcessorBase orderProcessor;

    /// <summary>
    ///  VisitorOrderSecurity instance.
    /// </summary>
    private VisitorOrderSecurity orderSecurity;

    /// <summary>
    /// Gets or sets the order repository.
    /// </summary>
    /// <value>
    /// The order repository.
    /// </value>
    public virtual VisitorOrderManager OrderRepository
    {
      get
      {
        return this.orderRepository ?? (this.orderRepository = Sitecore.Ecommerce.Context.Entity.Resolve<VisitorOrderManager>());
      }

      set
      {
        this.orderRepository = value;
      }
    }

    /// <summary>
    /// Gets or sets the order processor.
    /// </summary>
    /// <value>
    /// The order processor.
    /// </value>
    public virtual VisitorOrderProcessorBase OrderProcessor
    {
      get
      {
        return this.orderProcessor ?? (this.orderProcessor = Sitecore.Ecommerce.Context.Entity.Resolve<VisitorOrderProcessorBase>());
      }

      set
      {
        this.orderProcessor = value;
      }
    }

    /// <summary>
    /// Gets or sets the order security.
    /// </summary>
    /// <value>
    /// The order security.
    /// </value>
    public virtual VisitorOrderSecurity OrderSecurity
    {
      get
      {
        return this.orderSecurity ?? (this.orderSecurity = Sitecore.Ecommerce.Context.Entity.Resolve<VisitorOrderSecurity>());
      }

      set
      {
        this.orderSecurity = value;
      }
    }

    /// <summary>
    /// Gets the quantity.
    /// </summary>
    /// <param name="dataItem">The data item.</param>
    /// <returns>The quantity of all products.</returns>
    protected static decimal GetQuantity(object dataItem)
    {
      Order order = dataItem as Order;
      if (order == null)
      {
        return 0;
      }

      uint itemsInShoppingCart = 0;
      order.OrderLines.ToList().ForEach(p => itemsInShoppingCart += (uint)p.LineItem.Quantity);
      return itemsInShoppingCart;
    }


    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      this.orderList.DataSource = this.OrderRepository.GetAll().Where(o => o.ShopContext == Sitecore.Context.Site.Name).ToList();
      this.orderList.DataBind();
    }

    /// <summary>
    /// Handles the Command event of the CancelOrderLink control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void CancelOrderLink_Click(object sender, EventArgs e)
    {
      LinkButton linkButton = (LinkButton)sender;
      string orderNumber = linkButton.CommandArgument;
      Order order = this.GetOrder(orderNumber);
      if (order != null)
      {
        this.OrderProcessor.CancelOrder(order);

        Response.Redirect(Request.RawUrl);
      }
    }

    /// <summary>
    /// Handles the DataBind event of the CancelOrderLink control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void CancelOrderLink_DataBind(object sender, EventArgs e)
    {
      LinkButton linkButton = (LinkButton)sender;
      string orderNumber = linkButton.CommandArgument;
      Order order = this.GetOrder(orderNumber);
      if (order != null)
      {
        bool isCancelled = order.State != null && order.State.Code == CancelledState;
        bool canCancel = this.OrderSecurity.CanCancel(order);
        linkButton.Text = Translate.Text(isCancelled ? Texts.TheOrderIsCancelled : (canCancel ? Texts.CancelOrder : Texts.TheOrderCannotBeCancelled));
        linkButton.Enabled = !isCancelled && canCancel;
      }
    }

    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <returns>
    /// The order.
    /// </returns>
    private Order GetOrder(string orderNumber)
    {
      return this.OrderRepository.GetAll().FirstOrDefault(o => o.OrderId == orderNumber);
    }
  }
}