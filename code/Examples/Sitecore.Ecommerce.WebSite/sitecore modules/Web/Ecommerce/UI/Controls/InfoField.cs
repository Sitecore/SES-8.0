// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoField.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015 
// </copyright>
// <summary>
//   The info field control.
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

namespace Sitecore.Ecommerce.Web.UI.Controls
{
  using System;
  using System.Web.UI;
  using Sitecore.Form.Core.Attributes;
  using Sitecore.Form.Core.Controls.Data;
  using Sitecore.Form.Core.Visual;
  using Sitecore.Form.Web.UI.Controls;

  /// <summary>
  /// The info field control.
  /// </summary>
  public class InfoField : HelpControl
  {
    /// <summary>
    /// Gets or sets the information.
    /// </summary>
    /// <value>The information.</value>
    [VisualFieldType(typeof(TextAreaField))]
    [VisualCategory("Appearance")]
    [VisualProperty("HTML:")]
    [Localize]
    public new string Information { get; set; }

    /// <summary>
    /// Gets the control result.
    /// </summary>
    /// <value>The control result.</value>
    public override ControlResult Result
    {
      get
      {
        return new ControlResult(this.ControlName, string.Empty, null);
      }

      set
      {
        this.ControlName = value.FieldName;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.
    /// </returns>
    protected override HtmlTextWriterTag TagKey
    {
      get { return HtmlTextWriterTag.Div; }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      this.Attributes["class"] = "scfSectionUsefulInfo";
      var literal = new LiteralControl(this.Information ?? string.Empty);
      this.Controls.Add(literal);
    }
  }
}