// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Order.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order class.
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

namespace Sitecore.Ecommerce.OrderManagement.Orders
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using Common;
  using Diagnostics;

  /// <summary>
  /// Defines the order class.
  /// </summary>
  public class Order
  {
    /// <summary>
    /// The allowance charge.
    /// </summary>
    private ICollection<AllowanceCharge> allowanceCharge;

    /// <summary>
    /// The anticipated monetary total.
    /// </summary>
    private MonetaryTotal anticipatedMonetaryTotal;

    /// <summary>
    /// The order lines.
    /// </summary>
    private ICollection<OrderLine> orderLines;

    /// <summary>
    /// The order state.
    /// </summary>
    private State state;

    /// <summary>
    /// The issue date.
    /// </summary>
    private DateTime issueDate;

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class.
    /// </summary>
    public Order()
    {
      this.issueDate = DateTime.Now;
    }

    /// <summary>
    /// Gets or sets the ID.
    /// </summary>
    /// <value>
    /// The ID.
    /// </value>
    public virtual Guid ID { get; set; }

    /// <summary>
    /// The OrderId From the Merchant.
    /// </summary>
    public virtual string OrderId { get; set; }

    /// <summary>
    /// Gets or sets the issue date.
    /// </summary>
    /// <value>The issue date.</value>
    public virtual DateTime IssueDate
    {
      get { return this.issueDate; }
      set { this.issueDate = value; }
    }

    /// <summary>
    /// Gets the issue time.
    /// </summary>
    public virtual TimeSpan IssueTime
    {
      get { return this.IssueDate.TimeOfDay; }
    }

    /// <summary>
    /// Note on the order.
    /// </summary>
    public virtual string Note { get; set; }

    /// <summary>
    /// Currencycode for the Price.
    /// </summary>
    public virtual string PricingCurrencyCode { get; set; }

    /// <summary>
    /// CurrencyCode for the Tax.
    /// </summary>
    public virtual string TaxCurrencyCode { get; set; }

    /// <summary>
    /// Information about the Buyer.
    /// </summary>
    public virtual CustomerParty BuyerCustomerParty { get; set; }

    /// <summary>
    /// Information about the seller.
    /// </summary>
    public virtual SupplierParty SellerSupplierParty { get; set; }

    /// <summary>
    /// Information about the transporter.
    /// </summary>
    public virtual ICollection<Party> FreightForwarderParty { get; set; }

    /// <summary>
    /// Information about who’s paying if different from Buyer.
    /// </summary>
    public virtual CustomerParty AccountingCustomerParty { get; set; }

    /// <summary>
    /// Delivery information.
    /// </summary>
    public virtual ICollection<Delivery> Delivery { get; set; }

    /// <summary>
    /// Payment type information.
    /// </summary>
    public virtual PaymentMeans PaymentMeans { get; set; }

    /// <summary>
    /// Discount information.
    /// </summary>
    public virtual ICollection<AllowanceCharge> AllowanceCharge
    {
      get { return this.allowanceCharge = this.allowanceCharge ?? new Collection<AllowanceCharge>(); }
      set { this.allowanceCharge = value; }
    }

    /// <summary>
    /// Destination Country.
    /// </summary>
    public virtual string DestinationCountryCode { get; set; }

    /// <summary>
    /// Gets or sets the tax total.
    /// </summary>
    /// <value> The tax total. </value>
    public virtual TaxTotal TaxTotal { get; set; }

    /// <summary>
    /// Expected Total of the order.
    /// </summary>
    public virtual MonetaryTotal AnticipatedMonetaryTotal
    {
      get { return this.anticipatedMonetaryTotal; }
      set { this.anticipatedMonetaryTotal = value; }
    }

    /// <summary>
    /// Gets or sets the shop context.
    /// </summary>
    /// <value>The shop context.</value>
    public virtual string ShopContext { get; set; }

    /// <summary>
    /// Gets or sets the order lines.
    /// </summary>
    /// <value>The order lines.</value>
    [NotNull]
    public virtual ICollection<OrderLine> OrderLines
    {
      get
      {
        return this.orderLines = this.orderLines ?? new Collection<OrderLine>();
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.orderLines = value;
      }
    }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    /// <value>
    /// The state.
    /// </value>
    [CanBeNull]
    public virtual State State
    {
      get { return this.state; }
      set { this.state = value; }
    }

    /// <summary>
    /// Gets or sets ReservationTicket.
    /// </summary>
    [NotNull]
    public virtual ReservationTicket ReservationTicket { get; set; }

    /// <summary>
    /// Gets the first delivery.
    /// </summary>
    [CanBeNull]
    public virtual Delivery DefaultDelivery
    {
      get
      {
        if (this.Delivery != null)
        {
          return this.Delivery.FirstOrDefault();
        }

        return null;
      }
    }

    /// <summary>
    /// Gets the first freight forwarder party.
    /// </summary>
    [CanBeNull]
    public virtual Party DefaultFreightForwarderParty
    {
      get
      {
        if (this.FreightForwarderParty != null)
        {
          return this.FreightForwarderParty.FirstOrDefault();
        }

        return null;
      }
    }
  }
}