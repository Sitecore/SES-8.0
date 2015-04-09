// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OmActionsObjectDetailList.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OmActionsObjectDetailList type.
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
  using Speak.Web.UI.WebControls;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using OrderManagement.ContextStrategies;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the om object detail list class.
  /// </summary>
  public class OmActionsObjectDetailList : ObjectDetailList
  {
    /// <summary>
    /// Action context.
    /// </summary>
    private readonly ActionContext actionContext = new ActionContext();

    /// <summary>
    /// The ubl entity resolving strategy.
    /// </summary>
    private readonly UblEntityResolvingStrategy ublEntityResolvingStrategy = new OnDefaultPageStrategy();

    /// <summary>
    /// Visibility of the details list.
    /// </summary>
    private bool? visible;

    /// <summary>
    /// Gets a value indicating whether [actions visibility].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [actions visibility]; otherwise, <c>false</c>.
    /// </value>
    protected virtual bool ActionsVisibility
    {
      get
      {
        if (this.visible == null)
        {
          Assert.IsNotNull(this.ublEntityResolvingStrategy, "UblEntityResolvingStrategy cannot be null.");
          Assert.IsNotNull(this.Page, "Page cannot be null.");
          this.actionContext.Owner = this.Page;
          Order order = this.ublEntityResolvingStrategy.GetEntity(this.actionContext) as Order;
          if (order != null)
          {
            this.visible = this.ublEntityResolvingStrategy.GetSecurityChecker()(order);
          }
        }

        return this.visible.GetValueOrDefault(true);
      }
    }

    /// <summary>
    /// Reinitializes the visibility.
    /// </summary>
    public void ReinitializeVisibility()
    {
      this.visible = null;
      this.ConfigureVisibility();
    }

    /// <summary>
    /// Initializes the actions.
    /// </summary>
    protected override void InitializeActions()
    {
      this.ConfigureVisibility(); 
      base.InitializeActions();
    }

    /// <summary>
    /// Configures the visibility.
    /// </summary>
    private void ConfigureVisibility()
    {
      this.actions.Visible = this.ActionsVisibility;
    }
  }
}