// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Contact.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Contact class.
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

  public class Contact : IEntity
  {
    /// <summary>
    /// Identifier
    /// </summary>
    public virtual string ID { get; set; }

    /// <summary>
    /// Name for the contact
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// Telephone for the contact
    /// </summary>
    public virtual string Telephone { get; set; }

    /// <summary>
    /// Telefax for the contact
    /// </summary>
    public virtual string Telefax { get; set; }

    /// <summary>
    /// Email for the contact
    /// </summary>
    public virtual string ElectronicMail { get; set; }

    /// <summary>
    /// Freetext note for the contact
    /// </summary>
    public virtual string Note { get; set; }

    /// <summary>
    /// Information about other communication possibilities
    /// </summary>
    public virtual ICollection<Communication> OtherCommunications { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
