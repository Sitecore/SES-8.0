// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LastOrdersReadyForCaptureDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the last orders ready for capture data source repository class.
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
  using Ecommerce.OrderManagement;
  using Merchant.OrderManagement;
  using Models;

  /// <summary>
  /// Defines the last orders ready for capture data source repository class.
  /// </summary>
  public class LastOrdersReadyForCaptureDataSourceRepository : OrderListModelDataSourceRepository
  {
    /// <summary>
    /// StateValidator instance.
    /// </summary>
    private StateValidator stateValidator;

    /// <summary>
    /// Gets or sets the state validator.
    /// </summary>
    /// <value>
    /// The state validator.
    /// </value>
    [NotNull]
    public StateValidator StateValidator
    {
      get
      {
        return this.stateValidator ?? (this.stateValidator = new StateValidator());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.stateValidator = value;
      }
    }

    /// <summary>
    /// Selects the orders.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>The orders.</returns>
    [NotNull]
    public override IEnumerable<OrderListModel> SelectEntities([CanBeNull] string rawQuery)
    {
      var orders = this
        .GetOrders()
        .AsQueryable()
        .Where(o => o.State.Code == OrderStateCode.InProcess);

      return orders.Where(o => o.State.Substates.Any(ss => (ss.Code == OrderStateCode.InProcessCapturedInFull) && (!ss.Active))).AsEnumerable()
                .Where(o => this.StateValidator.CanBeCaptured(o, OrderStateCode.InProcessCapturedInFull))
                .OrderByDescending(o => o.IssueDate)
                .Take(6)
                .Select(GetOrderListModel);
    }
  }
}