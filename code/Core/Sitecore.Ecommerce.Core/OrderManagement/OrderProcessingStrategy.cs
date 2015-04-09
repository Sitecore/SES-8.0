// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order processing strategy class.
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

namespace Sitecore.Ecommerce.OrderManagement
{
  using System.Collections.Generic;
  using Globalization;
  using Logging;
  using Orders;

  /// <summary>
  /// Defines the order processing strategy class.
  /// </summary>
  public abstract class OrderProcessingStrategy
  {
    /// <summary>
    /// Successful result.
    /// </summary>
    public static readonly string SuccessfulResult = Translate.Text("Success");

    /// <summary>
    /// Failed result.
    /// </summary>
    public static readonly string FailedResult = Translate.Text("Fail");

    /// <summary>
    /// Empty processing strategy.
    /// </summary>
    public static readonly OrderProcessingStrategy Empty = new EmptyStrategy();

    /// <summary>
    /// The default returning value for the GetAdditionalLogEntries() method.
    /// </summary>
    private readonly IList<LogEntry> additionalLogEntries = new List<LogEntry>();

    /// <summary>
    /// Defines custom results.
    /// </summary>
    public enum CustomResults
    {
      /// <summary>
      /// Out of stock result.
      /// </summary>
      OutOfStock = 0
    }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The result.</returns>
    [NotNull]
    public abstract string ProcessOrder(Order order, IDictionary<string, object> parameters);

    /// <summary>
    /// Gets log entry for sucess.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>Logging Entry.</returns>
    [NotNull]
    public abstract LogEntry GetLogEntryForSuccess(Order order);

    /// <summary>
    /// Performs additional fail processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The get addtional log entry for fail.</returns>
    [NotNull]
    public abstract LogEntry GetLogEntryForFail(Order order, params object[] parameters);

    /// <summary>
    /// Performs additional success processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>The get addtional log entry for sucess.</returns>
    [CanBeNull]
    public virtual IList<LogEntry> GetAdditionalLogEntriesForSuccess(Order order)
    {
      return this.additionalLogEntries;
    }

    /// <summary>
    /// Empty strategy.
    /// </summary>
    private class EmptyStrategy : OrderProcessingStrategy
    {
      /// <summary>
      /// Processes the order.
      /// </summary>
      /// <param name="order">The order.</param>
      /// <param name="parameters">The parameters.</param>
      /// <returns>The result.</returns>
      public override string ProcessOrder(Order order, IDictionary<string, object> parameters)
      {
        return SuccessfulResult;
      }

      /// <summary>
      /// Gets logging entry for sucess.
      /// </summary>
      /// <param name="order">The order.</param>
      /// <returns>Log entry.</returns>
      public override LogEntry GetLogEntryForSuccess(Order order)
      {
        return new LogEntry();
      }

      /// <summary>
      /// Performs additional fail processing.
      /// </summary>
      /// <param name="order">The order.</param>
      /// <param name="parameters">The parameters.</param>
      /// <returns>The get addtional log entry for fail.</returns>
      public override LogEntry GetLogEntryForFail(Order order, params object[] parameters)
      {
        return new LogEntry();
      }
    }
  }
}