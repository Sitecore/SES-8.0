// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataMapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The site settings provider.
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

namespace Sitecore.Ecommerce.Data
{
  using System;
  using System.Collections.Generic;
  using System.Reflection;
  using Diagnostics;
  using DomainModel.Data;
  using Reflection;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Ecommerce.Unity;
  using Sitecore.Resources.Media;

  /// <summary>
  /// The site settings provider.
  /// </summary>
  public class DataMapper : IDataMapper
  {
    /// <summary>
    /// The entity helper.
    /// </summary>
    private readonly EntityHelper entityHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataMapper" /> class.
    /// </summary>
    public DataMapper()
      : this(new EntityHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataMapper" /> class.
    /// </summary>
    /// <param name="entityHelper">The entity helper.</param>
    public DataMapper(EntityHelper entityHelper)
    {
      this.entityHelper = entityHelper;
    }

    /// <summary>
    /// Gets the container.
    /// </summary>
    /// <typeparam name="T">The container contract.</typeparam>
    /// <param name="item">The data item.</param>
    /// <returns>The container instance.</returns>
    public virtual T GetEntity<T>(Item item)
    {
      Assert.ArgumentNotNull(item, "Container item");

      return (T)this.GetEntity(item, typeof(T));
    }

    /// <summary>
    /// Gets the entity by it's location (item Id).
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="item">The data item.</param>
    /// <param name="mappingRuleName">The mapping rule.</param>
    /// <returns>The entity instance.</returns>
    public virtual T GetEntity<T>(Item item, string mappingRuleName)
    {
      Assert.ArgumentNotNull(item, "Container item");
      IMappingRule<T> mappingRule = Context.Entity.SmartResolve<IMappingRule<T>>(mappingRuleName);

      Assert.IsNotNull(mappingRule, "Mapping rule was nor found");

      if (mappingRule.MappingObject.Equals(default(T)))
      {
        object entity = Context.Entity.HasRegistration(typeof(T)) ? Context.Entity.Resolve(typeof(T)) : ReflectionUtil.CreateObject(typeof(T));
        Assert.IsNotNull(entity, "The instance of the entity is null");

        mappingRule.MappingObject = (T)entity;
      }

      if (mappingRule.MappingObject is IEntity)
      {
        ((IEntity)mappingRule.MappingObject).Alias = item.ID.ToString();
      }

      mappingRule = this.GetEntity(item, mappingRule) as IMappingRule<T>;
      Assert.IsNotNull(mappingRule, "The instance of the mapping rule is null");

      return mappingRule.MappingObject;
    }

    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <param name="item">The container item.</param>
    /// <param name="type">The container contract.</param>
    /// <returns>The container instance.</returns>
    public virtual object GetEntity(Item item, Type type)
    {
      Assert.ArgumentNotNull(type, "Type is null");
      Assert.ArgumentNotNull(item, "Container item is null");

      object entity = Context.Entity.HasRegistration(type) ? Context.Entity.Resolve(type) : ReflectionUtil.CreateObject(type);

      return this.GetEntity(item, entity);
    }

    /// <summary>
    /// Saves the entity.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="item">The data item.</param>
    public virtual void SaveEntity(object value, Item item)
    {
      Assert.ArgumentNotNull(value, "Input container object is null");
      Assert.ArgumentNotNull(item, "Input item");

      IDictionary<string, object> fieldsCollection = this.entityHelper.GetPropertiesValues(value);

      item.Fields.ReadAll();
      using (new SecurityModel.SecurityDisabler())
      {
        item.Editing.BeginEdit();
        foreach (KeyValuePair<string, object> keyValuePair in fieldsCollection)
        {
          string stringValue = Utils.TypeUtil.TryParse(keyValuePair.Value, string.Empty);

          Field field = item.Fields[keyValuePair.Key];
          if (field == null || field.Value.Equals(value))
          {
            continue;
          }

          item[field.Name] = stringValue;
        }

        item.Editing.EndEdit();
      }
    }

    /// <summary>
    /// Saves the entity.
    /// </summary>
    /// <typeparam name="T">Save the entity to data storage.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="item">The data item.</param>
    /// <param name="mappingRuleName">The mapping rule name.</param>
    public virtual void SaveEntity<T>(T value, Item item, string mappingRuleName)
    {
      Assert.ArgumentNotNull(value, "Input container ");
      Assert.ArgumentNotNull(item, "Input item");

      IMappingRule<T> mappingRule = Context.Entity.SmartResolve<IMappingRule<T>>(mappingRuleName);

      Assert.IsNotNull(mappingRule, "Mapping rule was nor found");

      mappingRule.MappingObject = value;

      this.SaveEntity(mappingRule, item);
    }

    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <param name="item">The container item.</param>
    /// <param name="entity">The container contract.</param>
    /// <returns>The container instance.</returns>
    public virtual object GetEntity(Item item, object entity)
    {
      Assert.ArgumentNotNull(entity, "The instance of the entity is null");
      Assert.ArgumentNotNull(item, "Container item is null");

      if (entity is IEntity)
      {
        ((IEntity)entity).Alias = item.ID.ToString();
      }

      foreach (PropertyInfo info in entity.GetType().GetProperties())
      {
        EntityAttribute entityAttribute = Attribute.GetCustomAttribute(info, typeof(EntityAttribute), true) as EntityAttribute;
        if (entityAttribute == null || string.IsNullOrEmpty(entityAttribute.FieldName))
        {
          continue;
        }

        Field field = item.Fields[entityAttribute.FieldName];
        if (field == null || field.Value == null)
        {
          continue;
        }

        Type instanceType = info.PropertyType;

        object instanceValue = null;

        if (info.CanRead)
        {
          instanceValue = info.GetValue(entity, null);
        }

        if (instanceValue != null)
        {
          instanceType = instanceValue.GetType();
        }

        if (entityAttribute.Rule == FieldMappingRule.GetItemName)
        {
          string itemId = instanceValue as string;
          if (!string.IsNullOrEmpty(itemId) && ID.IsID(itemId))
          {
            Item targetItem = Sitecore.Context.Database.GetItem(itemId) ?? Sitecore.Context.ContentDatabase.GetItem(itemId);
            if (info.CanWrite)
            {
              info.SetValue(entity, Utils.TypeUtil.Parse(targetItem.Name, info.PropertyType), null);
            }
          }

          continue;
        }

        if (typeof(IEntity).IsAssignableFrom(instanceType))
        {
          string itemPath = Utils.TypeUtil.TryParse(this.GetFieldValue(field), string.Empty);
          if (!string.IsNullOrEmpty(itemPath) && ID.IsID(itemPath))
          {
            Item nestedContainerItem = Sitecore.Context.Database.GetItem(itemPath) ?? Sitecore.Context.ContentDatabase.GetItem(itemPath);
            if (nestedContainerItem != null)
            {
              object nestedContainer = this.GetEntity(nestedContainerItem, info.PropertyType);
              if (info.CanWrite)
              {
                info.SetValue(entity, nestedContainer, null);
              }
            }
          }

          continue;
        }

        if (info.CanWrite)
        {
          info.SetValue(entity, Utils.TypeUtil.Parse(this.GetFieldValue(field), info.PropertyType), null);
        }
      }

      return entity;
    }

    /// <summary>
    /// Gets the field value.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <returns>The field value.</returns>
    protected virtual object GetFieldValue(Field field)
    {
      if (field.Type.Equals("Checkbox"))
      {
        bool value = MainUtil.GetBool(field.Value, false);
        return value;
      }

      if (field.Type.Equals("Image"))
      {
        try
        {
          ImageField imageField = new ImageField(field);
          var cleanUrl = MediaManager.GetMediaUrl(imageField.MediaItem);
          var hashedUrl = HashingUtils.ProtectAssetUrl(cleanUrl);
          return hashedUrl;
        }
        catch
        {
        }
      }

      if (field.Type.Equals("General Link"))
      {
        try
        {
          LinkField linkField = new LinkField(field);
          return linkField.Url;
        }
        catch
        {
        }
      }

      return field.Value;
    }
  }
}