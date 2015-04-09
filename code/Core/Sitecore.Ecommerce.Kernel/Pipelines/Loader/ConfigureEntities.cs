// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureEntities.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Initializes the Unity container configuration on the first start.
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
  using Diagnostics;
  using DomainModel.Prices;
  using IO;
  using Microsoft.Practices.Unity;
  using Prices;
  using Sitecore.Ecommerce.Unity;
  using Sitecore.Pipelines;

  /// <summary>
  /// Initializes the Unity container configuration on the first start.
  /// </summary>
  public class ConfigureEntities
  {
    /// <summary>
    /// Gets or sets the unity configuration file source.
    /// </summary>
    /// <value>The unity configuration file source.</value>
    public string UnityConfigSource { get; set; }

    /// <summary>
    /// Processes the specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void Process(PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      Assert.IsNotNullOrEmpty(this.UnityConfigSource, "Unity config source is not set.");

      string configFileName = FileUtil.MapPath(this.UnityConfigSource);

      Context.Entity.RegisterExtension<TypeTrackingExtension>();
      Context.Entity.RegisterType(typeof(TotalsFactory), typeof(DefaultTotalsFactory), null, null, new InjectionConstructor(((IoCContainer)Context.Entity).InnerContainer));
      Context.Entity.LoadConfigurationFromFile(configFileName);

      args.CustomData["UnityContainer"] = Context.Entity;
    }
  }
}