// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceClientArgsFactory.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The service client args factory.
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

namespace Sitecore.Ecommerce.ServiceModel
{
  using DomainModel.Users;
  using Services;

  /// <summary>
  /// Defines the service client args factory class.
  /// </summary>
  public class ServiceClientArgsFactory
  {
    /// <summary>
    /// Defines customer manager.
    /// </summary>
    private readonly ICustomerManager<CustomerInfo> customerManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceClientArgsFactory" /> class.
    /// </summary>
    /// <param name="customerManager">The customer manager.</param>
    public ServiceClientArgsFactory(ICustomerManager<CustomerInfo> customerManager)
    {
      this.customerManager = customerManager;
    }

    /// <summary>
    /// Gets the service client args.
    /// </summary>
    /// <returns>
    /// The service client args.
    /// </returns>
    public virtual ServiceClientArgs GetServiceClientArgs()
    {
      return new ServiceClientArgs
      {
        SiteName = Sitecore.Context.GetSiteName(),
        CustomerId = this.customerManager.CurrentUser.CustomerId
      };
    }
  }
}
