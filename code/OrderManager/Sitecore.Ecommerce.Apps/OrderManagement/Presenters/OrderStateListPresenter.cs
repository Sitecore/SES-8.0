// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStateListPresenter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderStateListPresenter type.
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
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;
  using Models;
  using Views;

  /// <summary>
  /// Defines the order state list presenter class.
  /// </summary>
  public class OrderStateListPresenter
  {
    /// <summary>
    /// Stores reference to the view.
    /// </summary>
    private readonly IOrderStateListView view;

    /// <summary>
    /// Stores reference to the order which state is represented by the view.
    /// </summary>
    private readonly Order order;

    /// <summary>
    /// Stores reference to the MerchantOrderStateConfiguration instance.
    /// </summary>
    private readonly MerchantOrderStateConfiguration merchantOrderStateConfiguration;

    /// <summary>
    /// StateSubstateChecker instance.
    /// </summary>
    private readonly OrderStateListValidator orderStateListValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStateListPresenter"/> class.
    /// </summary>
    /// <param name="view">The view.</param>
    /// <param name="order">The order.</param>
    /// <param name="merchantOrderStateConfiguration">The merchant order state configuration.</param>
    /// <param name="orderStateListValidator">The order state list validator.</param>
    public OrderStateListPresenter([NotNull] IOrderStateListView view, [NotNull] Order order, [NotNull] MerchantOrderStateConfiguration merchantOrderStateConfiguration, [NotNull] OrderStateListValidator orderStateListValidator)
    {
      Assert.ArgumentNotNull(view, "view");
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(merchantOrderStateConfiguration, "merchantOrderStateConfiguration");
      Assert.ArgumentNotNull(orderStateListValidator, "orderStateListValidator");

      this.view = view;
      this.order = order;
      this.merchantOrderStateConfiguration = merchantOrderStateConfiguration;
      this.orderStateListValidator = orderStateListValidator;
    }

    /// <summary>
    /// Gets the view.
    /// </summary>
    /// <value>The view.</value>
    [NotNull]
    protected IOrderStateListView View
    {
      get
      {
        return this.view;
      }
    }

    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <value>The order.</value>
    [NotNull]
    protected Order Order
    {
      get
      {
        return this.order;
      }
    }

    /// <summary>
    /// Gets the merchant order state configuration.
    /// </summary>
    /// <value>The merchant order state configuration.</value>
    [NotNull]
    protected MerchantOrderStateConfiguration MerchantOrderStateConfiguration
    {
      get
      {
        return this.merchantOrderStateConfiguration;
      }
    }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    protected internal void Initialize()
    {
      this.View.SetUpControls(this.GetStates(this.Order), this.Order.State);
      this.View.OrderStateListViewSubstateCreated += this.OnOrderStateListViewSubstateCreated;
    }

    /// <summary>
    /// Gets the states.
    /// </summary>
    /// <param name="sourceOrder">The source order.</param>
    /// <returns>The states.</returns>
    [NotNull]
    protected virtual IQueryable<StateModel> GetStates([NotNull] Order sourceOrder)
    {
      Assert.ArgumentNotNull(sourceOrder, "sourceOrder");

      return new MerchantOrderStateConfigurationAdapter(this.MerchantOrderStateConfiguration).GetStates(sourceOrder);
    }

    /// <summary>
    /// Called when the order state list view substate has created.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="Sitecore.Ecommerce.Apps.OrderManagement.Views.OrderStateListViewSubstateCreatedEventArgs"/> instance containing the event data.</param>
    [NotNull]
    protected virtual void OnOrderStateListViewSubstateCreated([NotNull] object sender, [NotNull] OrderStateListViewSubstateCreatedEventArgs e)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(e, "e");

      e.Enabled = this.MerchantOrderStateConfiguration.GetAdmissibleSubstates(e.State).Any(substate => substate.Code == e.Substate.Code);

      this.orderStateListValidator.CheckAvailabilityOfCapturedInFull(this.Order, e);
    }
  }
}