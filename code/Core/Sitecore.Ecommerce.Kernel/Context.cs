// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Context.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Represents Ecommerce context.
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

namespace Sitecore.Ecommerce
{
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using Microsoft.Practices.Unity;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Represents Ecommerce context.
  /// </summary>
  public static class Context
  {
    /// <summary>
    /// The container key.
    /// </summary>
    internal const string RequestContainerKey = "SES_REQUEST_CONTAINER";

    /// <summary>
    /// Defines centralized storage for prototypes of shop IoC containers.
    /// </summary>
    private static readonly IDictionary<string, IUnityContainer> ShopContainers = new ConcurrentDictionary<string, IUnityContainer>();

    /// <summary>
    /// Gets the IUnityContainer instance that represents the container of eCommerce providers. 
    /// This is the Dependency Injection (DI) pattern - a special case of the IoC pattern and is
    /// an interface programming technique based on altering class behavior without the 
    /// changing the class internals. Developers code against an interface for the class 
    /// and use a container that injects dependent object instances into the class based 
    /// on the interface or object type. The techniques for injecting object instances
    /// are interface injection, constructor injection, property (setter) injection, and 
    /// method call injection. 
    /// </summary>
    [NotNull]
    private static IIoCContainer applicationContainer = new IoCContainer();

    /// <summary>
    /// The request container.
    /// </summary>
    private static IIoCContainer container;

    /// <summary>
    /// Gets or sets the entity container.
    /// </summary>
    /// <value>The entity.</value>
    /// <exception cref="LicenseException">License is missing.</exception>
    [NotNull]
    public static IIoCContainer Entity
    {
      get
      {
        if (container != null)
        {
          return container;
        }

        var requestContainer = Sitecore.Context.Items[RequestContainerKey] as IIoCContainer;

        return requestContainer ?? applicationContainer;
      }

      set
      {
        container = value;
      }
    }

    /// <summary>
    /// Gets or sets the application IoC container.
    /// </summary>
    /// <value>The application IoC container.</value>
    [NotNull]
    internal static IIoCContainer AppContainer
    {
      get { return applicationContainer; }
      set { applicationContainer = value; }
    }

    /// <summary>
    /// Gets the shop IoC containers. Use shop name as key to get container for specific web shop.
    /// </summary>
    [NotNull]
    internal static IDictionary<string, IUnityContainer> ShopIoCContainers
    {
      get { return ShopContainers; }
    }
  }
}