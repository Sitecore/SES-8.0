// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingOrderChecker.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the MissingOrderChecker type.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls
{
  using System;
  using System.IO;
  using System.Web;
  using System.Web.UI.WebControls;
  using Merchant.OrderManagement;

  /// <summary>
  /// Defines the MissingOrderChecker type.
  /// </summary>
  public class MissingOrderChecker : WebControl
  {
    /// <summary>
    /// The order manager.
    /// </summary>
    private readonly MerchantOrderManager orderManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="MissingOrderChecker" /> class.
    /// </summary>
    public MissingOrderChecker()
    {
      this.orderManager = Ecommerce.Context.Entity.Resolve<MerchantOrderManager>();

      if (System.Web.HttpContext.Current != null)
      {
        this.HttpContext = new HttpContextWrapper(System.Web.HttpContext.Current);
      }
    }

    /// <summary>
    /// Gets or sets the HTTP context.
    /// </summary>
    /// <value>The HTTP context.</value>
    public HttpContextBase HttpContext { get; set; }

    /// <summary>
    /// Checks the order.
    /// </summary>
    public virtual void CheckOrder()
    {
      const string Key = "OrderId";
      const string ProjectVirtualFolder = "ordermanager";

      var orderId = this.HttpContext.Request.QueryString[Key];
      var order = this.orderManager.GetOrder(orderId);

      var path = Path.Combine(Sitecore.Context.Site.VirtualFolder, ProjectVirtualFolder);

      if (order == null)
      {
        this.HttpContext.Response.Redirect(path);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      this.CheckOrder();
    }
  }
}