// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delivery.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Delivery class.
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

  public class Delivery : IEntity
  {
    /// <summary>
    /// Quantity in delivery
    /// </summary>
    public virtual decimal Quantity { get; set; }

    /// <summary>
    /// Minimum quantity in delivery
    /// </summary>
    public virtual decimal MinimumQuantity { get; set; }

    /// <summary>
    /// Maximum quantity in Delivery
    /// </summary>
    public virtual decimal MaximumQuantity { get; set; }

    /// <summary>
    /// Latest possible date for delivery given by buyer
    /// </summary>
    public virtual DateTime LatestDeliveryDate { get; set; }

    /// <summary>
    /// Latest possible delivery time given by buyer
    /// </summary>
    public virtual TimeSpan LatestDeliveryTime { get; set; }

    /// <summary>
    /// Delivery tracking ID
    /// </summary>
    public virtual string TrackingID { get; set; }

    /// <summary>
    /// Information about delivery location
    /// </summary>
    public virtual Location DeliveryLocation { get; set; }

    /// <summary>
    /// Information about the delivery address
    /// </summary>
    public virtual Address DeliveryAdress { get; set; }

    /// <summary>
    /// Alternative Delivery location
    /// </summary>
    public virtual Location AlternativeDeliveryLocation { get; set; }

    /// <summary>
    /// The wanted delivery period
    /// </summary>
    public virtual Period RequestedDeliveryPeriod { get; set; }

    /// <summary>
    /// The party the delivery is for
    /// </summary>
    public virtual Party DeliveryParty { get; set; }

    /// <summary>
    /// Information about the sender of the delivery
    /// </summary>
    public virtual Despatch Despatch { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
