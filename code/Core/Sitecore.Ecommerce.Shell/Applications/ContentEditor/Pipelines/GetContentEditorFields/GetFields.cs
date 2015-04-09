// -------------------------------------------------------------------------------------------
// <copyright file="GetFields.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Shell.Applications.ContentEditor.Pipelines.GetContentEditorFields
{
  using Sitecore.Data.Fields;
  using Sitecore.Data.Templates;
  using Sitecore.Shell;
  using Text;

  public class GetFields : Sitecore.Shell.Applications.ContentEditor.Pipelines.GetContentEditorFields.GetFields
  {
    /// <summary>
    /// Fields to hide.
    /// </summary>
    private ListString fieldsToHide = new ListString();

    /// <summary>
    /// Gets or sets the hidden fields.
    /// </summary>
    /// <value>The hidden fields.</value>
    public virtual string HiddenFields { get; set; }

    /// <summary>
    /// Gets the fields to hide.
    /// </summary>
    /// <value>The fields to hide.</value>
    protected virtual ListString FieldsToHide
    {
      get
      {
        if (!string.IsNullOrEmpty(this.HiddenFields) && this.fieldsToHide.Count == 0)
        {
          this.fieldsToHide = new ListString(this.HiddenFields);
        }

        return this.fieldsToHide;
      }
    }

    /// <summary>
    /// Determines whether this instance can show the field.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="templateField">The template field.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can show the field the specified field; otherwise, <c>false</c>.
    /// </returns>
    protected override bool CanShowField([NotNull] Field field, [NotNull] TemplateField templateField)
    {
      Diagnostics.Assert.ArgumentNotNull(field, "field");
      Diagnostics.Assert.ArgumentNotNull(templateField, "templateField");

      if (!UserOptions.ContentEditor.ShowSystemFields)
      {
        if (this.FieldsToHide.Contains(field.ID.ToString()) || FieldsToHide.Contains(field.Name))
        {
          return false;
        }
      }

      return base.CanShowField(field, templateField);
    }
  }
}