// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetConfigurationProcessor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Base class for all GetConfiguration pipeline processors.
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
  using Data;
  using DomainModel.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// Base class for all GetConfiguration pipeline processors.
  /// </summary>
  public abstract class GetConfigurationProcessor
  {
    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public abstract void Process(ConfigurationPipelineArgs args);

    /// <summary>
    /// Registers the settings object instance.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <param name="container">The container.</param>
    /// <param name="source">The source.</param>
    protected virtual void RegisterInstance(ConfigurationPipelineArgs args, IEntity container, Item source)
    {
      IDataMapper mapper = Context.Entity.Resolve<IDataMapper>();
      args.ConfigurationItem = mapper.GetEntity(source, args.ConfigurationItemType);
    }
  }
}