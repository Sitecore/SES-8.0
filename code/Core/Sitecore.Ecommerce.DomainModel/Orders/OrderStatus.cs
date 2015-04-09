// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStatus.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The order status base class.
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

namespace Sitecore.Ecommerce.DomainModel.Orders
{
  using System;
  using System.Collections.Generic;
  
  /// <summary>
  /// The order status.
  /// </summary>
  [Serializable]
  [Obsolete]
  public abstract class OrderStatus : IComparable
  {
    /// <summary>
    /// Gets or sets code for the order status
    /// </summary>
    public virtual string Code { get; set; }

    /// <summary>
    /// Gets or sets title for the order status
    /// </summary>    
    public virtual string Title { get; set; }

    /// <summary>
    /// Gets the available following statuses.
    /// </summary>
    /// <returns>Collection of the available following statuses</returns>
    public abstract IEnumerable<OrderStatus> GetAvailableFollowingStatuses();

    /// <summary>Compares to instances.</summary>
    /// <param name="obj">The object.</param>
    /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
    /// <returns>The result of the comparison of the Type names.</returns>
    int IComparable.CompareTo(object obj)
    {
      OrderStatus otherOrderStatus = obj as OrderStatus;
      if (otherOrderStatus != null)
      {
        return string.Compare(this.GetType().Name, otherOrderStatus.GetType().Name, StringComparison.Ordinal);
      }

      throw new ArgumentException("Object is not a OrderStatus");
    }

    /// <summary>
    /// Processes the specified order.
    /// </summary>
    /// <typeparam name="T">The order type.</typeparam>
    /// <param name="order">The order instance.</param>
    protected virtual void Process<T>(T order) where T : Order
    {
      throw new NotImplementedException();
    }
  }
}