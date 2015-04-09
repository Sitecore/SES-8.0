// -------------------------------------------------------------------------------------------
// <copyright file="PropertyComparer.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Collections
{
  using System.Collections;
  using System.Reflection;

  /// <summary>
  /// The class helper to compare two objects by Entity properties
  /// </summary>
  public class PropertyComparer : IEqualityComparer
  {
    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="x">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <param name="y">The object to compare</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other.
    /// </exception>
    public virtual new bool Equals(object x, object y)
    {
      int equalProperties = 0;
      int totalPropertiesCount = 0;
      foreach (PropertyInfo propertyInfo in x.GetType().GetProperties())
      {
        totalPropertiesCount++;
        PropertyInfo specifiedPropery = y.GetType().GetProperty(propertyInfo.Name);

        object xValue = propertyInfo.GetValue(x, null);
        object yValue = specifiedPropery.GetValue(y, null);

        if (xValue == null && yValue == null)
        {
          equalProperties++;
          continue;
        }

        if (xValue == null || yValue == null)
        {
          continue;
        }

        if (xValue.Equals(yValue))
        {
          equalProperties++;
        }
      }

      return equalProperties == totalPropertiesCount;
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <param name="obj">The object</param>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
    /// </exception>
    public virtual int GetHashCode(object obj)
    {
      return base.GetHashCode();
    }    
  }
}