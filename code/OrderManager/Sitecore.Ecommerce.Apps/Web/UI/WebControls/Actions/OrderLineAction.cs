// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineAction type.
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
  using System.Linq;
  using Diagnostics;
  using Merchant.OrderManagement;
  using OrderManagement.ContextStrategies;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.WebControls.Extensions;
  using Action = Sitecore.Web.UI.WebControls.Action;

  /// <summary>
  /// Defines the order line action class.
  /// </summary>
  public abstract class OrderLineAction : Action
  {
    /// <summary>
    /// The merchant order manager
    /// </summary>
    private MerchantOrderManager orderManager;

    /// <summary>
    /// Gets or sets the id resolver.
    /// </summary>
    /// <value>
    /// The id resolver.
    /// </value>
    public UblEntityResolvingStrategy UblEntityResolver { get; set; }

    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>
    /// The order manager.
    /// </value>
    [NotNull]
    public MerchantOrderManager OrderManager
    {
      get
      {
        return this.orderManager ?? (this.orderManager = Context.Entity.Resolve<MerchantOrderManager>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        this.orderManager = value;
      }
    }

    /// <summary>
    /// Initializes the smart panel.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="url">The URL.</param>
    protected virtual void ShowPopup([NotNull] ActionContext context, NameValueCollection parameters = null, string url = null)
    {
      Assert.ArgumentNotNull(context, "context");

      Actions owner = context.Owner as Actions;
      if (owner == null)
      {
        return;
      }

      if (context.Owner.Page == null)
      {
        return;
      }

      SmartPanelRenderer smartPanelRenderer = context.Owner.Page.Controls.Flatten<SmartPanelRenderer>().FirstOrDefault();

      if (smartPanelRenderer != null)
      {
        if (parameters != null)
        {
          smartPanelRenderer.SmartPanel.Parameters.Add(parameters);
        }

        smartPanelRenderer.SmartPanel.Show();
      }
    }
  }
}