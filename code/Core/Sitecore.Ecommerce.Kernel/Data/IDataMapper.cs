// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataMapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The settings provider interface.
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
  using Sitecore.Data.Items;

  /// <summary>
  /// The settings provider interface.
  /// </summary>
  public interface IDataMapper
  {
    /// <summary>
    /// Gets the entity by it's location (item Id).
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="item">The data item.</param>
    /// <returns>The entity instance.</returns>
    T GetEntity<T>(Item item);

    /// <summary>
    /// Gets the entity by it's location (item Id).
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="item">The data item.</param>
    /// <param name="mappingRule">The mapping rule.</param>
    /// <returns>The entity instance.</returns>
    T GetEntity<T>(Item item, string mappingRule);

    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <param name="item">The container item.</param>
    /// <param name="type">The container contract.</param>
    /// <returns>The container instance.</returns>
    object GetEntity(Item item, Type type);

    /// <summary>
    /// Saves the entity the data storage.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="item">The data item.</param>
    void SaveEntity(object value, Item item);

    /// <summary>
    /// Saves the entity to the data storage.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="item">The data item.</param>
    /// <param name="mappingRule">The mapping rule.</param>
    void SaveEntity<T>(T value, Item item, string mappingRule);
  }
}