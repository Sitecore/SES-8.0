// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderQuickInfoSmartPanelButtonsView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderQuickInfoSmartPanelButtonsView type.
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
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Linq;
  using System.Web.UI;
  using ComponentModel;
  using Diagnostics;
  using DomainModel.Payments;
  using Ecommerce.OrderManagement.Orders; 
  using Merchant.OrderManagement;
  using Presenters;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.WebControls.Extensions;
  using Speak.Extensions;
  using Speak.Web.UI.WebControls;
  using Web.UI.WebControls;

  /// <summary>
  /// Defines the order quick info smart panel buttons view class.
  /// </summary>
  public class OrderQuickInfoSmartPanelButtonsView : SmartPanelButtons, IOrderTaskFlowButtonsView
  {
    /// <summary>
    /// Stores reference to the dictionary of changed orders and corresponding orderChanges.
    /// </summary>
    private readonly IDictionary<Order, IDictionary> orderChanges = new Dictionary<Order, IDictionary>();

    /// <summary>
    /// Stores reference to the update order command.
    /// </summary>
    private readonly OrderTaskFlowButtonsPresenter orderTaskFlowButtonsPresenter;

    /// <summary>
    /// Occurs when smart panel is closed.
    /// </summary>
    public event EventHandler SmartPanelClosed;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderQuickInfoSmartPanelButtonsView"/> class.
    /// </summary>
    public OrderQuickInfoSmartPanelButtonsView()
    {
      this.orderTaskFlowButtonsPresenter = new OrderTaskFlowButtonsPresenter(this, Ecommerce.Context.Entity.Resolve<MerchantOrderManager>(), Ecommerce.Context.Entity.Resolve<MerchantOrderSecurity>(), Ecommerce.Context.Entity.Resolve<PaymentProviderFactory>());
      this.ShowSaveButton = true;
    }

    /// <summary>
    /// Gets or sets a value indicating whether save button must be shown.
    /// </summary>
    /// <value><c>true</c> if save button is shown; otherwise, <c>false</c>.</value>
    public bool ShowSaveButton { get; set; }

    /// <summary>
    /// Gets the changed orders.
    /// </summary>
    /// <value>The changed orders.</value>
    [NotNull]
    public IEnumerable<Order> TrackedEntities
    {
      get { return this.orderChanges.Keys; }
    }

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    public void Refresh()
    {
    }

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public void Close()
    {
      ((PopupPage)this.Page).Close(null, true);
    }

    /// <summary>
    /// Shows the non sticky info message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void ShowNonStickyInformationalMessage([NotNull] string message)
    {
      Assert.ArgumentNotNull(message, "message");

      this.Page.ShowNonStickyInfoMessage(message);
    }

    /// <summary>
    /// Shows the non sticky error message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void ShowNonStickyErrorMessage([NotNull] string message)
    {
      Assert.ArgumentNotNull(message, "message");

      this.Page.ShowNonStickyErrorMessage(message);
    }

    /// <summary>
    /// Shows the confirmation dialog.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="description">The description.</param>
    /// <returns>
    /// The confirmation dialog.
    /// </returns>
    public bool ShowConfirmationDialog([NotNull] string message, [NotNull] string description)
    {
      Assert.ArgumentNotNull(message, "message");
      Assert.ArgumentNotNull(description, "description");

      return this.Page.ShowConfirmationDialog(message, description);
    }

    /// <summary>
    /// Gets the order orderChanges.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>The order orderChanges.</returns>
    public IDictionary GetEntityChanges(Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      return this.orderChanges[order];
    }

    /// <summary>
    /// Clears the tracked entities.
    /// </summary>
    protected void ClearTrackedEntities()
    {
      this.orderChanges.Clear();
    }

    /// <summary>
    /// Tracks the entity.
    /// </summary>
    /// <param name="order">The order.</param>
    protected void TrackEntity([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      if (!this.orderChanges.ContainsKey(order))
      {
        this.orderChanges.Add(order, new OrderedDictionary());
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");

      base.OnLoad(e);

      this.ClearTrackedEntities();

      Control parent = this.Page.FindControl<Control>(this.UpdateControlID) ?? this.Page;
      IEnumerable<OrderFieldEditor> changes = parent.Controls.Flatten<OrderFieldEditor>();

      foreach (OrderFieldEditor orderFieldEditorControl in changes)
      {
        if (orderFieldEditorControl.Order != null)
        {
          this.TrackEntity(orderFieldEditorControl.Order);
        }
      }

      this.orderTaskFlowButtonsPresenter.SetControlVisibility();
    }

    /// <summary>
    /// Called when the save has click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected override void OnSaveClick([NotNull] object sender, [NotNull] EventArgs e)
    {
      Debug.ArgumentNotNull(sender, "sender");
      Debug.ArgumentNotNull(e, "e");

      Control parent = this.Page.FindControl<Control>(this.UpdateControlID) ?? this.Page;
      IEnumerable<OrderFieldEditor> changes = parent.Controls.Flatten<OrderFieldEditor>().ToArray();

      foreach (OrderFieldEditor orderFieldEditorControl in changes)
      {
        if (orderFieldEditorControl.Order != null)
        {
          orderFieldEditorControl.NewValuesHolder = this.GetEntityChanges(orderFieldEditorControl.Order);
        }
      }

      bool validState = parent.Controls.Flatten<IValidateChangeTracking>().All(c => c.Validate(null));
      foreach (IChangeTracking control in changes)
      {
        if (validState && control.IsChanged)
        {
          control.AcceptChanges();
        }

        if (string.IsNullOrEmpty(this.UpdateControlID) && (control != null))
        {
          ScriptManager.GetCurrent(this.Page).UpdateControl((Control)control);
        }
      }

      if (!string.IsNullOrEmpty(this.UpdateControlID))
      {
        Control control = this.Page.FindControl<Control>(this.UpdateControlID);
        if (control != null)
        {
          ScriptManager.GetCurrent(this.Page).UpdateControl(control);
        }
      }

      this.orderTaskFlowButtonsPresenter.PerformUpdate();
    }



    /// <summary>
    /// Called when the pre has render.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      this.Page.RegisterStylesheet("OrderManagerStyles", "/styles/OrderManager.css");

      this.btnSave.Visible = this.ShowSaveButton;
    }
  }
}