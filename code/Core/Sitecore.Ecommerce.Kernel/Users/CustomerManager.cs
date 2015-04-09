// -------------------------------------------------------------------------------------------
// <copyright file="CustomerManager.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Users
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Security;
  using CheckOuts;
  using Configuration;
  using Diagnostics;
  using DomainModel.Addresses;
  using DomainModel.Carts;
  using DomainModel.CheckOuts;
  using DomainModel.Data;
  using DomainModel.Mails;
  using DomainModel.Users;
  using Security;
  using SecurityModel;
  using Sitecore.Data;
  using Sitecore.Data.Managers;
  using Sitecore.Data.Templates;
  using Sitecore.Pipelines;
  using Sitecore.Security;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using Utils;

  /// <summary>
  /// The customer info class.
  /// </summary>
  /// <typeparam name="T">The CustomerInfo type.</typeparam>
  public class CustomerManager<T> : ICustomerManager<T> where T : DomainModel.Users.CustomerInfo
  {
    #region Fields

    /// <summary>
    /// Customer profile item id.
    /// </summary>
    private const string CustomerProfileItemId = "{AC45E370-869B-4AEA-BC51-C4AFB248D498}";

    /// <summary>
    /// The user profile name.
    /// </summary>
    private const string UserProfileName = "Name";

    /// <summary>
    /// The user profile e-mail.
    /// </summary>
    private const string UserProfileEmail = "E-mail";

    /// <summary>
    /// The user profile customer id.
    /// </summary>
    private const string UserProfileCustomerId = "Customer ID";

    /// <summary>
    /// The billing address name.
    /// </summary>
    private const string BillingAddressName = "Billing Address Name";

    /// <summary>
    /// The billing address name 2.
    /// </summary>
    private const string BillingAddressName2 = "Billing Address Name 2";

    /// <summary>
    /// The billing address.
    /// </summary>
    private const string BillingAddress = "Billing Address";

    /// <summary>
    /// The billing address 2.
    /// </summary>
    private const string BillingAddress2 = "Billing Address 2";

    /// <summary>
    /// The billing address country code.
    /// </summary>
    private const string BillingAddressCountryCode = "Billing Address Country Code";

    /// <summary>
    /// The billing address zip.
    /// </summary>
    private const string BillingAddresZip = "Billing Address Zip";

    /// <summary>
    /// The billing address city.
    /// </summary>
    private const string BillingAddresCity = "Billing Address City";

    /// <summary>
    /// The billing address state.
    /// </summary>
    private const string BillingAddresState = "Billing Address State";

    /// <summary>
    /// The shipping address name.
    /// </summary>
    private const string ShippingAddressName = "Shipping Address Name";

    /// <summary>
    /// The shipping address name 2.
    /// </summary>
    private const string ShippingAddressName2 = "Shipping Address Name 2";

    /// <summary>
    /// The shipping address.
    /// </summary>
    private const string ShippingAddress = "Shipping Address";

    /// <summary>
    /// The shipping address 2.
    /// </summary>
    private const string ShippingAddress2 = "Shipping Address 2";

    /// <summary>
    /// The shipping address country code.
    /// </summary>
    private const string ShippingAddressCountryCode = "Shipping Address Country Code";

    /// <summary>
    /// The shipping address zip.
    /// </summary>
    private const string ShippingAddresZip = "Shipping Address Zip";

    /// <summary>
    /// The shipping address city.
    /// </summary>
    private const string ShippingAddresCity = "Shipping Address City";

    /// <summary>
    /// The shipping address state.
    /// </summary>
    private const string ShippingAddresState = "Shipping Address State";

    /// <summary>
    /// Defines the database that contains profile item.
    /// </summary>
    private const string ProfileDatabase = "core";

    #endregion

    /// <summary>
    /// Gets or sets the membership.
    /// </summary>
    /// <value>
    /// The membership.
    /// </value>
    [CanBeNull]
    public CustomerMembership CustomerMembership { get; set; }

    /// <summary>
    /// Gets or sets the current user.
    /// </summary>
    /// <value>The current user.</value>
    public virtual T CurrentUser
    {
      get
      {
        var customerInfo = Context.Entity.GetInstance<T>();

        if (!string.IsNullOrEmpty(customerInfo.NickName) && !string.IsNullOrEmpty(customerInfo.CustomerId))
        {
          return customerInfo;
        }

        var membershipUser = Membership.GetUser();
        if (membershipUser != null)
        {
          var name = Sitecore.Context.Domain.GetShortName(membershipUser.UserName);
          var savedCustomerInfo = this.GetCustomerInfo(name);
          if (savedCustomerInfo != null)
          {
            customerInfo = savedCustomerInfo;
          }
        }

        Context.Entity.SetInstance(customerInfo);

        return customerInfo;
      }

      set
      {
        Context.Entity.SetInstance(value);
      }
    }

    /// <summary>
    /// Resets the current user.
    /// </summary>
    public void ResetCurrentUser()
    {
      this.CurrentUser = Context.Entity.Resolve<T>();

      var shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
      shoppingCart.CustomerInfo = this.CurrentUser;
      Context.Entity.SetInstance(shoppingCart);

      var checkOut = Context.Entity.GetInstance<ICheckOut>();
      var @out = checkOut as CheckOut;
      if (@out != null)
      {
        @out.HasOtherShippingAddressBeenChecked = false;
      }

      Context.Entity.SetInstance(checkOut);
    }

    /// <summary>
    /// Gets the customer info.
    /// </summary>
    /// <param name="nickName">Name of the nick.</param>
    /// <returns>
    /// If user account exists, returns the user info, else returns null.
    /// </returns>
    public virtual T GetCustomerInfo(string nickName)
    {
      Assert.IsNotNullOrEmpty(nickName, "User nick name is null or empty");

      var customerInfo = Context.Entity.Resolve<T>();

      var fullNickName = Sitecore.Context.Domain.GetFullName(nickName);
      var user = User.FromName(fullNickName, true);

      if (user == null)
      {
        Log.Warn(string.Format("User: '{0}' cannot be logged in", fullNickName), this);
        return customerInfo;
      }

      var userProfile = user.Profile;
      Assert.IsNotNull(userProfile, "User profile is null");

      customerInfo.NickName = nickName;

      IList<TemplateField> profileFields = this.GetProfileFields(userProfile).Where(pg => (new[] { "billing ", "shipping " }).All(t => !pg.Name.ToLower().StartsWith(t))).ToList();
      foreach (var field in profileFields)
      {
        customerInfo[field.Name] = this.GetCustomProperty(userProfile, field.Name);
      }

      var customerId = this.GetCustomProperty(userProfile, UserProfileCustomerId);
      if (string.IsNullOrEmpty(customerId) || !ID.IsID(customerId))
      {
        customerInfo.CustomerId = ID.NewID.ToString();
      }
      else
      {
        customerInfo.CustomerId = customerId;
      }

      customerInfo.BillingAddress = customerInfo.BillingAddress ?? Context.Entity.Resolve<AddressInfo>();
      customerInfo.BillingAddress.Name = this.GetCustomProperty(userProfile, BillingAddressName);
      customerInfo.BillingAddress.Name2 = this.GetCustomProperty(userProfile, BillingAddressName2);
      customerInfo.BillingAddress.Address = this.GetCustomProperty(userProfile, BillingAddress);
      customerInfo.BillingAddress.Address2 = this.GetCustomProperty(userProfile, BillingAddress2);
      customerInfo.BillingAddress.Zip = this.GetCustomProperty(userProfile, BillingAddresZip);
      customerInfo.BillingAddress.City = this.GetCustomProperty(userProfile, BillingAddresCity);
      customerInfo.BillingAddress.State = this.GetCustomProperty(userProfile, BillingAddresState);

      var billingCountryCode = this.GetCustomProperty(userProfile, BillingAddressCountryCode);
      var countryProvider = Context.Entity.Resolve<IEntityProvider<Country>>();
      customerInfo.BillingAddress.Country = !string.IsNullOrEmpty(billingCountryCode) ? countryProvider.Get(billingCountryCode) : this.GetDefaultCountry();

      customerInfo.ShippingAddress = customerInfo.ShippingAddress ?? Context.Entity.Resolve<AddressInfo>();
      customerInfo.ShippingAddress.Name = this.GetCustomProperty(userProfile, ShippingAddressName);
      customerInfo.ShippingAddress.Name2 = this.GetCustomProperty(userProfile, ShippingAddressName2);
      customerInfo.ShippingAddress.Address = this.GetCustomProperty(userProfile, ShippingAddress);
      customerInfo.ShippingAddress.Address2 = this.GetCustomProperty(userProfile, ShippingAddress2);
      customerInfo.ShippingAddress.Zip = this.GetCustomProperty(userProfile, ShippingAddresZip);
      customerInfo.ShippingAddress.City = this.GetCustomProperty(userProfile, ShippingAddresCity);
      customerInfo.ShippingAddress.State = this.GetCustomProperty(userProfile, ShippingAddresState);

      var shippingCountryCode = this.GetCustomProperty(userProfile, ShippingAddressCountryCode);
      customerInfo.ShippingAddress.Country = !string.IsNullOrEmpty(shippingCountryCode) ? countryProvider.Get(shippingCountryCode) : customerInfo.BillingAddress.Country;

      return customerInfo;
    }

    /// <summary>
    /// Creates the customer with customer info.
    /// </summary>
    /// <param name="nickName">Name of the nick.</param>
    /// <param name="password">The password.</param>
    /// <param name="email">The email.</param>
    /// <returns>
    /// If customer is created, returns customer, else return null.
    /// </returns>
    public virtual T CreateCustomerAccount(string nickName, string password, string email)
    {
      Assert.IsNotNullOrEmpty(nickName, "User nick name is null or empty");
      Assert.IsNotNullOrEmpty(email, "User e-mail is null or empty");

      password = string.IsNullOrEmpty(password) ? Membership.GeneratePassword(8, 2) : password;

      var fullNickName = Sitecore.Context.Domain.GetFullName(nickName);

      if (User.Exists(fullNickName))
      {
        return this.GetCustomerInfo(fullNickName);
      }

      var user = User.Create(fullNickName, password);

      var customerId = ID.NewID;

      var userProfile = user.Profile;
      Assert.IsNotNull(userProfile, "User profile is null");

      using (new SecurityDisabler())
      {
        var membershipUser = Membership.GetUser(fullNickName);
        Assert.IsNotNull(membershipUser, "Membership user is null");

        membershipUser.Email = email;
        Membership.UpdateUser(membershipUser);

        userProfile.FullName = fullNickName;
        userProfile.Name = Sitecore.Context.Domain.GetShortName(nickName);
        userProfile.Email = email;
        userProfile.ProfileItemId = CustomerProfileItemId;
        userProfile.SetCustomProperty(UserProfileCustomerId, customerId.ToString());
        userProfile.SetCustomProperty(UserProfileEmail, email);
        userProfile.Save();
      }

      var customerInfo = Context.Entity.Resolve<T>();
      customerInfo.NickName = fullNickName;
      customerInfo.Email = email;
      customerInfo.CustomerId = customerId.ToString();
      customerInfo.BillingAddress = Context.Entity.Resolve<AddressInfo>();
      customerInfo.BillingAddress.Country = Context.Entity.Resolve<IEntityProvider<Country>>().GetDefault();
      customerInfo.ShippingAddress = Context.Entity.Resolve<AddressInfo>();
      customerInfo.ShippingAddress.Country = customerInfo.BillingAddress.Country;

      var args = new PipelineArgs();
      args.CustomData["user"] = user;
      args.CustomData["customerInfo"] = customerInfo;
      args.CustomData["userName"] = fullNickName;
      args.CustomData["password"] = password;
      args.CustomData["email"] = email;

      CorePipeline.Run("customerCreated", args);

      return (T)args.CustomData["customerInfo"];
    }

    /// <summary>
    /// Updates the customer info.
    /// </summary>
    /// <param name="customerInfo">The customer info.</param>
    public virtual void UpdateCustomerProfile(T customerInfo)
    {
      Assert.IsNotNull(customerInfo, "Customer info is null");

      var fullNickName = Sitecore.Context.Domain.GetFullName(customerInfo.NickName);
      var user = User.FromName(fullNickName, true);
      Assert.IsNotNull(user, "User is null");

      var userProfile = user.Profile;
      Assert.IsNotNull(userProfile, "User profile is null");

      using (new SecurityDisabler())
      {
        var userName = Sitecore.Context.Domain.GetFullName(customerInfo.NickName);

        var membershipUser = Membership.GetUser(userName);
        Assert.IsNotNull(membershipUser, "Membership user is null");

        membershipUser.Email = customerInfo.Email;
        Membership.UpdateUser(membershipUser);

        userProfile.FullName = userName;
        userProfile.Name = Sitecore.Context.Domain.GetShortName(customerInfo.NickName);
        userProfile.Email = customerInfo.Email;

        this.SetCustomProperty(userProfile, UserProfileName, userProfile.Name);

        this.SetCustomProperty(userProfile, BillingAddressName, customerInfo.BillingAddress.Name);
        this.SetCustomProperty(userProfile, BillingAddressName2, customerInfo.BillingAddress.Name2);
        this.SetCustomProperty(userProfile, BillingAddress, customerInfo.BillingAddress.Address);
        this.SetCustomProperty(userProfile, BillingAddress2, customerInfo.BillingAddress.Address2);
        this.SetCustomProperty(userProfile, BillingAddressCountryCode, customerInfo.BillingAddress.Country.Code);
        this.SetCustomProperty(userProfile, BillingAddresZip, customerInfo.BillingAddress.Zip);
        this.SetCustomProperty(userProfile, BillingAddresCity, customerInfo.BillingAddress.City);
        this.SetCustomProperty(userProfile, BillingAddresState, customerInfo.BillingAddress.State);

        this.SetCustomProperty(userProfile, ShippingAddressName, customerInfo.ShippingAddress.Name);
        this.SetCustomProperty(userProfile, ShippingAddressName2, customerInfo.ShippingAddress.Name2);
        this.SetCustomProperty(userProfile, ShippingAddress, customerInfo.ShippingAddress.Address);
        this.SetCustomProperty(userProfile, ShippingAddress2, customerInfo.ShippingAddress.Address2);
        this.SetCustomProperty(userProfile, ShippingAddressCountryCode, customerInfo.ShippingAddress.Country.Code);
        this.SetCustomProperty(userProfile, ShippingAddresZip, customerInfo.ShippingAddress.Zip);
        this.SetCustomProperty(userProfile, ShippingAddresCity, customerInfo.ShippingAddress.City);
        this.SetCustomProperty(userProfile, ShippingAddresState, customerInfo.ShippingAddress.State);

        IList<TemplateField> profileFields = this.GetProfileFields(userProfile).Where(pg => (new[] { "billing ", "shipping " }).All(t => !pg.Name.ToLower().StartsWith(t))).ToList();

        foreach (var field in profileFields)
        {
          this.SetCustomProperty(userProfile, field.Name, customerInfo[field.Name]);
        }

        userProfile.Save();
      }
    }

    /// <summary>
    /// Log in the customer.
    /// </summary>
    /// <param name="nickName">Name of the nick.</param>
    /// <param name="password">The customer password.</param>
    /// <returns>The login result.</returns>
    public virtual bool LogInCustomer(string nickName, string password)
    {
      var fullNickName = Sitecore.Context.Domain.GetFullName(nickName);
      var loginSucces = AuthenticationManager.Login(fullNickName, password);

      if (!loginSucces)
      {
        return false;
      }

      var customerInfo = this.GetCustomerInfo(fullNickName);
      if (customerInfo != null)
      {
        this.CurrentUser = customerInfo;

        var shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
        shoppingCart.CustomerInfo = customerInfo;
        Context.Entity.SetInstance(shoppingCart);

        return true;
      }

      Sitecore.Context.Logout();

      return false;
    }

    /// <summary>
    /// Logins the customer.
    /// </summary>
    /// <param name="customerInfo">The customer info.</param>
    /// <param name="password">The password.</param>
    /// <returns>
    /// The login result.
    /// </returns>
    public virtual bool LogInCustomer(T customerInfo, string password)
    {
      var fullNickName = Sitecore.Context.Domain.GetFullName(customerInfo.NickName);
      return this.LogInCustomer(fullNickName, password);
    }

    /// <summary>
    /// Notifies the user.
    /// </summary>
    /// <param name="mailTemplate">The mail template.</param>
    /// <param name="customerInfo">The customer info.</param>
    public virtual void NotifyUser(string mailTemplate, T customerInfo)
    {
      var usersParams = new { UserName = customerInfo.NickName, CustomerEmail = customerInfo.Email };
      this.NotifyUser(mailTemplate, usersParams);
    }

    /// <summary>
    /// Notifies the user.
    /// </summary>
    /// <param name="mailTemplate">The mail template.</param>
    /// <param name="userParams">The user parameters.</param>
    public virtual void NotifyUser(string mailTemplate, object userParams)
    {
      var mailProvider = Context.Entity.Resolve<IMail>();
      mailProvider.SendMail(mailTemplate, userParams, string.Empty);
    }

    /// <summary>
    /// Deletes the customer.
    /// </summary>
    /// <param name="nickName">Name of the nick.</param>
    public virtual void DeleteCustomer(string nickName)
    {
      var fullNickName = Sitecore.Context.Domain.GetFullName(nickName);
      if (!User.Exists(fullNickName))
      {
        return;
      }

      var user = User.FromName(fullNickName, false);
      var customerMembership = this.CustomerMembership;
      if (customerMembership != null && customerMembership.IsCustomer(user))
      {
        user.Delete();
      }
    }

    /// <summary>
    /// Deletes the customer.
    /// </summary>
    /// <param name="customerInfo">The customer info.</param>
    public virtual void DeleteCustomer(T customerInfo)
    {
      Assert.ArgumentNotNull(customerInfo, "customerInfo");
      Assert.ArgumentNotNull(customerInfo.NickName, string.Format("customerInfo.{0}", "NickName"));

      this.DeleteCustomer(customerInfo.NickName);
    }

    /// <summary>
    /// Deletes all customers.
    /// </summary>
    public virtual void DeleteAllCustomers()
    {
      foreach (var acount in Sitecore.Context.Domain.GetAccounts())
      {
        this.DeleteCustomer(acount.LocalName);
      }
    }

    /// <summary>
    /// Gets the default country.
    /// </summary>
    /// <returns>The default country.</returns>
    /// <exception cref="ArgumentException">List of countries is empty.</exception>
    protected virtual Country GetDefaultCountry()
    {
      var vatRegionprovider = Context.Entity.Resolve<IEntityProvider<VatRegion>>();
      var countryProvider = Context.Entity.Resolve<IEntityProvider<Country>>();

      var defaultVatRegion = vatRegionprovider.GetDefault();

      var countries = countryProvider.GetAll();
      var enumerable = countries as Country[] ?? countries.ToArray();
      if (enumerable.IsNullOrEmpty())
      {
        throw new ArgumentException("List of countries is empty.");
      }

      foreach (var country in enumerable.Where(country => country.VatRegion != null && !string.IsNullOrEmpty(country.VatRegion.Name)).Where(country => country.VatRegion.Name.Equals(defaultVatRegion.Name, StringComparison.OrdinalIgnoreCase)))
      {
        return country;
      }

      return countryProvider.GetDefault();
    }

    /// <summary>
    /// Sets the custom properties.
    /// </summary>
    /// <param name="userProfile">The user profile.</param>
    /// <param name="name">The property name.</param>
    /// <param name="value">The value.</param>
    protected virtual void SetCustomProperty(UserProfile userProfile, string name, string value)
    {
      if (!string.IsNullOrEmpty(value))
      {
        userProfile.SetCustomProperty(name, value);
      }
    }

    /// <summary>
    /// Gets the custom property.
    /// </summary>
    /// <param name="userProfile">The user profile.</param>
    /// <param name="name">The property name.</param>
    /// <returns>The value.</returns>
    protected virtual string GetCustomProperty(UserProfile userProfile, string name)
    {
      var value = userProfile.GetCustomProperty(name);

      return string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    /// <summary>
    /// Gets the list of the template fields.
    /// </summary>
    /// <param name="userProfile">The user profile.</param>
    /// <returns>The list of the fields.</returns>
    protected IEnumerable<TemplateField> GetProfileFields(UserProfile userProfile)
    {
      IList<TemplateField> profileFields = new List<TemplateField>();
      using (new SecurityDisabler())
      {
        var database = Factory.GetDatabase(ProfileDatabase);
        if (userProfile.ProfileItemId == null)
        {
          return profileFields;
        }

        var item = database.GetItem(userProfile.ProfileItemId);
        if (item == null)
        {
          return profileFields;
        }

        var template = TemplateManager.GetTemplate(item.TemplateID, database);
        if (template != null)
        {
          profileFields = template.GetFields().Where(tf => tf.Template == template).ToList();
        }
      }

      return profileFields;
    }
  }
}