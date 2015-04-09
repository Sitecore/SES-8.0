// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityMapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the entity mapper class.
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

namespace Sitecore.Ecommerce.DomainModel.Data
{
  /// <summary>
  /// Defines the entity data mapper class.
  /// </summary>
  /// <typeparam name="TSource">The type of the source.</typeparam>
  /// <typeparam name="TTarget">The type of the target.</typeparam>
  public abstract class EntityMapper<TSource, TTarget>
  {
    /// <summary>
    /// Maps the specified object of one type to another.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="target">The target.</param>
    public abstract void Map(TSource source, TTarget target);
  }
}