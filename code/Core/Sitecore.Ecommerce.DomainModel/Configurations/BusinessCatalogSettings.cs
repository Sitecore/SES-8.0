// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusinessCatalogSettings.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The business catalog settings class.
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

namespace Sitecore.Ecommerce.DomainModel.Configurations
{
  /// <summary>
  /// The business catalog settings class.
  /// </summary>
  public class BusinessCatalogSettings
  {
    /// <summary>
    /// Gets or sets products link.
    /// </summary>
    public virtual string ProductsLink { get; set; }

    /// <summary>
    /// Gets or sets order link.
    /// </summary>
    public virtual string OrdersLink { get; set; }

    /// <summary>
    /// Gets or sets the order statuses link.
    /// </summary>
    /// <value>The order statuses link.</value>
    public virtual string OrderStatusesLink { get; set; }

    /// <summary>
    /// Gets or sets countries link.
    /// </summary>
    public virtual string CountriesLink { get; set; }

    /// <summary>
    /// Gets or sets currencies link.
    /// </summary>
    public virtual string CurrenciesLink { get; set; }

    /// <summary>
    /// Gets or sets the currency matrix link.
    /// </summary>
    /// <value>The currency matrix link.</value>
    public virtual string CurrencyMatrixLink { get; set; }

    /// <summary>
    /// Gets or sets delivery alternatives link.
    /// </summary>
    public virtual string ShippingProvidersLink { get; set; }

    /// <summary>
    /// Gets or sets notification options link.
    /// </summary>
    public virtual string NotificationOptionsLink { get; set; }

    /// <summary>
    /// Gets or sets languages link.
    /// </summary>
    public virtual string LanguagesLink { get; set; }

    /// <summary>
    /// Gets or sets payment methods link.
    /// </summary>
    public virtual string PaymentSystemsLink { get; set; }

    /// <summary>
    /// Gets or sets VAT link.
    /// </summary>
    public virtual string VATLink { get; set; }

    /// <summary>
    /// Gets or sets Company Master Data link.
    /// </summary>
    public virtual string CompanyMasterDataLink { get; set; }

    /// <summary>
    /// Gets or sets Company Master Data link.
    /// </summary>
    public virtual string VATRegions { get; set; }
  }
}