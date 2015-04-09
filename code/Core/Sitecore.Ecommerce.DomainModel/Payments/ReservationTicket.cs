// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReservationTicket.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The reservation ticket.
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

namespace Sitecore.Ecommerce.DomainModel.Payments
{
  using System;
  using Orders;

  /// <summary>
  /// The reservation ticket.
  /// </summary>
  public class ReservationTicket
  {
    /// <summary>
    /// Authorization code.
    /// </summary>
    private string authorizationCode;

    /// <summary>
    /// Transaction number.
    /// </summary>
    private string transactionNumber;

    /// <summary>
    /// Invoice number.
    /// </summary>
    private string invoiceNumber;
    
    /// <summary>
    /// Amount value.
    /// </summary>
    private decimal amount;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReservationTicket"/> class.
    /// </summary>
    public ReservationTicket()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReservationTicket"/> class. 
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>The reservation ticket.</returns>
    public ReservationTicket(Order order)
    {
      this.authorizationCode = order.AuthorizationCode;
      this.transactionNumber = order.TransactionNumber;
      this.invoiceNumber = order.OrderNumber;
      this.amount = order.Totals != null ? Math.Round(order.Totals.PriceIncVat, 2, MidpointRounding.AwayFromZero) : 0M;
    }

    /// <summary>
    /// Gets or sets the Authorization code.
    /// </summary>
    public virtual string AuthorizationCode
    {
      get { return this.authorizationCode; }
      set { this.authorizationCode = value; }
    }

    /// <summary>
    /// Gets or sets the Transaction number.
    /// </summary>
    public virtual string TransactionNumber
    {
      get { return this.transactionNumber; }
      set { this.transactionNumber = value; }
    }

    /// <summary>
    /// Gets or sets the Invoice number.
    /// </summary>
    public virtual string InvoiceNumber
    {
      get { return this.invoiceNumber; }
      set { this.invoiceNumber = value; }
    }

    /// <summary>
    /// Gets or sets the Amount value.
    /// </summary>
    public virtual decimal Amount
    {
      get { return this.amount; }
      set { this.amount = value; }
    }
  }
}