// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransientOrderConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order converter class.
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

namespace Sitecore.Ecommerce.Visitor.OrderManagement.Transient
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Common;
  using Configurations;
  using Diagnostics;
  using DomainModel.Addresses;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using DomainModel.Payments;
  using DomainModel.Prices;
  using DomainModel.Shippings;
  using DomainModel.Users;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Orders.Statuses;
  using Products;

  using Sitecore.Ecommerce.Payments;

  using PaymentSystem = Sitecore.Ecommerce.DomainModel.Payments.PaymentSystem;

  /// <summary>
  /// Defines the order converter class.
  /// </summary>
  public class TransientOrderConverter
  {
    /// <summary>
    /// Default allowance charge reason code.
    /// </summary>
    private const string AllowanceChargeReasonCode = "ZZZ";

    /// <summary>
    /// Default allowance charge reason.
    /// </summary>
    private const string AllowanceChargeReason = "Freight";

    /// <summary>
    /// Default notification option.
    /// </summary>
    private const string DefaultNotificationOption = "Email";

    /// <summary>
    /// Gets or sets the order state configuration.
    /// </summary>
    /// <value>The order state configuration.</value>
    public CoreOrderStateConfiguration OrderStateConfiguration { get; set; }

    /// <summary>
    /// Gets or sets the company master data.
    /// </summary>
    [CanBeNull]
    public CompanyMasterData CompanyMasterData { get; set; }

    /// <summary>
    /// Gets or sets CountryProvider.
    /// </summary>
    [CanBeNull]
    public IEntityProvider<Country> CountryProvider { get; set; }

    /// <summary>
    /// Gets or sets NotificationOptionProvider.
    /// </summary>
    [CanBeNull]
    public IEntityProvider<NotificationOption> NotificationOptionProvider { get; set; }

    /// <summary>
    /// Gets or sets ShippingProvider.
    /// </summary>
    [CanBeNull]
    public IEntityProvider<ShippingProvider> ShippingProvider { get; set; }

    /// <summary>
    /// Gets or sets the payment provider factory.
    /// </summary>
    /// <value>
    /// The payment provider factory.
    /// </value>
    public PaymentProviderFactory PaymentProviderFactory { get; set; }

    /// <summary>
    /// Converts the specified source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>
    /// The order.
    /// </returns>
    [NotNull]
    public virtual Order Convert([NotNull] DomainModel.Orders.Order source)
    {
      Assert.ArgumentNotNull(source, "source");

      Order destination = new Order();

      const int Period = 7;
      DateTime tempEndDate = source.OrderDate.AddDays(Period);
      destination.IssueDate = source.OrderDate;
      destination.OrderId = source.OrderNumber;

      destination.State = new State();
      destination.PaymentMeans = new PaymentMeans
      {
        PaymentChannelCode = source.PaymentSystem.Code,
        PaymentDueDate = source.OrderDate,
        PaymentID = source.TransactionNumber,
        PaymentMeansCode = source.CustomerInfo.CustomProperties[TransactionConstants.CardType]
      };
      destination.Note = source.Comment;
      destination.PricingCurrencyCode = source.Currency.Code;
      destination.TaxCurrencyCode = source.Currency.Code;
      destination.TaxTotal = new TaxTotal { RoundingAmount = new Amount(0, source.Currency.Code) };
      destination.DestinationCountryCode = source.CustomerInfo.BillingAddress.Country.Code;

      this.MapAmounts(source, destination);

      destination.BuyerCustomerParty = new CustomerParty
      {
        Party = new Party
        {
          Contact = new Contact
          {
            Name = source.CustomerInfo.BillingAddress.Name,
            ElectronicMail = source.CustomerInfo.Email,
            Telefax = source.CustomerInfo.Fax,
            Telephone = source.CustomerInfo.Phone,
          },
          PostalAddress = new Address
          {
            StreetName = source.CustomerInfo.BillingAddress.Address,
            PostalZone = source.CustomerInfo.BillingAddress.Zip,
            CityName = source.CustomerInfo.BillingAddress.City,
            CountrySubentity = source.CustomerInfo.BillingAddress.State,
            Country = source.CustomerInfo.BillingAddress.Country.Code,
            AddressTypeCode = string.Empty
          },
          PartyName = source.CustomerInfo.BillingAddress.Name,
          LanguageCode = Sitecore.Context.Language.Name,
          Person = new Person(),
        },

        SupplierAssignedAccountID = source.CustomerInfo.CustomerId
      };
      destination.BuyerCustomerParty.Party.Contact.OtherCommunications = new List<Communication>
      {
        new Communication
        {
          Channel = source.CustomerInfo.Email2,
          Value = source.CustomerInfo.Email2
        },
        new Communication
        {
          Channel = source.CustomerInfo.Mobile,
          Value = source.CustomerInfo.Mobile
        }
      };
      destination.AccountingCustomerParty = new CustomerParty
      {
        Party = new Party
        {
          Contact = new Contact
          {
            Name = source.CustomerInfo.BillingAddress.Name,
            ElectronicMail = source.CustomerInfo.Email
          },
          PostalAddress = new Address
          {
            StreetName = source.CustomerInfo.BillingAddress.Address,
            PostalZone = source.CustomerInfo.BillingAddress.Zip,
            CityName = source.CustomerInfo.BillingAddress.City,
            CountrySubentity = source.CustomerInfo.BillingAddress.State,
            Country = source.CustomerInfo.BillingAddress.Country.Code,
            AddressTypeCode = string.Empty
          },
          PartyName = source.CustomerInfo.BillingAddress.Name,
          Person = new Person()
        }
      };
      destination.Delivery = new List<Delivery>(1);
      Delivery delivery = new Delivery
      {
        DeliveryParty = new Party
        {
          Contact = new Contact
          {
            Name = source.CustomerInfo.ShippingAddress.Name,
          },

          PostalAddress = new Address
          {
            StreetName = source.CustomerInfo.ShippingAddress.Address,
            PostalZone = source.CustomerInfo.ShippingAddress.Zip,
            CityName = source.CustomerInfo.ShippingAddress.City,
            CountrySubentity = source.CustomerInfo.ShippingAddress.State,
            Country = source.CustomerInfo.ShippingAddress.Country.Code,
            AddressTypeCode = string.Empty
          },
          Person = new Person(),
          PartyName = source.CustomerInfo.ShippingAddress.Name,
        },
        TrackingID = source.TrackingNumber,
        RequestedDeliveryPeriod = new Period
        {
          EndDate = tempEndDate,
          EndTime = tempEndDate.TimeOfDay,
          StartDate = source.OrderDate,
          StartTime = source.OrderDate.TimeOfDay,
        },
        LatestDeliveryDate = tempEndDate,
        LatestDeliveryTime = tempEndDate.TimeOfDay,
        DeliveryLocation = new Location
        {
          ValidityPeriod = new Period
          {
            EndDate = tempEndDate,
            EndTime = tempEndDate.TimeOfDay,
            StartDate = source.OrderDate,
            StartTime = source.OrderDate.TimeOfDay,
          },
          Address = new Address
          {
            StreetName = source.CustomerInfo.ShippingAddress.Address,
            PostalZone = source.CustomerInfo.ShippingAddress.Zip,
            CityName = source.CustomerInfo.ShippingAddress.City,
            CountrySubentity = source.CustomerInfo.ShippingAddress.State,
            Country = source.CustomerInfo.ShippingAddress.Country.Code,
            AddressTypeCode = string.Empty
          }
        }
      };
      destination.Delivery.Add(delivery);
      destination.FreightForwarderParty = new List<Party>(1);
      Party freightForwarderParty = new Party
      {
        Person = new Person(),
        PartyIdentification = source.ShippingProvider.Code,
        PostalAddress = new Address { AddressTypeCode = string.Empty }
      };
      destination.FreightForwarderParty.Add(freightForwarderParty);
      destination.State = this.GetState(source.Status);

      uint i = 0;
      foreach (DomainModel.Orders.OrderLine line in source.OrderLines)
      {
        OrderLine orderLine = new OrderLine
        {
          LineItem = new LineItem
          {
            Item = new Item
            {
              Code = line.Product.Code,
              Name = line.Product.Title,
              Description = line.Product is Product ? ((Product)line.Product).Description : string.Empty,
              AdditionalInformation = line.ImageUrl,
              Keyword = line.FriendlyUrl,
              PackQuantity = 1,
              PackSizeNumeric = 1
            },
            Price = new Price(new Amount(line.Totals.PriceExVat, destination.PricingCurrencyCode), line.Quantity),
            TotalTaxAmount = new Amount(line.Totals.TotalVat, destination.PricingCurrencyCode),
            Quantity = line.Quantity
          }
        };

        destination.OrderLines.Add(orderLine);

        destination.TaxTotal.TaxSubtotal.Add(
        new TaxSubTotal
        {
          TransactionCurrencyTaxAmount = new Amount(0, source.Currency.Code),
          TaxCategory = new TaxCategory
          {
            Name = source.CustomerInfo.BillingAddress.Country.VatRegion.Code,
            BaseUnitMeasure = new Measure(),
            PerUnitAmount = new Amount(0, source.Currency.Code),
            TaxScheme = new TaxScheme(),
            ID = "SimpleTaxCategory",
            Percent = line.Totals.VAT * 100
          },
          TaxableAmount = new Amount(line.Totals.TotalPriceExVat, source.Currency.Code),
          CalculationSequenceNumeric = i
        });
        i++;
      }

      this.MapSellerSupplierParty(destination);
      this.MapReservationTicket(source, destination);

      return destination;
    }

    /// <summary>
    /// Converts the specified source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>
    /// The order.
    /// </returns>
    [NotNull]
    public virtual DomainModel.Orders.Order Convert([NotNull] Order source)
    {
      Assert.ArgumentNotNull(source, "source");

      DomainModel.Orders.Order destination = new DomainModel.Orders.Order();

      destination.OrderDate = source.IssueDate;
      destination.OrderNumber = source.OrderId;
      destination.TrackingNumber = source.Delivery.First().TrackingID;
      destination.TransactionNumber = source.PaymentMeans.PaymentID;
      destination.NotificationOption = this.GetNotificationOption();
      destination.NotificationOptionValue = source.BuyerCustomerParty.Party.Contact.ElectronicMail;
      destination.PaymentSystem = new PaymentSystem { Code = source.PaymentMeans.PaymentChannelCode };
      destination.Comment = source.Note;
      destination.Currency = new Currency
      {
        Code = source.PricingCurrencyCode,
        Name = source.PricingCurrencyCode,
        Title = source.PricingCurrencyCode
      };
      destination.CustomerInfo = new CustomerInfo
      {
        CustomerId = source.BuyerCustomerParty.SupplierAssignedAccountID,
        Email = source.BuyerCustomerParty.Party.Contact.ElectronicMail,
        Fax = source.BuyerCustomerParty.Party.Contact.Telefax,
        Phone = source.BuyerCustomerParty.Party.Contact.Telephone,
        BillingAddress = new AddressInfo
        {
          Address = source.AccountingCustomerParty.Party.PostalAddress.StreetName,
          Address2 = source.AccountingCustomerParty.Party.PostalAddress.StreetName,
          Zip = source.AccountingCustomerParty.Party.PostalAddress.PostalZone,
          City = source.AccountingCustomerParty.Party.PostalAddress.CityName,
          State = source.AccountingCustomerParty.Party.PostalAddress.CountrySubentity,
          Country = this.GetCountry(source.AccountingCustomerParty.Party.PostalAddress.Country),
          Name2 = source.AccountingCustomerParty.Party.Contact.Name,
          Name = source.AccountingCustomerParty.Party.PartyName,
        },
        ShippingAddress = new AddressInfo
        {
          Address = source.Delivery.First().DeliveryLocation.Address.StreetName,
          Address2 = source.Delivery.First().DeliveryLocation.Address.StreetName,
          Zip = source.Delivery.First().DeliveryLocation.Address.PostalZone,
          City = source.Delivery.First().DeliveryLocation.Address.CityName,
          State = source.Delivery.First().DeliveryLocation.Address.CountrySubentity,
          Country = this.GetCountry(source.Delivery.First().DeliveryLocation.Address.Country),
          Name2 = source.Delivery.First().DeliveryParty.Contact.Name,
          Name = source.Delivery.First().DeliveryParty.PartyName,
        }
      };
      destination.CustomerInfo.CustomProperties.Add(TransactionConstants.CardType, source.PaymentMeans.PaymentMeansCode);
      destination.Totals = new Totals
      {
        TotalPriceIncVat = source.AnticipatedMonetaryTotal.PayableAmount.Value,
        TotalPriceExVat = source.AnticipatedMonetaryTotal.PayableAmount.Value - source.AnticipatedMonetaryTotal.TaxExclusiveAmount.Value,
        PriceIncVat = source.AnticipatedMonetaryTotal.PayableAmount.Value,
        PriceExVat = source.AnticipatedMonetaryTotal.PayableAmount.Value - source.AnticipatedMonetaryTotal.TaxExclusiveAmount.Value,
        TotalVat = source.AnticipatedMonetaryTotal.TaxExclusiveAmount.Value,
        VAT = source.TaxTotal.TaxSubtotal.First().TaxCategory.Percent / 100
      };

      var shippingPrice = source.AllowanceCharge.Sum(ac => ac.ChargeIndicator && ac.AllowanceChargeReasonCode == "ZZZ" ? ac.Amount.Value : 0);

      destination.ShippingPrice = shippingPrice;
      destination.ShippingProvider = this.GetShippingProvider(source.FreightForwarderParty.First().PartyIdentification);
      destination.ShippingProvider.Price = shippingPrice;
      destination.Status = this.GetStatus(source.State);
      destination.OrderLines = new List<DomainModel.Orders.OrderLine>(source.OrderLines.Count);

      uint i = 0;
      foreach (OrderLine orderLine in source.OrderLines)
      {
        DomainModel.Orders.OrderLine newOrderLine = new DomainModel.Orders.OrderLine
        {
          Product = new Product
          {
            Code = orderLine.LineItem.Item.Code,
            Name = orderLine.LineItem.Item.Name,
            Title = orderLine.LineItem.Item.Name,
            Description = orderLine.LineItem.Item.Description
          },
          Totals = new Totals
          {
            TotalPriceExVat = orderLine.LineItem.LineExtensionAmount.Value,
            PriceExVat = orderLine.LineItem.Price.PriceAmount.Value,
            TotalVat = orderLine.LineItem.TotalTaxAmount.Value,
            VAT = source.TaxTotal.TaxSubtotal.First(ts => ts.CalculationSequenceNumeric == i).TaxCategory.Percent / 100
          },
          Quantity = (uint)orderLine.LineItem.Quantity,
          ImageUrl = orderLine.LineItem.Item.AdditionalInformation,
          FriendlyUrl = orderLine.LineItem.Item.Keyword
        };

        newOrderLine.Totals.TotalPriceIncVat = newOrderLine.Totals.TotalPriceExVat + newOrderLine.Totals.TotalVat;
        newOrderLine.Totals.PriceIncVat = newOrderLine.Totals.PriceExVat * (1 + newOrderLine.Totals.VAT);
        destination.OrderLines.Add(newOrderLine);
        i++;
      }

      this.MapReservationTicket(source, destination);

      return destination;
    }

    /// <summary>
    /// Returns the country.
    /// </summary>
    /// <param name="code">The country code.</param>
    /// <returns>
    /// Country instance.
    /// </returns>
    [NotNull]
    protected virtual Country GetCountry([NotNull] string code)
    {
      Assert.ArgumentNotNull(code, "code");
      Assert.IsNotNull(this.CountryProvider, "CountryProvider mus be set.");

      return this.CountryProvider.Get(code);
    }

    /// <summary>
    /// Gets notification option.
    /// </summary>
    /// <returns>Notfication option.</returns>
    [NotNull]
    protected virtual NotificationOption GetNotificationOption()
    {
      Assert.IsNotNull(this.NotificationOptionProvider, "NotificationOptionProvider mus be set.");

      return this.NotificationOptionProvider.Get(DefaultNotificationOption);
    }

    /// <summary>
    /// Gets the Shipping Provider.
    /// </summary>
    /// <param name="code">The party identification.</param>
    /// <returns>
    /// Shipping Provider
    /// </returns>
    [NotNull]
    protected virtual ShippingProvider GetShippingProvider([NotNull] string code)
    {
      Assert.ArgumentNotNull(code, "code");
      Assert.IsNotNull(this.ShippingProvider, "ShippingProvider mus be set.");

      return this.ShippingProvider.Get(code);
    }

    /// <summary>
    /// Gets the company master data.
    /// </summary>
    /// <returns>
    /// The company master data.
    /// </returns>
    [NotNull]
    protected virtual CompanyMasterData GetCompanyMasterData()
    {
      Assert.IsNotNull(this.CompanyMasterData, "CompanyMasterData must be set.");

      this.CompanyMasterData.ReadData();

      return this.CompanyMasterData;
    }

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns>The state.</returns>
    [NotNull]
    protected virtual State GetState([NotNull] DomainModel.Orders.OrderStatus status)
    {
      Assert.ArgumentNotNull(status, "status");
      Assert.IsNotNull(this.OrderStateConfiguration, "OrderStateConfiguration must be set.");

      if ((status is Pending) || (status is Held))
      {
        return this.OrderStateConfiguration.GetStates().Single(state => state.Code == OrderStateCode.Open);
      }

      if (status is Processing)
      {
        return this.OrderStateConfiguration.GetStates().Single(state => state.Code == OrderStateCode.InProcess);
      }

      if (status is Captured)
      {
        State result = this.OrderStateConfiguration.GetStates().Single(state => state.Code == OrderStateCode.InProcess);

        result.Substates.Single(substate => substate.Code == OrderStateCode.InProcessCapturedInFull).Active = true;
        result.Substates.Single(substate => substate.Code == OrderStateCode.InProcessPackedInFull).Active = false;
        result.Substates.Single(substate => substate.Code == OrderStateCode.InProcessShippedInFull).Active = false;

        return result;
      }

      if (status is Completed)
      {
        State result = this.OrderStateConfiguration.GetStates().Single(state => state.Code == OrderStateCode.InProcess);

        result.Substates.Single(substate => substate.Code == OrderStateCode.InProcessCapturedInFull).Active = true;
        result.Substates.Single(substate => substate.Code == OrderStateCode.InProcessPackedInFull).Active = true;
        result.Substates.Single(substate => substate.Code == OrderStateCode.InProcessShippedInFull).Active = true;

        return result;
      }

      if (status is Canceled)
      {
        State result = this.OrderStateConfiguration.GetStates().Single(state => state.Code == OrderStateCode.Cancelled);

        result.Substates.Single(substate => substate.Code == OrderStateCode.CancelledShopOwner).Active = true;

        return result;
      }

      if (status is Closed)
      {
        return this.OrderStateConfiguration.GetStates().Single(state => state.Code == OrderStateCode.Closed);
      }

      return this.OrderStateConfiguration.GetStates().Single(state => state.Code == OrderStateCode.New);
    }

    /// <summary>
    /// Gets the status.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>The status.</returns>
    [NotNull]
    protected virtual DomainModel.Orders.OrderStatus GetStatus([NotNull] State state)
    {
      Assert.ArgumentNotNull(state, "state");

      if (state.Code == OrderStateCode.Open)
      {
        return Context.Entity.Resolve<Pending>();
      }

      if (state.Code == OrderStateCode.InProcess)
      {
        if (state.Substates.All(substate => !substate.Active))
        {
          return Context.Entity.Resolve<Processing>();
        }

        if (state.Substates.Single(substate => substate.Code == OrderStateCode.InProcessCapturedInFull).Active)
        {
          if (state.Substates.Single(substate => substate.Code == OrderStateCode.InProcessPackedInFull).Active &&
            state.Substates.Single(substate => substate.Code == OrderStateCode.InProcessShippedInFull).Active)
          {
            return Context.Entity.Resolve<Completed>();
          }

          if (!(state.Substates.Single(substate => substate.Code == OrderStateCode.InProcessPackedInFull).Active ||
            state.Substates.Single(substate => substate.Code == OrderStateCode.InProcessShippedInFull).Active))
          {
            return Context.Entity.Resolve<Captured>();
          }
        }
      }

      if (state.Code == OrderStateCode.Cancelled)
      {
        if (state.Substates.Single(substate => substate.Code == OrderStateCode.CancelledShopOwner).Active)
        {
          return Context.Entity.Resolve<Canceled>();
        }
      }

      if (state.Code == OrderStateCode.Closed)
      {
        return Context.Entity.Resolve<Closed>();
      }

      return Context.Entity.Resolve<NewOrder>();
    }

    /// <summary>
    /// Maps the amounts.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    protected virtual void MapAmounts([NotNull] DomainModel.Orders.Order source, [NotNull] Order destination)
    {
      Assert.ArgumentNotNull(source, "source");
      Assert.ArgumentNotNull(destination, "destination");
      Assert.IsNotNull(source.Currency, "currency");

      destination.AnticipatedMonetaryTotal = new MonetaryTotal { PrepaidAmount = new Amount(0, source.Currency.Code) };
      destination.AllowanceCharge = new List<AllowanceCharge>(1)
      {
        new AllowanceCharge
        {
          BaseAmount = new Amount(0, source.Currency.Code),
          ChargeIndicator = true,
          SequenceNumeric = 1,
          Amount = new Amount(source.ShippingPrice, source.Currency.Code),
          AllowanceChargeReasonCode = AllowanceChargeReasonCode,
          AllowanceChargeReason = AllowanceChargeReason
        }
      };
    }

    /// <summary>
    /// Maps the seller supplier party.
    /// </summary>
    /// <param name="order">The order.</param>
    protected virtual void MapSellerSupplierParty([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      if (order.SellerSupplierParty == null)
      {
        order.SellerSupplierParty = new SupplierParty();
      }

      if (order.SellerSupplierParty.Party == null)
      {
        order.SellerSupplierParty.Party = new Party();
      }

      if (order.SellerSupplierParty.Party.Contact == null)
      {
        order.SellerSupplierParty.Party.Contact = new Contact();
      }

      if (order.SellerSupplierParty.Party.PostalAddress == null)
      {
        order.SellerSupplierParty.Party.PostalAddress = new Address { AddressTypeCode = string.Empty };
      }

      if (order.SellerSupplierParty.Party.Person == null)
      {
        order.SellerSupplierParty.Party.Person = new Person();
      }

      CompanyMasterData data = this.GetCompanyMasterData();

      order.SellerSupplierParty.Party.PartyName = data.CompanyName;
      order.SellerSupplierParty.Party.WebsiteURI = data.Website;
      order.SellerSupplierParty.Party.Contact.ElectronicMail = data.Email;
      order.SellerSupplierParty.Party.Contact.Telephone = data.CompanyNumber;
      order.SellerSupplierParty.Party.Contact.Telefax = data.Fax;
      order.SellerSupplierParty.Party.PostalAddress.CityName = data.City;
      order.SellerSupplierParty.Party.PostalAddress.StreetName = data.Address;
      order.SellerSupplierParty.Party.PostalAddress.Country = data.Country;
      order.SellerSupplierParty.Party.PostalAddress.CountrySubentity = data.State;
      order.SellerSupplierParty.Party.PostalAddress.PostalZone = data.Zip;
      order.SellerSupplierParty.Party.PostalAddress.PostBox = data.PostBox;
      order.SellerSupplierParty.Party.PostalAddress.AddressTypeCode = string.Empty;
    }

    /// <summary>
    /// Maps the reservation ticket.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    protected virtual void MapReservationTicket([NotNull] DomainModel.Orders.Order source, [NotNull] Order destination)
    {
      Assert.IsNotNull(source, "source");
      Assert.IsNotNull(destination, "destination");

      Assert.IsNotNull(this.PaymentProviderFactory, "PaymentProviderFactory cannot be null.");
      Assert.IsNotNull(source.PaymentSystem, "source.PaymentSystem cannot be null.");
      Assert.IsNotNull(source.PaymentSystem.Code, "source.PaymentSystem.Code cannot be null");

      bool isReserved = this.PaymentProviderFactory.GetProvider(source.PaymentSystem.Code) is IReservable;
      if (isReserved)
      {
        DomainModel.Payments.ReservationTicket oldReservationTicket = new DomainModel.Payments.ReservationTicket(source);
        bool wasAlreadyCaptured = source.Status is Captured;

        destination.ReservationTicket = new Ecommerce.OrderManagement.Orders.ReservationTicket
        {
          Amount = oldReservationTicket.Amount,
          CapturedAmount = wasAlreadyCaptured ? oldReservationTicket.Amount : 0,
          AuthorizationCode = oldReservationTicket.AuthorizationCode,
          InvoiceNumber = oldReservationTicket.InvoiceNumber,
          TransactionNumber = oldReservationTicket.TransactionNumber
        };
      }
    }

    /// <summary>
    /// Maps the reservation ticket.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    protected virtual void MapReservationTicket([NotNull] Order source, [NotNull] DomainModel.Orders.Order destination)
    {
      Assert.IsNotNull(source, "source");
      Assert.IsNotNull(destination, "destination");

      if (source.ReservationTicket != null)
      {
        destination.AuthorizationCode = source.ReservationTicket.AuthorizationCode;
        destination.TransactionNumber = source.ReservationTicket.TransactionNumber;
      }
    }
  }
}