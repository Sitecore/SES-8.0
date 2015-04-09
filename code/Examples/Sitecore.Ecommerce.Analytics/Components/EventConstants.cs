// -------------------------------------------------------------------------------------------
// <copyright file="EventConstants.cs" company="Sitecore Corporation">
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
  using System;

  /// <summary>
  /// Events expressions container.
  /// </summary>
  public static class EventConstants
  {
    #region Shopping Cart page events

    /// <summary>
    /// Add to shopping cart.
    /// </summary>
    public static readonly string EventAddToShoppingCart = "Add to Shopping Cart";

    /// <summary>
    /// Shopping cart emptied.
    /// </summary>
    public static readonly string EventShoppingCartEmptied = "Shopping Cart Emptied";

    /// <summary>
    /// Shopping cart Continue Shopping.
    /// </summary>
    public static readonly string EventShoppingCartContinueShopping = "Shopping Cart Continue Shopping";

    /// <summary>
    /// Shopping cart Updated.
    /// </summary>
    public static readonly string EventShoppingCartUpdated = "Shopping Cart Updated";

    /// <summary>
    /// Go to shopping cart.
    /// </summary>
    public static readonly string EventGoToShoppingCart = "Go to Shopping Cart";

    /// <summary>
    /// Shopping cart item Deleted.
    /// </summary>
    public static readonly string EventShoppingCartItemRemoved = "Shopping Cart Item Removed";

    /// <summary>
    /// Shopping cart item Updated.
    /// </summary>
    public static readonly string EventShoppingCartItemUpdated = "Shopping Cart Item Updated";

    /// <summary>
    /// Product Deleted.
    /// </summary>
    public static readonly string EventProductRemoved = "Product Removed";

    /// <summary>
    /// Shopping cart viewed by user
    /// </summary>
    public static readonly string EventShoppingCartViewed = "Shopping Cart Viewed";

    #endregion

    #region Checkout page events

    /// <summary>
    /// Go to check out.
    /// </summary>
    public static readonly string EventGoToCheckout = "Go to Checkout";

    /// <summary>
    /// Checkout Delivery Next.
    /// </summary>
    public static readonly string EventCheckoutDeliveryNext = "Checkout Delivery Next";

    /// <summary>
    /// Checkout Delivery Alternative Changed.
    /// </summary>
    public static readonly string EventCheckoutDeliveryOptionSelected = "Checkout Delivery Option Selected";

    /// <summary>
    /// Checkout Notification Changed.
    /// </summary>
    public static readonly string EventCheckoutNotificationOptionSelected = "Checkout Notification Option Selected";

    /// <summary>
    /// Checkout PaymentMethod Changed.
    /// </summary>
    public static readonly string EventCheckoutPaymentMethodSelected = "Checkout Payment Method Selected";

    /// <summary>
    /// Checkout Next.
    /// </summary>
    public static readonly string EventCheckoutNext = "Checkout Next";

    /// <summary>
    /// Checkout Payment Next.
    /// </summary>
    public static readonly string EventCheckoutPaymentNext = "Checkout Payment Next";

    /// <summary>
    /// Checkout Previous.
    /// </summary>
    public static readonly string EventCheckoutPrevious = "Checkout Previous";

    #endregion

    #region Authentification events

    /// <summary>
    /// Clicked Login Button.
    /// </summary>
    public static readonly string EventClickedLoginButton = "Clicked Login Button";

    /// <summary>
    /// Clicked Login Link.
    /// </summary>
    public static readonly string EventClickedLoginLink = "Clicked Login Link";

    /// <summary>
    /// User logged out.
    /// </summary>
    public static readonly string EventUserLoggedOut = "User Logged Out";

    /// <summary>
    /// User Login Failed.
    /// </summary>
    public static readonly string EventUserLoginFailed = "User Login Failed";

    /// <summary>
    /// User Login Succeded.
    /// </summary>
    public static readonly string EventUserLoginSucceded = "User Login Succeded";

    /// <summary>
    /// Account Creation Failed.
    /// </summary>
    public static readonly string EventAccountCreationFailed = "Account Creation Failed";

    /// <summary>
    /// Account Created.
    /// </summary>
    public static readonly string EventAccountCreated = "Account Created";

    #endregion

    #region Search events

    /// <summary>
    /// The event Search.
    /// </summary> 
    public static readonly string EventSearch = "Search";

    /// <summary>
    /// No Search Hits Found
    /// </summary>
    public static readonly string EventNoSearchHitsFound = "No Search Hits Found";

    #endregion 

    #region Navigation page events

    /// <summary>
    /// Product Reviewed.
    /// </summary>
    public static readonly string EventProductReviewed = "Product Reviewed";

    /// <summary>
    /// Tab Selected.
    /// </summary>
    public static readonly string EventTabSelected = "Tab Selected";

    /// <summary>
    /// User Clicked Item In List.
    /// </summary>
    public static readonly string EventUserClickedItemInList = "User Clicked Item In List";

    /// <summary>
    /// User Followed Item In List.
    /// </summary>
    [Obsolete("Use EventUserClickedItemInList instead")]
    public static readonly string EventUserFollowedItemInList = "User Clicked Item In List";

    #endregion
  }
}