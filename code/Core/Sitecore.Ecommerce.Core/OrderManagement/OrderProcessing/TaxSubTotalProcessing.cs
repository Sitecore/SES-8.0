// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaxSubTotalProcessing.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the TaxSubTotalProcessing type.
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
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Common;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the TaxSubTotalProcessing type.
  /// </summary>
  public class TaxSubTotalProcessing : TaxSubTotal, IDynamicallyCalculated<TaxSubTotal>
  {
    /// <summary>
    /// The tax sub total.
    /// </summary>
    private readonly TaxSubTotal taxSubTotal;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaxSubTotalProcessing" /> class.
    /// </summary>
    /// <param name="taxSubTotal">The tax sub total.</param>
    public TaxSubTotalProcessing([NotNull] TaxSubTotal taxSubTotal)
    {
      Assert.ArgumentNotNull(taxSubTotal, "taxSubTotal");

      this.taxSubTotal = taxSubTotal;
    }

    /// <summary>
    /// Gets or sets the amount that is being used for multiplying when calculating the tax amount.
    /// </summary>
    /// <value>The taxable amount.</value>
    public override Amount TaxableAmount
    {
      get { return this.taxSubTotal.TaxableAmount; }
      set { this.taxSubTotal.TaxableAmount = value; }
    }

    /// <summary>
    /// The amount of tax.
    /// </summary>
    /// <exception cref="NotSupportedException">Cannot set the auto calculated property.</exception>
    public override Amount TaxAmount
    {
      get
      {
        Assert.IsNotNull(this.TaxCategory, "TaxCategory must be set.");
        Assert.IsNotNull(this.TaxableAmount, "TaxableAmount must be set.");

        decimal value = this.TaxableAmount.Value * this.TaxCategory.Percent / 100;

        return new Amount(value, this.TaxableAmount.CurrencyID);
      }

      set
      {
        throw new NotSupportedException("Cannot set the auto calculated property.");
      }
    }

    /// <summary>
    /// The order which the tax rate should be calculated if more than one tax rate defined for the order.
    /// </summary>
    public override decimal CalculationSequenceNumeric
    {
      get { return this.taxSubTotal.CalculationSequenceNumeric; }
      set { this.taxSubTotal.CalculationSequenceNumeric = value; }
    }

    /// <summary>
    /// Gets or sets the transaction currency tax amount.
    /// </summary>
    /// <value>The transaction currency tax amount.</value>
    public override Amount TransactionCurrencyTaxAmount
    {
      get { return this.taxSubTotal.TransactionCurrencyTaxAmount; }
      set { this.taxSubTotal.TransactionCurrencyTaxAmount = value; }
    }

    /// <summary>
    /// Gets or sets the tax category.
    /// </summary>
    /// <value>The tax category.</value>
    public override TaxCategory TaxCategory
    {
      get { return this.taxSubTotal.TaxCategory; }
      set { this.taxSubTotal.TaxCategory = value; }
    }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public override long Alias
    {
      get { return this.taxSubTotal.Alias; }
      set { this.taxSubTotal.Alias = value; }
    }

    /// <summary>
    /// Applies the calculations.
    /// </summary>
    /// <returns>Calculated TaxSubTotal.</returns>
    [NotNull]
    public TaxSubTotal ApplyCalculations()
    {
      this.taxSubTotal.TaxAmount = this.TaxAmount;

      return this.taxSubTotal;
    }
  }
}