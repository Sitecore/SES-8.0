// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShippingProvider.cs"  company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The shipping provider implementation.
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

namespace Sitecore.Ecommerce.Shippings
{
  using System;
  using Data;
  using DomainModel.Data;
  using Validators.Interception;

  /// <summary>
  /// The shipping provider implementation.
  /// </summary>
  [Serializable]
  public class ShippingProvider : DomainModel.Shippings.ShippingProvider, IEntity
  {
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The notification option code.</value>
    [Entity(FieldName = "Code")]
    public override string Code { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The notification option name.</value>
    [Entity(FieldName = "Name")]
    public override string Name { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The notification option title.</value>
    [Entity(FieldName = "Title")]
    public override string Title { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>The shipping method price.</value>
    [Entity(FieldName = "Price")]
    public override decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the short description.
    /// </summary>
    /// <value>The short description.</value>
    [Entity(FieldName = "Short Description")]
    public override string Description { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the icon URL.
    /// </summary>
    /// <value>The icon URL.</value>
    [Entity(FieldName = "Icon")]
    public override string IconUrl { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the available notification options.
    /// </summary>
    /// <value>The available notification options.</value>
    [Entity(FieldName = "Available Notification Options")]
    public override string AvailableNotificationOptions { get; [NotNullValue] set; }
   
    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual string Alias { get; [NotNullValue] set; }

    #endregion
  }
}