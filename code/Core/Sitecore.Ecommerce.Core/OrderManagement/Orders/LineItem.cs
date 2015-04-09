// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineItem.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the LineItem type.
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
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using Common;

  /// <summary>
  /// Defines the LineItem type.
  /// </summary>
  public class LineItem : IEntity
  {
    /// <summary>
    /// Sublines field.
    /// </summary>
    private ICollection<LineItem> subLines = new Collection<LineItem>();

    /// <summary>
    /// Internal identifier
    /// </summary>
    public virtual string ID { get; set; }

    /// <summary>
    /// The merchant Order ID
    /// </summary>
    public virtual string OrderID { get; set; }

    /// <summary>
    /// Freetext field
    /// </summary>
    public virtual string Note { get; set; }

    /// <summary>
    /// Status for the order line
    /// </summary>
    public virtual string LineStatusCode { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public virtual decimal Quantity { get; set; }

    /// <summary>
    /// Amount for the line
    /// </summary>
    public virtual Amount LineExtensionAmount { get; set; }

    /// <summary>
    /// Tax total on the orderline
    /// </summary>
    public virtual Amount TotalTaxAmount { get; set; }

    /// <summary>
    /// Minimum Quantity for ordering
    /// </summary>
    public virtual decimal MinimumQuantity { get; set; }

    /// <summary>
    /// Maximum quantity for ordering
    /// </summary>
    public virtual decimal MaximumQuantity { get; set; }

    /// <summary>
    /// Is Partial delivery allowed
    /// </summary>
    public virtual bool PartialDeliveryIndicator { get; set; }

    /// <summary>
    /// Information about delivery
    /// </summary>
    public virtual ICollection<Delivery> Delivery { get; set; }

    /// <summary>
    /// Information about order shipping
    /// </summary>
    public virtual OrderedShipment OrderedShipment { get; set; }

    /// <summary>
    /// Information about the item on the orderline
    /// </summary>
    public virtual Item Item { get; set; }

    /// <summary>
    /// Gets or sets the sub line item.
    /// </summary>
    /// <value>
    /// The sub line item.
    /// </value>
    public virtual ICollection<LineItem> SubLineItem
    {
      get { return this.subLines; }
      set { this.subLines = value; }
    }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>The price.</value>
    public virtual Price Price { get; set; }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual long Alias { get; set; }

    /// <summary>
    /// Gets or sets the order line.
    /// </summary>
    /// <value>The order line.</value>
    public virtual OrderLine OrderLine { get; set; }

    /// <summary>
    /// Gets or sets the parent line.
    /// </summary>
    /// <value>The parent line.</value>
    public virtual LineItem ParentLine { get; set; }

    /// <summary>
    /// Gets or sets the substates.
    /// </summary>
    /// <value>The substates.</value>
    public virtual ICollection<Substate> Substates { get; set; }
  }
}
