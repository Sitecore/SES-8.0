// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateOrderAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderManagerActions type.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Views
{
  using System.Web;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Apps.OrderManagement.Presenters;
  using Sitecore.Ecommerce.Merchant.OrderManagement;
  using Sitecore.Ecommerce.OrderManagement;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// The create order action.
  /// </summary>
  public class CreateOrderAction : Action,  ICreateOrderActionView
  {
    /// <summary>
    /// The presenter.
    /// </summary>
    private readonly CreateOrderActionPresenter presenter;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderAction" /> class.
    /// </summary>
    public CreateOrderAction()
    {
      var security = Context.Entity.Resolve<MerchantOrderSecurity>();
      var orderFactory = Context.Entity.Resolve<IOrderFactory>();
      var orderManager = Context.Entity.Resolve<MerchantOrderManager>();
      var stateConfiguration = Context.Entity.Resolve<MerchantOrderStateConfiguration>();

      this.presenter = new CreateOrderActionPresenter(this, security, orderFactory, orderManager, stateConfiguration);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="ICreateOrderActionView" /> is enabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if visible; otherwise, <c>false</c>.
    /// </value>
    public bool Enabled { get; set; }

    /// <summary>
    /// Redirects to create order.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    public void RedirectToOrderDetails([NotNull] string orderId)
    {
      Assert.ArgumentNotNull(orderId, "orderId");

      string url = string.Format("{0}ordermanager/order.aspx?orderId={1}", Speak.Extensions.Extensions.GetVirtualFolder(), orderId);

      HttpContext.Current.Response.Redirect(url);
    }

    /// <summary>
    /// Executes the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public override void Execute([CanBeNull] ActionContext context)
    {
      this.presenter.Execute();
    }

    /// <summary>
    /// Queries the state.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The state.
    /// </returns>
    public override ElementState QueryState([NotNull] ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      this.presenter.QueryState();

      return this.Enabled ? ElementState.Enabled : ElementState.Disabled;
    }
  }
}
