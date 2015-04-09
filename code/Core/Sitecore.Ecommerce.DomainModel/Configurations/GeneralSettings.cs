// -------------------------------------------------------------------------------------------
// <copyright file="GeneralSettings.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.DomainModel.Configurations
{
  /// <summary>
  /// The general settings container abstract class.
  /// </summary>
  public class GeneralSettings
  {
    /// <summary>
    /// Gets or sets the master currency.
    /// </summary>
    /// <value>The master currency.</value>
    public virtual string MasterCurrency { get; set; }

    /// <summary>
    /// Gets or sets the display currency.
    /// </summary>
    /// <value>The display currency.</value>
    public virtual string DisplayCurrency { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [include currency string on prices as default].
    /// </summary>
    /// <value>
    /// <c>true</c> if [include currency string on prices as default]; otherwise, <c>false</c>.
    /// </value>
    public virtual bool DisplayCurrencyOnPrices { get; set; }

    /// <summary>
    /// Gets or sets the default VA tregion.
    /// </summary>
    /// <value>The default VA tregion.</value>
    public virtual string DefaultVatRegion { get; set; }
    
    /// <summary>
    /// Gets or sets the price format string.
    /// </summary>
    /// <value>The price format string.</value>
    public virtual string PriceFormatString { get; set; }

    /// <summary>
    /// Gets or sets the validation field signature.
    /// </summary>
    /// <value>The validation field signature.</value>
    public virtual string ValidationFieldSignature { get; set; }

    /// <summary>
    /// Gets or sets the product delivery time.
    /// </summary>
    /// <value>The product delivery time.</value>
    public virtual string ProductDeliveryTime { get; set; }

    /// <summary>
    /// Gets or sets the copyright information.
    /// </summary>
    /// <value>The copyright information.</value>
    public virtual string CopyrightInformation { get; set; }

    /// <summary>
    /// Gets or sets the main login link.
    /// </summary>
    /// <value>The main login link.</value>
    public virtual string MainLoginLink { get; set; }

    /// <summary>
    /// Gets or sets the password reminder link.
    /// </summary>
    /// <value>The password reminder link.</value>
    public virtual string PasswordReminderLink { get; set; }

    /// <summary>
    /// Gets or sets the new user account link.
    /// </summary>
    /// <value>The new user account link.</value>
    public virtual string NewUserAccountLink { get; set; }

    /// <summary>
    /// Gets or sets my page link.
    /// </summary>
    /// <value>My page link.</value>
    public virtual string MyPageLink { get; set; }

    /// <summary>
    /// Gets or sets the search page link.
    /// </summary>
    /// <value>The search page link.</value>
    public virtual string SearchPageLink { get; set; }

    /// <summary>
    /// Gets or sets the mail templates link.
    /// </summary>
    /// <value>The mail templates link.</value>
    public virtual string MailTemplatesLink { get; set; }

    /// <summary>
    /// Gets or sets the RSS news feed link.
    /// </summary>
    /// <value>The RSS news feed link.</value>
    public virtual string RSSNewsFeedLink { get; set; }

    /// <summary>
    /// Gets or sets the default customer roles.
    /// </summary>
    /// <value>The default customer roles.</value>
    public virtual string DefaultCustomerRoles { get; set; }
  }
}