// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonetaryTotalProcessing.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the MonetaryTotalProcessing type.
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
  using System.Linq;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Common;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the MonetaryTotalProcessing type.
  /// </summary>
  public class MonetaryTotalProcessing : MonetaryTotal, IDynamicallyCalculated<MonetaryTotal>
  {
    /// <summary>
    /// The order.
    /// </summary>
    private readonly Order order;

    /// <summary>
    /// The inner monetary total.
    /// </summary>
    private readonly MonetaryTotal innerMonetaryTotal;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonetaryTotalProcessing" /> class.
    /// </summary>
    /// <param name="order">The order.</param>
    public MonetaryTotalProcessing([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      this.order = order;

      if (order.AnticipatedMonetaryTotal == null)
      {
        order.AnticipatedMonetaryTotal = new MonetaryTotal();
      }

      this.innerMonetaryTotal = order.AnticipatedMonetaryTotal;
    }

    /// <summary>
    /// Gets the line extension amount.
    /// </summary>
    /// <value>The line extension amount.</value>
    /// <exception cref="NotSupportedException">Cannot set the autocalculated property.</exception>
    public override Amount LineExtensionAmount
    {
      get
      {
        string currencyID = this.order.PricingCurrencyCode;

        Assert.IsTrue(this.order.OrderLines.All(orderLine => orderLine.LineItem.LineExtensionAmount.CurrencyID == currencyID), "Currency codes of order lines must be equal to the currency code of TaxExclusiveAmount.");

        return new Amount(this.order.OrderLines.Sum(orderLine => orderLine.LineItem.LineExtensionAmount.Value), currencyID);
      }

      set
      {
        throw new NotSupportedException("Cannot set the auto calculated property.");
      }
    }

    /// <summary>
    /// Gets the tax exclusive amount.
    /// </summary>
    /// <value>The tax exclusive amount.</value>
    /// <exception cref="NotSupportedException">Cannot set the auto calculated property.</exception>
    public override Amount TaxExclusiveAmount
    {
      get { return new Amount(this.order.TaxTotal.TaxAmount.Value, this.order.TaxTotal.TaxAmount.CurrencyID); }
      set { throw new NotSupportedException("Cannot set the auto calculated property."); }
    }

    /// <summary>
    /// Gets the tax inclusive amount.
    /// </summary>
    /// <value>The tax inclusive amount.</value>
    /// <exception cref="NotSupportedException">Cannot set the auto calculated property.</exception>
    public override Amount TaxInclusiveAmount
    {
      get { return this.order.AllowanceCharge.Aggregate(this.order.TaxTotal.TaxSubtotal.Aggregate(this.order.TaxTotal.TaxAmount, (result, taxSubtotal) => result + taxSubtotal.TaxableAmount), (result, allowanceCharge) => new Amount(result.Value + (allowanceCharge.Amount.Value * ((Convert.ToInt32(allowanceCharge.ChargeIndicator) << 1) - 1)), this.order.PricingCurrencyCode)); }
      set { throw new NotSupportedException("Cannot set the auto calculated property."); }
    }

    /// <summary>
    /// Gets the allowance total amount.
    /// </summary>
    /// <value>The allowance total amount.</value>
    /// <exception cref="NotSupportedException">Cannot set the auto calculated property.</exception>
    public override Amount AllowanceTotalAmount
    {
      get
      {
        string currencyID = this.order.PricingCurrencyCode;

        Assert.IsTrue(this.order.AllowanceCharge.Where(allowanceCharge => !allowanceCharge.ChargeIndicator).All(allowanceCharge => allowanceCharge.Amount.CurrencyID == currencyID), "Currency codes of allowance charges with charge indicator being not set must be equal to the currency code of TaxExclusiveAmount.");

        return new Amount(this.order.AllowanceCharge.Where(allowanceCharge => !allowanceCharge.ChargeIndicator).Sum(allowanceCharge => allowanceCharge.Amount.Value), currencyID);
      }

      set
      {
        throw new NotSupportedException("Cannot set the auto calculated property.");
      }
    }

    /// <summary>
    /// Gets the charge total amount.
    /// </summary>
    /// <value>The charge total amount.</value>
    /// <exception cref="NotSupportedException">Cannot set the auto calculated property.</exception>
    public override Amount ChargeTotalAmount
    {
      get
      {
        string currencyID = this.order.PricingCurrencyCode;

        Assert.IsTrue(this.order.AllowanceCharge.Where(allowanceCharge => allowanceCharge.ChargeIndicator).All(allowanceCharge => allowanceCharge.Amount.CurrencyID == currencyID), "Currency codes of allowance charges with charge indicator being set must be equal to the currency code of TaxExclusiveAmount.");

        return new Amount(this.order.AllowanceCharge.Where(allowanceCharge => allowanceCharge.ChargeIndicator).Sum(allowanceCharge => allowanceCharge.Amount.Value), currencyID);
      }

      set
      {
        throw new NotSupportedException("Cannot set the auto calculated property.");
      }
    }

    /// <summary>
    /// Gets the payable rounding amount.
    /// </summary>
    /// <value>The payable rounding amount.</value>
    /// <exception cref="NotSupportedException">Cannot set the auto calculated property.</exception>
    public override Amount PayableRoundingAmount
    {
      get { return this.order.TaxTotal.RoundingAmount; }
      set { throw new NotSupportedException("Cannot set the auto calculated property."); }
    }

    /// <summary>
    /// Gets the payable amount.
    /// </summary>
    /// <exception cref="NotSupportedException">Cannot set the auto calculated property.</exception>
    public override Amount PayableAmount
    {
      get { return new Amount(this.LineExtensionAmount.Value + this.TaxExclusiveAmount.Value - this.AllowanceTotalAmount.Value + this.ChargeTotalAmount.Value - this.PrepaidAmount.Value + this.PayableRoundingAmount.Value, this.order.PricingCurrencyCode); }
      set { throw new NotSupportedException("Cannot set the auto calculated property."); }
    }

    /// <summary>
    /// Gets or sets or sets the total prepaid amount.
    /// </summary>
    /// <value>The total prepaid amount.</value>
    public override Amount PrepaidAmount
    {
      get { return this.innerMonetaryTotal.PrepaidAmount == null ? this.innerMonetaryTotal.PrepaidAmount = new Amount(this.order.PricingCurrencyCode) : this.innerMonetaryTotal.PrepaidAmount; }
      set { this.innerMonetaryTotal.PrepaidAmount = value; }
    }

    /// <summary>
    /// Applies the calculations.
    /// </summary>
    /// <returns>Calculated MonetaryTotal.</returns>
    public MonetaryTotal ApplyCalculations()
    {
      this.innerMonetaryTotal.AllowanceTotalAmount = this.AllowanceTotalAmount;
      this.innerMonetaryTotal.ChargeTotalAmount = this.ChargeTotalAmount;
      this.innerMonetaryTotal.LineExtensionAmount = this.LineExtensionAmount;
      this.innerMonetaryTotal.PayableAmount = this.PayableAmount;
      this.innerMonetaryTotal.PayableRoundingAmount = this.PayableRoundingAmount;
      this.innerMonetaryTotal.PrepaidAmount = this.PrepaidAmount;
      this.innerMonetaryTotal.TaxExclusiveAmount = this.TaxExclusiveAmount;
      this.innerMonetaryTotal.TaxInclusiveAmount = this.TaxInclusiveAmount;

      return this.innerMonetaryTotal;
    }
  }
}