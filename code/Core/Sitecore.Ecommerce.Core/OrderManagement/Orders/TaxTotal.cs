// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaxTotal.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the tax total class.
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
  using Sitecore.Diagnostics;

  /// <summary>
  /// Defines the tax total class.
  /// </summary>
  public class TaxTotal : IEntity
  {
    /// <summary>
    /// The tax subtotal.
    /// </summary>
    private ICollection<TaxSubTotal> taxSubtotal;

    /// <summary>
    /// Gets or sets the tax amount.
    /// </summary>
    /// <value>The tax amount.</value>
    [NotNull]
    public virtual Amount TaxAmount { get; set; }

    /// <summary>
    /// Gets or sets the rounding amount.
    /// </summary>
    [NotNull]
    public virtual Amount RoundingAmount { get; set; }

    /// <summary>
    /// Gets or sets the information about tax subtotal for the order.
    /// </summary>
    [NotNull]
    public virtual ICollection<TaxSubTotal> TaxSubtotal
    {
      get
      {
        return this.taxSubtotal = this.taxSubtotal ?? new Collection<TaxSubTotal>();
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.taxSubtotal = value;
      }
    }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual long Alias { get; set; }
  }
}