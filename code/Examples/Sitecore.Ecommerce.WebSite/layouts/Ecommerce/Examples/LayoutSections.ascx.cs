// -------------------------------------------------------------------------------------------
// <copyright file="LayoutSections.ascx.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.UserControls
{
  using System;
  using System.Collections.Generic;
  using System.Web.UI;
  using Diagnostics;
  using Layouts;
  using SecurityModel;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Rules;
  using Sitecore.Rules.ConditionalRenderings;
  using Text;

  /// <summary>
  /// The layout_ sections.
  /// </summary>
  public partial class LayoutSections : UserControl
  {
    #region Protected methods

    /// <summary> Init page handler </summary>
    /// <param name="sender">sender object</param>
    /// <param name="e">event args params</param>
    protected void Page_Init(object sender, EventArgs e)
    {
      Item[] items = Sitecore.Context.Item.Axes.SelectItems("./layout section/*");
      if (items != null)
      {
        foreach (Item item in items)
        {
          foreach (RenderingReference rendering in this.GetRenderings(item))
          {
            UrlString urlString = new UrlString(rendering.Settings.Parameters);
            urlString.Parameters.Add("itemID", item.ID.ToString());
            rendering.Settings.Parameters = urlString.Query;
            this.SectionsContainer.Controls.Add(rendering.GetControl());
          }
        }
      }
    }

    /// <summary> Gets item renderings </summary>
    /// <param name="item">item to proceed</param>
    /// <returns>list of item renderings</returns>
    protected List<RenderingReference> GetRenderings(Item item)
    {
      RuleList<ConditionalRenderingsRuleContext> globalRules = this.GetGlobalRules(item);
      List<RenderingReference> resultCollection = new List<RenderingReference>(item.Visualization.GetRenderings(Sitecore.Context.Device, true));
      foreach (RenderingReference reference in new List<RenderingReference>(resultCollection))
      {
        string conditions = reference.Settings.Conditions;
        if (!string.IsNullOrEmpty(conditions))
        {
          List<Item> conditionItems = this.GetConditionItems(item.Database, conditions);
          if (conditionItems.Count > 0)
          {
            RuleList<ConditionalRenderingsRuleContext> rules = RuleFactory.GetRules<ConditionalRenderingsRuleContext>(conditionItems, "Rule");
            ConditionalRenderingsRuleContext ruleContext = new ConditionalRenderingsRuleContext(resultCollection, reference) { Item = item };
            rules.Run(ruleContext);
          }
        }

        if (globalRules != null)
        {
          ConditionalRenderingsRuleContext globalRuleContext = new ConditionalRenderingsRuleContext(resultCollection, reference) { Item = item };
          globalRules.Run(globalRuleContext);
        }
      }

      return resultCollection;
    }

    /// <summary> Gets global conditional rules </summary>
    /// <param name="item">item to proceed</param>
    /// <returns>rulles collection</returns>
    protected RuleList<ConditionalRenderingsRuleContext> GetGlobalRules(Item item)
    {
      Assert.ArgumentNotNull(item, "item");
      RuleList<ConditionalRenderingsRuleContext> rules = null;
      using (new SecurityDisabler())
      {
        Item parentItem = item.Database.GetItem(ItemIDs.ConditionalRenderingsGlobalRules);
        if (parentItem != null)
        {
          rules = RuleFactory.GetRules<ConditionalRenderingsRuleContext>(parentItem, "Rule");
        }
      }

      return rules;
    }

    /// <summary>
    /// Gets condition items
    /// </summary>
    /// <param name="database">database to proceed</param>
    /// <param name="conditions">string conditions</param>
    /// <returns>returns list of condition items</returns>
    private List<Item> GetConditionItems(Database database, string conditions)
    {
      Assert.ArgumentNotNull(database, "database");
      Assert.ArgumentNotNull(conditions, "conditions");
      List<Item> list = new List<Item>();
      foreach (string str in new ListString(conditions))
      {
        Item item = database.GetItem(str);
        if (item != null)
        {
          list.Add(item);
        }
      }

      return list;
    }

    #endregion
  }
}