// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateOrderActionPresenter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The order manager actions presenter.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Presenters
{
  using System.Linq;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Apps.OrderManagement.Views;
  using Sitecore.Ecommerce.Merchant.OrderManagement;
  using Sitecore.Ecommerce.OrderManagement;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// The order manager actions presenter.
  /// </summary>
  public class CreateOrderActionPresenter
  {
    /// <summary>
    /// The view.
    /// </summary>
    private readonly ICreateOrderActionView view;

    /// <summary>
    /// The merchant order security.
    /// </summary>
    private readonly MerchantOrderSecurity orderSecurity;

    /// <summary>
    /// The order factory.
    /// </summary>
    private readonly IOrderFactory orderFactory;

    /// <summary>
    /// The order manager.
    /// </summary>
    private readonly MerchantOrderManager orderManager;

    /// <summary>
    /// The state configuration
    /// </summary>
    private readonly MerchantOrderStateConfiguration stateConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderActionPresenter" /> class.
    /// </summary>
    /// <param name="view">The view.</param>
    /// <param name="orderSecurity">The merchant order security.</param>
    /// <param name="orderFactory">The order factory.</param>
    /// <param name="orderManager">The order manager.</param>
    /// <param name="stateConfiguration">The state configuration.</param>
    public CreateOrderActionPresenter([NotNull] ICreateOrderActionView view, [NotNull] MerchantOrderSecurity orderSecurity, [NotNull] IOrderFactory orderFactory, [NotNull] MerchantOrderManager orderManager, [NotNull] MerchantOrderStateConfiguration stateConfiguration)
    {
      Assert.ArgumentNotNull(view, "view");
      Assert.ArgumentNotNull(orderSecurity, "orderSecurity");
      Assert.ArgumentNotNull(orderFactory, "orderFactory");
      Assert.ArgumentNotNull(orderManager, "orderManager");
      Assert.ArgumentNotNull(stateConfiguration, "stateConfiguration");

      this.view = view;
      this.orderSecurity = orderSecurity;
      this.orderFactory = orderFactory;
      this.orderManager = orderManager;
      this.stateConfiguration = stateConfiguration;
    }

    /// <summary>
    /// Queries the state.
    /// </summary>
    public void QueryState()
    {
      this.view.Enabled = this.orderSecurity.CanCreate();
    }

    /// <summary>
    /// Executes this instance.
    /// </summary>
    public void Execute()
    {
      // Create new order using DefaultOrderFactory.
      var order = this.orderFactory.Create();

      // Get Open state from MerchantOrderStateConfiguration.
      var state = this.stateConfiguration.GetStates().Single(s => s.Code == OrderStateCode.Open);

      // Change order state from New to Open.
      order.State = state;

      // Save order with MerchantOrderManager.
      this.orderManager.Save(order);

      // Redirect to order details page.
      this.view.RedirectToOrderDetails(order.OrderId);
    }
  }
}
