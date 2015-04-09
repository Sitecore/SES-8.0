// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeOrderLineQuantityProcessingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ChangeOrderLineQuantityProcessingStrategy type.
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
  using Ecommerce.OrderManagement.Orders;
  using Logging;
  using Sitecore.Ecommerce.OrderManagement;

  /// <summary>
  /// Defines the change order line quantity processing strategy class.
  /// </summary>
  public class ChangeOrderLineQuantityProcessingStrategy : EditOrderLineProcessingStrategy
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeOrderLineQuantityProcessingStrategy" /> class.
    /// </summary>
    /// <param name="merchantOrderManager">The merchant order manager.</param>
    /// <param name="orderLineFactory">The order line factory.</param>
    /// <param name="productStockManager">The product stock manager.</param>
    public ChangeOrderLineQuantityProcessingStrategy(MerchantOrderManager merchantOrderManager, OrderLineFactory orderLineFactory, IProductStockManager productStockManager)
      : base(merchantOrderManager, orderLineFactory, productStockManager)
    {
    }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// The result.
    /// </returns>
    public override string ProcessOrder(Order order, [NotNull] IDictionary<string, object> parameters)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(parameters, "parameters");

      Assert.IsNotNull(parameters.FirstOrDefault(p => p.Key == "orderlineid").Value, "Order line ID should be passed as parameter.");
      Assert.IsNotNull(parameters.FirstOrDefault(p => p.Key == "quantity").Value, "Quantity should be passed as parameter.");

      long orderLineId = long.Parse(parameters["orderlineid"].ToString());
      long quantity = long.Parse(parameters["quantity"].ToString());

      // Resolving of the OrderLine.
      OrderLine orderLine = order.OrderLines.Single(ol => ol.Alias == orderLineId);
      Assert.IsNotNull(orderLine, "Cannot resolve order line");

      this.SetOrderStates(order);

      // Resolving of the Stock.
      ProductStockInfo productStockInfo = new ProductStockInfo
      {
        ProductCode = orderLine.LineItem.Item.Code
      };
      long productStock = this.ProductStockManager.GetStock(productStockInfo).Stock;
      long stockSubtrahend = (long)(quantity - orderLine.LineItem.Quantity);

      if (productStock < stockSubtrahend)
      {
        return CustomResults.OutOfStock.ToString();
      }

      // Updating of the stock
      this.ProductStockManager.Update(productStockInfo, productStock - stockSubtrahend);

      this.FormattedMessageForOldOrderLine = this.CreateFormattedMessage(orderLine);

      // Update order line
      orderLine.LineItem.Quantity = quantity;

      this.FormattedMessageForNewOrderLine = this.CreateFormattedMessage(orderLine);

      // Recalculate taxable amount
      order = this.UpdateTaxesForOrder(order, orderLine);

      // Saving of the Order
      this.OrderManager.Save(order);

      return SuccessfulResult;
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
    protected override Order UpdateTaxesForOrder([NotNull] Order order, [NotNull] OrderLine orderLine)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(orderLine, "orderLine");

      int i = order.OrderLines.ToList().IndexOf(orderLine);
      TaxSubTotal taxSubTotal = order.TaxTotal.TaxSubtotal.Where(ts => ts.CalculationSequenceNumeric == i).First();

      decimal vat = taxSubTotal.TaxCategory.Percent / 100;
      orderLine.LineItem.TotalTaxAmount = new Amount(vat * orderLine.LineItem.Quantity * orderLine.LineItem.Price.PriceAmount.Value, order.PricingCurrencyCode);

      decimal taxableValue = orderLine.LineItem.Quantity * orderLine.LineItem.Price.PriceAmount.Value;
      taxSubTotal.TaxableAmount = new Amount(taxableValue, orderLine.LineItem.Price.PriceAmount.CurrencyID);
      return order;
    }
  }
}