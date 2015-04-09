// -------------------------------------------------------------------------------------------
// <copyright file="EnableStructuredDataModule.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.StructuredData
{
  using System;
  using Events;
  using Sitecore.Configuration;
  using Sitecore.Data.Items;

  /// <summary>
  /// The item event handler
  /// </summary>
  public class EnableStructuredDataModule
  {
    /// <summary>
    /// Called when item is saved
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public void OnItemSaved(object sender, EventArgs args)
    {
      Item item = Event.ExtractParameter(args, 0) as Item;
      
      if (item == null)
      {
        return;
      }

      Item parent = item.Parent;

      if (parent == null)
      {
        return;
      }

      if (Settings.GetBoolSetting("Ecommerce.EnableStructuredDataModule", true))
      {
        // The item is in the first level of the structured tree.
        if (ItemIsRootOfStructuredTree(parent) && !item.TemplateName.Equals("StructuredData StructuredFolder"))
        {
          var tree = new StructuredTree(parent);
          tree.AddItem(item);
        }
      }
    }

    /// <summary>
    /// Check if items the is root item of a structured tree.
    /// </summary>
    /// <param name="item">The current item.</param>
    /// <returns>if items the is root item of a structured tree.</returns>
    private static bool ItemIsRootOfStructuredTree(Item item)
    {
      return RecursiveCheckBaseTemplatesIfStructuredTree(item.Template);
    }

    /// <summary>
    /// Cheks if selected templates has "StructuredData StructuredTreeArchive" as one of its base tamplates
    /// </summary>
    /// <param name="templateItem">The template item. </param>
    /// <returns>if selected templates has "StructuredData StructuredTreeArchive".</returns>
    private static bool RecursiveCheckBaseTemplatesIfStructuredTree(TemplateItem templateItem)
    {
      if (templateItem != null
          && !templateItem.Name.Equals("Standard template"))
      {
        if (templateItem.Name.Equals("StructuredData StructuredTreeArchive"))
        {
          return true;
        }

        TemplateItem[] baseTemplates = templateItem.BaseTemplates;
        if (baseTemplates != null)
        {
          foreach (TemplateItem baseTemplateItem in baseTemplates)
          {
            bool success = RecursiveCheckBaseTemplatesIfStructuredTree(baseTemplateItem);
            if (success)
            {
              return true;
            }
          }
        }
      }

      return false;
    }
  }
}