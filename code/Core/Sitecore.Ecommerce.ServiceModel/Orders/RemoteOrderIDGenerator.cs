// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoteOrderIDGenerator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the remote order ID generator class.
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
  using Diagnostics;
  using OrderIDGeneratorService;
  using OrderManagement;
  using Sitecore.Ecommerce.DomainModel.Orders;

  /// <summary>
  /// Defines the remote order ID generator class.
  /// </summary>
  public class RemoteOrderIDGenerator : OrderIDGenerator
  {
    /// <summary>
    /// Defines ServiceClientArgs factory.
    /// </summary>
    private readonly ServiceClientArgsFactory serviceClientArgsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteOrderIDGenerator"/> class.
    /// </summary>
    /// <param name="serviceClientArgsFactory">The service client args factory.</param>
    public RemoteOrderIDGenerator(ServiceClientArgsFactory serviceClientArgsFactory)
    {
      this.serviceClientArgsFactory = serviceClientArgsFactory;
    }

    /// <summary>
    /// Generates this instance.
    /// </summary>
    /// <returns>The string.</returns>
    public override string Generate()
    {
      Assert.IsNotNull(this.serviceClientArgsFactory, "serviceClientArgsFactory must not be null.");

      using (OrderIDGeneratorServiceClient client = new OrderIDGeneratorServiceClient())
      {
        return client.Generate(this.serviceClientArgsFactory.GetServiceClientArgs());
      }
    }
  }
}