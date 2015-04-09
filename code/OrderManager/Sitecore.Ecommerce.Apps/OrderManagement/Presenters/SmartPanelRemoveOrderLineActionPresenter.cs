// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmartPanelRemoveOrderLineActionPresenter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the smart panel remove order line action presenter class.
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
  using ContextStrategies;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Views;

  /// <summary>
  /// Defines the smart panel remove order line action presenter class.
  /// </summary>
  public class SmartPanelRemoveOrderLineActionPresenter : RemoveOrderLineActionPresenter
  {
    /// <summary>
    /// The resolving strategy
    /// </summary>
    private UblEntityResolvingStrategy resolvingStrategy;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmartPanelRemoveOrderLineActionPresenter"/> class.
    /// </summary>
    /// <param name="view">The view.</param>
    public SmartPanelRemoveOrderLineActionPresenter([NotNull] IRemoveOrderLineActionView view)
      : base(view)
    {
      Assert.ArgumentNotNull(view, "view");
    }

    /// <summary>
    /// Gets or sets the resolving strategy.
    /// </summary>
    /// <value>
    /// The resolving strategy.
    /// </value>
    public override UblEntityResolvingStrategy ResolvingStrategy
    {
      get
      {
        return this.resolvingStrategy ?? (this.resolvingStrategy = new OnSmartPanelOrderLineStrategy());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.resolvingStrategy = value;
      }
    }

    /// <summary>
    /// Gets the order.
    /// </summary>
    /// <returns>The order.</returns>
    protected override Order GetOrder()
    {
      var orderLine = this.GetOrderLine();

      return orderLine.Order;
    }

    /// <summary>
    /// Gets the order line.
    /// </summary>
    /// <returns>The order line.</returns>
    [NotNull]
    protected override OrderLine GetOrderLine()
    {
      var orderLine = this.ResolvingStrategy.GetEntity(this.ActionContext) as OrderLine;

      Assert.IsNotNull(orderLine, "Unable to initialize action. OrderLine cannot be null.");

      return orderLine;
    }
  }
}