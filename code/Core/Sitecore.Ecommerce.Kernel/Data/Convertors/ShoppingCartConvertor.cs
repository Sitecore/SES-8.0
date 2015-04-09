// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartConvertor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ShoppingCartConvertor type.
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

namespace Sitecore.Ecommerce.Data.Convertors
{
  using System.Data;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.DomainModel.Carts;
  using Sitecore.Ecommerce.DomainModel.Data;

  /// <summary>
  /// Converts the ShoppingCart to DataRow and vice-versa
  /// </summary>
  public class ShoppingCartConvertor : Convertor<ShoppingCart>
  {
    /// <summary>
    /// The shopping cart line convertor
    /// </summary>
    private static readonly ShoppingCartLineConvertor ShoppingCartLineConvertor = new ShoppingCartLineConvertor();

    /// <summary>
    /// The totals convertor
    /// </summary>
    private static readonly KeyValuePairConvertor<string, decimal> TotalsConvertor = new KeyValuePairConvertor<string, decimal>();

    /// <summary>
    /// Puts the content of the ShoppingCart to the DataRow
    /// </summary>
    /// <param name="model">
    /// The Shopping cart to store.
    /// </param>
    /// <param name="row">
    /// The row which contains the information.
    /// </param>
    public override void DomainModelToDTO(ShoppingCart model, ref DataRow row)
    {
      Assert.IsNotNull(model, "model must not be null");
      Assert.IsNotNull(row, "row must not be null");      

      if (model.Currency is IEntity)
      {
        row["Currency.Alias"] = ((IEntity)model.Currency).Alias;
      }
      
      row["Currency.Code"] = model.Currency.Code;
      row["Currency.Name"] = model.Currency.Name;
      row["Currency.Title"] = model.Currency.Title;

      if (model.CustomerInfo is IEntity)
      {
        row["CustomerInfo.Alias"] = ((IEntity)model.CustomerInfo).Alias;
      }

      row["CustomerInfo.AccountId"] = model.CustomerInfo.AccountId;
      row["CustomerInfo.AccountType"] = model.CustomerInfo.AccountType;

      if (model.CustomerInfo.BillingAddress is IEntity)
      {
        row["CustomerInfo.BillingAddress.Alias"] = ((IEntity)model.CustomerInfo.BillingAddress).Alias;
      }

      row["CustomerInfo.BillingAddress.Address"] = model.CustomerInfo.BillingAddress.Address;
      row["CustomerInfo.BillingAddress.Address2"] = model.CustomerInfo.BillingAddress.Address2;
      row["CustomerInfo.BillingAddress.City"] = model.CustomerInfo.BillingAddress.City;

      if (model.CustomerInfo.BillingAddress.Country is IEntity)
      {
        row["CustomerInfo.BillingAddress.Country.Alias"] = ((IEntity)model.CustomerInfo.BillingAddress.Country).Alias;
      }

      row["CustomerInfo.BillingAddress.Country.Code"] = model.CustomerInfo.BillingAddress.Country.Code;
      row["CustomerInfo.BillingAddress.Country.Name"] = model.CustomerInfo.BillingAddress.Country.Name;
      row["CustomerInfo.BillingAddress.Country.Title"] = model.CustomerInfo.BillingAddress.Country.Title;

      if (model.CustomerInfo.BillingAddress.Country.VatRegion is IEntity)
      {
        row["CustomerInfo.BillingAddress.Country.VatRegion.Alias"] = ((IEntity)model.CustomerInfo.BillingAddress.Country.VatRegion).Alias;
      }

      row["CustomerInfo.BillingAddress.Country.VatRegion.Code"] = model.CustomerInfo.BillingAddress.Country.VatRegion.Code;
      row["CustomerInfo.BillingAddress.Country.VatRegion.Name"] = model.CustomerInfo.BillingAddress.Country.VatRegion.Name;
      row["CustomerInfo.BillingAddress.Country.VatRegion.Title"] = model.CustomerInfo.BillingAddress.Country.VatRegion.Title;
      row["CustomerInfo.BillingAddress.Name"] = model.CustomerInfo.BillingAddress.Name;
      row["CustomerInfo.BillingAddress.Name2"] = model.CustomerInfo.BillingAddress.Name2;
      row["CustomerInfo.BillingAddress.State"] = model.CustomerInfo.BillingAddress.State;
      row["CustomerInfo.BillingAddress.Zip"] = model.CustomerInfo.BillingAddress.Zip;
      row["CustomerInfo.CustomerId"] = model.CustomerInfo.CustomerId;
      row["CustomerInfo.Email"] = model.CustomerInfo.Email;
      row["CustomerInfo.Email2"] = model.CustomerInfo.Email2;
      row["CustomerInfo.Fax"] = model.CustomerInfo.Fax;
      row["CustomerInfo.Mobile"] = model.CustomerInfo.Mobile;
      row["CustomerInfo.NickName"] = model.CustomerInfo.NickName;
      row["CustomerInfo.Phone"] = model.CustomerInfo.Phone;

      if (model.CustomerInfo.ShippingAddress is IEntity)
      {
        row["CustomerInfo.ShippingAddress.Alias"] = ((IEntity)model.CustomerInfo.ShippingAddress).Alias;
      }

      row["CustomerInfo.ShippingAddress.Address"] = model.CustomerInfo.ShippingAddress.Address;
      row["CustomerInfo.ShippingAddress.Address2"] = model.CustomerInfo.ShippingAddress.Address2;
      row["CustomerInfo.ShippingAddress.City"] = model.CustomerInfo.ShippingAddress.City;

      if (model.CustomerInfo.ShippingAddress.Country is IEntity)
      {
        row["CustomerInfo.ShippingAddress.Country.Alias"] = ((IEntity)model.CustomerInfo.ShippingAddress.Country).Alias;
      }

      row["CustomerInfo.ShippingAddress.Country.Code"] = model.CustomerInfo.ShippingAddress.Country.Code;
      row["CustomerInfo.ShippingAddress.Country.Name"] = model.CustomerInfo.ShippingAddress.Country.Name;
      row["CustomerInfo.ShippingAddress.Country.Title"] = model.CustomerInfo.ShippingAddress.Country.Title;

      if (model.CustomerInfo.ShippingAddress.Country.VatRegion is IEntity)
      {
        row["CustomerInfo.ShippingAddress.Country.VatRegion.Alias"] = ((IEntity)model.CustomerInfo.ShippingAddress.Country.VatRegion).Alias;
      }

      row["CustomerInfo.ShippingAddress.Country.VatRegion.Code"] = model.CustomerInfo.ShippingAddress.Country.VatRegion.Code;
      row["CustomerInfo.ShippingAddress.Country.VatRegion.Name"] = model.CustomerInfo.ShippingAddress.Country.VatRegion.Name;
      row["CustomerInfo.ShippingAddress.Country.VatRegion.Title"] = model.CustomerInfo.ShippingAddress.Country.VatRegion.Title;
      row["CustomerInfo.ShippingAddress.Name"] = model.CustomerInfo.ShippingAddress.Name;
      row["CustomerInfo.ShippingAddress.Name2"] = model.CustomerInfo.ShippingAddress.Name2;
      row["CustomerInfo.ShippingAddress.State"] = model.CustomerInfo.ShippingAddress.State;
      row["CustomerInfo.ShippingAddress.Zip"] = model.CustomerInfo.ShippingAddress.Zip;

      if (model.NotificationOption is IEntity)
      {
        row["NotificationOption.Alias"] = ((IEntity)model.NotificationOption).Alias;
      }

      row["NotificationOption.Code"] = model.NotificationOption.Code;
      row["NotificationOption.Name"] = model.NotificationOption.Name;
      row["NotificationOption.Title"] = model.NotificationOption.Title;
      row["NotificationOptionValue"] = model.NotificationOptionValue;
      row["OrderNumber"] = model.OrderNumber;

      if (model.PaymentSystem is IEntity)
      {
        row["PaymentSystem.Alias"] = ((IEntity)model.PaymentSystem).Alias;
      }

      row["PaymentSystem.Code"] = model.PaymentSystem.Code;
      row["PaymentSystem.Description"] = model.PaymentSystem.Description;
      row["PaymentSystem.LogoUrl"] = model.PaymentSystem.LogoUrl;
      row["PaymentSystem.Password"] = model.PaymentSystem.Password;
      row["PaymentSystem.PaymentSecondaryUrl"] = model.PaymentSystem.PaymentSecondaryUrl;
      row["PaymentSystem.PaymentSettings"] = model.PaymentSystem.PaymentSettings;
      row["PaymentSystem.PaymentUrl"] = model.PaymentSystem.PaymentUrl;
      row["PaymentSystem.Title"] = model.PaymentSystem.Title;
      row["PaymentSystem.Username"] = model.PaymentSystem.Username;
      row["ShippingPrice"] = model.ShippingPrice;

      if (model.ShippingProvider is IEntity)
      {
        row["ShippingProvider.Alias"] = ((IEntity)model.ShippingProvider).Alias;
      }

      row["ShippingProvider.AvailableNotificationOptions"] = model.ShippingProvider.AvailableNotificationOptions;
      row["ShippingProvider.Code"] = model.ShippingProvider.Code;
      row["ShippingProvider.Description"] = model.ShippingProvider.Description;
      row["ShippingProvider.IconUrl"] = model.ShippingProvider.IconUrl;
      row["ShippingProvider.Name"] = model.ShippingProvider.Name;
      row["ShippingProvider.Price"] = model.ShippingProvider.Price;
      row["ShippingProvider.Title"] = model.ShippingProvider.Title;
      row["ShoppingCartLines"] = ShoppingCartLineConvertor.DomainModelToDTO(model.ShoppingCartLines);
      row["Totals"] = TotalsConvertor.DomainModelToDTO(model.Totals);
    }

