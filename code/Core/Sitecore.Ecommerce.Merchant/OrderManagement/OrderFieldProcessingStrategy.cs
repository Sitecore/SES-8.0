// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderFieldProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderFieldProcessingStrategy type.
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
  using System.Reflection;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Logging;
  using Utils;

  /// <summary>
  /// Defines the order field processing strategy class.
  /// </summary>
  public class OrderFieldProcessingStrategy : OrderProcessingStrategy
  {
    /// <summary>
    /// Stores reference to the list of logging entries.
    /// </summary>
    private readonly ICollection<LogEntry> loggingEntries = new List<LogEntry>();

    /// <summary>
    /// Stores reference to the previous field values.
    /// </summary>
    private readonly IDictionary<string, object> previousFieldValues;

    /// <summary>
    /// Stores reference to the current order.
    /// </summary>
    private Order order;

    /// <summary>
    /// Gets the previous field values.
    /// </summary>
    /// <value>The previous field values.</value>
    [NotNull]
    protected IDictionary<string, object> PreviousFieldValues
    {
      get
      {
        return this.previousFieldValues;
      }
    }

    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <value>The order.</value>
    [CanBeNull]
    protected Order Order
    {
      get
      {
        return this.order;
      }
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
    /// Initializes a new instance of the <see cref="OrderFieldProcessingStrategy"/> class.
    /// </summary>
    /// <param name="previousFieldValues">The previous field values.</param>
    public OrderFieldProcessingStrategy([NotNull] IDictionary<string, object> previousFieldValues)
    {
      Assert.ArgumentNotNull(previousFieldValues, "previousFieldValues");

      this.previousFieldValues = previousFieldValues;
    }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The result.</returns>
    [NotNull]
    public override string ProcessOrder([NotNull] Order order, [CanBeNull] IDictionary<string, object> parameters)
    {
      Assert.ArgumentNotNull(order, "order");

      this.order = order;
      
      this.LoggingEntries.Clear();
      this.EvaluateLoggingEntries();

      this.order = null;

      return SuccessfulResult;
    }

    /// <summary>
    /// Evaluates the logging entries.
    /// </summary>
    protected virtual void EvaluateLoggingEntries()
    {
      if (this.Order != null)
      {
        foreach (KeyValuePair<string, object> pair in this.PreviousFieldValues)
        {
          this.LoggingEntries.Add(
            new LogEntry
            {
              Details = new LogEntryDetails(Constants.PropertySet, pair.Key, pair.Value, PropertyUtil.GetPropertyValue(this.order, pair.Key)),
              EntityID = this.Order.OrderId,
              EntityType = Constants.OrderEntityType,
              Action = Constants.UpdateOrderAction,
              LevelCode = Constants.UserLevel,
              Result = Constants.ApprovedResult
            });
        }
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
