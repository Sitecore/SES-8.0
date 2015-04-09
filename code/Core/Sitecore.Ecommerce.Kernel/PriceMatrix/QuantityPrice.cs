// -------------------------------------------------------------------------------------------
// <copyright file="QuantityPrice.cs" company="Sitecore Corporation">
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
  using System.Xml.Serialization;

  /// <summary>
  /// QuantityPrice
  /// </summary>
  [Serializable]
  public class QuantityPrice
  {
    /// <summary>
    /// </summary>
    [XmlText]
    public string Amount;

    /// <summary>
    /// </summary>
    [XmlIgnore]
    private IPriceMatrixItem parent;

    /// <summary>
    /// </summary>
    [XmlAttribute("quantity")]
    public string Quantity;

    /// <summary>
    /// </summary>
    public QuantityPrice()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="quantity">
    /// </param>
    /// <param name="amount">
    /// </param>
    public QuantityPrice(string quantity, string amount)
    {
      this.Quantity = quantity;
      this.Amount = amount;
    }

    /// <summary>
    /// </summary>
    [XmlIgnore]
    public IPriceMatrixItem Parent
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