// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaskedTextBox.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines MastedTextBox class.
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

namespace Sitecore.Ecommerce.Web.UI.WebControls
{
  using System.Web.UI;
  using System.Web.UI.WebControls;

  /// <summary>
  /// Defines MaskedTextBox class.
  /// </summary>
  public class MaskedTextBox : TextBox
  {
    /// <summary>
    /// Gets or sets the masked text.
    /// </summary>
    /// <value>The masked text.</value>
    public string MaskedText
    {
      get { return (string)this.ViewState["MaskedText"] ?? string.Empty; }
      set { this.ViewState["MaskedText"] = value; }
    }

    /// <summary>
    /// Gets or sets the masked CSS style.
    /// </summary>
    /// <value>The masked CSS style.</value>
    public string MaskedCssStyle
    {
      get { return (string)this.ViewState["MaskedCssStyle"] ?? string.Empty; }
      set { this.ViewState["MaskedCssStyle"] = value; }
    }

    /// <summary>
    /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object and stores tracing information about the control if tracing is enabled.
    /// </summary>
    /// <param name="writer">The <see cref="T:System.Web.UI.HTmlTextWriter"/> object that receives the control content.</param>
    public override void RenderControl(HtmlTextWriter writer)
    {
      writer.Write("<span");
      writer.Write(" onfocus=\"this.lastChild.style.zIndex = -1;\" ");
      writer.Write(" onblur=\"this.lastChild.style.zIndex = 'auto';\" style=\"display:block;margin-top:5px;\">");
      writer.Write("<label for=\"{0}\"", this.UniqueID);
      writer.Write(" class=\"{0}\"", this.MaskedCssStyle);
      writer.Write(" style=\"left: 0; z-index: {0};\">", string.IsNullOrEmpty(this.Text) ? "auto" : "-1");
      writer.Write(this.MaskedText);
      writer.Write("</label>");
      base.RenderControl(writer);
      writer.Write("</span>");
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnLoad(System.EventArgs e)
    {
      base.OnLoad(e);

      this.Attributes["onfocus"] = "this.nextSibling.style.zIndex = -1;";
      this.Attributes["onblur"] = "if (this.value == '') { this.nextSibling.style.zIndex = 'auto'; }";
    }
  }
}