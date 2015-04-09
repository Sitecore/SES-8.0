// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckOrderProcessorBase.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the CheckOrderProcessor type.
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

namespace Sitecore.Ecommerce.Merchant.Pipelines.OrderCreated
{
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Merchant.OrderManagement;
  using Sitecore.Ecommerce.OrderManagement;
  using Sitecore.Ecommerce.OrderManagement.Orders;
  using Sitecore.Pipelines;

  /// <summary>
  /// Defines the CheckOrderProcessor type.
  /// </summary>
  public abstract class CheckOrderProcessorBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckOrderProcessorBase" /> class.
    /// </summary>
    protected CheckOrderProcessorBase()
    {
      this.OrderManager = Context.Entity.Resolve<MerchantOrderManager>();
    }

    /// <summary>
    /// Gets the order manager.
    /// </summary>
    /// <value>The order manager.</value>
    [NotNull]
    public virtual MerchantOrderManager OrderManager { get; private set; }

    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <param name="args">The args.</param>
    /// <returns>The order.</returns>
    [NotNull]
    protected virtual Order GetOrder([NotNull] PipelineArgs args)
    {
      var orderNumber = args.CustomData["orderNumber"] as string;
      Assert.IsNotNull(orderNumber, "OrderNumber cannot be null.");

      var order = this.OrderManager.GetOrder(orderNumber);
      Assert.IsNotNull(order, "Order cannot be null.");

      return order;
    }

    /// <summary>
    /// Gets the suspicious sub states.
    /// </summary>
    /// <param name="args">The args.</param>
    /// <returns>The list of suspicious sub-states.</returns>
    [NotNull]
    protected IEnumerable<string> GetSuspiciousSubStates(PipelineArgs args)
    {
      return args.CustomData[OrderStateCode.Suspicious] as HashSet<string> ?? new HashSet<string>();
    }

    /// <summary>
    /// Marks the order as suspicious.
    /// </summary>
    /// <param name="args">The args.</param>
    /// <param name="suspiciousSubstateCode">The suspicious sub-state code.</param>
    protected virtual void MarkOrderAsSuspicious(PipelineArgs args, string suspiciousSubstateCode)
    {
      HashSet<string> hashSet = args.CustomData[OrderStateCode.Suspicious] as HashSet<string>;
      if (hashSet == null)
      {
        hashSet = new HashSet<string>();
        args.CustomData[OrderStateCode.Suspicious] = hashSet;
      }

      hashSet.Add(suspiciousSubstateCode);
    }

    /// <summary>
    /// Determines whether the specified args is suspicious.
    /// </summary>
    /// <param name="args">The args.</param>
    /// <returns>
    ///   <c>true</c> if the specified args is suspicious; otherwise, <c>false</c>.
    /// </returns>
    protected virtual bool IsSuspicious(PipelineArgs args)
    {
      HashSet<string> hashSet = args.CustomData[OrderStateCode.Suspicious] as HashSet<string>;

      return hashSet != null && hashSet.Any();
    }
  }
}