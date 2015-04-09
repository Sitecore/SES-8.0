// -------------------------------------------------------------------------------------------
// <copyright file="ProductUriLine.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Catalogs
{
  /// <summary>
  /// The Product Url Line.
  /// </summary>
  public struct ProductUriLine
  {
    /// <summary>
    /// Gets or sets the product item URI.
    /// </summary>
    /// <value>The product item URI.</value>
    public string ProductItemUri { get; set; }

    /// <summary>
    /// Gets or sets the product catalog item URI.
    /// </summary>
    /// <value>The product catalog item URI.</value>
    public string ProductCatalogItemUri { get; set; }

    /// <summary>
    /// Gets or sets the product presentation item URI.
    /// </summary>
    /// <value>The product presentation item URI.</value>
    public string ProductPresentationItemUri { get; set; }

    /// <summary>
    /// Gets or sets the display products mode.
    /// </summary>
    /// <value>The display products mode.</value>
    public string DisplayProductsMode { get; set; }
  }
}