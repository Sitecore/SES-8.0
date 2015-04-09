// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyOrderRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The empty order repository.
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
  using Data;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// The empty order repository.
  /// </summary>
  public class EmptyOrderRepository : Repository<Order>
  {
    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns>
    /// Empty enumeration.
    /// </returns>
    protected internal override IQueryable<Order> GetAll()
    {
      return Enumerable.Empty<Order>().AsQueryable();
    }

    /// <summary>
    /// Performs the saving.
    /// </summary>
    /// <param name="entities">
    /// The entities.
    /// </param>
    protected internal override void PerformSaving(IEnumerable<Order> entities)
    {
    }

    /// <summary>
    /// Deletes the specified entities.
    /// </summary>
    /// <param name="entities">
    /// The entities.
    /// </param>
    protected internal override void Delete(IEnumerable<Order> entities)
    {
    }

    /// <summary>
    /// Gets the persisted entity.
    /// </summary>
    /// <param name="entity">
    /// The entity.
    /// </param>
    /// <returns>
    /// The <see cref="Order"/>.
    /// </returns>
    protected override Order GetPersistedEntity(Order entity)
    {
      return null;
    }
  }
}