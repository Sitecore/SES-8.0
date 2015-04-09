// -------------------------------------------------------------------------------------------
// -------------------------------------------------------------------------------------------
// <copyright file="IOnlinePaymentProvider.cs" company="Sitecore Corporation">
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
  /// The online payment provider interface.
  /// </summary>
  /// <typeparam name="T">The payment method type.</typeparam>
  [Obsolete("Use the IOnlinePaymentProvider interface")]
  public interface IOnlinePaymentProvider<T> : IPaymentProvider<T> where T : PaymentSystem
  {
    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <value>The description.</value>
    [Obsolete("Use the Description property of the PaymentArgs instance")]
    string Description { get; }

    /// <summary>
    /// Gets or sets the payment return page URL.
    /// </summary>
    /// <value>The payment return page URL.</value>
    [Obsolete("Use the ReturnPageUrl property of the PaymentArgs instance")]
    string ReturnPageUrl { get; set; }

    /// <summary>
    /// Gets or sets the payment cancel page URL.
    /// </summary>
    /// <value>The payment cancel page URL.</value>
    [Obsolete("Use the CancelPageUrl property of the PaymentArgs instance")]
    string CancelPageUrl { get; set; }

    /// <summary>
    /// Processes the callback in terms of the HttpRequest and extracts either hidden form fields, or querystring parameters
    /// that is returned from the payment provider.
    /// Determines the payment status and saves that indication for later, could be in session, in db, or other storage.
    /// This information is important and used in the PaymentStatus.
    /// </summary>
    /// <param name="request">The HttpRequest that holds all posted variables</param>
    void ProcessPaymentCallBack(HttpRequest request);

    /// <summary>
    /// Finalizes the order. This function needs to be used after the payment
    /// was done for the order.
    /// </summary>
    /// <returns>
    /// The transaction Id in which the order creation resulted
    /// </returns>
    string FinalizePayment();

    /// <summary>
    /// Cancels the order / deletes saved data
    /// </summary>
    void CancelPayment();
  }
}