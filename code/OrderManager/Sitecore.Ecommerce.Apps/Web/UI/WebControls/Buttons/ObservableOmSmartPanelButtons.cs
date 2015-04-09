// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableOmSmartPanelButtons.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the observable smart panel buttons class.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls.Buttons
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Web.UI;
  using Diagnostics;
  using OrderManagement.Mixins;
  using Sitecore.Web.UI.WebControls.Extensions;

  /// <summary>
  /// Defines the observable smart panel buttons class.
  /// </summary>
  /// <typeparam name="T">Entity type.</typeparam>
  public class ObservableOmSmartPanelButtons<T> : OmSmartPanelButtons, IChangesObservable<T> where T : class
  {
    /// <summary>
    /// Stores reference to the dictionary of changed entities and corresponding entityChanges.
    /// </summary>
    private readonly IDictionary<T, IDictionary> entityChanges = new Dictionary<T, IDictionary>();

    /// <summary>
    /// Gets the changed entities.
    /// </summary>
    [NotNull]
    public IEnumerable<T> TrackedEntities
    {
      get { return this.entityChanges.Keys; }
    }

    /// <summary>
    /// Gets the entity changes.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>
    /// The entity changes.
    /// </returns>
    public IDictionary GetEntityChanges(T entity)
    {
      Assert.ArgumentNotNull(entity, "entity");

      if (!this.entityChanges.ContainsKey(entity))
      {
        this.entityChanges.Add(entity, new OrderedDictionary());
      }

      return this.entityChanges[entity];
    }

    /// <summary>
    /// Called when the save has click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The arguments.</param>
    protected override void OnSaveClick([CanBeNull]object sender, [CanBeNull]EventArgs args)
    {
      Control parent = this.Page;
      IEnumerable<IChangeTracking> changes = parent.Controls.Flatten<IChangeTracking>();

      foreach (IChangeTracking control in changes)
      {
        BaseFieldEditor fieldEditor = control as BaseFieldEditor;

        if (fieldEditor == null)
        {
          continue;
        }

        T entity = fieldEditor.DataItem as T;

        if (entity != null)
        {
          fieldEditor.NewValuesHolder = this.GetEntityChanges(entity);
        }
      }

      base.OnSaveClick(sender, args);
    }
  }
}
