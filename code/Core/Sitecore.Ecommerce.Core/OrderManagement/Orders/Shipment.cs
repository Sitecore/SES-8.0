// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Shipment.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The shipment.
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
  using Common;

  /// <summary>
  /// The shipment.
  /// </summary>
  public class Shipment : IEntity
  {
    public virtual long Alias { get; protected set; }

    public virtual string ShippingPriorityLevelCode { get; set; }
    public virtual string HandlingCode { get; set; }
    public virtual string HandlingInstructions { get; set; }
    public virtual string Information { get; set; }
    public virtual Measure GrossWeightMeasure { get; set; }
    public virtual Measure NetWeightMeasure { get; set; }
    public virtual Measure NetNetWeightMeasure { get; set; }
    public virtual Measure GrossVolumeMeasure { get; set; }
    public virtual Measure NetVolumeMeasure { get; set; }
    public virtual decimal TotalGoodsItemQuantity { get; set; }
    public virtual decimal TotalTransportHandlingUnitQuantity { get; set; }

    public virtual Amount InsuranceValueAmount { get; set; }

    public virtual Amount DeclaredCustomsValueAmount { get; set; }

    public virtual Amount DeclaredForCarriageValueAmount { get; set; }

    public virtual Amount DeclaredStatisticsValueAmount { get; set; }

    /// <summary>
    /// Gets or sets the free on board value amount.
    /// </summary>
    public virtual Amount FreeOnBoardValueAmount { get; set; }

    public virtual string SpecialInstructions { get; set; }
    public virtual string DeliveryInstructions { get; set; }
    public virtual bool SplitConsignmentIndicator { get; set; }
    public virtual string ExportCountry { get; set; }

    public virtual Delivery Delivery { get; set; }
    public virtual Address OriginAddress { get; set; }
    public virtual Location FirstArrivalPortLocation { get; set; }
    public virtual Location LastExitPortLocation { get; set; }
    public virtual AllowanceCharge FreightAllowanceCharge { get; set; }
  }
}