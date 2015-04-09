// -------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartEvents.cs" company="Sitecore Corporation">
//  Copyright (c) Sitecore Corporation 1999-2015 
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

namespace Sitecore.Ecommerce.Analytics.Components.PageEvents
{
    using System.Globalization;
    using Diagnostics;

    using Sitecore.Analytics;
    using Sitecore.Analytics.Data;

    using Text;
    using Utils;

    /// <summary>
    /// The shopping carts analytics extension.
    /// </summary>
    public class ShoppingCartEvents
    {
        /// <summary>
        /// Add to shopping cart event.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="price">The price.</param>
        public virtual void AddToShoppingCart(string productCode, string productName, uint quantity, decimal price)
        {
            Assert.ArgumentNotNull(productName, "productName");
            Assert.ArgumentNotNull(productCode, "productCode");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventAddToShoppingCart);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventAddToShoppingCart;
            }

            string text = description.FormatWith(new { Quantity = quantity, ProductName = productName, Price = price });
            ListString data = new ListString { productName, quantity.ToString(CultureInfo.InvariantCulture), price.ToString(CultureInfo.InvariantCulture), productCode.ToString(CultureInfo.InvariantCulture) };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventAddToShoppingCart) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Shopping Cart Emptied event.
        /// </summary>
        /// <param name="shoppingCartContent">Content of the shopping cart.</param>
        /// <param name="itemsInShoppingCart">The items in shopping cart.</param>
        public virtual void ShoppingCartEmptied(string shoppingCartContent, uint itemsInShoppingCart)
        {
            Assert.ArgumentNotNull(shoppingCartContent, "shoppingCartContent");
            Assert.ArgumentNotNull(itemsInShoppingCart, "itemsInShoppingCart");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventShoppingCartEmptied);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventShoppingCartEmptied;
            }

            string text = description.FormatWith(new { Amount = itemsInShoppingCart, ShoppingCartContent = shoppingCartContent });
            ListString data = new ListString { itemsInShoppingCart.ToString(CultureInfo.InvariantCulture), shoppingCartContent };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventShoppingCartEmptied) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Continue Shopping event.
        /// </summary>
        public virtual void ContinueShopping()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventShoppingCartContinueShopping);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventShoppingCartContinueShopping, description);
            }
        }

        /// <summary>
        /// Shopping cart updated.
        /// </summary>
        public virtual void ShoppingCartUpdated()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventShoppingCartUpdated);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventShoppingCartUpdated, description);
            }
        }

        /// <summary>
        /// Go to shopping cart.
        /// </summary>
        public virtual void GoToShoppingCart()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventGoToShoppingCart);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventGoToShoppingCart, description);
            }
        }

        /// <summary>
        /// Shopping cart item removed.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="amount">The amount.</param>
        public virtual void ShoppingCartItemRemoved(string productCode, string productName, uint amount)
        {
            Assert.ArgumentNotNull(productCode, "shoppingCartContent");
            Assert.ArgumentNotNull(productName, "itemsInShoppingCart");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventShoppingCartItemRemoved);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventShoppingCartItemRemoved;
            }

            string text = description.FormatWith(new { ProductName = productName, Amount = amount });
            ListString data = new ListString { productCode, productName, amount.ToString(CultureInfo.InvariantCulture) };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventShoppingCartItemRemoved) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Shoppings the cart item updated.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="amount">The products amount.</param>
        public virtual void ShoppingCartItemUpdated(string productCode, string productName, uint amount)
        {
            Assert.ArgumentNotNull(productCode, "shoppingCartContent");
            Assert.ArgumentNotNull(productName, "itemsInShoppingCart");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventShoppingCartItemUpdated);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventShoppingCartItemUpdated;
            }

            string text = description.FormatWith(new { ProductName = productName, Amount = amount });
            ListString data = new ListString { productCode, productName, amount.ToString(CultureInfo.InvariantCulture) };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventShoppingCartItemUpdated) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Product removed.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="amount">The amount.</param>
        public virtual void ProductRemoved(string productCode, string productName, uint amount)
        {
            Assert.ArgumentNotNull(productCode, "shoppingCartContent");
            Assert.ArgumentNotNull(productName, "itemsInShoppingCart");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventProductRemoved);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventProductRemoved;
            }

            string text = description.FormatWith(new { ProductName = productName, Amount = amount });
            ListString data = new ListString { productCode, productName, amount.ToString(CultureInfo.InvariantCulture) };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventProductRemoved) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Shopping cart viewed.
        /// </summary>
        public virtual void ShoppingCartViewed()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventShoppingCartViewed);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventShoppingCartViewed, description);
            }
        }
    }
}