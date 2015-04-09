// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchMethod.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SearchMethod type.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models
{
  using Collections;
  using Data;
  using Sitecore.Data;

  /// <summary>
  /// Defines the SearchMethod type.
  /// </summary>
  public class SearchMethod
  {
    /// <summary>
    /// Gets or sets the search method name.
    /// </summary>
    /// <value>The search method name.</value>
    public ID ID { get; set; }

    /// <summary>
    /// Gets or sets the search method title.
    /// </summary>
    /// <value>The search method title.</value>
    [Entity(FieldName = "Title")]
    public string Title { get; set; }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="T:System.NullReferenceException">
    /// The <paramref name="obj"/> parameter is null.
    /// </exception>
    public override bool Equals(object obj)
    {
      PropertyComparer comparer = new PropertyComparer();
      return comparer.Equals(this, obj);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}