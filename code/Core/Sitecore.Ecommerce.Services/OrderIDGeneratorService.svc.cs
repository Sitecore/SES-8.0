// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderIDGeneratorService.svc.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order ID generator service class.
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
  using System.ServiceModel.Activation;
  using OrderManagement;
  using Sitecore.Ecommerce.DomainModel.Orders;
  using Sites;

  /// <summary>
  /// Defines the order ID generator service class.
  /// </summary>
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  public class OrderIDGeneratorService : IOrderIDGeneratorService
  {
    /// <summary>
    /// Generates the specified args.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The string.</returns>
    public string Generate(ServiceClientArgs args)
    {
      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        OrderIDGenerator orderIdGenerator = Context.Entity.Resolve<OrderIDGenerator>();
        return orderIdGenerator.Generate();
      }
    }
  }
}
