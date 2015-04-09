// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnDefaultPageStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OnDefaultPageStrategy type.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.ContextStrategies
{
  using System;
  using System.Web;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the on default page strategy class.
  /// </summary>
  public class OnDefaultPageStrategy : UblEntityResolvingStrategy
  {
    /// <summary>
    /// HttpResponseWrapper instance.
    /// </summary>
    private HttpResponseWrapper responseWrapper;

    /// <summary>
    /// Gets or sets the response wrapper.
    /// </summary>
    /// <value>
    /// The response wrapper.
    /// </value>
    [NotNull]
    public virtual HttpResponseWrapper ResponseWrapper
    {
      get
      {
        return this.responseWrapper ?? (this.responseWrapper = new HttpResponseWrapper(HttpContext.Current.Response));
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.responseWrapper = value;
      }
    }

    /// <summary>
    /// Gets the order id.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The order id.
    /// </returns>
    public override object GetEntity([NotNull] object context)
    {
      Assert.ArgumentNotNull(context, "context");

      ActionContext actionContext = context as ActionContext;
      Assert.IsNotNull(actionContext, "context should be of ActionContext type.");

      Assert.IsNotNull(actionContext.Owner, "context.Owner cannot be null.");
      Assert.IsNotNull(actionContext.Owner.Page, "context.Owner.Page cannot be null.");
      Assert.IsNotNull(actionContext.Owner.Page.Request, "context.Owner.Page.Request cannot be null.");

      string orderId = actionContext.Owner.Page.Request["orderid"];
      if (string.IsNullOrEmpty(orderId))
      {
        this.ResponseWrapper.Redirect(string.Format("{0}ordermanager/", Speak.Extensions.Extensions.GetVirtualFolder()));
        return new object();
      }

      return this.GetEntity(orderId);
    }

    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>
    /// The entity.
    /// </returns>
    [CanBeNull]
    public override object GetEntity([NotNull] string id)
    {
      Assert.ArgumentNotNull(id, "id");

      return this.OrderManager.GetOrder(id);
    }

    /// <summary>
    /// Gets the security checker.
    /// </summary>
    /// <returns>
    /// The security checker.
    /// </returns>
    public override Func<Order, bool> GetSecurityChecker()
    {
      return this.OrderSecurity.CanEditOrderLines;
    }
  }
}