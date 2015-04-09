// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeTrackingExtension.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The unity type tracking extension.
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
  using System.Collections.Concurrent;
  using Microsoft.Practices.Unity;
  using Sitecore.Diagnostics;

  /// <summary>
  /// The Unity type tracking extension. Tracks instance and type registrations. Thread safe replacement for IsRegistered Unity extension methods.
  /// </summary>
  public class TypeTrackingExtension : UnityContainerExtension
  {
    /// <summary>
    /// The named type format string.
    /// </summary>
    private const string NamedTypeFormatString = "{0}, {1}";

    /// <summary>
    /// Dictionary for regular registered types.
    /// </summary>
    private readonly ConcurrentDictionary<Type, Type> types = new ConcurrentDictionary<Type, Type>();

    /// <summary>
    /// Dictionary for named registered types.
    /// </summary>
    private readonly ConcurrentDictionary<string, Type> namedTypes = new ConcurrentDictionary<string, Type>();

    /// <summary>
    /// Returns whether a type is registered. Provides similar to IsRegistered Unity extension method functionality but is thread safe.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>True if type is registered.</returns>
    public bool IsRegistered([NotNull] Type type)
    {
      Assert.ArgumentNotNull(type, "type");

      return this.types.ContainsKey(type) || (this.Container.Parent != null && this.Container.Parent.HasRegistration(type));
    }

    /// <summary>
    /// Returns whether a type is registered. Provides similar to IsRegistered Unity extension method functionality but is thread safe.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="name">The name the type is registered with.</param>
    /// <returns>True if type is registered.</returns>
    public bool IsRegistered([NotNull] Type type, [NotNull] string name)
    {
      Assert.ArgumentNotNull(type, "type");
      Assert.ArgumentNotNull(name, "name");

      var namedTypeKey = GetNamedTypeKey(type, name);

      return this.namedTypes.ContainsKey(namedTypeKey) || (this.Container.Parent != null && this.Container.Parent.HasRegistration(type, name));
    }

    /// <summary>
    /// Removes the extension's functions from the container.
    /// </summary>
    /// <remarks>
    ///   <para>
    /// This method is called when extensions are being removed from the container. It can be
    /// used to do things like disconnect event handlers or clean up member state. You do not
    /// need to remove strategies or policies here; the container will do that automatically.
    ///   </para>
    ///   <para>
    /// The default implementation of this method does nothing.</para>
    /// </remarks>
    public override void Remove()
    {
      this.Context.RegisteringInstance -= this.ContextOnRegisteringInstance;
      this.Context.Registering -= this.ContextOnRegistering;

      base.Remove();
    }

    /// <summary>
    /// Initial the container with this extension's functionality.
    /// </summary>
    /// <remarks>
    /// When overridden in a derived class, this method will modify the given
    /// <see cref="T:Microsoft.Practices.Unity.ExtensionContext" /> by adding strategies, policies, etc. to
    /// install it's functions into the container.
    /// </remarks>
    protected override void Initialize()
    {
      this.Context.RegisteringInstance += this.ContextOnRegisteringInstance;
      this.Context.Registering += this.ContextOnRegistering;
    }

    /// <summary>
    /// Returns a key for use in the namedTypes dictionary.
    /// </summary>
    /// <param name="type">The registered type.</param>
    /// <param name="name">The name of the registration.</param>
    /// <returns>Formatted string for named type.</returns>
    [NotNull]
    private static string GetNamedTypeKey([NotNull] Type type, [CanBeNull] string name)
    {
      Assert.ArgumentNotNull(type, "type");

      if (type.IsGenericType && !type.IsGenericTypeDefinition)
      {
        type = type.GetGenericTypeDefinition();
      }

      return string.Format(NamedTypeFormatString, type.AssemblyQualifiedName, name);
    }

    /// <summary>
    /// The context on registering.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The register event args.</param>
    private void ContextOnRegistering([CanBeNull]object sender, [NotNull] RegisterEventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");

      this.AddRegistrationToDictionaries(e.TypeFrom ?? e.TypeTo, e.Name);
    }

    /// <summary>
    /// Contexts the on registering instance.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RegisterInstanceEventArgs" /> instance containing the event data.</param>
    private void ContextOnRegisteringInstance([CanBeNull] object sender, [NotNull] RegisterInstanceEventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");

      this.AddRegistrationToDictionaries(e.RegisteredType, e.Name);
    }

    /// <summary>
    /// Adds a registration to the dictionaries.
    /// </summary>
    /// <param name="type">The from type.</param>
    /// <param name="name">The name of the registration.</param>
    private void AddRegistrationToDictionaries([NotNull] Type type, [CanBeNull] string name)
    {
      Assert.ArgumentNotNull(type, "type");

      if (!this.types.ContainsKey(type))
      {
        this.types.TryAdd(type, type);
      }

      if (name == null)
      {
        return;
      }

      var key = GetNamedTypeKey(type, name);
      if (!this.namedTypes.ContainsKey(key))
      {
        this.namedTypes.TryAdd(key, type);
      }
    }
  }
}