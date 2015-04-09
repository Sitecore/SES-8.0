// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstateControlDataBoundEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SubstateControlCreatedEventArgs type.
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
  using System.Web.UI;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the substate control created event args class.
  /// </summary>
  public class SubstateControlDataBoundEventArgs : System.EventArgs
  {
    /// <summary>
    /// Represents the control which represents the substate.
    /// </summary>
    private readonly Control control;

    /// <summary>
    /// The substate.
    /// </summary>
    private readonly Substate substate;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubstateControlDataBoundEventArgs"/> class.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="substate">The substate.</param>
    public SubstateControlDataBoundEventArgs(Control control, Substate substate)
    {
      Diagnostics.Assert.IsNotNull(control, "control");
      Diagnostics.Assert.IsNotNull(substate, "substate");

      this.control = control;
      this.substate = substate;
    }

    /// <summary>
    /// Gets the control.
    /// </summary>
    /// <value>The control.</value>
    public Control Control
    {
      get
      {
        return this.control;
      }
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