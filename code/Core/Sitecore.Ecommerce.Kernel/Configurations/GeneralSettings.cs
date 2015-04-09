// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneralSettings.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The general settings container class.
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

namespace Sitecore.Ecommerce.Configurations
{
  using Data;
  using DomainModel.Data;

  /// <summary>
  /// The general settings container class.
  /// </summary>
  public class GeneralSettings : DomainModel.Configurations.GeneralSettings, IEntity
  {
    /// <summary>
    /// Gets or sets the master currency.
    /// </summary>
    /// <value>The master currency.</value>
    [Entity(FieldName = "Master Currency")]
    public override string MasterCurrency { get; set; }

    /// <summary>
    /// Gets or sets the display currency.
    /// </summary>
    /// <value>The display currency.</value>
    [Entity(FieldName = "Display Currency")]
    public override string DisplayCurrency { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [include currency string on prices as default].
    /// </summary>
    /// <value>
    /// <c>true</c> if [include currency string on prices as default]; otherwise, <c>false</c>.
    /// </value>
    [Entity(FieldName = "Display currency on prices")]
    public override bool DisplayCurrencyOnPrices { get; set; }

    /// <summary>
    /// Gets or sets the default VA tregion.
    /// </summary>
    /// <value>The default VA tregion.</value>
    [Entity(FieldName = "Default VAT region")]
    public override string DefaultVatRegion { get; set; }

    /// <summary>
    /// Gets or sets the price format string.
    /// </summary>
    /// <value>The price format string.</value>
    [Entity(FieldName = "Price Format String")]
    public override string PriceFormatString { get; set; }

    /// <summary>
    /// Gets or sets the validation field signature.
    /// </summary>
    /// <value>The validation field signature.</value>
    [Entity(FieldName = "Validation Field Signature")]
    public override string ValidationFieldSignature { get; set; }

    /// <summary>
    /// Gets or sets the product delivery time.
    /// </summary>
    /// <value>The product delivery time.</value>
    [Entity(FieldName = "Product Delivery Time")]
    public override string ProductDeliveryTime { get; set; }

    /// <summary>
    /// Gets or sets the copyright information.
    /// </summary>
    /// <value>The copyright information.</value>
    [Entity(FieldName = "Copyright Information")]
    public override string CopyrightInformation { get; set; }

    /// <summary>
    /// Gets or sets the main login link.
    /// </summary>
    /// <value>The main login link.</value>
    [Entity(FieldName = "Main Login Link")]
    public override string MainLoginLink { get; set; }

    /// <summary>
    /// Gets or sets the password reminder link.
    /// </summary>
    /// <value>The password reminder link.</value>
    [Entity(FieldName = "Password Reminder Link")]
    public override string PasswordReminderLink { get; set; }

    /// <summary>
    /// Gets or sets the new user account link.
    /// </summary>
    /// <value>The new user account link.</value>
    [Entity(FieldName = "New User Account Link")]
    public override string NewUserAccountLink { get; set; }

    /// <summary>
    /// Gets or sets my page link.
    /// </summary>
    /// <value>My page link.</value>
    [Entity(FieldName = "My Page Link")]
    public override string MyPageLink { get; set; }

    /// <summary>
    /// Gets or sets the search page link.
    /// </summary>
    /// <value>The search page link.</value>
    [Entity(FieldName = "Search Page Link")]
    public override string SearchPageLink { get; set; }

    /// <summary>
    /// Gets or sets the mail templates link.
    /// </summary>
    /// <value>The mail templates link.</value>
    [Entity(FieldName = "Mail Templates Link")]
    public override string MailTemplatesLink { get; set; }

    /// <summary>
    /// Gets or sets the RSS news feed link.
    /// </summary>
    /// <value>The RSS news feed link.</value>
    [Entity(FieldName = "RSS News Feed Link")]
    public override string RSSNewsFeedLink { get; set; }

    /// <summary>
    /// Gets or sets the default customer roles.
    /// </summary>
    /// <value>The default customer roles.</value>
    [Entity(FieldName = "Default Customer Roles")]
    public override string DefaultCustomerRoles { get; set; }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public string Alias { get; set; }

    #endregion
  }
}