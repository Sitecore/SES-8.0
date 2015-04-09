// -------------------------------------------------------------------------------------------
// <copyright file="AddressInfo.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.DomainModel.Addresses
{
  using System;

  /// <summary>
  /// The address info abstract class.
  /// </summary>
  [Serializable]
  public class AddressInfo
  {
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The names.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets the name2.
    /// </summary>
    /// <value>The name2.</value>
    public virtual string Name2 { get; set; }

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    /// <value>The address.</value>
    public virtual string Address { get; set; }

    /// <summary>
    /// Gets or sets the address2.
    /// </summary>
    /// <value>The address2.</value>
    public virtual string Address2 { get; set; }

    /// <summary>
    /// Gets or sets the zip.
    /// </summary>
    /// <value>The zip code.</value>
    public virtual string Zip { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city name.</value>
    public virtual string City { get; set; }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    /// <value>The state.</value>
    public virtual string State { get; set; }

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    /// <value>The country.</value>
    public virtual Country Country { get; set; }
  }
}