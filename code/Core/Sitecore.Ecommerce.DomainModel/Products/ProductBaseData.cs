// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductBaseData.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the product base data class.
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
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using Sitecore.Ecommerce.DomainModel.Data;

  /// <summary>
  /// Defines the product base data class.
  /// </summary>
  [Serializable]
  public class ProductBaseData : IProductRepositoryItem, ITemplatedEntity
  {

    private ProductSpecification specifications;
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductBaseData"/> class.
    /// </summary>
    public ProductBaseData()
      : this(new Collection<string>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductBaseData"/> class.
    /// </summary>
    /// <param name="specificationKeys">The specification keys.</param>
    public ProductBaseData(IEnumerable<string> specificationKeys)
    {
      this.specifications = new ProductSpecification(specificationKeys);
    }

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

    /// <summary>
    /// Gets or sets the European Article Number.
    /// </summary>
    /// <value>The European Article Number.</value>
    public virtual string EAN { get; set; }

    /// <summary>
    /// Gets or sets the Stock-Keeping Unit.
    /// </summary>
    /// <value>The Stock-Keeping Unit.</value>
    public virtual string SKU { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    public virtual string Title { get; set; }

    /// <summary>
    /// Gets or sets the specifications.
    /// </summary>
    /// <value>The specifications.</value>
    public virtual ProductSpecification Specifications
    {
      get
      {
        return this.specifications;
      }

      set
      {
        this.specifications = value;
      }
    }

    #region IEntity Members

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    string IEntity.Alias { get; set; }

    #endregion

    #region ITemplatedEntity Members

    /// <summary>
    /// Gets or sets the entity template.
    /// </summary>
    /// <value>The template.</value>
    string ITemplatedEntity.Template { get; set; }

    #endregion
  }
}