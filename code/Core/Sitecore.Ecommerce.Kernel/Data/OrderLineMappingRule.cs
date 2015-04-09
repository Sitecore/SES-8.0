// -------------------------------------------------------------------------------------------
// <copyright file="OrderLineMappingRule.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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

namespace Sitecore.Ecommerce.Data
{
  using System.Globalization;
  using DomainModel.Orders;
  using Utils;
  using Validators.Interception;

  /// <summary>
  /// The product line mapping rule.
  /// </summary>
  public class OrderLineMappingRule : IMappingRule<OrderLine>
  {
    #region Implementation of IMappingRule<Order>

    /// <summary>
    /// Gets or sets the mapping object.
    /// </summary>
    /// <value>The mapping object.</value>
    public OrderLine MappingObject { get; [NotNullValue] set; }

    #endregion

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The type of the order line.</value>
    [Entity(FieldName = "Type")]
    public virtual string Type
    {
      get
      {
        return this.MappingObject.Type;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Type = value;
      }
    }

    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    /// <value>The product line id.</value>
    [Entity(FieldName = "Id")]
    public virtual string Id
    {
      get
      {
        return this.MappingObject.Product.Code;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Product.Code = value;
      }
    }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [Entity(FieldName = "Description")]
    public virtual string Description
    {
      get
      {
        return this.MappingObject.Product.Title;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Product.Title = value;
      }
    }

    /// <summary>
    /// Gets or sets the VAT.
    /// </summary>
    /// <value>The VAT value.</value>
    [Entity(FieldName = "Vat")]
    public virtual string Vat
    {
      get
      {
        return this.MappingObject.Totals.VAT.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.VAT = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the total price ex vat.
    /// </summary>
    /// <value>The total price ex vat.</value>
    [Entity(FieldName = "TotalPriceExVat")]
    public virtual string TotalPriceExVat
    {
      get
      {
        return this.MappingObject.Totals.TotalPriceExVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.TotalPriceExVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the total price inc vat.
    /// </summary>
    /// <value>The total price inc vat.</value>
    [Entity(FieldName = "TotalPriceIncVat")]
    public virtual string TotalPriceIncVat
    {
      get
      {
        return this.MappingObject.Totals.TotalPriceIncVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.TotalPriceIncVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the discount ex vat.
    /// </summary>
    /// <value>The discount ex vat.</value>
    [Entity(FieldName = "DiscountExVat")]
    public virtual string DiscountExVat
    {
      get
      {
        return this.MappingObject.Totals.DiscountExVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.DiscountExVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the discount inc vat.
    /// </summary>
    /// <value>The discount inc vat.</value>
    [Entity(FieldName = "DiscountIncVat")]
    public virtual string DiscountIncVat
    {
      get
      {
        return this.MappingObject.Totals.DiscountIncVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.DiscountIncVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the price ex vat.
    /// </summary>
    /// <value>The price ex vat.</value>
    [Entity(FieldName = "PriceExVat")]
    public virtual string PriceExVat
    {
      get
      {
        return this.MappingObject.Totals.PriceExVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.PriceExVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the price inc vat.
    /// </summary>
    /// <value>The price inc vat.</value>
    [Entity(FieldName = "PriceIncVat")]
    public virtual string PriceIncVat
    {
      get
      {
        return this.MappingObject.Totals.PriceIncVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.PriceIncVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the total vat.
    /// </summary>
    /// <value>The total vat.</value>
    [Entity(FieldName = "TotalVat")]
    public virtual string TotalVat
    {
      get
      {
        return this.MappingObject.Totals.TotalVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.TotalVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the possible discount ex vat.
    /// </summary>
    /// <value>The possible discount ex vat.</value>
    [Entity(FieldName = "PossibleDiscountExVat")]
    public virtual string PossibleDiscountExVat
    {
      get
      {
        return this.MappingObject.Totals.PossibleDiscountExVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.PossibleDiscountExVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the possible discount inc vat.
    /// </summary>
    /// <value>The possible discount inc vat.</value>
    [Entity(FieldName = "PossibleDiscountIncVat")]
    public virtual string PossibleDiscountIncVat
    {
      get
      {
        return this.MappingObject.Totals.PossibleDiscountIncVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.PossibleDiscountIncVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    [Entity(FieldName = "Quantity")]
    public virtual string Quantity
    {
      get
      {
        return this.MappingObject.Quantity.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Quantity = TypeUtil.Parse<uint>(value);
      }
    }

    /// <summary>
    /// Gets or sets the friendly URL.
    /// </summary>
    /// <value>The friendly URL.</value>
    [Entity(FieldName = "ProductUrl")]
    public virtual string FriendlyUrl
    {
      get
      {
        return this.MappingObject.FriendlyUrl;
      }
      set
      {
        this.MappingObject.FriendlyUrl = value;
      }
    }
  }
}