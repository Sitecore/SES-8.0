// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseFieldEditor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the BaseFieldEditor type.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls
{
  using System.Collections;
  using Diagnostics;
  using Sitecore.Web.UI.WebControls;
  using Speak.Web.UI.WebControls;

  /// <summary>
  /// Defines the base field editor class.
  /// </summary>
  public class BaseFieldEditor : ObjectFieldEditor
  {
    /// <summary>
    /// Gets the inner model.
    /// </summary>
    [CanBeNull]
    public object DataItem
    {
      get
      {
        return this.fieldEditorLeft.DataItem ?? this.fieldEditorRight.DataItem;
      }
    }

    /// <summary>
    /// Gets or sets the change storage.
    /// </summary>
    /// <value>The change storage.</value>
    public IDictionary NewValuesHolder { get; set; }

    /// <summary>
    /// Gets or sets the old values.
    /// </summary>
    /// <value>The old values.</value>
    public IDictionary OldValuesHolder { get; set; }

    /// <summary>
    /// Gets the field.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>The field.</returns>
    protected override DataField GetField(Sitecore.Data.Items.Item item)
    {
      DataField field = base.GetField(item);

      if (string.IsNullOrEmpty(field.Name))
      {
        field.Name = item["Name"];
      }

      TemplateField templateField = field as TemplateField;

      if (templateField != null)
      {
        if (!(templateField.EditTemplate is FieldEditTemplate))
        {
          templateField.ReadTemplate = templateField.EditTemplate;
        }

        this.SetField(templateField.ReadTemplate, templateField);
        this.SetField(templateField.EditTemplate, templateField);
      }

      return field;
    }

    /// <summary>
    /// Called when the field editors has updating.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="Sitecore.Web.UI.WebControls.FieldEditorUpdatingEventArgs"/> instance containing the event data.</param>
    protected override void OnFieldEditorsUpdating([NotNull] object sender, [NotNull] FieldEditorUpdatingEventArgs e)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(e, "e");

      if (this.OldValuesHolder != null)
      {
        this.MergeDictionaries(e.OldValues, this.OldValuesHolder);
      }

      if (this.NewValuesHolder != null)
      {
        this.MergeDictionaries(e.NewValues, this.NewValuesHolder);
      }

      e.Cancel = true;

      base.OnFieldEditorsUpdating(sender, e);
    }

    /// <summary>
    /// Sets the field.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="dataField">The data field.</param>
    private void SetField([CanBeNull] object obj, [NotNull] DataField dataField)
    {
      Assert.ArgumentNotNull(dataField, "dataField");

      IBoundToField boundToField = obj as IBoundToField;

      if (boundToField != null)
      {
        boundToField.Field = dataField;
      }
    }

    /// <summary>
    /// Merges the dictionaries.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    private void MergeDictionaries([NotNull] IDictionary source, [NotNull] IDictionary destination)
    {
      Assert.ArgumentNotNull(source, "source");
      Assert.ArgumentNotNull(destination, "destination");

      foreach (DictionaryEntry entry in source)
      {
        if (destination.Contains(entry.Key))
        {
          destination[entry.Key] = entry.Value;
        }
        else
        {
          destination.Add(entry.Key, entry.Value);
        }
      }
    }
  }
}