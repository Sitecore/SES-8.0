// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRemoveOrderLineActionView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the IDeleteOrderLineColumnView type.
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
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the IDeleteOrderLineColumnView type.
  /// </summary>
  public interface IRemoveOrderLineActionView
  {
    /// <summary>
    /// Gets or sets a value indicating whether action is disabled.
    /// </summary>
    /// <value><c>true</c> if action is disabled; otherwise, <c>false</c>.</value>
    bool IsActionDisabled { get; set; }

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    /// <param name="context">The context.</param>
    void Refresh(ActionContext context);

    /// <summary>
    /// Gets or sets a value indicating whether confirm removal.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The confirmation.
    /// </returns>
    /// <value><c>true</c> if confirm removal; otherwise, <c>false</c>.</value>
    bool RemovalConfirm([NotNull]ActionContext context);

    /// <summary>
    /// Shows the success message.
    /// </summary>
    /// <param name="context">The context.</param>
    void ShowSuccessMessage([NotNull]ActionContext context);

    /// <summary>
    /// Gets the selected order line id.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The selected order line id.
    /// </returns>
    [CanBeNull]
    string GetSelectedOrderLineId([NotNull]ActionContext context);

    /// <summary>
    /// Shows the error message.
    /// </summary>
    /// <param name="context">The context.</param>
    void ShowErrorMessage([NotNull]ActionContext context);
  }
}