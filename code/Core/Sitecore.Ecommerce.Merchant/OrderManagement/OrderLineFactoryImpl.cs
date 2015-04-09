// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineFactoryImpl.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineFactoryImpl type.
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

namespace Sitecore.Ecommerce.Merchant.OrderManagement
{
  using System.Linq;
  using Common;
  using Diagnostics;
  using DomainModel.Addresses;
  using DomainModel.Currencies;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Prices;
  using Products;

  /// <summary>
  /// Defines the order line factory class.
  /// </summary>
  public class OrderLineFactoryImpl : OrderLineFactory
  {
    /// <summary>
    /// The ProductBaseData.
    /// </summary>
    private Product product;

    /// <summary>
    /// The Totals.
    /// </summary>
    private DomainModel.Prices.Totals totals;

    /// <summary>
    /// Current currency.
    /// </summary>
    private Currency currency;

    /// <summary>
    /// Current vat.
    /// </summary>
    private decimal vat;

    /// <summary>
    /// Indicates that the data should be resolved.
    /// </summary>
    private bool needResolvePreConditions = true;

    /// <summary>
    /// Gets the VAT.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="productCode">The product code.</param>
    /// <returns>
    /// The VAT.
    /// </returns>
    public override decimal GetVat([NotNull] Order order, [NotNull] string productCode)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(productCode, "productCode");

      if (this.needResolvePreConditions)
      {
        this.ResolvePreConditions(order, productCode);
      }

      return this.vat;
    }

    /// <summary>
    /// Gets the order line to add.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="productCode">The product Code.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns>The order line.</returns>
    [CanBeNull]
    public override OrderLine CreateOrderLineFromOrder([NotNull] Order order, [NotNull] string productCode, long quantity)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(productCode, "productCode");

      Assert.IsTrue(productCode != string.Empty, "Product code must not be empty");
      Assert.IsTrue(quantity > 0, "Quantity should be greater than zero");
  
      this.ResolvePreConditions(order, productCode);
      OrderLine result = new OrderLine
      {
        Order = order,
        LineItem = this.CreateLineItemFromOrder(order, productCode, quantity)
      };

      return result;
    }

    /// <summary>
    /// Gets the line item to add.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="productCode">The product Code.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns>
    /// The line item.
    /// </returns>
    [CanBeNull]
    public override LineItem CreateLineItemFromOrder([NotNull] Order order, [NotNull] string productCode, long quantity)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(productCode, "productCode");

      Assert.IsTrue(productCode != string.Empty, "Product code must not be empty");
      Assert.IsTrue(quantity > 0, "Quantity should be greater than zero");

      if (this.needResolvePreConditions)
      {
        this.ResolvePreConditions(order, productCode);
      }

      LineItem lineItem = new LineItem
      {
        Quantity = quantity,
        Price = new Price(new Amount(this.totals.PriceExVat, order.PricingCurrencyCode), quantity),
        Item = new Item
        {
          PackQuantity = 1,
          PackSizeNumeric = 1,
          Code = this.product.Code,
          Name = this.product.Name,
          Description = this.product.ShortDescription,
          AdditionalInformation = this.product.ImageUrl,
          Keyword = string.Empty
        },
        OrderedShipment = new OrderedShipment(),
        TotalTaxAmount = new Amount(this.vat * this.totals.PriceExVat * quantity, order.PricingCurrencyCode),
      };

      return lineItem;
    }

    /// <summary>
    /// Resolves the preconditions: product and totals.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="productCode">The product code.</param>
    protected internal virtual void ResolvePreConditions([NotNull] Order order, [NotNull] string productCode)
    {
      Assert.ArgumentNotNull(order, "order");
      Assert.ArgumentNotNull(productCode, "productCode");

      this.needResolvePreConditions = false;

      this.product = this.ProductRepository.Get<Product>(productCode);
      Assert.IsNotNull(this.product, "Product cannot be resolved");
      this.currency = this.CurrencyProvider.Get(order.PricingCurrencyCode);
      Assert.IsNotNull(this.currency, "Currency cannot be resolved");
      this.totals = this.ProductPriceManager.GetProductTotals<DomainModel.Prices.Totals, Product, Currency>(this.product, this.currency);
      Assert.IsNotNull(this.totals, "Totals cannot be resolved");

      VatRegion vatRegion = this.GetVatRegion(order);
      this.vat = vatRegion != null ? ((ProductPriceManager)this.ProductPriceManager).GetVat(this.product, vatRegion) : this.totals.VAT;
    }

    /// <summary>
    /// Gets the vat region.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// The vat region.
    /// </returns>
    [CanBeNull]
    protected internal virtual VatRegion GetVatRegion([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      VatRegion result = null;

      if (order.TaxTotal != null)
      {
        if (order.TaxTotal.TaxSubtotal.Count > 0)
        {
          TaxSubTotal taxSubTotal = order.TaxTotal.TaxSubtotal.FirstOrDefault();
          Assert.IsNotNull(taxSubTotal, "taxSubTotal cannot be null.");
          TaxCategory taxCategory = taxSubTotal.TaxCategory;
          Assert.IsNotNull(taxCategory, "taxCategory cannot be null.");
          string code = taxCategory.Name;

          if (string.IsNullOrEmpty(code))
          {
            return null;
          }

          result = this.GetVatRegion(code);
        }
      }

      return result;
    }

    /// <summary>
    /// Returns the country.
    /// </summary>
    /// <param name="code">The country code.</param>
    /// <returns>
    /// Country instance.
    /// </returns>
    [NotNull]
    protected virtual VatRegion GetVatRegion([NotNull] string code)
    {
      Assert.ArgumentNotNull(code, "code");
      Assert.IsNotNull(this.VatRegionProvider, "VatRegionProvider must be set.");

      return this.VatRegionProvider.Get(code);
    }
  }
}