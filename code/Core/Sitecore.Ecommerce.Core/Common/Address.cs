// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Address.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Address class.
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

namespace Sitecore.Ecommerce.Common
{
  public class Address : IEntity
  {
    public virtual string ID { get; set; }

    /// <summary>
    /// Code for the address type ( firm/private
    /// </summary>
    public virtual string AddressTypeCode { get; set; }

    /// <summary>
    /// The code for the format of the current Address ( UN/CEFACT
    /// </summary>
    public virtual string AddressFormatCode { get; set; }

    /// <summary>
    /// Text for postbox
    /// </summary>
    public virtual string PostBox { get; set; }

    /// <summary>
    /// Text for Floor
    /// </summary>
    public virtual string Floor { get; set; }

    /// <summary>
    /// Text for Room
    /// </summary>
    public virtual string Room { get; set; }

    /// <summary>
    /// Name of the street
    /// </summary>
    public virtual string StreetName { get; set; }

    /// <summary>
    /// Additional text for streetname
    /// </summary>
    public virtual string AdditionalStreetName { get; set; }

    /// <summary>
    /// Building name
    /// </summary>
    public virtual string BuildingName { get; set; }

    /// <summary>
    /// Text for the number of the building
    /// </summary>
    public virtual string BuildingNumber { get; set; }

    /// <summary>
    /// Text for inhouse mail
    /// </summary>
    public virtual string InhouseMail { get; set; }

    /// <summary>
    /// Text for department
    /// </summary>
    public virtual string Department { get; set; }

    /// <summary>
    /// Text for attention
    /// </summary>
    public virtual string MarkAttention { get; set; }

    /// <summary>
    /// Text for care ( C/O
    /// </summary>
    public virtual string MarkCare { get; set; }

    /// <summary>
    /// Text for identifier for the plot where address is located
    /// </summary>
    public virtual string PlotIdentification { get; set; }

    /// <summary>
    /// Text for city sub divition
    /// </summary>
    public virtual string CitySubdivisionName { get; set; }

    /// <summary>
    /// Name of the city
    /// </summary>
    public virtual string CityName { get; set; }

    /// <summary>
    /// Postal code
    /// </summary>
    public virtual string PostalZone { get; set; }

    /// <summary>
    /// Sub part of the country ( county or something like that
    /// </summary>
    public virtual string CountrySubentity { get; set; }

    /// <summary>
    /// Code for the sub part of the country
    /// </summary>
    public virtual string CountrySubentityCode { get; set; }

    /// <summary>
    /// Text for the region
    /// </summary>
    public virtual string Region { get; set; }

    /// <summary>
    /// Text for the district
    /// </summary>
    public virtual string District { get; set; }

    /// <summary>
    /// Information about the addressline
    /// </summary>
    public virtual string AddressLine { get; set; }

    /// <summary>
    /// Information about the country type
    /// </summary>
    public virtual string Country { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
