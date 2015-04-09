// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmartPanelEntityResolvingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the smart panel entity resolving strategy class.
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
  using Speak.WebSite;
  using Diagnostics;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the smart panel entity resolving strategy class.
  /// </summary>
  public abstract class SmartPanelEntityResolvingStrategy : UblEntityResolvingStrategy
  {
    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="key">The key.</param>
    /// <returns>
    /// The entity.
    /// </returns>
    [CanBeNull]
    protected virtual string GetKeyValue([NotNull] object context, [NotNull] string key)
    {
      Assert.ArgumentNotNull(context, "context");
      Assert.ArgumentNotNull(key, "key");

      ActionContext actionContext = context as ActionContext;
      Assert.IsNotNull(actionContext, "context should be of ActionContext type.");

      Assert.IsNotNull(actionContext.Owner, "context.Owner cannot be null.");
      Assert.IsNotNull(actionContext.Owner.Page, "context.Owner.Page cannot be null.");

      SmartPanel smartPanel = actionContext.Owner.Page as SmartPanel;
      Assert.IsNotNull(smartPanel, "SmartPanel cannot be null.");

      return smartPanel.Parameters[key];
    }
  }
}
