// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderReportModel.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order report model class.
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

namespace Sitecore.Ecommerce.Report
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Common;
  using Configurations;
  using Diagnostics;
  using OrderManagement.Orders;
  using Stimulsoft.Base.Design;

  /// <summary>
  /// Defines the order report model class.
  /// </summary>
  public class OrderReportModel
  {
    /// <summary>
    /// The company master data.
    /// </summary>
    private CompanyMasterData companyMasterData;

    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>
    /// The order.
    /// </value>
    [StiBrowsable(false)]
    [CanBeNull]
    public virtual Order Order { get; set; }

    /// <summary>
    /// Gets or sets the company master data.
    /// </summary>
    /// <value>
    /// The company master data.
    /// </value>
    [StiBrowsable(false)]
    [CanBeNull]
    public virtual CompanyMasterData CompanyMasterData
    {
      get
      {
        return this.companyMasterData ?? (this.companyMasterData = new CompanyMasterData());
      }

      set
      {
        this.companyMasterData = value;
      }
    }

    /// <summary>
    /// Gets the language code.
    /// </summary>
    /// <value>
    /// The language code.
    /// </value>
    public virtual string LanguageCode
    {
      get
      {
        if (this.Order != null && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null))
        {
          return this.Order.BuyerCustomerParty.Party.LanguageCode;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the order id.
    /// </summary>
    /// <value>The order id.</value>
    [NotNull]
    public virtual string OrderId
    {
      get
      {
        if (this.Order != null)
        {
          return this.Order.OrderId;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the issue date.
    /// </summary>
    /// <value>The issue date.</value>
    public virtual DateTime IssueDate
    {
      get
      {
        if (this.Order != null)
        {
          return this.Order.IssueDate;
        }

        return DateTime.MinValue;
      }
    }

    /// <summary>
    /// Gets the name of the company.
    /// </summary>
    /// <value>The name of the company.</value>
    [NotNull]
    public virtual string CompanyName
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.CompanyName;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the company address.
    /// </summary>
    /// <value>The company address.</value>
    [NotNull]
    public virtual string CompanyAddress
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.Address;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the company city.
    /// </summary>
    /// <value>The company city.</value>
    [NotNull]
    public virtual string CompanyCity
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.City;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the company zip.
    /// </summary>
    /// <value>The company zip.</value>
    [NotNull]
    public virtual string CompanyZip
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.Zip;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the company country.
    /// </summary>
    /// <value>The company country.</value>
    [NotNull]
    public virtual string CompanyCountry
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.Country;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the company post box.
    /// </summary>
    /// <value>The company post box.</value>
    [NotNull]
    public virtual string CompanyPostBox
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.PostBox;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the company number.
    /// </summary>
    /// <value>The company number.</value>
    [NotNull]
    public virtual string CompanyNumber
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.CompanyNumber;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the company fax.
    /// </summary>
    /// <value>The company fax.</value>
    [NotNull]
    public virtual string CompanyFax
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.Fax;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the supplier party mail.
    /// </summary>
    /// <value>The supplier party mail.</value>
    [NotNull]
    public virtual string CompanyMail
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.Email;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the company website.
    /// </summary>
    /// <value>The company website.</value>
    [NotNull]
    public virtual string CompanyWebsite
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.Website;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the company logo URL.
    /// </summary>
    /// <value>The company logo URL.</value>
    [NotNull]
    public virtual string CompanyLogoUrl
    {
      get
      {
        if (this.CompanyMasterData != null)
        {
          return this.CompanyMasterData.LogoUrl;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the buyer party supplier assigned account id.
    /// </summary>
    /// <value>The buyer party supplier assigned account id.</value>
    [NotNull]
    public virtual string BuyerPartySupplierAssignedAccountId
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null))
        {
          return this.Order.BuyerCustomerParty.SupplierAssignedAccountID;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the name of the buyer party.
    /// </summary>
    /// <value>The name of the buyer party.</value>
    [NotNull]
    public virtual string BuyerPartyName
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.PartyName != null))
        {
            return this.Order.BuyerCustomerParty.Party.PartyName;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the name of the buyer contact.
    /// </summary>
    /// <value>
    /// The name of the buyer contact.
    /// </value>
    [NotNull]
    public virtual string BuyerContactName
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.Contact != null) && (this.Order.BuyerCustomerParty.Party.Contact.Name != null))
        {
          return this.Order.BuyerCustomerParty.Party.Contact.Name;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the name of the buyer party street.
    /// </summary>
    /// <value>The name of the buyer party street.</value>
    [NotNull]
    public virtual string BuyerPartyAddressLine
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.PostalAddress != null))
        {
          return this.Order.BuyerCustomerParty.Party.PostalAddress.AddressLine;
        }

        return string.Empty;
      }
    }


    /// <summary>
    /// Gets the name of the buyer party street.
    /// </summary>
    /// <value>
    /// The name of the buyer party street.
    /// </value>
    [NotNull]
    public virtual string BuyerPartyStreetName
    {
        get
        {
            if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.PostalAddress != null))
            {
                return this.Order.BuyerCustomerParty.Party.PostalAddress.StreetName;
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the name of the buyer party city.
    /// </summary>
    /// <value>The name of the buyer party city.</value>
    [NotNull]
    public virtual string BuyerPartyCityName
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.PostalAddress != null))
        {
          return this.Order.BuyerCustomerParty.Party.PostalAddress.CityName;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the buyer party postal zone.
    /// </summary>
    /// <value>The buyer party postal zone.</value>
    [NotNull]
    public virtual string BuyerPartyPostalZone
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.PostalAddress != null))
        {
          return this.Order.BuyerCustomerParty.Party.PostalAddress.PostalZone;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the buyer party country.
    /// </summary>
    /// <value>The buyer party country.</value>
    [NotNull]
    public virtual string BuyerPartyCountry
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.PostalAddress != null))
        {
          return this.Order.BuyerCustomerParty.Party.PostalAddress.Country;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the buyer party telephone.
    /// </summary>
    /// <value>The buyer party telephone.</value>
    [NotNull]
    public virtual string BuyerPartyTelephone
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.Contact != null))
        {
          return this.Order.BuyerCustomerParty.Party.Contact.Telephone;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the buyer party telephone.
    /// </summary>
    /// <value>The buyer party telephone.</value>
    [NotNull]
    public virtual string BuyerPartyTelefax
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.Contact != null))
        {
          return this.Order.BuyerCustomerParty.Party.Contact.Telefax;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the buyer party mail.
    /// </summary>
    /// <value>The buyer party mail.</value>
    [NotNull]
    public virtual string BuyerPartyMail
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.Contact != null))
        {
          return this.Order.BuyerCustomerParty.Party.Contact.ElectronicMail;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the buyer party note.
    /// </summary>
    /// <value>The buyer party note.</value>
    [NotNull]
    public virtual string BuyerPartyNote
    {
      get
      {
        if ((this.Order != null) && (this.Order.BuyerCustomerParty != null) && (this.Order.BuyerCustomerParty.Party != null) && (this.Order.BuyerCustomerParty.Party.Contact != null))
        {
          return this.Order.BuyerCustomerParty.Party.Contact.Note;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the name of the delivery party.
    /// </summary>
    /// <value>The name of the delivery party.</value>
    [NotNull]
    public virtual string DeliveryPartyName
    {
      get
      {
        if ((this.Delivery != null) && (this.Delivery.DeliveryParty != null) && (this.Delivery.DeliveryParty.Contact != null))
        {
          return this.Delivery.DeliveryParty.Contact.Name;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the name of the delivery party street.
    /// </summary>
    /// <value>The name of the delivery party street.</value>
    [NotNull]
    public virtual string DeliveryPartyAddressLine
    {
      get
      {
        if ((this.Delivery != null) && (this.Delivery.DeliveryParty != null) && (this.Delivery.DeliveryParty.PostalAddress != null))
        {
          return this.Delivery.DeliveryParty.PostalAddress.AddressLine;
        }

        return string.Empty;
      }
    }


    /// <summary>
    /// Gets the name of the delivery party street.
    /// </summary>
    /// <value>
    /// The name of the delivery party street.
    /// </value>
    [NotNull]
    public virtual string DeliveryPartyStreetName
    {
        get
        {
            if ((this.Delivery != null) && (this.Delivery.DeliveryParty != null) && (this.Delivery.DeliveryParty.PostalAddress != null))
            {
                return this.Delivery.DeliveryParty.PostalAddress.StreetName;
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the name of the delivery party city.
    /// </summary>
    /// <value>The name of the delivery party city.</value>
    [NotNull]
    public virtual string DeliveryPartyCityName
    {
      get
      {
        if ((this.Delivery != null) && (this.Delivery.DeliveryParty != null) && (this.Delivery.DeliveryParty.PostalAddress != null))
        {
          return this.Delivery.DeliveryParty.PostalAddress.CityName;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the delivery party postal zone.
    /// </summary>
    /// <value>The delivery party postal zone.</value>
    [NotNull]
    public virtual string DeliveryPartyPostalZone
    {
      get
      {
        if ((this.Delivery != null) && (this.Delivery.DeliveryParty != null) && (this.Delivery.DeliveryParty.PostalAddress != null))
        {
          return this.Delivery.DeliveryParty.PostalAddress.PostalZone;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the delivery party country.
    /// </summary>
    /// <value>The delivery party country.</value>
    [NotNull]
    public virtual string DeliveryPartyCountry
    {
      get
      {
        if ((this.Delivery != null) && (this.Delivery.DeliveryParty != null) && (this.Delivery.DeliveryParty.PostalAddress != null))
        {
          return this.Delivery.DeliveryParty.PostalAddress.Country;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the delivery party telephone.
    /// </summary>
    /// <value>The delivery party telephone.</value>
    [NotNull]
    public virtual string DeliveryPartyTelephone
    {
      get
      {
        if ((this.Delivery != null) && (this.Delivery.DeliveryParty != null) && (this.Delivery.DeliveryParty.Contact != null))
        {
          return this.Delivery.DeliveryParty.Contact.Telephone;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the delivery party mail.
    /// </summary>
    /// <value>The delivery party mail.</value>
    [NotNull]
    public virtual string DeliveryPartyMail
    {
      get
      {
        if ((this.Delivery != null) && (this.Delivery.DeliveryParty != null) && (this.Delivery.DeliveryParty.Contact != null))
        {
          return this.Delivery.DeliveryParty.Contact.ElectronicMail;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the delivery party note.
    /// </summary>
    /// <value>The delivery party note.</value>
    [NotNull]
    public virtual string DeliveryPartyNote
    {
      get
      {
        if ((this.Delivery != null) && (this.Delivery.DeliveryParty != null) && (this.Delivery.DeliveryParty.Contact != null))
        {
          return this.Delivery.DeliveryParty.Contact.Note;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the order lines.
    /// </summary>
    /// <value>The order lines.</value>
    [NotNull]
    public virtual IEnumerable<OrderLineReportModel> OrderLines
    {
      get
      {
        if ((this.Order != null) && (this.Order.OrderLines != null))
        {
          return this.Order.OrderLines.Select(orderLine => new OrderLineReportModel { OrderLine = orderLine });
        }

        return Enumerable.Empty<OrderLineReportModel>();
      }
    }

    /// <summary>
    /// Gets the anticipated monetary total tax exclusive amount.
    /// </summary>
    /// <value>The anticipated monetary total tax exclusive amount.</value>
    [NotNull]
    public virtual string AnticipatedMonetaryTotalTaxExclusiveAmount
    {
      get
      {
        if ((this.Order != null) && (this.Order.AnticipatedMonetaryTotal != null) && (this.Order.AnticipatedMonetaryTotal.TaxExclusiveAmount != null))
        {
          return this.GetAmountRepresentation(this.Order.AnticipatedMonetaryTotal.TaxExclusiveAmount);
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the anticipated monetary total tax inclusive amount.
    /// </summary>
    /// <value>The anticipated monetary total tax inclusive amount.</value>
    [NotNull]
    public virtual string AnticipatedMonetaryTotalTaxInclusiveAmount
    {
        get
        {
            if ((this.Order != null) && (this.Order.AnticipatedMonetaryTotal != null) && (this.Order.AnticipatedMonetaryTotal.TaxInclusiveAmount != null))
            {
                return this.GetAmountRepresentation(this.Order.AnticipatedMonetaryTotal.TaxInclusiveAmount);
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the aggregated allowance charges.
    /// </summary>
    /// <value>The aggregated allowance charges.</value>
    [NotNull]
    public virtual string AggregatedAllowanceCharges
    {
      get
      {
        if ((this.Order != null) && (this.Order.AllowanceCharge != null))
        {
          Amount resultingAmount = null;

          foreach (AllowanceCharge allowanceCharge in this.Order.AllowanceCharge)
          {
            if (resultingAmount == null)
            {
              resultingAmount = allowanceCharge.Amount;
            }
            else
            {
              Assert.AreEqual(resultingAmount.CurrencyID, allowanceCharge.Amount.CurrencyID, "All allowance charge amounts must be of the same currency");
              resultingAmount.Value += allowanceCharge.Amount.Value;
            }
          }

          if (resultingAmount != null)
          {
            return this.GetAmountRepresentation(resultingAmount);
          }
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the aggregated tax percents.
    /// </summary>
    /// <value>The aggregated tax percents.</value>
    public virtual decimal AggregatedTaxPercents
    {
      get
      {
        if ((this.Order != null) && (this.Order.TaxTotal != null))
        {
          return this.Order.TaxTotal.TaxSubtotal.Aggregate(0m, (result, taxSubtotal) => taxSubtotal.TaxCategory.Percent);
        }

        return 0;
      }
    }

    /// <summary>
    /// Gets the anticipated monetary total payable amount.
    /// </summary>
    public virtual string AnticipatedMonetaryTotalPayableAmount
    {
        get
        {
            if ((this.Order != null) && (this.Order.AnticipatedMonetaryTotal != null))
            {
                return this.GetAmountRepresentation(this.Order.AnticipatedMonetaryTotal.PayableAmount);
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the anticipated monetary total allowance total amount.
    /// </summary>
    /// <value>The anticipated monetary total allowance total amount.</value>
    [NotNull]
    public virtual string AnticipatedMonetaryTotalAllowanceTotalAmount
    {
      get
      {
        if ((this.Order != null) && (this.Order.AnticipatedMonetaryTotal != null))
        {
          return this.GetAmountRepresentation(this.Order.AnticipatedMonetaryTotal.AllowanceTotalAmount);
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the anticipated monetary total line extension amount.
    /// </summary>
    /// <value>The anticipated monetary total line extension amount.</value>
    [NotNull]
    public virtual string AnticipatedMonetaryTotalLineExtensionAmount
    {
      get
      {
        if ((this.Order != null) && (this.Order.AnticipatedMonetaryTotal != null))
        {
          return this.GetAmountRepresentation(this.Order.AnticipatedMonetaryTotal.LineExtensionAmount);
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the delivery tracking ID.
    /// </summary>
    /// <value>The delivery tracking ID.</value>
    [NotNull]
    public virtual string DeliveryTrackingID
    {
      get
      {
        if (this.Delivery != null)
        {
          return this.Delivery.TrackingID;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the delivery party website URI.
    /// </summary>
    /// <value>The delivery party website URI.</value>
    [NotNull]
    public virtual string DeliveryPartyWebsiteURI
    {
      get
      {
        if ((this.Delivery != null) && (this.Delivery.DeliveryParty != null))
        {
          return this.Delivery.DeliveryParty.WebsiteURI;
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the delivery.
    /// </summary>
    /// <value>The delivery.</value>
    [CanBeNull]
    protected virtual Delivery Delivery
    {
      get
      {
        if ((this.Order != null) && (this.Order.Delivery != null))
        {
          return this.Order.Delivery.FirstOrDefault();
        }

        return null;
      }
    }

    /// <summary>
    /// Gets the amount representation.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <returns>The amount representation.</returns>
    [NotNull]
    protected virtual string GetAmountRepresentation([NotNull] Amount amount)
    {
      Assert.IsNotNull(amount, "amount");

      return Ecommerce.Utils.MainUtil.FormatPrice(amount.Value, true, null, amount.CurrencyID);
    }
  }
}