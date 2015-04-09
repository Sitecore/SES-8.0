// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllowanceChargeProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the AllowanceChargeAmountValueProcessingStrategy type.
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
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Common;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Logging;
  using Utils;

  /// <summary>
  /// Defines the allowance charge amount value processing strategy class.
  /// </summary>
  public class ChangeAllowanceChargeAmountProcessingStrategy : OrderProcessingStrategy
  {
    /// <summary>
    /// The list of logging entries.
    /// </summary>
    private readonly IList<LogEntry> loggingEntries = new List<LogEntry>();

    /// <summary>
    /// The initial order from database.
    /// </summary>
    private Order initialOrder;

    /// <summary>
    /// The changed order from presentation.
    /// </summary>
    private Order changedOrder;

    /// <summary>
    /// The previous field values.
    /// </summary>
    private IDictionary<string, object> previousFieldValues;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeAllowanceChargeAmountProcessingStrategy"/> class.
    /// </summary>
    /// <param name="inintialOrder">The inintial order.</param>
    /// <param name="previousFieldValues">The previous field values.</param>
    public ChangeAllowanceChargeAmountProcessingStrategy([NotNull] Order inintialOrder, [NotNull] IDictionary<string, object> previousFieldValues)
    {
      Assert.ArgumentNotNull(inintialOrder, "inintialOrder");
      Assert.ArgumentNotNull(previousFieldValues, "previousFieldValues");

      this.initialOrder = inintialOrder;
      this.previousFieldValues = previousFieldValues;
    }

    /// <summary>
    /// Gets the previous field values.
    /// </summary>
    [NotNull]
    protected IDictionary<string, object> PreviousFieldValues
    {
      get { return this.previousFieldValues; }
    }


    /// <summary>
    /// Gets the logging entries.
    /// </summary>
    /// <value>The logging entries.</value>
    [NotNull]
    protected ICollection<LogEntry> LoggingEntries
    {
      get
      {
        return this.loggingEntries;
      }
    }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// The result.
    /// </returns>
    public override string ProcessOrder(Order order, IDictionary<string, object> parameters)
    {
      this.changedOrder = order;

      this.CheckOrderTotals();

      this.LoggingEntries.Clear();

      this.EvaluateLoggingEntries();

      return SuccessfulResult;
    }

    /// <summary>
    /// Checks the order totals.
    /// </summary>
    /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
    /// <exception cref="SaveOrdersException">Incorrect discounts values. Order AnticipatedMonetaryTotals cannot be negative.</exception>
    protected virtual void CheckOrderTotals()
    {
      try
      {
        Amount taxInclusiveAmount = this.changedOrder.AnticipatedMonetaryTotal.TaxInclusiveAmount;
        Amount payableAmount = this.changedOrder.AnticipatedMonetaryTotal.PayableAmount;
      }
      catch (ArgumentException exception)
      {
        if (exception.Message == "Amount cannot be negative.")
        {
          this.RevertAllowanceChargesAmounts();
          throw new SaveOrdersException("Incorrect discounts values. Order AnticipatedMonetaryTotals cannot be negative.", exception);
        }

        throw;
      }
    }

    /// <summary>
    /// Reverts the allowance charges amounts.
    /// </summary>
    protected virtual void RevertAllowanceChargesAmounts()
    {
      foreach (var changedCharge in this.changedOrder.AllowanceCharge)
      {
        var inintialCharge = this.initialOrder.AllowanceCharge.SingleOrDefault(ac => ac.Alias == changedCharge.Alias);

        if (inintialCharge != null && inintialCharge.Amount.Value != changedCharge.Amount.Value)
        {
          changedCharge.Amount = inintialCharge.Amount;
        }
      }
    }

    /// <summary>
    /// Evaluates the logging entries.
    /// </summary>
    protected virtual void EvaluateLoggingEntries()
    {
      foreach (KeyValuePair<string, object> pair in this.PreviousFieldValues)
      {
        this.LoggingEntries.Add(
          new LogEntry
          {
            Details = new LogEntryDetails(Constants.PropertySet, pair.Key, pair.Value, PropertyUtil.GetPropertyValue(this.changedOrder, pair.Key)),
            EntityID = this.changedOrder.OrderId,
            EntityType = Constants.OrderEntityType,
            Action = Constants.UpdateOrderAction,
            LevelCode = Constants.UserLevel,
            Result = Constants.ApprovedResult
          });
      }
    }


    /// <summary>
    /// Gets logging entry for sucess.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>Logging Entry.</returns>
    public override LogEntry GetLogEntryForSuccess(Order order)
    {
      return this.LoggingEntries.First();
    }

    /// <summary>
    /// Performs additional fail processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The get addtional logging entry for fail.</returns>
    public override LogEntry GetLogEntryForFail(Order order, params object[] parameters)
    {
      return new LogEntry();
    }

    /// <summary>
    /// Performs additional success processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// The get addtional logging entry for sucess.
    /// </returns>
    public override IList<LogEntry> GetAdditionalLogEntriesForSuccess(Order order)
    {
      return this.LoggingEntries.Skip(1).ToList();
    }
  }
}
