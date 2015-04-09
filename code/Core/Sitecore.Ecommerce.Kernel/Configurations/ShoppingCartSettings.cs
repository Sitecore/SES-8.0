// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartSettings.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The ShoppingCart settings container class.
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
  /// The ShoppingCart settings container class.
  /// </summary>
  public class ShoppingCartSettings : DomainModel.Configurations.ShoppingCartSettings, IEntity
  {
    /// <summary>
    /// Gets or sets the price format string.
    /// </summary>
    /// <value>The price format string.</value>
    [Entity(FieldName = "Price Format String")]
    public override string PriceFormatString { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show image].
    /// </summary>
    /// <value><c>true</c> if [show image]; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Show Image")]
    public override bool ShowImage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show price inc VAT].
    /// </summary>
    /// <value><c>true</c> if [show price inc VAT]; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Show Price Inc VAT")]
    public override bool ShowPriceIncVAT { get; set; }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public string Alias { get; set; }

    #endregion
  }
}