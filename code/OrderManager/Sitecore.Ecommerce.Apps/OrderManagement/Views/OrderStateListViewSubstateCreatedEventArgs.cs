// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStateListViewSubstateCreatedEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderStateListViewSubstateCreatedEventArgs type.
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
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the order state list view substate created event args class.
  /// </summary>
  public class OrderStateListViewSubstateCreatedEventArgs : EventArgs
  {
    /// <summary>
    /// Stores reference to the state;
    /// </summary>
    private readonly State state;

    /// <summary>
    /// Stores reference to the substate.
    /// </summary>
    private readonly Substate substate;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStateListViewSubstateCreatedEventArgs"/> class.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="substate">The substate.</param>
    public OrderStateListViewSubstateCreatedEventArgs([NotNull] State state, [NotNull] Substate substate)
    {
      Assert.ArgumentNotNull(state, "state");
      Assert.ArgumentNotNull(substate, "substate");

      this.state = state;
      this.substate = substate;

      this.Enabled = true;
    }

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <value>The state.</value>
    [NotNull]
    public State State
    {
      get
      {
        return this.state;
      }
    }

    /// <summary>
    /// Gets the substate.
    /// </summary>
    /// <value>The substate.</value>
    [NotNull]
    public Substate Substate
    {
      get
      {
        return this.substate;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="OrderStateListViewSubstateCreatedEventArgs"/> is enabled.
    /// </summary>
    /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
    public bool Enabled { get; set; }
  }
}