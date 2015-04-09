// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveSubstateChangingEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ActiveSubstateChangingEventArgs type.
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
  /// Defines the active substate changing event args class.
  /// </summary>
  public class ActiveSubstateChangingEventArgs : System.ComponentModel.CancelEventArgs
  {
    /// <summary>
    /// Represents the substate which active status is going to be changed.
    /// </summary>
    private readonly Substate substate;

    /// <summary>
    /// Represents the new value of the active status for the substate.
    /// </summary>
    private readonly bool newActiveStatusValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActiveSubstateChangingEventArgs"/> class.
    /// </summary>
    /// <param name="substate">The substate.</param>
    /// <param name="newActiveStatusValue">New value of active status for the substate.</param>
    public ActiveSubstateChangingEventArgs(Substate substate, bool newActiveStatusValue)
    {
      Diagnostics.Assert.IsNotNull(substate, "substate");

      this.substate = substate;
      this.newActiveStatusValue = newActiveStatusValue;
    }

    /// <summary>
    /// Gets the substate.
    /// </summary>
    /// <value>The substate.</value>
    public Substate Substate
    {
      get
      {
        return this.substate;
      }
    }

    /// <summary>
    /// Gets a value indicating whether [new active status value].
    /// </summary>
    /// <value>
    /// <c>true</c> if substate is in active status otherwise, <c>false</c>.
    /// </value>
    public bool NewActiveStatusValue
    {
      get
      {
        return this.newActiveStatusValue;
      }
    }
  }
}