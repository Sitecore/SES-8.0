// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriceExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Extensions class
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
  using System.Globalization;
  using Sitecore.Ecommerce.Utils;

  /// <summary>
  /// This class provides extension methods to manipulate prices
  /// </summary>
  public static class PriceExtensions
  {
    /// <summary>
    /// Convert the price to cents.
    /// </summary>
    /// <param name="price">The price.</param>
    /// <returns>The price in the string format that is rounded to cents.</returns>
    /// <exception cref="ArgumentException">Price should not be less than zero</exception>
    public static string ToCents(this decimal price)
    {
      if (price < 0)
      {
        throw new ArgumentException("Price should not be less than zero");
      }

      return Math.Round(price * 100, 0, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Rounds the price up to two decimal places. 
    /// </summary>
    /// <param name="price">The price.</param>
    /// <returns>The price in the string format that is rounded up to two decimal places.</returns>
    /// <exception cref="ArgumentException">Price should not be less than zero.</exception>
    public static string RoundToCents(this decimal price)
    {
      if (price < 0)
      {
        throw new ArgumentException("Price should not be less than zero");
      }

      return Math.Round(price, 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Convert the price from cents to dollars.
    /// </summary>
    /// <param name="price">The price.</param>
    /// <returns>The price in big units.</returns>
    /// <exception cref="ArgumentException">Price should not be null or empty.</exception>
    public static decimal FromCents(this string price)
    {
      if (string.IsNullOrEmpty(price))
      {
        throw new ArgumentException("Price should not be null or empty");
      }

      decimal amount = TypeUtil.TryParse(price, decimal.Zero) / 100;
      if (amount < 0)
      {
        throw new ArgumentException("Price should not be less than zero");
      }

      return amount;
    }
  }
}
