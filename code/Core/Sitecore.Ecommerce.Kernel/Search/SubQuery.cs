// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubQuery.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SubQuery type.
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

  /// <summary>
  /// Defines the SubQuery type.
  /// </summary>
  [Serializable]
  public abstract class SubQuery : QueryElement
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SubQuery"/> class.
    /// </summary>
    protected SubQuery()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubQuery"/> class.
    /// </summary>
    /// <param name="key">The field key.</param>
    /// <param name="value">The value.</param>
    /// <param name="matchVariant">The match variant.</param>
    protected SubQuery(string key, string value, MatchVariant matchVariant)
    {
      this.Key = key;
      this.Value = value;
      this.MatchVariant = matchVariant;
    }

    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    /// <value>The name of the field.</value>
    public string Key { get; set; }

    /// <summary>    
    /// Gets or sets the field value.
    /// </summary>
    /// <value>The field value.</value>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the condition.
    /// </summary>
    /// <value>The condition.</value>
    public MatchVariant MatchVariant { get; set; }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      var query = obj as SubQuery;
      if (query == null)
      {
        return false;
      }

      return string.Compare(this.Key, query.Key, StringComparison.OrdinalIgnoreCase) == 0 &&
             string.Compare(this.Value, query.Value, StringComparison.OrdinalIgnoreCase) == 0 && this.MatchVariant == query.MatchVariant;
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode()
    {
      unchecked
      {
        var result = this.Key.GetHashCode();
        result = (result * 397) ^ this.Value.GetHashCode();
        result = (result * 397) ^ this.MatchVariant.GetHashCode();
        return result;
      }
    }
  }
}