// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Substate.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Substate type.
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
  using Common;

  /// <summary>
  /// Defines the substate class.
  /// </summary>
  public class Substate : IEntity
  {
    /// <summary>
    /// Gets or sets the id of the substate.
    /// </summary>
    /// <value>The id of the substate.</value>
    public virtual string Code { get; set; }

    /// <summary>
    /// Gets or sets the name of the substate.
    /// </summary>
    /// <value>The name of the substate.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>
    /// The alias.
    /// </value>
    public virtual long Alias { get; protected set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="Substate"/> is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if active; otherwise, <c>false</c>.
    /// </value>
    public virtual bool Active { get; set; }

    /// <summary>
    /// Gets or sets the abbreviation.
    /// </summary>
    /// <value>The abbreviation.</value>
    public virtual string Abbreviation { get; set; }

    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="substate1">The substate1.</param>
    /// <param name="substate2">The substate2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(Substate substate1, Substate substate2)
    {
      if (ReferenceEquals(substate1, substate2))
      {
        return true;
      }

      if (ReferenceEquals(substate1, null) ^ ReferenceEquals(substate2, null))
      {
        return false;
      }

      return substate1.Equals(substate2);
    }

    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="substate1">The substate1.</param>
    /// <param name="substate2">The substate2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(Substate substate1, Substate substate2)
    {
      return !(substate1 == substate2);
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals([CanBeNull] object obj)
    {
      Substate substate = obj as Substate;

      return (substate != null) && (substate.Active == this.Active) && (substate.Code == this.Code);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode()
    {
      int codeHashCode = 0;

      if (this.Code != null)
      {
        codeHashCode = this.Code.GetHashCode();
      }

      if (this.Active)
      {
        codeHashCode ^= 1;
      }

      return codeHashCode;
    }
  }
}