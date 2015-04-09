// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the EditAction type.
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
  using Speak.Extensions;

  /// <summary>
  /// Defines the edit action class.
  /// </summary>
  public class EditAction : DefaultAction
  {
    /// <summary>
    /// Executes the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    protected override void Execute([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      System.Web.HttpContext.Current.Response.Redirect(string.Format("{0}ordermanager/order?orderid={1}", Extensions.GetVirtualFolder(), order.OrderId));
    }
  }
}