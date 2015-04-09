// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FastQueryBuilder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The Fast Query builder.
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
  using Sitecore.Data;

  /// <summary>
  /// The Fast Query builder.
  /// </summary>
  public class FastQueryBuilder : StringQueryBuilder
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="FastQueryBuilder"/> class.
    /// </summary>
    /// <param name="query">The query.</param>
    public FastQueryBuilder(Query query)
      : base(query)
    {
    }

    /// <summary>
    /// Gets the result query.
    /// </summary>    
    /// <returns>Returns result query.</returns>
    public override string BuildResultQuery()
    {
      return string.Concat("fast:", base.BuildResultQuery());
    }

    /// <summary>
    /// Builds the attribute sub query.
    /// </summary>
    /// <param name="subQuery">The subquery.</param>
    /// <returns>The subquery</returns>
    protected override string BuildAttributeSubQuery(AttributeQuery subQuery)
    {
      string variant = this.GetMatchVariant(subQuery.MatchVariant);
      if (string.IsNullOrEmpty(variant) && subQuery.MatchVariant == MatchVariant.Like)
      {
        return string.Format("@@{0} = '{2}{1}{2}'", subQuery.Key, this.EscapeSpecialCharacters(subQuery.Value), "%");
      }

      return string.Format("@@{0} {2} '{1}'", subQuery.Key, subQuery.Value, variant);
    }

    /// <summary>
    /// Builds the field sub query.
    /// </summary>
    /// <param name="subQuery">The sub query.</param>
    /// <returns>The subquery.</returns>
    protected override string BuildFieldSubQuery(FieldQuery subQuery)
    {
      string variant = this.GetMatchVariant(subQuery.MatchVariant);
      if (string.IsNullOrEmpty(variant) && subQuery.MatchVariant == MatchVariant.Like)
      {
        return string.Format("@{0} = '{2}{1}{2}'", subQuery.Key, this.EscapeSpecialCharacters(subQuery.Value), "%");
      }

      return string.Format("@{0} {2} '{1}'", subQuery.Key, subQuery.Value, variant);
    }

    /// <summary>
    /// Adds the search root.
    /// </summary>
    /// <param name="searchRoot">The search root.</param>
    /// <returns>Return formated search root.</returns>
    protected override string GetSearchRoot(string searchRoot)
    {
      return ID.IsID(searchRoot) ? string.Format("//*[@@id='{0}']//*", searchRoot) : base.GetSearchRoot(searchRoot);
    }

    /// <summary>
    /// Escapes the special characters.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The special characters.</returns>
    protected virtual string EscapeSpecialCharacters(string value)
    {
      StringBuilder result = new StringBuilder();

      foreach (char c in value)
      {
        switch (c)
        {
          case '%':
            {
              result.Append("[%]");
              break;
            }

          case '_':
            {
              result.Append("[_]");
              break;
            }

          case '[':
            {
              result.Append("[[]");
              break;
            }

          case ']':
            {
              result.Append("[]]");
              break;
            }

          default:
            {
              result.Append(c);
              break;
            }
        }
      }

      return result.ToString();
    }
  }
}