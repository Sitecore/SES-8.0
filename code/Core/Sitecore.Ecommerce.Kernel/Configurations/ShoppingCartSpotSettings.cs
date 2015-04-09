// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartSpotSettings.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The small ShoppingCart settings.
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
  /// The small ShoppingCart settings.
  /// </summary>
  public class ShoppingCartSpotSettings : DomainModel.Configurations.ShoppingCartSpotSettings, IEntity
  {
    /// <summary>
    /// Gets or sets a value indicating whether [show ShoppingCart item lines].
    /// </summary>
    /// <value><c>true</c> if [show ShoppingCart item lines]; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Show Shopping Cart Item Lines")]
    public override bool ShowShoppingCartItemLines { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show total].
    /// </summary>
    /// <value><c>true</c> if [show total]; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Show Total")]
    public override bool ShowTotal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show price info].
    /// </summary>
    /// <value><c>true</c> if [show price info]; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Show Price Info")]
    public override bool ShowPriceInfo { get; set; }

    /// <summary>
    /// Gets or sets the price info.
    /// </summary>
    /// <value>The price info.</value>
    [Entity(FieldName = "Price Info")]
    public override string PriceInfo { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show total inc vat].
    /// </summary>
    /// <value><c>true</c> if [show total inc vat]; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Show Total Inc Vat")]
    public override bool ShowTotalIncVat { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show amount in ShoppingCart status line].
    /// </summary>
    /// <value><c>true</c> if [show amount in ShoppingCart status line]; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Show Amount In Shopping Cart Status Line")]
    public override bool ShowAmountInShoppingCartStatusLine { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [always show ShoppingCart].
    /// </summary>
    /// <value><c>true</c> if [always show ShoppingCart]; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Always Show Shopping Cart")]
    public override bool AlwaysShowShoppingCart { get; set; }

    /// <summary>
    /// Gets or sets the price format string.
    /// </summary>
    /// <value>The price format string.</value>
    [Entity(FieldName = "Price Format String")]
    public override string PriceFormatString { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show delete option].
    /// </summary>
    /// <value><c>true</c> if [show delete option]; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Show Delete Option")]
    public override bool ShowDeleteOption { get; set; }

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

    /// <summary>
    /// Gets or sets the check out link.
    /// </summary>
    /// <value>The check out link.</value>
    [Entity(FieldName = "CheckOut Link")]
    public override string CheckOutLink { get; set; }

    /// <summary>
    /// Gets or sets the edit ShoppingCart link.
    /// </summary>
    /// <value>The edit ShoppingCart link.</value>
    [Entity(FieldName = "Edit Shopping Cart Link")]
    public override string EditShoppingCartLink { get; set; }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public string Alias { get; set; }

    #endregion
  }
}