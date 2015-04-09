// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridColumn.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the GridColumn type.
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

  /// <summary>
  /// Grid column class
  /// </summary>
  public class GridColumn 
  {
    /// <summary>
    /// Gets or sets the grid column field name.
    /// </summary>
    /// <value>The column name.</value>
    [Entity(FieldName = "Field Name")]
    public virtual string FieldName { get; set; }

    /// <summary>
    /// Gets or sets the grid column header.
    /// </summary>
    /// <value>The header.</value>
    [Entity(FieldName = "Header")]
    public virtual string Header { get; set; }

    /// <summary>
    /// Gets or sets the format string.
    /// </summary>
    /// <value>The format string.</value>
    [Entity(FieldName = "Format String")]
    public virtual string FormatString { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="GridColumn"/> is hidden.
    /// </summary>
    /// <value><c>true</c> if hidden; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Hidden")]
    public virtual bool Hidden { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [row ID].
    /// </summary>
    /// <value><c>true</c> if [row ID]; otherwise, <c>false</c>.</value>
    [Entity(FieldName = "Row ID")]
    public virtual bool RowID { get; set; }

    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(GridColumn left, GridColumn right)
    {
      return Equals(left, right);
    }

    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(GridColumn left, GridColumn right)
    {
      return !Equals(left, right);
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>    
    public override bool Equals(object obj)
    {
      var comparer = new PropertyComparer();
      return comparer.Equals(this, obj);           
    }

    /// <summary>
    /// Serves as a hash function for a particular type. 
    /// </summary>
    /// <returns>
    /// A hash code for the current <see cref="T:System.Object"/>.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public override int GetHashCode()
    {
      unchecked
      {
        return ((this.FieldName != null ? this.FieldName.GetHashCode() : 0) * 397) ^ (this.Header != null ? this.Header.GetHashCode() : 0);
      }
    }    
  }
}