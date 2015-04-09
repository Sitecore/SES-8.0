// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpeakFreeTextSearchExtractor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the speak free text search extractor class.
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

  /// <summary>
  /// Defines the speak free text search extractor class.
  /// </summary>
  /// <typeparam name="T">The type of value on which extractor will operate.</typeparam>
  public class SpeakFreeTextSearchExtractor<T>
  {
    /// <summary>
    /// The free text search operation token.
    /// </summary>
    private const string FreeTextSearchOperationToken = "FreeTextSearch(o.";

    /// <summary>
    /// The free text search expression template.
    /// </summary>
    private const string FreeTextSearchExpressionTemplate = "(o.{0} != null ? o.{0} : string.Empty).ToLower().Contains({1}.ToLower())";

    /// <summary>
    /// The false token.
    /// </summary>
    private const string FalseToken = "false";

    /// <summary>
    /// Extracts the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>The expression where free text search is applied only where it is supported.</returns>
    public string Extract(string expression)
    {
      int index = -1;
      
      do
      {
        index = expression.IndexOf(FreeTextSearchOperationToken, index + 1, System.StringComparison.Ordinal);

        if (index >= 0)
        {
          string propertyName = expression.Substring(index + FreeTextSearchOperationToken.Length, expression.IndexOf(',', index) - index - FreeTextSearchOperationToken.Length);
          string value = expression.Substring(expression.IndexOf(',', index) + 1, expression.IndexOf(')', index) - expression.IndexOf(',', index) - 1);

          expression = expression.Remove(index, expression.IndexOf(')', index) - index + 1);

          if (this.GetPropertyType(typeof(T), propertyName) == typeof(string))
          {
            expression = expression.Insert(index, string.Format(FreeTextSearchExpressionTemplate, propertyName, value));
          }
          else
          {
            expression = expression.Insert(index, FalseToken);
            --index;
          }
        }
        else
        {
          break;
        }
      }
      while (true);
      
      return expression;
    }

    /// <summary>
    /// Gets the type of the property.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>The type of the property.</returns>
    private Type GetPropertyType(Type type, string propertyName)
    {
      if (string.IsNullOrEmpty(propertyName))
      {
        return type;
      }

      return GetPropertyType(type.GetProperty(propertyName.Substring(0, propertyName.IndexOf('.') >= 0 ? propertyName.IndexOf('.') : propertyName.Length)).PropertyType, propertyName.IndexOf('.') != -1 ? propertyName.Substring(propertyName.IndexOf('.') + 1) : string.Empty);
    }
  }
}
