// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldBasedEntityMemberConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the field based entity member converter class.
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
  using DomainModel.Data;
  using Sitecore.Data.Fields;
  using Validators.Interception;

  /// <summary>
  /// Defines the field based entity member converter class.
  /// </summary>
  /// <typeparam name="T">The entity member type.</typeparam>
  public abstract class FieldBasedEntityMemberConverter<T> : EntityMemberConverter<T, Field>, IRequiresStorageObject<Field>
  {
    /// <summary>
    /// Gets or sets the field.
    /// </summary>
    /// <value>
    /// The field.
    /// </value>
    public Field StorageObject { get; [NotNullValue]set; }
  }
}