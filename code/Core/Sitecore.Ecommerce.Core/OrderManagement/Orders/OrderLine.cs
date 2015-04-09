// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLine.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLine class.
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
  using Common;

  public class OrderLine : IEntity
  {
    /// <summary>
    /// Freetext note field for the order line
    /// </summary>
    public virtual string Note { get; set; }

    /// <summary>
    /// Information about the order line
    /// </summary>
    public virtual LineItem LineItem { get; set; }

    /// <summary>
    /// Possible alternative items proposed by Seller
    /// </summary>
    public virtual ICollection<LineItem> SellerProposedSubstituteLineItem { get; set; }

    /// <summary>
    /// Actual alternative items given by seller
    /// </summary>
    public virtual ICollection<LineItem> SellerSubstitutedLineItem { get; set; }

    /// <summary>
    /// Possible alternative items proposed by Buyer
    /// </summary>
    public virtual ICollection<LineItem> BuyerProposedSubstituteLineItem { get; set; }

    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>The order.</value>
    public virtual Order Order { get; set; }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual long Alias { get; protected set; }
  }
}
