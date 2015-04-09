// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrderTaskFlowButtonsView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the IOrderTaskFlowButtonsView type.
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
  using System;
  using Ecommerce.OrderManagement.Orders;
  using Mixins;

  /// <summary>
  /// Represents inteface for all OrderTaskFlowButtonaViews.
  /// </summary>
  public interface IOrderTaskFlowButtonsView : IChangesObservable<Order>
  {
    /// <summary>
    /// Occurs when smart panel is closed.
    /// </summary>
    event EventHandler SmartPanelClosed;

    /// <summary>
    /// Gets or sets a value indicating whether save button must be shown.
    /// </summary>
    /// <value><c>true</c> if save button is shown; otherwise, <c>false</c>.</value>
    bool ShowSaveButton { get; set; }

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    void Refresh();

    /// <summary>
    /// Shows the non sticky info message.
    /// </summary>
    /// <param name="message">The message.</param>
    void ShowNonStickyInformationalMessage(string message);

    /// <summary>
    /// Shows the non sticky error message.
    /// </summary>
    /// <param name="message">The message.</param>
    void ShowNonStickyErrorMessage(string message);

    /// <summary>
    /// Shows the confirmation dialog.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="description">The description.</param>
    /// <returns>
    /// The confirmation dialog.
    /// </returns>
    bool ShowConfirmationDialog(string message, string description);
  }
}
