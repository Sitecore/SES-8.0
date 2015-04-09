// -------------------------------------------------------------------------------------------
// <copyright file="OrderMappingRule.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Data
{
  using System;
  using System.Globalization;
  using System.Collections.Specialized;
  using System.Linq;
  using Diagnostics;
  using DomainModel.Addresses;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using DomainModel.Orders;
  using DomainModel.Payments;
  using DomainModel.Shippings;
  using Microsoft.Practices.Unity;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Utils;
  using Validators.Interception;

  /// <summary>
  /// The sitecore order.
  /// </summary>
  public class OrderMappingRule : IMappingRule<Order>
  {
    #region Implementation of IMappingRule<Order>

    /// <summary>
    /// Gets or sets the mapping object.
    /// </summary>
    /// <value>The mapping object.</value>
    public Order MappingObject { get; [NotNullValue] set; }

    #endregion

    /// <summary>
    /// Gets or sets the order number.
    /// </summary>
    /// <value>The order number.</value>
    [Entity(FieldName = "OrderNumber")]
    public virtual string OrderNumber
    {
      get
      {
        return this.MappingObject.OrderNumber;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.OrderNumber = value;
      }
    }

    /// <summary>
    /// Gets or sets the order date.
    /// </summary>
    /// <value>The order date.</value>
    [Entity(FieldName = "OrderDate")]
    public virtual string OrderDate
    {
      get
      {
        return DateUtil.ToIsoDate(this.MappingObject.OrderDate);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.OrderDate = DateUtil.IsoDateToDateTime(value);
      }
    }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    [Entity(FieldName = "Status")]
    public virtual string Status
    {
      get
      {
        if (this.MappingObject.Status == null)
        {
          return string.Empty;
        }

        Type currentStatusType = this.MappingObject.Status.GetType();

        string statusCode = currentStatusType.Name;
        
        using (new SiteIndependentDatabaseSwitcher(Sitecore.Context.ContentDatabase))
        {
          IEntityProvider<OrderStatus> provider = Context.Entity.Resolve<IEntityProvider<OrderStatus>>();
          OrderStatus orderStatus = provider.Get(statusCode);

          if (orderStatus == default(OrderStatus) || !(orderStatus is IEntity))
          {
            return string.Empty;
          }

          return ((IEntity)orderStatus).Alias;
        }
      }

      [NotNullValue]
      set
      {
        if (!ID.IsID(value))
        {
          return;
        }

        Item orderStatusItem = Sitecore.Context.ContentDatabase.GetItem(value);
        Assert.IsNotNull(orderStatusItem, string.Concat("Order status item is null. ID: '", value, "'"));

        OrderStatus orderStatus;
        IDataMapper dataMapper = Context.Entity.Resolve<IDataMapper>();
        
        using (new SiteIndependentDatabaseSwitcher(Sitecore.Context.ContentDatabase))
        {
          var tempStatus = Context.Entity.Resolve<OrderStatus>(orderStatusItem["Code"]);
          orderStatus = dataMapper.GetEntity(orderStatusItem, tempStatus.GetType()) as OrderStatus;
        }

        this.MappingObject.Status = orderStatus;
      }
    }

    /// <summary>
    /// Gets or sets the tracking number.
    /// </summary>
    /// <value>The tracking number.</value>
    [Entity(FieldName = "TrackingNumber")]
    public virtual string TrackingNumber
    {
      get
      {
        return this.MappingObject.TrackingNumber;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.TrackingNumber = value;
      }
    }

    /// <summary>
    /// Gets or sets the transaction number.
    /// </summary>
    /// <value>The transaction number.</value>
    [Entity(FieldName = "TransactionNumber")]
    public virtual string TransactionNumber
    {
      get
      {
        return this.MappingObject.TransactionNumber;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.TransactionNumber = value;
      }
    }

    /// <summary>
    /// Gets or sets the payment method.
    /// </summary>
    /// <value>The payment method.</value>
    [Entity(FieldName = "PaymentSystem")]
    public virtual string PaymentSystem
    {
      get
      {
        IEntity entity = this.MappingObject.PaymentSystem as IEntity;
        return entity != null ? entity.Alias : string.Empty;
      }

      [NotNullValue]
      set
      {
        if (!ID.IsID(value))
        {
          return;
        }

        using (new SiteIndependentDatabaseSwitcher(Sitecore.Context.ContentDatabase))
        {
          IEntityProvider<PaymentSystem> provider = Context.Entity.Resolve<IEntityProvider<PaymentSystem>>();
          this.MappingObject.PaymentSystem = provider.Get(value);
        }
      }
    }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    /// <value>The currency.</value>
    [Entity(FieldName = "Currency")]
    public virtual string Currency
    {
      get
      {
        IEntity entity = this.MappingObject.Currency as IEntity;
        return entity != null ? entity.Alias : string.Empty;
      }

      [NotNullValue]
      set
      {
        if (!ID.IsID(value))
        {
          return;
        }

        using (new SiteIndependentDatabaseSwitcher(Sitecore.Context.ContentDatabase))
        {
          IEntityProvider<Currency> provider = Context.Entity.Resolve<IEntityProvider<Currency>>();
          this.MappingObject.Currency = provider.Get(value);
        }
      }
    }

    /// <summary>
    /// Gets or sets the discount code.
    /// </summary>
    /// <value>The discount code.</value>
    [Entity(FieldName = "DiscountCode")]
    public virtual string DiscountCode
    {
      get
      {
        return this.MappingObject.DiscountCode;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.DiscountCode = value;
      }
    }

    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    /// <value>The comment.</value>
    [Entity(FieldName = "Comment")]
    public virtual string Comment
    {
      get
      {
        return this.MappingObject.Comment;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Comment = value;
      }
    }

    /// <summary>
    /// Gets or sets the shipping provider.
    /// </summary>
    /// <value>The shipping provider.</value>
    [Entity(FieldName = "ShippingProvider")]
    public virtual string ShippingProvider
    {
      get
      {
        IEntity entity = this.MappingObject.ShippingProvider as IEntity;
        return entity != null ? entity.Alias : string.Empty;
      }

      [NotNullValue]
      set
      {
        if (!ID.IsID(value))
        {
          return;
        }

        using (new SiteIndependentDatabaseSwitcher(Sitecore.Context.ContentDatabase))
        {
          IEntityProvider<ShippingProvider> provider = Context.Entity.Resolve<IEntityProvider<ShippingProvider>>();
          this.MappingObject.ShippingProvider = provider.Get(value);
        }
      }
    }

    /// <summary>
    /// Gets or sets the shipping price.
    /// </summary>
    /// <value>The shipping price.</value>
    [Entity(FieldName = "Shipping Price")]
    public decimal ShippingPrice
    {
      get { return this.MappingObject.ShippingPrice; }
      set { this.MappingObject.ShippingPrice = value; }
    }

    /// <summary>
    /// Gets or sets the notification option.
    /// </summary>
    /// <value>The notification option.</value>
    [Entity(FieldName = "NotificationOption")]
    public virtual string NotificationOption
    {
      get
      {
        IEntity entity = this.MappingObject.NotificationOption as IEntity;
        return entity != null ? entity.Alias : string.Empty;
      }

      [NotNullValue]
      set
      {
        if (!ID.IsID(value))
        {
          return;
        }

        using (new SiteIndependentDatabaseSwitcher(Sitecore.Context.ContentDatabase))
        {
          IEntityProvider<NotificationOption> provider = Context.Entity.Resolve<IEntityProvider<NotificationOption>>();
          this.MappingObject.NotificationOption = provider.Get(value);
        }
      }
    }

    /// <summary>
    /// Gets or sets the notification option value.
    /// </summary>
    /// <value>The notification option value.</value>
    [Entity(FieldName = "NotificationOptionValue")]
    public virtual string NotificationOptionValue
    {
      get
      {
        return this.MappingObject.NotificationOptionValue;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.NotificationOptionValue = value;
      }
    }

    /// <summary>
    /// Gets or sets the customer id.
    /// </summary>
    /// <value>The customer id.</value>
    [Entity(FieldName = "CustomerId")]
    public virtual string CustomerId
    {
      get
      {
        return this.MappingObject.CustomerInfo.CustomerId;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.CustomerId = value;
      }
    }

    /// <summary>
    /// Gets or sets the account id.
    /// </summary>
    /// <value>The account id.</value>
    [Entity(FieldName = "AccountId")]
    public virtual string AccountId
    {
      get
      {
        return this.MappingObject.CustomerInfo.AccountId;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.AccountId = value;
      }
    }

    /// <summary>
    /// Gets or sets the type of the account.
    /// </summary>
    /// <value>The type of the account.</value>
    [Entity(FieldName = "AccountType")]
    public virtual string AccountType
    {
      get
      {
        return this.MappingObject.CustomerInfo.AccountType;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.AccountType = value;
      }
    }

    /// <summary>
    /// Gets or sets the sitecore user.
    /// </summary>
    /// <value>The sitecore user.</value>
    [Entity(FieldName = "SitecoreUser")]
    public virtual string SitecoreUser
    {
      get
      {
        return this.MappingObject.CustomerInfo.NickName;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.NickName = value;
      }
    }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [Entity(FieldName = "Email")]
    public virtual string Email
    {
      get
      {
        return this.MappingObject.CustomerInfo.Email;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.Email = value;
      }
    }

    /// <summary>
    /// Gets or sets the email2.
    /// </summary>
    /// <value>The email2.</value>
    [Entity(FieldName = "Email2")]
    public virtual string Email2
    {
      get
      {
        return this.MappingObject.CustomerInfo.Email2;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.Email2 = value;
      }
    }

    /// <summary>
    /// Gets or sets the phone.
    /// </summary>
    /// <value>The phone.</value>
    [Entity(FieldName = "Phone")]
    public virtual string Phone
    {
      get
      {
        return this.MappingObject.CustomerInfo.Phone;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.Phone = value;
      }
    }

    /// <summary>
    /// Gets or sets the phone2.
    /// </summary>
    /// <value>The phone2.</value>
    [Entity(FieldName = "Phone2")]
    public virtual string Phone2
    {
      get
      {
        return this.MappingObject.CustomerInfo.Phone;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.Phone = value;
      }
    }

    /// <summary>
    /// Gets or sets the mobile.
    /// </summary>
    /// <value>The mobile.</value>
    [Entity(FieldName = "Mobile")]
    public virtual string Mobile
    {
      get
      {
        return this.MappingObject.CustomerInfo.Mobile;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.Mobile = value;
      }
    }

    /// <summary>
    /// Gets or sets the fax.
    /// </summary>
    /// <value>The fax number.</value>
    [Entity(FieldName = "Fax")]
    public virtual string Fax
    {
      get
      {
        return this.MappingObject.CustomerInfo.Fax;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.Fax = value;
      }
    }

    /// <summary>
    /// Gets or sets the name of the billing.
    /// </summary>
    /// <value>The name of the billing.</value>
    [Entity(FieldName = "BillingName")]
    public virtual string BillingName
    {
      get
      {
        return this.MappingObject.CustomerInfo.BillingAddress.Name;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.BillingAddress.Name = value;
      }
    }

    /// <summary>
    /// Gets or sets the billing name2.
    /// </summary>
    /// <value>The billing name2.</value>
    [Entity(FieldName = "BillingName2")]
    public virtual string BillingName2
    {
      get
      {
        return this.MappingObject.CustomerInfo.BillingAddress.Name2;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.BillingAddress.Name2 = value;
      }
    }

    /// <summary>
    /// Gets or sets the billing address.
    /// </summary>
    /// <value>The billing address.</value>
    [Entity(FieldName = "BillingAddress")]
    public virtual string BillingAddress
    {
      get
      {
        return this.MappingObject.CustomerInfo.BillingAddress.Address;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.BillingAddress.Address = value;
      }
    }

    /// <summary>
    /// Gets or sets the billing address2.
    /// </summary>
    /// <value>The billing address2.</value>
    [Entity(FieldName = "BillingAddress2")]
    public virtual string BillingAddress2
    {
      get
      {
        return this.MappingObject.CustomerInfo.BillingAddress.Address2;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.BillingAddress.Address2 = value;
      }
    }

    /// <summary>
    /// Gets or sets the billing zip.
    /// </summary>
    /// <value>The billing zip.</value>
    [Entity(FieldName = "BillingZip")]
    public virtual string BillingZip
    {
      get
      {
        return this.MappingObject.CustomerInfo.BillingAddress.Zip;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.BillingAddress.Zip = value;
      }
    }

    /// <summary>
    /// Gets or sets the billing city.
    /// </summary>
    /// <value>The billing city.</value>
    [Entity(FieldName = "BillingCity")]
    public virtual string BillingCity
    {
      get
      {
        return this.MappingObject.CustomerInfo.BillingAddress.City;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.BillingAddress.City = value;
      }
    }

    /// <summary>
    /// Gets or sets the state of the billing.
    /// </summary>
    /// <value>The state of the billing.</value>
    [Entity(FieldName = "BillingState")]
    public virtual string BillingState
    {
      get
      {
        return this.MappingObject.CustomerInfo.BillingAddress.State;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.BillingAddress.State = value;
      }
    }

    /// <summary>
    /// Gets or sets the billing country.
    /// </summary>
    /// <value>The billing country.</value>
    [Entity(FieldName = "BillingCountry")]
    public virtual string BillingCountry
    {
      get
      {
        IEntity entity = this.MappingObject.CustomerInfo.BillingAddress.Country as IEntity;
        return entity != null ? entity.Alias : string.Empty;
      }

      [NotNullValue]
      set
      {
        if (!ID.IsID(value))
        {
          return;
        }

        using (new SiteIndependentDatabaseSwitcher(Sitecore.Context.ContentDatabase))
        {
          IEntityProvider<Country> provider = Context.Entity.Resolve<IEntityProvider<Country>>();
          this.MappingObject.CustomerInfo.BillingAddress.Country = provider.Get(value);
        }
      }
    }

    /// <summary>
    /// Gets or sets the name of the shipping.
    /// </summary>
    /// <value>The name of the shipping.</value>
    [Entity(FieldName = "ShippingName")]
    public virtual string ShippingName
    {
      get
      {
        return this.MappingObject.CustomerInfo.ShippingAddress.Name;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.ShippingAddress.Name = value;
      }
    }

    /// <summary>
    /// Gets or sets the shipping name2.
    /// </summary>
    /// <value>The shipping name2.</value>
    [Entity(FieldName = "ShippingName2")]
    public virtual string ShippingName2
    {
      get
      {
        return this.MappingObject.CustomerInfo.ShippingAddress.Name2;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.ShippingAddress.Name2 = value;
      }
    }

    /// <summary>
    /// Gets or sets the shipping address.
    /// </summary>
    /// <value>The shipping address.</value>
    [Entity(FieldName = "ShippingAddress")]
    public virtual string ShippingAddress
    {
      get
      {
        return this.MappingObject.CustomerInfo.ShippingAddress.Address;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.ShippingAddress.Address = value;
      }
    }

    /// <summary>
    /// Gets or sets the shipping address2.
    /// </summary>
    /// <value>The shipping address2.</value>
    [Entity(FieldName = "ShippingAddress2")]
    public virtual string ShippingAddress2
    {
      get
      {
        return this.MappingObject.CustomerInfo.ShippingAddress.Address2;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.ShippingAddress.Address2 = value;
      }
    }

    /// <summary>
    /// Gets or sets the shipping zip.
    /// </summary>
    /// <value>The shipping zip.</value>
    [Entity(FieldName = "ShippingZip")]
    public virtual string ShippingZip
    {
      get
      {
        return this.MappingObject.CustomerInfo.ShippingAddress.Zip;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.ShippingAddress.Zip = value;
      }
    }

    /// <summary>
    /// Gets or sets the shipping city.
    /// </summary>
    /// <value>The shipping city.</value>
    [Entity(FieldName = "ShippingCity")]
    public virtual string ShippingCity
    {
      get
      {
        return this.MappingObject.CustomerInfo.ShippingAddress.City;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.ShippingAddress.City = value;
      }
    }

    /// <summary>
    /// Gets or sets the state of the shipping.
    /// </summary>
    /// <value>The state of the shipping.</value>
    [Entity(FieldName = "ShippingState")]
    public virtual string ShippingState
    {
      get
      {
        return this.MappingObject.CustomerInfo.ShippingAddress.State;
      }

      [NotNullValue]
      set
      {
        this.MappingObject.CustomerInfo.ShippingAddress.State = value;
      }
    }

    /// <summary>
    /// Gets or sets the shipping country.
    /// </summary>
    /// <value>The shipping country.</value>
    [Entity(FieldName = "ShippingCountry")]
    public virtual string ShippingCountry
    {
      get
      {
        IEntity entity = this.MappingObject.CustomerInfo.ShippingAddress.Country as IEntity;
        return entity != null ? entity.Alias : string.Empty;
      }

      [NotNullValue]
      set
      {
        if (!ID.IsID(value))
        {
          return;
        }

        using (new SiteIndependentDatabaseSwitcher(Sitecore.Context.ContentDatabase))
        {
          IEntityProvider<Country> provider = Context.Entity.Resolve<IEntityProvider<Country>>();
          this.MappingObject.CustomerInfo.ShippingAddress.Country = provider.Get(value);
        }
      }
    }

    /// <summary>
    /// Gets or sets the items in shopping cart.
    /// </summary>
    /// <value>The items in shopping cart.</value>
    [Entity(FieldName = "ItemsInShoppingCart")]
    public virtual string ItemsInShoppingCart
    {
      get
      {
        uint itemsInShoppingCart = 0;

        if (this.MappingObject.OrderLines != null)
        {
          this.MappingObject.OrderLines.ToList().ForEach(p => itemsInShoppingCart += p.Quantity);
        }

        return itemsInShoppingCart.ToString();
      }

      [NotNullValue]
      set { return; }
    }

    /// <summary>
    /// Gets or sets the vat.
    /// </summary>
    /// <value>The vat price.</value>
    [Entity(FieldName = "Vat")]
    public virtual string Vat
    {
      get
      {
        return this.MappingObject.Totals.VAT.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.VAT = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the Total vat.
    /// </summary>
    /// <value>The toatal vat price.</value>
    [Entity(FieldName = "TotalVat")]
    public virtual string TotalVat
    {
      get
      {
        return this.MappingObject.Totals.TotalVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.TotalVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the price ex vat.
    /// </summary>
    /// <value>The price ex vat.</value>
    [Entity(FieldName = "PriceExVat")]
    public virtual string PriceExVat
    {
      get
      {
        return this.MappingObject.Totals.PriceExVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.PriceExVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the price inc vat.
    /// </summary>
    /// <value>The price inc vat.</value>
    [Entity(FieldName = "PriceIncVat")]
    public virtual string PriceIncVat
    {
      get
      {
        return this.MappingObject.Totals.PriceIncVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.PriceIncVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the discount ex vat.
    /// </summary>
    /// <value>The discount ex vat.</value>
    [Entity(FieldName = "DiscountExVat")]
    public virtual string DiscountExVat
    {
      get
      {
        return this.MappingObject.Totals.DiscountExVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.DiscountExVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the discount inc vat.
    /// </summary>
    /// <value>The discount inc vat.</value>
    [Entity(FieldName = "DiscountIncVat")]
    public virtual string DiscountIncVat
    {
      get
      {
        return this.MappingObject.Totals.DiscountIncVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.DiscountIncVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the possible discount ex vat.
    /// </summary>
    /// <value>The possible discount ex vat.</value>
    [Entity(FieldName = "PossibleDiscountExVat")]
    public virtual string PossibleDiscountExVat
    {
      get
      {
        return this.MappingObject.Totals.PossibleDiscountExVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.PossibleDiscountExVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the possible discount inc vat.
    /// </summary>
    /// <value>The possible discount inc vat.</value>
    [Entity(FieldName = "PossibleDiscountIncVat")]
    public virtual string PossibleDiscountIncVat
    {
      get
      {
        return this.MappingObject.Totals.PossibleDiscountIncVat.ToString(CultureInfo.InvariantCulture);
      }

      [NotNullValue]
      set
      {
        this.MappingObject.Totals.PossibleDiscountIncVat = TypeUtil.TryParse(value, decimal.Zero);
      }
    }

    /// <summary>
    /// Gets or sets the order number.
    /// </summary>
    /// <value>The order number.</value>
    [Entity(FieldName = "AuthorizationCode")]
    public virtual string AuthorizationCode
    {
      get
      {
        return this.MappingObject.AuthorizationCode;
  }

      [NotNullValue]
      set
      {
        this.MappingObject.AuthorizationCode = value;
      }
    }
  }
}