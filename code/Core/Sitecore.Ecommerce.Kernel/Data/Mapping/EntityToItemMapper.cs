// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityToItemMapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the entity to item mapper class.
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
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;

  /// <summary>
  /// Defines the entity to item mapper class.
  /// </summary>
  public class EntityToItemMapper : ItemBasedEntityMapper<IEntity, Item>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityToItemMapper" /> class.
    /// </summary>
    public EntityToItemMapper()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityToItemMapper" /> class.
    /// </summary>
    /// <param name="mappingItemMatcher">The mapping item matcher.</param>
    /// <param name="mappings">The mappings.</param>
    public EntityToItemMapper(MappingItemMatcher mappingItemMatcher, EntityMemberConverterLookupTable mappings)
      : base(mappingItemMatcher, mappings)
    {
    }

    /// <summary>
    /// Maps the specified source.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="item">The item.</param>
    public override void Map([NotNull] IEntity entity, [NotNull] Item item)
    {
      Assert.ArgumentNotNull(entity, "entity");
      Assert.ArgumentNotNull(item, "item");

      item.Fields.ReadAll();

      using (new EditContext(item))
      {
        foreach (PropertyInfo entityProperty in entity.GetType().GetProperties())
        {
          if (!entityProperty.CanRead)
          {
            continue;
          }

          object entityValue = entityProperty.GetValue(entity, null);

          object mappingItem = this.GetMappingItem(item, entityProperty);
          if (mappingItem == null)
          {
            continue;
          }

          IEntityMemberConverter rule = this.Mappings.GetConverter(entityProperty);
          if (mappingItem is Field && rule is IRequiresStorageObject<Field>)
          {
            ((IRequiresStorageObject<Field>)rule).StorageObject = (Field)mappingItem;
          }

          rule.ToStorage(entityValue);
        }
      }
    }
  }
}