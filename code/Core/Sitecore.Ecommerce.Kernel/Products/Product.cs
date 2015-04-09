// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Product entity.
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

namespace Sitecore.Ecommerce.Products
{
  using System;
  using System.Collections.Generic;
  using Data;
  using DomainModel.Data;
  using Validators.Interception;

  /// <summary>
  /// Product entity.
  /// </summary>
  [Serializable, Entity(TemplateId = "{B87EFAE7-D3D5-4E07-A6FC-012AAA13A6CF}")]
  public class Product : DomainModel.Products.ProductBaseData, IEntity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    public Product()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="specificationKeys">The specification keys.</param>
    public Product(IEnumerable<string> specificationKeys)
      : base(specificationKeys)
    {
    }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The product category title.</value>
    [Entity(FieldName = "Name")]
    public override string Name { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The product code.</value>
    [Entity(FieldName = "Product Code")]
    public override string Code { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the EAN.
    /// </summary>
    /// <value>The EAN.</value>
    [Entity(FieldName = "EAN")]
    public override string EAN { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the SKU.
    /// </summary>
    /// <value>The SKU.</value>
    [Entity(FieldName = "SKU")]
    public override string SKU { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    [Entity(FieldName = "Title")]
    public override string Title { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the short description.
    /// </summary>
    /// <value>The short description.</value>
    [Entity(FieldName = "Short Description")]
    public virtual string ShortDescription { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [Entity(FieldName = "Description")]
    public virtual string Description { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the image URL.
    /// </summary>
    /// <value>The image URL.</value>
    [Entity(FieldName = "Images")]
    public virtual string ImageUrl { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the brand.
    /// </summary>
    /// <value>The brand.</value>
    [Entity(FieldName = "Brand")]
    public virtual string Brand { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the type of the vat.
    /// </summary>
    /// <value>The type of the vat.</value>
    [Entity(FieldName = "VAT")]
    public virtual string VatType { get; [NotNullValue] set; }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual string Alias { get; [NotNullValue] set; }

    #endregion
  }
}