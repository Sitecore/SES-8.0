// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetFromContextSite.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the GetFromSiteContext type.
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
  using System.Linq;
  using Diagnostics;
  using DomainModel.Data;
  using IO;
  using Sitecore.Data.Items;

  /// <summary>
  /// Looks for site settings using context item.
  /// </summary>
  public class GetFromContextSite : GetConfigurationProcessor
  {
    /// <summary>
    /// The settinhs root template Id.
    /// </summary>
    private static readonly string SettingsRootTemplateId = Configuration.Settings.GetSetting("Ecommerce.Settings.SettingsRootTemplateId");

    /// <summary>
    /// Name of the custome site attribute.
    /// </summary>
    private const string EcommerceSiteSettingsAttribute = "EcommerceSiteSettings";

    /// <summary>
    /// Default relative path to site settings item.
    /// </summary>
    private const string EcommerceSiteSettingsDefaultValue = "/Site Settings";

    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public override void Process([NotNull] ConfigurationPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      object container = Context.Entity.Resolve(args.ConfigurationItemType);

      if (!(container is IEntity))
      {
        return;
      }

      if (Sitecore.Context.Site == null)
      {
        return;
      }

      if (Sitecore.Context.Database == null)
      {
        return;
      }

      string settingsPartialPath = Sitecore.Context.Site.Properties[EcommerceSiteSettingsAttribute];
      if (string.IsNullOrEmpty(settingsPartialPath))
      {
        settingsPartialPath = EcommerceSiteSettingsDefaultValue;
      }

      string settingsFullPath = FileUtil.MakePath(Sitecore.Context.Site.StartPath, settingsPartialPath);

      Item settingsItemRoot = Sitecore.Context.Database.GetItem(settingsFullPath);
      if (settingsItemRoot == null)
      {
        return;
      }

      if (settingsItemRoot.Template.ID.ToString() != SettingsRootTemplateId && settingsItemRoot.Template.BaseTemplates.Where(x => x.ID.ToString() == SettingsRootTemplateId).FirstOrDefault() == null)
      {
        return;
      }

      IEntity entity = (IEntity)container;
      Item settingsItem = settingsItemRoot.Axes.SelectSingleItem(string.Format(".//*[@@name='{0}']", entity.Alias));

      if (settingsItem != null)
      {
        this.RegisterInstance(args, (IEntity)container, settingsItem);
      }

      args.AbortPipeline();
    }
  }
}