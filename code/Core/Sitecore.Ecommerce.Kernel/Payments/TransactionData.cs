// -------------------------------------------------------------------------------------------
// <copyright file="TransactionData.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Payments
{
  using System;
  using System.Web;
  using DomainModel.Payments;
  using Text;

  /// <summary>
  /// The transaction session data provider.
  /// </summary>
  public class TransactionData : ITransactionData
  {
    /// <summary>
    /// The session key prefix.
    /// </summary>
    private static readonly string sessionKeyPrefix = "SE";

    /// <summary>
    /// Saves the persistent value.
    /// </summary>
    /// <param name="key">The context key.</param>
    /// <param name="value">The value.</param>
    public virtual void SavePersistentValue(string key, object value)
    {
      HttpContext.Current.Session[GetSessionKey(key)] = value;
    }

    /// <summary>
    /// Saves the persistent value.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="key">The context key.</param>
    /// <param name="value">The value.</param>
    public virtual void SavePersistentValue(string orderNumber, string key, object value)
    {
      this.SavePersistentValue(new ListString { orderNumber, key }.ToString(), value);
    }

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
    public virtual void SaveCallBackValues(string orderNumber, string paymentStatus, string transactionNumber, string finalAmount, string finalCurrency, string providerStatus, string providerErrorCode, string providerMessage, string cardType)
    {
      PaymentStatus paymentStatusValue = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), paymentStatus);
      this.SavePersistentValue(orderNumber, TransactionConstants.PaymentStatus, paymentStatusValue);
      this.SavePersistentValue(orderNumber, TransactionConstants.TransactionNumber, transactionNumber);
      this.SavePersistentValue(orderNumber, TransactionConstants.FinalAmount, finalAmount);
      this.SavePersistentValue(orderNumber, TransactionConstants.FinalCurrency, finalCurrency);
      this.SavePersistentValue(orderNumber, TransactionConstants.PaymentStatus, providerStatus);
      this.SavePersistentValue(orderNumber, TransactionConstants.ProviderErrorCode, providerErrorCode);
      this.SavePersistentValue(orderNumber, TransactionConstants.ProviderMessage, providerMessage);
      this.SavePersistentValue(orderNumber, TransactionConstants.CardType, cardType);
    }

    /// <summary>
    /// Saves the start values.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="currency">The currency.</param>
    /// <param name="paymentMethod">The payment method.</param>
    public virtual void SaveStartValues(string orderNumber, string amount, string currency, string paymentMethod)
    {
      this.SavePersistentValue(orderNumber, TransactionConstants.TotalAmount, amount);
      this.SavePersistentValue(orderNumber, TransactionConstants.Currency, currency);
      this.SavePersistentValue(orderNumber, TransactionConstants.PaymentSystemCode, paymentMethod);
    }

    /// <summary>
    /// Gets the persistent value.
    /// </summary>
    /// <param name="key">The context key.</param>
    /// <returns>The persistent value.</returns>
    public virtual object GetPersistentValue(string key)
    {
      if (string.IsNullOrEmpty(key))
      {
        return null;
      }

      if (HttpContext.Current != null && HttpContext.Current.Session != null)
      {
        return HttpContext.Current.Session[GetSessionKey(key)];
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets the persistent value.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    /// <param name="key">The context key.</param>
    /// <returns>The persistent value.</returns>
    public virtual object GetPersistentValue(string orderNumber, string key)
    {
      if (string.IsNullOrEmpty(orderNumber) || string.IsNullOrEmpty(key))
      {
        return null;
      }

      return this.GetPersistentValue(new ListString { orderNumber, key }.ToString());
    }

    /// <summary>
    /// Deletes the persistent values for internal ref.
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    public virtual void DeletePersistentValue(string orderNumber)
    {
      HttpContext.Current.Session[orderNumber] = string.Empty;
    }

    /// <summary>
    /// Gets the session key.
    /// </summary>
    /// <param name="key">The context key.</param>
    /// <returns>The session key.</returns>
    private static string GetSessionKey(string key)
    {
      return new ListString { sessionKeyPrefix, Sitecore.Context.Site.Name, key }.ToString();
    }
  }
}