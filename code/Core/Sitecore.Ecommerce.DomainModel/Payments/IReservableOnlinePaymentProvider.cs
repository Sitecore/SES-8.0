// -------------------------------------------------------------------------------------------
// <copyright file="IReservableOnlinePaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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
  using System;
  using System.Web;

  /// <summary>
  /// The online payment reservable credit card interface.
  /// </summary>
  /// <typeparam name="T">The payment method type.</typeparam>
  /// <typeparam name="TRT">The reservation ticket contract.</typeparam>
  [Obsolete("Use the IReservable interface")]
  public interface IReservableOnlinePaymentProvider<T, TRT> : IOnlinePaymentProvider<T>
    where T : PaymentSystem
    where TRT : ReservationTicket
  {
    /// <summary>
    /// Reserves the payment.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>The reservation ticket.</returns>
    TRT ReservePayment(HttpRequest request);

    /// <summary>
    /// Captures the payment.
    /// </summary>
    /// <param name="reservationTicket">The reservation ticket.</param>
    void CapturePayment(TRT reservationTicket);

    /// <summary>
    /// Cancels the payment reservation.
    /// </summary>
    /// <param name="reservationTicket">The reservation ticket.</param>
    void CancelPaymentReservation(TRT reservationTicket);

    /// <summary>
    /// Captures the payment.
    /// </summary>
    /// <param name="reservationTicket">The reservation ticket.</param>
    /// <param name="amount">The amount.</param>
    void CapturePayment(TRT reservationTicket, decimal amount);
  }
}