// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderPriceCalculator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Recalculates order prices.
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

namespace Sitecore.Ecommerce.DomainModel.Orders
{
  using System;
  using System.Linq;
  using Prices;

  /// <summary>
  /// Recalculates order prices.
  /// </summary>
  [Obsolete]
  public class OrderPriceCalculator
  {
    /// <summary>
    /// Recalculates the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <exception cref="ArgumentNullException"><paramref name="order"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException"><c>InvalidOperationException</c>.</exception>
    public virtual void Recalculate(ref Order order)
    {
      if (order == null)
      {
        throw new ArgumentNullException();
      }

      if (order.OrderLines == null || order.Totals == null)
      {
        throw new InvalidOperationException();
      }

      var orderTotals = new Totals();

      foreach (var orderLine in order.OrderLines)
      {
        var lineTotals = orderLine.Totals;
        if (lineTotals == null)
        {
          throw new InvalidOperationException();
        }

        decimal quantity = orderLine.Quantity;

        lineTotals.TotalVat = lineTotals.PriceExVat * lineTotals.VAT * quantity;

        lineTotals.PriceIncVat = lineTotals.PriceExVat * (1 + lineTotals.VAT);

        lineTotals.TotalPriceExVat = lineTotals.PriceExVat * quantity;
        lineTotals.TotalPriceIncVat = lineTotals.PriceIncVat * quantity;

        lineTotals.ToList().ForEach(p => orderTotals[p.Key] += p.Value);
      }

      orderTotals.TotalPriceExVat += order.ShippingPrice;
      orderTotals.TotalPriceIncVat += order.ShippingPrice;

      order.Totals.TotalVat = orderTotals.TotalVat;
      order.Totals.VAT = orderTotals.VAT;

      order.Totals.PriceExVat = orderTotals.TotalPriceExVat;
      order.Totals.PriceIncVat = orderTotals.TotalPriceIncVat;

      // Reset redundant fields.
      order.Totals.TotalPriceExVat = 0;
      order.Totals.TotalPriceIncVat = 0;
    }
  }
}
