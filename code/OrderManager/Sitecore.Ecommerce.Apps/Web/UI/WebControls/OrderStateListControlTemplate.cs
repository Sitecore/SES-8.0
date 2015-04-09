// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStateListControlTemplate.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderStateListControlTemplate type.
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
  using System.Collections.Specialized;
  using System.Web.UI;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;
  using OrderManagement;
  using OrderManagement.Presenters;
  using OrderManagement.Views;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the state substate control template class.
  /// </summary>
  public class OrderStateListControlTemplate : ControlTemplate<OrderStateList>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStateListControlTemplate"/> class.
    /// </summary>
    public OrderStateListControlTemplate()
    {
      this.StateUnlocalizer = new StateUnlocalizer();
    }    

    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>The order.</value>
    protected Order Order { get; set; }

    /// <summary>
    /// Gets or sets the state unlocalizer.
    /// </summary>
    /// <value>The state unlocalizer.</value>
    protected StateUnlocalizer StateUnlocalizer { get; set; }

    /// <summary>
    /// Instantiates the in.
    /// </summary>
    /// <param name="container">The container.</param>
    public override void InstantiateIn(Control container)
    {
      Assert.ArgumentNotNull(container, "container");

      FieldEditor fieldEditor = container.BindingContainer as FieldEditor;

      if (fieldEditor != null)
      {
        this.Order = (Order)fieldEditor.DataItem;
      }

      base.InstantiateIn(container);
    }

    /// <summary>
    /// Extracts the values.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <returns>The values.</returns>
    public override IOrderedDictionary ExtractValues(Control container)
    {
      Assert.ArgumentNotNull(container, "container");

      IOrderedDictionary result = base.ExtractValues(container);

      if (this.Field != null)
      {
        OrderStateList orderStateListControl = this.FindControlInContainer(container);

        if (orderStateListControl != null)
        {
          result.Add(this.Field.Name, this.StateUnlocalizer.UnlocalizeState(orderStateListControl.CurrentState));
        }
      }

      return result;
    }

    /// <summary>
    /// Instantiates the control.
    /// </summary>
    /// <returns>The control.</returns>
    protected override OrderStateList InstantiateControl()
    {
      OrderStateList orderStateListControl = base.InstantiateControl();
      OrderStateListView orderStateListView = new OrderStateListView(orderStateListControl);

      orderStateListControl.ID = "OrderStateListControl";

      (new OrderStateListPresenter(orderStateListView, this.Order, Ecommerce.Context.Entity.Resolve<MerchantOrderStateConfiguration>(), Ecommerce.Context.Entity.Resolve<OrderStateListValidator>())).Initialize();

      orderStateListControl.Enabled = !this.Field.ReadOnly;

      return orderStateListControl;
    }
  }
}