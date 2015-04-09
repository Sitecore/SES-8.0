// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllowanceCharge.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The allowance charge.
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
  /// The allowance charge.
  /// </summary>
  public class AllowanceCharge : IEntity
  {
    /// <summary>
    /// Identifier
    /// </summary>
    public virtual string ID { get; set; }

    /// <summary>
    /// Identifies if the discount is a fee or a discount ( true/false
    /// </summary>
    public virtual bool ChargeIndicator { get; set; }

    /// <summary>
    /// Reason for the discount as a code
    /// </summary>
    public virtual string AllowanceChargeReasonCode { get; set; }

    /// <summary>
    /// Reason for the discount as a text
    /// </summary>
    public virtual string AllowanceChargeReason { get; set; }

    /// <summary>
    /// The number the base amount is multiplied with to calculate the discount
    /// </summary>
    public virtual decimal MultiplierFactorNumeric { get; set; }

    /// <summary>
    /// Is discount prepaid or not
    /// </summary>
    public virtual bool PrepaidIndicator { get; set; }

    /// <summary>
    /// The sequence the discount is calculated in
    /// </summary>
    public virtual decimal SequenceNumeric { get; set; }

    /// <summary>
    /// The amount for the discount
    /// </summary>
    public virtual Amount Amount { get; set; }

    /// <summary>
    /// The amount used for MultiplierFactorNumeric when calculating discount
    /// </summary>
    public virtual Amount BaseAmount { get; set; }

    /// <summary>
    /// Information about the tax category
    /// </summary>
    public virtual TaxCategory TaxCategory { get; set; }

    /// <summary>
    /// Information about the taxtotal of the discount if any
    /// </summary>
    public virtual TaxTotal TaxTotal { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
