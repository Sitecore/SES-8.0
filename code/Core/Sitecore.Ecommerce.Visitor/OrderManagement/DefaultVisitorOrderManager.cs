// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultVisitorOrderManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the visitor order repository class.
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

namespace Sitecore.Ecommerce.Visitor.OrderManagement
{
  using System;
  using System.Linq;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Data;
  using Sitecore.Ecommerce.DomainModel.Users;
  using Sitecore.Ecommerce.Logging;
  using Sitecore.Ecommerce.OrderManagement;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the visitor order repository class.
  /// </summary>
  public class DefaultVisitorOrderManager : VisitorOrderManager, IUserAware
  {
    /// <summary>
    /// The inner repository
    /// </summary>
    private readonly Repository<Order> innerRepository;

    /// <summary>
    /// The state configuration.
    /// </summary>
    private readonly CoreOrderStateConfiguration stateConfiguration;

    /// <summary>
    /// The calculation strategy.
    /// </summary>
    private readonly IOrderCalculationStrategy calculationStrategy;

    /// <summary>
    /// The customer manager
    /// </summary>
    private readonly ICustomerManager<CustomerInfo> customerManager;

    /// <summary>
    /// Customer Id.
    /// </summary>
    private string customerId;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultVisitorOrderManager" /> class.
    /// </summary>
    /// <param name="innerRepository">The inner repository.</param>
    /// <param name="stateConfiguration">The state configuration.</param>
    /// <param name="calculationStrategy">The calculation strategy.</param>
    /// <param name="customerManager">The customer manager.</param>
    public DefaultVisitorOrderManager(Repository<Order> innerRepository, CoreOrderStateConfiguration stateConfiguration, IOrderCalculationStrategy calculationStrategy, ICustomerManager<CustomerInfo> customerManager)
    {
      Assert.ArgumentNotNull(innerRepository, "innerRepository");
      Assert.ArgumentNotNull(stateConfiguration, "stateConfiguration");
      Assert.ArgumentNotNull(calculationStrategy, "calculationStrategy");
      Assert.ArgumentNotNull(customerManager, "customerManager");

      this.innerRepository = innerRepository;
      this.stateConfiguration = stateConfiguration;
      this.calculationStrategy = calculationStrategy;
      this.customerManager = customerManager;
    }

    /// <summary>
    /// Gets or sets the customer id.
    /// </summary>
    /// <value>
    /// The customer id.
    /// </value>
    [NotNull]
    public virtual string CustomerId
    {
      get
      {
        if (this.customerId == null)
        {
          Assert.IsNotNull(this.customerManager.CurrentUser, "CustomerManager.CurrentUser must be set.");
          this.customerId = this.customerManager.CurrentUser.CustomerId;
        }

        return this.customerId;
      }

      set
      {
        this.customerId = value;
      }
    }

    /// <summary>
    /// Creates the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    [LogThis(Constants.CreateOrderAction, Constants.UserLevel)]
    public override void Create(Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      Assert.IsTrue(order.ID == Guid.Empty, "Unable to create the order. Order ID must be empty.");

      if (!this.stateConfiguration.IsValid(order.State))
      {
        throw new InvalidStateConfigurationException();
      }

      this.calculationStrategy.ApplyCalculations(order);

      this.innerRepository.Save(new[] { order });
    }

    /// <summary>
    /// Gets all the orders.
    /// </summary>
    /// <returns>
    /// The collection of the orders.
    /// </returns>
    [NotNull]
    public override IQueryable<Order> GetAll()
    {
      return this.innerRepository.GetAll().Where(o => o.BuyerCustomerParty.SupplierAssignedAccountID == this.CustomerId);
    }
  }
}