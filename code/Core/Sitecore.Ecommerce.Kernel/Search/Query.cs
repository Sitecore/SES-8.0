// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Query.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines Query class.
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
  using System;
  using System.Runtime.Serialization;

  /// <summary>
  /// Defines Query class.
  /// </summary>
  [DataContract]
  [KnownType(typeof(SubQuery))]
  [KnownType(typeof(AttributeQuery))]
  [KnownType(typeof(FieldQuery))]
  [KnownType(typeof(Condition))]
  public class Query : QueryElement
  {
    /// <summary>
    /// Gets the first node.
    /// </summary>
    /// <value>The first node.</value>
    [DataMember(IsRequired = true)]
    public QueryNode FirstNode { get; private set; }

    /// <summary>
    /// Gets last node.
    /// </summary>
    /// <value>The last node.</value>
    [DataMember(IsRequired = true)]
    public QueryNode LastNode { get; private set; }

    /// <summary>
    /// Gets or sets the search root.
    /// </summary>
    /// <value>The search root.</value>
    [DataMember(IsRequired = true)]
    public string SearchRoot { get; set; }

    /// <summary>
    /// Appends the subquery.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="value">The attribute value.</param>
    /// <param name="matchVariant">The match variant.</param>
    public void AppendAttribute(string key, string value, MatchVariant matchVariant)
    {
      AttributeQuery query = new AttributeQuery(key, value, matchVariant);
      QueryNode node = new QueryNode(query);
      this.AddNode(node);
    }

    /// <summary>
    /// Appends the field.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="value">The attribute value.</param>
    /// <param name="matchVariant">The match variant.</param>
    public void AppendField(string key, string value, MatchVariant matchVariant)
    {
      FieldQuery fldQuery = new FieldQuery(key, value, matchVariant);
      QueryNode node = new QueryNode(fldQuery);
      this.AddNode(node);
    }

    /// <summary>
    /// Adds the condition.
    /// </summary>
    /// <param name="condition">The condition.</param>
    public void AppendCondition(QueryCondition condition)
    {
      QueryNode node = new QueryNode(new Condition(condition));
      this.AddNode(node);
    }

    /// <summary>
    /// Appends the subquery.
    /// </summary>
    /// <param name="query">The query.</param>
    public void AppendSubquery(Query query)
    {
      QueryNode node = new QueryNode(query);
      this.AddNode(node);
    }

    /// <summary>
    /// Adds the specified field query.
    /// </summary>
    /// <param name="fieldQuery">The field query.</param>
    public void Add(FieldQuery fieldQuery)
    {
      this.AddNode(new QueryNode(fieldQuery));
    }

    /// <summary>
    /// Adds the specified atr query.
    /// </summary>
    /// <param name="atrQuery">The atribute query.</param>
    public void Add(AttributeQuery atrQuery)
    {
      this.AddNode(new QueryNode(atrQuery));
    }

    /// <summary>
    /// Adds the specified condition.
    /// </summary>
    /// <param name="condition">The condition.</param>
    public void Add(QueryCondition condition)
    {
      this.AppendCondition(condition);    
    }

    /// <summary>
    /// Determines whether this instance is empty.
    /// </summary>
    /// <returns>
    /// <c>true</c> if query instance is empty; otherwise, <c>false</c>.
    /// </returns>
    public bool IsEmpty()
    {
      return this.FirstNode == null;
    }

    /// <summary>
    /// Determines whether [contains] [the specified query].
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>
    /// <c>true</c> if [contains] [the specified query]; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(AttributeQuery query)
    {
      return this.Contains(this.FirstNode, query);
    }

    /// <summary>
    /// Determines whether [contains] [the specified query].
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>
    /// <c>true</c> if [contains] [the specified query]; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(FieldQuery query)
    {
      return this.Contains(this.FirstNode, query);
    }

    /// <summary>
    /// Gets the readable representation of the query.
    /// </summary>
    /// <returns>Readable representation of the query</returns>
    public override string ToString()
    {
      StringQueryBuilder builder = new FastQueryBuilder(this);
      return builder.BuildResultQuery().Substring(5);
    }

    /// <summary>
    /// Determines whether the specified Object is equal to the current <see cref="Query"/>.
    /// </summary>
    /// <param name="obj">The Object to compare with the current <see cref="Query"/>.</param>
    /// <returns>true if the specified Object is equal to the current <see cref="Query"/>; otherwise, false. </returns>
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }

      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (obj.GetType() != typeof(Query))
      {
        return false;
      }

      return this.Equals((Query)obj);
    }

    /// <summary>
    /// Determines whether the specified <see cref="Query"/> is equal to the current one.
    /// </summary>
    /// <param name="other">The <see cref="Query"/> to compare with the current one.</param>
    /// <returns>true if the specified <see cref="Query"/> is equal to the current one; otherwise, false. </returns>
    public bool Equals(Query other)
    {
      if (ReferenceEquals(null, other))
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return Object.Equals(other.FirstNode, this.FirstNode) && Object.Equals(other.LastNode, this.LastNode) && Object.Equals(other.SearchRoot, this.SearchRoot);
    }

    /// <summary>
    /// Gets a hash code.
    /// </summary>
    /// <returns>
    /// A hash code.
    /// </returns>
    public override int GetHashCode()
    {
      unchecked
      {
        int result = this.FirstNode != null ? this.FirstNode.GetHashCode() : 0;
        result = (result * 397) ^ (this.LastNode != null ? this.LastNode.GetHashCode() : 0);
        result = (result * 397) ^ (this.SearchRoot != null ? this.SearchRoot.GetHashCode() : 0);
        return result;
      }
    }

    /// <summary>
    /// Determines whether [contains] [the specified node].
    /// </summary>
    /// <param name="node">The query node.</param>
    /// <param name="query">The query.</param>
    /// <returns>
    /// <c>true</c> if [contains] [the specified node]; otherwise, <c>false</c>.
    /// </returns>
    protected virtual bool Contains(QueryNode node, SubQuery query)
    {
      if (node.Element.Equals(query))
      {
        return true;
      }

      if (node.IsQuery())
      {
        return this.Contains(((Query)node.Element).FirstNode, query);
      }

      if (!node.IsLast)
      {
        return this.Contains(node.NextNode, query);
      }

      return false;
    }

    /// <summary>
    /// Adds the node.
    /// </summary>
    /// <param name="node">The query node.</param>
    /// <exception cref="ArgumentException">Cannot add two conditions in succession</exception>
    protected virtual void AddNode(QueryNode node)
    {
      Query query = node.Element as Query;

      if (query != null && this.Equals(query))
      {
        throw new ArgumentException("Cannot insert same query in it self");
      }

      if (this.IsEmpty() && node.Element is Condition)
      {
        throw new ArgumentException("First argument cannot be a condition");
      }

      if (node.IsQuery() && ((Query)node.Element).IsEmpty())
      {
        throw new ArgumentException("Query is invalid");
      }

      if (this.IsEmpty())
      {
        this.FirstNode = node;
        this.LastNode = node;
      }
      else
      {
        if (this.LastNode.Element is Condition && node.Element is Condition)
        {
          throw new ArgumentException("Cannot add two conditions in succession");
        }

        if (this.LastNode.Element is SubQuery && node.Element is SubQuery)
        {
          throw new ArgumentException("Cannot add two subqueries without condition");
        }

        if (this.FirstNode.NextNode == null)
        {
          this.FirstNode.NextNode = node;
        }
        else
        {
          node.PreviousNode = this.LastNode;
          this.LastNode.NextNode = node;
        }

        this.LastNode = node;
      }
    }
  }
}