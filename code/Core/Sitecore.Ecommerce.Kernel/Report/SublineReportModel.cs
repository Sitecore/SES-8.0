// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SublineReportModel.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the subline report model class.
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

namespace Sitecore.Ecommerce.Report
{
  using System.Collections.Generic;
  using System.Linq;
  using Common;
  using Diagnostics;
  using OrderManagement.Orders;
  using Stimulsoft.Base.Design;

  /// <summary>
  /// Defines the subline report model class.
  /// </summary>
  public class SublineReportModel
  {
    /// <summary>
    /// Gets or sets the subline.
    /// </summary>
    /// <value>The subline.</value>
    [StiBrowsable(false)]
    [CanBeNull]
    public LineItem Subline { get; set; }

    /// <summary>
    /// Gets the order line item code.
    /// </summary>
    /// <value>The order line item code.</value>
    [NotNull]
    public virtual string SublineItemCode
    {
      get
      {
        if ((this.Subline != null) && (this.Subline.Item != null))
        {
          return this.Subline.Item.Code;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the name of the subline item.
    /// </summary>
    /// <value>The name of the subline item.</value>
    [NotNull]
    public virtual string SublineItemName
    {
      get
      {
        if ((this.Subline != null) && (this.Subline.Item != null))
        {
          return this.Subline.Item.Name;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    [NotNull]
    public virtual decimal SublineQuantity
    {
      get
      {
        if (this.Subline != null)
        {
          return this.Subline.Quantity;
        }

        return 0;
      }
    }

    /// <summary>
    /// Gets the order line price.
    /// </summary>
    /// <value>The order line price.</value>
    [NotNull]
    public virtual string SublinePrice
    {
      get
      {
        if ((this.Subline != null) && (this.Subline.Price != null) && (this.Subline.Price.PriceAmount != null))
        {
          return this.GetAmountRepresentation(this.Subline.Price.PriceAmount);
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the order line extension price.
    /// </summary>
    /// <value>The order line extension price.</value>
    [NotNull]
    public virtual string SublineExtensionPrice
    {
      get
      {
        if ((this.Subline != null) && (this.Subline.LineExtensionAmount != null))
        {
          return this.GetAmountRepresentation(this.Subline.LineExtensionAmount);
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the sublines.
    /// </summary>
    /// <value>The sublines.</value>
    [NotNull]
    public virtual IEnumerable<SublineReportModel> Sublines
    {
      get
      {
        if ((this.Subline != null) && (this.Subline.SubLineItem != null))
        {
          return this.Subline.SubLineItem.Select(s => new SublineReportModel { Subline = s });
        }

        return Enumerable.Empty<SublineReportModel>();
      }
    }

    /// <summary>
    /// Gets the order line total tax amount.
    /// </summary>
    public virtual string SubLineTotalTaxAmount
    {
      get
      {
        if (this.Subline != null)
        {
          return this.GetAmountRepresentation(this.Subline.TotalTaxAmount);
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the order line total tax percent.
    /// </summary>
    public virtual string SubLineTotalTaxPercent
    {
      get
      {
        if (this.Subline != null)
        {
          int i = this.Subline.OrderLine.Order.OrderLines.ToList().IndexOf(this.Subline.OrderLine);
          var taxSubtotal = this.Subline.OrderLine.Order.TaxTotal.TaxSubtotal.First(ts => ts.CalculationSequenceNumeric == i);
          if (null != taxSubtotal)
          {
            return Utils.MainUtil.FormatPrice(taxSubtotal.TaxCategory.Percent, false, null, null);
          }
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the amount representation.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <returns>The amount representation.</returns>
    [NotNull]
    protected virtual string GetAmountRepresentation([NotNull] Amount amount)
    {
      Assert.IsNotNull(amount, "amount");

      return Utils.MainUtil.FormatPrice(amount.Value, true, null, amount.CurrencyID);
    }
  }
}