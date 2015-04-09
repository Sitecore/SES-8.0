// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptManagedAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ScriptManagedAction type.
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
  using System.Web.UI;
  using Diagnostics;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the script managed action class.
  /// </summary>
  public abstract class ScriptManagedAction : DefaultAction
  {
    /// <summary>
    /// Current page.
    /// </summary>
    private Page page;

    /// <summary>
    /// Script manager.
    /// </summary>
    private ScriptManager scriptManager;

    /// <summary>
    /// Gets or sets the script manager.
    /// </summary>
    /// <value>
    /// The script manager.
    /// </value>
    [NotNull]
    public ScriptManager ScriptManager
    {
      get
      {
        if (this.scriptManager == null)
        {
          this.scriptManager = ScriptManager.GetCurrent(this.page);
        }

        Assert.IsNotNull(this.scriptManager, "ScriptManager cannot be null.");
        return this.scriptManager;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.scriptManager = value;
      }
    }

    /// <summary>
    /// Gets or sets the order id.
    /// </summary>
    /// <value>
    /// The order id.
    /// </value>
    protected string OrderId { get; set; }

    /// <summary>
    /// Executes the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public override void Execute([NotNull] ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      this.page = context.Owner.Page;

      base.Execute(context);
    }
  }
}