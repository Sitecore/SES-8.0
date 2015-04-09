// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyTaxScheme.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the PartyTaxScheme class.
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
  using OrderManagement.Orders;

  public class PartyTaxScheme : IEntity
  {
    /// <summary>
    /// Name of the party
    /// </summary>
    public virtual string RegistrationName { get; set; }

    /// <summary>
    /// Company identifier
    /// </summary>
    public virtual string CompanyID { get; set; }

    /// <summary>
    /// Code for exemption of tax
    /// </summary>
    public virtual string ExemptionReasonCode { get; set; }

    /// <summary>
    /// Text for exemption of tax
    /// </summary>
    public virtual string ExemptionReason { get; set; }

    /// <summary>
    /// Information about the address of the party
    /// </summary>
    public virtual Address RegistrationAddress { get; set; }

    /// <summary>
    /// Information about the Tax scheme
    /// </summary>
    public virtual TaxScheme TaxScheme { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
