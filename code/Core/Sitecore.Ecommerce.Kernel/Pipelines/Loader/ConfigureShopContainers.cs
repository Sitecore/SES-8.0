// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureShopContainers.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the configure webshop entities class.
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

namespace Sitecore.Ecommerce.Pipelines.Loader
{
  using Microsoft.Practices.Unity;
  using Sitecore.Ecommerce.Configurations;
  using Sitecore.Pipelines;

  /// <summary>
  /// Configures IoC containers for each webshop on application start.
  /// Each container is a child of default application unity container
  /// and can be customized and reconifgured without impact on other shop containers. 
  /// </summary>
  public class ConfigureShopContainers
  {
    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">
    /// The arguments. CustomData["UnityContainer_webSiteName"] will contain configured 
    /// IoC container for website after processor execution. 
    /// </param>
    public void Process(PipelineArgs args)
    {
      ShopContextFactory shopContextFactory = Context.AppContainer.Resolve<ShopContextFactory>();
      ShopIoCConfigurationProvider shopIoCConfigurationProvider = Context.AppContainer.Resolve<ShopIoCConfigurationProvider>();

      foreach (ShopContext shopContext in shopContextFactory.GetWebShops())
      {
        IUnityContainer shopContainer = Context.AppContainer.CreateChildContainer();

        shopIoCConfigurationProvider.ConfigureIoCContainerForWebShop(shopContext.InnerSite.Name, shopContainer);

        Context.ShopIoCContainers[shopContext.InnerSite.Name] = shopContainer;
        args.CustomData[GetPipelineArgumentKey(shopContext.InnerSite.Name)] = shopContainer;
      }
    }

    /// <summary>
    /// Gets the pipeline argument key.
    /// </summary>
    /// <param name="siteName">Name of the site.</param>
    /// <returns>The pipeline argument key.</returns>
    [NotNull]
    private static string GetPipelineArgumentKey([CanBeNull] string siteName)
    {
      return string.Format("UnityContainer_{0}", siteName);
    }
  }
}