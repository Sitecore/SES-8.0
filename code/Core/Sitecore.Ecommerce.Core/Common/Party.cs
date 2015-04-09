// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Party.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Party class.
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
  using System.Collections.Generic;

  public class Party : IEntity
  {
    /// <summary>
    /// URI for the party
    /// </summary>
    public virtual string WebsiteURI { get; set; }

    /// <summary>
    /// Reference to the logo of the party
    /// </summary>
    public virtual string LogoReferenceID { get; set; }

    /// <summary>
    /// The identifier of the party endpoint ( EAN
    /// </summary>
    public virtual string EndpointID { get; set; }

    /// <summary>
    /// Information about the party identifier
    /// </summary>
    public virtual string PartyIdentification { get; set; }

    /// <summary>
    /// Information about the party
    /// </summary>
    public virtual string PartyName { get; set; }

    /// <summary>
    /// Information about the party language
    /// </summary>
    public virtual string LanguageCode { get; set; }

    /// <summary>
    /// Information about the party address
    /// </summary>
    public virtual Address PostalAddress { get; set; }

    /// <summary>
    /// Information about the party location
    /// </summary>
    public virtual Location PhysicalLocation { get; set; }

    /// <summary>
    /// Information about the taxscheme for the party
    /// </summary>
    public virtual ICollection<PartyTaxScheme> PartyTaxScheme { get; set; }

    /// <summary>
    /// Information about legal information for the party
    /// </summary>
    public virtual PartyLegalEntity PartyLegalEntity { get; set; }

    /// <summary>
    /// Information about the contact of the party
    /// </summary>
    public virtual Contact Contact { get; set; }

    /// <summary>
    /// Information about the persons of the party
    /// </summary>
    public virtual Person Person { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
