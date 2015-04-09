// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ProductRepository type. The repository is based on Sitecore items
//   and can perform CRUD operations for Products, Prices, Stocks and Categories.
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

namespace Sitecore.Ecommerce.Products
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Data;
  using Data.Mapping;
  using Diagnostics;
  using DomainModel.Configurations;
  using DomainModel.Data;
  using DomainModel.Products;
  using Microsoft.Practices.Unity;
  using Search;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Exceptions;
  using Utils;

  /// <summary>
  /// Defines the ProductRepository type. The repository is based on Sitecore items
  /// and can perform CRUD operations for Products, Prices, Stocks and Categories.
  /// </summary>
  public class ProductRepository : IProductRepository
  {
    /// <summary>
    /// The business catalog settings.
    /// </summary>
    private BusinessCatalogSettings businessCatalogSettings;

    /// <summary>
    /// The product repository database.
    /// </summary>
    private Database database;

    /// <summary>
    /// The search provider.
    /// </summary>
    private ISearchProvider searchProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    public ProductRepository()
      : this(new SitecoreQuerySearchProvider())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository" /> class.
    /// </summary>
    /// <param name="searchProvider">The search provider.</param>
    public ProductRepository(ISearchProvider searchProvider)
      : this(searchProvider, new ItemProductFactory(), new ItemToEntityMapper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository" /> class.
    /// </summary>
    /// <param name="searchProvider">The search provider.</param>
    /// <param name="productFactory">The product factory.</param>
    /// <param name="itemToEntityMapper">The item to entity mapper.</param>
    public ProductRepository([NotNull] ISearchProvider searchProvider, ProductFactory productFactory, EntityMapper<Item, IEntity> itemToEntityMapper)
    {
      Assert.ArgumentNotNull(searchProvider, "searchProvider");
      Assert.ArgumentNotNull(productFactory, "productFactory");
      Assert.ArgumentNotNull(itemToEntityMapper, "itemToEntityMapper");

      this.searchProvider = searchProvider;
      this.ProductFactory = productFactory;
      this.ItemToEntityMapper = itemToEntityMapper;
    }

    /// <summary>
    /// Gets or sets the search provider.
    /// </summary>
    /// <value>The search provider.</value>
    [NotNull]
    public virtual ISearchProvider SearchProvider
    {
      get { return this.searchProvider; }
      set { this.searchProvider = value; }
    }

    /// <summary>
    /// Gets or sets the product factory.
    /// </summary>
    /// <value>
    /// The product factory.
    /// </value>
    [NotNull]
    public ProductFactory ProductFactory { get; set; }

    /// <summary>
    /// Gets or sets the data itemToEntityMapper.
    /// </summary>
    /// <value>
    /// The data itemToEntityMapper.
    /// </value>
    [NotNull]
    [Dependency]
    public virtual IDataMapper DataMapper { get; set; }

    /// <summary>
    /// Gets or sets the entity helper.
    /// </summary>
    /// <value>The entity helper.</value>
    [NotNull]
    [Dependency]
    public EntityHelper EntityHelper { get; set; }

    /// <summary>
    /// Gets or sets the item to entity mapper.
    /// </summary>
    /// <value>The item to entity mapper.</value>
    public EntityMapper<Item, IEntity> ItemToEntityMapper { get; set; }

    /// <summary>
    /// Gets or sets the shop context.
    /// </summary>
    /// <value>
    /// The shop context.
    /// </value>
    [CanBeNull]
    public ShopContext ShopContext { get; set; }

    /// <summary>
    /// Gets the business catalog settings.
    /// </summary>
    /// <value>The business catalog settings.</value>
    [NotNull]
    protected virtual BusinessCatalogSettings BusinessCatalogSettings
    {
      get
      {
        if (this.businessCatalogSettings == null)
        {
          Assert.IsNotNull(this.ShopContext, "Unable to resolve business settings. Context shop cannot be null.");

          this.businessCatalogSettings = this.ShopContext.BusinessCatalogSettings;
          Assert.IsNotNull(this.businessCatalogSettings, this.GetType(), "Business Catalog settings not found.");
        }

        return this.businessCatalogSettings;
      }
    }

    /// <summary>
    /// Gets or sets the database.
    /// </summary>
    /// <value>
    /// The database.
    /// </value>
    [NotNull]
    public virtual Database Database
    {
      get
      {
        if (this.database == null)
        {
          Assert.IsNotNull(this.ShopContext, "Unable to resolve database. Context shop cannot be null.");

          this.database = this.ShopContext.Database;
          Assert.IsNotNull(this.database, "Unable to resolve database.");
        }

        return this.database;
      }

      set
      {
        this.database = value;
      }
    }

    #region Implementation of IProductRepository

    /// <summary>
    /// Creates the specified repository item.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItem">The repository item.</param>
    /// <exception cref="ArgumentException">Product repository item title is invalid.</exception>
    public virtual void Create<T>(T repositoryItem) where T : IProductRepositoryItem
    {
      Assert.ArgumentNotNull(repositoryItem, "repositoryItem");
      Assert.IsNotNullOrEmpty(repositoryItem.Name, "Product repository item name is null or empty.");
      Assert.IsNotNullOrEmpty(repositoryItem.Code, "Product repository item code is null or empty.");

      Item item = this.Database.GetItem(this.GetRepositoryRootPath());

      this.Insert(item, repositoryItem);
    }

    /// <summary>
    /// Creates the product.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <typeparam name="TParent">The type of the parent.</typeparam>
    /// <param name="parentRepositoryItemCode">The parent repository item code.</param>
    /// <param name="repositoryItem">The repository item.</param>
    /// <exception cref="ArgumentException">Product repository item title is invalid.</exception>
    public virtual void Create<T, TParent>(string parentRepositoryItemCode, T repositoryItem)
      where T : IProductRepositoryItem
      where TParent : IProductRepositoryItem
    {
      Assert.ArgumentNotNull(repositoryItem, "repositoryItem");
      Assert.ArgumentNotNullOrEmpty(parentRepositoryItemCode, "parentRepositoryItemCode");
      Assert.IsNotNullOrEmpty(repositoryItem.Name, "Product repository item name is null or empty.");
      Assert.IsNotNullOrEmpty(repositoryItem.Code, "Product repository item code is null or empty.");

      Item parentItem = this.GetItem<TParent>(parentRepositoryItemCode);

      this.Insert(parentItem, repositoryItem);
    }

    /// <summary>
    /// Gets the product.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItemCode">The repository item code.</param>
    /// <returns>The result product.</returns>
    public virtual T Get<T>(string repositoryItemCode) where T : IProductRepositoryItem
    {
      Assert.ArgumentNotNullOrEmpty(repositoryItemCode, "repositoryItemCode");

      Item item = this.GetItem<T>(repositoryItemCode);

      if (item == null)
      {
        return default(T);
      }

      return this.GetProduct<T>(item);
    }

    /// <summary>
    /// Gets the products.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="searchQuery">The search query.</param>
    /// <returns>The result product list.</returns>
    public virtual IEnumerable<T> Get<T, TQuery>(TQuery searchQuery) where T : IProductRepositoryItem
    {
      return this.Get<T, TQuery>(searchQuery, 0, int.MaxValue);
    }

    /// <summary>
    /// Gets the specified search query.
    /// </summary>
    /// <typeparam name="T">The type of product repository item.</typeparam>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="searchQuery">The search query.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns>The result.</returns>
    public virtual IEnumerable<T> Get<T, TQuery>(TQuery searchQuery, int startIndex, int pageSize) where T : IProductRepositoryItem
    {
      Assert.ArgumentNotNull(searchQuery, "searchQuery");
      Assert.IsTrue(searchQuery is Query, "Query type is invalid");

      Query query = searchQuery as Query;

      IEnumerable<Item> items = this.Get(query, typeof(T));
      if (items == null)
      {
        yield break;
      }

      foreach (Item item in items.Skip(startIndex * pageSize).Take(pageSize))
      {
        if (item != null)
        {
          if (this.searchProvider is LuceneSearchProvider || ProductRepositoryUtil.IsBasedOnTemplate(item.Template, this.GetRepositoryItemTemplateId(typeof(T))))
          {
            T productItem = this.GetProduct<T>(item);
            if (productItem != null)
            {
              yield return productItem;
            }
          }
        }
      }
    }

    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <typeparam name="T">The type of product repository item.</typeparam>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <returns>Returns selected instances count.</returns>
    public virtual int GetCount<T, TQuery>(TQuery query) where T : IProductRepositoryItem
    {
      return this.Get(query as Query, typeof(T)).Count();
    }

    /// <summary>
    /// Gets the specified parent repository item code.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <typeparam name="TParent">The type of the parent.</typeparam>
    /// <param name="parentRepositoryItemCode">The parent repository item code.</param>
    /// <param name="repositoryItemCode">The repository item code.</param>
    /// <returns>The result product repository item.</returns>
    public virtual T Get<T, TParent>(string parentRepositoryItemCode, string repositoryItemCode)
      where T : IProductRepositoryItem
      where TParent : IProductRepositoryItem
    {
      Assert.ArgumentNotNullOrEmpty(parentRepositoryItemCode, "parentRepositoryItemCode");
      Assert.ArgumentNotNullOrEmpty(repositoryItemCode, "repositoryItemCode");

      Item parentItem = this.GetItem<TParent>(parentRepositoryItemCode);
      Assert.IsNotNull(parentItem, "Parent item is null");

      return this.Get<T>(repositoryItemCode);
    }

    /// <summary>
    /// Gets the category products.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <typeparam name="TParent">The type of the parent.</typeparam>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="parentRepositoryItemCode">The parent repository item code.</param>
    /// <param name="searchQuery">The search query.</param>
    /// <returns>The category products.</returns>
    public virtual IEnumerable<T> Get<T, TParent, TQuery>(string parentRepositoryItemCode, TQuery searchQuery)
      where T : IProductRepositoryItem
      where TParent : IProductRepositoryItem
    {
      Assert.ArgumentNotNullOrEmpty(parentRepositoryItemCode, "parentRepositoryItemCode");
      Assert.ArgumentNotNull(searchQuery, "searchQuery");
      Assert.IsTrue(searchQuery is Query, "Query type is invalid");

      Query query = searchQuery as Query;

      Item parentItem = this.GetItem<TParent>(parentRepositoryItemCode);
      Assert.IsNotNull(parentItem, "Parent item is null");

      query.SearchRoot = parentItem.ID.ToString();

      return this.Get<T, TQuery>(searchQuery);
    }

    /// <summary>
    /// Gets the category.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent.</typeparam>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItemCode">The repository item code.</param>
    /// <returns>The category.</returns>
    public virtual TParent GetParent<TParent, T>(string repositoryItemCode)
      where T : IProductRepositoryItem
      where TParent : IProductRepositoryItem
    {
      Assert.ArgumentNotNullOrEmpty(repositoryItemCode, "parentRepositoryItemCode");

      Item item = this.GetItem<T>(repositoryItemCode);
      Assert.IsNotNull(item, "Repository item was not found");

      return item.Parent == null ? default(TParent) : this.DataMapper.GetEntity<TParent>(item.Parent);
    }

    /// <summary>
    /// Gets the sub items.
    /// </summary>
    /// <typeparam name="TChild">The type of the child.</typeparam>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItemCode">The repository item code.</param>
    /// <param name="deep">if set to <c>true</c> [deep].</param>
    /// <returns>The sub items.</returns>
    public virtual IEnumerable<TChild> GetSubItems<TChild, T>(string repositoryItemCode, bool deep)
      where T : IProductRepositoryItem
      where TChild : IProductRepositoryItem
    {
      Assert.ArgumentNotNullOrEmpty(repositoryItemCode, "repositoryItemCode");

      Item parentItem = this.GetItem<T>(repositoryItemCode);
      Assert.IsNotNull(parentItem, "Parent item is null");

      IEnumerable<Item> items = deep ? parentItem.Axes.GetDescendants() : parentItem.Children.ToArray();

      if (items == null)
      {
        yield break;
      }

      foreach (Item item in items)
      {
        if (this.searchProvider is LuceneSearchProvider || ProductRepositoryUtil.IsBasedOnTemplate(item.Template, this.GetRepositoryItemTemplateId(typeof(TChild))))
        {
          yield return this.DataMapper.GetEntity<TChild>(item);
        }
      }
    }

    /// <summary>
    /// Moves the item.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <typeparam name="TCatalog">The type of the catalog.</typeparam>
    /// <param name="repositoryItemCode">The repository item code.</param>
    /// <param name="newParentRepositoryItemCode">The new parent repository item code.</param>
    public virtual void Move<T, TCatalog>(string repositoryItemCode, string newParentRepositoryItemCode)
      where T : IProductRepositoryItem
      where TCatalog : IProductRepositoryItem
    {
      Assert.ArgumentNotNullOrEmpty(repositoryItemCode, "repositoryItemCode");
      Assert.ArgumentNotNullOrEmpty(newParentRepositoryItemCode, "newParentRepositoryItemCode");

      Item item = this.GetItem<T>(repositoryItemCode);
      Assert.IsNotNull(item, "Product repository item is null");

      Item newParentItem = this.GetItem<TCatalog>(newParentRepositoryItemCode);
      Assert.IsNotNull(newParentItem, "New parent item is null");

      item.MoveTo(newParentItem);
    }

    /// <summary>
    /// Updates the item.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItem">The repository item.</param>
    public virtual void Update<T>(T repositoryItem) where T : IProductRepositoryItem
    {
      Assert.ArgumentNotNull(repositoryItem, "Product");
      Assert.IsNotNullOrEmpty(repositoryItem.Code, "Product repository item code is null or empty.");

      Item item = this.GetItem<T>(repositoryItem.Code);

      if (item == null)
      {
        Log.Error("Product repository item was not found", this);
        return;
      }

      this.DataMapper.SaveEntity(repositoryItem, item);
    }

    /// <summary>
    /// Deletes the item.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItemCode">The repository item code.</param>
    public virtual void Delete<T>(string repositoryItemCode) where T : IProductRepositoryItem
    {
      Assert.ArgumentNotNullOrEmpty(repositoryItemCode, "productCode");

      Item item = this.GetItem<T>(repositoryItemCode);

      if (item == null)
      {
        Log.Error("Product repository item was not found", this);
        return;
      }

      item.Delete();
    }

    #endregion

    /// <summary>
    /// Gets the specified code.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="location">The location.</param>
    /// <returns>The needed item.</returns>
    protected virtual Item GetItem<T>(string location) where T : IProductRepositoryItem
    {
      Assert.ArgumentNotNullOrEmpty(location, "code");

      /*
      This functionality has tree possible scenario: 
      if code is Item Id it tries to find the product item by id (fastest way), by path.
      else it tries to find the product item by code and unit of measures. 
      */
      Item item;

      if (ID.IsID(location))
      {
        item = this.Database.GetItem(location);
        if (item != null)
        {
          return item;
        }
      }

      Item root = this.Database.GetItem(this.GetRepositoryRootPath());
      Assert.IsNotNull(root, "Repository root item is null");

      string path = IO.FileUtil.MakePath(root.Paths.FullPath, location);

      item = this.Database.GetItem(path);
      if (item != null)
      {
        return item;
      }

      string key = this.EntityHelper.GetField<T>(i => i.Code);
      if (string.IsNullOrEmpty(key))
      {
        Log.Warn(string.Concat("Field name is undefined. Type: ", typeof(T).ToString(), ". Property: 'Code'."), this);
        key = "Code";
      }

      Query query = new Query();
      query.Add(new FieldQuery(key, location, MatchVariant.Exactly));

      return this.Get(query, typeof(T)).FirstOrDefault(o => o != null);
    }

    /// <summary>
    /// Gets the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="type">The type of the repository item.</param>
    /// <returns>The items collection.</returns>
    protected virtual IEnumerable<Item> Get(Query query, Type type)
    {
      Assert.ArgumentNotNull(query, "Query");

      if (string.IsNullOrEmpty(query.SearchRoot))
      {
        query.SearchRoot = this.GetRepositoryRootPath();
      }

      return this.searchProvider.Search(query, this.Database);
    }

    /// <summary>
    /// Inserts the specified parent item.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="parentItem">The parent item.</param>
    /// <param name="item">The needed item.</param>
    /// <exception cref="ArgumentException">Product repository item title is invalid.</exception>
    protected virtual void Insert<T>(Item parentItem, T item) where T : IProductRepositoryItem
    {
      Assert.ArgumentNotNull(parentItem, "parentItem");
      Assert.ArgumentNotNull(item, "item");

      this.CheckDuplicatedCode(item.Code);

      Item newItem;

      ID templateId = null;
      ITemplatedEntity templatedEntity = item as ITemplatedEntity;
      if (templatedEntity != null && !string.IsNullOrEmpty(templatedEntity.Template))
      {
        Item templateItem = this.Database.GetItem(templatedEntity.Template);
        templateId = templateItem.ID;
      }

      try
      {
        if (ID.IsNullOrEmpty(templateId))
        {
          templateId = this.GetRepositoryItemTemplateId(typeof(T));
        }

        newItem = parentItem.Add(item.Name, new TemplateID(templateId));
      }
      catch (InvalidItemNameException exception)
      {
        Log.Error("Product title is invalid.", exception, this);
        throw new ArgumentException("Product repository item title is invalid.", exception);
      }
      catch (Exception exception)
      {
        Log.Error("Failed to create product item", exception, this);
        throw;
      }

      Assert.IsNotNull(newItem, "Failed to create product item.");

      Assert.IsNotNull(this.DataMapper, "DataMapper cannot be null.");
      this.DataMapper.SaveEntity(item, newItem);

      ProductBaseData product = item as ProductBaseData;
      if (product == null)
      {
        return;
      }

      using (new EditContext(newItem))
      {
        foreach (KeyValuePair<string, object> specification in product.Specifications)
        {
          newItem[specification.Key] = specification.Value.ToString();
        }
      }
    }

    /// <summary>
    /// Gets the root.
    /// </summary>
    /// <returns>The repository root.</returns>
    protected virtual string GetRepositoryRootPath()
    {
      Item root = this.GetRepositoryRoot();

      return root.ID.ToString();
    }

    /// <summary>
    /// Gets the product template id.
    /// </summary>
    /// <param name="type">The type of the repository item.</param>
    /// <returns>The product template id.</returns>
    /// <exception cref="ArgumentException">Product repository item template Id was not set</exception>
    protected virtual ID GetRepositoryItemTemplateId(Type type)
    {
      string template = this.EntityHelper.GetTemplate(type);

      if (string.IsNullOrEmpty(template) || !ID.IsID(template))
      {
        throw new ArgumentException("Product repository item template Id was not set");
      }

      return new ID(template);
    }

    /// <summary>
    /// Checks whether a product with such code already exists.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <exception cref="InvalidOperationException">Duplicated product code.</exception>
    protected virtual void CheckDuplicatedCode([NotNull] string code)
    {
      Assert.ArgumentNotNull(code, "code");

      Item root = this.GetRepositoryRoot();

      Item[] duplicatedProducts = root.Axes.SelectItems(string.Format(".//*[@Product Code = '{0}']", code));
      if (duplicatedProducts != null)
      {
        throw new InvalidOperationException("Duplicated product code.");
      }
    }

    /// <summary>
    /// Gets the product repository root item.
    /// </summary>
    /// <returns>The repository root.</returns>
    [NotNull]
    protected virtual Item GetRepositoryRoot()
    {
      string productLink = this.BusinessCatalogSettings.ProductsLink;
      Assert.IsNotNullOrEmpty(productLink, "Product link cannot be null or empty.");

      Item productsRoot = this.Database.GetItem(productLink);
      Assert.IsNotNull(productsRoot, "Product repository root item is null");

      return productsRoot;
    }

    /// <summary>
    /// Gets the product.
    /// </summary>
    /// <typeparam name="T">The type of product repository item.</typeparam>
    /// <param name="item">The item.</param>
    /// <returns>
    /// The product.
    /// </returns>
    [CanBeNull]
    protected virtual T GetProduct<T>([NotNull] Item item)
    {
      Debug.ArgumentNotNull(item, "item");

      T repositoryItem;

      using (new SiteIndependentDatabaseSwitcher(item.Database))
      {
        if (typeof(T) == typeof(ProductBaseData) || typeof(T).IsSubclassOf(typeof(ProductBaseData)))
        {
          ID productBaseTemplateId = new ID(Configuration.Settings.GetSetting("Ecommerce.Product.BaseTemplateId"));
          bool basedOnProductTemplate = ProductRepositoryUtil.IsBasedOnTemplate(item.Template, productBaseTemplateId);
          if (!basedOnProductTemplate)
          {
            return default(T);
          }

          Assert.IsNotNull(this.ProductFactory, "Unable to get the product. ProductFactory cannot be null.");
          repositoryItem = (T)((object)this.ProductFactory.Create(item.TemplateID.ToString()));
        }
        else
        {
          repositoryItem = Context.Entity.Resolve<T>();
        }

        if (repositoryItem is IEntity)
        {
          this.ItemToEntityMapper.Map(item, (IEntity)repositoryItem);
        }
      }

      return repositoryItem;
    }
  }
}