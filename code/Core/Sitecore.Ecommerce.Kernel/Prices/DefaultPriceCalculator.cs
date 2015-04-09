// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultPriceCalculator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the price calculator class.
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
  using System.Collections.Generic;
  using Diagnostics;
  using DomainModel.Prices;

  /// <summary>
  /// Defines the price calculator class.
  /// </summary>
  public class DefaultPriceCalculator : PriceCalculator
  {
    /// <summary>
    /// Defines totals factory.
    /// </summary>
    private readonly TotalsFactory totalsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultPriceCalculator" /> class.
    /// </summary>
    /// <param name="priceKey">The price key for all the payment operations.</param>
    /// <param name="totalsFactory">The totals factory.</param>
    public DefaultPriceCalculator([NotNull] string priceKey, [NotNull] TotalsFactory totalsFactory)
    {
      Assert.ArgumentNotNullOrEmpty(priceKey, "priceKey");
      Assert.ArgumentNotNull(totalsFactory, "totalsFactory");

      this.totalsFactory = totalsFactory;
      this.PriceKey = priceKey;
    }

    /// <summary>
    /// Gets the totals factory.
    /// </summary>
    [NotNull]
    protected TotalsFactory TotalsFactory
    {
      get { return this.totalsFactory; }
    }

    /// <summary>
    /// Calculates the specified price matrix.
    /// </summary>
    /// <param name="priceMatrix">The price matrix.</param>
    /// <param name="vat">The vat.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns>
    /// The totals.
    /// </returns>
    [NotNull]
    public override DomainModel.Prices.Totals Calculate([NotNull] IDictionary<string, decimal> priceMatrix, decimal vat, uint quantity)
    {
      Assert.ArgumentNotNull(priceMatrix, "priceMatrix");

      decimal price = priceMatrix[this.PriceKey];
      decimal normalPrice = priceMatrix["NormalPrice"];
      decimal memberPrice = priceMatrix["MemberPrice"];

      if (price == decimal.Zero)
      {
        price = normalPrice;
      }

      if (memberPrice == decimal.Zero)
      {
        memberPrice = normalPrice;
      }

      DomainModel.Prices.Totals totals = this.TotalsFactory.Create();
      
      totals.PriceExVat = price;
      totals.PriceIncVat = price * (1 + vat);
      totals.MemberPrice = memberPrice;
      totals.VAT = vat;

      if (quantity == 0)
      {
        return totals;
      }

      totals.TotalPriceExVat = totals.PriceExVat * quantity;
      totals.TotalPriceIncVat = totals.PriceIncVat * quantity;

      totals.TotalVat = totals.TotalPriceIncVat - totals.TotalPriceExVat;

      totals.DiscountExVat = (normalPrice - price) * quantity;
      totals.DiscountIncVat = totals.DiscountExVat * (1 + vat);

      totals.PossibleDiscountExVat = (price - memberPrice) * quantity;
      totals.PossibleDiscountIncVat = totals.PossibleDiscountExVat * (1 + vat);

      return totals;
    }
  }
}