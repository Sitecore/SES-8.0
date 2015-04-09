// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderFieldEditor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order field editor class.
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
  using OrderManagement.Views;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.WebControls.Extensions;

  /// <summary>
  /// Defines the order field editor class.
  /// </summary>
  public class OrderFieldEditor : BaseFieldEditor
  {
    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <value>The order.</value>
    [CanBeNull]
    public Order Order
    {
      get { return (Order)this.DataItem; }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      Debug.ArgumentNotNull(e, "e");

      base.OnPreRender(e);

      IOrderDetailsView view = this.Page.Items["OrderDetailsView"] as IOrderDetailsView;
      Assert.IsNotNull(view, "Unable to render Order Field Editor. View cannot be null.");

      bool needsDataBind = false;
      foreach (DataField field in this.fieldEditorLeft.Fields.Concat(this.fieldEditorRight.Fields))
      {
        if (!field.ReadOnly && view.IsReadOnly)
        {
          needsDataBind = true;
          field.ReadOnly = true;
        }

        if (field.Name == "State")
        {
          needsDataBind = true;
          field.ReadOnly = field.ReadOnly && !view.CanReopenOrder;
        }
      }

      if (needsDataBind)
      {
        OrderStateList orderStateList = this.fieldEditorLeft.Controls.Flatten<OrderStateList>().Concat(this.fieldEditorRight.Controls.Flatten<OrderStateList>()).SingleOrDefault();
        string persistedValue = null;
        bool disableSubstateAnimation = false;

        if (orderStateList != null)
        {
          persistedValue = orderStateList.GetValue();
          disableSubstateAnimation = orderStateList.DisableSubstateAnimation;
        }

        this.fieldEditorLeft.DataBind();
        this.fieldEditorRight.DataBind();

        orderStateList = this.fieldEditorLeft.Controls.Flatten<OrderStateList>().Concat(this.fieldEditorRight.Controls.Flatten<OrderStateList>()).SingleOrDefault();

        if (orderStateList != null)
        {
          orderStateList.SetValue(persistedValue);
          orderStateList.DisableSubstateAnimation = disableSubstateAnimation;
        }
      }
    }
  }
}