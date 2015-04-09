// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Amount.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the amount class.
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

namespace Sitecore.Ecommerce.Common
{
  using System;
  using Diagnostics;

  /// <summary>
  /// Defines the amount class.
  /// </summary>
  public class Amount
  {
    /// <summary>
    /// Stores currency ID.
    /// </summary>
    private string currencyId;

    /// <summary>
    /// Stores value.
    /// </summary>
    private decimal value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Amount" /> class.
    /// </summary>
    public Amount()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Amount"/> class.
    /// </summary>
    /// <param name="currencyId">The currency id.</param>
    public Amount([NotNull] string currencyId)
      : this(0, currencyId)
    {
      Assert.ArgumentNotNull(currencyId, "currencyId");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Amount"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="currencyId">The currency id.</param>
    /// <exception cref="ArgumentOutOfRangeException"><c>Amount cannot be negative.</c> is out of range.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><c>value</c> is out of range.</exception>
    public Amount(decimal value, [NotNull] string currencyId)
    {
      Assert.ArgumentCondition(value >= 0, null, "Amount cannot be negative.");
      Assert.ArgumentNotNullOrEmpty(currencyId, "currencyId");

      this.value = value;
      this.currencyId = currencyId;
    }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public virtual decimal Value
    {
      get
      {
        return this.value;
      }

      set
      {
        Assert.ArgumentCondition(value >= 0, null, "Amount cannot be negative.");

        this.value = value;
      }
    }

    /// <summary>
    /// Gets or sets the currency ID.
    /// </summary>
    /// <value>
    /// The currency ID.
    /// </value>
    [NotNull]
    public virtual string CurrencyID
    {
      get
      {
        return this.currencyId;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        Assert.ArgumentNotNullOrEmpty(value, "value");

        this.currencyId = value;
      }
    }

    /// <summary>
    /// Adds the specified amount1.
    /// </summary>
    /// <param name="amount1">The amount1.</param>
    /// <param name="amount2">The amount2.</param>
    /// <returns>The amount.</returns>
    [NotNull]
    public static Amount Add([NotNull] Amount amount1, [NotNull] Amount amount2)
    {
      Assert.ArgumentNotNull(amount1, "amount1");
      Assert.ArgumentNotNull(amount2, "amount2");

      AssertCurrencies(amount1, amount2, "summarize", "+");

      return new Amount(amount1.Value + amount2.Value, amount1.CurrencyID);
    }

    /// <summary>
    /// Subtracts the specified amount1.
    /// </summary>
    /// <param name="amount1">The amount1.</param>
    /// <param name="amount2">The amount2.</param>
    /// <returns>The amount.</returns>
    [NotNull]
    public static Amount Subtract([NotNull] Amount amount1, [NotNull] Amount amount2)
    {
      Assert.ArgumentNotNull(amount1, "amount1");
      Assert.ArgumentNotNull(amount2, "amount2");

      AssertCurrencies(amount1, amount2, "substract", "-");

      return new Amount(amount1.Value - amount2.Value, amount1.CurrencyID);
    }

    /// <summary>
    /// Multiplies the specified amount.
    /// </summary>
    /// <param name="amount">The amount to multiply.</param>
    /// <param name="multiplier">The multiplier.</param>
    /// <returns>The resulting amount.</returns>
    [NotNull]
    public static Amount Multiply([NotNull] Amount amount, decimal multiplier)
    {
      Assert.ArgumentNotNull(amount, "amount");
      Assert.ArgumentCondition(multiplier >= 0, null, "Multiplier must be not negative.");

      return new Amount(amount.Value * multiplier, amount.CurrencyID);
    }

    /// <summary>
    /// Divides the specified amount.
    /// </summary>
    /// <param name="amount">The amount to divide.</param>
    /// <param name="divisor">The divisor.</param>
    /// <returns>The resulting amount.</returns>
    [NotNull]
    public static Amount Divide([NotNull] Amount amount, decimal divisor)
    {
      Assert.ArgumentNotNull(amount, "amount");
      Assert.ArgumentCondition(divisor >= 0, null, "Divisor must be positive.");

      return new Amount(amount.Value / divisor, amount.CurrencyID);
    }

    /// <summary>
    /// Implements the operator +.
    /// </summary>
    /// <param name="amount1">The amount1.</param>
    /// <param name="amount2">The amount2.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static Amount operator +(Amount amount1, Amount amount2)
    {
      return Add(amount1, amount2);
    }

    /// <summary>
    /// Implements the operator -.
    /// </summary>
    /// <param name="amount1">The amount1.</param>
    /// <param name="amount2">The amount2.</param>
    /// <returns>The result of the operator.</returns>
    public static Amount operator -(Amount amount1, Amount amount2)
    {
      return Subtract(amount1, amount2);
    }

    /// <summary>
    /// Implements the operator *.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="multiplier">The multiplier.</param>
    /// <returns>The result of the operator.</returns>
    public static Amount operator *(Amount amount, decimal multiplier)
    {
      return Multiply(amount, multiplier);
    }

    /// <summary>
    /// Implements the operator *.
    /// </summary>
    /// <param name="multiplier">The multiplier.</param>
    /// <param name="amount">The amount.</param>
    /// <returns>The result of the operator.</returns>
    public static Amount operator *(decimal multiplier, Amount amount)
    {
      return Multiply(amount, multiplier);
    }

    /// <summary>
    /// Implements the operator /.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="divisor">The divisor.</param>
    /// <returns>The result of the operator.</returns>
    public static Amount operator /(Amount amount, decimal divisor)
    {
      return Divide(amount, divisor);
    }

    /// <summary>
    /// ==s the specified amount1.
    /// </summary>
    /// <param name="amount1">The amount1.</param>
    /// <param name="amount2">The amount2.</param>
    /// <returns></returns>
    public static bool operator ==(Amount amount1, Amount amount2)
    {
      if ((object)amount1 == null && (object)amount2 == null)
      {
        return true;
      }

      if ((object)amount1 == null || (object)amount2 == null)
      {
        return false;
      }

      return amount1.Equals(amount2);
    }

    /// <summary>
    /// !=s the specified amount1.
    /// </summary>
    /// <param name="amount1">The amount1.</param>
    /// <param name="amount2">The amount2.</param>
    /// <returns></returns>
    public static bool operator !=(Amount amount1, Amount amount2)
    {
      return !(amount1 == amount2);
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="amount">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object amount)
    {
      if (ReferenceEquals(null, amount))
      {
        return false;
      }

      if (ReferenceEquals(this, amount))
      {
        return true;
      }

      if (amount.GetType() != this.GetType())
      {
        return false;
      }

      return Equals((Amount)amount);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode()
    {
      unchecked
      {
        return ((this.currencyId != null ? this.currencyId.GetHashCode() : 0) * 397) ^ this.value.GetHashCode();
      }
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      return string.Concat(this.CurrencyID, this.value);
    }

    /// <summary>
    /// Equalses the specified other.
    /// </summary>
    /// <param name="other">The other.</param>
    /// <returns>The result.</returns>
    protected bool Equals(Amount other)
    {
      return string.Equals(this.currencyId, other.currencyId, StringComparison.InvariantCultureIgnoreCase) && this.value == other.value;
    }

    /// <summary>
    /// Asserts the currencies.
    /// </summary>
    /// <param name="a1">The amount1.</param>
    /// <param name="a2">The amount2.</param>
    /// <param name="operationName">The operationName.</param>
    /// <exception cref="System.ArgumentException"></exception>
    private static void AssertCurrencies(Amount a1, Amount a2, string operationName, string operationSign)
    {
      if (string.Compare(a1.CurrencyID, a2.CurrencyID, StringComparison.OrdinalIgnoreCase) != 0)
      {
        throw new ArgumentException(string.Format("Cannot {0} amounts in different currencies: {1} {2} {3}.", operationName, a1, operationSign, a2));
      }
    }
  }
}