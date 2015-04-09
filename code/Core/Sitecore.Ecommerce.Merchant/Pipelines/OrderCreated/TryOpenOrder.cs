// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryOpenOrder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the TryOpenOrder type.
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
  using System.Linq;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Merchant.OrderManagement;
  using Sitecore.Ecommerce.OrderManagement;
  using Sitecore.Pipelines;

  /// <summary>
  /// The order validator.
  /// </summary>
  public class TryOpenOrder : CheckOrderProcessorBase
  {
    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>
    /// The order manager.
    /// </value>
    private readonly MerchantOrderManager orderManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="TryOpenOrder" /> class.
    /// </summary>
    public TryOpenOrder()
    {
      this.orderManager = Context.Entity.Resolve<MerchantOrderManager>();
    }

    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void Process([NotNull] PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      var order = this.GetOrder(args);
      var states = this.orderManager.StateConfiguration.GetStates();

      if (!this.IsSuspicious(args))
      {
        order.State = states.Single(s => s.Code == OrderStateCode.Open);
      }
      else
      {
        var suspicionState = states.Single(s => s.Code == OrderStateCode.Suspicious);
        foreach (var suspicionSubStateCode in this.GetSuspiciousSubStates(args))
        {
          suspicionState.Substates.Single(s => s.Code == suspicionSubStateCode).Active = true;
        }

        order.State = suspicionState;
      }

      this.orderManager.Save(order);
    }
  }
}