// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlTemplate.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ControlTemplate type.
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
  using System.Collections.Specialized;
  using System.Linq;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using Diagnostics;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the bindable control template class.
  /// </summary>
  /// <typeparam name="TControl">The type of the control.</typeparam>
  public class ControlTemplate<TControl> : WebControl, IBindableTemplate, IBoundToField where TControl : Control, new()
  {
    /// <summary>
    /// Gets or sets the field.
    /// </summary>
    /// <value>The field.</value>
    public DataField Field { get; set; }

    /// <summary>
    /// Defines the <see cref="T:System.Web.UI.Control"/> object that child controls and templates belong to. These child controls are in turn defined within an inline template.
    /// </summary>
    /// <param name="container">The <see cref="T:System.Web.UI.Control"/> object to contain the instances of controls from the inline template.</param>
    public virtual void InstantiateIn([NotNull] Control container)
    {
      Assert.ArgumentNotNull(container, "container");

      container.Controls.Add(this.InstantiateControl());
      
      Sitecore.Web.UI.WebControls.TemplateField templateField = this.Field as Sitecore.Web.UI.WebControls.TemplateField;
      if (templateField != null)
      {
        if (string.IsNullOrEmpty(container.ID))
        {
          container.ID = templateField.Name;
        }

        templateField.AssociatedControlID = container.ID;
      }
    }

    /// <summary>
    /// When implemented by a class, retrieves a set of name/value pairs for values bound using two-way ASP.NET data-binding syntax within the templated content.
    /// </summary>
    /// <param name="container">The <see cref="T:System.Web.UI.Control"/> from which to extract name/value pairs, which are passed by the data-bound control to an associated data source control in two-way data-binding scenarios.</param>
    /// <returns>
    /// An <see cref="T:System.Collections.Specialized.IOrderedDictionary"/> of name/value pairs. The name represents the name of a control within templated content, and the value is the current value of a property value bound using two-way ASP.NET data-binding syntax.
    /// </returns>
    [NotNull]
    public virtual IOrderedDictionary ExtractValues([NotNull] Control container)
    {
      Assert.ArgumentNotNull(container, "container");
      IOrderedDictionary result = new OrderedDictionary();

      if (this.Field != null)
      {
        ITextControl textControl = this.FindControlInContainer(container) as ITextControl;

        if (textControl != null)
        {
          result.Add(this.Field.Name, textControl.Text);
        }
      }

      return result;
    }

    /// <summary>
    /// Instantiates the control.
    /// </summary>
    /// <returns>The control.</returns>
    [NotNull]
    protected virtual TControl InstantiateControl()
    {
      return new TControl();
    }

    /// <summary>
    /// Finds the control in container.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <returns>The control in container.</returns>
    [CanBeNull]
    protected virtual TControl FindControlInContainer([NotNull] Control container)
    {
      Assert.ArgumentNotNull(container, "container");

      TControl result = container.Controls.Cast<Control>().OfType<TControl>().SingleOrDefault();

      if (result == null)
      {
        foreach (Control control in container.Controls)
        {
          result = this.FindControlInContainer(control);

          if (result != null)
          {
            break;
          }
        }
      }

      return result;
    }
  }
}