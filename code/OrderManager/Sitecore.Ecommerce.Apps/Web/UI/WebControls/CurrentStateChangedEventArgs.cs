﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentStateChangedEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the StateChangedEventArgs type.
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
  /// Defines the state changed event args class.
  /// </summary>
  public class CurrentStateChangedEventArgs : System.EventArgs
  {
    /// <summary>
    /// Stores reference to the state that has just been changed.
    /// </summary>
    private readonly State state;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentStateChangedEventArgs"/> class.
    /// </summary>
    /// <param name="state">The state.</param>
    public CurrentStateChangedEventArgs(State state)
    {
      this.state = state;
    }

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <value>The state.</value>
    public State State
    {
      get
      {
        return this.state;
      }
    }
  }
}