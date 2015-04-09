// -------------------------------------------------------------------------------------------
// <copyright file="ITransactionData.cs" company="Sitecore Corporation">
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
  /// <summary>
  /// The transactional data provider.
  /// </summary>
  public interface ITransactionData
  {
    /// <summary>
    /// Saves the persistent value.
    /// </summary>
    /// <param name="key">The context key.</param>
    /// <param name="value">The value.</param>
    void SavePersistentValue(string key, object value);

    /// <summary>
    /// Saves the persistent value.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="key">The context key.</param>
    /// <param name="value">The value.</param>
    void SavePersistentValue(string orderNumber, string key, object value);

    /// <summary>
    /// Saves the call back values.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="paymentStatus">The payment status.</param>
    /// <param name="transactionNumber">The transaction number.</param>
    /// <param name="finalAmount">The final amount.</param>
    /// <param name="finalCurrency">The final currency.</param>
    /// <param name="providerStatus">The provider status.</param>
    /// <param name="providerErrorCode">The provider error code.</param>
    /// <param name="providerMessage">The provider message.</param>
    /// <param name="cardType">Type of the card.</param>
    void SaveCallBackValues(string orderNumber, string paymentStatus, string transactionNumber, string finalAmount, string finalCurrency, string providerStatus, string providerErrorCode, string providerMessage, string cardType);

    /// <summary>
    /// Saves the start values.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="currency">The currency.</param>
    /// <param name="paymentMethod">The payment method.</param>
    void SaveStartValues(string orderNumber, string amount, string currency, string paymentMethod);

    /// <summary>
    /// Gets the persistent value.
    /// </summary>
    /// <param name="key">The context key.</param>
    /// <returns>The persistent value.</returns>
    object GetPersistentValue(string key);

    /// <summary>
    /// Gets the persistent value.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="key">The context key.</param>
    /// <returns>The persistent value.</returns>
    object GetPersistentValue(string orderNumber, string key);

    /// <summary>
    /// Deletes the persistent values for internal ref.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    void DeletePersistentValue(string orderNumber);
  }
}