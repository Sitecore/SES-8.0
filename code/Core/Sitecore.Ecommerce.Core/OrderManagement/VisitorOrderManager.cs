// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisitorOrderManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the VisitorOrderManager type.
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

namespace Sitecore.Ecommerce.OrderManagement
{
  using System.Linq;
  using Orders;

  /// <summary>
  /// Defines the VisitorOrderManager class.
  /// </summary>
  public abstract class VisitorOrderManager
  {
    /// <summary>
    /// Creates the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    public abstract void Create([NotNull] Order order);

    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>
    /// The all orders.
    /// </returns>
    public abstract IQueryable<Order> GetAll();
  }
}