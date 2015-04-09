// -------------------------------------------------------------------------------------------
// <copyright file="CustomData.cs" company="Sitecore Corporation">
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
  /// <summary>
  /// Custom data class.
  /// </summary>
  public class CustomData : IEntity
  {
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    /// <value>The id.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    /// <value>The key.</value>
    public virtual string EntityAlias { get; set; }

    /// <summary>
    /// Gets or sets the type of the entity.
    /// </summary>
    /// <value>The type of the entity.</value>
    public virtual string EntityType { get; set; }

    /// <summary>
    /// Gets or sets the property.
    /// </summary>
    /// <value>The property name.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public virtual string Value { get; set; }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual string Alias { get; set; }
  }
}