// -------------------------------------------------------------------------------------------
// <copyright file="OrderLine.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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

namespace Sitecore.Ecommerce.Orders
{
  using System;

  using DomainModel.Prices;
  using Validators.Interception;

  /// <summary>
  /// The order line abstract class.
  /// </summary>
  [Obsolete]
  public class OrderLine : DomainModel.Orders.OrderLine
  {
    /// <summary>
    /// Gets or sets the ID.
    /// </summary>
    /// <value>The product ID.</value>
    public override string Id { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The item type.</value>
    public override string Type { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the product.
    /// </summary>
    /// <value>The product.</value>
    public override DomainModel.Products.ProductBaseData Product { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the totals.
    /// </summary>
    /// <value>The totals.</value>
    public override Totals Totals { get; [NotNullValue] set; }
  }
}