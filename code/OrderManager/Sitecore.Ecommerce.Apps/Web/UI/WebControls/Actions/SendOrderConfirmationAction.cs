// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SendOrderConfirmationAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the send order confirmation action class.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls.Actions
{
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Logging;
  using Sitecore.Ecommerce.Apps.OrderManagement.Views;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.WebControls.Extensions;

  /// <summary>
  /// Defines the send order confirmation action class.
  /// </summary>
  public class SendOrderConfirmationAction : ScriptManagedAction
  {
    /// <summary>
    /// Executes the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public override void Execute(ActionContext context)
    {
      base.Execute(context);
      this.Refresh(context);
    }    

    /// <summary>
    /// Executes the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    protected override void Execute([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");
      this.OrderId = order.OrderId;

      OrderConfirmation orderConfirmation = Context.Entity.Resolve<OrderConfirmation>();
      Assert.IsNotNull(orderConfirmation, "OrderConfirmation cannot be null.");
      Assert.IsNotNull(orderConfirmation.ConfirmationMessageBuilder, "OrderConfirmation.ConfirmationMessageBuilder cannot be null.");

      orderConfirmation.ConfirmationMessageBuilder.Order = order;
      orderConfirmation.Send();
    }

    /// <summary>
    /// Performs the post steps.
    /// </summary>
    protected override void PerformPostSteps()
    {
      if (System.Web.HttpContext.Current != null)
      {
        this.ScriptManager.Message(new Message(Ecommerce.Texts.AnOrderConfirmationHasBeenSentToTheCustomer) { Sticky = false, Type = MessageType.Info });
      }

      LogEntry logEntry = new LogEntry
      {
        Details = new LogEntryDetails(Constants.OrderConfirmationSent),
        Action = Constants.SendOrderConfirmationAction,
        EntityID = this.OrderId,
        EntityType = Constants.OrderEntityType,
        LevelCode = Constants.UserLevel,
        Result = Constants.ApprovedResult
      };

      this.Logger.Write(logEntry);
    }

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    /// <param name="context">The context.</param>
    private void Refresh(ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      OrderTaskFlowButtonsView orderTaskFlowButtonsView = context.Owner.Page.Controls.Flatten<OrderTaskFlowButtonsView>().SingleOrDefault();

      if (orderTaskFlowButtonsView != null)
      {
        orderTaskFlowButtonsView.Refresh();
      }
    }
  }
}