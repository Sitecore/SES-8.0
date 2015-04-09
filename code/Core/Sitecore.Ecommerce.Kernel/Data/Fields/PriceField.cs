// -------------------------------------------------------------------------------------------
// <copyright file="PriceField.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Data.Fields
{
  using PriceMatrix;
  using Sitecore.Data.Fields;
 
  /// <summary>
  /// The price field.
  /// </summary>
  public class PriceField : CustomField
  {
    /// <summary>
    /// The current field.
    /// </summary>
    private readonly Field field;

    /// <summary>
    /// Initializes a new instance of the <see cref="PriceField"/> class.
    /// </summary>
    /// <param name="field">
    /// The field.
    /// </param>
    private PriceField(Field field) : base(field)
    {
      this.field = field;
    }

    /// <summary>
    /// Gets the price matrix for this field
    /// </summary>
    /// <value>The price matrix.</value>
    public PriceMatrix PriceMatrix
    {
      get
      {
        return PriceMatrix.Load(this.field.Value);
      }
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="Sitecore.Data.Fields.Field"/> to <see cref="PriceField"/>.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator PriceField(Field field)
    {
      var priceField = new PriceField(field);
      return priceField;
    }
  }
}