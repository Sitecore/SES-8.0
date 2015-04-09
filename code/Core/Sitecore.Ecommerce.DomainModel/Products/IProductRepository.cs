// -------------------------------------------------------------------------------------------
// <copyright file="IProductRepository.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.DomainModel.Products
{
  using System.Collections.Generic;

  /// <summary>
  /// The product provider interface.
  /// </summary>
  public interface IProductRepository
  {
    /// <summary>
    /// Creates the specified repository item under the root of the product repository.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItem">The repository item.</param>
    void Create<T>(T repositoryItem) where T : IProductRepositoryItem;

    /// <summary>
    /// Creates the product.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <typeparam name="TParent">The type of the parent product repository item..</typeparam>
    /// <param name="parentRepositoryItemCode">The parent repository item code.</param>
    /// <param name="repositoryItem">The repository item.</param>
    void Create<T, TParent>(string parentRepositoryItemCode, T repositoryItem)
      where T : IProductRepositoryItem
      where TParent : IProductRepositoryItem;

    /// <summary>
    /// Gets the product.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItemCode">The repository item code or path.</param>
    /// <returns>The result product repository item.</returns>
    T Get<T>(string repositoryItemCode) where T : IProductRepositoryItem;

    /// <summary>
    /// Gets the products.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="query">The query.</param>
    /// <returns>The result product repository items.</returns>
    IEnumerable<T> Get<T, TQuery>(TQuery query) where T : IProductRepositoryItem;

    /// <summary>
    /// Gets the specified search query.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="searchQuery">The search query.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns></returns>
    IEnumerable<T> Get<T, TQuery>(TQuery searchQuery, int startIndex, int pageSize) where T : IProductRepositoryItem;

    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="searchQuery">The search query.</param>
    /// <returns>Returns selected instances count</returns>
    int GetCount<T, TQuery>(TQuery searchQuery) where T : IProductRepositoryItem;

    /// <summary>
    /// Gets the specified parent repository item code.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <typeparam name="TParent">The type of the parent.</typeparam>
    /// <param name="parentRepositoryItemCode">The parent repository item code or path.</param>
    /// <param name="repositoryItemCode">The repository item code or path.</param>
    /// <returns>The result product repository item.</returns>
    T Get<T, TParent>(string parentRepositoryItemCode, string repositoryItemCode)
      where T : IProductRepositoryItem
      where TParent : IProductRepositoryItem;

    /// <summary>
    /// Gets the category products.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <typeparam name="TParent">The type of the parent product repository item.</typeparam>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <param name="parentRepositoryItemCode">The parent repository item code.</param>
    /// <param name="query">The query.</param>
    /// <returns>The category products.</returns>
    IEnumerable<T> Get<T, TParent, TQuery>(string parentRepositoryItemCode, TQuery query)
      where T : IProductRepositoryItem
      where TParent : IProductRepositoryItem;

    /// <summary>
    /// Gets the parent.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent product repository item.</typeparam>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItemCode">The repository item code or path.</param>
    /// <returns>The category.</returns>
    TParent GetParent<TParent, T>(string repositoryItemCode)
      where T : IProductRepositoryItem
      where TParent : IProductRepositoryItem;

    /// <summary>
    /// Gets the sub items.
    /// </summary>
    /// <typeparam name="TChild">The type of the child product repository item.</typeparam>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItemCode">The repository item code or path.</param>
    /// <param name="deep">if set to <c>true</c> [deep].</param>
    /// <returns>The sub items.</returns>
    IEnumerable<TChild> GetSubItems<TChild, T>(string repositoryItemCode, bool deep)
      where T : IProductRepositoryItem
      where TChild : IProductRepositoryItem;

    /// <summary>
    /// Moves the item.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <typeparam name="TCatalog">The type of the catalog.</typeparam>
    /// <param name="repositoryItemCode">The repository item code or path.</param>
    /// <param name="newParentRepositoryItemCode">The new parent repository item code or path.</param>
    void Move<T, TCatalog>(string repositoryItemCode, string newParentRepositoryItemCode)
      where T : IProductRepositoryItem
      where TCatalog : IProductRepositoryItem;

    /// <summary>
    /// Updates the item.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItem">The repository item.</param>
    void Update<T>(T repositoryItem) where T : IProductRepositoryItem;

    /// <summary>
    /// Deletes the item.
    /// </summary>
    /// <typeparam name="T">The type of the product repository item.</typeparam>
    /// <param name="repositoryItemCode">The repository item code or path.</param>
    void Delete<T>(string repositoryItemCode) where T : IProductRepositoryItem;
  }
}