// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringQueryBuilder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Query Builder class.
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

namespace Sitecore.Ecommerce.Search
{
  using System.Text;

  /// <summary>
  /// Query Builder class.
  /// </summary>
  public abstract class StringQueryBuilder
  {
    /// <summary>
    /// Inner Query
    /// </summary>
    private Query innerQuery;

    /// <summary>
    /// Initializes a new instance of the <see cref="StringQueryBuilder"/> class.
    /// </summary>
    /// <param name="query">The query.</param>
    protected StringQueryBuilder(Query query)
    {
      this.innerQuery = query;
    }

    /// <summary>
    /// Gets the result query.
    /// </summary>    
    /// <returns>Returns result query.</returns>
    public virtual string BuildResultQuery()
    {
      StringBuilder query = new StringBuilder();

      query.Append(this.GetSearchRoot(this.innerQuery.SearchRoot));

      bool hasClauses = !this.innerQuery.IsEmpty();
      if (hasClauses)
      {
        query.Append("[");
      }

      this.BuildQuery(this.innerQuery.FirstNode, query);

      if (hasClauses)
      {
        query.Append("]");
      }

      return query.ToString();
    }

    /// <summary>
    /// Builds the query.
    /// </summary>
    /// <param name="node">The query node.</param>
    /// <param name="query">The query.</param>
    protected virtual void BuildQuery(QueryNode node, StringBuilder query)
    {
      if (node == null)
      {
        return;
      }

      if (node.Element is Query)
      {
        Query subQuery = node.Element as Query;
        if (!string.IsNullOrEmpty(subQuery.SearchRoot))
        {
          query.AppendFormat(" {0}//*", subQuery.SearchRoot);
        }

        if (subQuery.FirstNode != null)
        {
          query.Append("(");
          this.BuildQuery(subQuery.FirstNode, query);
          query.Append(")");
        }
      }
      else if (node.Element is Condition)
      {
        Condition condition = node.Element as Condition;
        query.AppendFormat(" {0} ", condition.QueryCondition.ToString().ToLower());
      }
      else if (node.Element is AttributeQuery)
      {
        query.Append(this.BuildAttributeSubQuery(node.Element as AttributeQuery));
      }
      else if (node.Element is FieldQuery)
      {
        query.Append(this.BuildFieldSubQuery(node.Element as FieldQuery));
      }

      if (node.NextNode != null)
      {
        this.BuildQuery(node.NextNode, query);
      }
    }

    /// <summary>
    /// Builds the attribute sub query.
    /// </summary>    
    /// <param name="subQuery">The subquery.</param>
    /// <returns>The subquery</returns>
    protected abstract string BuildAttributeSubQuery(AttributeQuery subQuery);

    /// <summary>
    /// Builds the field sub query.
    /// </summary>    
    /// <param name="subQuery">The sub query.</param>
    /// <returns>The subquery.</returns>
    protected abstract string BuildFieldSubQuery(FieldQuery subQuery);

    /// <summary>
    /// Gets the match variant.
    /// </summary>
    /// <param name="variant">The variant.</param>
    /// <returns>Returns string representing match variant.</returns>
    protected virtual string GetMatchVariant(MatchVariant variant)
    {
      string result = string.Empty;
      switch (variant)
      {
        case MatchVariant.Exactly:
          result = "=";
          break;
        case MatchVariant.GreaterThan:
          result = ">";
          break;
        case MatchVariant.GreaterThanOrEqual:
          result = ">=";
          break;
        case MatchVariant.LessThan:
          result = "<";
          break;
        case MatchVariant.LessThanOrEqual:
          result = "<=";
          break;
        case MatchVariant.NotEquals:
          result = "!=";
          break;
      }

      return result;
    }

    /// <summary>
    /// Adds the search root.
    /// </summary>
    /// <param name="searchRoot">The search root.</param>
    /// <returns>Return formated search root.</returns>
    protected virtual string GetSearchRoot(string searchRoot)
    {
      return this.innerQuery.SearchRoot + "//*";
    }
  }
}