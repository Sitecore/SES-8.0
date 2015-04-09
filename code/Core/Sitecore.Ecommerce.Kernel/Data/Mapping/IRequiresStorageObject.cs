// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRequiresStorageObject.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Determines if a storage object is required for the mapping rule.
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

namespace Sitecore.Ecommerce.Data.Mapping
{
  /// <summary>
  /// Determines if a storage object is required for the mapping rule.
  /// </summary>
  /// <typeparam name="T">The storage object type.</typeparam>
  public interface IRequiresStorageObject<T>
  {
    /// <summary>
    /// Gets or sets the field.
    /// </summary>
    /// <value>
    /// The field.
    /// </value>
    T StorageObject { get; set; }
  }
}