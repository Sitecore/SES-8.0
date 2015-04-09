// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateModel.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the state list model class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Models
{
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the state list model class.
  /// </summary>
  public class StateModel
  {
    /// <summary>
    /// The inner state
    /// </summary>
    private readonly State innerState;

    /// <summary>
    /// Initializes a new instance of the <see cref="StateModel"/> class.
    /// </summary>
    /// <param name="state">The state.</param>
    public StateModel([NotNull] State state)
    {
      Assert.ArgumentNotNull(state, "state");

      this.innerState = state;
    }

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <value>
    /// The state.
    /// </value>
    [NotNull]
    public State InnerState
    {
      get { return this.innerState; }
    }

    /// <summary>
    /// Gets the code.
    /// </summary>
    [NotNull]
    public string Code
    {
      get { return this.innerState.Code; }
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    [NotNull]
    public string Name
    {
      get { return this.innerState.Name; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="StateModel"/> is enabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if enabled; otherwise, <c>false</c>.
    /// </value>
    public bool Enabled { get; set; }
  }
}