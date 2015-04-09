// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyValuePairConvertor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the KeyValuePairConvertor type.
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
  using Sitecore.Diagnostics;

  /// <summary>
  /// Puts the content of the KeyValuePair struct to DataRow and vice-versa
  /// </summary>
  /// <typeparam name="TK">
  /// Type of the key
  /// </typeparam>
  /// <typeparam name="TV">
  /// Type of the value
  /// </typeparam>
  public class KeyValuePairConvertor<TK, TV> : Convertor<KeyValuePair<TK, TV>>
  {
    /// <summary>
    /// Puts the content of the KeyValuePair struct to DataRow
    /// </summary>
    /// <param name="model">
    /// The model to put
    /// </param>
    /// <param name="row">
    /// The row which stores the values
    /// </param>
    public override void DomainModelToDTO(KeyValuePair<TK, TV> model, ref System.Data.DataRow row)
    {
      Assert.IsNotNull(row, "row must not be null");
      Assert.IsNotNull(model, "model must not be null");

      row["Key"] = model.Key;
      row["Value"] = model.Value;
    }

    /// <summary>
    /// Retrieves the content of the KeyValuePair struct from DataRow
    /// </summary>
    /// <param name="row">
    /// The row to get values from
    /// </param>
    /// <param name="model">
    /// The KeyValuePair struct to restore
    /// </param>
    public override void DTOToDomainModel(System.Data.DataRow row, ref KeyValuePair<TK, TV> model)
    {
      Assert.IsNotNull(row, "model must not be null");
      Assert.IsNotNull(model, "model must not be null");

      model = new KeyValuePair<TK, TV>(this.ConvertDataRowValue<TK>(row["Key"]), this.ConvertDataRowValue<TV>(row["Value"]));
    }

    /// <summary>
    /// Creates an instance of the KeyValuePair struct
    /// </summary>
    /// <returns>new KeyValuePair struct</returns>
    public override KeyValuePair<TK, TV> GetInstance()
    {
      return new KeyValuePair<TK, TV>();
    }
  }
}
