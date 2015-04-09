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

namespace Sitecore.Ecommerce.Configurations
{
  using Data;
  using DomainModel.Data;

  /// <summary>
  /// The business catalog settings class.
  /// </summary>
  public class BusinessCatalogSettings : DomainModel.Configurations.BusinessCatalogSettings, IEntity
  {
    /// <summary>
    /// Gets or sets products link.
    /// </summary>
    [Entity(FieldName = "Products Link")]
    public override string ProductsLink { get; set; }

    /// <summary>
    /// Gets or sets order link.
    /// </summary>
    [Entity(FieldName = "Orders Link")]
    public override string OrdersLink { get; set; }

    /// <summary>
    /// Gets or sets order link.
    /// </summary>
    [Entity(FieldName = "Order Statuses Link")]
    public override string OrderStatusesLink { get; set; }

    /// <summary>
    /// Gets or sets countries link.
    /// </summary>
    [Entity(FieldName = "Countries Link")]
    public override string CountriesLink { get; set; }

    /// <summary>
    /// Gets or sets currencies link.
    /// </summary>
    [Entity(FieldName = "Currencies Link")]
    public override string CurrenciesLink { get; set; }

    /// <summary>
    /// Gets or sets the currency matrix link.
    /// </summary>
    /// <value>The currency matrix link.</value>
    [Entity(FieldName = "Currency Matrix Link")]
    public override string CurrencyMatrixLink { get; set; }

    /// <summary>
    /// Gets or sets shipping providers link.
    /// </summary>
    [Entity(FieldName = "Shipping Providers Link")]
    public override string ShippingProvidersLink { get; set; }

    /// <summary>
    /// Gets or sets notification options link.
    /// </summary>
    [Entity(FieldName = "Notification Options Link")]
    public override string NotificationOptionsLink { get; set; }

    /// <summary>
    /// Gets or sets languages link.
    /// </summary>
    [Entity(FieldName = "Languages Link")]
    public override string LanguagesLink { get; set; }

    /// <summary>
    /// Gets or sets payment methods link.
    /// </summary>
    [Entity(FieldName = "Payment Systems Link")]
    public override string PaymentSystemsLink { get; set; }

    /// <summary>
    /// Gets or sets VAT link.
    /// </summary>
    [Entity(FieldName = "VAT Link")]
    public override string VATLink { get; set; }

    /// <summary>
    /// Gets or sets Company Master Data link.
    /// </summary>
    [Entity(FieldName = "Company Master Data Link")]
    public override string CompanyMasterDataLink { get; set; }

    /// <summary>
    /// Gets or sets Company Master Data link.
    /// </summary>
    /// <value></value>
    [Entity(FieldName = "VAT Regions")]
    public override string VATRegions { get; set; }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual string Alias { get; set; }

    #endregion
  }
}