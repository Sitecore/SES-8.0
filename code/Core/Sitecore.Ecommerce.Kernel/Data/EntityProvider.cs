// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The entity provider class.
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

namespace Sitecore.Ecommerce.Data
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Configuration.Provider;
  using System.Linq;
  using Diagnostics;
  using DomainModel.Configurations;
  using DomainModel.Data;
  using Globalization;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;

  /// <summary>
  /// The entity provider class. 
  /// </summary>
  /// <typeparam name="T">The container interface.</typeparam>
  public class EntityProvider<T> : ProviderBase, IEntityProvider<T> where T : class
  {
    /// <summary>
    /// The configuration provider.
    /// </summary>
    private readonly IDataMapper dataMapper;

    /// <summary>
    /// The containers root item id.
    /// </summary>
    private string settingName;

    /// <summary>
    /// The default container name.
    /// </summary>
    private string defaultContainerName;

    /// <summary>
    /// The container item template Id.
    /// </summary>
    private string containersItemTemplateId;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityProvider&lt;T&gt;"/> class.
    /// </summary>
    public EntityProvider()
    {
      this.dataMapper = Context.Entity.Resolve<IDataMapper>();
    }

    /// <summary>
    /// Gets the shop context.
    /// </summary>
    /// <value>
    /// The shop context.
    /// </value>
    [CanBeNull]
    public ShopContext ShopContext
    {
      get { return Context.Entity.Resolve<ShopContext>(); }
    }

    /// <summary>
    /// Gets the containers root item.
    /// </summary>
    /// <value>The containers root item.</value>
    [CanBeNull]
    protected virtual Item ContainersRootItem
    {
      get
      {
        EntityHelper entityHelper = Context.Entity.Resolve<EntityHelper>();

        string rootItemPath = entityHelper.GetPropertyValueByField<string, BusinessCatalogSettings>(this.BusinessCatalogSettings, this.settingName);

        if (string.IsNullOrEmpty(rootItemPath) || !ID.IsID(rootItemPath))
        {
          Log.Warn("Site container root path is invalid", this);
          return default(Item);
        }

        Assert.IsNotNull(this.Database, "Cannot get current database");

        Item rootItem = this.Database.GetItem(rootItemPath);

        if (rootItem == null)
        {
          Log.Warn("Site container root item is null", this);
          return default(Item);
        }

        return rootItem;
      }
    }

    /// <summary>
    /// Gets the business catalog settings.
    /// </summary>
    /// <value>The business catalog settings.</value>
    [NotNull]
    private BusinessCatalogSettings BusinessCatalogSettings
    {
      get
      {
        Assert.IsNotNull(this.ShopContext, "Unable to resolve business settings. Context shop cannot be null.");
        Assert.IsNotNull(this.ShopContext.BusinessCatalogSettings, "Business Catalog settings not found.");
        return this.ShopContext.BusinessCatalogSettings;
      }
    }

    /// <summary>
    /// Gets the business catalog settings.
    /// </summary>
    /// <value>The business catalog settings.</value>
    [NotNull]
    private GeneralSettings GeneralSettings
    {
      get
      {
        Assert.IsNotNull(this.ShopContext, "Unable to resolve general settings. Context shop cannot be null.");
        Assert.IsNotNull(this.ShopContext.GeneralSettings, "General settings not found.");
        return this.ShopContext.GeneralSettings;
      }
    }

    /// <summary>
    /// Gets the product repository database.
    /// </summary>
    /// <value>The database.</value>
    [NotNull]
    private Database Database
    {
      get
      {
        Assert.IsNotNull(this.ShopContext, "Unable to resolve database. Context shop cannot be null.");
        Assert.IsNotNull(this.ShopContext.Database, "Unable to resolve database.");
        return this.ShopContext.Database;
      }
    }

    /// <summary>
    /// Initializes the provider.
    /// </summary>
    /// <param name="name">The friendly name of the provider.</param>
    /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// The name of the provider is null.
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    /// The name of the provider has a length of zero.
    /// </exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"/> on a provider after the provider has already been initialized.
    /// </exception>
    public override void Initialize(string name, NameValueCollection config)
    {
      Assert.ArgumentNotNullOrEmpty(name, "name");
      Assert.ArgumentNotNull(config, "config");

      base.Initialize(name, config);

      this.settingName = config["setting name"];
      config.Remove("setting name");

      this.defaultContainerName = config["default container name"];
      config.Remove("default container name");

      this.containersItemTemplateId = config["containers item template Id"];
      config.Remove("containers item template Id");
    }

    /// <summary>
    /// Gets the default value.
    /// </summary>
    /// <returns>
    /// The default value of container.
    /// </returns>
    [CanBeNull]
    public virtual T GetDefault()
    {
      EntityHelper entityHelper = Context.Entity.Resolve<EntityHelper>();
      IDictionary<string, object> fieldsCollection = entityHelper.GetPropertiesValues(this.GeneralSettings);

      string defaultItemId = string.Empty;

      if (fieldsCollection.ContainsKey(this.defaultContainerName))
      {
        defaultItemId = fieldsCollection[this.defaultContainerName] as string;
      }

      if (!string.IsNullOrEmpty(defaultItemId) && ID.IsID(defaultItemId))
      {
        Item item = ItemManager.GetItem(defaultItemId, Language.Current, Version.Latest, Sitecore.Context.Database);
        if (item != null)
        {
          return this.dataMapper.GetEntity<T>(item);
        }
      }

      if (Sitecore.Context.Item == null)
      {
        return this.GetAll().FirstOrDefault();
      }

      string defaultContainer = Sitecore.Context.Item[this.defaultContainerName];
      if (string.IsNullOrEmpty(defaultContainer))
      {
        return this.GetAll().FirstOrDefault();
      }

      Assert.IsNotNull(this.ContainersRootItem, "Unable to get default container value. ContainerRootItem cannot be null.");

      Item defaultContainerItem = this.ContainersRootItem.Children[new ID(defaultContainer)];

      return defaultContainerItem != null
               ? this.dataMapper.GetEntity<T>(defaultContainerItem)
               : this.GetAll().FirstOrDefault();
    }

    /// <summary>
    /// Gets all containers.
    /// </summary>
    /// <returns>
    /// The site containers collection
    /// </returns>
    [NotNull]
    public virtual IEnumerable<T> GetAll()
    {
      if (this.ContainersRootItem == null)
      {
        yield break;
      }

      foreach (Item item in this.ContainersRootItem.Children.Where(i => i.TemplateID.ToString().Equals(this.containersItemTemplateId)))
      {
        T container = this.dataMapper.GetEntity<T>(item);
        yield return container;
      }
    }

    /// <summary>
    /// Gets the container by code.
    /// </summary>
    /// <param name="code">The container code.</param>
    /// <returns>
    /// The container by code.
    /// </returns>
    [CanBeNull]
    public virtual T Get([NotNull] string code)
    {
      Assert.ArgumentNotNullOrEmpty(code, "code");

      Item item = null;

      if (ID.IsID(code) && this.ContainersRootItem != null)
      {
        item = this.ContainersRootItem.Database.GetItem(code);
      }
      else if (this.ContainersRootItem != null)
      {
        item = this.ContainersRootItem.Axes.SelectSingleItem(string.Format("./*[@Code='{0}']", code));
      }

      if (item == null)
      {
        return default(T);
      }

      T container = this.dataMapper.GetEntity<T>(item);

      return container;
    }
  }
}