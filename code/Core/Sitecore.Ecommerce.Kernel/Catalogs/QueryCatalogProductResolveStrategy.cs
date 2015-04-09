// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryCatalogProductResolveStrategy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The query catalog product resolve strategy.
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

namespace Sitecore.Ecommerce.Catalogs
{
  using System.Collections.Generic;
  using System.Web;
  using Diagnostics;
  using DomainModel.Configurations;
  using Search;
  using Sitecore.Data.Items;
  using Sitecore.Exceptions;
  using Text;

  /// <summary>
  /// The query catalog product resolve strategy.
  /// </summary>
  public class QueryCatalogProductResolveStrategy : CatalogProductResolveStrategyBase
  {
    /// <summary>
    /// The Search Text Boxes field name.
    /// </summary>
    private readonly string searchTextBoxesFieldName;

    /// <summary>
    /// The Search Checklists field name.
    /// </summary>
    private readonly string searchChecklistsFieldName;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryCatalogProductResolveStrategy"/> class.
    /// </summary>
    /// <param name="searchTextBoxesFieldName">Name of the search text boxes field.</param>
    /// <param name="searchChecklistsFieldName">Name of the search checklists field.</param>
    public QueryCatalogProductResolveStrategy(string searchTextBoxesFieldName, string searchChecklistsFieldName)
    {
      Assert.ArgumentNotNullOrEmpty(searchTextBoxesFieldName, "searchTextBoxesFieldName");
      Assert.ArgumentNotNullOrEmpty(searchChecklistsFieldName, "searchChecklistsFieldName");

      this.searchChecklistsFieldName = searchChecklistsFieldName;
      this.searchTextBoxesFieldName = searchTextBoxesFieldName;
    }

    /// <summary>
    /// Gets or sets the search provider.
    /// </summary>
    /// <value>The search provider.</value>
    [NotNull]
    public ISearchProvider SearchProvider { get; set; }

    #region Implementation of ICatalogProductResolveStrategy

    /// <summary>
    /// Gets products collection from the product catalog.
    /// </summary>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <typeparam name="T">The catalog item type.</typeparam>
    /// <param name="catalogItem">The catalog item.</param>
    /// <returns>The products collection.</returns>
    /// <exception cref="ItemNullException">Catalog item is null.</exception>
    public override IEnumerable<TProduct> GetCatalogProducts<TProduct, T>(T catalogItem)
    {
      Assert.ArgumentNotNull(catalogItem, "catalogItem");

      Item catalog = catalogItem as Item;
      if (catalog == null)
      {
        throw new ItemNullException("Catalog item is null.");
      }

      Query query = this.BuildSearchQuery(catalog);

      Assert.IsNotNull(this.ProductRepository, "Unable to get the catalog products. ProductRepository cannot be null.");
      IEnumerable<TProduct> products = this.ProductRepository.Get<TProduct, Query>(query);

      return products;
    }

    #endregion

    /// <summary>
    /// Gets the catalog product items.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <returns>The catalog product items.</returns>
    public override IEnumerable<Item> GetCatalogProductItems(Item catalogItem)
    {
      Query query = this.BuildSearchQuery(catalogItem);

      Assert.IsNotNull(this.SearchProvider, "Unable to get catalog product items. Search provider cannot be null.");

      return this.SearchProvider.Search(query, catalogItem.Database);
    }

    /// <summary>
    /// Builds the search query.
    /// </summary>
    /// <param name="catalogItem">The catalog item.</param>
    /// <returns>
    /// The search query
    /// </returns>
    [NotNull]
    protected virtual Query BuildSearchQuery([NotNull] Item catalogItem)
    {
      Assert.ArgumentNotNull(catalogItem, "catalogItem");

      string searchTextBoxesText = catalogItem[this.searchTextBoxesFieldName];
      string searchCheckListsText = catalogItem[this.searchChecklistsFieldName];

      BusinessCatalogSettings businessCatalogSettings = Context.Entity.GetConfiguration<BusinessCatalogSettings>();
      Assert.IsNotNull(businessCatalogSettings, this.GetType(), "Business Catalog settings not found.");

      Item productRepositoryRootItem = catalogItem.Database.GetItem(businessCatalogSettings.ProductsLink);

      Assert.IsNotNull(productRepositoryRootItem, "Product Repository Root Item is null.");

      Query query = new Query { SearchRoot = productRepositoryRootItem.ID.ToString() };

      if (!string.IsNullOrEmpty(searchTextBoxesText))
      {
        this.ParseSearchTextBoxes(searchTextBoxesText, ref query);
      }

      if (!string.IsNullOrEmpty(searchTextBoxesText) && !string.IsNullOrEmpty(searchCheckListsText))
      {
        query.AppendCondition(QueryCondition.And);
        query.AppendSubquery(this.ParseSearchCheckLists(searchCheckListsText));
      }
      else if (!string.IsNullOrEmpty(searchCheckListsText))
      {
        query.AppendSubquery(this.ParseSearchCheckLists(searchCheckListsText));
      }

      return query;
    }

    /// <summary>
    /// Parses the search text boxes.
    /// </summary>
    /// <param name="searchTextBoxesText">The search text boxes text.</param>
    /// <param name="query">The query.</param>
    protected virtual void ParseSearchTextBoxes(string searchTextBoxesText, ref Query query)
    {
      UrlString searchTextBoxes = new UrlString(HttpUtility.UrlDecode(searchTextBoxesText));

      for (int i = 0; i < searchTextBoxes.Parameters.Count; i++)
      {
        string fieldName = searchTextBoxes.Parameters.Keys[i];
        query.AppendField(fieldName, searchTextBoxes[fieldName], MatchVariant.Like);
        if (i < searchTextBoxes.Parameters.Count - 1)
        {
          query.AppendCondition(QueryCondition.And);
        }
      }
    }

    /// <summary>
    /// Parses the search check lists.
    /// </summary>
    /// <param name="searchCheckListsText">The search check lists text.</param>
    /// <returns>The search Query SubQueries.</returns>
    protected virtual Query ParseSearchCheckLists(string searchCheckListsText)
    {
      Query checkListSubQuery = new Query();
      UrlString checklists = new UrlString(searchCheckListsText);

      for (int cl = 0; cl < checklists.Parameters.Count; cl++)
      {
        string fieldName = checklists.Parameters.Keys[cl];
        ListString checkBoxesValues = new ListString(checklists[fieldName], '|');

        Query subquery = new Query();

        for (int cb = 0; cb < checkBoxesValues.Count; cb++)
        {
          subquery.AppendField(fieldName, checkBoxesValues[cb], MatchVariant.Exactly);
          if (cb < checkBoxesValues.Count - 1)
          {
            subquery.AppendCondition(QueryCondition.Or);
          }
        }

        checkListSubQuery.AppendSubquery(subquery);

        if (cl < checklists.Parameters.Count - 1)
        {
          checkListSubQuery.AppendCondition(QueryCondition.And);
        }
      }

      return checkListSubQuery;
    }
  }
}