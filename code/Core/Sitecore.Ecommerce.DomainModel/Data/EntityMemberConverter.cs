// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityMemberConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the entity member converter class.
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
  /// Defines the entity member converter class.
  /// </summary>
  /// <typeparam name="TEntityMember">The type of the entity member.</typeparam>
  /// <typeparam name="TStorage">The type of the storage.</typeparam>
  public abstract class EntityMemberConverter<TEntityMember, TStorage> : IEntityMemberConverter
  {
    /// <summary>
    /// Converts the storage object to an entity member type.
    /// </summary>
    /// <param name="storage">The storage object.</param>
    /// <returns>
    /// The entity member value.
    /// </returns>
    public abstract TEntityMember ConvertFrom(TStorage storage);

    /// <summary>
    /// Converts the entity member to a storage object.
    /// </summary>
    /// <param name="entityMember">The entity member.</param>
    /// <returns>
    /// The storage object.
    /// </returns>
    public abstract TStorage ToStorage(TEntityMember entityMember);

    /// <summary>
    /// Converts the storage object to an entity member type.
    /// </summary>
    /// <param name="storage">The storage object.</param>
    /// <returns>
    /// The entity member value.
    /// </returns>
    object IEntityMemberConverter.ConvertFrom(object storage)
    {
      return this.ConvertFrom((TStorage)storage);
    }

    /// <summary>
    /// Converts the entity member to a storage object.
    /// </summary>
    /// <param name="entityMember">The entity member.</param>
    /// <returns>
    /// The storage object.
    /// </returns>
    object IEntityMemberConverter.ToStorage(object entityMember)
    {
      return this.ToStorage((TEntityMember)entityMember);
    }
  }
}