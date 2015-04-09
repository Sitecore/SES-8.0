// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductStock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The product stock class.
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
  using Sitecore.Ecommerce.Data;

  /// <summary>
  /// The product stock class.
  /// </summary>
  [Entity(TemplateId = "{DDF12595-B4C7-45F3-A696-7F9F88C99951}")]
  public class ProductStock : DomainModel.Products.ProductStock
  {
    /// <summary>
    /// Gets or sets the stock.
    /// </summary>
    /// <value>The stock.</value>
    [Entity(FieldName = "Stock")]
    public override long Stock
    {
      get { return base.Stock; }
      set { base.Stock = value; }
    }

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The product category code.</value>
    [Entity(FieldName = "Product Code")]
    public override string Code
    {
      get { return base.Code; }
      set { base.Code = value; }
    }
  }
}