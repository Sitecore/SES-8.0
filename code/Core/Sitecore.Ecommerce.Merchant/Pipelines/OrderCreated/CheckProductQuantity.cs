// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckProductQuantity.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the CheckProductQuantity type.
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
  using Sitecore.Ecommerce.OrderManagement;
  using Sitecore.Pipelines;

  /// <summary>
  /// The product quantity validator.
  /// </summary>
  public class CheckProductQuantity : CheckOrderProcessorBase
  {
    /// <summary>
    /// Gets or sets the suspicious quantity.
    /// </summary>
    /// <value>
    /// The suspicious quantity.
    /// </value>
    public decimal MaximumQuantity { get; set; }

    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void Process([NotNull] PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      var order = this.GetOrder(args);

      foreach (var orderLine in order.OrderLines.Where(orderLine => orderLine.LineItem.Quantity > this.MaximumQuantity))
      {
        this.MarkOrderAsSuspicious(args, OrderStateCode.SuspiciousProductQuantity);
      }
    }
  }
}