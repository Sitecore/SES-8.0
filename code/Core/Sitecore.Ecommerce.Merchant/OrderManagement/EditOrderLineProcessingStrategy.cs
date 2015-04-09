// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditOrderLineProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the EditOrderLineProcessingStrategy type.
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
  using Ecommerce.OrderManagement.Extensions;
  using Ecommerce.OrderManagement.Orders;
  using Logging;
  using Sitecore.Ecommerce.OrderManagement.OrderProcessing;

  /// <summary>
  /// Edit order line processing strategy.
  /// </summary>
  public class EditOrderLineProcessingStrategy : OrderLineProcessingStrategy
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
    /// Initializes a new instance of the <see cref="EditOrderLineProcessingStrategy" /> class.
    /// </summary>
    /// <param name="merchantOrderManager">The merchant order manager.</param>
    /// <param name="orderLineFactory">The order line factory.</param>
    /// <param name="productStockManager">The product stock manager.</param>
    public EditOrderLineProcessingStrategy(MerchantOrderManager merchantOrderManager, OrderLineFactory orderLineFactory, IProductStockManager productStockManager)
    {
      Assert.ArgumentNotNull(merchantOrderManager, "merchantOrderManager");
      Assert.ArgumentNotNull(orderLineFactory, "orderLineFactory");
      Assert.ArgumentNotNull(productStockManager, "productStockManager");

      this.merchantOrderManager = merchantOrderManager;
      this.orderLineFactory = orderLineFactory;
      this.productStockManager = productStockManager;
    }

    /// <summary>
    /// Gets the order manager.
    /// </summary>
    /// <value>The order manager.</value>
    [NotNull]
    protected MerchantOrderManager OrderManager
    {
      get { return this.merchantOrderManager; }
    }

    /// <summary>
    /// Gets the order line factory.
    /// </summary>
    /// <value>The order line factory.</value>
    [NotNull]
    protected OrderLineFactory OrderLineFactory
    {
      get { return this.orderLineFactory; }
    }

    /// <summary>
    /// Gets the product stock manager.
    /// </summary>
    /// <value>The product stock manager.</value>
    [NotNull]
    protected IProductStockManager ProductStockManager
    {
      get { return this.productStockManager; }
    }

    /// <summary>
    /// Gets or sets the formatted message for old order line.
    /// </summary>
    /// <value>
    /// The formatted message for old order line.
    /// </value>
    protected string FormattedMessageForOldOrderLine { get; set; }

    /// <summary>
    /// Gets or sets the formatted message for new order line.
    /// </summary>
    /// <value>
    /// The formatted message for new order line.
    /// </value>
    protected string FormattedMessageForNewOrderLine { get; set; }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The result.</returns>
    [NotNull]
    public override string ProcessOrder([NotNull] Order order, IDictionary<string, object> parameters)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(parameters, "parameters");

      Assert.IsNotNull(parameters.FirstOrDefault(p => p.Key == "orderlineid").Value, "Order line ID should be passed as parameter.");
      Assert.IsNotNull(parameters.FirstOrDefault(p => p.Key == "productcode").Value, "Product code should be passed as parameter.");
      Assert.IsNotNull(parameters.FirstOrDefault(p => p.Key == "quantity").Value, "Quantity should be passed as parameter.");

      string productCode = parameters["productcode"].ToString();
      Assert.IsTrue(productCode != string.Empty, "Product code must not be empty");

      long quantity = long.Parse(parameters["quantity"].ToString());
      long orderLineId = long.Parse(parameters["orderlineid"].ToString());

      // Resolving of the OrderLine.
      OrderLine orderLine = order.OrderLines.Single(ol => ol.Alias == orderLineId);
      Assert.IsNotNull(orderLine, "Cannot resolve order line");

      this.SetOrderStates(order);

      // Resolving of the Stock.
      ProductStockInfo currentProductStockInfo = new ProductStockInfo
      {
        ProductCode = orderLine.LineItem.Item.Code
      };
      long currentProductStock = this.productStockManager.GetStock(currentProductStockInfo).Stock;

      ProductStockInfo newProductStockInfo = new ProductStockInfo
      {
        ProductCode = productCode
      };
      long newProductStock = this.productStockManager.GetStock(newProductStockInfo).Stock;

      if (newProductStock < quantity)
      {
        return CustomResults.OutOfStock.ToString();
      }

      // Updating of the stock
      this.productStockManager.Update(currentProductStockInfo, currentProductStock + (long)orderLine.LineItem.Quantity);
      this.productStockManager.Update(newProductStockInfo, newProductStock - quantity);

      this.FormattedMessageForOldOrderLine = this.CreateFormattedMessage(orderLine);

      // Updating of the LineItem
      Assert.IsNotNull(this.orderLineFactory, "OrderLineFactory cannot be null.");
      LineItem newLineItem = this.orderLineFactory.CreateLineItemFromOrder(order, productCode, quantity);
      orderLine.LineItem.CopyLineItemFrom(newLineItem);

      this.FormattedMessageForNewOrderLine = this.CreateFormattedMessage(orderLine);

      // Recalculate taxable amount.
      order = this.UpdateTaxesForOrder(order, orderLine);

      // Saving of the Order)
      this.merchantOrderManager.Save(order);

      return SuccessfulResult;
    }

    /// <summary>
    /// Gets logging entry for sucess.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>Logging Entry.</returns>
    [NotNull]
    public override LogEntry GetLogEntryForSuccess([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      return new LogEntry
      {
        Details = new LogEntryDetails(Constants.OrderLineEdited, this.FormattedMessageForOldOrderLine, this.FormattedMessageForNewOrderLine),
        Action = Constants.EditOrderLineAction,
        EntityID = order.OrderId,
        EntityType = Constants.OrderEntityType,
        LevelCode = Constants.UserLevel,
        Result = Constants.ApprovedResult
      };
    }

    /// <summary>
    /// Gets logging entry for fail.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>Logging entry for fail.</returns>
    [NotNull]
    public override LogEntry GetLogEntryForFail([NotNull] Order order, params object[] parameters)
    {
      Assert.ArgumentNotNull(order, "order");

      LogEntry logEntryForFail = this.GetLogEntryForSuccess(order);
      logEntryForFail.Result = Constants.DeniedResult;
      logEntryForFail.Details = new LogEntryDetails(Constants.OrderLineEditingFailed, parameters);

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
      decimal vat = this.orderLineFactory.GetVat(order, orderLine.LineItem.Item.Code);
      taxSubtotal.TaxCategory.Percent = vat * 100;
      taxSubtotal.TaxableAmount = new LineItemProcessing(orderLine.LineItem, order.PricingCurrencyCode).ApplyCalculations().LineExtensionAmount;
      orderLine.LineItem.TotalTaxAmount = new Amount(vat * orderLine.LineItem.Quantity * orderLine.LineItem.Price.PriceAmount.Value, order.PricingCurrencyCode);

      return order;
    }

    /// <summary>
    /// Creates formatted message.
    /// </summary>
    /// <param name="orderLine">The order line.</param>
    /// <returns>The create formatted message.</returns>
    protected virtual string CreateFormattedMessage(OrderLine orderLine)
    {
      return string.Format(
        "[Product Code: {0}; Price: {1}; Quantity: {2}; Product Name: {3}]",
        orderLine.LineItem.Item.Code,
        orderLine.LineItem.Price.PriceAmount.Value,
        orderLine.LineItem.Quantity,
        orderLine.LineItem.Item.Name);
    }
  }
}