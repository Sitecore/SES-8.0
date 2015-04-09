// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Totals.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Product prices list.
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

namespace Sitecore.Ecommerce.Prices
{
  using System;
  using System.Collections.Generic;
  using System.Runtime.Serialization;
  using Data;
  using DomainModel.Data;
  using Validators.Interception;
 
  /// <summary>
  /// Product prices list.
  /// </summary>
  [Serializable, CollectionDataContract, Obsolete("Use Sitecore.Ecommerce.DomainModel.Prices.Totals instead.")]
  public class Totals : DomainModel.Prices.Totals, IEntity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Totals"/> class.
    /// </summary>
    public Totals()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Totals"/> class.
    /// </summary>
    /// <param name="totals">The totals.</param>
    public Totals(IDictionary<string, decimal> totals) : base(totals)
    {
    }

    /// <summary>
    /// Gets or sets the indirect tax.
    /// </summary>
    /// <value>The indirect tax.</value>
    [Entity(FieldName = "Vat")]
    public override decimal VAT 
    { 
      get { return base.VAT; }
      set { base.VAT = value; }
    }

    /// <summary>
    /// Gets or sets the total vat.
    /// </summary>
    /// <value>The total vat.</value>
    [Entity(FieldName = "TotalVat")]
    public override decimal TotalVat
    { 
      get { return base.TotalVat; }
      set { base.TotalVat = value; }
    }

    /// <summary>
    /// Gets or sets the member price.
    /// </summary>
    /// <value>The member price.</value>
    [Entity(FieldName = "Member Price")]
    public override decimal MemberPrice
    { 
      get { return base.MemberPrice; }
      set { base.MemberPrice = value; }
    }

    /// <summary>
    /// Gets or sets the full price.
    /// </summary>
    /// <value>The full price.</value>
    [Entity(FieldName = "TotalPriceExVat")]
    public override decimal TotalPriceExVat
    { 
      get { return base.TotalPriceExVat; }
      set { base.TotalPriceExVat = value; }
    }

    /// <summary>
    /// Gets or sets the total price inc vat.
    /// </summary>
    /// <value>The total price inc vat.</value>
    [Entity(FieldName = "TotalPriceIncVat")]
    public override decimal TotalPriceIncVat
    { 
      get { return base.TotalPriceIncVat; }
      set { base.TotalPriceIncVat = value; }
    }

    /// <summary>
    /// Gets or sets the price inc vat.
    /// </summary>
    /// <value>The price inc vat.</value>
    [Entity(FieldName = "PriceIncVat")]
    public override decimal PriceIncVat
    { 
      get { return base.PriceIncVat; }
      set { base.PriceIncVat = value; }
    }

    /// <summary>
    /// Gets or sets the price ex vat.
    /// </summary>
    /// <value>The price ex vat.</value>
    [Entity(FieldName = "PriceExVat")]
    public override decimal PriceExVat
    { 
      get { return base.PriceExVat; }
      set { base.PriceExVat = value; }
    }

    /// <summary>
    /// Gets or sets the discount inc vat.
    /// </summary>
    /// <value>The discount inc vat.</value>
    [Entity(FieldName = "DiscountIncVat")]
    public override decimal DiscountIncVat
    { 
      get { return base.DiscountIncVat; }
      set { base.DiscountIncVat = value; }
    }

    /// <summary>
    /// Gets or sets the discount ex vat.
    /// </summary>
    /// <value>The discount ex vat.</value>
    [Entity(FieldName = "DiscountExVat")]
    public override decimal DiscountExVat
    { 
      get { return base.DiscountExVat; }
      set { base.DiscountExVat = value; }
    }

    /// <summary>
    /// Gets or sets the possible discount inc vat.
    /// </summary>
    /// <value>The possible discount inc vat.</value>
    [Entity(FieldName = "PossibleDiscountIncVat")]
    public override decimal PossibleDiscountIncVat
    { 
      get { return base.PossibleDiscountIncVat; }
      set { base.PossibleDiscountIncVat = value; }
    }

    /// <summary>
    /// Gets or sets the possible discount ex vat.
    /// </summary>
    /// <value>The possible discount ex vat.</value>
    [Entity(FieldName = "PossibleDiscountExVat")]
    public override decimal PossibleDiscountExVat
    { 
      get { return base.PossibleDiscountExVat; }
      set { base.PossibleDiscountExVat = value; }
    }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual string Alias { get; [NotNullValue] set; }

    #endregion
  }
}