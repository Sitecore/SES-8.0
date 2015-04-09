// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackOrderProcessingStrategy.cs" company="Sitecore Corporation">
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
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Logging;

  /// <summary>
  /// Defines the pack order processing strategy class.
  /// </summary>
  public class PackOrderProcessingStrategy : OrderStateProcessingStrategy
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="PackOrderProcessingStrategy" /> class.
    /// </summary>
    /// <param name="orderStateConfiguration">The order state configuration.</param>
    public PackOrderProcessingStrategy(MerchantOrderStateConfiguration orderStateConfiguration)
      : base(orderStateConfiguration)
    {
    }

    /// <summary>
    /// Gets logging entry for sucess.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// Logging Entry.
    /// </returns>
    public override LogEntry GetLogEntryForSuccess(Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      return this.GetLogEntry(order, Constants.InProcessSubstateSuccessfullyChanged, Constants.ApprovedResult);
    }

    /// <summary>
    /// Performs additional fail processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// The get addtional logging entry for fail.
    /// </returns>
    public override LogEntry GetLogEntryForFail(Order order, params object[] parameters)
    {
      Assert.ArgumentNotNull(order, "order");

      return this.GetLogEntry(order, Constants.InProcessSubstateFailedToChange, Constants.DeniedResult);
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
        Details = new LogEntryDetails(template, "Not Packed", "Packed in full"),
        Action = string.Format(Constants.ProcessInFullAction, "Pack"),
        EntityID = order.OrderId,
        EntityType = Constants.OrderEntityType,
        LevelCode = Constants.UserLevel,
        Result = result
      };
    }
  }
}
