// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SendConfirmationProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SendConfirmationProcessingStrategy type.
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
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Logging;

  /// <summary>
  /// Sends order confirmation.
  /// </summary>
  public class SendConfirmationProcessingStrategy : OrderProcessingStrategy
  {
    /// <summary>
    /// Gets or sets OrderConfirmation.
    /// </summary>
    public OrderConfirmation OrderConfirmation { get; set; }


    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// The result.
    /// </returns>
    [NotNull]
    public override string ProcessOrder([NotNull] Order order, IDictionary<string, object> parameters)
    {
      Assert.ArgumentNotNull(order, "order");

      Assert.IsNotNull(this.OrderConfirmation, "OrderConfirmation cannot be null.");
      Assert.IsNotNull(this.OrderConfirmation.ConfirmationMessageBuilder, "OrderConfirmation.ConfirmationMessageBuilder cannot be null.");
      this.OrderConfirmation.ConfirmationMessageBuilder.Order = order;

      this.OrderConfirmation.Send();

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

      return new LogEntry
      {
        Details = new LogEntryDetails(Constants.OrderConfirmationSent), 
        Action = Constants.SendOrderConfirmationAction, 
        EntityID = order.OrderId, 
        EntityType = Constants.OrderEntityType,
        LevelCode = Constants.UserLevel,
        Result = Constants.ApprovedResult
      };
    }

    /// <summary>
    /// Performs additional fail processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// The get addtional logging entry for fail.
    /// </returns>
    [NotNull]
    public override LogEntry GetLogEntryForFail([NotNull] Order order, params object[] parameters)
    {
      Assert.ArgumentNotNull(order, "order");

      LogEntry logEntryForFail = this.GetLogEntryForSuccess(order);
      logEntryForFail.Result = Constants.DeniedResult;
      logEntryForFail.Details = new LogEntryDetails(Constants.OrderConfirmationFailed);

      return logEntryForFail;
    }
  }
}