// -------------------------------------------------------------------------------------------
// <copyright file="IEntityProvider.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.DomainModel.Data
{
  using System.Collections.Generic;

  /// <summary>
  /// The container provider interface.
  /// </summary>
  /// <typeparam name="T">The container interface.</typeparam>
  public interface IEntityProvider<T> where T : class
  {
    /// <summary>
    /// Gets the default value.
    /// </summary>
    /// <typeparam name="T">The entity interface.</typeparam>
    /// <returns>The default value from entities collection.</returns>
    T GetDefault();

    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>The entities collection.</returns>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Gets the container by code.
    /// </summary>
    /// <typeparam name="T">The container interface.</typeparam>
    /// <param name="code">The container code.</param>
    /// <returns>The container by code.</returns>
    T Get(string code);
  }
}