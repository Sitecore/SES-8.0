// -------------------------------------------------------------------------------------------
// <copyright file="PaymentSystem.cs" company="Sitecore Corporation">
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

  /// <summary>
  /// The payment method  abstract class.
  /// </summary>
  [Serializable]
  public class PaymentSystem : IComparable
  {
    /// <summary>
    /// Gets or sets the payment method code.
    /// </summary>
    /// <value>The payment method code.</value>
    public virtual string Code { get; set; }

    /// <summary>
    /// Gets or sets the payment method title.
    /// </summary>
    /// <value>The payment method title.</value>
    public virtual string Title { get; set; }

    /// <summary>
    /// Gets or sets the payment method description.
    /// </summary>
    /// <value>The description.</value>
    public virtual string Description { get; set; }

    /// <summary>
    /// Gets or sets the payment method logo URL.
    /// </summary>
    /// <value>The logo URL.</value>
    public virtual string LogoUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is online payment method.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is online payment method; otherwise, <c>false</c>.
    /// </value>
    [Obsolete("Check if provider is inherited from the IOnlinePaymentProvider<T>")]
    public virtual bool IsOnlinePayment { get; set; }

    /// <summary>
    /// Gets or sets the username for the payment method.
    /// </summary>
    /// <value>The username.</value>
    public virtual string Username { get; set; }

    /// <summary>
    /// Gets or sets the password for the payment method.
    /// </summary>
    /// <value>The password.</value>
    public virtual string Password { get; set; }

    /// <summary>
    /// Gets or sets the payment URL.
    /// </summary>
    /// <value>The payment URL.</value>
    public virtual string PaymentUrl { get; set; }

    /// <summary>
    /// Gets or sets the payment secondary URL.
    /// </summary>
    /// <value>The payment secondary URL.</value>
    public virtual string PaymentSecondaryUrl { get; set; }

    /// <summary>
    /// Gets or sets the payment settings.
    /// </summary>
    /// <value>The payment settings.</value>
    public virtual string PaymentSettings { get; set; }

    /// <summary>Compares to instances.</summary>
    /// <param name="obj">The obj.</param>
    /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
    /// <returns>The result of the comparison of the Type names.</returns>
    int IComparable.CompareTo(object obj)
    {
      PaymentSystem otherPaymentSystem = obj as PaymentSystem;
      if (otherPaymentSystem != null)
      {
        return this.Code.CompareTo(otherPaymentSystem.Code);
      }

      throw new ArgumentException("Object is not a PaymentSystem");
    }
  }
}