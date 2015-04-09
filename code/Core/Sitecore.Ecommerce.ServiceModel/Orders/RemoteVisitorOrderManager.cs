// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoteVisitorOrderManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the RemoteVisitorOrderManager type.
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
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using Diagnostics;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Serialization;
  using OrderManagement;
  using OrderManagement.Orders;
  using OrderRepositoryService;
  using Utils;

  /// <summary>
  /// Defines the remote order repository class.
  /// </summary>
  public class RemoteVisitorOrderManager : VisitorOrderManager, IQueryProvider
  {
    /// <summary>
    /// Defines service client args factory.
    /// </summary>
    private readonly ServiceClientArgsFactory serviceClientArgsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteVisitorOrderManager" /> class.
    /// </summary>
    /// <param name="serviceClientArgsFactory">The service client args factory.</param>
    public RemoteVisitorOrderManager(ServiceClientArgsFactory serviceClientArgsFactory)
    {
      this.serviceClientArgsFactory = serviceClientArgsFactory;
    }

    /// <summary>
    /// Creates the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    public override void Create(Order order)
    {
      Assert.IsNotNull(this.serviceClientArgsFactory, "serviceClientArgsFactory must not be null");

      using (OrderRepositoryServiceClient client = new OrderRepositoryServiceClient())
      {
        var settings = new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
          MissingMemberHandling = MissingMemberHandling.Ignore,
          PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
        string orderSerialization = JsonConvert.SerializeObject(order, Formatting.None, settings);

        client.Create(orderSerialization, this.serviceClientArgsFactory.GetServiceClientArgs());
      }
    }

    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>The all orders.</returns>
    [NotNull]
    public override IQueryable<Order> GetAll()
    {
      return new RemoteOrderQueryable(this);
    }

    /// <summary>
    /// Constructs an <see cref="T:System.Linq.IQueryable"/> object that can evaluate the query represented by a specified expression tree.
    /// </summary>
    /// <param name="expression">An expression tree that represents a LINQ query.</param>
    /// <returns>
    /// An <see cref="T:System.Linq.IQueryable"/> that can evaluate the query represented by the specified expression tree.
    /// </returns>
    [NotNull]
    public virtual IQueryable CreateQuery(Expression expression)
    {
      return this.CreateQuery<Order>(expression);
    }

    /// <summary>
    /// Creates the query.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    /// <param name="expression">The expression.</param>
    /// <returns> The query. </returns>
    /// <exception cref="NotSupportedException">Types other than Order are not supported.</exception>
    public virtual IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
      if (typeof(TElement) != typeof(Order))
      {
        throw new NotSupportedException("Types other than Order are not supported.");
      }

      return (IQueryable<TElement>)new RemoteOrderQueryable(this, expression);
    }

    /// <summary>
    /// Executes the query represented by a specified expression tree.
    /// </summary>
    /// <param name="expression">An expression tree that represents a LINQ query.</param>
    /// <returns>
    /// The value that results from executing the specified query.
    /// </returns>
    [CanBeNull]
    public virtual object Execute(Expression expression)
    {
      return this.Execute<Order>(expression);
    }

    /// <summary>
    /// Executes the specified expression.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="expression">The expression.</param>
    /// <returns>The T result.</returns>
    /// <exception cref="NotSupportedException">Types other than Order or IEnumerable of Order are not supported.</exception>
    /// <exception cref="FormatException">Wrong serialization format</exception>
    [CanBeNull]
    public virtual TResult Execute<TResult>(Expression expression)
    {
      if ((typeof(TResult) != typeof(Order)) && (typeof(TResult) != typeof(IEnumerable<Order>)))
      {
        throw new NotSupportedException("Types other than Order or IEnumerable of Order are not supported.");
      }

      Assert.IsNotNull(this.serviceClientArgsFactory, "serviceClientArgsFactory must not be null");

      using (OrderRepositoryServiceClient client = new OrderRepositoryServiceClient())
      {
        var serviceResult = client.GetAll((new ExpressionSerializer()).Serialize(expression), this.serviceClientArgsFactory.GetServiceClientArgs());
        var settings = new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
          MissingMemberHandling = MissingMemberHandling.Ignore,
          PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
        settings.Error += this.DeserializationErrorHandler;

        if (typeof(TResult) == typeof(Order))
        {
          return (TResult)(object)JsonConvert.DeserializeObject<Order>(serviceResult, settings);
        }

        return (TResult)(object)JsonConvert.DeserializeObject<List<Order>>(serviceResult, settings);
      }
    }

    /// <summary>
    /// Deserializations the error handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="Newtonsoft.Json.Serialization.ErrorEventArgs"/> instance containing the event data.</param>
    private void DeserializationErrorHandler(object sender, ErrorEventArgs e)
    {
      e.ErrorContext.Handled = true;
    }
  }
}