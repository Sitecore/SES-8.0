// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddressInfo.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The address info class.
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

namespace Sitecore.Ecommerce.Addresses
{
  using System;
  using Data;
  using DomainModel.Data;
  using Validators.Interception;

  /// <summary>
  /// The address info class.
  /// </summary>
  [Serializable]
  public class AddressInfo : DomainModel.Addresses.AddressInfo, IEntity
  {
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The adress name.</value>
    [Entity(FieldName = "Name")]
    public override string Name { get; [NotNullValue] set; }
   
    /// <summary>
    /// Gets or sets the name 2.
    /// </summary>
    /// <value>The name 2.</value>
    [Entity(FieldName = "Name 2")]
    public override string Name2 { get; [NotNullValue] set; }
 
    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    /// <value>The address.</value>
    [Entity(FieldName = "Address")]
    public override string Address { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the address 2.
    /// </summary>
    /// <value>The address 2.</value>
    [Entity(FieldName = "Address 2")]
    public override string Address2 { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the zip.
    /// </summary>
    /// <value>The zip code.</value>
    [Entity(FieldName = "Zip")]
    
    public override string Zip { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city name.</value>
    [Entity(FieldName = "City")]
    
    public override string City { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    /// <value>The state.</value>
    [Entity(FieldName = "State")]
    public override string State { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    /// <value>The country.</value>
    [Entity(FieldName = "Country")]
    
    public override DomainModel.Addresses.Country Country { get; [NotNullValue] set; }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual string Alias { get; [NotNullValue] set; }

    #endregion
  }
}