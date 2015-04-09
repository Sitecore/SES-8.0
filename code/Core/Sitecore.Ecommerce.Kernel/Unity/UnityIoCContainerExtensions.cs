// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityIoCContainerExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the unity IoC container extensions class.
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

namespace Sitecore.Ecommerce.Unity
{
  using System;
  using System.Configuration;
  using Microsoft.Practices.Unity;
  using Microsoft.Practices.Unity.Configuration;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Defines the unity IoC container extensions class.
  /// </summary>
  public static class UnityIoCContainerExtensions
  {
    /// <summary>
    /// Loads the configuration from file.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>
    /// The configuration from file.
    /// </returns>
    [NotNull]
    public static IUnityContainer LoadConfigurationFromFile([NotNull] this IUnityContainer container, [NotNull] string fileName)
    {
      Assert.ArgumentNotNull(container, "container");
      Assert.ArgumentNotNull(fileName, "fileName");

      ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
      {
        ExeConfigFilename = fileName
      };

      Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
      UnityConfigurationSection section = config.GetSection("unity") as UnityConfigurationSection;

      return container.LoadConfiguration(section);
    }

    /// <summary>
    /// Should be used when named registration is resolved but there is a possibility that instance with supplied name was not registered in Unity container. 
    /// In such cases simple type resolve is performed.
    /// </summary>
    /// <typeparam name="T">Registered in Unity container Type.</typeparam>
    /// <param name="container">The container.</param>
    /// <param name="name">The name.</param>
    /// <returns>Resolved entity.</returns>
    [NotNull]
    public static T SmartResolve<T>([NotNull] this IUnityContainer container, [NotNull] string name)
      where T : class
    {
      Assert.ArgumentNotNull(container, "container");
      Assert.ArgumentNotNull(name, "name");

      return container.HasRegistration(typeof(T), name) ? container.Resolve<T>(name) : container.Resolve<T>();
    }

    /// <summary>
    /// Determines whether the specified type has registration in container. This is thread safe replacement of Unity container IsRegistered method.
    /// IsRegistered should not be used in production environment.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="type">The type.</param>
    /// <param name="name">The name.</param>
    /// <returns>
    ///   <c>true</c> if the specified type has registration; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasRegistration([NotNull] this IUnityContainer container, [NotNull] Type type, [NotNull] string name)
    {
      Assert.ArgumentNotNull(container, "container");
      Assert.ArgumentNotNull(type, "type");
      Assert.ArgumentNotNull(name, "name");

      return GetExtension(container).IsRegistered(type, name);
    }

    /// <summary>
    /// Determines whether the specified type has registration in container. This is thread safe replacement of Unity container IsRegistered method.
    /// IsRegistered should not be used in production environment.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="type">The type.</param>
    /// <returns>
    ///   <c>true</c> if the specified type has registration; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasRegistration([NotNull] this IUnityContainer container, [NotNull] Type type)
    {
      Assert.ArgumentNotNull(container, "container");
      Assert.ArgumentNotNull(type, "type");

      return GetExtension(container).IsRegistered(type);
    }

    /// <summary>
    /// Registers the extension only once. If such an extension is already registered, skips registration.
    /// </summary>
    /// <typeparam name="T">Unity container extension type.</typeparam>
    /// <param name="container">The container.</param>
    public static void RegisterExtension<T>([NotNull] this IUnityContainer container)
      where T : UnityContainerExtension
    {
      Assert.ArgumentNotNull(container, "container");

      if (container.Configure<TypeTrackingExtension>() == null)
      {
        container.AddNewExtension<T>();
      }
    }

    /// <summary>
    /// Gets the TypeTrackingExtension instance. If instance is not registered in Unity container, throws an exception.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <returns>The TypeTrackingExtension instance.</returns>
    [NotNull]
    private static TypeTrackingExtension GetExtension([NotNull] IUnityContainer container)
    {
      Assert.ArgumentNotNull(container, "container");
      var extension = container.Configure<TypeTrackingExtension>();

      Assert.IsNotNull(container, "TypeTrackingExtension is not registered in Unity container.");

      return extension;
    }
  }
}
