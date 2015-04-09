// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductPrice.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ProductPrice type.
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

namespace Sitecore.Ecommerce.Products
{
  using Data;
  using DomainModel.Data;
  using DomainModel.Products;

  /// <summary>
  /// </summary>
  [Entity(TemplateId = "{93D13834-8A93-4380-A6EA-EF4C36CCD857}")]
  public class ProductPrice : ProductPriceBaseData, IEntity
  {
    /// <summary>
    /// Gets or sets PriceMatrix.
    /// </summary>
    [Entity(FieldName = "Price")]
    public override string PriceMatrix
    {
      get { return base.PriceMatrix; }
      set { base.PriceMatrix = value; }
    }

    /// <summary>
    /// Gets or sets Code.
    /// </summary>
    [Entity(FieldName = "Product Code")]
    public override string Code
    {
      get { return base.Code; }
      set { base.Code = value; }
    }

    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    [Entity(FieldName = "Name")]
    public override string Name
    {
      get { return base.Name; }
      set { base.Name = value; }
    }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public string Alias { get; set; }
  }
}