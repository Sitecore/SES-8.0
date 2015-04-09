// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineItemProcessing.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the LineItemProcessing type.
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
  using System.Linq;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Common;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the LineItemProcessing type.
  /// </summary>
  public class LineItemProcessing : LineItem, IDynamicallyCalculated<LineItem>
  {
    /// <summary>
    /// The line item.
    /// </summary>
    private readonly LineItem lineItem;

    /// <summary>
    /// The currency ID
    /// </summary>
    private readonly string currencyID;

    /// <summary>
    /// The processing sub line items
    /// </summary>
    private readonly List<LineItem> processingSubLineItems = new List<LineItem>(); 

    /// <summary>
    /// Initializes a new instance of the <see cref="LineItemProcessing" /> class.
    /// </summary>
    /// <param name="lineItem">The line item.</param>
    /// <param name="currencyID">The currency ID.</param>
    public LineItemProcessing([NotNull] LineItem lineItem, [NotNull] string currencyID)
    {
      Assert.ArgumentNotNull(lineItem, "lineItem");
      Assert.ArgumentNotNullOrEmpty(currencyID, "currencyID");

      this.lineItem = lineItem;
      this.currencyID = currencyID;

      this.PrepareSubLinesForProcessing();
    }

    /// <summary>
    /// Information about delivery
    /// </summary>
    public override ICollection<Delivery> Delivery
    {
      get { return this.lineItem.Delivery; }
      set { this.lineItem.Delivery = value; }
    }

    /// <summary>
    /// Internal identifier
    /// </summary>
    public override string ID
    {
      get { return this.lineItem.ID; }
      set { this.lineItem.ID = value; }
    }

    /// <summary>
    /// Information about the item on the order line
    /// </summary>
    public override Item Item
    {
      get { return this.lineItem.Item; }
      set { this.lineItem.Item = value; }
    }

    /// <summary>
    /// Status for the order line
    /// </summary>
    public override string LineStatusCode
    {
      get { return this.lineItem.LineStatusCode; }
      set { this.lineItem.LineStatusCode = value; }
    }

    /// <summary>
    /// Amount for the line
    /// </summary>
    /// <exception cref="System.NotSupportedException">Calculated property. Setter is not supported.</exception>
    public override Amount LineExtensionAmount
    {
      get
      {
        Assert.IsTrue(this.SubLineItem.All(sl => sl.Price.PriceAmount.CurrencyID == this.currencyID), "Currency codes of price amounts of sublines must be equal to the order PriceCurrencyCode.");

        return new Amount((this.Price.PriceAmount.Value * this.Price.OrderableUnitFactorRate * this.Quantity) + this.SubLineItem.Aggregate(0m, (result, element) => result + element.LineExtensionAmount.Value), this.currencyID);
      }

      set
      {
        throw new NotSupportedException();
      }
    }

    /// <summary>
    /// Maximum quantity for ordering
    /// </summary>
    public override decimal MaximumQuantity
    {
      get { return this.lineItem.MaximumQuantity; }
      set { this.lineItem.MaximumQuantity = value; }
    }

    /// <summary>
    /// Minimum Quantity for ordering
    /// </summary>
    public override decimal MinimumQuantity
    {
      get { return this.lineItem.MinimumQuantity; }
      set { this.lineItem.MinimumQuantity = value; }
    }

    /// <summary>
    /// Free text field
    /// </summary>
    public override string Note
    {
      get { return this.lineItem.Note; }
      set { this.lineItem.Note = value; }
    }

    /// <summary>
    /// Information about order shipping
    /// </summary>
    public override OrderedShipment OrderedShipment
    {
      get { return this.lineItem.OrderedShipment; }
      set { this.lineItem.OrderedShipment = value; }
    }

    /// <summary>
    /// The merchant Order ID
    /// </summary>
    public override string OrderID
    {
      get { return this.lineItem.OrderID; }
      set { this.lineItem.OrderID = value; }
    }

    /// <summary>
    /// Gets or sets the order line.
    /// </summary>
    /// <value>
    /// The order line.
    /// </value>
    public override OrderLine OrderLine
    {
      get { return this.lineItem.OrderLine; }
      set { this.lineItem.OrderLine = value; }
    }

    /// <summary>
    /// Gets or sets the parent line.
    /// </summary>
    /// <value>
    /// The parent line.
    /// </value>
    public override LineItem ParentLine
    {
      get { return this.lineItem.ParentLine; }
      set { this.lineItem.ParentLine = value; }
    }

    /// <summary>
    /// Is Partial delivery allowed
    /// </summary>
    public override bool PartialDeliveryIndicator
    {
      get { return this.lineItem.PartialDeliveryIndicator; }
      set { this.lineItem.PartialDeliveryIndicator = value; }
    }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>
    /// The price.
    /// </value>
    public override Price Price
    {
      get { return this.lineItem.Price; }
      set { this.lineItem.Price = value; }
    }

    /// <summary>
    /// The Quantity
    /// </summary>
    public override decimal Quantity
    {
      get { return this.lineItem.Quantity; }
      set { this.lineItem.Quantity = value; }
    }

    /// <summary>
    /// Gets or sets the sub line item.
    /// </summary>
    /// <value>
    /// The sub line item.
    /// </value>
    public override ICollection<LineItem> SubLineItem
    {
      get { return this.lineItem.SubLineItem; }
      set { this.lineItem.SubLineItem = value; }
    }

    /// <summary>
    /// Gets or sets the sub states.
    /// </summary>
    /// <value>
    /// The sub states.
    /// </value>
    public override ICollection<Substate> Substates
    {
      get { return this.lineItem.Substates; }
      set { this.lineItem.Substates = value; }
    }

    /// <summary>
    /// Tax total on the order line
    /// </summary>
    public override Amount TotalTaxAmount
    {
      get { return this.lineItem.TotalTaxAmount; }
      set { this.lineItem.TotalTaxAmount = value; }
    }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public override long Alias
    {
      get { return this.lineItem.Alias; }
      set { this.lineItem.Alias = value; }
    }

    /// <summary>
    /// Applies the calculations.
    /// </summary>
    /// <returns>Calculated LineItem.</returns>
    [NotNull]
    public LineItem ApplyCalculations()
    {
      this.processingSubLineItems.Select(CalculateInnerSubLine).ToList();
      this.lineItem.LineExtensionAmount = this.LineExtensionAmount;

      return this.lineItem;
    }

    /// <summary>
    /// Calculates the inner sub line.
    /// </summary>
    /// <param name="lineItem">The line item.</param>
    /// <returns>Calculated LineItem.</returns>
    private static LineItem CalculateInnerSubLine(LineItem lineItem)
    {
      var lineItemProcessing = (LineItemProcessing)lineItem;

      return lineItemProcessing.ApplyCalculations();
    }

    /// <summary>
    /// Prepares the sub lines for processing.
    /// </summary>
    private void PrepareSubLinesForProcessing()
    {
      foreach (var item in this.lineItem.SubLineItem)
      {
        this.processingSubLineItems.Add(new LineItemProcessing(item, this.currencyID)); 
      }
    }
  }
}