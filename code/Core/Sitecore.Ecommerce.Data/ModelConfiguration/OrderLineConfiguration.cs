// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineConfiguration.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineConfiguration class.
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

namespace Sitecore.Ecommerce.Data.ModelConfiguration
{
  using System.Data.Entity.ModelConfiguration;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the order line configuration class.
  /// </summary>
  public class OrderLineConfiguration : EntityTypeConfiguration<OrderLine>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderLineConfiguration"/> class.
    /// </summary>
    public OrderLineConfiguration()
    {
      this.HasKey(ol => ol.Alias);
      this.HasRequired(ol => ol.LineItem).WithOptional(li => li.OrderLine).WillCascadeOnDelete();
    }
  }
}