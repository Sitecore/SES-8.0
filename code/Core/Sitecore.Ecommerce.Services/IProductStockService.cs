// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductStockService.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the contract for product stock service.
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

namespace Sitecore.Ecommerce.Services
{
  using System.ServiceModel;    
  using Sitecore.Ecommerce.DomainModel.Products;

  /// <summary>
  /// Defines the contract for product stock service.
  /// </summary>
  [ServiceContract]
  public interface IProductStockService
  {
    /// <summary>
    /// Gets the specified product stock.
    /// </summary>
    /// <param name="stockInfo">The stock info.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The product stock.</returns>
    [OperationContract]
    ProductStock Get(ProductStockInfo stockInfo, ServiceClientArgs args);

    /// <summary>
    /// Updates the specified product stock.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="newAmount">The new amount.</param>
    /// <param name="args">The arguments.</param>
    [OperationContract]
    void Update(string code, long newAmount, ServiceClientArgs args);
  }
}