// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineProcessingStrategy type.
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
  /// Defines the order line processing strategy class.
  /// </summary>
  public abstract class OrderLineProcessingStrategy : OrderProcessingStrategy
  {
    /// <summary>
    /// Available states of the order.
    /// </summary>
    private readonly string[] availableStates = { "In Process", "New", "Open", "In Process" };

    /// <summary>
    /// Denied substates of the order.
    /// </summary>
    private readonly string[] deniedSubstates = { "Shipped In Full", "Captured In Full" };

    /// <summary>
    /// Set of substate transitions.
    /// </summary>
    private readonly IDictionary<string, List<string>> substateDeactivations =
      new Dictionary<string, List<string>>
      {
        { "In Process", new List<string> { "Packed In Full" } }
      };

    /// <summary>
    /// Stores additional logging entries for success.
    /// </summary>
    private readonly IList<LogEntry> additionalLoggingEntriesForSuccess = new List<LogEntry>();

    /// <summary>
    /// Gets Additional logging entry.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>List of additional logging entries.</returns>
    public override IList<LogEntry> GetAdditionalLogEntriesForSuccess(Order order)
    {
      return this.additionalLoggingEntriesForSuccess;
    }

    /// <summary>
    /// Sets the order states.
    /// </summary>
    /// <param name="order">The order.</param>
    protected virtual void SetOrderStates([NotNull] Order order)
    {
      Assert.IsNotNull(order, "order");

      Assert.IsTrue(
        this.availableStates.Any(avs => avs == order.State.Code),
        "Unable to add new order line to the order that is not in \"New\", \"Open\" or \"In Process\" state.");


      Substate[] deniedSubstatesMatches = order.State.Substates.Where(ss => this.deniedSubstates.Any(ds => ds == ss.Code)).ToArray();

      foreach (Substate substate in deniedSubstatesMatches)
      {
        Assert.IsFalse(
          substate.Active,
          "Unable to add new order line to the order that is in \"Shipped In Full\" or \"Captured In Full\" substate.");
      }

      foreach (string state in this.substateDeactivations.Keys)
      {
        if (order.State.Code == state)
        {
          foreach (string substate in this.substateDeactivations[order.State.Code])
          {
            string substateStep = substate;
            Substate substateToDeactivate = order.State.Substates.Single(ss => ss.Code == substateStep);

            if (substateToDeactivate.Active)
            {
              substateToDeactivate.Active = false;

              this.AddSubstateDeactivationLogEntry(order, substateToDeactivate);
            }
          }
        }
      }
    }

    /// <summary>
    /// Adds the substate transition log entry.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="substateToDeactivate">The substate deactivate.</param>
    protected virtual void AddSubstateDeactivationLogEntry([NotNull] Order order, [NotNull] Substate substateToDeactivate)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(substateToDeactivate, "substateToDeactivate");

      LogEntry logEntry = new LogEntry
      {
        Details = new LogEntryDetails(
          Constants.OrderStatusSet,
          order.State.Name,
          order.State.Substates.Aggregate(
            new System.Text.StringBuilder(),
            (sb, substate) => substate.Active ^ (substate.Code == substateToDeactivate.Code) ? (sb.Length == 0 ? sb : sb.Append("; "))
              .Append(substate.Name) : sb),
          order.State.Name,
          order.State.Substates.Aggregate(
            new System.Text.StringBuilder(),
            (sb, substate) => substate.Active ? (sb.Length == 0 ? sb : sb.Append("; "))
              .Append(substate.Name) : sb)),
        EntityID = order.OrderId,
        EntityType = Constants.OrderEntityType,
        Action = Constants.UpdateOrderAction,
        LevelCode = Constants.UserLevel,
        Result = Constants.ApprovedResult
      };

      this.additionalLoggingEntriesForSuccess.Add(logEntry);
    }

    /// <summary>
    /// Updates the taxes.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="orderLine">The order line.</param>
    /// <returns>
    /// The taxes for order.
    /// </returns>
    [NotNull]
    protected abstract Order UpdateTaxesForOrder(Order order, OrderLine orderLine);
  }
}