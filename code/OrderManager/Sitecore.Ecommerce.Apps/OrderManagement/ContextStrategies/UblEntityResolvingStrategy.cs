// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UblEntityResolvingStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the UblEntityResolvingStrategy type.
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
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;

  /// <summary>
  /// Defines the UblEntityResolvingStrategy type.
  /// </summary>
  public abstract class UblEntityResolvingStrategy
  {
    /// <summary>
    /// The merchant order manager
    /// </summary>
    private MerchantOrderManager orderManager;

    /// <summary>
    /// The order security.
    /// </summary>
    private MerchantOrderSecurity orderSecurity;

    /// <summary>
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>
    /// The order manager.
    /// </value>
    [NotNull]
    public virtual MerchantOrderManager OrderManager
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
    /// Gets or sets the order manager.
    /// </summary>
    /// <value>
    /// The order manager.
    /// </value>
    [NotNull]
    public virtual MerchantOrderSecurity OrderSecurity
    {
      get
      {
        return this.orderSecurity ?? (this.orderSecurity = new MerchantOrderSecurity());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        this.orderSecurity = value;
      }
    }

    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The entity.
    /// </returns>
    [CanBeNull]
    public abstract object GetEntity(object context);

    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>
    /// The entity.
    /// </returns>
    [CanBeNull]
    public virtual object GetEntity(long id)
    {
      return null;
    }

    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>
    /// The entity.
    /// </returns>
    [CanBeNull]
    public virtual object GetEntity(string id)
    {
      return null;
    }

    /// <summary>
    /// Gets the security checker.
    /// </summary>
    /// <returns>
    /// The security checker.
    /// </returns>
    [NotNull]
    public abstract Func<Order, bool> GetSecurityChecker();
  }
}