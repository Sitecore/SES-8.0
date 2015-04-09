// -------------------------------------------------------------------------------------------
// <copyright file="Ajax.asmx.cs" company="Sitecore Corporation">
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
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Text;
  using System.Web;
  using System.Web.Script.Services;
  using System.Web.Services;
  using Analytics.Components;
  using Diagnostics;
  using DomainModel.Addresses;
  using DomainModel.Carts;
  using DomainModel.Data;
  using DomainModel.Payments;
  using DomainModel.Users;
  using Sitecore.Analytics;
  using Sitecore.Data.Items;
  using Sitecore.Security.Authentication;
  using Utils;

  /// <summary>
  /// The ajax page.
  /// </summary>
  [WebService(Namespace = "http://www.sitecore.net/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  [ToolboxItem(false)]
  [ScriptService]
  public class Ajax : WebService
  {
    /// <summary>
    /// Loads the rendering.
    /// </summary>
    /// <param name="rendering">The rendering.</param>
    /// <param name="id">The item id.</param>
    /// <returns>The loaded rendering.</returns>
    [WebMethod(EnableSession = true)]
    public string LoadRendering(string rendering, string id)
    {
      try
      {
        Tracker.StartTracking();
      }
      catch (Exception ex)
      {
        LogException(ex);
      }

      Sitecore.Context.Item = Sitecore.Context.Database.GetItem(id);
      return MainUtil.LoadRendering(rendering);
    }

    /// <summary>
    /// Loads the sublayout.
    /// </summary>
    /// <param name="sublayout">The sublayout.</param>
    /// <param name="id">The item id.</param>
    /// <returns>The loaded sublayout.</returns>
    [WebMethod(EnableSession = true)]
    public string LoadSublayout(string sublayout, string id)
    {
      try
      {
        Tracker.StartTracking();
      }
      catch (Exception ex)
      {
        LogException(ex);
      }

      try
      {
        return MainUtil.RenderSublayout(id, sublayout);
      }
      catch (Exception err)
      {
        return err.ToString();
      }
    }

    /// <summary>
    /// Sets the current tab.
    /// </summary>
    /// <param name="tab">The current tab.</param>
    [WebMethod(EnableSession = true)]
    public void SetCurrentTab(string tab)
    {
      HttpContext.Current.Session["EcProductTabName"] = tab;
      try
      {
        Tracker.StartTracking();
        AnalyticsUtil.NavigationTabSelected(tab);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Adds to shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="quantity">The quantity.</param>
    [WebMethod(EnableSession = true)]
    public void AddToShoppingCart(string productCode, string quantity)
    {
      ShoppingCartWebHelper.AddToShoppingCart(productCode, quantity);
    }

    /// <summary>
    /// Deletes from shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    [WebMethod(EnableSession = true)]
    public void DeleteFromShoppingCart(string productCode)
    {
      ShoppingCartWebHelper.DeleteFromShoppingCart(productCode);
    }

    /// <summary>
    /// Deletes the product line from shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    [WebMethod(EnableSession = true)]
    public void DeleteProductLineFromShoppingCart(string productCode)
    {
      ShoppingCartWebHelper.DeleteProductLineFromShoppingCart(productCode);
    }

    /// <summary>
    /// Updates the shopping cart.
    /// </summary>
    /// <param name="productCode">The product code.</param>
    /// <param name="quantity">The quantity.</param>
    [WebMethod(EnableSession = true)]
    public void UpdateShoppingCart(string productCode, string quantity)
    {
      Assert.ArgumentNotNullOrEmpty(productCode, "productCode");
      Assert.ArgumentNotNullOrEmpty(quantity, "quantity");

      uint q;
      if (string.IsNullOrEmpty(quantity) || !uint.TryParse(quantity, out q))
      {
        return;
      }

      IShoppingCartManager shoppingCartManager = Sitecore.Ecommerce.Context.Entity.Resolve<IShoppingCartManager>();
      shoppingCartManager.UpdateProductQuantity(productCode, q);
      try
      {
        Tracker.StartTracking();
        AnalyticsUtil.ShoppingCartUpdated();
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Payments the method changed.
    /// </summary>
    /// <param name="paymentMethodCode">The payment method code.</param>
    [WebMethod(EnableSession = true)]
    public void PaymentMethodChanged(string paymentMethodCode)
    {
      Assert.ArgumentNotNullOrEmpty(paymentMethodCode, "paymentMethodCode");

      IEntityProvider<PaymentSystem> provider = Sitecore.Ecommerce.Context.Entity.Resolve<IEntityProvider<PaymentSystem>>();
      PaymentSystem paymentMethod = provider.Get(paymentMethodCode);

      ShoppingCart shoppingCart = Sitecore.Ecommerce.Context.Entity.GetInstance<ShoppingCart>();
      shoppingCart.PaymentSystem = paymentMethod;
      try
      {
        Tracker.StartTracking();
        AnalyticsUtil.CheckoutPaymentMethodSelected(paymentMethod.Title, paymentMethod.Code);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }

      Sitecore.Ecommerce.Context.Entity.SetInstance(shoppingCart);
    }

    /// <summary>
    /// Shopping cart spot checkout.
    /// </summary>
    [WebMethod(EnableSession = true)]
    public void ShoppingCartSpotCheckout()
    {
      try
      {
        Tracker.StartTracking();
        AnalyticsUtil.GoToCheckOut();
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Creates the customer account.
    /// </summary>
    [WebMethod(EnableSession = true)]
    public void CreateCustomerAccount()
    {
      ICustomerManager<CustomerInfo> manager = Sitecore.Ecommerce.Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
      if (string.IsNullOrEmpty(manager.CurrentUser.Email) || Sitecore.Context.User.IsAuthenticated)
      {
        return;
      }

      manager.CreateCustomerAccount(manager.CurrentUser.Email, string.Empty, manager.CurrentUser.Email);
      manager.UpdateCustomerProfile(manager.CurrentUser);

      try
      {
        Tracker.StartTracking();
        AnalyticsUtil.AuthentificationAccountCreated();
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Logins the user.
    /// </summary>
    [WebMethod(EnableSession = true)]
    public void LoginUser()
    {
      try
      {
        Tracker.StartTracking();
        AnalyticsUtil.AuthentificationClickedLoginLink();
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Logs the out current user.
    /// </summary>   
    [WebMethod(EnableSession = true)]
    public void LogOutCurrentUser()
    {
      ICustomerManager<CustomerInfo> customerManager = Sitecore.Ecommerce.Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();

      try
      {
        Tracker.StartTracking();
        AnalyticsUtil.AuthentificationUserLoggedOut(customerManager.CurrentUser.NickName);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }

      AuthenticationManager.Logout();

      customerManager.ResetCurrentUser();
    }

    /// <summary>
    /// Get Country states
    /// </summary>
    /// <param name="countryCode">The country code.</param>
    /// <returns>returns list of states</returns>
    /// <exception cref="ArgumentException">List of countries is empty.</exception>
    [WebMethod(EnableSession = true)]
    public string GetCountryStates(string countryCode)
    {
      Assert.ArgumentNotNullOrEmpty(countryCode, "countryCode");

      IEntityProvider<Country> countryProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IEntityProvider<Country>>();
      IEnumerable<Country> countries = countryProvider.GetAll();
      if (countries.IsNullOrEmpty())
      {
        throw new ArgumentException("List of countries is empty.");
      }

      Country country = (from c in countries
                         where string.Compare(c.Code, countryCode) == 0 ||
                               string.Compare(c.Name, countryCode) == 0
                         select c).FirstOrDefault();

      IEntity countryEntity = country as IEntity;
      if (countryEntity == null)
      {
        return string.Empty;
      }

      Item countryItem = Sitecore.Context.Database.SelectSingleItem(countryEntity.Alias);
      StringBuilder result = new StringBuilder();
      if (countryItem != null)
      {
        foreach (Item stateItem in countryItem.Children)
        {
          result.AppendFormat("<option value=\"{0}\">{1}</option>", stateItem["Name"], stateItem["Name"]);
        }

        return result.ToString();
      }

      return string.Empty;
    }

    /// <summary>
    /// Saves the delivery address.
    /// </summary>
    /// <param name="countryCode">The country code.</param>
    /// <param name="state">The state.</param>    
    [WebMethod(EnableSession = true)]
    public void SaveShippingAddress(string countryCode, string state)
    {
      ICustomerManager<CustomerInfo> customerManager = Sitecore.Ecommerce.Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
      IEntityProvider<Country> countryProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IEntityProvider<Country>>();
      Country country = countryProvider.Get(countryCode);
      if (country != null)
      {
        customerManager.CurrentUser.ShippingAddress.Country = country;
        customerManager.CurrentUser.BillingAddress.Country = country;
      }

      customerManager.CurrentUser.ShippingAddress.State = state;
      customerManager.CurrentUser.BillingAddress.State = state;
      customerManager.CurrentUser.ShippingAddress.City = string.Empty;
      customerManager.CurrentUser.BillingAddress.City = string.Empty;
    }

    /// <summary>
    /// Edits the shoping cart clicked.
    /// </summary>
    [WebMethod(EnableSession = true)]
    public void EditShopingCartClicked()
    {
      try
      {
        Tracker.StartTracking();
        AnalyticsUtil.GoToShoppingCart();
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
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