    /// <summary>
    /// Restores the ShoppingCart from the DataRow
    /// </summary>
    /// <param name="row">
    /// The row to retrieve information from.
    /// </param>
    /// <param name="model">
    /// The model to restore.
    /// </param>
    public override void DTOToDomainModel(DataRow row, ref ShoppingCart model)
    {
      Assert.IsNotNull(model, "model must not be null");
      Assert.IsNotNull(row, "row must not be null");

      if (model.Currency is IEntity)
      {
        ((IEntity)model.Currency).Alias = this.ConvertDataRowValue<string>(row["Currency.Alias"]);
      }
      
      model.Currency.Code = this.ConvertDataRowValue<string>(row["Currency.Code"]);
      model.Currency.Name = this.ConvertDataRowValue<string>(row["Currency.Name"]);
      model.Currency.Title = this.ConvertDataRowValue<string>(row["Currency.Title"]);

      if (model.CustomerInfo is IEntity)
      {
        ((IEntity)model.CustomerInfo).Alias = this.ConvertDataRowValue<string>(row["CustomerInfo.Alias"]);
      }

      model.CustomerInfo.AccountId = this.ConvertDataRowValue<string>(row["CustomerInfo.AccountId"]);
      model.CustomerInfo.AccountType = this.ConvertDataRowValue<string>(row["CustomerInfo.AccountType"]);

      if (model.CustomerInfo.BillingAddress is IEntity)
      {
        ((IEntity)model.CustomerInfo.BillingAddress).Alias = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Alias"]);
      }

      model.CustomerInfo.BillingAddress.Address = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Address"]);
      model.CustomerInfo.BillingAddress.Address2 = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Address2"]);
      model.CustomerInfo.BillingAddress.City = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.City"]);

      if (model.CustomerInfo.BillingAddress.Country is IEntity)
      {
        ((IEntity)model.CustomerInfo.BillingAddress.Country).Alias = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Country.Alias"]);
      }

      model.CustomerInfo.BillingAddress.Country.Code = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Country.Code"]);
      model.CustomerInfo.BillingAddress.Country.Name = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Country.Name"]);
      model.CustomerInfo.BillingAddress.Country.Title = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Country.Title"]);

      if (model.CustomerInfo.BillingAddress.Country.VatRegion is IEntity)
      {
        ((IEntity)model.CustomerInfo.BillingAddress.Country.VatRegion).Alias = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Country.VatRegion.Alias"]);
      }

      model.CustomerInfo.BillingAddress.Country.VatRegion.Code = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Country.VatRegion.Code"]);
      model.CustomerInfo.BillingAddress.Country.VatRegion.Name = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Country.VatRegion.Name"]);
      model.CustomerInfo.BillingAddress.Country.VatRegion.Title = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Country.VatRegion.Title"]);
      model.CustomerInfo.BillingAddress.Name = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Name"]);
      model.CustomerInfo.BillingAddress.Name2 = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Name2"]);
      model.CustomerInfo.BillingAddress.State = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.State"]);
      model.CustomerInfo.BillingAddress.Zip = this.ConvertDataRowValue<string>(row["CustomerInfo.BillingAddress.Zip"]);
      model.CustomerInfo.CustomerId = this.ConvertDataRowValue<string>(row["CustomerInfo.CustomerId"]);
      model.CustomerInfo.Email = this.ConvertDataRowValue<string>(row["CustomerInfo.Email"]);
      model.CustomerInfo.Email2 = this.ConvertDataRowValue<string>(row["CustomerInfo.Email2"]);
      model.CustomerInfo.Fax = this.ConvertDataRowValue<string>(row["CustomerInfo.Fax"]);
      model.CustomerInfo.Mobile = this.ConvertDataRowValue<string>(row["CustomerInfo.Mobile"]);
      model.CustomerInfo.NickName = this.ConvertDataRowValue<string>(row["CustomerInfo.NickName"]);
      model.CustomerInfo.Phone = this.ConvertDataRowValue<string>(row["CustomerInfo.Phone"]);

      if (model.CustomerInfo.ShippingAddress is IEntity)
      {
        ((IEntity)model.CustomerInfo.ShippingAddress).Alias = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Alias"]);
      }

      model.CustomerInfo.ShippingAddress.Address = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Address"]);
      model.CustomerInfo.ShippingAddress.Address2 = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Address2"]);
      model.CustomerInfo.ShippingAddress.City = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.City"]);

      if (model.CustomerInfo.ShippingAddress.Country is IEntity)
      {
        ((IEntity)model.CustomerInfo.ShippingAddress.Country).Alias = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Country.Alias"]);
      }

      model.CustomerInfo.ShippingAddress.Country.Code = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Country.Code"]);
      model.CustomerInfo.ShippingAddress.Country.Name = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Country.Name"]);
      model.CustomerInfo.ShippingAddress.Country.Title = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Country.Title"]);

      if (model.CustomerInfo.ShippingAddress.Country.VatRegion is IEntity)
      {
        ((IEntity)model.CustomerInfo.ShippingAddress.Country.VatRegion).Alias = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Country.VatRegion.Alias"]);
      }

      model.CustomerInfo.ShippingAddress.Country.VatRegion.Code = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Country.VatRegion.Code"]);
      model.CustomerInfo.ShippingAddress.Country.VatRegion.Name = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Country.VatRegion.Name"]);
      model.CustomerInfo.ShippingAddress.Country.VatRegion.Title = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Country.VatRegion.Title"]);
      model.CustomerInfo.ShippingAddress.Name = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Name"]);
      model.CustomerInfo.ShippingAddress.Name2 = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Name2"]);
      model.CustomerInfo.ShippingAddress.State = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.State"]);
      model.CustomerInfo.ShippingAddress.Zip = this.ConvertDataRowValue<string>(row["CustomerInfo.ShippingAddress.Zip"]);

      if (model.NotificationOption is IEntity)
      {
        ((IEntity)model.NotificationOption).Alias = this.ConvertDataRowValue<string>(row["NotificationOption.Alias"]);
      }

      model.NotificationOption.Code = this.ConvertDataRowValue<string>(row["NotificationOption.Code"]);
      model.NotificationOption.Name = this.ConvertDataRowValue<string>(row["NotificationOption.Name"]);
      model.NotificationOption.Title = this.ConvertDataRowValue<string>(row["NotificationOption.Title"]);
      model.NotificationOptionValue = this.ConvertDataRowValue<string>(row["NotificationOptionValue"]);
      model.OrderNumber = this.ConvertDataRowValue<string>(row["OrderNumber"]);

      if (model.PaymentSystem is IEntity)
      {
        ((IEntity)model.PaymentSystem).Alias = this.ConvertDataRowValue<string>(row["PaymentSystem.Alias"]);
      }

      model.PaymentSystem.Code = this.ConvertDataRowValue<string>(row["PaymentSystem.Code"]);
      model.PaymentSystem.Description = this.ConvertDataRowValue<string>(row["PaymentSystem.Description"]);
      model.PaymentSystem.LogoUrl = this.ConvertDataRowValue<string>(row["PaymentSystem.LogoUrl"]);
      model.PaymentSystem.Password = this.ConvertDataRowValue<string>(row["PaymentSystem.Password"]);
      model.PaymentSystem.PaymentSecondaryUrl = this.ConvertDataRowValue<string>(row["PaymentSystem.PaymentSecondaryUrl"]);
      model.PaymentSystem.PaymentSettings = this.ConvertDataRowValue<string>(row["PaymentSystem.PaymentSettings"]);
      model.PaymentSystem.PaymentUrl = this.ConvertDataRowValue<string>(row["PaymentSystem.PaymentUrl"]);
      model.PaymentSystem.Title = this.ConvertDataRowValue<string>(row["PaymentSystem.Title"]);
      model.PaymentSystem.Username = this.ConvertDataRowValue<string>(row["PaymentSystem.Username"]);
      model.ShippingPrice = this.ConvertDataRowValue<decimal>(row["ShippingPrice"]);

      if (model.ShippingProvider is IEntity)
      {
        ((IEntity)model.ShippingProvider).Alias = this.ConvertDataRowValue<string>(row["ShippingProvider.Alias"]);
      }

      model.ShippingProvider.AvailableNotificationOptions = this.ConvertDataRowValue<string>(row["ShippingProvider.AvailableNotificationOptions"]);
      model.ShippingProvider.Code = this.ConvertDataRowValue<string>(row["ShippingProvider.Code"]);
      model.ShippingProvider.Description = this.ConvertDataRowValue<string>(row["ShippingProvider.Description"]);
      model.ShippingProvider.IconUrl = this.ConvertDataRowValue<string>(row["ShippingProvider.IconUrl"]);
      model.ShippingProvider.Name = this.ConvertDataRowValue<string>(row["ShippingProvider.Name"]);
      model.ShippingProvider.Price = this.ConvertDataRowValue<decimal>(row["ShippingProvider.Price"]);
      model.ShippingProvider.Title = this.ConvertDataRowValue<string>(row["ShippingProvider.Title"]);

      foreach (ShoppingCartLine shoppingCartLine in ShoppingCartLineConvertor.DTOToDomainModel((DataTable)row["ShoppingCartLines"]))
      {
        model.ShoppingCartLines.Add(shoppingCartLine);
      }
    }

    /// <summary>
    /// Creates an instance of ShoppingCart
    /// </summary>
    /// <returns>
    /// New ShoppingCar instance
    /// </returns>
    public override ShoppingCart GetInstance()
    {
      return Context.Entity.Resolve<ShoppingCart>();
    }
  }
}
