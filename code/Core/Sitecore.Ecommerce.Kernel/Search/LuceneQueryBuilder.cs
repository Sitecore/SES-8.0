// -------------------------------------------------------------------------------------------
// <copyright file="LuceneQueryBuilder.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Search
{
  using Diagnostics;
  using Lucene.Net.Index;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using LuceneSearch = Lucene.Net.Search;

  /// <summary>
  /// Lucene Query Builder class
  /// </summary>
  public class LuceneQueryBuilder
  {
    /// <summary>
    ///  Query builder result query
    /// </summary>
    private LuceneSearch.BooleanQuery resultQuery;

    /// <summary>
    /// Internal query
    /// </summary>
    private Query internalQuery;

    /// <summary>
    /// Initializes a new instance of the <see cref="LuceneQueryBuilder"/> class.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="database">The database.</param>
    public LuceneQueryBuilder(Query query, Database database)
    {
      Assert.ArgumentNotNull(query, "query");
      Assert.ArgumentNotNull(database, "database");

      this.resultQuery = new LuceneSearch.BooleanQuery();
      this.internalQuery = query;
      this.Database = database;
    }

    /// <summary>
    /// Gets or sets the database.
    /// </summary>
    /// <value>The database.</value>
    protected Database Database { get; set; }

    /// <summary>
    /// Builds the result query.
    /// </summary>
    /// <returns>returns Query object</returns>
    public Lucene.Net.Search.BooleanQuery BuildResultQuery()
    {
      this.AddSearchRoot(this.resultQuery, this.internalQuery.SearchRoot);

      if (this.internalQuery.FirstNode != null)
      {
        Lucene.Net.Search.BooleanQuery condition = this.resultQuery;
        
        this.BuildQuery(new Lucene.Net.Search.BooleanQuery(), this.internalQuery.FirstNode);
        condition.Add(this.resultQuery, Lucene.Net.Search.Occur.MUST);

        this.resultQuery = condition;
      }

      return this.resultQuery;
    }

    /// <summary>
    /// Adds the serch root.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="searchRoot">The search root.</param>
    protected virtual void AddSearchRoot(LuceneSearch.BooleanQuery query, string searchRoot)
    {
      if (!string.IsNullOrEmpty(searchRoot))
      {
        if (ID.IsID(searchRoot))
        {
          searchRoot = this.GetItemPath(new ID(searchRoot));
        }
        else
        {
          Item rootItem = this.Database.SelectSingleItem(searchRoot);
          if (rootItem != null)
          {
            searchRoot = this.GetItemPath(rootItem.ID);
          }
        }

        query.Add(new LuceneSearch.TermQuery(new Term(Sitecore.Search.BuiltinFields.Path, searchRoot)), LuceneSearch.Occur.MUST);
      }
    }

    /// <summary>
    /// Builds the query.
    /// </summary>
    /// <param name="query">The result query.</param>
    /// <param name="node">The query node.</param>
    protected virtual void BuildQuery(LuceneSearch.BooleanQuery query, QueryNode node)
    {
      Query subQuery = node.Element as Query;
      if (subQuery != null && !subQuery.IsEmpty())
      {
        LuceneSearch.BooleanQuery booleanQuery = new LuceneSearch.BooleanQuery();

        if (!string.IsNullOrEmpty(subQuery.SearchRoot))
        {
          this.AddSearchRoot(booleanQuery, subQuery.SearchRoot);
        }

        this.BuildQuery(booleanQuery, subQuery.FirstNode);
        LuceneSearch.Occur occurance = LuceneSearch.Occur.MUST;
        if (node.IsFirst)
        {
          if (!node.IsLast && (node.NextNode.Element is Condition))
          {
            occurance = this.GetOccur(((Condition)node.NextNode.Element).QueryCondition);
          }
        }
        else
        {
          if (!node.IsFirst && (node.PreviousNode.Element is Condition))
          {
            occurance = this.GetOccur(((Condition)node.PreviousNode.Element).QueryCondition);
          }
        }

        query.Add(booleanQuery, occurance);
      }
      else if (node.Element is AttributeQuery || node.Element is FieldQuery)
      {
        QueryCondition condition = QueryCondition.And;
        
        if (node.IsFirst)
        {
          if (!node.IsLast && (node.NextNode.Element is Condition))
          {
            condition = ((Condition)node.NextNode.Element).QueryCondition;
          }          
        }
        else
        {
          if (!node.IsFirst && (node.PreviousNode.Element is Condition))
          {
            condition = ((Condition)node.PreviousNode.Element).QueryCondition;
          }
        }

        this.AddSubQuery(query, node.Element as SubQuery, condition);
      }

      if (!node.IsLast)
      {
        this.BuildQuery(query, node.NextNode);
      }

      this.resultQuery = query;
    }

    /// <summary>
    /// Adds the term query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="subquery">The subquery.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="isFirst">if set to <c>true</c> [is first].</param>
    protected virtual void AddSubQuery(LuceneSearch.BooleanQuery query, SubQuery subquery, QueryCondition condition)
    {
      string value = ID.IsID(subquery.Value) ? subquery.Value : subquery.Value.ToLower();
      string key = subquery.Key.ToLower();
      if (subquery is AttributeQuery)
      {
        key = this.MapAttributes(key);
      }

      // optimizing search query
      if (key == Sitecore.Search.BuiltinFields.ID ||
          key == Sitecore.Search.BuiltinFields.Template ||
          key == BuiltinFields.ProductBaseTemplate ||
          key == Sitecore.Search.BuiltinFields.AllTemplates)
      {
        this.AddIdQuery(query, key, value, condition);
      }
      else
      {
        this.AddContentSubQuery(query, key, value, subquery.MatchVariant, condition);
      }
    }

    /// <summary>
    /// Gets the occur.
    /// </summary>
    /// <param name="condition">The condition.</param>
    /// <returns>Returns Lucene Occurrence</returns>
    protected virtual LuceneSearch.Occur GetOccur(QueryCondition condition)
    {
      return condition == QueryCondition.And ? LuceneSearch.Occur.MUST : LuceneSearch.Occur.SHOULD;
    }

    /// <summary>
    /// Maps the attributes.
    /// </summary>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <returns> Returns lucene presentation of attributes </returns>
    protected virtual string MapAttributes(string attributeName)
    {
      switch (attributeName.ToLower())
      {
        case "templateid":
          return Sitecore.Search.BuiltinFields.Template;
        case "id":
          return Sitecore.Search.BuiltinFields.ID;
        case "name":
          return Sitecore.Search.BuiltinFields.Name;
        case "key":
          return Sitecore.Search.BuiltinFields.Name;
        default:
          return attributeName;
      }
    }

    /// <summary>
    /// Gets the item path.
    /// </summary>
    /// <param name="id">The item to proceed</param>
    /// <returns>Returns lucente representation of Sitecore item path.</returns>
    protected string GetItemPath(ID id)
    {
      return ShortID.Encode(id).ToLowerInvariant();
    }

    /// <summary>
    /// Adds the simple query.
    /// </summary>
    /// <param name="query">The boolean query.</param>
    /// <param name="key">The field key.</param>
    /// <param name="value">The field value.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="isFirst">if set to <c>true</c> [is first].</param>
    private void AddIdQuery(LuceneSearch.BooleanQuery query, string key, string value, QueryCondition condition)
    {
      value = this.GetItemPath(new ID(value));
      LuceneSearch.Occur occurrence = this.GetOccur(condition);
      query.Add(new LuceneSearch.TermQuery(new Term(key, value)), occurrence);
    }

    /// <summary>
    /// Adds the content sub query.
    /// </summary>
    /// <param name="query">The boolean query.</param>
    /// <param name="key">The field key.</param>
    /// <param name="value">The field value.</param>
    /// <param name="matchVariant">The match variant.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="isFirst">if set to <c>true</c> [is first].</param>
    private void AddContentSubQuery(LuceneSearch.BooleanQuery query, string key, string value, MatchVariant matchVariant, QueryCondition condition)
    {
      if (matchVariant == MatchVariant.NotEquals)
      {
        query.Add(new LuceneSearch.TermQuery(new Term(key, value)), LuceneSearch.Occur.MUST_NOT);
        return;
      }

      LuceneSearch.Occur occurrence = this.GetOccur(condition);

      LuceneSearch.TermRangeQuery rangeQuery = this.GetRangeQuery(key, value, matchVariant);
      if (rangeQuery != null)
      {
        query.Add(rangeQuery, occurrence);
        return;
      }

      string[] keywords = value.Split(' ');
      if (keywords.Length > 1)
      {
        LuceneSearch.PhraseQuery phraseQuery = new Lucene.Net.Search.PhraseQuery();

        foreach (string keyword in keywords)
        {
          phraseQuery.Add(new Term(key, keyword));
        }

        query.Add(phraseQuery, occurrence);
      }
      else if (matchVariant == MatchVariant.Like)
      {
        query.Add(new LuceneSearch.WildcardQuery(new Term(key, value + "*")), occurrence);
      }
      else
      {
        query.Add(new LuceneSearch.TermQuery(new Term(key, value)), occurrence);
      }
    }

    protected virtual LuceneSearch.TermRangeQuery GetRangeQuery(string key, string value, MatchVariant variant)
    {
      switch (variant)
      {
        case MatchVariant.GreaterThan:
          return new LuceneSearch.TermRangeQuery(key, value, null, false, true);
        case MatchVariant.GreaterThanOrEqual:
          return new LuceneSearch.TermRangeQuery(key, value, null, true, true);
        case MatchVariant.LessThan:
          return new LuceneSearch.TermRangeQuery(key, null, value, true, false);
        case MatchVariant.LessThanOrEqual:
          return new LuceneSearch.TermRangeQuery(key, null, value, true, true);
      }

      return null;
    }
  }
}