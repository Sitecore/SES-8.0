// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrderStateListView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the IOrderStateListView type.
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
  using System.Collections.Generic;
  using Ecommerce.OrderManagement.Orders;
  using Models;

  /// <summary>
  /// Represents interface for OrderStateList view.
  /// </summary>
  public interface IOrderStateListView
  {
    /// <summary>
    /// Occurs when [order state list view substate created].
    /// </summary>
    event EventHandler<OrderStateListViewSubstateCreatedEventArgs> OrderStateListViewSubstateCreated;

    /// <summary>
    /// Sets up controls.
    /// </summary>
    /// <param name="stateList">The state list.</param>
    /// <param name="currentState">State of the current.</param>
    void SetUpControls([NotNull] IEnumerable<StateModel> stateList, [NotNull] State currentState);
  }
}
