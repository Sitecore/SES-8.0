// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrderRepositoryService.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the IOrderRepositoryService type.
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
  using OrderManagement.Orders;

  /// <summary>
  ///   Defines the IOrderRepositoryService type.
  /// </summary>
  [ServiceContract]
  public interface IOrderRepositoryService
  {
    /// <summary>
    /// Creates the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="args">The arguments.</param>
    [OperationContract]
    void Create(string order, ServiceClientArgs args);

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <param name="filterExpression">The filter expression.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>
    /// The all.
    /// </returns>
    [OperationContract]
    string GetAll(string filterExpression, ServiceClientArgs args);
  }
}
