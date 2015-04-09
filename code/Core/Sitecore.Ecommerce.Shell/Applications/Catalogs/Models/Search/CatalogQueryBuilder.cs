// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatalogQueryBuilder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the CatalogQueryBuilder type.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models.Search
{
  using System.Collections.Specialized;
  using Ecommerce.Search;
  using Text;

  /// <summary>
  /// Defines the CatalogQueryBuilder type.
  /// </summary>
  public class CatalogQueryBuilder : QueryBuilderWrapper
  {
    /// <summary>
    /// Field Sub Query
    /// </summary>
    private Query fieldSubQuery;

    /// <summary>
    ///  Templates sub query
    /// </summary>
    private Query templateSubQuery;

    /// <summary>
    /// Check List SubQuery
    /// </summary>
    private Query checkListSubQuery;

    /// <summary>
    /// Gets or sets the field query.
    /// </summary>
    /// <value>The field query.</value>
    public Query FieldSubQuery
    {
      get
      {
        return this.fieldSubQuery ?? (this.fieldSubQuery = new Query());
      }

      set
      {
        this.fieldSubQuery = value;
      }
    }

    /// <summary>
    /// Gets or sets the templates sub query.
    /// </summary>
    /// <value>The templates sub query.</value>
    public Query TemplatesSubQuery
    {
      get
      {
        return this.templateSubQuery ?? (this.templateSubQuery = new Query());
      }

      set
      {
        this.templateSubQuery = value;
      }
    }

    /// <summary>
    /// Gets or sets the check lists sub query.
    /// </summary>
    /// <value>The check lists sub query.</value>
    public Query CheckListsSubQuery
    {
      get
      {
        return this.checkListSubQuery ?? (this.checkListSubQuery = new Query());
      }

      set
      {
        this.checkListSubQuery = value;
      }
    }

    /// <summary>
    /// Builds the query.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns>Returns Query</returns>
    public virtual Query BuildQuery(SearchOptions options)
    {
      this.SearchRoot = options.SearchRoot;

      this.AddSearchFields(options.SearchFields, QueryCondition.And);
      this.AddCheckboxes(options.Checklists, QueryCondition.And);
      this.AddTemplates(options.Templates, QueryCondition.And);

      return this.InnerBuilder;
    }

    /// <summary>
    /// Adds the checkboxes.
    /// </summary>
    /// <param name="checklists">The checklists.</param>
    /// <param name="condition">The condition.</param>
    public virtual void AddCheckboxes(NameValueCollection checklists, QueryCondition condition)
    {
      for (var cl = 0; cl < checklists.Count; cl++)
      {
        var fieldName = checklists.Keys[cl];
        var checkBoxesValues = new ListString(checklists[cl]);
        var subquery = new Query();
        for (var cb = 0; cb < checkBoxesValues.Count; cb++)
        {
          subquery.Add(new FieldQuery(fieldName, checkBoxesValues[cb], MatchVariant.Exactly));
          if (cb < checkBoxesValues.Count - 1)
          {
            subquery.AppendCondition(QueryCondition.Or);
          }
        }

        this.CheckListsSubQuery.AppendSubquery(subquery);
        if (cl < checklists.Count - 1)
        {
          this.CheckListsSubQuery.AppendCondition(QueryCondition.And);
        }
      }

      this.AddSubquery(this.CheckListsSubQuery, condition);
    }

    /// <summary>
    /// Adds the search fields.
    /// </summary>
    /// <param name="searchFields">The search fields.</param>
    /// <param name="condtion">The condition.</param>
    public virtual void AddSearchFields(NameValueCollection searchFields, QueryCondition condtion)
    {
      for (var i = 0; i < searchFields.Count; i++)
      {
        var key = searchFields.Keys[i];
        this.InnerBuilder.Add(new FieldQuery(key, searchFields[key], MatchVariant.Like));
        if (i < searchFields.Keys.Count - 1)
        {
          this.InnerBuilder.AppendCondition(QueryCondition.And);
        }
      }
    }

    /// <summary>
    /// Adds the templates.
    /// </summary>
    /// <param name="templates">The templates.</param>
    /// <param name="condition">The condition.</param>
    public virtual void AddTemplates(ListString templates, QueryCondition condition)
    {
      for (var i = 0; i < templates.Count; i++)
      {
        this.TemplatesSubQuery.Add(new AttributeQuery("templateId", templates[i], MatchVariant.Exactly));
        if (i < templates.Count - 1)
        {
          this.TemplatesSubQuery.AppendCondition(QueryCondition.Or);
        }
      }

      this.AddSubquery(this.TemplatesSubQuery, condition);
    }

    /// <summary>
    /// Adds the sub query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="condition">The condition.</param>
    protected virtual void AddSubquery(Query query, QueryCondition condition)
    {
      if (query.IsEmpty())
      {
        return;
      }

      if (this.InnerBuilder.IsEmpty())
      {
        this.InnerBuilder.AppendSubquery(query);
      }
      else
      {
        this.InnerBuilder.AppendCondition(condition);
        this.InnerBuilder.AppendSubquery(query);
      }
    }
  }
}