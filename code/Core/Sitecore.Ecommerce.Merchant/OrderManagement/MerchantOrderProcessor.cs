// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MerchantOrderProcessor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderProcessor type.
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
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Logging;

  /// <summary>
  /// Order processing helper.
  /// </summary>
  public class MerchantOrderProcessor
  {
    /// <summary>
    /// Gets or sets the order processing strategy.
    /// </summary>
    /// <value>
    /// The order processing strategy.
    /// </value>
    public virtual OrderProcessingStrategy OrderProcessingStrategy { get; set; }

    /// <summary>
    /// Gets or sets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    public virtual Logger Logger { get; set; }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The order.</returns>
    [NotNull]
    public virtual string ProcessOrder([NotNull] Order order, [NotNull] IDictionary<string, object> parameters)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(parameters, "parameters");

      Assert.IsNotNull(this.OrderProcessingStrategy, "OrderProcessingStrategy cannot be null.");
      Assert.IsNotNull(this.Logger, "Logger cannot be null.");

      LogEntry logEntry = LogEntry.Empty;

      try
      {
        string result = this.OrderProcessingStrategy.ProcessOrder(order, parameters);

        if (result == OrderProcessingStrategy.SuccessfulResult)
        {
          IList<LogEntry> additionalLoggingEntriesForSuccess = this.OrderProcessingStrategy.GetAdditionalLogEntriesForSuccess(order);
          if (additionalLoggingEntriesForSuccess != null)
          {
            foreach (LogEntry additionalLogEntry in additionalLoggingEntriesForSuccess)
            {
              this.Logger.Log(additionalLogEntry);
            }
          }

          logEntry = this.OrderProcessingStrategy.GetLogEntryForSuccess(order);
        }
        else
        {
          logEntry = this.OrderProcessingStrategy.GetLogEntryForFail(order, result);
        }

        return result;
      }
      catch (Exception exception)
      {
        logEntry = this.OrderProcessingStrategy.GetLogEntryForFail(order, exception.Message);
        throw;
      }
      finally
      {
        this.Logger.Log(logEntry);
        this.Logger.Flush();
      }
    }
  }
}