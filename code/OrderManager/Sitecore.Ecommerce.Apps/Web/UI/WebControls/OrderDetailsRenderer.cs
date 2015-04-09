// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderDetailsRenderer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order details renderer class.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls
{
  using System;
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;
  using OrderManagement;
  using OrderManagement.Presenters;
  using OrderManagement.Views;
  using Sitecore.Web.UI.WebControls.Extensions;
  using Speak.Web.UI.WebControls;

  /// <summary>
  /// Defines the order details renderer class.
  /// </summary>
  public class OrderDetailsRenderer : ChildRenderer
  {
    /// <summary>
    /// The presenter.
    /// </summary>
    private OrderDetailsPresenter presenter;

    /// <summary>
    /// Reinitializes this instance.
    /// </summary>
    public void Reinitialize()
    {
      this.presenter.Initialize();
      this.Page.Items["OrderDetailsView"] = this.presenter.View;
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      Debug.ArgumentNotNull(e, "e");

      base.OnLoad(e);

      this.GetOrder(); // INFO: do not remove this "magic" call, otherwise the following line will not work.
      OrderDetailsView view = new OrderDetailsView { Order = this.GetOrder() };

      this.presenter = new OrderDetailsPresenter(view) 
      { 
        OrderSecurity = Ecommerce.Context.Entity.Resolve<MerchantOrderSecurity>(), 
        OrderStateListValidator = Ecommerce.Context.Entity.Resolve<OrderStateListValidator>() 
      };
      this.presenter.Initialize();

      this.Page.Items["OrderDetailsView"] = view;
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");

      base.OnPreRender(e);
    }

    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <returns>The order.</returns>
    [CanBeNull]
    private Order GetOrder()
    {
      OrderFieldEditor fieldEditor = this.Controls.Flatten<OrderFieldEditor>().FirstOrDefault();

      return fieldEditor != null ? fieldEditor.Order : null;
    }
  }
}