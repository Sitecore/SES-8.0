// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnSmartPanelAllowanceChargeStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OnSmartPanelAllowanceChargeStrategy type.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.ContextStrategies
{
  using System;
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the on smart panel allowance charge strategy class.
  /// </summary>
  public class OnSmartPanelAllowanceChargeStrategy : SmartPanelEntityResolvingStrategy
  {
    /// <summary>
    /// Order id key.
    /// </summary>
    private const string AllowanceChargeKey = "Alias";

    /// <summary>
    /// Gets the order id.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The order id.
    /// </returns>
    public override object GetEntity([NotNull] object context)
    {
      Assert.ArgumentNotNull(context, "context");

      string aliasStr = this.GetKeyValue(context, AllowanceChargeKey);
      Assert.IsNotNullOrEmpty(aliasStr, "Alias should be provided.");

      long alias = long.Parse(aliasStr);

      return alias < 0 ? null : this.GetEntity(alias);
    }

    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>
    /// The entity.
    /// </returns>
    public override object GetEntity(long id)
    {
      return this.OrderManager.GetOrders().FirstOrDefault(o => o.AllowanceCharge.Any(ac => ac.Alias == id));
    }

    /// <summary>
    /// Gets the security checker.
    /// </summary>
    /// <returns>
    /// The security checker.
    /// </returns>
    public override Func<Order, bool> GetSecurityChecker()
    {
      return this.OrderSecurity.CanEditOrderLines;
    }
  }
}