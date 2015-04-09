// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the PrintAction type.
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
  using Sitecore.Ecommerce.Apps.OrderManagement.Views;
  using Sitecore.Web.UI.WebControls.Extensions;
  using Speak.Extensions;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Logging;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the print action class.
  /// </summary>
  public class PrintAction : ScriptManagedAction
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

      string url = string.Format("{0}ordermanager/printorder?orderid={1}&sc_lang={2}", Extensions.GetVirtualFolder(), this.OrderId, Sitecore.Context.Language.Name);
      string script = string.Format("window.open('{0}','PrintMe','resizable=yes,scrollbars=yes,location=no');", url);

      this.ScriptManager.RegisterStartupScript(script);
    }

    /// <summary>
    /// Performs the post steps.
    /// </summary>
    protected override void PerformPostSteps()
    {
      LogEntry logEntry = new LogEntry
      {
        Details = new LogEntryDetails(Constants.OrderPrinted),
        Action = Constants.PrintOrderAction,
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