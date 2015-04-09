// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddNewOrderLineProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the AddNewOrderLineProcessingStrategy type.
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
  using Common;
  using Diagnostics;
  using DomainModel.Products;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Logging;
  using Sitecore.Ecommerce.OrderManagement.OrderProcessing;

  /// <summary>
  /// Defines the class that adds new order line to existing order.
  /// </summary>
  public class AddNewOrderLineProcessingStrategy : OrderLineProcessingStrategy
  {
    /// <summary>
    /// The order manager.
    /// </summary>
    [NotNull]
    private readonly MerchantOrderManager merchantOrderManager;

    /// <summary>
    /// The order line factory.
    /// </summary>
    [NotNull]
    private readonly OrderLineFactory orderLineFactory;

    /// <summary>
    /// Gets or sets the product stock manager.
    /// </summary>
    /// <value>The product stock manager.</value>
    [NotNull]
    private readonly IProductStockManager productStockManager;

    /// <summary>
    /// Gets or sets FormattedMessage.
    /// </summary>
    private string formattedMessage;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddNewOrderLineProcessingStrategy" /> class.
    /// </summary>
    /// <param name="merchantOrderManager">The order manager.</param>
    /// <param name="orderLineFactory">The order line factory.</param>
    /// <param name="productStockManager">The product stock manager.</param>
    public AddNewOrderLineProcessingStrategy(MerchantOrderManager merchantOrderManager, OrderLineFactory orderLineFactory, IProductStockManager productStockManager)
    {
      Assert.ArgumentNotNull(merchantOrderManager, "merchantOrderManager");
      Assert.ArgumentNotNull(orderLineFactory, "orderLineFactory");
      Assert.ArgumentNotNull(productStockManager, "productStockManager");

      this.merchantOrderManager = merchantOrderManager;
      this.orderLineFactory = orderLineFactory;
      this.productStockManager = productStockManager;
    }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// The process order.
    /// </returns>
    public override string ProcessOrder([NotNull] Order order, [NotNull] IDictionary<string, object> parameters)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(parameters, "parameters");

      Assert.IsNotNull(parameters.FirstOrDefault(p => p.Key == "productcode").Value, "Product code should be passed as parameter.");
      Assert.IsNotNull(parameters.FirstOrDefault(p => p.Key == "quantity").Value, "Quantity should be passed as parameter.");

      string productCode = parameters["productcode"].ToString();
      Assert.IsTrue(productCode != string.Empty, "Product code must not be empty");

      long quantity = long.Parse(parameters["quantity"].ToString());

      Assert.IsNotNull(this.orderLineFactory, "orderLineFactory cannot be null.");
      OrderLine orderLine = this.orderLineFactory.CreateOrderLineFromOrder(order, productCode, quantity);
      Assert.IsNotNull(orderLine, "Cannot construct new order line from the available parameters.");

      this.CreateFormattedMessage(orderLine);

      this.SetOrderStates(order);

      ProductStockInfo productStockInfo = new ProductStockInfo
      {
        ProductCode = orderLine.LineItem.Item.Code
      };

      long stock = this.productStockManager.GetStock(productStockInfo).Stock;

      if (stock < orderLine.LineItem.Quantity)
      {
        return CustomResults.OutOfStock.ToString();
      }

      this.productStockManager.Update(productStockInfo, stock - (long)orderLine.LineItem.Quantity);
      order.OrderLines.Add(orderLine);

      // Recalculate taxable amount.
      order = this.UpdateTaxesForOrder(order, orderLine);

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
        Details = new LogEntryDetails(Constants.OrderLineAdded, this.formattedMessage),
        Action = Constants.AddOrderLineAction,
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
      logEntryForFail.Details = new LogEntryDetails(Constants.OrderLineAddingFailed, parameters);
      return logEntryForFail;
    }

    /// <summary>
    /// Gets Additional logging entry.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>List of additional logging entries.</returns>
    public override IList<LogEntry> GetAdditionalLogEntriesForSuccess(Order order)
    {
      return new List<LogEntry>();
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
      new LineItemProcessing(orderLine.LineItem, order.PricingCurrencyCode).ApplyCalculations();

      TaxSubTotal taxSubtotal = new TaxSubTotal
      {
        TaxableAmount = orderLine.LineItem.LineExtensionAmount,
        TaxCategory = new TaxCategory
        {
          BaseUnitMeasure = new Measure(),
          TaxScheme = new TaxScheme(),
          ID = "SimpleTaxCategory",
          PerUnitAmount = new Amount(0, order.PricingCurrencyCode),
          Percent = this.orderLineFactory.GetVat(order, orderLine.LineItem.Item.Code) * 100
        },
        CalculationSequenceNumeric = order.OrderLines.Count - 1,
        TransactionCurrencyTaxAmount = new Amount(0, order.PricingCurrencyCode)
      };

      order.TaxTotal.TaxSubtotal.Add(taxSubtotal);
      return order;
    }

    /// <summary>
    /// Creates formatted message.
    /// </summary>
    /// <param name="orderLine">The order line.</param>
    private void CreateFormattedMessage(OrderLine orderLine)
    {
      this.formattedMessage = string.Format(
        "[Product Code: {0}; Price: {1}; Quantity: {2}; Product Name: {3}]",
        orderLine.LineItem.Item.Code,
        orderLine.LineItem.Price.PriceAmount.Value,
        orderLine.LineItem.Quantity,
        orderLine.LineItem.Item.Name);
    }
  }
}