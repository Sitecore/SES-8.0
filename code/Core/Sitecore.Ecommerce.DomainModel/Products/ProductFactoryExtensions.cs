// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductFactoryExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the product factory extensions class.
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
  /// <summary>
  /// Defines the product factory extensions class.
  /// </summary>
  public static class ProductFactoryExtensions
  {
    /// <summary>
    /// Creates the specified template.
    /// </summary>
    /// <typeparam name="T">The product type.</typeparam>
    /// <param name="factory">The factory.</param>
    /// <param name="template">The template.</param>
    /// <returns>
    /// The product.
    /// </returns>
    public static T Create<T>(this ProductFactory factory, string template) where T : ProductBaseData
    {
      return factory.Create(template) as T;
    }
  }
}