// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriceCalculator.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.DomainModel.Prices
{
  using System.Collections.Generic;

  /// <summary>
  /// Defines the price calculator class.
  /// </summary>
  public abstract class PriceCalculator
  {
    /// <summary>
    /// Gets or sets the price key.
    /// </summary>
    /// <value>
    /// The price key.
    /// </value>
    public string PriceKey { get; protected set; }

    /// <summary>
    /// Calculates the specified price matrix.
    /// </summary>
    /// <param name="priceMatrix">The price matrix.</param>
    /// <param name="vat">The vat.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns>
    /// The totals.
    /// </returns>
    public abstract Totals Calculate(IDictionary<string, decimal> priceMatrix, decimal vat, uint quantity);
  }
}