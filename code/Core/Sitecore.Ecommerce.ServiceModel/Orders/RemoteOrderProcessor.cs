// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoteOrderProcessor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the remote order processor class.
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

namespace Sitecore.Ecommerce.ServiceModel.Orders
{
  using OrderManagement;
  using OrderManagement.Orders;
  using OrderProcessorService;
  using Services;

  /// <summary>
  /// Defines the remote order processor class.
  /// </summary>
  public class RemoteOrderProcessor : VisitorOrderProcessorBase
  {
    /// <summary>
    /// Defines ServiceClientArgs factory.
    /// </summary>
    private readonly ServiceClientArgsFactory serviceClientArgsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteOrderProcessor" /> class.
    /// </summary>
    /// <param name="serviceClientArgsFactory">The service client args factory.</param>
    public RemoteOrderProcessor(ServiceClientArgsFactory serviceClientArgsFactory)
    {
      this.serviceClientArgsFactory = serviceClientArgsFactory;
    }
        
    /// <summary>
    /// Cancels the order.
    /// </summary>
    /// <param name="order">The order.</param>
    public override void CancelOrder(Order order)
    {
      ServiceClientArgs args = this.serviceClientArgsFactory.GetServiceClientArgs();
      using (OrderProcessorServiceClient client = new OrderProcessorServiceClient())
      {
        client.CancelOrder(order.OrderId, args);
      }
    }
  }
}