// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MerchantExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the MerchantExtensions type.
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

namespace Sitecore.Ecommerce.Merchant.OrderManagement.Extensions
{
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Order extensions.
  /// </summary>
  public static class MerchantExtensions
  {
    /// <summary>
    /// Creates reservation ticket from the Order.ReservationTicket.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// ReservationTicket from the DomainModel.
    /// </returns>
    public static DomainModel.Payments.ReservationTicket GetReservationTicket([NotNull] this Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      return new DomainModel.Payments.ReservationTicket
      {
        Amount = order.ReservationTicket.Amount,
        AuthorizationCode = order.ReservationTicket.AuthorizationCode,
        InvoiceNumber = order.ReservationTicket.InvoiceNumber,
        TransactionNumber = order.ReservationTicket.TransactionNumber
      };
    }

    /// <summary>
    /// Finds the specified merchant order manager.
    /// </summary>
    /// <param name="merchantOrderManager">The merchant order manager.</param>
    /// <param name="orderId">The order id.</param>
    /// <returns>The order.</returns>
    [CanBeNull]
    public static Order Find([NotNull] this MerchantOrderManager merchantOrderManager, [NotNull] string orderId)
    {
      Assert.IsNotNull(merchantOrderManager, "merchantOrderManager");
      Assert.IsNotNull(orderId, "orderId");

      return merchantOrderManager.GetOrders().SingleOrDefault(o => o.OrderId == orderId);
    }
  }
}