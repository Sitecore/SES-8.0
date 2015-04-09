// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStateListView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderStateListView type.
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
  using System;
  using System.Collections.Generic;
  using System.Web.UI.WebControls;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Models;
  using Web.UI.WebControls;

  /// <summary>
  /// Defines the order state list view class.
  /// </summary>
  public class OrderStateListView : IOrderStateListView
  {
    /// <summary>
    /// Stores reference to the instance of OrderStateList control.
    /// </summary>
    private readonly OrderStateList orderStateList;

    /// <summary>
    /// Occurs when order state list view substate is created.
    /// </summary>
    public event EventHandler<OrderStateListViewSubstateCreatedEventArgs> OrderStateListViewSubstateCreated;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStateListView"/> class.
    /// </summary>
    /// <param name="orderStateList">The order state list.</param>
    public OrderStateListView([NotNull] OrderStateList orderStateList)
    {
      Assert.ArgumentNotNull(orderStateList, "orderStateList");

      this.orderStateList = orderStateList;
    }

    /// <summary>
    /// Gets the order state list.
    /// </summary>
    /// <value>The order state list.</value>
    [NotNull]
    protected OrderStateList OrderStateList
    {
      get
      {
        return this.orderStateList;
      }
    }

    /// <summary>
    /// Sets the states to display.
    /// </summary>
    /// <param name="stateList">The state list.</param>
    /// <param name="currentState">State of the current.</param>
    public void SetUpControls([NotNull] IEnumerable<StateModel> stateList, [NotNull] State currentState)
    {
      Assert.ArgumentNotNull(stateList, "stateList");
      Assert.ArgumentNotNull(currentState, "currentState");

      this.OrderStateList.DataSource = stateList;
      this.OrderStateList.CurrentState = currentState;

      this.OrderStateList.SubstateControlDataBound += this.OnSubstateControlDataBound;
    }

    /// <summary>
    /// Called when the substate control data has bound.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnSubstateControlDataBound([NotNull] object sender, [NotNull] SubstateControlDataBoundEventArgs e)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(e, "e");

      OrderStateList control = sender as OrderStateList;

      if (control == null)
      {
        return;
      }

      EventHandler<OrderStateListViewSubstateCreatedEventArgs> handler = this.OrderStateListViewSubstateCreated;
      if (handler != null)
      {
        OrderStateListViewSubstateCreatedEventArgs eventArgs = new OrderStateListViewSubstateCreatedEventArgs(control.CurrentState, e.Substate);
        
        handler(this, eventArgs);
        
        ((WebControl)e.Control).Enabled = eventArgs.Enabled;
      }
    }
  }
}