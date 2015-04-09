// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemBasedOrderIDGenerator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the item based order ID generator class.
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
  using Diagnostics;
  using SecurityModel;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Ecommerce.DomainModel.Orders;

  /// <summary>
  /// Defines the item based order ID generator class.
  /// </summary>
  public class ItemBasedOrderIDGenerator : OrderIDGenerator
  {
    /// <summary>
    /// The last order id constant.
    /// </summary>
    private const string LastOrderId = "Latest Order ID";

    /// <summary>
    /// Is used for locking
    /// </summary>
    private static readonly object LockObject = new object();

    /// <summary>
    /// The shop context
    /// </summary>
    private readonly ShopContext shopContext;

    /// <summary>
    /// The generation strategy.
    /// </summary>
    private readonly OrderIDGenerationStrategy generationStrategy;

    /// <summary>
    /// The orders item.
    /// </summary>
    private Item ordersItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemBasedOrderIDGenerator" /> class.
    /// </summary>
    /// <param name="shopContext">The shop context.</param>
    /// <param name="generationStrategy">The generation strategy.</param>
    public ItemBasedOrderIDGenerator(ShopContext shopContext, OrderIDGenerationStrategy generationStrategy)
    {
      Assert.IsNotNull(shopContext, "Unable to generate order id. ShopContext cannot be null.");
      Assert.IsNotNull(generationStrategy, "Unable to generate order id. Order id generation strategy cannot be null.");

      this.shopContext = shopContext;
      this.generationStrategy = generationStrategy;
    }

    /// <summary>
    /// Gets the orders item.
    /// </summary>
    [NotNull]
    private Item OrdersItem
    {
      get
      {
        if (this.ordersItem == null)
        {
          Assert.IsNotNull(this.shopContext, "Unable to get order number. ShopContext cannot be null.");
          Assert.IsNotNull(this.shopContext.BusinessCatalogSettings, "Unable to get order number. BusinessCatalogSettings cannot be null.");

          Database database = this.shopContext.InnerSite.ContentDatabase;
          Assert.IsNotNull(database, "Unable to generate order number. Content database cannot be null.");

          this.ordersItem = database.GetItem(this.shopContext.BusinessCatalogSettings.OrdersLink);
        }

        Assert.IsNotNull(this.ordersItem, "Unable to get order number. OrdersItem cannot be null.");

        return this.ordersItem;
      }
    }

    /// <summary>
    /// Generates the order ID.
    /// </summary>
    /// <returns>
    /// The string.
    /// </returns>
    [NotNull]
    public override string Generate()
    {
      lock (LockObject)
      {
        string previousId = this.GetPreviousId();

        string newId = this.generationStrategy.Generate(previousId);

        this.SaveLastOrderId(newId);

        return newId;
      }
    }

    /// <summary>
    /// Gets the previous id.
    /// </summary>
    /// <returns>
    /// The previous id.
    /// </returns>
    protected virtual string GetPreviousId()
    {
      return this.OrdersItem[LastOrderId];
    }

    /// <summary>
    /// Saves the last order id.
    /// </summary>
    /// <param name="lastOrderId">The last order id.</param>
    protected virtual void SaveLastOrderId([NotNull]string lastOrderId)
    {
      Debug.ArgumentNotNull(lastOrderId, "lastOrderId");

      using (new SecurityDisabler())
      {
        using (new EditContext(this.OrdersItem))
        {
          this.OrdersItem[LastOrderId] = lastOrderId;
        }
      }
    }
  }
}