// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The payment provider base class.
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
  /// The payment provider base class.
  /// </summary>
  public abstract class PaymentProvider
  {
    /// <summary>
    /// Gets or sets the payment system.
    /// </summary>
    /// <value>
    /// The payment system.
    /// </value>
    public virtual PaymentSystem PaymentOption { get; set; }

    /// <summary>
    /// Gets or sets the payment status.
    /// </summary>
    /// <value>The payment status.</value>
    public virtual PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Invokes the specified payment args.
    /// </summary>
    /// <param name="paymentArgs">The payment args.</param>
    public virtual void Invoke(PaymentArgs paymentArgs)
    {
      this.Invoke(this.PaymentOption, paymentArgs);
    }

    /// <summary>
    /// Processes the callback.
    /// </summary>
    /// <param name="paymentArgs">The payment args.</param>
    public virtual void ProcessCallback(PaymentArgs paymentArgs)
    {
      this.ProcessCallback(this.PaymentOption, paymentArgs);
    }

    /// <summary>
    /// Invokes the payment.
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="paymentArgs">The payment args.</param>
    public abstract void Invoke(PaymentSystem paymentSystem, PaymentArgs paymentArgs);

    /// <summary>
    /// Processes the callback in terms of the HttpRequest and extracts either hidden form fields, or querystring parameters
    /// that is returned from the payment provider.
    /// Determines the payment status and saves that indication for later, could be in session, in db, or other storage.
    /// This information is important and used in the PaymentStatus.
    /// </summary>
    /// <param name="paymentSystem">The payment system.</param>
    /// <param name="paymentArgs">The payment arguments.</param>
    public abstract void ProcessCallback(PaymentSystem paymentSystem, PaymentArgs paymentArgs);
  }
}