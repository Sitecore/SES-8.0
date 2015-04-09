// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityResultDataConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Converts item collection to the result data grid.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models.Search
{
  using System.Collections.Generic;
  using System.Linq;
  using Data;
  using Data.Mapping;
  using Diagnostics;
  using DomainModel.Data;
  using Sitecore.Data;
  using Utils;

  /// <summary>
  /// Converts entity collection to the result data grid.
  /// </summary>
  /// <typeparam name="T">The entity type</typeparam>
  public class EntityResultDataConverter<T> : IResultDataConverter<IEnumerable<T>>
  {
    /// <summary>
    /// Converts the specified initial result.
    /// </summary>
    /// <param name="initialResult">The initial result.</param>
    /// <param name="columns">The columns.</param>
    /// <returns>
    /// The data converted to the GridData format.
    /// </returns>
    public virtual GridData Convert(IEnumerable<T> initialResult, List<GridColumn> columns)
    {
      Assert.ArgumentNotNull(initialResult, "initialResult");
      Assert.ArgumentNotNull(columns, "columns");

      var data = new GridData();

      foreach (var item in initialResult)
      {
        var row = new List<string>(columns.Count);
        foreach (var column in columns)
        {
          if (column.RowID)
          {
            var mappingObject = this.GetMappingObject(item);
            var dataObject = mappingObject ?? item;

            var o = dataObject as IEntity;
            if (o != null && ID.IsID(o.Alias))
            {
              row.Add(ID.Parse(o.Alias).ToString());
              continue;
            }
          }

          if (string.IsNullOrEmpty(column.FieldName))
          {
            row.Add(string.Empty);
            continue;
          }

          var value = this.GetProductFiledValue(item, column);
          if (DateUtil.IsIsoDate(value) && !string.IsNullOrEmpty(column.FormatString))
          {
            value = DateUtil.FormatIsoDate(value, column.FormatString);
          }

          row.Add(value);
        }

        data.Rows.Add(row);
      }

      return data;
    }

    /// <summary>
    /// Gets the product filed value.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="column">The column.</param>
    /// <returns>The value of the field.</returns>
    protected virtual string GetProductFiledValue(T container, GridColumn column)
    {
      Assert.ArgumentNotNull(container, "container");
      Assert.ArgumentNotNull(column, "column");

      string[] value = { null };

      var matcher = Context.Entity.Resolve<MappingItemMatcher>();

      foreach (var info in container.GetType().GetProperties())
      {
        if (!string.IsNullOrEmpty(value[0]))
        {
          break;
        }

        if (!info.CanRead)
        {
          continue;
        }

        var names = matcher.GetFieldNames(info);
        var info1 = info;
        foreach (var instanceValue in from name in names.TakeWhile(name => string.IsNullOrEmpty(value[0])) where column.FieldName == name select info1.GetValue(container, null))
        {
          value[0] = TypeUtil.TryParse(instanceValue, string.Empty);

          if (!ID.IsID(value[0]))
          {
            continue;
          }

          var item = Sitecore.Context.Database.GetItem(value[0]) ?? Sitecore.Context.ContentDatabase.GetItem(value[0]);
          if (item != null)
          {
            value[0] = item.DisplayName;
          }
        }
      }

      if (value[0] == null)
      {
        var propertyInfo = container.GetType().GetProperty("Specifications");
        if (propertyInfo != null)
        {
          var specification = propertyInfo.GetValue(container, null);

          if (specification != null)
          {
            var methodInfo = specification.GetType().GetMethod("ContainsKey", new[] { typeof(string) });

            if (true.Equals(methodInfo.Invoke(specification, new object[] { column.FieldName })))
            {
              propertyInfo = specification.GetType().GetProperty("Item", new[] { typeof(string) });

              if (propertyInfo != null)
              {
                var rawValue = propertyInfo.GetValue(specification, new object[] { column.FieldName });

                value[0] = TypeUtil.TryParse(rawValue, string.Empty);

                if (ID.IsID(value[0]))
                {
                  var item = Sitecore.Context.Database.GetItem(value[0]) ?? Sitecore.Context.ContentDatabase.GetItem(value[0]);
                  if (item != null)
                  {
                    value[0] = item.DisplayName;
                  }
                }
              }
            }
          }
        }
      }

      if (value[0] != null || !(container is IEntity))
      {
        return value[0] ?? string.Empty;
      }

      var id = ((IEntity)container).Alias;
      if (string.IsNullOrEmpty(id))
      {
        return value[0] ?? string.Empty;
      }

      var theItem = Sitecore.Context.Database.GetItem(id);
      if (theItem == null)
      {
        return value[0] ?? string.Empty;
      }

      value[0] = theItem[column.FieldName];
      if (!ID.IsID(value[0]))
      {
        return value[0] ?? string.Empty;
      }

      var linkedItem = theItem.Database.GetItem(value[0]);
      if (linkedItem != null)
      {
        value[0] = linkedItem.Name;
      }

      return value[0] ?? string.Empty;
    }

    /// <summary>
    /// Determines whether [is napping rule] [the specified item].
    /// </summary>
    /// <param name="item">The object.</param>
    /// <returns>
    ///   <c>true</c> if [is napping rule] [the specified item]; otherwise, <c>false</c>.
    /// </returns>
    protected virtual object GetMappingObject(T item)
    {
      Assert.ArgumentNotNull(item, "item");

      return (from i in item.GetType().GetInterfaces() where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMappingRule<object>).GetGenericTypeDefinition() select i.GetGenericArguments()[0] into mappingObjectType from propertyInfo in item.GetType().GetProperties().Where(propertyInfo => propertyInfo.PropertyType.IsAssignableFrom(mappingObjectType)) select propertyInfo.GetValue(item, null)).FirstOrDefault();
    }
  }
}