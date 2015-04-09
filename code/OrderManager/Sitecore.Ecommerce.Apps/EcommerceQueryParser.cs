// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EcommerceQueryParser.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ecommerce query parser class.
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

namespace Sitecore.Ecommerce.Apps
{
  using System;
  using Diagnostics;
  using Search;

  /// <summary>
  /// Defines the ecommerce query parser class.
  /// </summary>
  public class EcommerceQueryParser
  {
    /// <summary>
    /// Parses the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>The converted query.</returns>
    /// <exception cref="InvalidOperationException">Query contains operators that are not supported</exception>
    public virtual Query Parse(Sitecore.Data.Query.Opcode query)
    {
      Assert.IsNotNull(query, "Query must not be null");

      Sitecore.Data.Query.Predicate predicate = query as Sitecore.Data.Query.Predicate;

      if (predicate != null)
      {
        Assert.IsNull(predicate.NextStep, "Continuation is not supported");
        return this.Parse(predicate.Expression);
      }

      Sitecore.Data.Query.BinaryOperator binaryOperator = query as Sitecore.Data.Query.BinaryOperator;

      if (binaryOperator != null)
      {
        if (binaryOperator is Sitecore.Data.Query.AndOperator)
        {
          Query result = new Query();
          result.AppendSubquery(this.Parse(binaryOperator.Left));
          result.AppendCondition(QueryCondition.And);
          result.AppendSubquery(this.Parse(binaryOperator.Right));

          return result;
        }

        if (binaryOperator is Sitecore.Data.Query.OrOperator)
        {
          Query result = new Query();
          result.AppendSubquery(this.Parse(binaryOperator.Left));
          result.AppendCondition(QueryCondition.Or);
          result.AppendSubquery(this.Parse(binaryOperator.Right));

          return result;
        }

        string fieldName = null;
        string fieldValue = null;

        if (binaryOperator.Left is Sitecore.Data.Query.FieldElement)
        {
          fieldName = ((Sitecore.Data.Query.FieldElement)binaryOperator.Left).Name;

          if (binaryOperator.Right is Sitecore.Data.Query.BooleanValue)
          {
            fieldValue = ((Sitecore.Data.Query.BooleanValue)binaryOperator.Right).Value.ToString();
          }
          else if (binaryOperator.Right is Sitecore.Data.Query.Number)
          {
            fieldValue = ((Sitecore.Data.Query.Number)binaryOperator.Right).Value.ToString();
          }
          else if (binaryOperator.Right is Sitecore.Data.Query.Literal)
          {
            fieldValue = ((Sitecore.Data.Query.Literal)binaryOperator.Right).Text;
          }
        }

        if (binaryOperator.Right is Sitecore.Data.Query.FieldElement)
        {
          fieldName = ((Sitecore.Data.Query.FieldElement)binaryOperator.Right).Name;

          if (binaryOperator.Left is Sitecore.Data.Query.BooleanValue)
          {
            fieldValue = ((Sitecore.Data.Query.BooleanValue)binaryOperator.Left).Value.ToString();
          }
          else if (binaryOperator.Left is Sitecore.Data.Query.Number)
          {
            fieldValue = ((Sitecore.Data.Query.Number)binaryOperator.Left).Value.ToString();
          }
          else if (binaryOperator.Left is Sitecore.Data.Query.Literal)
          {
            fieldValue = ((Sitecore.Data.Query.Literal)binaryOperator.Left).Text;
          }
        }

        if (binaryOperator is Sitecore.Data.Query.EqualsOperator)
        {
          Assert.IsNotNull(fieldName, "fieldName must not be null");
          Assert.IsNotNull(fieldValue, "fieldValue must not be null");

          Query result = new Query();

          if (fieldValue.StartsWith("%") && fieldValue.EndsWith("%") && (fieldValue.Length > 1))
          {
            if (fieldName.StartsWith("@"))
            {
              result.AppendAttribute(fieldName.Substring(1), fieldValue.Substring(1, fieldValue.Length - 2), MatchVariant.Like);
            }
            else
            {
              result.AppendField(fieldName, fieldValue.Substring(1, fieldValue.Length - 2), MatchVariant.Like);
            }
          }
          else
          {
            if (fieldName.StartsWith("@"))
            {
              result.AppendAttribute(fieldName.Substring(1), fieldValue, MatchVariant.Exactly);
            }
            else
            {
              result.AppendField(fieldName, fieldValue, MatchVariant.Exactly);
            }
          }

          return result;
        }

        if (binaryOperator is Sitecore.Data.Query.UnequalsOperator)
        {
          Assert.IsNotNull(fieldName, "fieldName must not be null");
          Assert.IsNotNull(fieldValue, "fieldValue must not be null");
          Assert.IsFalse(fieldValue.StartsWith("%") && fieldValue.EndsWith("%") && (fieldValue.Length > 1), "'%' sign is legal only with equality operator");

          Query result = new Query();

          if (fieldName.StartsWith("@"))
          {
            result.AppendAttribute(fieldName.Substring(1), fieldValue, MatchVariant.NotEquals);
          }
          else
          {
            result.AppendField(fieldName, fieldValue, MatchVariant.NotEquals);
          }

          return result;
        }

        if (binaryOperator is Sitecore.Data.Query.GreaterOperator)
        {
          Assert.IsNotNull(fieldName, "fieldName must not be null");
          Assert.IsNotNull(fieldValue, "fieldValue must not be null");
          Assert.IsFalse(fieldValue.StartsWith("%") && fieldValue.EndsWith("%") && (fieldValue.Length > 1), "'%' sign is legal only with equality operator");

          Query result = new Query();

          if (fieldName.StartsWith("@"))
          {
            result.AppendAttribute(fieldName.Substring(1), fieldValue, MatchVariant.GreaterThan);
          }
          else
          {
            result.AppendField(fieldName, fieldValue, MatchVariant.GreaterThan);
          }

          return result;
        }

        if (binaryOperator is Sitecore.Data.Query.SmallerOperator)
        {
          Assert.IsNotNull(fieldName, "fieldName must not be null");
          Assert.IsNotNull(fieldValue, "fieldValue must not be null");
          Assert.IsFalse(fieldValue.StartsWith("%") && fieldValue.EndsWith("%") && (fieldValue.Length > 1), "'%' sign is legal only with equality operator");

          Query result = new Query();

          if (fieldName.StartsWith("@"))
          {
            result.AppendAttribute(fieldName.Substring(1), fieldValue, MatchVariant.LessThan);
          }
          else
          {
            result.AppendField(fieldName, fieldValue, MatchVariant.LessThan);
          }

          return result;
        }
      }

      throw new InvalidOperationException("Query contains operators that are not supported");
    }
  }
}
