// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyCustomer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the notify customer class.
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

namespace Sitecore.Ecommerce.Visitor.Pipelines.OrderCreated
{
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Sitecore.Pipelines;

  /// <summary>
  /// Defines the notify customer class.
  /// </summary>
  public class NotifyCustomer
  {
    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void Process([NotNull] PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      object orderNumber = args.CustomData["orderNumber"];
      Assert.IsNotNull(orderNumber, "OrderNumber cannot be null.");

      VisitorOrderManager orderRepository = Context.Entity.Resolve<VisitorOrderManager>();
      Assert.IsNotNull(orderRepository, "OrderManager cannot be null.");

      string orderId = orderNumber.ToString();
      Order order = orderRepository.GetAll().SingleOrDefault(o => (o != null) && (o.OrderId == orderId));
      Assert.IsNotNull(order, "Order cannot be null.");

      OrderConfirmation orderConfirmation = Context.Entity.Resolve<OrderConfirmation>();
      Assert.IsNotNull(orderConfirmation, "OrderConfirmation cannot be null.");
      Assert.IsNotNull(orderConfirmation.ConfirmationMessageBuilder, "OrderConfirmation.ConfirmationMessageBuilder cannot be null.");

      orderConfirmation.ConfirmationMessageBuilder.Order = order;

      orderConfirmation.Send();
    }
  }
}