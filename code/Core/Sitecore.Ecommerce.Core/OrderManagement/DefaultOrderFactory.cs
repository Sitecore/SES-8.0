// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultOrderFactory.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The default order factory.
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
  using System.Collections.ObjectModel;
  using System.Linq;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Common;
  using Sitecore.Ecommerce.DomainModel.Currencies;
  using Sitecore.Ecommerce.DomainModel.Data;
  using Sitecore.Ecommerce.DomainModel.Orders;
  using Sitecore.Ecommerce.OrderManagement.Orders;
  using Order = Sitecore.Ecommerce.OrderManagement.Orders.Order;

  /// <summary>
  /// The default order factory.
  /// </summary>
  public class DefaultOrderFactory : IOrderFactory
  {
    /// <summary>
    /// The id generator.
    /// </summary>
    private readonly OrderIDGenerator idGenerator;

    /// <summary>
    /// The shop context.
    /// </summary>
    private readonly ShopContext shopContext;

    /// <summary>
    /// The state configuration.
    /// </summary>
    private readonly CoreOrderStateConfiguration stateConfiguration;

    /// <summary>
    /// The currency provider.
    /// </summary>
    private readonly IEntityProvider<Currency> currencyProvider;

    /// <summary>
    /// The payment channel code.
    /// </summary>
    private string paymentChannelCode = "MoneyTransfer";

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultOrderFactory"/> class.
    /// </summary>
    /// <param name="idGenerator">The id generator.</param>
    /// <param name="shopContext">The shop context.</param>
    /// <param name="stateConfiguration">The state configuration.</param>
    /// <param name="currencyProvider">The currency provider.</param>
    public DefaultOrderFactory([NotNull] OrderIDGenerator idGenerator, [NotNull] ShopContext shopContext, [NotNull] CoreOrderStateConfiguration stateConfiguration, [NotNull] IEntityProvider<Currency> currencyProvider)
    {
      Assert.ArgumentNotNull(idGenerator, "idGenerator");
      Assert.ArgumentNotNull(shopContext, "shopContext");
      Assert.ArgumentNotNull(stateConfiguration, "stateConfiguration");
      Assert.ArgumentNotNull(currencyProvider, "currencyProvider");

      this.idGenerator = idGenerator;
      this.shopContext = shopContext;
      this.stateConfiguration = stateConfiguration;
      this.currencyProvider = currencyProvider;
    }

    /// <summary>
    /// Gets or sets the payment channel code.
    /// </summary>
    /// <value>
    /// The payment channel code.
    /// </value>
    [NotNull]
    public string PaymentChannelCode
    {
      get
      {
        return this.paymentChannelCode;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.paymentChannelCode = value;
      }
    }

    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>New order.</returns>
    public Order Create()
    {
      var orderId = this.idGenerator.Generate();
      var state = this.stateConfiguration.GetStates().SingleOrDefault(s => s.Code == OrderStateCode.New);
      var currency = this.currencyProvider.Get(this.shopContext.GeneralSettings.DisplayCurrency);

      Assert.IsNotNull(state, "Unable to create the order. State was not found.");

      var order = new Order
      {
        OrderId = orderId,
        ShopContext = this.shopContext.InnerSite.Name,
        PricingCurrencyCode = currency.Code,
        State = state,
        PaymentMeans = new PaymentMeans { PaymentChannelCode = this.paymentChannelCode }
      };

      order.PaymentMeans.PaymentDueDate = order.IssueDate;

      var endDate = order.IssueDate.AddDays(7);

      var delivery = new Delivery
      {
        LatestDeliveryDate = endDate,
        LatestDeliveryTime = order.IssueTime,
        RequestedDeliveryPeriod = new Period
        {
          StartDate = order.IssueDate,
          EndDate = endDate,
          StartTime = order.IssueTime,
          EndTime = order.IssueTime
        },
        DeliveryLocation = new Location
        {
          ValidityPeriod = new Period
          {
            StartDate = order.IssueDate,
            EndDate = endDate,
            StartTime = order.IssueTime,
            EndTime = order.IssueTime               
          }
        }
      };

      order.Delivery = new Collection<Delivery> { delivery };

      return order;
    }
  }
}