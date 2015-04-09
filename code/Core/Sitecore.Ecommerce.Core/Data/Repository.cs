// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the repository class.
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
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;

  /// <summary>
  /// Defines the repository class.
  /// </summary>
  /// <typeparam name="T">The type of repository entity.</typeparam>
  public abstract class Repository<T>
  {
    /// <summary>
    /// Occurs when entity is abount to be saved.
    /// </summary>
    public event EventHandler<SaveEntityEventArgs<T>> EntitySaving;

    /// <summary>
    /// Occurs when entity is saved.
    /// </summary>
    public event EventHandler<EntityEventArgs<T>> EntitySaved;

    /// <summary>
    /// Saves the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    internal virtual void Save([NotNull]IEnumerable<T> entities)
    {
      Debug.ArgumentNotNull(entities, "entities");

      entities = entities.ToList();

      foreach (T entity in entities)
      {
        this.OnEntitySaving(new SaveEntityEventArgs<T>(this.GetPersistedEntity(entity), entity));
      }

      this.PerformSaving(entities);

      foreach (T entity in entities)
      {
        this.OnEntitySaved(new EntityEventArgs<T>(entity));
      }
    }

    /// <summary>
    /// Gets all the repository items.
    /// </summary>
    /// <returns>
    /// The collection of the repository items.
    /// </returns>
    [NotNull]
    protected internal abstract IQueryable<T> GetAll();

    /// <summary>
    /// Performs the saving.
    /// </summary>
    /// <param name="entities">The entities.</param>
    protected internal abstract void PerformSaving([NotNull] IEnumerable<T> entities);

    /// <summary>
    /// Deletes the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    protected internal abstract void Delete([NotNull]IEnumerable<T> entities);

    /// <summary>
    /// Gets the persisted entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>The persisted entity.</returns>
    [CanBeNull]
    protected abstract T GetPersistedEntity([NotNull] T entity);

    /// <summary>
    /// Called when the entity has saving.
    /// </summary>
    /// <param name="e">The <see cref="SaveEntityEventArgs{T}"/> instance containing the event data.</param>
    protected virtual void OnEntitySaving([NotNull] SaveEntityEventArgs<T> e)
    {
      Debug.ArgumentNotNull(e, "e");

      EventHandler<SaveEntityEventArgs<T>> handler = this.EntitySaving;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Called when the entity has saved.
    /// </summary>
    /// <param name="e">The <see cref="Sitecore.Ecommerce.Data.EntityEventArgs&lt;T&gt;"/> instance containing the event data.</param>
    protected virtual void OnEntitySaved([NotNull] EntityEventArgs<T> e)
    {
      Debug.ArgumentNotNull(e, "e");

      EventHandler<EntityEventArgs<T>> handler = this.EntitySaved;
      if (handler != null)
      {
        handler(this, e);
      }
    }
  }
}