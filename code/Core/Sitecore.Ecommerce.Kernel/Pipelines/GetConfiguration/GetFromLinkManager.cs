// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetFromLinkManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Looks for site settings using Link Database.
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
  using DomainModel.Data;
  using Links;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// Looks for site settings using Link Database.
  /// </summary>
  public class GetFromLinkManager : GetConfigurationProcessor
  {
    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public override void Process(ConfigurationPipelineArgs args)
    {
      object container = Context.Entity.Resolve(args.ConfigurationItemType, null);

      if (!(container is IEntity))
      {
        return;
      }

      Item item = args.CustomData["item"] as Item;
      if (item == null)
      {
        return;
      }

      LinkDatabase linkDatabase = Globals.LinkDatabase;
      ItemLink[] links = linkDatabase.GetReferrers(item);

      foreach (ItemLink link in links)
      {
        Item source = link.GetSourceItem();
        if (source.Parent.TemplateID != new ID(Configuration.Settings.GetSetting("Ecommerce.Settings.SettingsRootTemplateId")))
        {
          continue;
        }

        this.RegisterInstance(args, (IEntity)container, source);
        args.AbortPipeline();

        break;
      }
    }
  }
}