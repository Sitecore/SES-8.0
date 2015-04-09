// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the DefaultAction type.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls.Actions
{
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Logging;
  using OrderManagement.ContextStrategies;
  using Sitecore.Web.UI.WebControls;
  using Speak.WebSite;

  /// <summary>
  /// Defines the default action class.
  /// </summary>
  public abstract class DefaultAction : Action
  {
    /// <summary>
    /// Logger instance.
    /// </summary>
    private Logger logger;

    /// <summary>
    /// Gets or sets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    [NotNull]
    public Logger Logger
    {
      get
      {
        return this.logger ?? (this.logger = Context.Entity.Resolve<Logger>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.logger = value;
      }
    }

    /// <summary>
    /// Gets or sets the order ID resolver.
    /// </summary>
    /// <value>
    /// The order ID resolver.
    /// </value>
    public UblEntityResolvingStrategy UblEntityResolver { get; set; }

    /// <summary>
    /// Executes the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public override void Execute([NotNull] ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      Order order = this.GetOrder(context);
      Assert.IsNotNull(order, "Order cannot be null or empty.");

      this.Execute(order);

      this.PerformPostSteps();
    }

    /// <summary>
    /// Gets the order id.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The order id.
    /// </returns>
    [CanBeNull]
    protected virtual Order GetOrder([NotNull] ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      if (this.UblEntityResolver == null)
      {
        Assert.IsNotNull(context.Owner, "context.Owner cannot be null.");
        Assert.IsNotNull(context.Owner.Page, "context.Owner.Page cannot be null.");
        if (context.Owner.Page is SmartPanel)
        {
          this.UblEntityResolver = new OnSmartPanelStrategy();
        }
        else if (context.Owner.Page is Default)
        {
          this.UblEntityResolver = new OnDefaultPageStrategy();
        }

        else { }
      }

      Assert.IsNotNull(this.UblEntityResolver, "UblEntityResolver cannot be null.");
      return this.UblEntityResolver.GetEntity(context) as Order;
    }

    /// <summary>
    /// Executes the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    protected abstract void Execute(Order order);

    /// <summary>
    /// Performs the post steps.
    /// </summary>
    protected virtual void PerformPostSteps()
    {
    }
  }
}