// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisitorOrderProcessor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the visitor order processor class.
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

namespace Sitecore.Ecommerce.Visitor.OrderManagement
{
  using System.Collections.ObjectModel;
  using Data;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Logging;

  /// <summary>
  /// Defines the visitor order processor class.
  /// </summary>
  public class VisitorOrderProcessor : VisitorOrderProcessorBase
  {
    /// <summary>
    /// The inner processor.
    /// </summary>
    private readonly OrderProcessor innerProcessor;

    /// <summary>
    /// The processing strategy.
    /// </summary>
    private readonly ProcessingStrategy processingStrategy;

    /// <summary>
    /// The repository.
    /// </summary>
    private readonly Repository<Order> repository;

    /// <summary>
    /// The order security.
    /// </summary>
    private readonly VisitorOrderSecurity orderSecurity;

    /// <summary>
    /// Initializes a new instance of the <see cref="VisitorOrderProcessor" /> class.
    /// </summary>
    /// <param name="innerProcessor">The inner processor.</param>
    /// <param name="processingStrategy">The processing strategy.</param>
    /// <param name="repository">The repository.</param>
    /// <param name="orderSecurity">The order security.</param>
    public VisitorOrderProcessor(OrderProcessor innerProcessor, ProcessingStrategy processingStrategy, Repository<Order> repository, VisitorOrderSecurity orderSecurity)
    {
      Assert.IsNotNull(innerProcessor, "Unable to cancel the order. Inner Order Processor cannot be null.");
      Assert.IsNotNull(processingStrategy, "Unable to cancel the order. Order Processing Strategy cannot be null.");
      Assert.IsNotNull(repository, "Unable to cancel the order. Order Repository cannot be null.");
      Assert.IsNotNull(orderSecurity, Texts.UnableToSaveTheOrdersOrderSecurityCannotBeNull);

      this.innerProcessor = innerProcessor;
      this.processingStrategy = processingStrategy;
      this.repository = repository;
      this.orderSecurity = orderSecurity;
    }

    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="order">The order.</param>
    [LogThis("Cancel order", Constants.UserLevel)]
    public override void CancelOrder(Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      Assert.IsTrue(this.orderSecurity.CanCancel(order), Texts.YouDoNotHaveTheNecessaryPermissionsToCancelTheOrder);

      this.innerProcessor.Order = order;
      this.innerProcessor.Process(this.processingStrategy);

      this.repository.Save(new Collection<Order> { order });
    }
  }
}