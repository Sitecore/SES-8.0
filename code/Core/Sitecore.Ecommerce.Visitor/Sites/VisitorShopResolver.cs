// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisitorShopResolver.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the VisitorShopResolver type.
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

namespace Sitecore.Ecommerce.Visitor.Sites
{
  using Diagnostics;
  using DomainModel.Configurations;
  using Sitecore.Sites;

  /// <summary>
  /// Defines the visitor shop resolver class.
  /// </summary>
  public class VisitorShopResolver
  {
    /// <summary>
    /// Gets the shop context.
    /// </summary>
    /// <param name="siteContext">The site context.</param>
    /// <returns>
    /// The shop context.
    /// </returns>
    [CanBeNull]
    public virtual ShopContext GetShopContext([NotNull] SiteContext siteContext)
    {
      Assert.ArgumentNotNull(siteContext, "siteContext");

      if (siteContext.Properties["EcommerceSiteSettings"] == null)
      {
        return null;
      }

      BusinessCatalogSettings businessSettings = Context.Entity.GetConfiguration<BusinessCatalogSettings>();
      GeneralSettings generalSettings = Context.Entity.GetConfiguration<GeneralSettings>();

      ShopContext shopContext = new ShopContext(siteContext)
      {
        BusinessCatalogSettings = businessSettings,
        GeneralSettings = generalSettings,
        Database = siteContext.Database
      };

      return shopContext;
    }
  }
}