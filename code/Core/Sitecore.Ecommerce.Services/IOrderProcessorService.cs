// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrderProcessorService.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the IOrderProcessorService type.
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
  ///  Defines the IOrderProcessorService type.
  /// </summary>
  [ServiceContract]
  public interface IOrderProcessorService
  {
    /// <summary>
    /// Cancels the order.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="args">The arguments.</param>
    [OperationContract]
    void CancelOrder([NotNull] string orderNumber, [NotNull] ServiceClientArgs args);
  }
}
