// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaxTotalProcessing.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the TaxTotalProcessing type.
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
  /// Defines the TaxTotalProcessing type.
  /// </summary>
  public class TaxTotalProcessing : TaxTotal, IDynamicallyCalculated<TaxTotal>
  {
    /// <summary>
    /// The tax total.
    /// </summary>
    private readonly TaxTotal taxTotal;

    /// <summary>
    /// The currency ID
    /// </summary>
    private readonly string currencyID;

    /// <summary>
    /// The processing tax sub total.
    /// </summary>
    private readonly List<TaxSubTotalProcessing> processingTaxSubTotal = new List<TaxSubTotalProcessing>(); 

    /// <summary>
    /// Initializes a new instance of the <see cref="TaxTotalProcessing" /> class.
    /// </summary>
    /// <param name="order">The order.</param>
    public TaxTotalProcessing([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      if (order.TaxTotal == null)
      {
        order.TaxTotal = new TaxTotal();
      }

      Assert.IsNotNullOrEmpty(order.PricingCurrencyCode, "Order should contain PricingCurrencyCode.");
      this.currencyID = order.PricingCurrencyCode;

      this.taxTotal = order.TaxTotal;

      this.processingTaxSubTotal.AddRange(order.TaxTotal.TaxSubtotal.Select(tst => new TaxSubTotalProcessing(tst)));
    }

    /// <summary>
    /// Gets or sets the rounding amount.
    /// </summary>
    public override Amount RoundingAmount
    {
      get { return this.taxTotal.RoundingAmount; }
      set { this.taxTotal.RoundingAmount = value; }
    }

    /// <summary>
    /// Gets or sets the information about tax subtotal for the order.
    /// </summary>
    public override ICollection<TaxSubTotal> TaxSubtotal
    {
      get { return this.taxTotal.TaxSubtotal; }
      set { this.taxTotal.TaxSubtotal = value; }
    }

    /// <summary>
    /// Gets the tax amount.
    /// </summary>
    /// <exception cref="NotSupportedException">Cannot set the auto calculated property.</exception>
    public override Amount TaxAmount
    {
      get
      {
        Assert.IsNotNull(this.TaxSubtotal, "TaxSubtotal collection must be set.");

        if (this.RoundingAmount == null)
        {
          this.RoundingAmount = new Amount(this.currencyID);
        }

        Assert.IsTrue(this.TaxSubtotal.All(ts => ts.TaxAmount.CurrencyID == this.currencyID), "TaxSubtotals must have the same currency.");

        Amount taxAmount = this.TaxSubtotal.Count > 0 ? new Amount(this.TaxSubtotal.Sum(ts => ts.TaxAmount.Value), this.TaxSubtotal.First().TaxableAmount.CurrencyID) : new Amount(this.currencyID);

        if (taxAmount.Value > 0 && this.RoundingAmount.Value > 0)
        {
          Assert.IsTrue(taxAmount.CurrencyID == this.RoundingAmount.CurrencyID, "RoundingAmount must has the same currency.");

          taxAmount.Value = Math.Ceiling(taxAmount.Value / this.RoundingAmount.Value) * this.RoundingAmount.Value;
        }

        return taxAmount;
      }

      set
      {
        throw new NotSupportedException("Cannot set the auto calculated property.");
      }
    }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public override long Alias
    {
      get { return this.taxTotal.Alias; }
      set { this.taxTotal.Alias = value; }
    }

    /// <summary>
    /// Applies the calculations.
    /// </summary>
    /// <returns>Calculated TaxTotal.</returns>
    [NotNull]
    public TaxTotal ApplyCalculations()
    {
      this.processingTaxSubTotal.ForEach(pts => pts.ApplyCalculations());

      this.taxTotal.TaxAmount = this.TaxAmount;

      return this.taxTotal;
    }
  }
}