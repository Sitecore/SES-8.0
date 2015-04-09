// -------------------------------------------------------------------------------------------
// <copyright file="IProductPriceManager.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.DomainModel.Prices
{
  using Currencies;
  using Products;

  /// <summary>
  /// The product price manager interface.
  /// </summary>
  /// <typeparam name="TProduct">The type of the product.</typeparam>
  /// <typeparam name="TTotals">The type of the totals.</typeparam>
  /// <typeparam name="TCurrency">The type of the currency.</typeparam>
  public interface IProductPriceManager
  {
    /// <summary>
    /// Gets the product totals.
    /// </summary>
    /// <typeparam name="TTotals">The type of the totals.</typeparam>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <typeparam name="TCurrency">The type of the currency.</typeparam>
    /// <param name="product">The product.</param>
    /// <param name="currency">The currency.</param>
    /// <returns>The product prices list.</returns>
    TTotals GetProductTotals<TTotals, TProduct, TCurrency>(TProduct product, TCurrency currency)
      where TProduct : ProductBaseData
      where TTotals : Totals
      where TCurrency : Currency;

    /// <summary>
    /// Gets the product totals.
    /// </summary>
    /// <typeparam name="TTotals">The type of the totals.</typeparam>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <typeparam name="TCurrency">The type of the currency.</typeparam>
    /// <param name="product">The product.</param>
    /// <param name="currency">The currency.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns>The product prices list.</returns>
    TTotals GetProductTotals<TTotals, TProduct, TCurrency>(TProduct product, TCurrency currency, uint quantity)
      where TProduct : ProductBaseData
      where TTotals : Totals
      where TCurrency : Currency;
  }
}