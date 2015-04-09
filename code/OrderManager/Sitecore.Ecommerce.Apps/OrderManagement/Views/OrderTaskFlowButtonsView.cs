// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderTaskFlowButtonsView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order task flow buttons class.
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
  /// Defines the order task flow buttons class.
  /// </summary>
  public class OrderTaskFlowButtonsView : TaskFlowButtons, IOrderTaskFlowButtonsView
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
    /// Initializes a new instance of the <see cref="OrderTaskFlowButtonsView"/> class.
    /// </summary>
    public OrderTaskFlowButtonsView()
    {
      this.orderTaskFlowButtonsPresenter = new OrderTaskFlowButtonsPresenter(this, Ecommerce.Context.Entity.Resolve<MerchantOrderManager>(), Ecommerce.Context.Entity.Resolve<MerchantOrderSecurity>(), Ecommerce.Context.Entity.Resolve<PaymentProviderFactory>());

      this.ShowSaveButton = true;
    }

    /// <summary>
    /// Occurs when smart panel is closed.
    /// </summary>
    public event EventHandler SmartPanelClosed;

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
    /// Gets or sets a value indicating whether save button must be shown.
    /// </summary>
    /// <value><c>true</c> if save button is shown; otherwise, <c>false</c>.</value>
    public bool ShowSaveButton { get; set; }

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    public void Refresh()
    {
      foreach (Control control in this.Page.Controls.Flatten<ObjectDetailList>().Concat<Control>(this.Page.Controls.Flatten<ObjectInfoSpot>()))
      {
        control.DataBind();
        ScriptManager.GetCurrent(this.Page).UpdateControl(control);
      }

      foreach (OmActionsObjectDetailList control in this.Page.Controls.Flatten<OmActionsObjectDetailList>())
      {
        control.ReinitializeVisibility();
        control.DataBind();
        ScriptManager.GetCurrent(this.Page).UpdateControl(control);
      }

      var orderDetailsRenderer = this.Page.Controls.Flatten<OrderDetailsRenderer>().FirstOrDefault();
      if (orderDetailsRenderer != null)
      {
        orderDetailsRenderer.Reinitialize();
      }

      foreach (OrderFieldEditor orderFieldEditorControl in this.Page.Controls.Flatten<OrderFieldEditor>())
      {
        if (orderFieldEditorControl.Order != null)
        {
          orderFieldEditorControl.DataBind();
          ScriptManager.GetCurrent(this.Page).UpdateControl(orderFieldEditorControl);
        }
      }
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
    /// Registers the update dependencies.
    /// </summary>
    protected void RegisterUpdateDependencies()
    {
      foreach (Popup popup in this.GetControls<Popup>())
      {
        popup.PopupClose += this.SmartPanelClosedHandler;
      }
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

      this.RegisterUpdateDependencies();

      this.ClearTrackedEntities();

      Control parent = this.Page.FindControl<Control>(this.UpdateControlID) ?? this.Page;
      IEnumerable<OrderFieldEditor> changes = parent.Controls.Flatten<OrderFieldEditor>().ToList();

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
    /// Called when the accept has click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected override void OnAcceptClick([NotNull] object sender, [NotNull] EventArgs e)
    {
      Debug.ArgumentNotNull(sender, "sender");
      Debug.ArgumentNotNull(e, "e");

      IEnumerable<OrderFieldEditor> changes = this.GetControls<OrderFieldEditor>();
      bool validState = this.GetControls<IValidateChangeTracking>().All(c => c.Validate());

      foreach (OrderFieldEditor orderFieldEditorControl in changes)
      {
        if (validState && orderFieldEditorControl.IsChanged && (orderFieldEditorControl.Order != null))
        {
          IDictionary oldValuesHolder = new OrderedDictionary();
          
          orderFieldEditorControl.NewValuesHolder = this.GetEntityChanges(orderFieldEditorControl.Order);
          orderFieldEditorControl.OldValuesHolder = oldValuesHolder;

          orderFieldEditorControl.AcceptChanges();

          this.ExcludeUnchangedFields(orderFieldEditorControl.OldValuesHolder, orderFieldEditorControl.NewValuesHolder);
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

      this.saveButton.Visible = this.ShowSaveButton;
    }

    /// <summary>
    /// Called when the smart panel has closed.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnSmartPanelClosed(EventArgs e)
    {
      EventHandler smartPanelClosedHandler = this.SmartPanelClosed;

      if (smartPanelClosedHandler != null)
      {
        smartPanelClosedHandler(this, e);
      }
    }

    /// <summary>
    /// Initializes the leave without saving warning.
    /// </summary>
    protected override void InitializeLeaveWithoutSavingWarning()
    {
    }

    /// <summary>
    /// Initializes the back to button.
    /// </summary>
    protected override void InitializeBackToButton()
    {
      base.InitializeBackToButton();
      this.backToButton.OnClientClick = "var stored=sessionStorage.getItem('previous');var back=stored?stored:document.referrer;window.location.href=stored;return false;";
      if (!this.Page.IsPostBack)
      {
        ScriptManager.GetCurrent(this.Page).RegisterStartupScript("sessionStorage.setItem('previous', document.referrer);");
      }
    }

    /// <summary>
    /// Gets the controls.
    /// </summary>
    /// <typeparam name="T">Type of controls to return</typeparam>
    /// <returns>The controls.</returns>
    [NotNull]
    protected IEnumerable<T> GetControls<T>()
    {
      Control parent = this.Page.FindControl<Control>(this.UpdateControlID) ?? this.Page;
      return parent.Controls.Flatten<T>();
    }

    /// <summary>
    /// Excludes the unchanged fields.
    /// </summary>
    /// <param name="oldValues">The old values.</param>
    /// <param name="newValues">The new values.</param>
    private void ExcludeUnchangedFields([NotNull] IDictionary oldValues, [NotNull] IDictionary newValues)
    {
      Assert.ArgumentNotNull(oldValues, "oldValues");
      Assert.ArgumentNotNull(newValues, "newValues");

      ICollection<object> keysToRemove = new LinkedList<object>();

      foreach (DictionaryEntry entry in newValues)
      {
        if (oldValues.Contains(entry.Key) && entry.Value.Equals(oldValues[entry.Key]))
        {
          keysToRemove.Add(entry.Key);
        }
      }

      foreach (object keyToRemove in keysToRemove)
      {
        newValues.Remove(keyToRemove);
      }
    }

    /// <summary>
    /// Called when the smart panel has closed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void SmartPanelClosedHandler(object sender, EventArgs e)
    {
      this.OnSmartPanelClosed(e);
    }
  }
}