// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderProcessorService.svc.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderProcessorService type.
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
  using System.Linq;
  using System.ServiceModel;
  using System.ServiceModel.Activation;
  using Diagnostics;
  using OrderManagement;
  using OrderManagement.Orders;
  using Sites;

  /// <summary>
  /// Defines the order processor service class.
  /// </summary>
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  public class OrderProcessorService : IOrderProcessorService
  {
    /// <summary>
    /// Cancels the order.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="args">The arguments.</param>
    public void CancelOrder(string orderNumber, ServiceClientArgs args)
    {
       SiteContext site = Utils.GetExistingSiteContext(args);
       using (new SiteContextSwitcher(site))
       {
         VisitorOrderManager orderManager = Context.Entity.Resolve<VisitorOrderManager>();
         VisitorOrderProcessorBase visitorOrderProcessor = Context.Entity.Resolve<VisitorOrderProcessorBase>();

         Utils.SetCustomerId(args, orderManager);

         Order order = orderManager.GetAll().FirstOrDefault(o => o.OrderId == orderNumber);
         Assert.IsNotNull(order, "order cannot be null.");
         visitorOrderProcessor.CancelOrder(order);
       }
    }
  }
}
