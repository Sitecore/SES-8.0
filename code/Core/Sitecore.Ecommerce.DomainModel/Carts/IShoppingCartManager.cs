// -------------------------------------------------------------------------------------------
// <copyright file="IShoppingCartManager.cs" company="Sitecore Corporation">
//  Copyright (c) Sitecore Corporation 1999-2015 
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

namespace Sitecore.Ecommerce.DomainModel.Carts
{
  /// <summary>
  /// The shopping cart manager interface.
  /// </summary>
  public interface IShoppingCartManager
  {
    /// <summary>
    /// Adds to shopping cart.
    /// </summary>
    /// <param name="productCode">
    /// The product code.
    /// </param>
    /// <param name="quantity">
    /// The quantity.
    /// </param>
    void AddProduct(string productCode, uint quantity);

    /// <summary>
    /// Updates the product quantity.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="quantity">The quantity.</param>
    void UpdateProductQuantity(string productCode, uint quantity);

    /// <summary>
    /// Removes the product from shopping cart.
    /// </summary>
    /// <param name="productCode">
    /// The product code.
    /// </param>
    void RemoveProduct(string productCode);

    /// <summary>
    /// Removes the product line from shopping cart.
    /// </summary>
    /// <param name="productCode">
    /// The product code.
    /// </param>
    void RemoveProductLine(string productCode);
  }
}