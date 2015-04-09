// -------------------------------------------------------------------------------------------
// <copyright file="AnalyticsUtil.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Analytics.Components
{
  using Diagnostics;
  using PageEvents;
  using Sitecore.Analytics;

    /// <summary>
  /// The analitics util.
  /// </summary>
  public class AnalyticsUtil
  {
    #region Fields

    /// <summary>
    /// The authentification events class instance.
    /// </summary>
    private static readonly AuthentificationEvents authentificationEvents = new AuthentificationEvents();

    /// <summary>
    /// The checkout events instance
    /// </summary>
    private static readonly CheckoutEvents checkoutEvents = new CheckoutEvents();

    /// <summary>
    /// The navigation events class instance
    /// </summary>
    private static readonly NavigationEvents navigationEvents = new NavigationEvents();

    /// <summary>
    /// The search events.
    /// </summary>
    private static readonly SearchEvents searchEvents = new SearchEvents();

    /// <summary>
    /// The shopping cart events instance
    /// </summary>
    private static readonly ShoppingCartEvents shoppingCartEvents = new ShoppingCartEvents();

    #endregion

    /// <summary>
    /// Registers the no parameter event.
    /// </summary>
    /// <param name="event">The event.</param>
    public static void RegisterNoParameterEvent(string @event)
    {
      if (!Tracker.IsActive)
      {
        return;
      }

      AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
      Assert.IsNotNull(analyticsHelper, "analyticsHelper");

      string description = analyticsHelper.GetPageEventDescription(@event);

      var currentPage = Tracker.Current.CurrentPage;
      if (currentPage != null)
      {
        currentPage.Register(@event, description);
      }
    }

    #region Shopping Cart page events

    /// <summary>
    /// Adds to shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="productName">Name of the product.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="price">The price.</param>
    public static void AddToShoppingCart(string productCode, string productName, uint quantity, decimal price)
    {
      shoppingCartEvents.AddToShoppingCart(productCode, productName, quantity, price);
    }

    /// <summary>
    /// Shoppings the cart emptied.
    /// </summary>
    /// <param name="shoppingCartContent">Content of the shopping cart.</param>
    /// <param name="itemsInShoppingCart">The items in shopping cart.</param>
    public static void ShoppingCartEmptied(string shoppingCartContent, uint itemsInShoppingCart)
    {
      shoppingCartEvents.ShoppingCartEmptied(shoppingCartContent, itemsInShoppingCart);
    }

    /// <summary>
    /// Shopping cart continue shopping event.
    /// </summary>
    public static void ShoppingCartContinueShopping()
    {
      shoppingCartEvents.ContinueShopping();
    }

    /// <summary>
    /// Shopping cart updated event.
    /// </summary>
    public static void ShoppingCartUpdated()
    {
      shoppingCartEvents.ShoppingCartUpdated();
    }

    /// <summary>
    /// Go to shopping cart.
    /// </summary>
    public static void GoToShoppingCart()
    {
      shoppingCartEvents.GoToShoppingCart();
    }

    /// <summary>
    /// Shopping cart item removed.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="productName">Name of the product.</param>
    /// <param name="amount">The amount.</param>
    public static void ShoppingCartItemRemoved(string productCode, string productName, uint amount)
    {
      shoppingCartEvents.ShoppingCartItemRemoved(productCode, productName, amount);
    }

    /// <summary>
    /// Updates the shopping cart item for analitics.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="productName">Name of the product.</param>
    /// <param name="amount">The amount.</param>
    public static void ShoppingCartItemUpdated(string productCode, string productName, uint amount)
    {
      shoppingCartEvents.ShoppingCartItemUpdated(productCode, productName, amount);
    }

    /// <summary>
    /// Deletes the shopping cart item for analitics.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="productName">Name of the product.</param>
    /// <param name="amount">The amount.</param>
    public static void ShoppingCartProductRemoved(string productCode, string productName, uint amount)
    {
      shoppingCartEvents.ProductRemoved(productCode, productName, amount);
    }

    /// <summary>
    /// Shopping cart viewed.
    /// </summary>
    public static void ShoppingCartViewed()
    {
      shoppingCartEvents.ShoppingCartViewed();
    }

    #endregion

    #region Checkout page events

    /// <summary>
    /// Go to check out.
    /// </summary>
    public static void GoToCheckOut()
    {
      checkoutEvents.GoToCheckOut();
    }

    /// <summary>
    /// Checkous the delivery next.
    /// </summary>
    /// <param name="deliveryAlternativeOption">The delivery alternative option.</param>
    /// <param name="notificationOption">The notification option.</param>
    /// <param name="notificationText">The notification text.</param>
    public static void CheckoutDeliveryNext(string deliveryAlternativeOption, string notificationOption, string notificationText)
    {
      checkoutEvents.DeliveryNext(deliveryAlternativeOption, notificationOption, notificationText);
    }

    /// <summary>
    /// Checkouts the delivery next.
    /// </summary>
    /// <param name="deliveryAlternativeOption">The delivery alternative option.</param>
    public static void CheckoutDeliveryOptionSelected(string deliveryAlternativeOption)
    {
      checkoutEvents.DeliveryOptionSelected(deliveryAlternativeOption);
    }

    /// <summary>
    /// Checkout payment method selected.
    /// </summary>
    /// <param name="optionTitle">The option title.</param>
    /// <param name="optionCode">The option code.</param>
    public static void CheckoutPaymentMethodSelected(string optionTitle, string optionCode)
    {
      checkoutEvents.PaymentMethodSelected(optionTitle, optionCode);
    }

    /// <summary>
    /// Checkout next.
    /// </summary>
    public static void CheckoutNext()
    {
      checkoutEvents.Next();
    }

    /// <summary>
    /// Checkout payment next.
    /// </summary>
    public static void CheckoutPaymentNext()
    {
      checkoutEvents.PaymentNext();
    }

    /// <summary>
    /// Checkouts the notification changed.
    /// </summary>
    /// <param name="deliveryAlternativeOption">The delivery alternative option.</param>
    public static void CheckoutNotificationOptionSelected(string deliveryAlternativeOption)
    {
      checkoutEvents.NotificationOptionSelected(deliveryAlternativeOption);
    }

    /// <summary>
    /// Checkout Previous.
    /// </summary>
    public static void CheckoutPrevious()
    {
      checkoutEvents.Previous();
    }

    #endregion

    #region Authentification page events

    /// <summary>
    /// Authentification clicked login button.
    /// </summary>
    public static void AuthentificationClickedLoginButton()
    {
      authentificationEvents.ClickedLoginButton();
    }

    /// <summary>
    /// Authentification clicked login link.
    /// </summary>
    public static void AuthentificationClickedLoginLink()
    {
      authentificationEvents.ClickedLoginLink();
    }

    /// <summary>
    /// Authentifications the user logged out.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    public static void AuthentificationUserLoggedOut(string userName)
    {
      authentificationEvents.UserLoggedOut(userName);
    }

    /// <summary>
    /// Users the login succeeded.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    public static void AuthentificationUserLoginSucceeded(string userName)
    {
      authentificationEvents.UserLoginSucceded(userName);
    }

    /// <summary>
    /// Users the login failed.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    public static void AuthentificationUserLoginFailed(string userName)
    {
      authentificationEvents.UserLoginFailed(userName);
    }

    /// <summary>
    /// Account creation failed.
    /// </summary>
    public static void AuthentificationAccountCreationFailed()
    {
      authentificationEvents.AccountCreationFailed();
    }

    /// <summary>
    /// Account creation succeded.
    /// </summary>
    public static void AuthentificationAccountCreated()
    {
      authentificationEvents.AccountCreated();
    }

    #endregion

    #region Navigation page events

    /// <summary>
    /// Registers the no parameter event.
    /// </summary>
    /// <param name="tabName">Name of the tab.</param>
    public static void NavigationTabSelected(string tabName)
    {
      navigationEvents.TabSelected(tabName);
    }

    /// <summary>
    /// Tabs the selected.
    /// </summary>
    /// <param name="code">The product code.</param>
    /// <param name="name">The product name.</param>
    /// <param name="title">The review title.</param>
    /// <param name="text">The review text.</param>
    /// <param name="rate">The review rate.</param>
    public static void NavigationProductReviewed(string code, string name, string title, string text, string rate)
    {
      navigationEvents.ProductReviewed(code, name, title, text, rate);
    }

    /// <summary>
    /// Navigation follow list hit.
    /// </summary>
    public static void NavigationFollowListHit()
    {
      navigationEvents.FollowListHit();
    }

    /// <summary>
    /// Adds the trigger event string to query string.
    /// </summary>
    /// <param name="url">
    /// The current URL.
    /// </param>
    /// <param name="eventName">
    /// Name of the event.
    /// </param>
    /// <returns>
    /// The add trigger event string to query string.
    /// </returns>
    public static string AddTriggerEventStringToQueryString(string url, string eventName)
    {
      return navigationEvents.AddFollowListToQueryString(url, eventName);
    }

    /// <summary>
    /// Adds the follow list to query string.
    /// </summary>
    /// <param name="url">
    /// The request URL.
    /// </param>
    /// <param name="eventName">
    /// Name of the event.
    /// </param>
    /// <returns>
    /// The add follow list to query string.
    /// </returns>
    public static string AddFollowListToQueryString(string url, string eventName)
    {
      return navigationEvents.AddFollowListToQueryString(url, eventName);
    }

    #endregion

    /// <summary>
    /// Searches the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="hits">The search hits.</param>
    public static void Search(string query, int hits)
    {
      searchEvents.Search(query, hits);
    }
  }
}