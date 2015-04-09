// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResetClonedDetailListHeadersPostStep.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the reset detail list headers class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Install
{
  using System.Collections.Specialized;
  using System.Linq;
  using Diagnostics;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Install.Framework;
  using StringExtensions;

  /// <summary>
  /// Defines the reset detail list headers class.
  /// </summary>
  public class ResetClonedDetailListHeadersPostStep : IPostStep
  {
    /// <summary>
    /// The order list template item id.
    /// </summary>
    [NotNull]
    private static readonly ID OrderListTemplateItemID = new ID("{301008C7-E382-4FDF-8DDE-A31746CD40FC}");

    /// <summary>
    /// The '/sitecore/templates/Speak/Column fields/Column Field' template id.
    /// </summary>
    private static readonly ID ColumnFieldTemplateID = new ID("{6A296415-6F3E-448A-B818-D6FFC596A43E}");

    /// <summary>
    /// The header columns name
    /// </summary>
    private const string HeaderName = "HeaderText";

    /// <summary>
    /// Runs this post step.
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="metaData">The meta data.</param>
    public virtual void Run(ITaskOutput output, NameValueCollection metaData)
    {
      Item detailListTemplate = Database.GetDatabase("master").GetItem(OrderListTemplateItemID);

      foreach (Item column in detailListTemplate.Children)
      {
        foreach (Item columnClone in Globals.LinkDatabase.GetReferrers(column).Select(link => link.GetSourceItem()))
        {
          if (columnClone.TemplateID != ColumnFieldTemplateID)
          {
            continue;
          }

          string expectedHeader = column.Fields[HeaderName].Value;
          string clonnedHeader = columnClone.Fields[HeaderName].Value;
          if (clonnedHeader == expectedHeader)
          {
            continue;
          }

          using (new EditContext(columnClone))
          {
            columnClone.Fields[HeaderName].Reset();
            Log.Info("Resetting detail list header for \"{0}\" column.".FormatWith(columnClone.Paths.FullPath), this);
          }
        }
      }
    }
  }
}