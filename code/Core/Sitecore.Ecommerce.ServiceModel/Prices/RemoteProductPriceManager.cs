// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoteProductPriceManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The remote product price manager. Provides product price management through WCF service.
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

namespace Sitecore.Ecommerce.ServiceModel.Prices
{
  using Diagnostics;
  using DomainModel.Prices;
  using Ecommerce.Prices;
  using PriceMatrix;
  using ProductPriceService;
  using Services;

  /// <summary>
  /// The remote product price manager. Provides product price management through WCF service.
  /// </summary>
  public class RemoteProductPriceManager : ProductPriceManager
  {
    /// <summary>
    /// Defines ServiceClientArgs factory.
    /// </summary>
    private readonly ServiceClientArgsFactory serviceClientArgsFactory;    

    /// <summary>
    /// The price matrix.
    /// </summary>
    private string priceMatrix;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteProductPriceManager" /> class.
    /// </summary>
    /// <param name="serviceClientArgsFactory">The service client args factory.</param>
    /// <param name="priceCalculatorFactory">The price calculator factory.</param>
    /// <param name="shopContext">The shop context.</param>
    public RemoteProductPriceManager(ServiceClientArgsFactory serviceClientArgsFactory, PriceCalculatorFactory priceCalculatorFactory, ShopContext shopContext) : base(priceCalculatorFactory, shopContext)
    {
      Assert.ArgumentNotNull(serviceClientArgsFactory, "serviceClientArgsFactory");
      this.serviceClientArgsFactory = serviceClientArgsFactory;
    }

    /// <summary>
    /// Gets the product totals.
    /// </summary>
    /// <typeparam name="TTotals">The type of the totals.</typeparam>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <typeparam name="TCurrency">The type of the currency.</typeparam>
    /// <param name="product">The product.</param>
    /// <param name="currency">The currency.</param>
    /// <returns>The totals.</returns>
    public override TTotals GetProductTotals<TTotals, TProduct, TCurrency>(TProduct product, TCurrency currency)
    {
      return this.GetProductTotals<TTotals, TProduct, TCurrency>(product, currency, 1);
    }

    /// <summary>
    /// Gets the product totals.
    /// </summary>
    /// <typeparam name="TTotals">The type of the totals.</typeparam>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <typeparam name="TCurrency">The type of the currency.</typeparam>
    /// <param name="product">The product.</param>
    /// <param name="currency">The currency.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns>The totals.</returns>
    public override TTotals GetProductTotals<TTotals, TProduct, TCurrency>(TProduct product, TCurrency currency, uint quantity)
    {
      Assert.ArgumentNotNull(product, "product");
      Assert.ArgumentNotNull(currency, "currency");

      Assert.IsNotNull(product.Code, "Product code is not specified.");

      ServiceClientArgs args = this.serviceClientArgsFactory.GetServiceClientArgs();

      using (ProductPriceServiceClient client = new ProductPriceServiceClient())
      {
        this.priceMatrix = client.GetPriceMatrix(product.Code, args);

        TTotals totals = base.GetProductTotals<TTotals, TProduct, TCurrency>(product, currency, quantity);

        return totals;
      }
    }

    /// <summary>
    /// Gets the price matrix price.
    /// </summary>
    /// <typeparam name="TProduct">The type of the product.</typeparam>
    /// <param name="product">The product.</param>
    /// <param name="priceMatrixName">Name of the price matrix.</param>
    /// <returns>The price.</returns>
    protected override decimal GetPriceMatrixPrice<TProduct>(TProduct product, string priceMatrixName)
    {
      PriceMatrix pm = PriceMatrix.Load(this.priceMatrix);

      string query = string.Format("./Shop/{0}", priceMatrixName);
      IPriceMatrixItem priceMatrixItem = pm.SelectSingleItem(query);
      CategoryItem categoryItem = priceMatrixItem as CategoryItem;

      if (categoryItem == null)
      {
        return decimal.Zero;
      }

      string price = categoryItem.Amount;

      decimal priceValue;
      return !decimal.TryParse(price, out priceValue) ? 0 : priceValue;
    }
  }
}