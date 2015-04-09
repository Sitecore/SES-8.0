// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemProductFactory.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the item product factory class.
//   Creates product instance according to product type.
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
  using System.Collections.ObjectModel;
  using Diagnostics;
  using DomainModel.Data;
  using DomainModel.Products;
  using Sitecore.Data.Items;
  using Sitecore.Ecommerce.Unity;

  /// <summary>
  /// Defines the item product factory class.
  /// Creates product instance according to product type. 
  /// </summary>
  public class ItemProductFactory : ProductFactory
  {
    /// <summary>
    /// Gets or sets the shop context.
    /// </summary>
    /// <value>
    /// The shop context.
    /// </value>
    [CanBeNull]
    public ShopContext ShopContext { get; set; }

    /// <summary>
    /// Creates product of the specified type.
    /// </summary>
    /// <param name="template">The product template.</param>
    /// <returns>
    /// The product base data.
    /// </returns>
    [NotNull]
    public override ProductBaseData Create([NotNull]string template)
    {
      Assert.ArgumentNotNull(template, "template");

      Collection<string> spec = new Collection<string>();

      Assert.IsNotNull(this.ShopContext, "Unable to create a product. Context shop cannot be null.");
      Assert.IsNotNull(this.ShopContext.Database, "Unable to create a product. Context shop database cannot be null.");

      TemplateItem templateItem = this.ShopContext.Database.GetItem(template);
      Assert.IsNotNull(templateItem, "Product template is not found.");

      this.FillSpecificationKeys(templateItem, ref spec);

      ProductBaseData product = Context.Entity.SmartResolve<ProductBaseData>(template);

      product.Specifications = new ProductSpecification(spec);
      ((ITemplatedEntity)product).Template = template;

      return product;
    }

    /// <summary>
    /// Fills the specification keys.
    /// </summary>
    /// <param name="template">The template.</param>
    /// <param name="spec">The spec.</param>
    private void FillSpecificationKeys([NotNull] TemplateItem template, [NotNull] ref Collection<string> spec)
    {
      Assert.ArgumentNotNull(template, "template");
      Assert.ArgumentNotNull(spec, "spec");

      TemplateSectionItem section = template.GetSection("Specification");
      if (section == null)
      {
        return;
      }

      foreach (TemplateFieldItem field in section.GetFields())
      {
        if (!spec.Contains(field.Name))
        {
          spec.Add(field.Name);
        }
      }

      foreach (TemplateItem baseTemplate in template.BaseTemplates)
      {
        this.FillSpecificationKeys(baseTemplate, ref spec);
      }
    }
  }
}