// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStateProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the PackOrderProcessingStrategy type.
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

  /// <summary>
  /// Defines the pack order processing strategy class.
  /// </summary>
  public class OrderStateProcessingStrategy : OrderProcessingStrategy
  {
    /// <summary>
    /// The order state configuration.
    /// </summary>
    private readonly MerchantOrderStateConfiguration orderStateConfiguration;

    /// <summary>
    /// Logging entries list.
    /// </summary>
    private Order initialOrder;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStateProcessingStrategy" /> class.
    /// </summary>
    /// <param name="orderStateConfiguration">The order state configuration.</param>
    public OrderStateProcessingStrategy(MerchantOrderStateConfiguration orderStateConfiguration)
    {
      this.orderStateConfiguration = orderStateConfiguration;
    }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// The result.
    /// </returns>
    public override string ProcessOrder([NotNull] Order order, [NotNull]IDictionary<string, object> parameters)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(parameters, "parameters");

      if (parameters.ContainsKey("oldOrder"))
      {
        this.initialOrder = parameters["oldOrder"] as Order;
      }

      Assert.IsNotNull(this.initialOrder, "Unable to process the order. Initial order should be supplied in parameters.");
      Assert.IsNotNull(this.orderStateConfiguration, "Unable to process the order. OrderStateConfiguration cannot be null.");
      Assert.IsTrue(this.orderStateConfiguration.IsValid(order.State), "Unable to process the order. Substate combination is not valid.");

      return SuccessfulResult;
    }

    /// <summary>
    /// Gets logging entry for sucess.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// Logging Entry.
    /// </returns>
    [NotNull]
    public override LogEntry GetLogEntryForSuccess([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      return this.GetLogEntry(order, Constants.OrderStatusSet, Constants.ApprovedResult);
    }

    /// <summary>
    /// Performs additional fail processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// The get addtional logging entry for fail.
    /// </returns>
    public override LogEntry GetLogEntryForFail([NotNull] Order order, [CanBeNull] params object[] parameters)
    {
      Assert.ArgumentNotNull(order, "order");

      return this.GetLogEntry(order, Constants.OrderStatusSetFailed, Constants.DeniedResult);
    }

    /// <summary>
    /// Gets the logging entry.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="template">The template.</param>
    /// <param name="result">The result.</param>
    /// <returns>
    /// The logging entry.
    /// </returns>
    [NotNull]
    internal virtual LogEntry GetLogEntry([NotNull] Order order, [NotNull] string template, [NotNull] string result)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(template, "template");
      Assert.ArgumentNotNull(result, "result");
      Assert.IsNotNull(this.initialOrder, "Unable to get logging entries. Initial order cannot be null.");

      return new LogEntry
      {
        Details = new LogEntryDetails(
          template,
          this.initialOrder.State.Name,
          this.initialOrder.State.Substates.Aggregate(new System.Text.StringBuilder(), (sb, substate) => substate.Active ? (sb.Length == 0 ? sb : sb.Append("; ")).Append(substate.Name) : sb),
          order.State.Name,
          order.State.Substates.Aggregate(new System.Text.StringBuilder(), (sb, substate) => substate.Active ? (sb.Length == 0 ? sb : sb.Append("; ")).Append(substate.Name) : sb)),
        Action = Constants.UpdateOrderAction,
        EntityID = order.OrderId,
        EntityType = Constants.OrderEntityType,
        LevelCode = Constants.UserLevel,
        Result = result
      };
    }
  }
}
