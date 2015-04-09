// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChargeDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the charge data source repository class.
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

  /// <summary>
  /// Defines the charge data source repository class.
  /// </summary>
  public class ChargeDataSourceRepository : AllowanceChargeDataSourceRepository
  {
    /// <summary>
    /// Selects allowance char collection.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>
    /// The allowance charge collection.
    /// </returns>
    [NotNull]
    public override IEnumerable<AllowanceCharge> SelectEntities(string rawQuery)
    {
      Assert.ArgumentNotNull(rawQuery, "rawQuery");

      return base.SelectEntities(rawQuery).Where(ac => ac.ChargeIndicator);
    }
  }
}