// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureUnityContainer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ConfigureUnityContainer type.
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

namespace Sitecore.Ecommerce.Apps.Pipelines.Loader
{
  using Microsoft.Practices.Unity;
  using OrderManagement.DataSources;
  using Sitecore.Pipelines;

  /// <summary>
  /// Defines the ConfigureUnityContainer type.
  /// </summary>
  public class ConfigureUnityContainer
  {
    /// <summary>
    /// Processes the specified args.
    /// </summary>
    /// <param name="args">The args.</param>
    public void Process(PipelineArgs args)
    {
      IUnityContainer container = args.CustomData["UnityContainer"] as IUnityContainer;

      ShopContextFactory factory = container.Resolve<ShopContextFactory>();
      container.RegisterType<ContextSwitcherDataSourceBase, ContentContextSwitcherDataSource>(new InjectionConstructor(factory));
    }
  }
}