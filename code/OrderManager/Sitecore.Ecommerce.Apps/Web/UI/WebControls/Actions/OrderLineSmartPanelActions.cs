// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineSmartPanelActions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineSmartPanelActions type.
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
  using OrderManagement.ContextStrategies;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines OrderLineSmartPanelActions class.
  /// </summary>
  public class OrderLineSmartPanelActions : Speak.Web.UI.WebControls.Actions
  {
    /// <summary>
    /// Action context instance.
    /// </summary>
    private readonly ActionContext actionContext = new ActionContext();

    /// <summary>
    /// Inner UblEntityResolvingStrategy.
    /// </summary>
    private readonly UblEntityResolvingStrategy ublEntityResolvingStrategy = new OnSmartPanelOrderLineStrategy();

    /// <summary>
    /// Stores the information about visibility of the panel.
    /// </summary>
    private bool? visible;

    /// <summary>
    /// Gets or sets a value that indicates whether a server control is rendered as UI on the page.
    /// </summary>
    /// <returns>true if the control is visible on the page; otherwise false.</returns>
    public override bool Visible
    {
      get
      {
        if (this.visible == null)
        {
          Assert.IsNotNull(this.ublEntityResolvingStrategy, "UblEntityResolvingStrategy cannot be null.");
          Assert.IsNotNull(this.Page, "Page cannot be null.");
          this.actionContext.Owner = this.Page;
          OrderLine orderLine = this.ublEntityResolvingStrategy.GetEntity(this.actionContext) as OrderLine;
          if (orderLine != null)
          {
            this.visible = this.ublEntityResolvingStrategy.GetSecurityChecker()(orderLine.Order);
          }
        }

        return this.visible.GetValueOrDefault(true);
      }

      set
      {
        base.Visible = value;
      }
    }
  }
}