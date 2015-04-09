// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveEntityEventArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the save entity event args class.
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
  /// Defines the save entity event args class.
  /// </summary>
  /// <typeparam name="T">The entity type.</typeparam>
  public class SaveEntityEventArgs<T> : EventArgs
  {
    /// <summary>
    /// Stores reference to the old entity instance.
    /// </summary>
    private readonly T oldEntity;

    /// <summary>
    /// Stores reference to the new entity instance.
    /// </summary>
    private readonly T newEntity;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveEntityEventArgs{T}"/> class. 
    /// </summary>
    /// <param name="oldEntity">The old entity.</param>
    /// <param name="newEntity">The new entity.</param>
    public SaveEntityEventArgs([CanBeNull] T oldEntity, [NotNull] T newEntity)
    {
      Assert.ArgumentNotNull(newEntity, "newEntity");

      this.oldEntity = oldEntity;
      this.newEntity = newEntity;
    }

    /// <summary>
    /// Gets the old entity.
    /// </summary>
    /// <value>The old entity.</value>
    public T OldEntity
    {
      get { return this.oldEntity; }
    }

    /// <summary>
    /// Gets the new entity.
    /// </summary>
    /// <value>The new entity.</value>
    public T NewEntity
    {
      get { return this.newEntity; }
    }
  }
}