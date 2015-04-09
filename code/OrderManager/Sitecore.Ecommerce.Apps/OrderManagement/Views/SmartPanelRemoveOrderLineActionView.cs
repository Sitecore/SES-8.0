// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmartPanelRemoveOrderLineActionView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the smart panel remove order line action view class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Views
{
  using System.Web.UI;
  using Diagnostics;
  using Presenters;
  using Sitecore.Web.UI;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the smart panel remove order line action view class.
  /// </summary>
  public class SmartPanelRemoveOrderLineActionView : Action, IRemoveOrderLineActionView
  {
    /// <summary>
    /// The presenter.
    /// </summary>
    private readonly SmartPanelRemoveOrderLineActionPresenter presenter;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmartPanelRemoveOrderLineActionView"/> class.
    /// </summary>
    public SmartPanelRemoveOrderLineActionView()
    {
      this.presenter = new SmartPanelRemoveOrderLineActionPresenter(this);
    }

    /// <summary>
    /// Gets or sets a value indicating whether action is disabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if action is disabled; otherwise, <c>false</c>.
    /// </value>
    public bool IsActionDisabled { get; set; }

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    /// <param name="context">The context.</param>
    public void Refresh(ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");
    }

    /// <summary>
    /// Gets or sets a value indicating whether confirm removal.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The confirmation.
    /// </returns>
    /// <value><c>true</c> if confirm removal; otherwise, <c>false</c>.</value>
    public bool RemovalConfirm(ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      ScriptManager scriptManager = ScriptManager.GetCurrent(context.Owner.Page);
      DialogResult dialogResult = scriptManager.Confirm(Texts.TheSelectedOrderLineWillBeDeletedIfYouProceed, context.Owner.Page);

      bool result = dialogResult == DialogResult.Yes;
      if (result && context.Owner.Page is PopupPage)
      {
        ((PopupPage)context.Owner.Page).Close();
      }

      return result;
    }

    /// <summary>
    /// Shows the success message.
    /// </summary>
    /// <param name="context">The context.</param>
    public void ShowSuccessMessage(ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      ScriptManager scriptManager = ScriptManager.GetCurrent(context.Owner.Page);
      scriptManager.Message(
        new Message(Texts.TheOrderLineHasBeenRemovedFromTheOrder) { Sticky = false, Type = MessageType.Info });
    }

    /// <summary>
    /// Gets the selected order line id.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The selected order line id.
    /// </returns>
    /// <exception cref="System.NotImplementedException">Not implemented.</exception>
    [NotNull]
    public string GetSelectedOrderLineId(ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Shows the error message.
    /// </summary>
    /// <param name="context">The context.</param>
    public void ShowErrorMessage(ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      ScriptManager scriptManager = ScriptManager.GetCurrent(context.Owner.Page);
      scriptManager.Message(new Message(Texts.NoOrderLineWasSelected) { Sticky = false, Type = MessageType.Error });
    }

    /// <summary>
    /// Executes the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public override void Execute([NotNull] ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      this.presenter.Execute(context);
    }

    /// <summary>
    /// Queries the state.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The state.
    /// </returns>
    public override ElementState QueryState([NotNull] ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      this.presenter.QueryState(context);

      return this.IsActionDisabled ? ElementState.Disabled : ElementState.Enabled;
    }
  }
}
