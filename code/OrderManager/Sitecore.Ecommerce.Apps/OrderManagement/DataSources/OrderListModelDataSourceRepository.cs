// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderListModelDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order list model data source repository class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.DataSources
{
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Globalization;
  using Models;

  /// <summary>
  /// Defines the order list model data source repository class.
  /// </summary>
  public abstract class OrderListModelDataSourceRepository : OrderDataSourceRepository<OrderListModel>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderListModelDataSourceRepository"/> class.
    /// </summary>
    protected OrderListModelDataSourceRepository()
    {
      this.Translate = new TranslateWrapper();
    }

    /// <summary>
    /// Gets or sets Translate.
    /// </summary>
    [NotNull]
    internal TranslateWrapper Translate { get; set; }

    /// <summary>
    /// Gets the order list model.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// The order list model.
    /// </returns>
    [NotNull]
    protected virtual OrderListModel GetOrderListModel([NotNull] Order order)
    {
      Debug.ArgumentNotNull(order, "order");

      string state = this.GetState(order.State);
      string customerName = this.GetCustomerName(order.BuyerCustomerParty);
      decimal totalAmount = this.GetTotalAmount(order.AnticipatedMonetaryTotal);

      return new OrderListModel
      {
        ID = order.OrderId,
        IssueDate = order.IssueDate,
        IssueTime = order.IssueDate.TimeOfDay,
        State = state,
        CustomerName = customerName,
        TotalAmount = totalAmount,
        Currency = order.PricingCurrencyCode
      };
    }

    /// <summary>
    /// Gets the name of the customer.
    /// </summary>
    /// <param name="buyerCustomerParty">The buyer customer party.</param>
    /// <returns>The customer name.</returns>
    [NotNull]
    private string GetCustomerName([CanBeNull]CustomerParty buyerCustomerParty)
    {
      return (buyerCustomerParty != null && buyerCustomerParty.Party != null && buyerCustomerParty.Party.PartyName != null) ? buyerCustomerParty.Party.PartyName : string.Empty;
    }

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>The formatted state.</returns>
    [NotNull]
    private string GetState([CanBeNull] State state)
    {
      if (state == null)
      {
        return string.Empty;
      }

      string stateStr = this.Translate.Text(state.Name);
      if (!string.IsNullOrEmpty(stateStr))
      {
        string substates = this.GetSubstates(state.Substates);
        if (!string.IsNullOrEmpty(substates))
        {
          stateStr += " (" + substates + ")";
        }
      }

      return stateStr;
    }

    /// <summary>
    /// Shorts the substate.
    /// </summary>
    /// <param name="substates">The substates.</param>
    /// <returns>
    /// The substate.
    /// </returns>
    [NotNull]
    private string GetSubstates([CanBeNull] IEnumerable<Substate> substates)
    {
      if (substates == null)
      {
        return string.Empty;
      }

      IEnumerable<Substate> activeSubtates = substates.Where(s => s.Active && !string.IsNullOrEmpty(s.Abbreviation)).ToList();
      if (!activeSubtates.Any())
      {
        return string.Empty;
      }

      if (activeSubtates.Count() == 1)
      {
        return this.Translate.Text(activeSubtates.First().Abbreviation);
      }

      return activeSubtates.Select(s => s.Abbreviation).Aggregate((s1, s2) => this.Translate.Text(s1) + ", " + this.Translate.Text(s2));
    }

    /// <summary>
    /// Gets the total amount.
    /// </summary>
    /// <param name="anticipatedMonetaryTotal">The anticipated monetary total.</param>
    /// <returns>
    /// The total amount.
    /// </returns>
    private decimal GetTotalAmount([CanBeNull] MonetaryTotal anticipatedMonetaryTotal)
    {
      return anticipatedMonetaryTotal != null ? anticipatedMonetaryTotal.PayableAmount.Value : 0;
    }
  }
}