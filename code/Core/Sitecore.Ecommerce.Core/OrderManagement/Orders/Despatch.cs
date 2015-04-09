// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Despatch.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Despatch class.
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

namespace Sitecore.Ecommerce.OrderManagement.Orders
{
  using System;
  using Common;

  public class Despatch : IEntity
  {
    /// <summary>
    /// Identifier
    /// </summary>
    public virtual string ID { get; set; }

    /// <summary>
    /// Latest dispatch date requested by buyer
    /// </summary>
    public virtual DateTime RequestedDespatchDate { get; set; }

    /// <summary>
    /// Latest dispatch time requested by buyer
    /// </summary>
    public virtual TimeSpan RequestedDespatchTime { get; set; }

    /// <summary>
    /// Expected dispatch date estimated by seller or dispatcher
    /// </summary>
    public virtual DateTime EstimatedDespatchDate { get; set; }

    /// <summary>
    /// Expected dispatch time estimated by seller
    /// </summary>
    public virtual TimeSpan EstimatedDespatchTime { get; set; }

    /// <summary>
    /// The actual dispatch Date
    /// </summary>
    public virtual DateTime ActualDespatchDate { get; set; }

    /// <summary>
    /// The actual dispatch time
    /// </summary>
    public virtual TimeSpan ActualDespatchTime { get; set; }

    /// <summary>
    /// Address of the despatcher
    /// </summary>
    public virtual Address DespatchAddress { get; set; }

    /// <summary>
    /// The dispatching party
    /// </summary>
    public virtual Party DespatchParty { get; set; }

    /// <summary>
    /// Information about the contact
    /// </summary>
    public virtual Contact Contact { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
