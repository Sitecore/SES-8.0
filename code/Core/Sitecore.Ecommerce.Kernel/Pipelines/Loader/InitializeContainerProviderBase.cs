// -------------------------------------------------------------------------------------------
// <copyright file="InitializeContainerProviderBase.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Pipelines.Loader
{
  using System;
  using System.Collections.Specialized;
  using System.Configuration.Provider;
  using DomainModel.Data;
  using Unity;

  /// <summary>
  /// Initialize container provider base.
  /// </summary>
  public abstract class InitializeContainerProviderBase
  {
    /// <summary>
    /// Registers the provider.
    /// </summary>
    /// <typeparam name="T">The container interface.</typeparam>
    /// <param name="name">The provider's name.</param>
    /// <param name="config">The provider's config.</param>
    public virtual void RegisterProvider<T>(string name, NameValueCollection config) where T : class
    {
      IEntityProvider<T> provider = Context.Entity.Resolve<IEntityProvider<T>>();

      var providerBase = (ProviderBase)provider;

#if (DEBUG)
      try
      {
        providerBase.Initialize(name, config);
      }
      catch (InvalidOperationException)
      {
      }
#else
      providerBase.Initialize(name, config);
#endif

      Context.Entity.RegisterInstance(provider);
    }
  }
}