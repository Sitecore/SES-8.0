// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Convertor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Convertor class.
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

namespace Sitecore.Ecommerce.Data.Convertors
{
  using System.Collections.Generic;
  using System.Data;

  public abstract class Convertor<T> where T : new()
  {

    public abstract void DomainModelToDTO(T model, ref DataRow row);
    public abstract void DTOToDomainModel(DataRow row, ref T model);
    public abstract T GetInstance();

    public IEnumerable<T> DTOToDomainModel(DataTable table)
    {
      foreach (DataRow row in table.Rows)
      {
        T item = this.GetInstance();
        DTOToDomainModel(row, ref item);
        yield return item;
      }
    }

    public virtual DataTable DomainModelToDTO(T model)
    {
      // typeof(T) replaced to model.GetType() in order to handle derived type convertions.
      DataTable resultTable = StructuresStorage.Instance.InitialType(model.GetType()).Clone();
      MapToTable(resultTable, model);
      return resultTable;
    }

    public virtual DataTable DomainModelToDTO(IEnumerable<T> model)
    {
      DataTable resultTable = StructuresStorage.Instance.InitialType(typeof(T)).Clone();
      foreach (var item in model)
      {
        MapToTable(resultTable, item);
      }

      return resultTable;
    }

    private void MapToTable(DataTable resultTable, T item)
    {
      DataRow row = resultTable.NewRow();
      DomainModelToDTO(item, ref row);
      resultTable.Rows.Add(row);
    }

    protected TValue ConvertDataRowValue<TValue>(object obj)
    {
      if (obj is System.DBNull)
      {
        return default(TValue);
      }

      return (TValue)obj;
    }
  }
}