// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReservationTicket.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ReservationTicket type.
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
  using Common;

  /// <summary>
  /// Represents the Reservation Ticket.
  /// </summary>
  public class ReservationTicket : IEntity
  {
    /// <summary>
    /// Gets or sets Alias.
    /// </summary>
    public virtual long Alias { get; protected set; }

    /// <summary>
    /// Gets or sets the Authorization code.
    /// </summary>
    public virtual string AuthorizationCode { get; set; }

    /// <summary>
    /// Gets or sets the Transaction number.
    /// </summary>
    public virtual string TransactionNumber { get; set; }

    /// <summary>
    /// Gets or sets the Invoice number.
    /// </summary>
    public virtual string InvoiceNumber { get; set; }

    /// <summary>
    /// Gets or sets the Amount value.
    /// </summary>
    public virtual decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the captured amount.
    /// </summary>
    /// <value>
    /// The captured amount.
    /// </value>
    public virtual decimal CapturedAmount { get; set; }
  }
}
