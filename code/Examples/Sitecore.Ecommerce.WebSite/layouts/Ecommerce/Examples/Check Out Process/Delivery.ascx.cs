// -------------------------------------------------------------------------------------------
// <copyright file="Delivery.ascx.cs" company="Sitecore Corporation">
//    Copyright (c) Sitecore Corporation 1999-2015
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Web;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using Configurations;
  using Diagnostics;
  using DomainModel.Addresses;
  using DomainModel.Data;
  using DomainModel.Shippings;
  using DomainModel.Users;
  using Examples.CheckOutProcess;
  using Globalization;
  using Sitecore.Analytics;
  using Sitecore.Data.Items;
  using Sitecore.Ecommerce.Examples;
  using Utils;
  using DomainModel.Currencies;

  /// <summary>
  /// The delivery.
  /// </summary>
  public partial class Delivery : UserControl
  {
    /// <summary>
    /// customer info
    /// </summary>
    private CustomerInfo customerInfo;

    /// <summary>
    /// Shipping provider.
    /// </summary>
    private ShippingProvider shippingProvider;

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    /// <value>The currency.</value>
    public virtual Currency Currency { get; set; }

    /// <summary>
    /// Gets the customer info.
    /// </summary>
    /// <value>The customer info.</value>
    public virtual CustomerInfo CustomerInfo
    {
      get
      {
        if (this.customerInfo == null)
        {
          ICustomerManager<CustomerInfo> customerManager = Sitecore.Ecommerce.Context.Entity.Resolve<ICustomerManager<CustomerInfo>>();
          this.customerInfo = customerManager.CurrentUser;
        }

        return this.customerInfo;
      }
    }

    /// <summary>
    /// Gets or sets the shipping provider.
    /// </summary>
    /// <value>The shipping provider.</value>
    public virtual ShippingProvider ShippingProvider
    {
      get
      {
        if (this.shippingProvider == null)
        {
          DomainModel.Carts.ShoppingCart shoppingCart = Sitecore.Ecommerce.Context.Entity.GetInstance<DomainModel.Carts.ShoppingCart>();
          this.shippingProvider = shoppingCart.ShippingProvider;
        }

        return this.shippingProvider;
      }

      set
      {
        this.shippingProvider = value;
      }
    }

    /// <summary>
    /// Gets or sets the display mode.
    /// </summary>
    /// <value>The display mode.</value>
    public OrderDisplayMode DisplayMode { get; set; }

    #region Protected methods

    /// <summary>
    /// The page_ load.
    /// </summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e"> The event args. </param>
    protected void Page_Load(object sender, EventArgs e)
    {
      this.DisplayShippingMethod();

      this.lblShippingMethods.Text = Translate.Text(Texts.ShippingMethod);

      this.lblShippingDestination.Text = string.Format("{0}: {1}", Translate.Text(Texts.Location), this.GetShippingDestination());
      this.changeDestination.InnerText = Translate.Text(Texts.ChangeYourLocation);

      this.FillShippingOptions();

      this.DisplayPrice(this.ShippingProvider.Price);

      this.lblCountries.Text = Translate.Text(Texts.Country);
      this.lblStates.Text = Translate.Text(Texts.StateProvince);

      this.FillCountries();
      this.FillStates();
    }

    /// <summary>
    /// Displays the price.
    /// </summary>
    /// <param name="price">The price.</param>
    protected virtual void DisplayPrice(decimal price)
    {
      ShoppingCartSettings shoppingCartSettings = Sitecore.Ecommerce.Context.Entity.GetConfiguration<ShoppingCartSettings>();
      GeneralSettings generalSetting = Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>();

      Assert.IsNotNull(this.Currency, "Unable to display price. Currency cannot be null.");
      Assert.IsNotNullOrEmpty(this.Currency.Code, "Unable to display price. Currency code cannot be null or empty.");

      this.lblShippingCost.Text = MainUtil.FormatPrice(price, generalSetting.DisplayCurrencyOnPrices, shoppingCartSettings.PriceFormatString, this.Currency.Code);
    }

    /// <summary>
    /// Fills the countries.
    /// </summary>
    /// <exception cref="ArgumentException">List of countries is empty.</exception>
    protected virtual void FillCountries()
    {
      IEntityProvider<Country> countryProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IEntityProvider<Country>>();
      IEnumerable<Country> countries = countryProvider.GetAll();

      if (countries.IsNullOrEmpty())
      {
        throw new ArgumentException("List of countries is empty.");
      }

      this.ddlShippingCountries.DataSource = countries;
      this.ddlShippingCountries.DataTextField = "Title";
      this.ddlShippingCountries.DataValueField = "Code";
      this.ddlShippingCountries.DataBind();

      AddressInfo info = this.GetUserShippingInfo();

      if (info != null)
      {
        this.ddlShippingCountries.Items.FindByValue(info.Country.Code).Selected = true;
      }
      else
      {
        ListItem empty = new ListItem("  ", "NotSelected");
        this.ddlShippingCountries.Items.Insert(0, empty);
      }
    }

    /// <summary>
    /// Fills the states.
    /// </summary>
    protected virtual void FillStates()
    {
      AddressInfo info = this.GetUserShippingInfo();
      if (info == null)
      {
        return;
      }

      Item countryItem = Sitecore.Context.Database.GetItem(((IEntity)info.Country).Alias);
      if (countryItem == null)
      {
        return;
      }

      foreach (Item state in countryItem.Children)
      {
        ListItem item = new ListItem(state["Name"], state["Name"]);
        if (info.State == state["Code"])
        {
          item.Selected = true;
        }

        this.ddlShippingStates.Items.Add(item);
      }
    }

    /// <summary>
    /// Gets the user country code.
    /// </summary>
    /// <returns>returns current user country code</returns>
    protected virtual AddressInfo GetUserShippingInfo()
    {
      return this.CustomerInfo.ShippingAddress;
    }

    /// <summary>
    /// Fills the shipping options.
    /// </summary>
    /// <exception cref="ArgumentException">List of shipping methods is empty.</exception>
    protected virtual void FillShippingOptions()
    {
      DomainModel.Carts.ShoppingCart shoppingCart = Sitecore.Ecommerce.Context.Entity.GetInstance<DomainModel.Carts.ShoppingCart>();
      AddressInfo info = this.GetUserShippingInfo();
      if (info != null)
      {
        IEntityProvider<ShippingProvider> shippingMethodProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IEntityProvider<ShippingProvider>>();
        Assert.IsNotNull(shippingMethodProvider, "Shipping methods provider is null");

        IEnumerable<ShippingProvider> methods = shippingMethodProvider.GetAll();

        if (!IsPostBack)
        {
          foreach (ShippingProvider method in methods)
          {
            ListItem item = new ListItem
            {
              Value = method.Code,
              Text = string.Format("{0} - {1}{2}", method.Name, this.GetCurrency(shoppingCart), method.Price)
            };

            if (method.Code == shoppingCart.ShippingProvider.Code)
            {
              item.Selected = true;
            }

            this.ddlShippingMethods.Items.Add(item);
          }

          if (shoppingCart.ShippingProvider != null && string.IsNullOrEmpty(shoppingCart.ShippingProvider.Code) && methods.Count() > 0)
          {
            shoppingCart.ShippingProvider = methods.FirstOrDefault();
            shoppingCart.ShippingPrice = shoppingCart.ShippingProvider.Price;
            Sitecore.Ecommerce.Context.Entity.SetInstance(shoppingCart);
          }
        }
        else
        {
          if (this.ddlShippingMethods.SelectedItem != null)
          {
            shoppingCart.ShippingProvider = methods.FirstOrDefault(m => m.Code == this.ddlShippingMethods.SelectedItem.Value);
            shoppingCart.ShippingPrice = shoppingCart.ShippingProvider.Price;
            Sitecore.Ecommerce.Context.Entity.SetInstance(shoppingCart);
          }
        }
      }
    }

    /// <summary>
    /// Gets the currency.
    /// </summary>
    /// <param name="shoppingCart">The shopping cart.</param>
    /// <returns>Returns currency</returns>
    protected virtual string GetCurrency(DomainModel.Carts.ShoppingCart shoppingCart)
    {
      GeneralSettings settings = Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>();
      if (settings != null && settings.DisplayCurrencyOnPrices)
      {
        return shoppingCart.Currency.Title;
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets the shippingdestination.
    /// </summary>
    /// <returns>retuns shpping information string</returns>
    protected virtual string GetShippingDestination()
    {
      AddressInfo info = this.GetUserShippingInfo();
      if (info != null)
      {
        StringBuilder stringInfo = new StringBuilder();
        if (info.Country != null)
        {
          stringInfo.Append(info.Country.Name);
        }

        if (!string.IsNullOrEmpty(info.State))
        {
          Item country = Sitecore.Context.Database.SelectSingleItem(info.Country.Code);
          if (country != null)
          {
            Item state = country.Children[info.State];
            if (state != null)
            {
              stringInfo.AppendFormat(", {0}", state["Code"]);
            }
          }
          else
          {
            stringInfo.AppendFormat(", {0}", info.State);
          }
        }

        if (!string.IsNullOrEmpty(info.City))
        {
          stringInfo.AppendFormat(", {0}", info.City);
        }

        return HttpUtility.HtmlEncode(stringInfo.ToString());
      }

      return string.Empty;
    }

    /// <summary>
    /// Displays the shipping method.
    /// </summary>
    private void DisplayShippingMethod()
    {
      if (this.DisplayMode == OrderDisplayMode.ShoppingCart)
      {
        this.lblShippingMethods.Visible = true;
        this.lblShippingMethod.Visible = false;
      }
      else if (this.DisplayMode == OrderDisplayMode.OrderConfirmation)
      {
        this.lblShippingMethod.Text = this.ShippingProvider.Name;
        this.lblShippingMethod.Visible = true;
        this.lblShippingMethods.Visible = true;
        this.lblShippingMethods.Text = Translate.Text(Texts.ShippingMethod);
        this.ddlShippingMethods.Visible = false;
        this.changeDestinationForm.Visible = false;
      }
    }

    #endregion
  }
}