// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaptureOrderProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the capture order processing strategy class.
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
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Currencies;
  using DomainModel.Payments;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Extensions;
  using Logging;
  using Sitecore.Pipelines;

  /// <summary>
  /// Defines the capture order processing strategy class.
  /// </summary>
  public class CaptureOrderProcessingStrategy : OrderProcessingStrategy
  {
    /// <summary>
    /// InProcess order state code.
    /// </summary>
    private const string InProcessStateCode = OrderStateCode.InProcess;

    /// <summary>
    /// Captured In Full order sub-state code.
    /// </summary>
    private const string CapturedInFullSubstateCode = OrderStateCode.InProcessCapturedInFull;

    /// <summary>
    /// Captured status code of order line.
    /// </summary>
    private const string CapturedCode = "Captured";

    /// <summary>
    /// The payment provider factory.
    /// </summary>
    private readonly PaymentProviderFactory paymentProviderFactory;

    /// <summary>
    /// The pipeline wrapper.
    /// </summary>
    private readonly PipelineWrapper pipelineWrapper;

    /// <summary>
    /// The transaction data provider.
    /// </summary>
    private readonly ITransactionData transactionDataProvider;

    /// <summary>
    /// Payment provider instance.
    /// </summary>
    private PaymentProvider paymentProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CaptureOrderProcessingStrategy"/> class.
    /// </summary>
    /// <param name="paymentProviderFactory">The payment provider factory.</param>
    /// <param name="transactionDataProvider">The transaction Data Provider.</param>
    /// <param name="pipelineWrapper">The pipeline wrapper.</param>
    public CaptureOrderProcessingStrategy(PaymentProviderFactory paymentProviderFactory, ITransactionData transactionDataProvider, PipelineWrapper pipelineWrapper)
    {
      Assert.ArgumentNotNull(paymentProviderFactory, "paymentProviderFactory");
      Assert.ArgumentNotNull(transactionDataProvider, "transactionDataProvider");
      Assert.ArgumentNotNull(pipelineWrapper, "pipelineWrapper");

      this.paymentProviderFactory = paymentProviderFactory;
      this.transactionDataProvider = transactionDataProvider;
      this.pipelineWrapper = pipelineWrapper;
    }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The process order.</returns>
    public override string ProcessOrder([NotNull] Order order, [NotNull] IDictionary<string, object> parameters)
    {
      Assert.ArgumentNotNull(order, "order");

      PaymentProvider paymentProviderLocal = this.GetPaymentProvider(order);

      if (paymentProviderLocal is IReservable)
      {
        Assert.IsNotNull(order.AnticipatedMonetaryTotal, "Order anticipated monetary total cannot be null.");
        decimal amountToCapture = order.AnticipatedMonetaryTotal.PayableAmount.Value;

        Assert.IsNotNull(order.ReservationTicket, "Order reservation ticket cannot be null");
        decimal reservedAmount = order.ReservationTicket.Amount;

        Assert.IsTrue(amountToCapture > 0 && amountToCapture <= reservedAmount, "Unable to capture the order. Total payable amount exceeds the reserved amount.");

        IReservable reservablePaymentProvider = (IReservable)paymentProviderLocal;

        ShoppingCart shoppingCart = new ShoppingCart { Currency = new Currency { Code = order.PricingCurrencyCode }, OrderNumber = order.OrderId };

        reservablePaymentProvider.Capture(paymentProviderLocal.PaymentOption, new PaymentArgs { ShoppingCart = shoppingCart }, order.GetReservationTicket(), amountToCapture);

        string result = this.transactionDataProvider.GetPersistentValue(order.OrderId).ToString();
        if (result == CapturedCode)
        {
          result = SuccessfulResult;
          order.ReservationTicket.CapturedAmount += amountToCapture;
          this.PerformActionsForSuccess(order);
        }
        else
        {
          this.RevertOrderStates(order);
        }

        return result;
      }

      this.PerformActionsForSuccess(order);
      return SuccessfulResult;
    }

    /// <summary>
    /// Gets logging entry for sucess.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>Logging Entry.</returns>
    public override LogEntry GetLogEntryForSuccess([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      LogEntry result = new LogEntry
      {
        Action = Constants.CaptureOrderAction,
        EntityID = order.OrderId,
        EntityType = Constants.OrderEntityType,
        Result = Constants.ApprovedResult
      };

      if (this.GetPaymentProvider(order) is IReservable)
      {
        string cardType = order.PaymentMeans.PaymentMeansCode;
        result.Details = string.IsNullOrEmpty(cardType) ? new LogEntryDetails(Constants.OrderCaptured) : new LogEntryDetails(Constants.OrderCapturedWithCard, cardType);
        result.LevelCode = Constants.PaymentSystemLevel;
      }
      else
      {
        result.Details = new LogEntryDetails(Constants.OrderIsCapturedInFull);
        result.LevelCode = Constants.UserLevel;
      }

      return result;
    }

    /// <summary>
    /// Performs additional success processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>The get additional logging entry for success.</returns>
    public override IList<LogEntry> GetAdditionalLogEntriesForSuccess([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.IsNotNull(order.State, "order.State cannot be null.");

      LogEntry logEntry = new LogEntry
      {
        Details = new LogEntryDetails(
          Constants.OrderStatusSet,
          order.State.Code,
          order.State.Substates.Aggregate(new System.Text.StringBuilder(), (sb, substate) => (substate.Active ^ (substate.Code == OrderStateCode.InProcessCapturedInFull)) ? (sb.Length == 0 ? sb : sb.Append("; ")).Append(substate.Code) : sb),
          order.State.Code,
          order.State.Substates.Aggregate(new System.Text.StringBuilder(), (sb, substate) => substate.Active ? (sb.Length == 0 ? sb : sb.Append("; ")).Append(substate.Code) : sb)),
        Action = Constants.CaptureOrderAction,
        EntityID = order.OrderId,
        EntityType = Constants.OrderEntityType,
        Result = Constants.ApprovedResult
      };

      if (this.GetPaymentProvider(order) is IReservable)
      {
        logEntry.LevelCode = Constants.PaymentSystemLevel;
      }
      else
      {
        logEntry.LevelCode = Constants.UserLevel;
      }

      return new List<LogEntry> { logEntry };
    }

    /// <summary>
    /// Performs additional fail processing.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// The get logging entry for fail.
    /// </returns>
    public override LogEntry GetLogEntryForFail([NotNull] Order order, params object[] parameters)
    {
      Assert.ArgumentNotNull(order, "order");

      LogEntry logEntryForFail = this.GetLogEntryForSuccess(order);
      logEntryForFail.Result = Constants.DeniedResult;
      logEntryForFail.Details = new LogEntryDetails(Constants.OrderCapturingFailed, parameters);
      return logEntryForFail;
    }

    /// <summary>
    /// Sets the order states.
    /// </summary>
    /// <param name="order">The order.</param>
    protected virtual void SetOrderStates([NotNull] Order order)
    {
      Assert.IsNotNull(order, "order");

      Assert.IsNotNull(order.State, "order.State cannot be null.");
      Assert.IsTrue(order.State.Code == InProcessStateCode, "Unable to capture order which is not in \"In Process\" state.");

      Substate capturedInFullSubstate = order.State.Substates.Single(substate => substate.Code == CapturedInFullSubstateCode);

      capturedInFullSubstate.Active = true;

      foreach (OrderLine orderLine in order.OrderLines)
      {
        orderLine.LineItem.LineStatusCode = CapturedCode;
      }
    }

    /// <summary>
    /// Reverts the order states.
    /// </summary>
    /// <param name="order">The order.</param>
    protected virtual void RevertOrderStates([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      Assert.IsNotNull(order.State, "order.State cannot be null.");
      Assert.IsTrue(order.State.Code == InProcessStateCode, "Unable to revert order which is not in \"In Process\" state.");

      Substate capturedInFullSubstate = order.State.Substates.Single(substate => substate.Code == CapturedInFullSubstateCode);

      capturedInFullSubstate.Active = false;
    }

    /// <summary>
    /// Starts the order create pipeline.
    /// </summary>
    /// <param name="order">The order.</param>
    protected virtual void StartOrderCapturedPipeline([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      PipelineArgs pipelineArgs = new PipelineArgs();
      pipelineArgs.CustomData.Add("order", order);

      this.pipelineWrapper.Start("orderCaptured", pipelineArgs);
    }

    /// <summary>
    /// Gets the payment provider.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// The payment provider.
    /// </returns>
    [NotNull]
    private PaymentProvider GetPaymentProvider([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      Assert.IsNotNull(order.PaymentMeans, "Order payment means cannot be null.");

      if (this.paymentProvider == null)
      {
        string paymentSystemCode = order.PaymentMeans.PaymentChannelCode;
        Assert.IsNotNullOrEmpty(paymentSystemCode, "Order payment channel code cannot be null or empty.");

        this.paymentProvider = this.paymentProviderFactory.GetProvider(paymentSystemCode);
      }

      return this.paymentProvider;
    }

    /// <summary>
    /// Performs the actions for success.
    /// </summary>
    /// <param name="order">The order.</param>
    private void PerformActionsForSuccess([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      this.SetOrderStates(order);
      this.StartOrderCapturedPipeline(order);
    }
  }
}