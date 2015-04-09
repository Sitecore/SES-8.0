// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuresStorage.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the StructuresStorage class.
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
  using System.Linq;
  using System.Data;
  using System.Reflection;
  using DomainModel.Data;

  internal class StructuresStorage : Dictionary<Type, DataTable>
  {
    public static StructuresStorage Instance = new StructuresStorage();

    StructuresStorage() { }

    public DataTable InitialType(Type type)
    {
      DataTable table = this.Keys.Contains(type) ? this[type] : null;

      if (table == null)
      {
        table = new DataTable(type.Name);
        CreateTableColumns(type, table, new PrefixHolder());
        this[type] = table;
      }
      return table;
    }

    protected virtual void CreateTableColumns(Type type, DataTable dataTable, PrefixHolder prefixHolder)
    {
      prefixHolder.AppendToPrefix("Alias");
      dataTable.Columns.Add(prefixHolder.Prefix, typeof(string));
      prefixHolder.RemoveLastPart();

      foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
      {
        if (propertyInfo.GetIndexParameters().Length == 0)
        {
          prefixHolder.AppendToPrefix(propertyInfo.Name);

          if (propertyInfo.PropertyType.IsPrimitive ||
              propertyInfo.PropertyType == typeof(string) ||
              propertyInfo.PropertyType == typeof(DateTime) ||
              propertyInfo.PropertyType == typeof(decimal))
          {
            if (!dataTable.Columns.Contains(prefixHolder.Prefix))
            {
              dataTable.Columns.Add(prefixHolder.Prefix, propertyInfo.PropertyType);
            }
          }
          else
          {
            Type[] interfaces = propertyInfo.PropertyType.GetInterfaces();
            bool isCollection = false;

            foreach (Type intrf in interfaces)
            {
              if (intrf.IsGenericType && (intrf.GetGenericTypeDefinition() == typeof(ICollection<>)))
              {
                isCollection = true;
                break;
              }
            }

            if (isCollection)
            {
              dataTable.Columns.Add(prefixHolder.Prefix, typeof(DataTable));
            }
            else
            {
              this.CreateTableColumns(propertyInfo.PropertyType, dataTable, prefixHolder);

              dataTable.Columns.Add(prefixHolder.Prefix, typeof(string));

              if (propertyInfo.PropertyType == typeof(DomainModel.Products.ProductBaseData))
              {
                prefixHolder.AppendToPrefix("Brand");
                dataTable.Columns.Add(prefixHolder.Prefix, typeof(string));
                prefixHolder.RemoveLastPart();

                prefixHolder.AppendToPrefix("Description");
                dataTable.Columns.Add(prefixHolder.Prefix, typeof(string));
                prefixHolder.RemoveLastPart();

                prefixHolder.AppendToPrefix("ImageUrl");
                dataTable.Columns.Add(prefixHolder.Prefix, typeof(string));
                prefixHolder.RemoveLastPart();

                prefixHolder.AppendToPrefix("ShortDescription");
                dataTable.Columns.Add(prefixHolder.Prefix, typeof(string));
                prefixHolder.RemoveLastPart();

                prefixHolder.AppendToPrefix("Stock");
                dataTable.Columns.Add(prefixHolder.Prefix, typeof(int));
                prefixHolder.RemoveLastPart();

                prefixHolder.AppendToPrefix("VatType");
                dataTable.Columns.Add(prefixHolder.Prefix, typeof(string));
                prefixHolder.RemoveLastPart();
              }
            }
          }

          prefixHolder.RemoveLastPart();
        }
      }
    }
  }
}

