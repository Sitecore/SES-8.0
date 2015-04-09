// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStateListValidator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderStateListValidator type.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement
{
  using System.Web;
  using System.Web.UI;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Sitecore.Ecommerce.Merchant.OrderManagement;
  using Sitecore.Web.UI.WebControls;
  using Views;

  /// <summary>
  /// Defines the state substate checker class.
  /// </summary>
  public class OrderStateListValidator
  {
    /// <summary>
    /// ScriptManager instance.
    /// </summary>
    private ScriptManager scriptManager;

    /// <summary>
    /// StateValidator instance.
    /// </summary>
    private StateValidator stateValidator;

    /// <summary>
    /// Gets or sets the script manager.
    /// </summary>
    /// <value>
    /// The script manager.
    /// </value>
    [CanBeNull]
    public virtual ScriptManager ScriptManager
    {
      get
      {
        return this.scriptManager ?? (this.scriptManager = ScriptManager.GetCurrent((Page)HttpContext.Current.Handler));
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.scriptManager = value;
      }
    }

    /// <summary>
    /// Gets or sets the state validator.
    /// </summary>
    /// <value>
    /// The state validator.
    /// </value>
    [NotNull]
    public virtual StateValidator StateValidator
    {
      get
      {
        return this.stateValidator ?? (this.stateValidator = new StateValidator());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.stateValidator = value;
      }
    }

    /// <summary>
    /// Checks the availability of captured in full.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="e">The <see cref="Sitecore.Ecommerce.Apps.OrderManagement.Views.OrderStateListViewSubstateCreatedEventArgs"/> instance containing the event data.</param>
    public virtual void CheckAvailabilityOfCapturedInFull([NotNull] Order order, [NotNull] OrderStateListViewSubstateCreatedEventArgs e)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(e, "e");

      bool checkingResult = this.StateValidator.CanBeCaptured(order, e.Substate.Code);
      if (!checkingResult)
      {
        e.Enabled = false;
      }
    }

    /// <summary>
    /// Shows the warning if captured in full not available.
    /// </summary>
    /// <param name="order">The order.</param>
    public virtual void ShowWarningIfCapturedInFullNotAvailable([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      if (order.State != null)
      {
        if (order.State.Code == OrderStateCode.InProcess)
        {
          if (!this.StateValidator.CanBeCaptured(order, OrderStateCode.InProcessCapturedInFull))
          {
            this.ScriptManager.Message(new Message(Texts.TheTotalPayableAmountExceedsTheAmountThatIsReservedOnThePaymentProviderSideTheOrderCannotBeCapturedInFull) { Sticky = false, Type = MessageType.Warning });
          }
        }
      }
    }
  }
}