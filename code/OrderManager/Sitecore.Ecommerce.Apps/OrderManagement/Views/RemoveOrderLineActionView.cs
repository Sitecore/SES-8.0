// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveOrderLineActionView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the DeleteOrderLineColumnView type.
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
  using System.Linq;
  using System.Web.UI;
  using Diagnostics;
  using Presenters;
  using Sitecore.Web.UI;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.WebControls.Extensions;
  using Web.UI.WebControls;

  /// <summary>
  /// Defines the delete order line column view class.
  /// </summary>
  public class RemoveOrderLineActionView : Action, IRemoveOrderLineActionView
  {
    /// <summary>
    /// The presenter.
    /// </summary>
    private readonly RemoveOrderLineActionPresenter presenter;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveOrderLineActionView"/> class.
    /// </summary>
    public RemoveOrderLineActionView()
    {
      this.presenter = new RemoveOrderLineActionPresenter(this);
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

      OrderTaskFlowButtonsView orderTaskFlowButtonsView = context.Owner.Page.Controls.Flatten<OrderTaskFlowButtonsView>().SingleOrDefault();

      if (orderTaskFlowButtonsView != null)
      {
        orderTaskFlowButtonsView.Refresh();
      }
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

      return dialogResult == DialogResult.Yes;
    }

    /// <summary>
    /// Shows the success message.
    /// </summary>
    /// <param name="context">The context.</param>
    public void ShowSuccessMessage(ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      ScriptManager scriptManager = ScriptManager.GetCurrent(context.Owner.Page);
      scriptManager.Message(new Message(Texts.TheOrderLineHasBeenRemovedFromTheOrder) { Sticky = false, Type = MessageType.Info });
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
    /// Gets the selected order line id.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The selected order line id.
    /// </returns>
    public string GetSelectedOrderLineId(ActionContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      var id = string.Empty;

      OmActionsObjectDetailList speakDetailList = context.Owner.Parent.GetParentOfType(typeof(OmActionsObjectDetailList)) as OmActionsObjectDetailList;

      if (speakDetailList != null && !speakDetailList.List.Multiselect)
      {
        id = speakDetailList.List.SelectedRows.FirstOrDefault();
      }

      return id;
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