// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaymentSystem.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The payment method realization.
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

namespace Sitecore.Ecommerce.Payments
{
  using System;
  using Data;
  using DomainModel.Data;
  using Validators.Interception;

  /// <summary>
  /// The payment method realization.
  /// </summary>
  [Serializable]
  public class PaymentSystem : DomainModel.Payments.PaymentSystem, IEntity
  {
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The notification option code.</value>
    [Entity(FieldName = "Code")]
    public override string Code { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The notification option title.</value>
    [Entity(FieldName = "Title")]
    public override string Title { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the payment provider description.
    /// </summary>
    /// <value>The description.</value>
    [Entity(FieldName = "Short Description")]
    public override string Description { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the payment provider logo URL.
    /// </summary>
    /// <value>The logo URL.</value>
    [Entity(FieldName = "Icon")]
    public override string LogoUrl { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is online payment method.
    /// </summary>
    /// <value><c>true</c> if this instance is online payment method; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Online Payment"), Obsolete("Check if provider is inherited from the IOnlinePaymentProvider<T>")]
    public override bool IsOnlinePayment { get; set; }

    /// <summary>
    /// Gets or sets the username for the payment provider.
    /// </summary>
    /// <value>The username.</value>
    [Entity(FieldName = "Username")]
    public override string Username { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the password for the payment provider.
    /// </summary>
    /// <value>The password.</value>
    [Entity(FieldName = "Password")]
    public override string Password { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the payment URL.
    /// </summary>
    /// <value>The payment URL.</value>
    [Entity(FieldName = "Payment Provider Url")]
    public override string PaymentUrl { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the payment secondary URL.
    /// </summary>
    /// <value>The payment secondary URL.</value>
    [Entity(FieldName = "Payment Provider Secondary Url")]
    public override string PaymentSecondaryUrl { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the payment settings.
    /// </summary>
    /// <value>The payment settings.</value>
    [Entity(FieldName = "Settings")]
    public override string PaymentSettings { get; [NotNullValue] set; }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual string Alias { get; [NotNullValue] set; }

    #endregion
  }
}