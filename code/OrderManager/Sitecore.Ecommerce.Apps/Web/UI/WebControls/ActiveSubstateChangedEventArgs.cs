// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveSubstateChangedEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ActiveSubstateChangedEventArgs type.
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
  /// Defines the active substate changed event args class.
  /// </summary>
  public class ActiveSubstateChangedEventArgs : System.EventArgs
  {
    /// <summary>
    /// Represents the substate that has been changed.
    /// </summary>
    private readonly Substate substate;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActiveSubstateChangedEventArgs"/> class.
    /// </summary>
    /// <param name="substate">The substate.</param>
    public ActiveSubstateChangedEventArgs(Substate substate)
    {
      Diagnostics.Assert.IsNotNull(substate, "substate");

      this.substate = substate;
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
  }
}