// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductStock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ProductStock type.
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

namespace Sitecore.Ecommerce.DomainModel.Products
{
  using Data;

  /// <summary>
  /// Defines the ProductStock type.
  /// </summary>
  public class ProductStock : IProductRepositoryItem, IEntity
  {
    /// <summary>
    /// Gets or sets the stock.
    /// </summary>
    /// <value>The stock.</value>
    public virtual long Stock { get; set; }

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The product category code.</value>
    public virtual string Code { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The product category title.</value>
    public virtual string Name { get; set; }

    #region IEntity Members

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>
    /// The alias.
    /// </value>
    string IEntity.Alias { get; set; }

    #endregion
  }
}