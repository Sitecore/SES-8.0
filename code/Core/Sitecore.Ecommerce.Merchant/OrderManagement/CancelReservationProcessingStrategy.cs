// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CancelReservationProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Cancels the reservation of the money.
//   After that the one will not be able to capture the amount.
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
  using DomainModel.Payments;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Extensions;
  using Logging;

  /// <summary>
  /// Cancels the reservation of the money. 
  /// After that the one will not be able to capture the amount.
  /// </summary>
  public class CancelReservationProcessingStrategy : OrderProcessingStrategy
  {
    /// <summary>
    /// The order manager.
    /// </summary>
    [NotNull]
    private readonly MerchantOrderManager merchantOrderManager;

    /// <summary>
    /// The payment provider factory.
    /// </summary>
    [NotNull]
    private readonly PaymentProviderFactory paymentProviderFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelReservationProcessingStrategy" /> class.
    /// </summary>
    /// <param name="merchantOrderManager">The merchant order manager.</param>
    /// <param name="paymentProviderFactory">The payment provider factory.</param>
    public CancelReservationProcessingStrategy(MerchantOrderManager merchantOrderManager, PaymentProviderFactory paymentProviderFactory)
    {
      Assert.ArgumentNotNull(merchantOrderManager, "merchantOrderManager");
      Assert.ArgumentNotNull(paymentProviderFactory, "paymentProviderFactory");

      this.merchantOrderManager = merchantOrderManager;
      this.paymentProviderFactory = paymentProviderFactory;
    }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// The result.
    /// </returns>
    public override string ProcessOrder([NotNull] Order order, [NotNull] IDictionary<string, object> parameters)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(parameters, "parameters");

      Assert.IsNotNull(order.PaymentMeans, "Order payment means cannot be null.");

      string paymentSystemCode = order.PaymentMeans.PaymentChannelCode;
      Assert.IsNotNullOrEmpty(paymentSystemCode, "Order payment channel code cannot be null or empty.");
      Assert.IsNotNull(order.ReservationTicket, "Order reservation ticket cannot be null");

      PaymentProvider paymentProvider = this.paymentProviderFactory.GetProvider(paymentSystemCode);

      Assert.IsTrue(paymentProvider is IReservable, "Unable to cancel amount reservation. Reservable payment provider is expected.");

      IReservable reservablePaymentProvider = (IReservable)paymentProvider;

      this.merchantOrderManager.Save(order);
      reservablePaymentProvider.CancelReservation(paymentProvider.PaymentOption, new PaymentArgs(), order.GetReservationTicket());

      ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
      string result = transactionDataProvider.GetPersistentValue(order.OrderId).ToString();
      if (result == "Canceled")
      {
        result = SuccessfulResult;
      }

      return result;
    }

    /// <summary>
    /// Gets logging entry for sucess.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// Logging Entry.
    /// </returns>
    public override LogEntry GetLogEntryForSuccess([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      return new LogEntry
      {
        Details = new LogEntryDetails(Constants.OrderReservationCanceled),
        Action = Constants.CancelReservationOrderAction,
        EntityID = order.OrderId,
        EntityType = Constants.OrderEntityType,
        LevelCode = Constants.PaymentSystemLevel,
        Result = Constants.ApprovedResult
      };
    }

    /// <summary>
    /// Performs additional fail processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The get addtional logging entry for fail.</returns>
    public override LogEntry GetLogEntryForFail([NotNull] Order order, [NotNull] params object[] parameters)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(parameters, "parameters");

      LogEntry logEntryForFail = this.GetLogEntryForSuccess(order);
      logEntryForFail.Result = Constants.DeniedResult;
      logEntryForFail.Details = new LogEntryDetails(Constants.OrderReservationCancelingFailed, parameters);

      return logEntryForFail;
    }
  }
}