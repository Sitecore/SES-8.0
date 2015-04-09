// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrderService.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The order service contract.
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
  using System.Data;
  using System.ServiceModel;
  using Search;

  /// <summary>
  /// The order service contract.
  /// </summary>
  [ServiceContract(Namespace = "http://www.sitecore.net/")]
  public interface IOrderService
  {
    /// <summary>
    /// Generates the order number.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The order number.</returns>
    [OperationContract]
    string GenerateOrderNumber(ServiceClientArgs args);

    /// <summary>
    /// Creates the order.
    /// </summary>
    /// <param name="shoppingCartData">The shopping cart data.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The order.</returns>
    [OperationContract]
    DataTable Create(DataTable shoppingCartData, ServiceClientArgs args);

    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The string.</returns>
    [OperationContract]
    DataTable Get(string orderNumber, ServiceClientArgs args);

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The orders.</returns>
    [OperationContract]
    DataTable GetByQuery(Query query, ServiceClientArgs args);

    /// <summary>
    /// Gets the by query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The orders.</returns>
    [OperationContract]
    DataTable GetRangeByQuery(Query query, int pageIndex, int pageSize, ServiceClientArgs args);


    /// <summary>
    /// Gets the orders count.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The orders count.</returns>
    [OperationContract]
    int GetCount(Query query, ServiceClientArgs args);

    /// <summary>
    /// Saves the specified order.
    /// </summary>
    /// <param name="orderData">The order data.</param>
    /// <param name="args">The arguments.</param>
    [OperationContract]
    void Save(DataTable orderData, ServiceClientArgs args);
  }
}