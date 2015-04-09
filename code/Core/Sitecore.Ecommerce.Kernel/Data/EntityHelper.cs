// -------------------------------------------------------------------------------------------
// <copyright file="EntityHelper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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
  using System.Linq.Expressions;
  using System.Reflection;
  using Diagnostics;
  using DomainModel.Data;
  using Microsoft.Practices.Unity;
  using Sitecore.Ecommerce.Unity;
  using Utils;

  /// <summary>
  /// The Entity Serializer implementation.
  /// </summary>
  public class EntityHelper
  {
    /// <summary>
    /// Serializes the specified entity to the key-value collection of properties values.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="entity">The entity. </param>
    /// <returns>The key-value collection of properties values.</returns>
    public virtual IDictionary<string, object> GetPropertiesValues<T>(T entity)
    {
      Assert.ArgumentNotNull(entity, "entity");

      Dictionary<string, object> dictionary = new Dictionary<string, object>();

      var entityType = entity.GetType();
      object obj = Context.Entity.HasRegistration(entityType) ? Context.Entity.Resolve(entityType) : entity;

      foreach (PropertyInfo info in entity.GetType().GetProperties())
      {
        EntityAttribute entityAttribute = Attribute.GetCustomAttribute(obj.GetType().GetProperty(info.Name), typeof(EntityAttribute)) as EntityAttribute;
        if (entityAttribute == null || string.IsNullOrEmpty(entityAttribute.FieldName))
        {
          continue;
        }

        if (info.PropertyType.IsClass && !info.PropertyType.IsValueType && !info.PropertyType.IsPrimitive && info.PropertyType.FullName != "System.String")
        {
          object nestedEntity = info.GetValue(entity, null);

          IDictionary<string, object> fieldsCollection = nestedEntity != null ? this.GetPropertiesValues(nestedEntity) : new Dictionary<string, object>();

          if (nestedEntity is IEntity)
          {
            string aliasValue = ((IEntity)nestedEntity).Alias;
            if (!dictionary.ContainsKey(entityAttribute.FieldName))
            {
              dictionary.Add(entityAttribute.FieldName, aliasValue);
            }
          }

          foreach (KeyValuePair<string, object> keyValuePair in fieldsCollection)
          {
            string key = string.Concat(entityAttribute.FieldName, keyValuePair.Key);

            if (!dictionary.ContainsKey(key))
            {
              dictionary.Add(key, keyValuePair.Value);
            }
          }
        }
        else
        {
          if (!dictionary.ContainsKey(entityAttribute.FieldName))
          {
            dictionary.Add(entityAttribute.FieldName, info.GetValue(entity, null));
          }
        }
      }

      return dictionary;
    }

    /// <summary>
    /// Gets the field.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="property">The property.</param>
    /// <returns>The field name.</returns>
    public virtual string GetField<T>(Expression<Func<T, object>> property)
    {
      Assert.ArgumentNotNull(property, "property");
      Assert.ArgumentNotNull(property.Body, "property.Body");

      MemberExpression memberExpression = property.Body as MemberExpression;

      if (memberExpression == null)
      {
        return string.Empty;
      }

      string propertyName = memberExpression.Member.Name;

      return string.IsNullOrEmpty(propertyName) ? string.Empty : this.GetField(typeof(T), propertyName);
    }

    /// <summary>
    /// Gets the field.
    /// </summary>
    /// <param name="type">The entity type.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>The field name.</returns>
    public virtual string GetField(Type type, string propertyName)
    {
      Assert.ArgumentNotNullOrEmpty(propertyName, "propertyName");
      Assert.ArgumentNotNull(type, "type");

      if (Context.Entity.HasRegistration(type))
      {
        type = Context.Entity.Resolve(type).GetType();
      }

      PropertyInfo info = type.GetProperty(propertyName);
      if (info == null)
      {
        return string.Empty;
      }

      EntityAttribute entityAttribute = Attribute.GetCustomAttribute(info, typeof(EntityAttribute), true) as EntityAttribute;
      if (entityAttribute == null || string.IsNullOrEmpty(entityAttribute.FieldName))
      {
        return string.Empty;
      }

      return entityAttribute.FieldName;
    }

    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <param name="type">The type of the object.</param>
    /// <returns>The template Id.</returns>
    public virtual string GetTemplate(Type type)
    {
      Assert.ArgumentNotNull(type, "type");

      if (Context.Entity.HasRegistration(type))
      {
        type = Context.Entity.Resolve(type).GetType();
      }

      EntityAttribute entityAttribute = Attribute.GetCustomAttribute(type, typeof(EntityAttribute), true) as EntityAttribute;
      if (entityAttribute != null && !string.IsNullOrEmpty(entityAttribute.TemplateId))
      {
        return entityAttribute.TemplateId;
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>The template Id.</returns>
    public virtual string GetTemplate<T>()
    {
      return this.GetTemplate(typeof(T));
    }

    /// <summary>
    /// Gets the name of the property value by attribute.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <typeparam name="Tr">The type of the business object.</typeparam>
    /// <param name="entity">The entity instance.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns>The object value.</returns>
    public virtual T GetPropertyValueByField<T, Tr>(Tr entity, string fieldName)
    {
      Assert.ArgumentNotNull(entity, "entity");
      Assert.ArgumentNotNullOrEmpty(fieldName, "fieldName");

      var entityType = entity.GetType();
      object obj = Context.Entity.HasRegistration(entityType) ? Context.Entity.Resolve(entityType) : entity;

      foreach (PropertyInfo info in entity.GetType().GetProperties())
      {
        EntityAttribute entityAttribute = Attribute.GetCustomAttribute(obj.GetType().GetProperty(info.Name), typeof(EntityAttribute), true) as EntityAttribute;
        if (entityAttribute == null || string.IsNullOrEmpty(entityAttribute.FieldName) || !string.Equals(fieldName, entityAttribute.FieldName))
        {
          continue;
        }

        return TypeUtil.TryParse(info.GetValue(entity, null), default(T));
      }

      return default(T);
    }

    /// <summary>
    /// Copies the properties values.
    /// </summary>
    /// <typeparam name="T">The type of the source entity</typeparam>
    /// <typeparam name="Tr">The type of the target entity.</typeparam>
    /// <param name="sourceEntity">The source entity.</param>
    /// <param name="targetEntity">The target entity.</param>
    public virtual void CopyPropertiesValues<T, Tr>(T sourceEntity, ref Tr targetEntity)
    {
      Assert.ArgumentNotNull(sourceEntity, "Argument source entity is null");
      Assert.ArgumentNotNull(targetEntity, "Argument target entity is null");

      var sourceEntityType = sourceEntity.GetType();
      object sourceObject = Context.Entity.HasRegistration(sourceEntityType) ? Context.Entity.Resolve(sourceEntityType) : sourceEntity;

      var targetEntityType = targetEntity.GetType();
      object targetObject = Context.Entity.HasRegistration(targetEntityType) ? Context.Entity.Resolve(targetEntityType) : targetEntity;

      foreach (PropertyInfo targetInfo in targetEntity.GetType().GetProperties())
      {
        EntityAttribute targetEntityAttribute = Attribute.GetCustomAttribute(targetObject.GetType().GetProperty(targetInfo.Name), typeof(EntityAttribute)) as EntityAttribute;
        if (targetEntityAttribute == null || string.IsNullOrEmpty(targetEntityAttribute.FieldName))
        {
          continue;
        }

        foreach (PropertyInfo sourceInfo in sourceEntity.GetType().GetProperties())
        {
          EntityAttribute sourcerEntityAttribute = Attribute.GetCustomAttribute(sourceObject.GetType().GetProperty(sourceInfo.Name), typeof(EntityAttribute)) as EntityAttribute;
          if (sourcerEntityAttribute == null || string.IsNullOrEmpty(sourcerEntityAttribute.FieldName) || !string.Equals(sourcerEntityAttribute.FieldName, targetEntityAttribute.FieldName, StringComparison.OrdinalIgnoreCase))
          {
            continue;
          }

          if (!sourceInfo.CanRead || !targetInfo.CanWrite || sourceInfo.PropertyType != targetInfo.PropertyType)
          {
            continue;
          }

          object source = sourceInfo.GetValue(sourceEntity, null);
          if (source != null)
          {
            targetInfo.SetValue(targetEntity, source, null);
          }
        }
      }
    }
  }
}