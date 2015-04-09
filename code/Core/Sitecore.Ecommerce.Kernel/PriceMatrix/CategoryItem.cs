// -------------------------------------------------------------------------------------------
// <copyright file="CategoryItem.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.PriceMatrix
{
  using System;
  using System.Collections.Generic;
  using System.Xml.Serialization;

  /// <summary>
  /// Price
  /// </summary>
  [Serializable]
  public class CategoryItem : IPriceMatrixItem
  {
    /// <summary>
    /// </summary>
    [XmlElement("price")]
    public string Amount;

    /// <summary>
    /// </summary>
    [XmlAttribute("id")]
    public string Id;

    /// <summary>
    /// </summary>
    [XmlIgnore]
    private Category parent;

    /// <summary>
    /// </summary>
    [XmlArray("quantities")]
    [XmlArrayItem("quantity", typeof(QuantityPrice))]
    public List<QuantityPrice> QuantityPrices = new List<QuantityPrice>();

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryItem"/> class.
    /// </summary>
    public CategoryItem()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryItem"/> class.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="amount">The amount.</param>
    public CategoryItem(string id, string amount)
    {
      this.Id = id;
      this.Amount = amount;
    }

    /// <summary>
    /// </summary>
    [XmlIgnore]
    public Category Parent
    {
      get
      {
        return this.parent;
      }

      set
      {
        this.parent = value;
      }
    }

    #region IPriceMatrixItem Members

    /// <summary>
    /// </summary>
    [XmlIgnore]
    public string ID
    {
      get
      {
        return this.Id;
      }

      set
      {
        this.Id = value;
      }
    }

    #endregion

    /// <summary>
    /// This will add the quantity price
    /// </summary>
    /// <param name="quantityPrice">
    /// </param>
    public void AddQuantity(QuantityPrice quantityPrice)
    {
      quantityPrice.Parent = this;
      this.QuantityPrices.Add(quantityPrice);
    }

    /// <summary>
    /// Used to convert the <c>Amount</c> to decimal. If <c>showPriceIncVat</c> is true, it will also add <c>vat</c> to <c>Amount</c>.
    /// </summary>
    /// <param name="showPriceIncVat">
    /// </param>
    /// <param name="vat">
    /// </param>
    /// <returns>
    /// Amount converted to decimal.
    /// </returns>
    public string FormatedAmount(bool showPriceIncVat, decimal vat)
    {
      decimal dec = Convert.ToDecimal(this.Amount, Sitecore.Context.Culture);
      if (showPriceIncVat)
      {
        dec = dec + (dec * vat);
      }

      string formatted = dec.ToString("N2");
      return formatted;
    }
  }
}