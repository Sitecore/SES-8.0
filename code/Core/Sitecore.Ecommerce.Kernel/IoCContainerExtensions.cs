// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoCContainerExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Extension class that adds a set of convenience overloads to the IIoCContainer interface. 
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
  using System;
  using Diagnostics;
  using Microsoft.Practices.Unity;

  /// <summary>
  /// Extension class that adds a set of convenience overloads to the IIoCContainer interface. 
  /// </summary>
  [Obsolete("Use Microsoft.Practices.Unity.UnityContainerExtension instead.")]
  public static class IoCContainerExtensions
  {
    /// <summary>
    /// Resolves the specified type.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="container">The container.</param>
    /// <param name="overrides">The overrides.</param>
    /// <returns>The object instance.</returns>
    public static T Resolve<T>(this IIoCContainer container, params ResolverOverride[] overrides)
    {
      return (T)container.Resolve(typeof(T), overrides);
    }

    /// <summary>
    /// Resolves the specified name.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="container">The container.</param>
    /// <param name="name">The mapping name.</param>
    /// <param name="overrides">Any overrides for the resolve call.</param>
    /// <returns>The object instance.</returns>
    public static T Resolve<T>(this IIoCContainer container, string name, params ResolverOverride[] overrides)
    {
      return (T)container.Resolve(typeof(T), name, overrides);
    }

    /// <summary>
    /// Resolves the specified type.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="type">Type of object to get from the container.</param>
    /// <param name="overrides">The overrides.</param>
    /// <returns>The object instance.</returns>
    public static object Resolve(this IIoCContainer container, Type type, params ResolverOverride[] overrides)
    {
      return container.Resolve(type, string.Empty, overrides);
    }

    /// <summary>
    /// Registers the instance.
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <param name="container">The container.</param>
    /// <param name="instance">The instance.</param>
    /// <returns>The UnityContainer object that this method was called on (this in C#, Me in Visual Basic).</returns>
    public static IUnityContainer RegisterInstance<TInterface>(this IIoCContainer container, TInterface instance)
    {
      Assert.ArgumentNotNull(instance, "instance");

      return container.RegisterInstance(null, instance, new ContainerControlledLifetimeManager());
    }

    /// <summary>
    /// Registers the instance.
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <param name="container">The container.</param>
    /// <param name="name">The name of the instance.</param>
    /// <param name="instance">The instance.</param>
    /// <returns>
    /// The UnityContainer object that this method was called on (this in C#, Me in Visual Basic).
    /// </returns>
    public static IUnityContainer RegisterInstance<TInterface>(this IUnityContainer container, string name, TInterface instance)
    {
      Assert.ArgumentNotNull(instance, "instance");

      return container.RegisterInstance(typeof(TInterface), name, instance, new ContainerControlledLifetimeManager());
    }
  }
}