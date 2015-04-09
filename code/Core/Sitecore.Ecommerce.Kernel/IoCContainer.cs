// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoCContainer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The IoC Container.
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
    using System.Collections.Generic;
    using System.Web;
    using Diagnostics;
    using Microsoft.Practices.Unity;
    using Pipelines.GetConfiguration;
    using Sitecore.Ecommerce.Unity;
    using Sitecore.Pipelines;

    /// <summary>
    /// The IoC Container.
    /// </summary>
    public class IoCContainer : IIoCContainer
    {
        /// <summary>
        /// Inner container instance.
        /// </summary>
        private readonly IUnityContainer innerContainer;

        /// <summary>
        /// Is set to true when instance is disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCContainer"/> class.
        /// </summary>
        public IoCContainer()
            : this(new UnityContainer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCContainer"/> class.
        /// </summary>
        /// <param name="unityContainer">The unity container.</param>
        public IoCContainer(IUnityContainer unityContainer)
        {
            this.innerContainer = unityContainer;
            this.innerContainer.RegisterExtension<TypeTrackingExtension>();
        }

        /// <summary>
        /// Gets the inner container.
        /// </summary>
        public virtual IUnityContainer InnerContainer
        {
            get { return this.innerContainer; }
        }

        /// <summary>
        /// Gets the parent of this container.
        /// </summary>
        /// <value>
        /// The parent container, or null if this container doesn't have one.
        /// </value>
        public IUnityContainer Parent
        {
            get { return this.innerContainer.Parent; }
        }

        /// <summary>
        /// Gets a sequence of <see cref="T:Microsoft.Practices.Unity.ContainerRegistration"/> that describe the current state
        /// of the container.
        /// </summary>
        public IEnumerable<ContainerRegistration> Registrations
        {
            get { return this.innerContainer.Registrations; }
        }

        /// <summary>
        /// Gets the entity instance.
        /// </summary>
        /// <typeparam name="T">The entity contract.</typeparam>
        /// <returns>The entity instance.</returns>
        public virtual T GetConfiguration<T>() where T : class
        {
            var entityName = string.Format("{0}_{1}", Sitecore.Context.Site.Name, Sitecore.Context.Database.Name);

            try
            {
                var args = new ConfigurationPipelineArgs(typeof(T)) { EntityName = entityName };
                CorePipeline.Run("getConfiguration", args);

                Assert.IsNotNull(args.ConfigurationItem, typeof(T), "Unable to resolve '\"{0}\"' configuration.", typeof(T).Name);

                this.innerContainer.RegisterInstance(entityName, (T)args.ConfigurationItem);

                return (T)args.ConfigurationItem;
            }
            catch (Exception exception)
            {
                Log.Warn(exception.Message, exception);

                lock (this)
                {
                    return UnityContainerExtensions.Resolve<T>(this);
                }
            }
        }

        /// <summary>
        /// Sets the entity instance.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <typeparam name="T">The entity contract.</typeparam>
        public virtual void SetInstance<T>(T entity)
        {
            Assert.ArgumentNotNull(entity, "entity");

            var entityName = this.GenerateEntityName<T>();

            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                this.RegisterInstance(typeof(T), string.Empty, entity, new ContainerControlledLifetimeManager());
                return;
            }

            if (HttpContext.Current.Session[entityName] == null)
            {
                HttpContext.Current.Session[entityName] = entity;
            }
            else
            {
                var storedEntity = (T)HttpContext.Current.Session[entityName];

                if (!storedEntity.Equals(entity))
                {
                    HttpContext.Current.Session[entityName] = entity;
                }
            }
        }

        /// <summary>
        /// Gets the entity instance.
        /// </summary>
        /// <typeparam name="T">The entity contract.</typeparam>
        /// <returns>The entity instance.</returns>
        public virtual T GetInstance<T>()
        {
            var entityName = this.GenerateEntityName<T>();

            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                lock (this)
                {
                    return (T)this.innerContainer.Resolve(typeof(T), null);
                }
            }

            var entityObject = HttpContext.Current.Session[entityName];
            T entity;

            if (entityObject == null)
            {
                lock (this)
                {
                    entity = (T)this.innerContainer.Resolve(typeof(T), null);
                }
            }
            else
            {
                entity = (T)entityObject;
            }

            this.SetInstance(entity);

            return entity;
        }

        /// <summary>
        /// Register a type mapping with the container, where the created instances will use
        /// the given <see cref="T:Microsoft.Practices.Unity.LifetimeManager"/>.
        /// </summary>
        /// <param name="from"><see cref="T:System.Type"/> that will be requested.</param>
        /// <param name="to"><see cref="T:System.Type"/> that will actually be returned.</param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="lifetimeManager">The <see cref="T:Microsoft.Practices.Unity.LifetimeManager"/> that controls the lifetime
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Practices.Unity.UnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).
        /// </returns>
        public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return this.innerContainer.RegisterType(from, to, name, lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="lifetime">
        /// The lifetime.
        /// </param>
        /// <returns>
        /// The unity container instance.
        /// </returns>
        public IUnityContainer RegisterInstance(Type type, string name, object instance, LifetimeManager lifetime)
        {
            return this.innerContainer.RegisterInstance(type, name, instance, lifetime);
        }

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="resolverOverrides">The resolver overrides.</param>
        /// <returns>
        /// The object.
        /// </returns>
        public object Resolve(Type type, string name, params ResolverOverride[] resolverOverrides)
        {
            return this.innerContainer.Resolve(type, name, resolverOverrides);
        }

        /// <summary>
        /// Resolves all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="resolverOverrides">The resolver overrides.</param>
        /// <returns>
        /// The all.
        /// </returns>
        public IEnumerable<object> ResolveAll(Type type, params ResolverOverride[] resolverOverrides)
        {
            return this.innerContainer.ResolveAll(type, resolverOverrides);
        }

        /// <summary>
        /// Builds up.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="existing">The existing.</param>
        /// <param name="name">The name.</param>
        /// <param name="resolverOverrides">The resolver overrides.</param>
        /// <returns>
        /// The up.
        /// </returns>
        public object BuildUp(Type type, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return this.innerContainer.BuildUp(type, existing, name, resolverOverrides);
        }

        /// <summary>
        /// Run an existing object through the container, and clean it up.
        /// </summary>
        /// <param name="o">The object to tear down.</param>
        public void Teardown(object o)
        {
            this.innerContainer.Teardown(o);
        }

        /// <summary>
        /// Add an extension object to the container.
        /// </summary>
        /// <param name="extension"><see cref="T:Microsoft.Practices.Unity.UnityContainerExtension"/> to add.</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Practices.Unity.UnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).
        /// </returns>
        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            return this.innerContainer.AddExtension(extension);
        }

        /// <summary>
        /// Resolve access to a configuration interface exposed by an extension.
        /// </summary>
        /// <param name="configurationInterface"><see cref="T:System.Type"/> of configuration interface required.</param>
        /// <returns>
        /// The requested extension's configuration interface, or null if not found.
        /// </returns>
        public object Configure(Type configurationInterface)
        {
            return this.innerContainer.Configure(configurationInterface);
        }

        /// <summary>
        /// Remove all installed extensions from this container.
        /// </summary>
        /// <returns>
        /// The <see cref="T:Microsoft.Practices.Unity.UnityContainer"/> object that this method was called on (this in C#, Me in Visual Basic).
        /// </returns>
        public IUnityContainer RemoveAllExtensions()
        {
            return this.innerContainer.RemoveAllExtensions();
        }

        /// <summary>
        /// Create a child container.
        /// </summary>
        /// <returns>
        /// The new child container.
        /// </returns>
        public IUnityContainer CreateChildContainer()
        {
            if (this.innerContainer == null)
            {
                return null;
            }

            var child = this.innerContainer.CreateChildContainer();
            child.RegisterExtension<TypeTrackingExtension>();

            return child;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (null != this.innerContainer)
                {
                    this.innerContainer.Dispose();
                }
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// Gets the entity name for the site based entity.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The entity name.</returns>
        [AllowNull("*")]
        protected string GetSiteBasedName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var contextSite = Sitecore.Context.Site;
            if (contextSite == null || string.IsNullOrEmpty(contextSite.Name))
            {
                return name;
            }

            var siteName = contextSite.Name;

            return name.StartsWith(siteName) ? name : string.Concat(siteName, "_", name);
        }

        /// <summary>
        /// Gets the name of entity in the session.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>The name of entity in the session.</returns>
        protected virtual string GenerateEntityName<T>()
        {
            return typeof(T).Name;
        }
    }
}