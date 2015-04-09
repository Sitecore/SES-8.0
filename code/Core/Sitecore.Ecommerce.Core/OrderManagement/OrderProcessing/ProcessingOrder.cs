// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessingOrder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderProcessing type.
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

namespace Sitecore.Ecommerce.OrderManagement.OrderProcessing
{
  using System;
  using System.Collections.Generic;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the OrderProcessing type.
  /// </summary>
  public class ProcessingOrder : Order, IDynamicallyCalculated<Order>
  {
    /// <summary>
    /// The order.
    /// </summary>
    private readonly Order order;

    /// <summary>
    /// The processing line items.
    /// </summary>
    private readonly List<LineItemProcessing> processingLineItems = new List<LineItemProcessing>();

    /// <summary>
    /// The processing monetary total.
    /// </summary>
    private readonly MonetaryTotalProcessing processingMonetaryTotal;

    /// <summary>
    /// The processing tax total.
    /// </summary>
    private readonly TaxTotalProcessing processingTaxTotal;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessingOrder" /> class.
    /// </summary>
    /// <param name="order">The order.</param>
    public ProcessingOrder([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      this.order = order;

      this.PrepareOrderLinesForProcessing(this.order.OrderLines);

      this.processingMonetaryTotal = new MonetaryTotalProcessing(this.order);

      this.processingTaxTotal = new TaxTotalProcessing(this.order);
    }

    /// <summary>
    /// Gets or sets the order lines.
    /// </summary>
    /// <value>The order lines.</value>
    public override ICollection<OrderLine> OrderLines
    {
      get { return this.order.OrderLines; }
      set { this.order.OrderLines = value; }
    }

    /// <summary>
    /// Expected Total of the order.
    /// </summary>
    public override MonetaryTotal AnticipatedMonetaryTotal
    {
      get { return this.order.AnticipatedMonetaryTotal; }
      set { this.order.AnticipatedMonetaryTotal = value; }
    }

    /// <summary>
    /// Information about who’s paying if different from Buyer.
    /// </summary>
    public override CustomerParty AccountingCustomerParty
    {
      get { return this.order.AccountingCustomerParty; }
      set { this.order.AccountingCustomerParty = value; }
    }

    /// <summary>
    /// Information about the Buyer.
    /// </summary>
    public override CustomerParty BuyerCustomerParty
    {
      get { return this.order.BuyerCustomerParty; }
      set { this.order.BuyerCustomerParty = value; }
    }

    /// <summary>
    /// Gets the first delivery.
    /// </summary>
    public override Delivery DefaultDelivery
    {
      get { return this.order.DefaultDelivery; }
    }

    /// <summary>
    /// Gets the first freight forwarder party.
    /// </summary>
    public override Common.Party DefaultFreightForwarderParty
    {
      get { return this.order.DefaultFreightForwarderParty; }
    }

    /// <summary>
    /// Delivery information.
    /// </summary>
    public override ICollection<Delivery> Delivery
    {
      get { return this.order.Delivery; }
      set { this.order.Delivery = value; }
    }

    /// <summary>
    /// Destination Country.
    /// </summary>
    public override string DestinationCountryCode
    {
      get { return this.order.DestinationCountryCode; }
      set { this.order.DestinationCountryCode = value; }
    }

    /// <summary>
    /// Information about the transporter.
    /// </summary>
    public override ICollection<Common.Party> FreightForwarderParty
    {
      get { return this.order.FreightForwarderParty; }
      set { this.order.FreightForwarderParty = value; }
    }

    /// <summary>
    /// Gets or sets the ID.
    /// </summary>
    /// <value>
    /// The ID.
    /// </value>
    public override Guid ID
    {
      get { return this.order.ID; }
      set { this.order.ID = value; }
    }

    /// <summary>
    /// Gets or sets the issue date.
    /// </summary>
    /// <value>
    /// The issue date.
    /// </value>
    public override DateTime IssueDate
    {
      get { return this.order.IssueDate; }
      set { this.order.IssueDate = value; }
    }

    /// <summary>
    /// Gets the issue time.
    /// </summary>
    public override TimeSpan IssueTime
    {
      get { return this.order.IssueTime; }
    }

    /// <summary>
    /// Note on the order.
    /// </summary>
    public override string Note
    {
      get { return this.order.Note; }
      set { this.order.Note = value; }
    }

    /// <summary>
    /// The OrderId From the Merchant.
    /// </summary>
    public override string OrderId
    {
      get { return this.order.OrderId; }
      set { this.order.OrderId = value; }
    }

    /// <summary>
    /// Payment type information.
    /// </summary>
    public override PaymentMeans PaymentMeans
    {
      get { return this.order.PaymentMeans; }
      set { this.order.PaymentMeans = value; }
    }

    /// <summary>
    /// Currencycode for the Price.
    /// </summary>
    public override string PricingCurrencyCode
    {
      get { return this.order.PricingCurrencyCode; }
      set { this.order.PricingCurrencyCode = value; }
    }

    /// <summary>
    /// Gets or sets ReservationTicket.
    /// </summary>
    public override ReservationTicket ReservationTicket
    {
      get { return this.order.ReservationTicket; }
      set { this.order.ReservationTicket = value; }
    }

    /// <summary>
    /// Information about the seller.
    /// </summary>
    public override SupplierParty SellerSupplierParty
    {
      get { return this.order.SellerSupplierParty; }
      set { this.order.SellerSupplierParty = value; }
    }

    /// <summary>
    /// Gets or sets the shop context.
    /// </summary>
    /// <value>
    /// The shop context.
    /// </value>
    public override string ShopContext
    {
      get { return this.order.ShopContext; }
      set { this.order.ShopContext = value; }
    }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    /// <value>The state.</value>
    public override State State
    {
      get { return this.order.State; }
      set { this.order.State = value; }
    }

    /// <summary>
    /// CurrencyCode for the Tax.
    /// </summary>
    public override string TaxCurrencyCode
    {
      get { return this.order.TaxCurrencyCode; }
      set { this.order.TaxCurrencyCode = value; }
    }

    /// <summary>
    /// Discount information.
    /// </summary>
    public override ICollection<AllowanceCharge> AllowanceCharge
    {
      get { return this.order.AllowanceCharge; }
      set { this.order.AllowanceCharge = value; }
    }

    /// <summary>
    /// Gets or sets the tax total.
    /// </summary>
    /// <value>The tax total.</value>
    public override TaxTotal TaxTotal
    {
      get { return this.order.TaxTotal; }
      set { this.order.TaxTotal = value; }
    }

    /// <summary>
    /// Applies the calculations.
    /// </summary>
    /// <returns>Calculated order.</returns>
    [NotNull]
    public Order ApplyCalculations()
    {
      this.UpdateOrderLine();

      this.processingTaxTotal.ApplyCalculations();

      this.processingMonetaryTotal.ApplyCalculations();

      return this.order;
    }

    /// <summary>
    /// Prepares the order lines for processing.
    /// </summary>
    /// <param name="orderLines">The order lines.</param>
    private void PrepareOrderLinesForProcessing([NotNull] IEnumerable<OrderLine> orderLines)
    {
      Assert.ArgumentNotNull(orderLines, "orderLines");

      foreach (OrderLine line in orderLines)
      {
        this.processingLineItems.Add(new LineItemProcessing(line.LineItem, this.order.PricingCurrencyCode));
      }
    }

    /// <summary>
    /// Updates the order line.
    /// </summary>
    private void UpdateOrderLine()
    {
      this.processingLineItems.ForEach(pli => pli.ApplyCalculations());
    }
  }
}