// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartManager.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//    The shopping cart manager.
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

namespace Sitecore.Ecommerce.Carts
{
  using System.Linq;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Currencies;
  using DomainModel.Prices;
  using DomainModel.Products;

  /// <summary>
  /// The shopping cart manager.
  /// </summary>
  public class ShoppingCartManager : IShoppingCartManager
  {
    /// <summary>
    /// The shopping cart instance.
    /// </summary>
    private readonly DomainModel.Carts.ShoppingCart shoppingCart;

    /// <summary>
    /// The product price manager instance.
    /// </summary>
    private readonly IProductPriceManager productPriceManager;

    /// <summary>
    /// The product repository instance.
    /// </summary>
    private readonly IProductRepository productRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShoppingCartManager"/> class.
    /// </summary>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="productPriceManager">The product price manager.</param>
    public ShoppingCartManager(IProductRepository productRepository, IProductPriceManager productPriceManager)
    {
      this.shoppingCart = Context.Entity.GetInstance<DomainModel.Carts.ShoppingCart>();
      this.productPriceManager = productPriceManager;
      this.productRepository = productRepository;
    }

    /// <summary>
    /// Adds the product to shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    public virtual void AddProduct(string productCode)
    {
      Assert.ArgumentNotNullOrEmpty(productCode, "productCode");

      this.AddProduct(productCode, 1);
    }

    /// <summary>
    /// Adds to shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="quantity">The quantity.</param>
    public virtual void AddProduct(string productCode, uint quantity)
    {
      Assert.ArgumentNotNullOrEmpty(productCode, "productCode");
      if (quantity == 0)
      {
        return;
      }

      DomainModel.Carts.ShoppingCartLine existingShoppingCartLine = this.shoppingCart.ShoppingCartLines.FirstOrDefault(p => p.Product.Code.Equals(productCode));

      if (existingShoppingCartLine != null)
      {
        existingShoppingCartLine.Quantity += quantity;
        existingShoppingCartLine.Totals = this.productPriceManager.GetProductTotals<Totals, ProductBaseData, Currency>(existingShoppingCartLine.Product, this.shoppingCart.Currency, existingShoppingCartLine.Quantity);
      }
      else
      {
        existingShoppingCartLine = Context.Entity.Resolve<DomainModel.Carts.ShoppingCartLine>();

        ProductBaseData product = this.productRepository.Get<ProductBaseData>(productCode);
        Assert.IsNotNull(product, "Unable to add product \"{0}\" to the shopping cart. The product is not found.", productCode);

        existingShoppingCartLine.Product = product;
        existingShoppingCartLine.Quantity = quantity;

        existingShoppingCartLine.Totals = this.productPriceManager.GetProductTotals<Totals, ProductBaseData, Currency>(product, this.shoppingCart.Currency, existingShoppingCartLine.Quantity);
        
        this.shoppingCart.ShoppingCartLines.Add(existingShoppingCartLine);
      }

      Context.Entity.SetInstance(this.shoppingCart);
    }

    /// <summary>
    /// Updates the product quantity.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="quantity">The quantity.</param>
    public virtual void UpdateProductQuantity(string productCode, uint quantity)
    {
      Assert.ArgumentNotNullOrEmpty(productCode, "productCode");

      ProductLine existingShoppingCartLine = this.shoppingCart.ShoppingCartLines.FirstOrDefault(p => p.Product.Code.Equals(productCode));

      if (existingShoppingCartLine == null)
      {
        return;
      }

      if (existingShoppingCartLine.Quantity < quantity)
      {
        this.AddProduct(productCode, quantity - existingShoppingCartLine.Quantity);
      }
      else if (existingShoppingCartLine.Quantity > quantity)
      {
        this.RemoveProduct(productCode, existingShoppingCartLine.Quantity - quantity);
      }
    }

    /// <summary>
    /// Removes the product from shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    public virtual void RemoveProduct(string productCode)
    {
      Assert.ArgumentNotNullOrEmpty(productCode, "productCode");

      this.RemoveProduct(productCode, 1);
    }

    /// <summary>
    /// Removes the product line from shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    public virtual void RemoveProductLine(string productCode)
    {
      Assert.ArgumentNotNullOrEmpty(productCode, "productCode");

      DomainModel.Carts.ShoppingCartLine existingShoppingCartLine = this.shoppingCart.ShoppingCartLines.FirstOrDefault(p => p.Product.Code.Equals(productCode));

      if (existingShoppingCartLine == null)
      {
        return;
      }

      this.shoppingCart.ShoppingCartLines.Remove(existingShoppingCartLine);

      Context.Entity.SetInstance(this.shoppingCart);
    }

    /// <summary>
    /// Removes the product.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="quantity">The quantity.</param>
    protected virtual void RemoveProduct(string productCode, uint quantity)
    {
      Assert.ArgumentNotNullOrEmpty(productCode, "productCode");

      ProductLine existingProductLine = this.shoppingCart.ShoppingCartLines.FirstOrDefault(p => p.Product.Code.Equals(productCode));

      if (existingProductLine == null)
      {
        return;
      }

      if (existingProductLine.Quantity <= quantity)
      {
        this.RemoveProductLine(productCode);
      }
      else if (quantity > 0)
      {
        existingProductLine.Quantity -= quantity;
      }

      existingProductLine.Totals = this.productPriceManager.GetProductTotals<Totals, ProductBaseData, Currency>(existingProductLine.Product, this.shoppingCart.Currency, existingProductLine.Quantity);

      Context.Entity.SetInstance(this.shoppingCart);
    }
  }
}