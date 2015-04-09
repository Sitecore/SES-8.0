// --------------------------------------------------------------------------------------------------------------------
// <copyright file="State.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the State type.
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

namespace Sitecore.Ecommerce.OrderManagement.Orders
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using Common;
  using Diagnostics;

  /// <summary>
  /// Defines the state class.
  /// </summary>
  public class State : IEntity
  {
    /// <summary>
    /// The sub-states.
    /// </summary>
    private ICollection<Substate> substates;

    /// <summary>
    /// Gets or sets the id of the state.
    /// </summary>
    /// <value>The id of the state.</value>
    public virtual string Code { get; set; }

    /// <summary>
    /// Gets or sets the name of the state.
    /// </summary>
    /// <value>The name of the state.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="state1">The state1.</param>
    /// <param name="state2">The state2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(State state1, State state2)
    {
      if (ReferenceEquals(state1, state2))
      {
        return true;
      }

      if (ReferenceEquals(state1, null) ^ ReferenceEquals(state2, null))
      {
        return false;
      }

      return state1.Equals(state2);
    }

    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="state1">The state1.</param>
    /// <param name="state2">The state2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(State state1, State state2)
    {
      return !(state1 == state2);
    }

    /// <summary>
    /// Gets the substates.
    /// </summary>
    /// <value>
    /// The substates.
    /// </value>
    [NotNull]
    public virtual ICollection<Substate> Substates
    {
      get
      {
        return this.substates = this.substates ?? new Collection<Substate>();
      }

      protected set
      {
        Assert.ArgumentNotNull(value, "value");

        this.substates = value;
      }
    }

    public virtual long Alias { get; protected set; }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      State state = obj as State;

      if (state == null)
      {
        return false;
      }

      HashSet<Substate> thisHashSet = new HashSet<Substate>(this.Substates);
      HashSet<Substate> objHashSet = new HashSet<Substate>(state.Substates);

      return (state.Code == this.Code) && HashSet<Substate>.CreateSetComparer().Equals(thisHashSet, objHashSet);
    }

    /// <summary>
    /// Gets the object hash code.
    /// </summary>
    /// <returns>the objects hash code</returns>
    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
