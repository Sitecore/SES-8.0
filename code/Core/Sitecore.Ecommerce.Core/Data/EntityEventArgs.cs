// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the EntityEventArgs type.
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
  using Diagnostics;

  /// <summary>
  /// Defines the entity saved event args class.
  /// </summary>
  /// <typeparam name="T">Type of the entity being saved</typeparam>
  public class EntityEventArgs<T> : EventArgs
  {
    /// <summary>
    /// Stores reference to the entity.
    /// </summary>
    private readonly T entity;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityEventArgs&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public EntityEventArgs(T entity)
    {
      Assert.ArgumentNotNull(entity, "entity");

      this.entity = entity;
    }

    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <value>The entity.</value>
    public T Entity
    {
      get
      {
        return this.entity;
      }
    }
  }
}
