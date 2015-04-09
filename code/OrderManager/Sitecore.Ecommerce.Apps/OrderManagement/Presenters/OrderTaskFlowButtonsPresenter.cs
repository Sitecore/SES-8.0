// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderTaskFlowButtonsPresenter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order updater class.
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
  using System;
  using System.Collections;
  using System.Collections.Specialized;
  using System.Linq;
  using ComponentModel;
  using Data;
  using Diagnostics;
  using DomainModel.Payments;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Globalization;
  using Merchant.OrderManagement;
  using Utils;
  using Views;

  /// <summary>
  /// Defines the order updater class.
  /// </summary>
  public class OrderTaskFlowButtonsPresenter
  {
    /// <summary>
    /// Stores reference to the view.
    /// </summary>
    private readonly IOrderTaskFlowButtonsView view;

    /// <summary>
    /// Stores reference to the merchant order manager.
    /// </summary>
    private readonly MerchantOrderManager merchantOrderManager;

    /// <summary>
    /// Stores reference to the order security instance;
    /// </summary>
    private readonly MerchantOrderSecurity orderSecurity;

    /// <summary>
    ///  PaymentProviderFactory instance.
    /// </summary>
    private PaymentProviderFactory paymentProviderFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderTaskFlowButtonsPresenter"/> class.
    /// </summary>
    /// <param name="view">The view.</param>
    /// <param name="merchantOrderManager">The merchant order manager.</param>
    /// <param name="orderSecurity">The order security.</param>
    /// <param name="paymentProviderFactory">The payment provider factory.</param>
    public OrderTaskFlowButtonsPresenter([NotNull] IOrderTaskFlowButtonsView view, [NotNull] MerchantOrderManager merchantOrderManager, [NotNull] MerchantOrderSecurity orderSecurity,  PaymentProviderFactory paymentProviderFactory)
    {
      Assert.ArgumentNotNull(view, "view");
      Assert.ArgumentNotNull(merchantOrderManager, "merchantOrderManager");
      Assert.ArgumentNotNull(orderSecurity, "orderSecurity");

      this.view = view;
      this.view.SmartPanelClosed += this.SmartPanelClosedHandler;
      this.merchantOrderManager = merchantOrderManager;
      this.orderSecurity = orderSecurity;
      this.paymentProviderFactory = paymentProviderFactory;
    }

    /// <summary>
    /// Gets the view.
    /// </summary>
    /// <value>The view.</value>
    [NotNull]
    protected IOrderTaskFlowButtonsView View
    {
      get { return this.view; }
    }

    /// <summary>
    /// Gets the merchant order manager.
    /// </summary>
    /// <value>The merchant order manager.</value>
    [NotNull]
    protected MerchantOrderManager MerchantOrderManager
    {
      get { return this.merchantOrderManager; }
    }

    /// <summary>
    /// Gets the order security.
    /// </summary>
    /// <value>The order security.</value>
    [NotNull]
    protected MerchantOrderSecurity OrderSecurity
    {
      get { return this.orderSecurity; }
    }

    /// <summary>
    /// Sets the control visibility.
    /// </summary>
    public void SetControlVisibility()
    {
      this.View.ShowSaveButton = this.View.TrackedEntities.Any(order => this.orderSecurity.CanProcess(order) || this.orderSecurity.CanReopen(order));
    }

    /// <summary>
    /// Performs the update.
    /// </summary>
    public void PerformUpdate()
    {
      try
      {
        this.MerchantOrderManager.OrderSaved += this.ShowMessages;

        foreach (Order order in this.View.TrackedEntities)
        {
          IDictionary changes = this.View.GetEntityChanges(order);

          if (!this.ConfirmPayment(changes, order))
          {
            return;
          }

          this.ApplyChanges(order, changes);
          this.MerchantOrderManager.Save(order);

          if (this.View is OrderQuickInfoSmartPanelButtonsView)
          {
            ((OrderQuickInfoSmartPanelButtonsView)this.View).Close();
          }
        }

        this.View.Refresh();
        this.View.ShowNonStickyInformationalMessage(Texts.TheChangesToTheOrderHaveBeenSaved);
      }
      catch (InvalidStateConfigurationException)
      {
        this.View.ShowNonStickyErrorMessage(Texts.UnableToSaveTheOrderTheOrderStateOrSubstateIsInvalid);
      }
      finally
      {
        this.MerchantOrderManager.OrderSaved -= this.ShowMessages;
      }
    }

    /// <summary>
    /// Shows the messages.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The arguments.</param>
    public void ShowMessages([NotNull] object sender, [NotNull] SaveEntityEventArgs<Order> args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      var newState = args.NewEntity.State;
      Assert.IsNotNull(newState, "Unable to show message. State of the changed order cannot be null.");

      if (newState.Code != OrderStateCode.InProcess)
      {
        return;
      }

      var oldState = args.OldEntity.State;
      Assert.IsNotNull(oldState, "Unable to show message. State of the initial order cannot be null.");

      this.ShowMessage(oldState, newState, OrderStateCode.InProcessCapturedInFull, Texts.OrderStateChangedToCaptured);
      this.ShowMessage(oldState, newState, OrderStateCode.InProcessPackedInFull, Texts.OrderStateChangedToPacked);
      this.ShowMessage(oldState, newState, OrderStateCode.InProcessShippedInFull, Texts.OrderStateChangedToShipped);
    }

    /// <summary>
    /// Applies the changes.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="changes">The changes.</param>
    protected virtual void ApplyChanges([NotNull] Order order, [NotNull] IDictionary changes)
    {
      Debug.ArgumentNotNull(order, "order");
      Debug.ArgumentNotNull(changes, "changes");

      changes = this.GetFilteredChanges(order, changes);

      foreach (DictionaryEntry change in changes)
      {
        PropertyUtil.EnsurePropertyAccessibility(order, change.Key.ToString());
      }

      Binder.Current.GetModelBinderFactory().GetBinder(typeof(Order)).BindToModel(order, changes);
    }

    /// <summary>
    /// Gets the filtered changes.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="changes">The changes.</param>
    /// <returns>The filtered changes.</returns>
    [NotNull]
    protected virtual IDictionary GetFilteredChanges([NotNull] Order order, [NotNull] IDictionary changes)
    {
      Debug.ArgumentNotNull(order, "order");
      Debug.ArgumentNotNull(changes, "changes");

      IDictionary result = new OrderedDictionary();

      foreach (DictionaryEntry dictionaryEntry in changes)
      {
        string stringValue = dictionaryEntry.Value as string;

        if ((Binder.Current.GetModelBinderFactory().GetBinder(typeof(Order)).GetValue(order, dictionaryEntry.Key.ToString()) != null) || (string.Empty != stringValue))
        {
          result.Add(dictionaryEntry.Key, dictionaryEntry.Value);
        }
      }

      return result;
    }

    /// <summary>
    /// Shows the message.
    /// </summary>
    /// <param name="oldState">The old state.</param>
    /// <param name="newState">The new state.</param>
    /// <param name="substateCode">The substate code.</param>
    /// <param name="message">The message.</param>
    private void ShowMessage([NotNull] State oldState, [NotNull] State newState, [NotNull] string substateCode, [NotNull] string message)
    {
      Debug.ArgumentNotNull(oldState, "oldState");
      Debug.ArgumentNotNull(newState, "newState");
      Debug.ArgumentNotNull(substateCode, "substateCode");
      Debug.ArgumentNotNull(message, "message");

      var oldSubstate = oldState.Substates.SingleOrDefault(s => s.Code == substateCode);
      var newSubstate = newState.Substates.SingleOrDefault(s => s.Code == substateCode);

      if ((oldState.Code != OrderStateCode.InProcess || (oldState.Code == OrderStateCode.InProcess && oldSubstate != null && !oldSubstate.Active)) &&
        newSubstate != null && newSubstate.Active)
      {
        this.view.ShowNonStickyInformationalMessage(message);
      }
    }

    /// <summary>
    /// Confirms the payment.
    /// </summary>
    /// <param name="changes">The changes.</param>
    /// <param name="order">The order.</param>
    /// <returns>
    /// The payment.
    /// </returns>
    private bool ConfirmPayment(IDictionary changes, Order order)
    {
      var stateChanged = changes.Values.OfType<State>().FirstOrDefault(s => s.Code == OrderStateCode.InProcess);
      if (stateChanged != null && this.IsOrderReserved(order))
      {
        if (order.ReservationTicket.CapturedAmount == 0)
        {
          var substateChanged = stateChanged.Substates.SingleOrDefault(s => s.Code == OrderStateCode.InProcessCapturedInFull && s.Active);
          if (substateChanged != null)
          {
            return this.View.ShowConfirmationDialog(Translate.Text(Texts.DoYouWantToProceed), Translate.Text(Texts.TheFullAmountWillBeCapturedFromTheCustomerAccountIfYouProceed));
          }
        }
      }

      return true;
    }

    /// <summary>
    /// Determines whether [is order reserved] [the specified order].
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    ///   <c>true</c> if [is order reserved] [the specified order]; otherwise, <c>false</c>.
    /// </returns>
    private bool IsOrderReserved(Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      Assert.IsNotNull(order.PaymentMeans, "Order payment means cannot be null.");

      return this.paymentProviderFactory.GetProvider(order.PaymentMeans.PaymentChannelCode) is IReservable;
    }

    /// <summary>
    /// Smarts the panel closed handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void SmartPanelClosedHandler(object sender, EventArgs e)
    {
      this.view.Refresh();
    }
  }
}