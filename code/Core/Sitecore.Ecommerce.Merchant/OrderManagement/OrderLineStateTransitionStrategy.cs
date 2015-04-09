// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineStateTransitionStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineStateTransitionStrategy type.
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

namespace Sitecore.Ecommerce.Merchant.OrderManagement
{
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Logging;
  using Sitecore.Pipelines;

  /// <summary>
  /// Defines the order line state transition strategy class.
  /// </summary>
  public class OrderLineStateTransitionStrategy : OrderProcessingStrategy
  {
    /// <summary>
    /// Gets or sets the order line status.
    /// </summary>
    /// <value>The order line status.</value>
    public string OrderLineState { get; set; }

    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>The order manager.</value>
    public MerchantOrderManager OrderManager { get; set; }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The result.</returns>
    public override string ProcessOrder([NotNull] Order order, IDictionary<string, object> parameters)
    {
      Assert.IsNotNull(this.OrderManager, "OrderManager");
      Assert.IsNotNullOrEmpty(this.OrderLineState, "OrderLineState");

      Assert.ArgumentNotNull(order, "order");

      Assert.IsTrue(order.State.Code == "In Process", "Unable to process order which is not in \"In Process\" state.");
      Assert.IsFalse(order.State.Substates.Single(substate => substate.Code == string.Format("{0} In Full", this.OrderLineState)).Active, string.Format("Unable to process order which is already in \"{0} In Full\" state.", this.OrderLineState));

      Substate partialSubstate = order.State.Substates.SingleOrDefault(substate => substate.Code == string.Format("{0} Partially", this.OrderLineState));

      if (partialSubstate != null)
      {
        partialSubstate.Active = false;
      }

      order.State.Substates.Single(substate => substate.Code == string.Format("{0} In Full", this.OrderLineState)).Active = true;
      foreach (OrderLine orderLine in order.OrderLines)
      {
        orderLine.LineItem.LineStatusCode = this.OrderLineState;
      }

      this.OrderManager.Save(order);
      this.StartOrderPackedPipeline(order);

      return SuccessfulResult;
    }

    /// <summary>
    /// Gets logging entry for sucess.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>Logging Entry.</returns>
    public override LogEntry GetLogEntryForSuccess(Order order)
    {
      Assert.IsNotNullOrEmpty(this.OrderLineState, "OrderLineState");

      Assert.ArgumentNotNull(order, "order");

      return new LogEntry
      {
        EntityID = order.OrderId,
        EntityType = Constants.OrderEntityType,
        Action = string.Format(Constants.ProcessInFullAction, this.OrderLineState),
        LevelCode = Constants.UserLevel,
        Result = Constants.ApprovedResult,
        Details = new LogEntryDetails(Constants.InProcessSubstateSuccessfullyChanged, order.State.Substates.Any(substate => (substate.Code == string.Format("{0} Partially", this.OrderLineState)) && substate.Active) ? string.Format("{0} Partially", this.OrderLineState) : string.Format("Not {0}", this.OrderLineState), string.Format("{0} In Full", this.OrderLineState))
      };
    }

    /// <summary>
    /// Performs additional fail processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The get addtional logging entry for fail.</returns>
    public override LogEntry GetLogEntryForFail(Order order, params object[] parameters)
    {
      Assert.IsNotNullOrEmpty(this.OrderLineState, "OrderLineState");

      Assert.ArgumentNotNull(order, "order");

      return new LogEntry
      {
        EntityID = order.OrderId,
        EntityType = Constants.OrderEntityType,
        Action = string.Format(Constants.ProcessInFullAction, this.OrderLineState),
        LevelCode = Constants.UserLevel,
        Result = Constants.DeniedResult,
        Details = new LogEntryDetails(Constants.InProcessSubstateFailedToChange, order.State.Substates.Any(substate => (substate.Code == string.Format("{0} Partially", this.OrderLineState)) && substate.Active) ? string.Format("{0} Partially", this.OrderLineState) : string.Format("Not {0}", this.OrderLineState), string.Format("{0} In Full", this.OrderLineState))
      };
    }

    /// <summary>
    /// Starts the order packed pipeline.
    /// </summary>
    /// <param name="order">The order.</param>
    protected virtual void StartOrderPackedPipeline(Order order)
    {
      Assert.IsNotNullOrEmpty(this.OrderLineState, "OrderLineState");

      Assert.ArgumentNotNull(order, "order");

      PipelineArgs pipelineArgs = new PipelineArgs();
      pipelineArgs.CustomData.Add("order", order);

      Pipeline.Start(string.Format("order{0}", this.OrderLineState), pipelineArgs);
    }
  }
}