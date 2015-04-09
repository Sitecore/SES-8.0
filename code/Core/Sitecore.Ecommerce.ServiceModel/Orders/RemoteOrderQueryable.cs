// --------------------------------------------------------------------------------------------------------------------
// <copyright  file="RemoteOrderQueryable.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the RemoteOrderQueryable type.
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

namespace Sitecore.Ecommerce.ServiceModel.Orders
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the RemoteOrderQueryable class. This class allows to perform operations on IQueryable remotely.
  /// </summary>
  public class RemoteOrderQueryable : IQueryable<Order>
  {
    /// <summary>
    /// Defines query provider to use in order to get actual orders.
    /// </summary>
    private readonly IQueryProvider provider;

    /// <summary>
    /// Defines expression to apply while getting orders.
    /// </summary>
    private readonly Expression expression;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteOrderQueryable"/> class.
    /// </summary>
    /// <param name="provider">The provider.</param>
    public RemoteOrderQueryable([NotNull] IQueryProvider provider)
    {
      Assert.ArgumentNotNull(provider, "provider");

      this.provider = provider;
      this.expression = Expression.Constant(this);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteOrderQueryable"/> class.
    /// </summary>
    /// <param name="provider">The query provider.</param>
    /// <param name="expression">The expression.</param>
    public RemoteOrderQueryable([NotNull] IQueryProvider provider, [NotNull] Expression expression)
    {
      Assert.ArgumentNotNull(provider, "provider");
      Assert.ArgumentNotNull(expression, "expression");

      this.provider = provider;
      this.expression = expression;
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<Order> GetEnumerator()
    {
      return this.Provider.Execute<IEnumerable<Order>>(this.Expression).GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    /// <summary>
    /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"/>.
    /// </summary>
    /// <returns>The <see cref="T:System.Linq.Expressions.Expression"/> that is associated with this instance of <see cref="T:System.Linq.IQueryable"/>.</returns>
    public Expression Expression
    {
      get
      {
        return this.expression;
      }
    }

    /// <summary>
    /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
    /// </summary>
    /// <returns>A <see cref="T:System.Type"/> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
    public Type ElementType
    {
      get
      {
        return typeof(Order);
      }
    }

    /// <summary>
    /// Gets the query provider that is associated with this data source.
    /// </summary>
    /// <returns>The <see cref="T:System.Linq.IQueryProvider"/> that is associated with this data source.</returns>
    public IQueryProvider Provider
    {
      get
      {
        return this.provider;
      }
    }
  }
}
