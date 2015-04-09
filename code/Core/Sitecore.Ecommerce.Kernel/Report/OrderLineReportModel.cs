// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineReportModel.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order line report model class.
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
  /// Defines the order line report model class.
  /// </summary>
  public class OrderLineReportModel
  {
    /// <summary>
    /// Gets or sets the order line.
    /// </summary>
    /// <value>The order line.</value>
    [StiBrowsable(false)]
    [CanBeNull]
    public OrderLine OrderLine { get; set; }

    /// <summary>
    /// Gets the order line item code.
    /// </summary>
    /// <value>The order line item code.</value>
    [NotNull]
    public virtual string OrderLineItemCode
    {
      get
      {
        if ((this.OrderLine != null) && (this.OrderLine.LineItem != null) && (this.OrderLine.LineItem.Item != null))
        {
          return this.OrderLine.LineItem.Item.Code;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the name of the order line item.
    /// </summary>
    /// <value>The name of the order line item.</value>
    [NotNull]
    public virtual string OrderLineItemName
    {
      get
      {
        if ((this.OrderLine != null) && (this.OrderLine.LineItem != null) && (this.OrderLine.LineItem.Item != null))
        {
          return this.OrderLine.LineItem.Item.Name;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the order line item description.
    /// </summary>
    /// <value>The order line item description.</value>
    [NotNull]
    public virtual string OrderLineItemDescription
    {
      get
      {
        if ((this.OrderLine != null) && (this.OrderLine.LineItem != null) && (this.OrderLine.LineItem.Item != null))
        {
          return StringUtil.RemoveTags(this.OrderLine.LineItem.Item.Description);
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    public virtual decimal OrderLineQuantity
    {
      get
      {
        if ((this.OrderLine != null) && (this.OrderLine.LineItem != null))
        {
          return this.OrderLine.LineItem.Quantity;
        }

        return 0;
      }
    }

    /// <summary>
    /// Gets the order line price.
    /// </summary>
    /// <value>The order line price.</value>
    [NotNull]
    public virtual string OrderLinePrice
    {
      get
      {
        if ((this.OrderLine != null) && (this.OrderLine.LineItem != null) && (this.OrderLine.LineItem.Price != null) && (this.OrderLine.LineItem.Price.PriceAmount != null))
        {
          return this.GetAmountRepresentation(this.OrderLine.LineItem.Price.PriceAmount);
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the order line extension price.
    /// </summary>
    /// <value>The order line extension price.</value>
    [NotNull]
    public virtual string OrderLineExtensionPrice
    {
      get
      {
        if ((this.OrderLine != null) && (this.OrderLine.LineItem != null) && (this.OrderLine.LineItem.LineExtensionAmount != null))
        {
          return this.GetAmountRepresentation(this.OrderLine.LineItem.LineExtensionAmount);
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the order line sub-lines.
    /// </summary>
    /// <value>The order line sub-lines.</value>
    [NotNull]
    public virtual IEnumerable<SublineReportModel> OrderLineSublines
    {
      get
      {
        if ((this.OrderLine != null) && (this.OrderLine.LineItem != null) && (this.OrderLine.LineItem.SubLineItem != null))
        {
          return this.OrderLine.LineItem.SubLineItem.Select(subline => new SublineReportModel { Subline = subline });
        }

        return Enumerable.Empty<SublineReportModel>();
      }
    }

    /// <summary>
    /// Gets the order line total tax amount.
    /// </summary>
    public virtual string OrderLineTotalTaxAmount
    {
      get
      {
        if ((this.OrderLine != null) && (this.OrderLine.LineItem != null) && (this.OrderLine.LineItem.TotalTaxAmount != null))
        {
          return this.GetAmountRepresentation(this.OrderLine.LineItem.TotalTaxAmount);
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the order line total tax percent.
    /// </summary>
    public virtual string OrderLineTotalTaxPercent
    {
      get
      {
        if (this.OrderLine != null)
        {
          int i = this.OrderLine.Order.OrderLines.ToList().IndexOf(this.OrderLine);
          TaxSubTotal taxSubtotal = this.OrderLine.Order.TaxTotal.TaxSubtotal.First(ts => ts.CalculationSequenceNumeric == i);
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
      Debug.ArgumentNotNull(amount, "amount");

      return Utils.MainUtil.FormatPrice(amount.Value, true, null, amount.CurrencyID);
    }
  }
}