// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaxCategory.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the TaxCategory type.
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
  /// Defines the TaxCategory type.
  /// </summary>
  public class TaxCategory : IEntity
  {
    /// <summary>
    /// Identifier for the taxcategory
    /// </summary>
    public virtual string ID { get; set; }

    /// <summary>
    /// Name of the taxcategory
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// The percentage for the taxcategory
    /// </summary>
    public virtual decimal Percent { get; set; }

    /// <summary>
    /// The amount of measure the tax is calculated for if the tax defined as a term for a unit
    /// </summary>
    public virtual Measure BaseUnitMeasure { get; set; }

    /// <summary>
    /// The tax that is set per unit if this is defined
    /// </summary>
    [NotNull]
    public virtual Amount PerUnitAmount { get; set; }

    /// <summary>
    /// Reason for not paying Tax given in a code
    /// </summary>
    public virtual string TaxExemptionReasonCode { get; set; }

    /// <summary>
    /// Reason for not paying tax given in tax
    /// </summary>
    public virtual string TaxExemptionReason { get; set; }

    /// <summary>
    /// Taxarea information
    /// </summary>
    public virtual TaxScheme TaxScheme { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
