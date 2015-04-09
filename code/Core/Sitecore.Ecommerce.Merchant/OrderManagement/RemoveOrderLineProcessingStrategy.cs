// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveOrderLineProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the RemoveOrderLineProcessingStrategy type.
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
  using DomainModel.Products;
  using Ecommerce.OrderManagement.Orders;
  using Logging;
  using Sitecore.Ecommerce.OrderManagement;

  /// <summary>
  /// Order processing strategy that removes order line.
  /// </summary>
  public class RemoveOrderLineProcessingStrategy : OrderLineProcessingStrategy
  {
    /// <summary>
    /// The order manager.
    /// </summary>
    [NotNull]
    private readonly MerchantOrderManager merchantOrderManager;

    /// <summary>
    /// Gets or sets the product stock manager.
    /// </summary>
    /// <value>The product stock manager.</value>
    [NotNull]
    private readonly IProductStockManager productStockManager;

    /// <summary>
    /// The calculation strategy.
    /// </summary>
    [NotNull]
    private readonly IOrderCalculationStrategy calculationStrategy;

    /// <summary>
    /// Gets or sets FormattedMessage.
    /// </summary>
    private string formattedMessage;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveOrderLineProcessingStrategy" /> class.
    /// </summary>
    /// <param name="merchantOrderManager">The merchant order manager.</param>
    /// <param name="productStockManager">The product stock manager.</param>
    /// <param name="calculationStrategy">The calculation strategy.</param>
    public RemoveOrderLineProcessingStrategy(MerchantOrderManager merchantOrderManager, IProductStockManager productStockManager, IOrderCalculationStrategy calculationStrategy)
    {
      Assert.ArgumentNotNull(merchantOrderManager, "merchantOrderManager");
      Assert.ArgumentNotNull(productStockManager, "productStockManager");
      Assert.ArgumentNotNull(calculationStrategy, "calculationStrategy");

      this.merchantOrderManager = merchantOrderManager;
      this.productStockManager = productStockManager;
      this.calculationStrategy = calculationStrategy;
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

      Assert.IsNotNull(parameters.FirstOrDefault(p => p.Key == "orderline").Value, "Order line should be passed as parameter.");

      OrderLine orderLine = parameters["orderline"] as OrderLine;
      Assert.IsNotNull(orderLine, "Cannot resolve order line");

      Assert.IsTrue(order.OrderLines.Count > 1, "Cannot remove the last order line.");

      this.SetOrderStates(order);

      this.CreateFormattedMessage(orderLine);

      // Resolving of the Stock.
      ProductStockInfo currentProductStockInfo = new ProductStockInfo
      {
        ProductCode = orderLine.LineItem.Item.Code
      };

      ProductStock productStock = this.productStockManager.GetStock(currentProductStockInfo);
      Assert.IsNotNull(productStock, "This is unknown product");

      long currentProductStock = productStock.Stock;

      // Updating of the stock.
      this.productStockManager.Update(currentProductStockInfo, currentProductStock + (long)orderLine.LineItem.Quantity);

      // Recalculate taxable amount.
      order = this.UpdateTaxesForOrder(order, orderLine);

      // Remove order line. 
      order.OrderLines.Remove(orderLine);

      // Save order
      this.merchantOrderManager.Save(order);

      return SuccessfulResult;
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
        Details = new LogEntryDetails(Constants.OrderLineDeleted, this.formattedMessage),
        Action = Constants.DeleteOrderLineAction,
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
    public override LogEntry GetLogEntryForFail([NotNull] Order order, params object[] parameters)
    {
      Assert.ArgumentNotNull(order, "order");

      LogEntry logEntryForFail = this.GetLogEntryForSuccess(order);
      logEntryForFail.Result = Constants.DeniedResult;
      logEntryForFail.Details = new LogEntryDetails(Constants.OrderLineDeletingFailed);

      return logEntryForFail;
    }

    /// <summary>
    /// Updates the taxes.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="orderLine">The order line.</param>
    /// <returns>
    /// The taxes for order.
    /// </returns>
    protected override Order UpdateTaxesForOrder(Order order, OrderLine orderLine)
    {
      int i = order.OrderLines.ToList().IndexOf(orderLine);
      TaxSubTotal taxSubtotal = order.TaxTotal.TaxSubtotal.Where(ts => ts.CalculationSequenceNumeric == i).First();
      order.TaxTotal.TaxSubtotal.Remove(taxSubtotal);

      foreach (TaxSubTotal taxSubTotal in order.TaxTotal.TaxSubtotal)
      {
        if (taxSubTotal.CalculationSequenceNumeric > i)
        {
          taxSubTotal.CalculationSequenceNumeric--;
        }
      }

      this.calculationStrategy.ApplyCalculations(order);

      return order;
    }

    /// <summary>
    /// Creates formatted message.
    /// </summary>
    /// <param name="orderLine">The order line.</param>
    private void CreateFormattedMessage(OrderLine orderLine)
    {
      this.formattedMessage = string.Format(
        "[Description: {0}; Price: {1}; Quantity: {2}]",
        orderLine.LineItem.Item.Description,
        orderLine.LineItem.Price.PriceAmount.Value,
        orderLine.LineItem.Quantity);
    }
  }
}