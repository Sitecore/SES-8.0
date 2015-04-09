// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Location.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Location class.
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
  public class Location : IEntity
  {
    /// <summary>
    /// Identifier of the location
    /// </summary>
    public virtual string ID { get; set; }

    /// <summary>
    /// Text for description
    /// </summary>
    public virtual string Description { get; set; }

    /// <summary>
    /// Text for sub entity ( county
    /// </summary>
    public virtual string CountrySubentity { get; set; }

    /// <summary>
    /// Code for sub entity ( county
    /// </summary>
    public virtual string CountrySubEntityCode { get; set; }

    /// <summary>
    /// Information about the validity period
    /// </summary>
    public virtual Period ValidityPeriod { get; set; }

    /// <summary>
    /// Information about the Address
    /// </summary>
    public virtual Address Address { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
