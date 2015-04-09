// -------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartWebHelper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.Examples
{
  using System;
  using System.Linq;
  using Analytics.Components;
  using Diagnostics;
  using DomainModel.Carts;
  using Sitecore.Analytics;

  /// <summary>
  /// Shopping Cart Web Helper
  /// </summary>
  public class ShoppingCartWebHelper
  {
    /// <summary>
    /// Adds to shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="quantity">The quantity.</param>    
    public static void AddToShoppingCart(string productCode, string quantity)
    {
      Assert.ArgumentNotNullOrEmpty(productCode, "productCode");
      Assert.ArgumentNotNullOrEmpty(quantity, "quantity");

      IShoppingCartManager shoppingCartManager = Context.Entity.Resolve<IShoppingCartManager>();

      uint q;
      if (string.IsNullOrEmpty(quantity) || !uint.TryParse(quantity, out q))
      {
        shoppingCartManager.AddProduct(productCode, 1);
      }
      else
      {
        shoppingCartManager.AddProduct(productCode, q);
      }

      ShoppingCart shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
      ShoppingCartLine existingShoppingCartLine = shoppingCart.ShoppingCartLines.FirstOrDefault(p => p.Product.Code.Equals(productCode));

      try
      {
        Tracker.StartTracking();
        AnalyticsUtil.AddToShoppingCart(existingShoppingCartLine.Product.Code, existingShoppingCartLine.Product.Title, 1, existingShoppingCartLine.Totals.PriceExVat);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Deletes from shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    public static void DeleteFromShoppingCart(string productCode)
    {
      Assert.ArgumentNotNullOrEmpty(productCode, "productCode");

      ShoppingCart shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
      ShoppingCartLine existingShoppingCartLine = shoppingCart.ShoppingCartLines.FirstOrDefault(p => p.Product.Code.Equals(productCode));

      try
      {
        if (existingShoppingCartLine != null)
        {
          Tracker.StartTracking();
          AnalyticsUtil.ShoppingCartProductRemoved(existingShoppingCartLine.Product.Code, existingShoppingCartLine.Product.Title, existingShoppingCartLine.Quantity);
        }
      }
      catch (Exception ex)
      {
        LogException(ex);
      }

      IShoppingCartManager shoppingCartManager = Context.Entity.Resolve<IShoppingCartManager>();
      shoppingCartManager.RemoveProduct(productCode);
    }

    /// <summary>
    /// Deletes the product line from shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    public static void DeleteProductLineFromShoppingCart(string productCode)
    {
      Assert.ArgumentNotNullOrEmpty(productCode, "productCode");

      ShoppingCart shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
      ShoppingCartLine existingShoppingCartLine = shoppingCart.ShoppingCartLines.FirstOrDefault(p => p.Product.Code.Equals(productCode));

      if (existingShoppingCartLine != null)
      {
        try
        {
          Tracker.StartTracking();
          AnalyticsUtil.ShoppingCartItemRemoved(existingShoppingCartLine.Product.Code, existingShoppingCartLine.Product.Title, existingShoppingCartLine.Quantity);
        }
        catch (Exception ex)
        {
          LogException(ex);
        }
      }

      IShoppingCartManager shoppingCartManager = Context.Entity.Resolve<IShoppingCartManager>();
      shoppingCartManager.RemoveProductLine(productCode);
    }

    /// <summary>
    /// Logs the exception.
    /// </summary>
    /// <param name="ex">The exception.</param>
    private static void LogException(Exception ex)
    {
      Log.Error("Analytics error:", ex);
    }
  }
}