// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetFromWebSite.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the GetFromWebSite type.
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

namespace Sitecore.Ecommerce.Pipelines.GetConfiguration
{
  using Configuration;
  using Data;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sites;
  using Utils;
  using Context = Sitecore.Context;

  /// <summary>
  /// Looks for site settings trying to resolve a website using context item.
  /// </summary>
  public class GetFromWebSite : GetFromContextSite
  {
    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public override void Process(ConfigurationPipelineArgs args)
    {
      Item item = Context.Item;
      if (item == null && !string.IsNullOrEmpty(Sitecore.Web.WebUtil.GetQueryString()))
      {
        ItemUri uri = ItemUri.ParseQueryString();
        if (uri != null)
        {
          item = Database.GetItem(uri);
        }
      }

      if (item == null)
      {
        return;
      }

      args.CustomData["item"] = item;

      string siteName = SiteUtils.GetSiteByItemPath(item.Paths.FullPath);
      if (string.IsNullOrEmpty(siteName))
      {
        return;
      }

      using (new SiteContextSwitcher(Factory.GetSite(siteName)))
      {
        using (new SiteIndependentDatabaseSwitcher(item.Database))
        {
          base.Process(args);
        }
      }
    }
  }
}