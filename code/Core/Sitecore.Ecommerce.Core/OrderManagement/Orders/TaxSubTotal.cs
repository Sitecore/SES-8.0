// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaxSubTotal.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the tax sub total class.
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
  using Sitecore.Ecommerce.Common;

  /// <summary>
  /// Defines the tax sub total class.
  /// </summary>
  public class TaxSubTotal : IEntity
  {
    /// <summary>
    /// Gets or sets the netto amount the that is being used for multiplieng when calculating the taxamount.
    /// </summary>
    /// <value>The taxable amount.</value>
    [NotNull]
    public virtual Amount TaxableAmount { get; set; }

    /// <summary>
    /// Gets or sets the tax amount.
    /// </summary>
    /// <value>The tax amount.</value>
    [NotNull]
    public virtual Amount TaxAmount { get; set; }

    /// <summary>
    /// The order which the tax rate should be calculated if more than one tax rate defined for the order.
    /// </summary>
    public virtual decimal CalculationSequenceNumeric { get; set; }

    /// <summary>
    /// Gets or sets the transaction currency tax amount.
    /// </summary>
    /// <value>The transaction currency tax amount.</value>
    [NotNull]
    public virtual Amount TransactionCurrencyTaxAmount { get; set; }

    /// <summary>
    /// Gets or sets the tax category.
    /// </summary>
    /// <value>The tax category.</value>
    [NotNull]
    public virtual TaxCategory TaxCategory { get; set; }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual long Alias { get; set; }
  }
}