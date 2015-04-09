// -------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartLine.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.DomainModel.Carts
{
  using System;
  using Products;

  /// <summary>
  /// The order line abstract class.
  /// </summary>
  [Serializable]
  public class ShoppingCartLine : ProductLine
  {
    /// <summary>
    /// Gets or sets the friendly URL.
    /// </summary>
    /// <value>The friendly URL.</value>
    public virtual string FriendlyUrl { get; set; }

    /// <summary>
    /// Gets or sets the image URL.
    /// </summary>
    /// <value>The image URL.</value>
    public virtual string ImageUrl { get; set; }
  }
}