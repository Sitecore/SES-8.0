// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemToEntityMapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the item to entity mapper class.
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
  using System.Reflection;
  using Diagnostics;
  using DomainModel.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// Defines the item to entity mapper class.
  /// </summary>
  public class ItemToEntityMapper : ItemBasedEntityMapper<Item, IEntity>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemToEntityMapper" /> class.
    /// </summary>
    public ItemToEntityMapper()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemToEntityMapper" /> class.
    /// </summary>
    /// <param name="mappingItemMatcher">The mapping item matcher.</param>
    /// <param name="mappings">The mappings.</param>
    public ItemToEntityMapper(MappingItemMatcher mappingItemMatcher, EntityMemberConverterLookupTable mappings)
      : base(mappingItemMatcher, mappings)
    {
    }

    /// <summary>
    /// Maps the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="entity">The entity.</param>
    public override void Map([NotNull] Item item, [NotNull] IEntity entity)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(entity, "entity");

      item.Fields.ReadAll();

      Assert.IsNotNull(this.Mappings, "Unable to convert an item to an entity. EntityMemberConverterLookupTable cannot be null.");

      foreach (PropertyInfo entityProperty in entity.GetType().GetProperties())
      {
        IEntityMemberConverter converter = this.Mappings.GetConverter(entityProperty);

        if (converter is IRequiresEntityMemberType)
        {
          ((IRequiresEntityMemberType)converter).MemberType = entityProperty.PropertyType;
        }

        object mappingItem;
        if (converter is IRequiresStorageObject<Item>)
        {
          mappingItem = item;
        }
        else
        {
          mappingItem = this.GetMappingItem(item, entityProperty);
        }

        if (mappingItem == null)
        {
          var defaultConverter = this.Mappings.DirectMappingEntityMemberConverter;
          defaultConverter.StorageObject = item;

          mappingItem = item.GetType().GetProperty(entityProperty.Name);
          if (mappingItem == null)
          {
            continue;
          }

          converter = defaultConverter;
        }

        object entityValue = converter.ConvertFrom(mappingItem);

        if (entityProperty.CanWrite)
        {
          entityProperty.SetValue(entity, entityValue, null);
        }
      }

      entity.Alias = item.ID.ToString();
    }
  }
}