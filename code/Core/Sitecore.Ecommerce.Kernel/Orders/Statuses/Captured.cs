// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Captured.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Captured type.
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

namespace Sitecore.Ecommerce.Orders.Statuses
{
  using System;
  using DomainModel.Carts;
  using DomainModel.Orders;
  using DomainModel.Payments;
  using Sitecore.Exceptions;

  /// <summary>
  /// The Captured state.
  /// </summary>
  [Obsolete]
  public class Captured : OrderStatusBase
  {
    /// <summary>
    /// Processes the specified order.
    /// </summary>
    /// <typeparam name="T">The order type.</typeparam>
    /// <param name="order">The order instance.</param>
    /// <exception cref="InvalidTypeException">The provider doesn't implemente IReservable interface.</exception>
    protected override void Process<T>(T order)
    {
      bool captureSuccess = false;
      ReservationTicket reservationTicket = new ReservationTicket(order);
      PaymentArgs paymentArgs = new PaymentArgs
      {
        ShoppingCart = new ShoppingCart
        {
          Currency = order.Currency
        }
      };
      
      PaymentProvider paymentProvider = Context.Entity.Resolve<PaymentProvider>(order.PaymentSystem.Code);
      IReservable reservableProvider = paymentProvider as IReservable;

      if (reservableProvider != null)
      {
        reservableProvider.Capture(order.PaymentSystem, paymentArgs, reservationTicket, reservationTicket.Amount);

        ITransactionData transactionDataProvider = Context.Entity.Resolve<ITransactionData>();
        string result = transactionDataProvider.GetPersistentValue(order.OrderNumber) as string;

        if (string.Compare(result, "Captured", StringComparison.OrdinalIgnoreCase) == 0)
        {
          captureSuccess = true;
        }
      }

      if (!captureSuccess)
      {
        order.Status = Context.Entity.Resolve<OrderStatus>("In Process");
        IOrderManager<Order> orderManager = Context.Entity.Resolve<IOrderManager<Order>>();
        orderManager.SaveOrder(order);
      }

      if (reservableProvider == null)
      {
        throw new InvalidTypeException("The provider doesn't implement IReservable interface.");
      }
    }
  }
}
