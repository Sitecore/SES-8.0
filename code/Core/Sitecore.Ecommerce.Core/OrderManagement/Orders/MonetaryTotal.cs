// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonetaryTotal.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Information about Legal Totals (as opposed to Tax Totals).
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
  using Common;

  /// <summary>
  /// Information about Legal Totals (as opposed to Tax Totals).
  /// </summary>
  public class MonetaryTotal : IEntity
  {
    /// <summary>
    /// Gets or sets the line extension amount.
    /// </summary>
    /// <value>The line extension amount.</value>
    [NotNull]
    public virtual Amount LineExtensionAmount { get; set; }

    /// <summary>
    /// Gets or sets the tax exclusive amount.
    /// </summary>
    /// <value>The tax exclusive amount.</value>
    [NotNull]
    public virtual Amount TaxExclusiveAmount { get; set; }

    /// <summary>
    /// Gets or sets the tax inclusive amount.
    /// </summary>
    /// <value>The tax inclusive amount.</value>
    [NotNull]
    public virtual Amount TaxInclusiveAmount { get; set; }

    /// <summary>
    /// Gets or sets the allowance total amount.
    /// </summary>
    /// <value>The allowance total amount.</value>
    [NotNull]
    public virtual Amount AllowanceTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the charge total amount.
    /// </summary>
    /// <value>The charge total amount.</value>
    [NotNull]
    public virtual Amount ChargeTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets or sets the total prepaid amount.
    /// </summary>
    /// <value>The total prepaid amount.</value>
    [NotNull]
    public virtual Amount PrepaidAmount { get; set; }

    /// <summary>
    /// Gets or sets the payable rounding amount.
    /// </summary>
    /// <value>The payable rounding amount.</value>
    [NotNull]
    public virtual Amount PayableRoundingAmount { get; set; }

    /// <summary>
    /// Gets or sets the payable amount.
    /// </summary>
    /// <value> The payable amount. </value>
    [NotNull]
    public virtual Amount PayableAmount { get; set; }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual long Alias { get; protected set; }
  }
}