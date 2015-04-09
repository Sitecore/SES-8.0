// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The payment provider interface.
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
  using System;

  /// <summary>
  /// The payment provider interface.
  /// </summary>
  /// <typeparam name="T">The payment method type.</typeparam>
  [Obsolete("Use the PaymentProvider abstract class")]
  public interface IPaymentProvider<T> where T : PaymentSystem
  {
    /// <summary>
    /// Gets or sets the payment success page URL.
    /// </summary>
    /// <value>The payment success page URL.</value>
    [Obsolete("Use the SuccessPageUrl property of the PaymentArgs instance")]
    string SuccessPageUrl { get; set; }

    /// <summary>
    /// Gets or sets the payment failure page URL.
    /// </summary>
    /// <value>The payment failure page URL.</value>
    [Obsolete("Use the FailurePageUrl property of the PaymentArgs instance")]
    string FailurePageUrl { get; set; }

    /// <summary>
    /// Gets or sets the selected payment method.
    /// </summary>
    /// <value>The payment method.</value>
    [Obsolete("Use the additional PaymentSystem parameter")]
    T PaymentSystem { get; set; }

    /// <summary>
    /// Gets or sets the payment status.
    /// </summary>
    /// <value>The payment status.</value>
    /// <returns>
    /// The payment status.
    /// </returns>
    PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Initializes the payment.
    /// </summary>
    void InvokePayment();
  }
}