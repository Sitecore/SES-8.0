// -------------------------------------------------------------------------------------------
// <copyright file="XslExtensions.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Analytics.Components.Xsl
{
  using System.Xml.XPath;
  using Catalogs;
  using Diagnostics;
  using Globalization;
  using PageEvents;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Utils;

  /// <summary>
  /// The analitics XslExtensions
  /// </summary>
  public class XslExtensions
  {
    #region Fields

    /// <summary>
    /// The Authentification events
    /// </summary>
    private readonly AuthentificationEvents authentificationEvents;

    /// <summary>
    /// The checkout events instance
    /// </summary>
    private readonly CheckoutEvents checkoutEvents;

    /// <summary>
    /// The navigation events class instance
    /// </summary>
    private readonly NavigationEvents navigationEvents;

    /// <summary>
    /// The shopping cart events instance
    /// </summary>
    private readonly ShoppingCartEvents shoppingCartEvents;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="XslExtensions"/> class.
    /// </summary>
    public XslExtensions()
    {
      this.shoppingCartEvents = new ShoppingCartEvents();
      this.checkoutEvents = new CheckoutEvents();
      this.authentificationEvents = new AuthentificationEvents();
      this.navigationEvents = new NavigationEvents();
    }

    #region Shopping Cart page events

    /// <summary>
    /// Adds to shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="productName">Name of the product.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="price">The price.</param>
    public virtual void AddToShoppingCart(string productCode, string productName, uint quantity, decimal price)
    {
      this.shoppingCartEvents.AddToShoppingCart(productCode, productName, quantity, price);
    }

    /// <summary>
    /// Shoppings the cart emptied.
    /// </summary>
    /// <param name="shoppingCartContent">Content of the shopping cart.</param>
    /// <param name="itemsInShoppingCart">The items in shopping cart.</param>
    public virtual void ShoppingCartEmptied(string shoppingCartContent, uint itemsInShoppingCart)
    {
      this.shoppingCartEvents.ShoppingCartEmptied(shoppingCartContent, itemsInShoppingCart);
    }

    /// <summary>
    /// Shopping cart continue shopping event.
    /// </summary>
    public virtual void ShoppingCartContinueShopping()
    {
      this.shoppingCartEvents.ContinueShopping();
    }

    /// <summary>
    /// Shopping the cart updated.
    /// </summary>
    public virtual void ShoppingCartUpdated()
    {
      this.shoppingCartEvents.ShoppingCartUpdated();
    }

    /// <summary>
    /// Go to shopping cart.
    /// </summary>
    public virtual void GoToShoppingCart()
    {
      this.shoppingCartEvents.GoToShoppingCart();
    }

    /// <summary>
    /// Shoppings the cart item removed.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="productName">Name of the product.</param>
    /// <param name="amount">The amount.</param>
    public virtual void ShoppingCartItemRemoved(string productCode, string productName, uint amount)
    {
      this.shoppingCartEvents.ShoppingCartItemRemoved(productCode, productName, amount);
    }

    /// <summary>
    /// Updates the shopping cart item for analitics.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="productName">Name of the product.</param>
    /// <param name="amount">The amount.</param>
    public virtual void ShoppingCartItemUpdated(string productCode, string productName, uint amount)
    {
      this.shoppingCartEvents.ShoppingCartItemUpdated(productCode, productName, amount);
    }

    /// <summary>
    /// Deletes the shopping cart item for analitics.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="productName">Name of the product.</param>
    /// <param name="amount">The amount.</param>
    public virtual void ShoppingCartProductRemoved(string productCode, string productName, uint amount)
    {
      this.shoppingCartEvents.ProductRemoved(productCode, productName, amount);
    }

    /// <summary>
    /// Shopping cart viewed.
    /// </summary>
    public virtual void ShoppingCartViewed()
    {
      this.shoppingCartEvents.ShoppingCartViewed();
    }

    #endregion

    #region Checkout page events

    /// <summary>
    /// Go to check out.
    /// </summary>
    public virtual void GoToCheckOut()
    {
      this.checkoutEvents.GoToCheckOut();
    }

    /// <summary>
    /// Checkous the delivery next.
    /// </summary>
    /// <param name="deliveryAlternativeOption">The delivery alternative option.</param>
    /// <param name="notificationOption">The notification option.</param>
    /// <param name="notificationText">The notification text.</param>
    public virtual void CheckoutDeliveryNext(string deliveryAlternativeOption, string notificationOption, string notificationText)
    {
      this.checkoutEvents.DeliveryNext(deliveryAlternativeOption, notificationOption, notificationText);
    }

    /// <summary>
    /// Checkouts the delivery next.
    /// </summary>
    /// <param name="deliveryAlternativeOption">The delivery alternative option.</param>
    public virtual void CheckoutDeliveryOptionSelected(string deliveryAlternativeOption)
    {
      this.checkoutEvents.DeliveryOptionSelected(deliveryAlternativeOption);
    }

    /// <summary>
    /// Checkout payment method selected.
    /// </summary>
    /// <param name="optionTitle">The option title.</param>
    /// <param name="optionCode">The option code.</param>
    public virtual void CheckoutPaymentMethodSelected(string optionTitle, string optionCode)
    {
      this.checkoutEvents.PaymentMethodSelected(optionTitle, optionCode);
    }

    /// <summary>
    /// Checkout next.
    /// </summary>
    public virtual void CheckoutNext()
    {
      this.checkoutEvents.Next();
    }

    /// <summary>
    /// Checkout payment next.
    /// </summary>
    public virtual void CheckoutPaymentNext()
    {
      this.checkoutEvents.PaymentNext();
    }

    /// <summary>
    /// Checkouts the notification changed.
    /// </summary>
    /// <param name="deliveryAlternativeOption">The delivery alternative option.</param>
    public virtual void CheckoutNotificationOptionSelected(string deliveryAlternativeOption)
    {
      this.checkoutEvents.NotificationOptionSelected(deliveryAlternativeOption);
    }

    /// <summary>
    /// Checkout Previous.
    /// </summary>
    public virtual void CheckoutPrevious()
    {
      this.checkoutEvents.Previous();
    }

    #endregion

    #region Authentification page events

    /// <summary>
    /// Authentification clicked login button.
    /// </summary>
    public virtual void AuthentificationClickedLoginButton()
    {
      this.authentificationEvents.ClickedLoginButton();
    }

    /// <summary>
    /// Authentification clicked login link.
    /// </summary>
    public virtual void AuthentificationClickedLoginLink()
    {
      this.authentificationEvents.ClickedLoginLink();
    }

    /// <summary>
    /// Authentifications the user logged out.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    public virtual void AuthentificationUserLoggedOut(string userName)
    {
      this.authentificationEvents.UserLoggedOut(userName);
    }

    /// <summary>
    /// Users the login succeeded.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    public virtual void AuthentificationUserLoginSucceeded(string userName)
    {
      this.authentificationEvents.UserLoginSucceded(userName);
    }

    /// <summary>
    /// Users the login failed.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    public virtual void AuthentificationUserLoginFailed(string userName)
    {
      this.authentificationEvents.UserLoginFailed(userName);
    }

    /// <summary>
    /// Account creation failed.
    /// </summary>
    public virtual void AuthentificationAccountCreationFailed()
    {
      this.authentificationEvents.AccountCreationFailed();
    }

    /// <summary>
    /// Account creation succeded.
    /// </summary>
    public virtual void AuthentificationAccountCreated()
    {
      this.authentificationEvents.AccountCreated();
    }

    #endregion

    #region Navigation page events

    /// <summary>
    /// Registers the no parameter event.
    /// </summary>
    /// <param name="tabName">Name of the tab.</param>
    public virtual void NavigationTabSelected(string tabName)
    {
      this.navigationEvents.TabSelected(tabName);
    }

    /// <summary>
    /// Tabs the selected.
    /// </summary>
    /// <param name="code">The product code.</param>
    /// <param name="name">The product name.</param>
    /// <param name="title">The review title.</param>
    /// <param name="text">The review text.</param>
    /// <param name="rate">The review rate.</param>
    public virtual void NavigationProductReviewed(string code, string name, string title, string text, string rate)
    {
      this.navigationEvents.ProductReviewed(code, name, title, text, rate);
    }

    /// <summary>
    /// Navigation follow list hit.
    /// </summary>
    public virtual void NavigationFollowListHit()
    {
      this.navigationEvents.FollowListHit();
    }

    #endregion

    /// <summary>
    /// Searches the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="hits">The search hits.</param>
    public virtual void Search(string query, int hits)
    {
      new SearchEvents().Search(query, hits);
    }

    /// <summary>
    /// Adds the follow list to query string.
    /// </summary>
    /// <param name="url">The current URL.</param>
    /// <param name="listName">Name of the list.</param>
    /// <returns>The follow list to query string</returns>
    public virtual string AddFollowListToQueryString(string url, string listName)
    {
      return this.navigationEvents.AddFollowListToQueryString(url, listName);
    }

    /// <summary>
    /// Adds the trigger event string to query string.
    /// </summary>
    /// <param name="url">The needed URL.</param>
    /// <param name="eventName">Name of the event.</param>
    /// <returns>Event string to query string.</returns>
    public virtual string AddTriggerEventStringToQueryString(string url, string eventName)
    {
      if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(eventName))
      {
        return this.navigationEvents.AddTriggerEventStringToQueryString(url, eventName);
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets the virtual product URL with analitics query string.
    /// </summary>
    /// <param name="folderNi">The folder ni.</param>
    /// <param name="productNi">The product ni.</param>
    /// <returns>The virtual product URL with analitics query string.</returns>
    public virtual string GetVirtualProductUrlWithAnaliticsQueryString(XPathNodeIterator folderNi, XPathNodeIterator productNi)
    {
      Assert.ArgumentNotNull(folderNi, "folderNi");
      Assert.ArgumentNotNull(productNi, "productNi");

      Item catalogItem = this.GetItem(folderNi);
      Item productItem = this.GetItem(productNi);

      if (catalogItem == null || productItem == null)
      {
        Log.Warn("Product catalog item and product item are null", this);
        return string.Empty;
      }

      string virtualProductUrl = Context.Entity.Resolve<VirtualProductResolver>().GetVirtualProductUrl(catalogItem, productItem);

      if (string.IsNullOrEmpty(virtualProductUrl))
      {
        Log.Warn("Fail to get virtual product URL", this);
        return string.Empty;
      }

      string name = ProductRepositoryUtil.IsBasedOnTemplate(catalogItem.Template, new ID(Configuration.Settings.GetSetting("Ecommerce.Product.BaseTemplateId"))) ? catalogItem["Title"] : catalogItem.Name;

      return this.navigationEvents.AddFollowListToQueryString(virtualProductUrl, name);
    }

    /// <summary>
    /// Gets the virtual product URL with analitics query string.
    /// </summary>
    /// <param name="productItem">The product item.</param>
    /// <returns>The virtual product URL with analitics query string.</returns>
    public virtual string GetVirtualProductUrlWithAnaliticsQueryString(Item productItem)
    {
      Assert.ArgumentNotNull(productItem, "productItem");

      VirtualProductResolver virtualProductResolver = Context.Entity.Resolve<VirtualProductResolver>();
      Item catalogItem = virtualProductResolver.ProductCatalogItem;

      if (catalogItem == null)
      {
        Log.Warn("Product catalog item is null", this);
        return string.Empty;
      }

      string virtualProductUrl = virtualProductResolver.GetVirtualProductUrl(catalogItem, productItem);

      if (string.IsNullOrEmpty(virtualProductUrl))
      {
        Log.Warn("Fail to get virtual product URL", this);
        return string.Empty;
      }

      return this.navigationEvents.AddFollowListToQueryString(virtualProductUrl, catalogItem.Name);
    }

    /// <summary>
    /// Gets the first item from an XPathNodeIterator
    /// </summary>
    /// <param name="iterator">The node iterator.</param>
    /// <returns>The result item.</returns>
    protected virtual Item GetItem(XPathNodeIterator iterator)
    {
      Assert.ArgumentNotNull(iterator, "iterator");
      
      if (!iterator.MoveNext())
      {
        return null;
      }

      ID id;
      if (!ID.TryParse(iterator.Current.GetAttribute("id", string.Empty), out id))
      {
        return null;
      }

      return Sitecore.Context.Database == null ? null : ItemManager.GetItem(id, Language.Current, Version.Latest, Sitecore.Context.Database);
    }
  }
}