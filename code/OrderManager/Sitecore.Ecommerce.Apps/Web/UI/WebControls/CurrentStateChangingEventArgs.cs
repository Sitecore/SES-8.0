// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateChangingEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the StateChangingEventArgs type.
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
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the state changing event args class.
  /// </summary>
  public class CurrentStateChangingEventArgs : System.ComponentModel.CancelEventArgs
  {
    /// <summary>
    /// Stores reference to the old state being changed.
    /// </summary>
    private readonly State oldState;

    /// <summary>
    /// Stores reference to the new state value.
    /// </summary>
    private readonly State newState;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentStateChangingEventArgs"/> class.
    /// </summary>
    /// <param name="oldState">The old state.</param>
    /// <param name="newState">The new state.</param>
    public CurrentStateChangingEventArgs(State oldState, State newState)
    {
      this.oldState = oldState;
      this.newState = newState;
    }

    /// <summary>
    /// Gets the old state.
    /// </summary>
    /// <value>The old state.</value>
    public State OldState
    {
      get
      {
        return this.oldState;
      }
    }

    /// <summary>
    /// Gets the new state.
    /// </summary>
    /// <value>The new state.</value>
    public State NewState
    {
      get
      {
        return this.newState;
      }
    }
  }
}