// -------------------------------------------------------------------------------------------
// <copyright file="ClearSiteSettingsCacheEventHandler.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Unity
{
  using System;
  using System.Linq;
  using Diagnostics;
  using DomainModel.Data;
  using Events;
  using Microsoft.Practices.Unity;
  using Sitecore.Data.Items;
  using Utils;

  /// <summary>
  /// The Refresh Settings event class.
  /// </summary>
  public class ClearSiteSettingsCacheEventHandler
  {
    /// <summary>
    /// The settinhs root template Id.
    /// </summary>
    private readonly string settingsRootTemplateId = Configuration.Settings.GetSetting("Ecommerce.Settings.SettingsRootTemplateId");

    /// <summary>
    /// Called when item is saved
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public void OnItemSaved(object sender, EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

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

      if (!parent.TemplateID.ToString().Equals(this.settingsRootTemplateId))
      {
        return;
      }

      Log.Info(string.Concat("RefreshSettingsEventHandler clearing site settings caches for item: (", item.Paths.FullPath, ")."), this);

      string siteName = SiteUtils.GetSiteByItem(item);
      if (string.IsNullOrEmpty(siteName))
      {
        return;
      }

      string entityName = string.Format("{0}_{1}", siteName, item.Database.Name);

      RegisterInstanceEventArgs arg = QueryableContainerExtension.Instances.FirstOrDefault(e => e.Instance != null && e.Instance is IEntity
        && string.Equals(e.Name, entityName, StringComparison.OrdinalIgnoreCase) && ((IEntity)e.Instance).Alias.Equals(item.ID.ToString()));

      if (arg != null)
      {
        QueryableContainerExtension.Instances.Remove(arg);
      }
    }
  }
}