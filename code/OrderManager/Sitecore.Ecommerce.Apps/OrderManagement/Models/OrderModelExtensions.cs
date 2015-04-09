// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderModelExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderModelExtensions type.
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
  using System.Linq;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the order model extensions class.
  /// </summary>
  public static class OrderModelExtensions
  {
    #region Cloning

    /// <summary>
    /// Clones the specified state.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>The cloned state.</returns>
    public static State Clone(this State state)
    {
      Diagnostics.Assert.IsNotNull(state, "state");

      State result = new State { Code = state.Code, Name = state.Name };

      foreach (Substate substate in state.Substates)
      {
        result.Substates.Add(substate.Clone());
      }

      return result;
    }

    /// <summary>
    /// Clones the specified substate.
    /// </summary>
    /// <param name="substate">The substate.</param>
    /// <returns>The cloned substate.</returns>
    public static Substate Clone(this Substate substate)
    {
      Diagnostics.Assert.IsNotNull(substate, "substate");

      return new Substate { Code = substate.Code, Name = substate.Name, Active = substate.Active, Abbreviation = substate.Abbreviation };
    }

    #endregion

    #region Comparison

    /// <summary>
    /// Equalses the specified state1.
    /// </summary>
    /// <param name="state1">The state1.</param>
    /// <param name="state2">The state2.</param>
    /// <returns>The boolean.</returns>
    public static bool EqualsTo(this State state1, State state2)
    {
      if (ReferenceEquals(state1, state2))
      {
        return true;
      }

      if ((state1 == null) ^ (state2 == null))
      {
        return false;
      }

      if (state1.Code != state2.Code)
      {
        return false;
      }

      System.Collections.Generic.IDictionary<string, bool> substateStatuses1 = state1.Substates.ToDictionary(substate => substate.Code, substate => substate.Active);
      System.Collections.Generic.IDictionary<string, bool> substateStatuses2 = state2.Substates.ToDictionary(substate => substate.Code, substate => substate.Active);

      if (substateStatuses1.Count != substateStatuses2.Count)
      {
        return false;
      }

      foreach (System.Collections.Generic.KeyValuePair<string, bool> keyValuePair in substateStatuses1)
      {
        if (!substateStatuses2.ContainsKey(keyValuePair.Key))
        {
          return false;
        }

        if (!keyValuePair.Value.Equals(substateStatuses2[keyValuePair.Key]))
        {
          return false;
        }
      }

      return true;
    }

    /// <summary>
    /// Equalses the specified substate1.
    /// </summary>
    /// <param name="substate1">The substate1.</param>
    /// <param name="substate2">The substate2.</param>
    /// <returns>The boolean.</returns>
    public static bool EqualsTo(this Substate substate1, Substate substate2)
    {
      if (ReferenceEquals(substate1, substate2))
      {
        return true;
      }

      if ((substate1 == null) ^ (substate2 == null))
      {
        return false;
      }

      if (substate1.Code != substate2.Code)
      {
        return false;
      }

      if (substate1.Active != substate2.Active)
      {
        return false;
      }

      return true;
    }

    #endregion
  }
}