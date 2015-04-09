﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductPriceService.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the contract for product price service.
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

  /// <summary>
  /// Defines the contract for product price service.
  /// </summary>
  [ServiceContract]
  public interface IProductPriceService
  {
    /// <summary>
    /// Gets the product price matrix.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="args">The argumets.</param>
    /// <returns>The product price matrix.</returns>
    [OperationContract]
    string GetPriceMatrix(string code, ServiceClientArgs args);
  }
}