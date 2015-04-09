// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductSpecification.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the product specification class.
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

namespace Sitecore.Ecommerce.DomainModel.Products
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// Defines the product specification class.
  /// </summary>
  [Serializable]
  public class ProductSpecification : IEnumerable<KeyValuePair<string, object>>
  {
    /// <summary>
    /// The inner dictionary.
    /// </summary>
    private readonly IDictionary<string, object> innerData;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductSpecification"/> class.
    /// </summary>
    /// <param name="keys">The keys.</param>
    public ProductSpecification(IEnumerable<string> keys)
    {
      this.innerData = new Dictionary<string, object>(keys.Count());

      foreach (string key in keys)
      {
        this.innerData.Add(key, string.Empty);
      }
    }

    /// <summary>
    /// Gets the keys.
    /// </summary>
    public ICollection<string> Keys
    {
      get { return this.innerData.Keys; }
    }
    
    /// <summary>
    /// Gets the count.
    /// </summary>
    public int Count
    {
      get { return this.innerData.Count; }
    }

    /// <summary>
    /// Gets or sets the <see cref="System.Object"/> with the specified key.
    /// </summary>
    /// <param name="key">The product specification key.</param>
    /// <exception cref="KeyNotFoundException">The given product specification field is not found.</exception>
    public object this[string key]
    {
      get
      {
        return this.innerData[key];
      }

      set
      {
        if (!this.ContainsKey(key))
        {
          throw new KeyNotFoundException("The given product specification key is not found.");
        }

        this.innerData[key] = value;
      }
    }

    /// <summary>
    /// Determines whether the specification contains key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>
    ///   <c>true</c> if the specification contains key; otherwise, <c>false</c>.
    /// </returns>
    public bool ContainsKey(string key)
    {
      return this.innerData.ContainsKey(key);
    }

    #region IEnumerable<KeyValuePair<string,object>> Members

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
    {
      return this.innerData.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.innerData.GetEnumerator();
    }

    #endregion
  }
}