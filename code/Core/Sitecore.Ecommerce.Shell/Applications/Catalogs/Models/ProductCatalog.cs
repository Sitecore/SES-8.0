// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductCatalog.cs"  company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ProductCatalog type.
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
  using System.Collections.Generic;
  using Sitecore.Data.Items;

  /// <summary>
  /// Defines the ProductCatalog type.
  /// </summary>
  public class ProductCatalog : Catalog
  {
    /// <summary>
    /// Gets the search methods.
    /// </summary>
    /// <returns>returns list of catalog search methods</returns>
    public virtual List<SearchMethod> GetSearchMethods()
    {
      List<SearchMethod> methods = new List<SearchMethod>();

      if (this.CatalogItem != null)
      {
        Item methodsItem = this.CatalogItem.Database.GetItem("/sitecore/system/Modules/Ecommerce/System/Product Selection Method");
        if (methodsItem != null)
        {
          foreach (Item methodItem in methodsItem.Children)
          {
            SearchMethod method = this.DataMapper.GetEntity<SearchMethod>(methodItem);
            method.ID = methodItem.ID;
            
            methods.Add(method);
          }
        }
      }

      return methods;
    }
  }
}