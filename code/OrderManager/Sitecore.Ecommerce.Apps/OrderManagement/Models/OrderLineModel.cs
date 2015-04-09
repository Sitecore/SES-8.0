// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineModel.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order line model class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Models
{
  /// <summary>
  /// Defines the order line model class.
  /// </summary>
  public class OrderLineModel
  {
    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>
    /// The alias.
    /// </value>
    public long Alias { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the product code.
    /// </summary>
    /// <value>
    /// The product code.
    /// </value>
    public virtual string ProductCode { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    /// <value>
    /// The name of the product.
    /// </value>
    public virtual string ProductName { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>
    /// The quantity.
    /// </value>
    public virtual long Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    /// <value>
    /// The unit price.
    /// </value>
    public virtual decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the unit price formatted.
    /// </summary>
    /// <value>
    /// The unit price formatted.
    /// </value>
    public virtual string UnitPriceFormatted { get; set; }
  }
}