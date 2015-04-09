// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitecoreQueryBuilder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The Sitecore query builder.
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
  using Sitecore.Data;

  /// <summary>
  /// The Sitecore query builder.
  /// </summary>
  public class SitecoreQueryBuilder : StringQueryBuilder
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SitecoreQueryBuilder"/> class.
    /// </summary>
    /// <param name="query">The query.</param>
    public SitecoreQueryBuilder(Query query)
      : base(query)
    {
    }

    /// <summary>
    /// Builds the attribute sub query.
    /// </summary>
    /// <param name="subQuery">The subquery.</param>
    /// <returns>The subquery</returns>
    protected override string BuildAttributeSubQuery(AttributeQuery subQuery)
    {
      string matchVariant = this.GetMatchVariant(subQuery.MatchVariant);
      if (string.IsNullOrEmpty(matchVariant) && MatchVariant.Like == subQuery.MatchVariant)
      {
        return string.Format("contains(@@{0}, '{1}')", subQuery.Key, subQuery.Value);
      }

      return string.Format("@@{0} {2} '{1}'", subQuery.Key, subQuery.Value, matchVariant);
    }

    /// <summary>
    /// Builds the field sub query.
    /// </summary>
    /// <param name="subQuery">The sub query.</param>
    /// <returns>The subquery.</returns>
    protected override string BuildFieldSubQuery(FieldQuery subQuery)
    {
      string matchVariant = this.GetMatchVariant(subQuery.MatchVariant);
      if (string.IsNullOrEmpty(matchVariant) && MatchVariant.Like == subQuery.MatchVariant)
      {
        return string.Format("contains(@{0}, '{1}')", subQuery.Key, subQuery.Value);
      }

      return string.Format("@{0} {2} '{1}'", subQuery.Key, subQuery.Value, matchVariant);
    }

    /// <summary>
    /// Adds the search root.
    /// </summary>
    /// <param name="searchRoot">The search root.</param>
    /// <returns>Return formated search root.</returns>
    protected override string GetSearchRoot(string searchRoot)
    {
      return ID.IsID(searchRoot) ? string.Format("//{0}//*", searchRoot) : base.GetSearchRoot(searchRoot);
    }
  }
}