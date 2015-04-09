// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrderLineAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the AddOrderLineAction type.
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
  using System.Collections.Specialized;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using OrderManagement.ContextStrategies;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the add order line action class.
  /// </summary>
  public class AddOrderLineAction : OrderLineAction
  {
    /// <summary>
    /// Executes the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public override void Execute([NotNull] ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      this.UblEntityResolver = new OnDefaultPageStrategy();
      Order order = this.UblEntityResolver.GetEntity(context) as Order;
      Assert.IsNotNull(order, "Order cannot be resolved");

      NameValueCollection parameters = new NameValueCollection
      {
        { "ID", order.OrderId },
        { "Alias", "-1" },
        { "orderlineaction", OrderLineActions.Add.ToString() }
      };

      this.ShowPopup(context, parameters);
    }
  }
}