// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateValidator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the StateValidator type.
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

namespace Sitecore.Ecommerce.Merchant.OrderManagement
{
  using System.Collections.Generic;
  using System.Linq;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Defines the state validator class.
  /// </summary>
  public class StateValidator
  {
    /// <summary>
    /// Gets the admissible substates.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="substateCombinationSet">The substate combination set.</param>
    /// <returns>The admissible substates.</returns>
    [NotNull]
    public virtual IEnumerable<Substate> GetAdmissibleSubstates([NotNull] State state, [NotNull] SubstateCombinationSet substateCombinationSet)
    {
      Assert.ArgumentNotNull(state, "state");
      Assert.ArgumentNotNull(substateCombinationSet, "substateCombinationSet");

      if (!substateCombinationSet.SubstateCombinations.Any())
      {
        return state.Substates;
      }

      System.Collections.BitArray flags = new System.Collections.BitArray(state.Substates.Count);
      int index = 0;

      foreach (Substate substate in state.Substates)
      {
        flags[index] = substate.Active;
        ++index;
      }

      foreach (IDictionary<string, bool> substateCombination in substateCombinationSet.SubstateCombinations)
      {
        bool flag = state.Substates.Aggregate(
          true, (current, substate) => current && ((!substate.Active) || substateCombination[substate.Code]));

        index = 0;
        foreach (Substate substate in state.Substates)
        {
          flags[index] = flags[index] || (flag && substateCombination[substate.Code]);
          ++index;
        }
      }

      LinkedList<Substate> result = new LinkedList<Substate>();

      index = 0;
      foreach (Substate substate in state.Substates)
      {
        if (flags[index])
        {
          result.AddLast(substate);
        }

        ++index;
      }

      return result;
    }

    /// <summary>
    /// Determines whether the specified order can be captured.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="substateCode">The substate code.</param>
    /// <returns>
    ///   <c>true</c> if the specified order can be captured; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool CanBeCaptured([NotNull] Order order, [NotNull] string substateCode)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(substateCode, "substateCode");

      if (order.ReservationTicket != null && OrderStateCode.InProcessCapturedInFull == substateCode)
      {
        if (order.AnticipatedMonetaryTotal.PayableAmount.Value > order.ReservationTicket.Amount)
        {
          return false;
        }
      }

      return true;
    }
  }
}