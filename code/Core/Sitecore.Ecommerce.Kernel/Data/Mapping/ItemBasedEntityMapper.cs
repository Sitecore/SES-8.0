// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemBasedEntityMapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the item based entity mapper class.
//   Maps items to entities and vice-versa.
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
  using System.Linq;
  using System.Reflection;
  using Diagnostics;
  using DomainModel.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// Defines the item based entity mapper class.
  /// Maps items to entities and vice-versa.
  /// </summary>
  /// <typeparam name="TSource">The type of the source.</typeparam>
  /// <typeparam name="TTarget">The type of the target.</typeparam>
  public abstract class ItemBasedEntityMapper<TSource, TTarget> : EntityMapper<TSource, TTarget>
  {
    /// <summary>
    /// The item field matcher.
    /// </summary>
    private readonly MappingItemMatcher mappingItemMatcher;

    /// <summary>
    /// The mappings.
    /// </summary>
    private readonly EntityMemberConverterLookupTable mappings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemBasedEntityMapper{TTarget}" /> class.
    /// </summary>
    protected ItemBasedEntityMapper()
      : this(new MappingItemMatcher(), new EntityMemberConverterLookupTable())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see><cref>ItemBasedEntityMapper{TTarget}</cref></see> class.
    /// </summary>
    /// <param name="mappingItemMatcher">The mapping item matcher.</param>
    /// <param name="mappings">The mappings.</param>
    protected ItemBasedEntityMapper(MappingItemMatcher mappingItemMatcher, EntityMemberConverterLookupTable mappings)
    {
      Assert.IsNotNull(mappingItemMatcher, "mappingItemMatcher");
      Assert.IsNotNull(mappings, "mappings");

      this.mappingItemMatcher = mappingItemMatcher;
      this.mappings = mappings;
    }

    /// <summary>
    /// Gets or sets the mappings.
    /// </summary>
    /// <value>The mappings.</value>
    public EntityMemberConverterLookupTable Mappings
    {
      get { return mappings; }
    }

    /// <summary>
    /// Gets the mapping field.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="entityProperty">The entity property.</param>
    /// <returns>
    /// The mapping field.
    /// </returns>
    [CanBeNull]
    protected object GetMappingItem([NotNull] Item item, [NotNull] PropertyInfo entityProperty)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(entityProperty, "entityProperty");

      return this.mappingItemMatcher.GetFieldNames(entityProperty).Select(itemField => item.Fields[itemField]).FirstOrDefault(field => field != null);
    }
  }
}