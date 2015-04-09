// -------------------------------------------------------------------------------------------
// <copyright file="CustomerDetails.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Form.Action
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Web;
  using System.Web.Security;
  using Analytics.Components;
  using CheckOuts;
  using Diagnostics;
  using DomainModel.Addresses;
  using DomainModel.Carts;
  using DomainModel.CheckOuts;
  using DomainModel.Data;
  using DomainModel.Shippings;
  using DomainModel.Users;
  using Forms;
  using Forms.Actions;
  using Sitecore.Data;
  using Sitecore.Ecommerce.Data;
  using Sitecore.Form.Core.Client.Data.Submit;
  using Sitecore.Form.Core.Client.Submit;
  using Sitecore.Form.Core.Pipelines.RenderForm;
  using Sitecore.Form.Submit;
  using Utils;

  /// <summary>
  /// CheckOut name and address form
  /// </summary>
  public class CustomerDetails : ILoad, ISaveAction
  {
    #region Constants

    /// <summary>
    /// Navigation link capture.
    /// </summary>
    private const string NavigationLinkNext = "Next";

    #endregion

    #region Public methods

    /// <summary>
    /// Adds CustomerInfo, Shippingdetails and Billingdetails into the ShoppingCart instance
    /// Also updates the CustomerInfo session
    /// </summary>
    /// <param name="formid">The formid.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="data">The data.</param>
    /// <exception cref="ValidatorException">Throws <c>ValidatorException</c> in a customer is already exists.</exception>
    public void Execute(ID formid, AdaptedResultList fields, params object[] data)
    {
      AnalyticsUtil.CheckoutNext();

      NameValueCollection orderInfo = new NameValueCollection();
      ActionHelper.FillFormData(orderInfo, fields, this.FillOrderInfo);
      bool isNewUser = false;
      ICustomerManager<CustomerInfo> customerManager = Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
      CustomerInfo customerInfo;

      if (orderInfo["HideCreateUserSection"] == "1" && !string.IsNullOrEmpty(orderInfo["Password"]) && !Sitecore.Context.User.IsAuthenticated)
      {
        try
        {
          string fullNickName = Sitecore.Context.Domain.GetFullName(orderInfo["Email"]);
          customerInfo = customerManager.CreateCustomerAccount(fullNickName, orderInfo["Password"], orderInfo["Email"]);
        }
        catch (MembershipCreateUserException ex)
        {
          Log.Error("Unable to create a customer account.", ex, this);
          throw new ValidatorException(ex.Message, ex);
        }

        isNewUser = true;
      }
      else
      {
        customerInfo = customerManager.CurrentUser;
      }

      Assert.IsNotNull(customerInfo, "Cannot create user");

      foreach (string key in orderInfo.AllKeys)
      {
        customerInfo[key] = orderInfo[key];
      }

      customerInfo.BillingAddress.Name = orderInfo["Name"];
      customerInfo.BillingAddress.Address = orderInfo["Address"];
      customerInfo.BillingAddress.Zip = orderInfo["Zip"];
      customerInfo.BillingAddress.City = orderInfo["City"];
      customerInfo.BillingAddress.State = orderInfo["State"];
      customerInfo.Email = orderInfo["Email"];

      // Find country.
      if (!string.IsNullOrEmpty(orderInfo["Country"]))
      {
        IEntityProvider<Country> countryProvider = Context.Entity.Resolve<IEntityProvider<Country>>();
        customerInfo.BillingAddress.Country = countryProvider.Get(orderInfo["Country"]);
      }

      // If shipping checkbox is checked
      if (orderInfo["HideThisSection"] == "1")
      {
        customerInfo.ShippingAddress.Name = orderInfo["ShippingName"];
        customerInfo.ShippingAddress.Address = orderInfo["ShippingAddress"];
        customerInfo.ShippingAddress.Zip = orderInfo["ShippingZip"];
        customerInfo.ShippingAddress.City = orderInfo["ShippingCity"];
        customerInfo.ShippingAddress.State = orderInfo["ShippingState"];

        if (!string.IsNullOrEmpty(orderInfo["ShippingCountry"]))
        {
          IEntityProvider<Country> countryProvider = Context.Entity.Resolve<IEntityProvider<Country>>();
          customerInfo.ShippingAddress.Country = countryProvider.Get(orderInfo["ShippingCountry"]);
        }
      }
      else
      {
        EntityHelper entityHepler = Context.Entity.Resolve<EntityHelper>();
        AddressInfo targetAddressInfo = customerInfo.ShippingAddress;

        entityHepler.CopyPropertiesValues(customerInfo.BillingAddress, ref targetAddressInfo);        
      }

      ShoppingCart shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
      shoppingCart.CustomerInfo = customerInfo;

      IEntityProvider<NotificationOption> notificationProvider = Context.Entity.Resolve<IEntityProvider<NotificationOption>>();
      Assert.IsNotNull(notificationProvider, "Notification options provider is null");

      shoppingCart.NotificationOption = notificationProvider.Get("Email");
      shoppingCart.NotificationOptionValue = orderInfo["Email"];

      if (isNewUser)
      {
        customerManager.UpdateCustomerProfile(customerInfo);
      }

      customerManager.CurrentUser = customerInfo;
      Context.Entity.SetInstance(shoppingCart);

      // Indicates that the form has been filled out and that it should be initialized if the user returnes to this page.
      ICheckOut checkOut = Context.Entity.GetInstance<ICheckOut>();
      if (checkOut is CheckOut)
      {
        ((CheckOut)checkOut).HasOtherShippingAddressBeenChecked = orderInfo["HideThisSection"] == "1";
      }

      Context.Entity.SetInstance(checkOut);

      ItemUtil.RedirectToNavigationLink(NavigationLinkNext, false);
    }

    /// <summary>
    /// Gets the most recent set options from the ShoppingCart and customerInfo instance
    /// and displays the options in the web form.
    /// If there are no items in the ShoppingCart, the user will be redirected to the
    /// home page.
    /// </summary>
    /// <param name="isPostback">Gets a value that indicates whether the form is being rendered for the first time or is being loaded in response to a postback.</param>
    /// <param name="args">The render form arguments.</param>
    public void Load(bool isPostback, RenderFormArgs args)
    {
      // Checks if the user has appropiate access.
      ICheckOut checkOut = Context.Entity.GetInstance<ICheckOut>();
      if (checkOut != null && checkOut is CheckOut)
      {
        if (!((CheckOut)checkOut).BeginCheckOut || !((CheckOut)checkOut).DeliveryDone)
        {
          HttpContext.Current.Response.Redirect("/");
        }
      }

      HtmlFormModifier form = new HtmlFormModifier(args);

      form.DisableInputField("UserName");

      /*
      Item navigationLinkItem = Utils.ItemUtil.GetNavigationLinkItem("Previous");
      if (navigationLinkItem != null)
      {
        form.AddButtonFromNavigationLink(navigationLinkItem, true);
      }
      */

      // If the user is logged in we hide the Create User boks.
      if (MainUtil.IsLoggedIn())
      {
        form.HideSectionByField("Create Username", "HideCreateUserSection");
      }

      ICustomerManager<CustomerInfo> customerManager = Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
      form.SetInputValue("UserName", customerManager.CurrentUser.Email);

      if (isPostback)
      {
        return;
      }

      ShoppingCart shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
      shoppingCart.CustomerInfo = customerManager.CurrentUser;

      Context.Entity.SetInstance(shoppingCart);

      // Checks the Use other shipping address checkbox.
      // form.SetCheckboxSelected("HideThisSection", true);

      form.SetInputValue("Name", customerManager.CurrentUser.BillingAddress.Name);
      form.SetInputValue("Address", customerManager.CurrentUser.BillingAddress.Address);
      form.SetInputValue("Zip", customerManager.CurrentUser.BillingAddress.Zip);
      form.SetInputValue("City", customerManager.CurrentUser.BillingAddress.City);
      form.SetInputValue("Email", customerManager.CurrentUser.Email);

      if (customerManager.CurrentUser.BillingAddress.Country.Code != "US")
      {
        form.HideField("State");
      }
      else
      {
        form.SetSelectedDropListValue("State", customerManager.CurrentUser.BillingAddress.State);
      }

      if (customerManager.CurrentUser.ShippingAddress.Country.Code != "US")
      {
        form.HideField("ShippingState");
      }
      else
      {
        form.SetSelectedDropListValue("ShippingState", customerManager.CurrentUser.ShippingAddress.State);
      }

      form.SetSelectedDropListValue("Country", customerManager.CurrentUser.BillingAddress.Country.Name);

      /*
      // Only field out shipping address if it was checked before.
      if (checkOut is CheckOut)
      {
        if (!((CheckOut)checkOut).HasOtherShippingAddressBeenChecked)
        {
          return;
        }
      }
      */
      form.SetInputValue("ShippingName", customerManager.CurrentUser.ShippingAddress.Name);
      form.SetInputValue("ShippingAddress", customerManager.CurrentUser.ShippingAddress.Address);
      form.SetInputValue("ShippingZip", customerManager.CurrentUser.ShippingAddress.Zip);
      form.SetInputValue("ShippingCity", customerManager.CurrentUser.ShippingAddress.City);

      form.SetSelectedDropListValue("ShippingCountry", customerManager.CurrentUser.ShippingAddress.Country.Title);

      foreach (string key in customerManager.CurrentUser.CustomProperties.AllKeys)
      {
        form.SetInputValue(key, customerManager.CurrentUser.CustomProperties[key]);
      }
    }

    #endregion

    #region Private methods

    /// <summary> Fill name value collaction by form data </summary>
    /// <param name="fieldName"> name of field</param>
    /// <param name="fieldValue"> form fields</param>
    /// <returns> returns collection of form data</returns>
    private NameValueCollection FillOrderInfo(string fieldName, string fieldValue)
    {
      NameValueCollection orderInfo = new NameValueCollection();
      switch (fieldName)
      {
        case "Country":
          fieldValue = BusinessCatalogUtil.GetOptionFromName(fieldValue, BusinessCatalogUtil.COUNTRIES, "Code");
          break;
        case "ShippingCountry":
          fieldValue = (!string.IsNullOrEmpty(fieldValue)) ? BusinessCatalogUtil.GetOptionFromName(fieldValue, BusinessCatalogUtil.COUNTRIES, "Code") : string.Empty;
          break;
      }

      orderInfo.Add(fieldName, fieldValue);
      return orderInfo;
    }

    #endregion
  }
}