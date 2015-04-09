// -------------------------------------------------------------------------------------------
// <copyright file="DropDownList.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess
{
  using System.Collections.Generic;  
  using System.Web;
  using System.Web.UI;
  using System.Web.UI.WebControls;

  /// <summary>
  /// DropDownList wrapper
  /// </summary>
  public class DropDownList : System.Web.UI.WebControls.DropDownList
  {
    /// <summary>
    /// Renders the items in the <see cref="T:System.Web.UI.WebControls.ListControl"/> control.
    /// </summary>
    /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream used to write content to a Web page.</param>
    protected override void RenderContents(HtmlTextWriter writer)
    {      
      string currentOptionGroup;
      List<string> renderedOptionGroups = new List<string>();

      foreach (ListItem item in this.Items)
      {
        if (item.Attributes["OptionGroup"] != null)
        {
          // 'The item is part of an option group' 
          currentOptionGroup = item.Attributes["OptionGroup"];

          // 'the option header was already written, just render the list item' 
          if (renderedOptionGroups.Contains(currentOptionGroup))
          {
            this.RenderListItem(item, writer);
          }
          else
          {
            // the header was not written- do that first' 
            if (renderedOptionGroups.Count > 0)
            {
              this.RenderOptionGroupEndTag(writer); // 'need to close previous group' 
            }

            this.RenderOptionGroupBeginTag(currentOptionGroup, writer);
            renderedOptionGroups.Add(currentOptionGroup);
            this.RenderListItem(item, writer);
          }
        }
        else if (item.Text == "--")
        {
          // simple separator 
          this.RenderOptionGroupBeginTag("--", writer);
          this.RenderOptionGroupEndTag(writer);
        }
        else
        {
          // default behavior: render the list item as normal' 
          this.RenderListItem(item, writer);
        }
      }

      if (renderedOptionGroups.Count > 0)
      {
        this.RenderOptionGroupEndTag(writer);
      }
    }

    /// <summary>
    /// The render list item.
    /// </summary>
    /// <param name="item"> The list item. </param>
    /// <param name="writer"> The writer. </param>
    protected virtual void RenderListItem(ListItem item, HtmlTextWriter writer)
    {
      writer.WriteBeginTag("option");
      writer.WriteAttribute("value", item.Value, true);
      if (item.Selected)
      {
        writer.WriteAttribute("selected", "selected", false);
      }

      foreach (string key in item.Attributes.Keys)
      {
        writer.WriteAttribute(key, item.Attributes[key]);
      }

      writer.Write(HtmlTextWriter.TagRightChar);
      HttpUtility.HtmlEncode(item.Text, writer);
      writer.WriteEndTag("option");
      writer.WriteLine();
    }

    /// <summary>
    /// The render option group begin tag.
    /// </summary>
    /// <param name="name"> The option name. </param>
    /// <param name="writer"> The writer. </param>
    protected virtual void RenderOptionGroupBeginTag(string name, HtmlTextWriter writer)
    {
      writer.WriteBeginTag("optgroup");
      writer.WriteAttribute("label", name);
      writer.Write(HtmlTextWriter.TagRightChar);
      writer.WriteLine();
    }

    /// <summary>
    /// The render option group end tag.
    /// </summary>
    /// <param name="writer"> The writer. </param>
    protected virtual void RenderOptionGroupEndTag(HtmlTextWriter writer)
    {
      writer.WriteEndTag("optgroup");
      writer.WriteLine();
    }
  }
}