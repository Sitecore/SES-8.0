// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityMemberConverterLookupTable.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the entity member converter lookup table class.
//   Resolve entity member converter according to the entity
//   property info.
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

namespace Sitecore.Ecommerce.Data.Mapping
{
  using System;
  using System.Linq;
  using System.Reflection;
  using Converters;
  using Diagnostics;
  using DomainModel.Data;
  using Products;

  /// <summary>
  /// Defines the entity member converter lookup table class.
  /// ResolveByCustomName entity member converter according to the entity
  /// property info.
  /// </summary>
  public class EntityMemberConverterLookupTable
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityMemberConverterLookupTable" /> class.
    /// </summary>
    protected internal EntityMemberConverterLookupTable()
      : this(new BooleanEntityMemberConverter(), new ConvertibleEntityMemberConverter(), new DateTimeEntityMemberConverter(), new ProductSpecificationEntityMemberConverter(new ItemProductFactory()), new DirectMappingEntityMemberConverter())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityMemberConverterLookupTable" /> class.
    /// </summary>
    /// <param name="booleanConverter">The boolean entity member converter.</param>
    /// <param name="convertibleEntityMember">The convertible entity member converter.</param>
    /// <param name="dateTimeConverter">The date time entity member converter.</param>
    /// <param name="productSpecificationConverter">The product specification entity member converter.</param>
    /// <param name="directMappingEntityMemberConverter"> </param>
    public EntityMemberConverterLookupTable(BooleanEntityMemberConverter booleanConverter, ConvertibleEntityMemberConverter convertibleEntityMember, DateTimeEntityMemberConverter dateTimeConverter, ProductSpecificationEntityMemberConverter productSpecificationConverter, DirectMappingEntityMemberConverter directMappingEntityMemberConverter)
    {
      this.BooleanEntityMemberConverter = booleanConverter;
      this.ConvertibleEntityMemberConverter = convertibleEntityMember;
      this.DateTimeEntityMemberConverter = dateTimeConverter;
      this.ProductSpecificationEntityMemberConverter = productSpecificationConverter;
      this.DirectMappingEntityMemberConverter = directMappingEntityMemberConverter;
    }

    /// <summary>
    /// Gets the name of the mapping rule.
    /// </summary>
    /// <param name="entityProperty">The entity property.</param>
    /// <returns>
    /// The mapping rule name.
    /// </returns>
    [NotNull]
    public virtual IEntityMemberConverter GetConverter([NotNull] PropertyInfo entityProperty)
    {
      Assert.ArgumentNotNull(entityProperty, "entityProperty");

      //EntityAttribute entityAttribute = Attribute.GetCustomAttribute(entityProperty, typeof(EntityAttribute)) as EntityAttribute;
      //if (entityAttribute != null)
      //{
      //  return this.ResolveByCustomName(entityAttribute.MemberConverter);
      //}

      string customName = entityProperty.PropertyType.Name + "EntityMemberConverter";
      var converter = this.ResolveByCustomName(customName);
      if (converter != null)
      {
        return converter;
      }

      if (entityProperty.PropertyType == typeof(IConvertible) || entityProperty.PropertyType.GetInterfaces().Any(i => i == typeof(IConvertible)))
      {
        return this.ConvertibleEntityMemberConverter;
      }

      return this.DirectMappingEntityMemberConverter;
    }

    /// <summary>
    /// Gets or sets the boolean entity member converter.
    /// </summary>
    /// <value>
    /// The boolean entity member converter.
    /// </value>
    public BooleanEntityMemberConverter BooleanEntityMemberConverter { get; set; }

    /// <summary>
    /// Gets or sets the convertible entity member converter.
    /// </summary>
    /// <value>The convertible entity member converter.</value>
    public ConvertibleEntityMemberConverter ConvertibleEntityMemberConverter { get; set; }

    /// <summary>
    /// Gets or sets the date time entity member converter.
    /// </summary>
    /// <value>The date time entity member converter.</value>
    public DateTimeEntityMemberConverter DateTimeEntityMemberConverter { get; set; }

    /// <summary>
    /// Gets or sets the product specification entity member converter.
    /// </summary>
    /// <value>The product specification entity member converter.</value>
    public ProductSpecificationEntityMemberConverter ProductSpecificationEntityMemberConverter { get; set; }

    /// <summary>
    /// Gets or sets the direct mapping entity member converter.
    /// </summary>
    /// <value>The direct mapping entity member converter.</value>
    public DirectMappingEntityMemberConverter DirectMappingEntityMemberConverter { get; set; }

    /// <summary>
    /// Resolves the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>
    /// The I entity member converter.
    /// </returns>
    [CanBeNull]
    private IEntityMemberConverter ResolveByCustomName([CanBeNull]string name)
    {
      switch (name)
      {
        case "BooleanEntityMemberConverter":
          {
            return this.BooleanEntityMemberConverter;
          }

        case "DateTimeEntityMemberConverter":
          {
            return this.DateTimeEntityMemberConverter;
          }

        case "ProductSpecificationEntityMemberConverter":
          {
            return this.ProductSpecificationEntityMemberConverter;
          }
        default:
          return null;
      }
    }
  }
}