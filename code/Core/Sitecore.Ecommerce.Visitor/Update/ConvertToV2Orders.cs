// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertToV2Orders.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the convert to v2 orders class.
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

namespace Sitecore.Ecommerce.Update
{
  using System;
  using System.Collections.Generic;
  using Data;
  using Diagnostics;
  using DomainModel.Orders;
  using Search;
  using Sitecore.Ecommerce.OrderManagement.OrderProcessing;
  using Sitecore.Pipelines;
  using Visitor.OrderManagement.Transient;

  /// <summary>
  /// Defines the convert to v2 orders class.
  /// </summary>
  public class ConvertToV2Orders
  {
    /// <summary>
    /// Gets or sets the shop context.
    /// </summary>
    /// <value>The shop context.</value>
    public ShopContext ShopContext { get; set; }

    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>The order manager.</value>
    public IOrderManager<Order> OrderManager { get; set; }

    /// <summary>
    /// Gets or sets the order repository.
    /// </summary>
    /// <value>The order repository.</value>
    public Repository<OrderManagement.Orders.Order> OrderRepository { get; set; }

    /// <summary>
    /// Gets or sets the transient order converter.
    /// </summary>
    /// <value>The transient order converter.</value>
    public TransientOrderConverter TransientOrderConverter { get; set; }

    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public void Process([NotNull] PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      Assert.IsNotNull(this.ShopContext, Texts.UnableToSaveTheOrderShopContextCannotBeNull);
      Assert.IsNotNull(this.OrderManager, Texts.UnableToSaveTheOrderOrderManagerCannotBeNull);
      Assert.IsNotNull(this.OrderRepository, Texts.UnableToSaveTheOrderOrderRepositoryCannotBeNull);
      Assert.IsNotNull(this.TransientOrderConverter, Texts.UnableToSaveTheOrderTransientOrderConverterCannotBeNull);

      Query searchQuery = new Query();
      searchQuery.AppendField("OrderNumber", string.Empty, MatchVariant.Like);

      IEnumerator<Order> orderEnumerator = this.OrderManager.GetOrders(searchQuery).GetEnumerator();
      ICollection<Exception> caughtExceptions = new LinkedList<Exception>();

      do
      {
        try
        {
          if (!orderEnumerator.MoveNext())
          {
            break;
          }

          Order originalOrder = orderEnumerator.Current;

          OrderManagement.Orders.Order convertedOrder = this.TransientOrderConverter.Convert(originalOrder);
          convertedOrder.ShopContext = this.ShopContext.InnerSite.Name;
          new ProcessingOrder(convertedOrder).ApplyCalculations();

          this.OrderRepository.Save(new[] { convertedOrder });
        }
        catch (Exception e)
        {
          caughtExceptions.Add(e);
        }
      } 
      while (true);

      if (caughtExceptions.Count > 0)
      {
        throw new AggregateException(caughtExceptions);
      }
    }
  }
}
