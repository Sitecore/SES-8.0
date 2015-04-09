// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOfflinePaymentProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The offline payment provider.
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
  using Carts;

  /// <summary>
  /// The offline payment provider.
  /// </summary>
  /// <typeparam name="T">The payment method type.</typeparam>
  [Obsolete("The interface is deprecated. Use DomainModel.Payments.PaymentProvider class instead.")]
  public interface IOfflinePaymentProvider<T> : IPaymentProvider<T> where T : PaymentSystem
  {
    /// <summary>
    /// Completes the payment.
    /// </summary>
    /// <typeparam name="TSC">The shopping cart type.</typeparam>
    /// <param name="shoppingCart">The shopping cart.</param>
    void ProcessPayment<TSC>(TSC shoppingCart) where TSC : ShoppingCart;
  }
}