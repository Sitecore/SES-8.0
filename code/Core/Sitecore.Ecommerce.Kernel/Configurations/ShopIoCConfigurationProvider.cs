// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopIoCConfigurationProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the shop IoC configuration provider class.
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

namespace Sitecore.Ecommerce.Configurations
{
  using Microsoft.Practices.Unity;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Unity;
  using Sitecore.IO;

  /// <summary>
  /// Defines the shop IoC configuration provider class.
  /// </summary>
  public class ShopIoCConfigurationProvider
  {
    /// <summary>
    /// Configures the IoC container for web shop.
    /// </summary>
    /// <param name="shopName">Name of the shop.</param>
    /// <param name="container">The container to be configured.</param>
    public virtual void ConfigureIoCContainerForWebShop([NotNull] string shopName, [NotNull] IUnityContainer container)
    {
      Assert.ArgumentNotNull(shopName, "shopName");
      Assert.ArgumentNotNull(container, "container");

      string configurationFileName = FileUtil.MapPath(string.Format("/App_Config/{0}.Unity.Config", shopName));
      
      if (FileUtil.Exists(configurationFileName))
      {
        container.LoadConfigurationFromFile(configurationFileName);
      }
    }
  }
}
