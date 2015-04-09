// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveOrderLineActionPresenter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the delete order line column presenter class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Presenters
{
  using System.Collections.Generic;
  using System.Linq;
  using ContextStrategies;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;
  using Sitecore.Web.UI.WebControls;
  using Views;

  /// <summary>
  /// Defines the delete order line column presenter class.
  /// </summary>
  public class RemoveOrderLineActionPresenter
  {
    /// <summary>
    /// The view.
    /// </summary>
    private readonly IRemoveOrderLineActionView view;

    /// <summary>
    /// The resolving strategy
    /// </summary>
    private UblEntityResolvingStrategy resolvingStrategy;

    /// <summary>
    /// The order processing strategy.
    /// </summary>
    private OrderProcessingStrategy processingStrategy;

    /// <summary>
    /// The merchant order processor.
    /// </summary>
    private MerchantOrderProcessor orderProcessor;

    /// <summary>
    /// ActionContext instance.
    /// </summary>
    private ActionContext actionContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveOrderLineActionPresenter"/> class.
    /// </summary>
    /// <param name="view">The view.</param>
    public RemoveOrderLineActionPresenter([NotNull]IRemoveOrderLineActionView view)
    {
      Assert.ArgumentNotNull(view, "view");

      this.view = view;
    }

    /// <summary>
    /// Gets or sets the resolving strategy.
    /// </summary>
    /// <value>
    /// The resolving strategy.
    /// </value>
    [NotNull]
    public virtual UblEntityResolvingStrategy ResolvingStrategy
    {
      get
      {
        return this.resolvingStrategy ?? (this.resolvingStrategy = new OnDefaultPageStrategy());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.resolvingStrategy = value;
      }
    }

    /// <summary>
    /// Gets or sets the order line procfessing strategy.
    /// </summary>
    /// <value>
    /// The order line processing strategy.
    /// </value>
    [NotNull]
    public OrderProcessingStrategy ProcessingStrategy
    {
      get
      {
        return this.processingStrategy ?? (this.processingStrategy = Context.Entity.Resolve<OrderProcessingStrategy>("RemoveOrderLine"));
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.processingStrategy = value;
      }
    }

    /// <summary>
    /// Gets or sets the order processor.
    /// </summary>
    /// <value>
    /// The order processor.
    /// </value>
    [NotNull]
    public MerchantOrderProcessor OrderProcessor
    {
      get
      {
        return this.orderProcessor ?? (this.orderProcessor = Context.Entity.Resolve<MerchantOrderProcessor>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.orderProcessor = value;
      }
    }

    /// <summary>
    /// Gets or sets the context.
    /// </summary>
    /// <value>
    /// The context.
    /// </value>
    [NotNull]
    protected ActionContext ActionContext
    {
      get
      {
        Assert.IsNotNull(this.actionContext, "Action context cannot be null.");

        return this.actionContext;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.actionContext = value;
      }
    }

    /// <summary>
    /// Executs the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public void Execute([NotNull]ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      this.ActionContext = context;

      var orderLine = this.GetOrderLine();

      if (orderLine != null && this.view.RemovalConfirm(this.ActionContext))
      {
        this.OrderProcessor.OrderProcessingStrategy = this.ProcessingStrategy;

        IDictionary<string, object> parameters = new Dictionary<string, object> { { "orderline", orderLine } };
        string result = this.OrderProcessor.ProcessOrder(orderLine.Order, parameters);

        if (result == OrderProcessingStrategy.SuccessfulResult)
        {
          this.view.Refresh(this.ActionContext);
          this.view.ShowSuccessMessage(this.ActionContext);
        }
      }
    }

    /// <summary>
    /// Queries the state.
    /// </summary>
    /// <param name="context">The context.</param>
    public void QueryState([NotNull]ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      this.ActionContext = context;

      var order = this.GetOrder();

      Assert.IsNotNull(order, "Unable to initialize action. Order cannot be null.");
      Assert.IsNotNull(order.State, "Unable to initialize action. Order.State cannot be null.");

      if (order.OrderLines.Count == 1 || (order.State.Code == OrderStateCode.InProcess &&
        order.State.Substates.Any(s => (s.Code == OrderStateCode.InProcessShippedInFull || s.Code == OrderStateCode.InProcessCapturedInFull) && s.Active)))
      {
        this.view.IsActionDisabled = true;
      }
    }

    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <returns>The order.</returns>
    [NotNull]
    protected virtual Order GetOrder()
    {
      var order = this.ResolvingStrategy.GetEntity(this.ActionContext) as Order;

      Assert.IsNotNull(order, "Unable to initialize action. Order cannot be null.");

      return order;
    }

    /// <summary>
    /// Gets the order line.
    /// </summary>
    /// <returns>The order line.</returns>
    [CanBeNull]
    protected virtual OrderLine GetOrderLine()
    {
      var orderLineId = this.view.GetSelectedOrderLineId(this.ActionContext);
      if (string.IsNullOrEmpty(orderLineId))
      {
        this.view.ShowErrorMessage(this.ActionContext);
        return null;
      }

      var order = this.GetOrder();

      var alias = long.Parse(orderLineId);
      var orderLine = order.OrderLines.SingleOrDefault(ol => ol.Alias == alias);

      return orderLine;
    }
  }
}