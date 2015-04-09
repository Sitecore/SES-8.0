// -------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartSpotSettings.cs" company="Sitecore Corporation">
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
  /// The small ShoppingCart settings container abstract class .
  /// </summary>
  public class ShoppingCartSpotSettings
  {
    /// <summary>
    /// Gets or sets a value indicating whether [show ShoppingCart item lines].
    /// </summary>
    /// <value>
    /// <c>true</c> if [show ShoppingCart item lines]; otherwise, <c>false</c>.
    /// </value>
    public virtual bool ShowShoppingCartItemLines { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show total].
    /// </summary>
    /// <value><c>true</c> if [show total]; otherwise, <c>false</c>.</value>
    public virtual bool ShowTotal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show price info].
    /// </summary>
    /// <value><c>true</c> if [show price info]; otherwise, <c>false</c>.</value>
    public virtual bool ShowPriceInfo { get; set; }

    /// <summary>
    /// Gets or sets the price info.
    /// </summary>
    /// <value>The price info.</value>
    public virtual string PriceInfo { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show total inc vat].
    /// </summary>
    /// <value><c>true</c> if [show total inc vat]; otherwise, <c>false</c>.</value>
    public virtual bool ShowTotalIncVat { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show amount in ShoppingCart status line].
    /// </summary>
    /// <value>
    /// <c>true</c> if [show amount in ShoppingCart status line]; otherwise, <c>false</c>.
    /// </value>
    public virtual bool ShowAmountInShoppingCartStatusLine { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [always show ShoppingCart].
    /// </summary>
    /// <value><c>true</c> if [always show ShoppingCart]; otherwise, <c>false</c>.</value>
    public virtual bool AlwaysShowShoppingCart { get; set; }

    /// <summary>
    /// Gets or sets the price format string.
    /// </summary>
    /// <value>The price format string.</value>
    public virtual string PriceFormatString { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show delete option].
    /// </summary>
    /// <value><c>true</c> if [show delete option]; otherwise, <c>false</c>.</value>
    public virtual bool ShowDeleteOption { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show image].
    /// </summary>
    /// <value><c>true</c> if [show image]; otherwise, <c>false</c>.</value>
    public virtual bool ShowImage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [show price inc VAT].
    /// </summary>
    /// <value><c>true</c> if [show price inc VAT]; otherwise, <c>false</c>.</value>
    public virtual bool ShowPriceIncVAT { get; set; }

    /// <summary>
    /// Gets or sets the check out link.
    /// </summary>
    /// <value>The check out link.</value>
    public virtual string CheckOutLink { get; set; }

    /// <summary>
    /// Gets or sets the edit ShoppingCart link.
    /// </summary>
    /// <value>The edit ShoppingCart link.</value>
    public virtual string EditShoppingCartLink { get; set; }
  }
}