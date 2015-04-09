// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllowanceChargeSmartPanelButtons.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the AllowanceChargeSmartPanelButtons type.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls.Buttons
{
  using System;
  using System.Linq;
  using System.Web.UI;
  using ComponentModel;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;
  using OrderManagement.ContextStrategies;

  using Sitecore.Ecommerce.OrderManagement;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the allowance charge smart panel buttons class.
  /// </summary>
  public class AllowanceChargeSmartPanelButtons : ObservableOmSmartPanelButtons<AllowanceCharge>
  {
    /// <summary>
    /// The merchant order manager
    /// </summary>
    private MerchantOrderManager orderManager;

    /// <summary>
    /// Order line resolver.
    /// </summary>
    private UblEntityResolvingStrategy ublEntityResolver;

    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>
    /// The order manager.
    /// </value>
    [NotNull]
    public virtual MerchantOrderManager OrderManager
    {
      get
      {
        return this.orderManager ?? (this.orderManager = Ecommerce.Context.Entity.Resolve<MerchantOrderManager>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        this.orderManager = value;
      }
    }

    /// <summary>
    /// Gets or sets the order processor.
    /// </summary>
    /// <value>
    /// The order processor.
    /// </value>
    [NotNull]
    public override UblEntityResolvingStrategy UblEntityResolver
    {
      get
      {
        return this.ublEntityResolver ?? (this.ublEntityResolver = new OnSmartPanelAllowanceChargeStrategy());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.ublEntityResolver = value;
      }
    }

    /// <summary>
    /// Called when the save has click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <exception cref="NotImplementedException">Such action is not supported.</exception>
    /// <exception cref="NotSupportedException">Such action is not supported.</exception>
    protected override void OnSaveClick([CanBeNull]object sender, [CanBeNull]EventArgs e)
    {
      base.OnSaveClick(sender, e);

      this.PerformUpdate();
    }

    /// <summary>
    /// Performs the update.
    /// </summary>
    private void PerformUpdate()
    {
      AllowanceCharge allowanceCharge = this.TrackedEntities.FirstOrDefault();

      if (allowanceCharge == null)
      {
        return;
      }

      Binder.Current.GetModelBinderFactory().GetBinder(typeof(AllowanceCharge)).BindToModel(allowanceCharge, this.GetEntityChanges(allowanceCharge));

      Order order = this.UblEntityResolver.GetEntity(allowanceCharge.Alias) as Order;

      Assert.IsNotNull(order, "Order cannot be null.");

      try
      {
        this.OrderManager.Save(order);
        ScriptManager.GetCurrent(this.Page).Message(new Message(Ecommerce.Texts.TheAllowanceChargeHasBeenChanged) { Sticky = false, Type = MessageType.Info });
      }
      catch (SaveOrdersException)
      {
        ScriptManager.GetCurrent(this.Page).Message(new Message(Ecommerce.Texts.UnableToSaveTheChangesTheAllowanceChargeAmountIsIncorrect) { Sticky = false, Type = MessageType.Error });
      }
    }
  }
}
