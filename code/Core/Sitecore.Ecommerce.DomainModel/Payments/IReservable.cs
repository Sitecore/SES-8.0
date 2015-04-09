// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReservable.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the IReservable payment provider type.
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

namespace Sitecore.Ecommerce.DomainModel.Payments
{
  /// <summary>
  /// The online payment reservable credit card interface.
  /// </summary>
  public interface IReservable
  {
    /// <summary>
    /// Invokes the specified payment.
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="paymentArgs">The payment args.</param>
    void Invoke(PaymentSystem paymentSystem, PaymentArgs paymentArgs);

    /// <summary>
    /// Captures the payment.
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment arguments.</param>
    /// <param name="reservationTicket">The reservation ticket.</param>
    /// <param name="amount">The amount.</param>
    void Capture(PaymentSystem paymentSystem, PaymentArgs paymentArgs, ReservationTicket reservationTicket, decimal amount);

    /// <summary>
    /// Cancels the payment reservation.
    /// </summary>
    /// <param name="paymentSystem">The payment System.</param>
    /// <param name="paymentArgs">The payment arguments.</param>
    /// <param name="reservationTicket">The reservation ticket.</param>
    void CancelReservation(PaymentSystem paymentSystem, PaymentArgs paymentArgs, ReservationTicket reservationTicket);
  }
}