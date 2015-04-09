// -------------------------------------------------------------------------------------------
// <copyright file="TransactionConstants.cs" company="Sitecore Corporation">
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
  /// <summary>
  /// The transaction constants 
  /// </summary>
  public static class TransactionConstants
  {
    /// <summary>
    /// The payment Status.
    /// </summary>
    public static readonly string PaymentStatus = "PaymentStatus";

    /// <summary>
    /// The transaction number.
    /// </summary>
    public static readonly string TransactionNumber = "TransactionNumber";

    /// <summary>
    /// The total amount.
    /// </summary>
    public static readonly string TotalAmount = "TotalAmount";

    /// <summary>
    /// The currency.
    /// </summary>
    public static readonly string Currency = "Currency";

    /// <summary>
    /// The payment method code.
    /// </summary>
    public static readonly string PaymentSystemCode = "PaymentSystemCode";

    /// <summary>
    /// The final ammount.
    /// </summary>
    public static readonly string FinalAmount = "FinalAmount";

    /// <summary>
    /// The final currency.
    /// </summary>
    public static readonly string FinalCurrency = "FinalCurrency";

    /// <summary>
    /// The provider status.
    /// </summary>
    public static readonly string ProviderStatus = "ProviderStatus";

    /// <summary>
    /// The provider error code.
    /// </summary>
    public static readonly string ProviderErrorCode = "ProviderErrorCode";

    /// <summary>
    /// The provider message.
    /// </summary>
    public static readonly string ProviderMessage = "ProviderMessage";

    /// <summary>
    /// The card type/
    /// </summary>
    public static readonly string CardType = "CardType";

    /// <summary>
    /// The username.
    /// </summary>
    public static readonly string Username = "Username";
  }
}