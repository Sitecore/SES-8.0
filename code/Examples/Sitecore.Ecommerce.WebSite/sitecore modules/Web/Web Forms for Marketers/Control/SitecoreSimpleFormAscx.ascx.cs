// -------------------------------------------------------------------------------------------
// <copyright file="SitecoreSimpleFormAscx.ascx.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Form.Web.UI.Controls
{
  using System;
  using System.Web.UI;
  using Configuration;
  using Diagnostics;
  using Sitecore.Form.Core.Pipelines.RenderForm;
  using Sitecore.Pipelines;
  using Sitecore.Web;

  /// <summary>
  /// Overrides WFM SitecoreSimpleFormAscx control in order to register JavaScript in the right way and to add missing pipelines for Web Forms for Marketers v 2.2.
  /// </summary>
  public partial class SitecoreSimpleFormAscx : Sitecore.Form.Web.UI.Controls.SitecoreSimpleFormAscx
  {
    /// <summary>
    /// Stores value indicating whether Web Forms for Marketers are at version >=2.2 where the postFormRender pipeline is missing
    /// </summary>
    private static bool? isWffm22OrLater;
    
    /// <summary>
    /// Renders the control.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public override void RenderControl(HtmlTextWriter writer)
    {
      var innerWriter = this.IsWffm22OrLater() ? new HtmlTextWriter(new System.IO.StringWriter()) : writer;
      innerWriter.Write("<div class='scfForm' id=\"" + this.ID + "\"");
      this.Attributes.Render(innerWriter);
      innerWriter.Write(">");

      this.RenderControl(innerWriter, this.Adapter);

      innerWriter.Write("</div>");

      if (!this.IsWffm22OrLater())
      {
        return;
      }

      var args = new RenderFormArgs(this.FormItem.InnerItem) 
      { 
        Parameters = WebUtil.ParseQueryString(this.Parameters),
        DisableWebEdit = this.DisableWebEditing
      };
      args.Result.FirstPart = args.Result.FirstPart + innerWriter.InnerWriter;
      using (new LongRunningOperationWatcher(Settings.Profiling.RenderFieldThreshold, "postRenderForm pipeline[id={0}]", this.FormID.ToString()))
      {
        CorePipeline.Run("postRenderForm", args);
      }

      writer.Write(args.Result);
    }

    /// <summary>
    /// Determines whether Web Forms for Marketers are at version >=2.2 with missing postFormRender pipeline
    /// </summary>
    /// <returns>true if Web Forms for Marketers are at version >=2.2, otherwise false</returns>
    private bool IsWffm22OrLater()
    {
      if (!isWffm22OrLater.HasValue)
      {
        foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly.GetName().Name == "Sitecore.Forms.Core")
          {
            var majorVer = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).FileMajorPart;
            var minorVer = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).FileMinorPart;
            if ((majorVer > 2) || ((majorVer == 2) && (minorVer >= 2)))
            {
              isWffm22OrLater = true;
              break;
            }
          }
        }

        isWffm22OrLater = isWffm22OrLater.GetValueOrDefault();
      }

      return isWffm22OrLater.Value;
    }
  }
}