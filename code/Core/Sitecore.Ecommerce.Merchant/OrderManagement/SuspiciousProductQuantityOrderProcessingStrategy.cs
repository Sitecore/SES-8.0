// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuspiciousProductQuantityOrderProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The suspicious order processing strategy.
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
  using System.Linq;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Logging;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// The suspicious product quantity order processing strategy.
  /// </summary>
  public class SuspiciousProductQuantityOrderProcessingStrategy : OrderStateProcessingStrategy
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SuspiciousProductQuantityOrderProcessingStrategy" /> class.
    /// </summary>
    /// <param name="orderStateConfiguration">The order state configuration.</param>
    public SuspiciousProductQuantityOrderProcessingStrategy(MerchantOrderStateConfiguration orderStateConfiguration)
      : base(orderStateConfiguration)
    {
    }

    /// <summary>
    /// Gets logging entry for success.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// Logging Entry.
    /// </returns>
    public override LogEntry GetLogEntryForSuccess(Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      var max = order.OrderLines.Max(ol => ol.LineItem.Quantity);
      var orderLine = order.OrderLines.First(ol => ol.LineItem.Quantity == max);
      var name = orderLine.LineItem.Item.Name;
      var code = orderLine.LineItem.Item.Code;
      var quantity = orderLine.LineItem.Quantity;
      string filledTemplate = string.Format(Constants.MarkedAsSuspiciousSuccessfully, name, code, quantity);

      return this.GetLogEntry(order, filledTemplate, Constants.ApprovedResult);
    }

    /// <summary>
    /// Performs fail processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// Gets logging entry for fail.
    /// </returns>
    public override LogEntry GetLogEntryForFail(Order order, params object[] parameters)
    {
      return this.GetLogEntry(order, Constants.FailedToMarkAsSuspicious, Constants.DeniedResult);
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
    internal override LogEntry GetLogEntry(Order order, string template, string result)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(template, "template");
      Assert.ArgumentNotNull(result, "result");

      return new LogEntry
      {
        Details = new LogEntryDetails(template),
        Action = Constants.MarkAsSuspicious,
        EntityID = order.OrderId,
        EntityType = Constants.OrderEntityType,
        LevelCode = Constants.UserLevel,
        Result = result
      };
    }
  }
}