// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductPriceService.svc.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Implements product price service contract.
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

namespace Sitecore.Ecommerce.Services
{
  using System.ServiceModel;
  using System.ServiceModel.Activation;
  using Data;
  using Diagnostics;
  using DomainModel.Products;

  using Products;

  using Sitecore.Data;
  using Sites;

  /// <summary>
  /// Implements product price service contract.
  /// </summary>
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  public class ProductPriceService : IProductPriceService
  {
    /// <summary>
    /// Gets the product price matrix.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The product price matrix.</returns>
    public string GetPriceMatrix(string code, ServiceClientArgs args)
    {
      Assert.ArgumentNotNullOrEmpty(code, "code");
      Assert.ArgumentNotNull(args, "args");

      SiteContext site = Utils.GetExistingSiteContext(args);
      Database contentDatabase = site.ContentDatabase;
      Assert.IsNotNull(contentDatabase, "Unable to resolve content database.");

      using (new SiteContextSwitcher(site))
      {
        using (new SiteIndependentDatabaseSwitcher(contentDatabase))
        {
          IProductRepository productRepository = Context.Entity.Resolve<IProductRepository>();

          var repository = productRepository as ProductRepository;
          if (repository != null)
          {
            repository.Database = contentDatabase;
          }

          ProductPriceBaseData productPrice = productRepository.Get<ProductPriceBaseData>(code);

          return productPrice.PriceMatrix;
        }
      }
    }
  }
}