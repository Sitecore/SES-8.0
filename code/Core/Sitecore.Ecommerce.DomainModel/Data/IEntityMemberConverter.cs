// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityMemberConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Converts the entity member to storage type
//   and vice-versa.
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
  /// Converts the entity member to storage type
  /// and vice-versa.
  /// </summary>
  public interface IEntityMemberConverter
  {
    /// <summary>
    /// Converts the storage object to an entity member type.
    /// </summary>
    /// <param name="storage">The storage object.</param>
    /// <returns>
    /// The entity member value.
    /// </returns>
    object ConvertFrom(object storage);

    /// <summary>
    /// Converts the entity member to a storage object.
    /// </summary>
    /// <param name="entityMember">The entity member.</param>
    /// <returns>
    /// The storage object.
    /// </returns>
    object ToStorage(object entityMember);
  }
}