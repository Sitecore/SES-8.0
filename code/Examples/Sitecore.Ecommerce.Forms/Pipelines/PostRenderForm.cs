// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostRenderForm.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Represents PostRenderForm pipeline processor. Used to modify rendered form by HtmlAgilityPack.
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

namespace Sitecore.Ecommerce.Forms.Pipelines
{
  using System.Linq;
  using System.Web;
  using System.Xml.Linq;
  using Form.Core.Pipelines.RenderForm;
  using Reflection;
  using Sitecore.Exceptions;

  /// <summary>
  ///   Represents PostRenderForm pipeline processor. Used to modify rendered form by HtmlAgilityPack.
  /// </summary>
  public class PostRenderForm
  {
    /// <summary>
    /// Gets a value indicating whether this instance is post back.
    /// </summary>
    /// <value><c>true</c> if this instance is post back; otherwise, <c>false</c>.</value>
    private static bool IsPostback
    {
      get
      {
        if (HttpContext.Current != null)
        {
          return HttpContext.Current.Request.Form.Count > 0;
        }

        return false;
      }
    }

    /// <summary>
    /// Processes the specified args.
    /// </summary>
    /// <param name="args">The render form args.</param>
    public void Process(RenderFormArgs args)
    {
      // replaces legends with div tags
      var form = new HtmlFormModifier(args);
      form.ReplaceLegendWithDiv();
      form.RemoveEmptyTags("div", "scfSectionUsefulInfo");
      form.RemoveEmptyTags("div", "scfTitleBorder");
      form.RemoveEmptyTags("div", "scfIntroBorder");
      form.RemoveEmptyTags("span", "scfError");
      form.SurroundContentWithUlLi("scfError");

      form.RemoveNbsp();

      var saveActions = args.Item["Save Actions"];
      if (!string.IsNullOrEmpty(saveActions))
      {
        var commands = XDocument.Parse(saveActions);
        var actionIds = (from c in commands.Descendants()
                                  where c.Name.LocalName == "li"
                                        && (c.Attribute(XName.Get("id")) != null)
                                  select c.Attribute(XName.Get("id")).Value).ToList();

        foreach (var actionId in actionIds)
        {
          var actionItem = args.Item.Database.GetItem(actionId);
          if (null == actionItem)
          {
            continue;
          }

          var assembly = actionItem["assembly"];
          var className = actionItem["Class"];

          if (string.IsNullOrEmpty(assembly) || string.IsNullOrEmpty(className))
          {
            continue;
          }

          var obj = ReflectionUtil.CreateObject(assembly, className, new object[] { });
          if (obj == null)
          {
            throw new ConfigurationException("Could not load " + className + " from " + assembly);
          }

          var method = ReflectionUtil.GetMethod(obj, "Load", new object[] { IsPostback, args });
          if (method != null)
          {
            ReflectionUtil.InvokeMethod(method, new object[] { IsPostback, args }, obj);
          }
        }
      }
    }
  }
}