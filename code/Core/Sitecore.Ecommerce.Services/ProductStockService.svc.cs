// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductStockService.svc.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Implements product stock service contract.
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
  using Sitecore.Ecommerce.DomainModel.Products;
  using Sitecore.Sites;

  /// <summary>
  /// Implements product stock service contract.
  /// </summary>
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  public class ProductStockService : IProductStockService
  {
    /// <summary>
    /// Gets the specified product stock.
    /// </summary>
    /// <param name="stockInfo">The stock info.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The product stock.</returns>
    public ProductStock Get(ProductStockInfo stockInfo, ServiceClientArgs args)
    {
      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        IProductStockManager stockManager = Context.Entity.Resolve<IProductStockManager>();

        return stockManager.GetStock(stockInfo);
      }
    }

    /// <summary>
    /// Updates the specified product stock.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="newAmount">The new amount.</param>
    /// <param name="args">The arguments.</param>
    public void Update(string code, long newAmount, ServiceClientArgs args)
    {
      ProductStockInfo stockInfo = new ProductStockInfo { ProductCode = code };

      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        IProductStockManager stockManager = Context.Entity.Resolve<IProductStockManager>();

        stockManager.Update(stockInfo, newAmount);
      }
    }
  }
}