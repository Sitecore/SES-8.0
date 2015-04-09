// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineFactory.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineFactory type.
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

namespace Sitecore.Ecommerce.OrderManagement
{
  using DomainModel.Addresses;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using DomainModel.Prices;
  using DomainModel.Products;
  using Orders;

  /// <summary>
  /// Type definition for the essense that can create order lines and line items.
  /// </summary>
  public abstract class OrderLineFactory
  {
    /// <summary>
    /// Gets or sets the product repository.
    /// </summary>
    /// <value>The product repository.</value>
    public virtual IProductRepository ProductRepository { get; set; }

    /// <summary>
    /// Gets or sets the product price manager.
    /// </summary>
    /// <value>The product price manager.</value>
    public virtual IProductPriceManager ProductPriceManager { get; set; }

    /// <summary>
    /// Gets or sets the currency provider.
    /// </summary>
    /// <value>The currency provider.</value>
    public virtual IEntityProvider<Currency> CurrencyProvider { get; set; }

    /// <summary>
    /// Gets or sets the country provider.
    /// </summary>
    /// <value>
    /// The country provider.
    /// </value>
    public virtual IEntityProvider<VatRegion> VatRegionProvider { get; set; }

    /// <summary>
    /// Gets the VAT.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="productCode">The product code.</param>
    /// <returns>
    /// The VAT.
    /// </returns>
    public abstract decimal GetVat(Order order, string productCode);

    /// <summary>
    /// Gets the order line to add.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="productCode">The product Code.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns>The order line.</returns>
    [CanBeNull]
    public abstract OrderLine CreateOrderLineFromOrder(Order order, string productCode, long quantity);

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
    public abstract LineItem CreateLineItemFromOrder(Order order, string productCode, long quantity);
  }
}
