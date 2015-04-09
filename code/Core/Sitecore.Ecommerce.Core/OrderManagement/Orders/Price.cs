// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Price.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the price class.
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

namespace Sitecore.Ecommerce.OrderManagement.Orders
{
  using System.Collections.Generic;
  using Common;
  using Diagnostics;

  /// <summary>
  /// Defines the price class.
  /// </summary>
  public class Price : IEntity
  {
    /// <summary>
    /// The amount.
    /// </summary>
    private Amount amount;

    /// <summary>
    /// The base quantity.
    /// </summary>
    private decimal baseQuantity;

    /// <summary>
    /// Orderable unit factor rate.
    /// </summary>
    private decimal orderableUnitFactorRate;

    /// <summary>
    /// Initializes a new instance of the <see cref="Price"/> class.
    /// </summary>
    public Price()
    {
      this.orderableUnitFactorRate = 1;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Price"/> class.
    /// </summary>
    /// <param name="amount">The amount.</param>
    public Price([NotNull] Amount amount)
      : this(amount, 1)
    {
      Assert.ArgumentNotNull(amount, "amount");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Price"/> class.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="baseQuantity">The base quantity.</param>
    public Price([NotNull] Amount amount, decimal baseQuantity)
    {
      Assert.ArgumentNotNull(amount, "amount");

      this.amount = amount;
      this.baseQuantity = baseQuantity;
      this.orderableUnitFactorRate = 1;
    }

    /// <summary>
    /// Gets or sets the price amount.
    /// </summary>
    /// <value>
    /// The price amount.
    /// </value>
    [NotNull]
    public virtual Amount PriceAmount
    {
      get
      {
        return this.amount;
      } 

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.amount = value;
      }
    }

    /// <summary>
    /// Gets or sets the base quantity.
    /// </summary>
    /// <value>
    /// The base quantity.
    /// </value>
    public virtual decimal BaseQuantity
    {
      get { return this.baseQuantity; }
      set { this.baseQuantity = value; }
    }

    public virtual string PriceChangeReason { get; set; }

    public virtual string PriceTypeCode { get; set; }

    public virtual string PriceType { get; set; }

    public virtual decimal OrderableUnitFactorRate
    {
      get
      {
        return this.orderableUnitFactorRate;
      }
      
      set
      {
        Assert.IsTrue(value >= 0, "Orderable unit factor rate must be a non-negative number.");
        this.orderableUnitFactorRate = value;
      }
    }

    public virtual long Alias { get; protected set; }

    public virtual IEnumerable<AllowanceCharge> AllowanceCharge { get; set; }
  }
}