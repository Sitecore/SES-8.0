// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductSpecificationEntityMemberConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the product specification entity member converter class.
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

namespace Sitecore.Ecommerce.Data.Mapping.Converters
{
  using System;
  using System.Linq;
  using Diagnostics;
  using DomainModel.Data;
  using DomainModel.Products;
  using Sitecore.Data.Items;
  using Validators.Interception;

  /// <summary>
  /// Defines the product specification entity member converter class.
  /// </summary>
  public class ProductSpecificationEntityMemberConverter : EntityMemberConverter<ProductSpecification, Item>, IRequiresStorageObject<Item>
  {
    /// <summary>
    /// The product factory.
    /// </summary>
    private readonly ProductFactory productFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductSpecificationEntityMemberConverter" /> class.
    /// </summary>
    /// <param name="productFactory">The product factory.</param>
    public ProductSpecificationEntityMemberConverter(ProductFactory productFactory)
    {
      this.productFactory = productFactory;
    }

    /// <summary>
    /// Gets or sets the storage object.
    /// </summary>
    /// <value>
    /// The storage object.
    /// </value>
    public Item StorageObject { get; [NotNullValue]set; }

    /// <summary>
    /// Gets the product factory.
    /// </summary>
    /// <value>The product factory.</value>
    public ProductFactory ProductFactory
    {
      get { return this.productFactory; }
    }

    /// <summary>
    /// Converts from item to product specification.
    /// </summary>
    /// <param name="storage">The storage.</param>
    /// <returns>
    /// The product specification.
    /// </returns>
    [NotNull]
    public override ProductSpecification ConvertFrom([NotNull] Item storage)
    {
      Assert.ArgumentNotNull(storage, "storage");

      ProductBaseData product = this.productFactory.Create(storage.TemplateID.ToString());
      ProductSpecification specification = product.Specifications;

      foreach (string key in specification.Keys.ToArray())
      {
        specification[key] = storage[key];
      }

      return specification;
    }

    /// <summary>
    /// Converts the <see cref="ProductSpecificationEntityMemberConverter"/> to a <see cref="Item"/>.
    /// </summary>
    /// <param name="entityMember">The entity member.</param>
    /// <returns>
    /// The <see cref="Item"/>.
    /// </returns>
    [NotNull]
    public override Item ToStorage([NotNull] ProductSpecification entityMember)
    {
      Assert.ArgumentNotNull(entityMember, "entityMember");

      throw new NotImplementedException();
    }
  }
}