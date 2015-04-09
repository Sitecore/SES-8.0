// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BranchLinkUpdater.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the class which performs actual link updating.
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

namespace Sitecore.Ecommerce.CommandTemplates
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Text;

  /// <summary>
  /// Defines the class which performs actual link updating.
  /// </summary>
  public class BranchLinkUpdater
  {
    /// <summary>
    /// The branch item should have the only children.
    /// </summary>
    private const string BranchItemShouldHaveTheOnlyChildren = "Branch item should have the only children";

    /// <summary>
    /// The name token.
    /// </summary>
    private const string NameToken = "$name";

    /// <summary>
    /// The separator.
    /// </summary>
    private const char Separator = '/';

    /// <summary>
    /// Updates the item links.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="branch">The branch.</param>
    public virtual void UpdateItemLinks(Item root, BranchItem branch)
    {
      Assert.IsTrue(branch.InnerItem.Children.Count == 1, BranchItemShouldHaveTheOnlyChildren);

      string nameTokenValue = root.Name;
      Dictionary<ID, string> relativePathMapping = new Dictionary<ID, string>();

      this.BuildRelativePath(new StringBuilder(), nameTokenValue, branch.InnerItem.Children.Single(), relativePathMapping);

      this.UpdateLinks(root, root, relativePathMapping);
    }

    /// <summary>
    /// Builds the relative path.
    /// </summary>
    /// <param name="parentPath">The parent path.</param>
    /// <param name="nameTokenValue">The name token value.</param>
    /// <param name="root">The root.</param>
    /// <param name="mapping">The mapping.</param>
    private void BuildRelativePath(StringBuilder parentPath, string nameTokenValue, Item root, IDictionary<ID, string> mapping)
    {
      mapping.Add(root.ID, parentPath.ToString());

      foreach (Item child in root.Children)
      {
        int length = parentPath.Length;
        
        if (length > 0)
        {
          parentPath.Append(Separator);
        }

        parentPath.Append(child.Name.Replace(NameToken, nameTokenValue));
        
        this.BuildRelativePath(parentPath, nameTokenValue, child, mapping);

        parentPath.Length = length;
      }
    }

    /// <summary>
    /// Updates the links.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="currentItem">The current item.</param>
    /// <param name="mapping">The mapping.</param>
    private void UpdateLinks(Item root, Item currentItem, IDictionary<ID, string> mapping)
    {
      using (new EditContext(currentItem))
      {
        foreach (Field field in currentItem.Fields)
        {
          ListString references = new ListString(field.Value);

          if (references.All(ID.IsID))
          {
            ListString updatedReferences = new ListString();
            
            foreach (string reference in references)
            {
              ID key = ID.Parse(reference);
              if (mapping.ContainsKey(key))
              {
                updatedReferences.Add(root.Axes.GetDescendant(mapping[key]).ID.ToString());
              }
              else
              {
                updatedReferences.Add(reference);
              }
            }

            field.Value = updatedReferences.ToString();
          }
        }
      }

      foreach (Item child in currentItem.Children)
      {
        this.UpdateLinks(root, child, mapping);
      }
    }
  }
}
