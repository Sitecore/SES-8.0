// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllowanceChargeDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the allowance charge data source repository class.
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
  using Merchant.OrderManagement;

  /// <summary>
  /// Defines the allowance charge data source repository class.
  /// </summary>
  public class AllowanceChargeDataSourceRepository : DataSourceRepository<AllowanceCharge>
  {
    /// <summary>
    /// The merchant order manager
    /// </summary>
    private MerchantOrderManager orderManager;

    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>
    /// The order manager.
    /// </value>
    [NotNull]
    public MerchantOrderManager OrderManager
    {
      get
      {
        return this.orderManager ?? (this.orderManager = Context.Entity.Resolve<MerchantOrderManager>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        this.orderManager = value;
      }
    }

    /// <summary>
    /// Gets the specified raw query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>
    /// The allowance charge.
    /// </returns>
    [CanBeNull]
    public virtual AllowanceCharge Get([NotNull] string rawQuery)
    {
      Assert.ArgumentNotNull(rawQuery, "rawQuery");

      long alias = long.Parse(rawQuery);

      AllowanceCharge charge = this.OrderManager.GetOrders()
        .First(o => o.AllowanceCharge.Any(ac => ac.Alias == alias))
        .AllowanceCharge.FirstOrDefault(ac => ac.Alias == alias);

      return charge;
    }

    /// <summary>
    /// Selects the specified raw query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>
    /// The I enumerable.
    /// </returns>
    public override IEnumerable<AllowanceCharge> SelectEntities([NotNull] string rawQuery)
    {
      Assert.ArgumentNotNull(rawQuery, "rawQuery");

      return this.OrderManager.GetOrder(rawQuery).AllowanceCharge;
    }
  }
}