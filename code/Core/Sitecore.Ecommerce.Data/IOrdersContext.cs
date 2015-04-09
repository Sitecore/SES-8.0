// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrdersContext.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Represents order context interface.
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

namespace Sitecore.Ecommerce.Data
{
  using System;
  using System.Data;
  using System.Data.Entity;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Represents order context interface.
  /// </summary>
  public interface IOrdersContext 
  {
    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <value>The orders.</value>
    IDbSet<Order> Orders { get; }

    /// <summary>
    /// Applies the entity values to the entity stored in EF ObjectStateManager.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void ApplyValuesFromEntity(object entity);

    /// <summary>
    /// Deletes the entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void CascadeDelete(object entity);

    /// <summary>
    /// Determines whether the specified order is attached.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>
    ///   <c>true</c> if the specified order is attached; otherwise, <c>false</c>.
    /// </returns>
    bool IsAttached(object entity);

    /// <summary>
    /// Changes the state of the object.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="entityState">State of the entity.</param>
    void Entry(object entity, EntityState entityState);

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns>The changes.</returns>
    int SaveChanges();
  }
}