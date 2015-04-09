// -------------------------------------------------------------------------------------------
// <copyright file="CreditCardInfo.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.DomainModel.Payments
{
  using System;

  /// <summary>
  /// The credit card info.
  /// </summary>
  public class CreditCardInfo
  {
    /// <summary>
    /// Gets or sets the name of the cards holder.
    /// </summary>
    /// <value>The name of the cards holder.</value>
    public virtual string CardsHolderName { get; set; }

    /// <summary>
    /// Gets or sets the card number.
    /// </summary>
    /// <value>The card number.</value>
    public virtual long CardNumber { get; set; }

    /// <summary>
    /// Gets or sets the expiration data.
    /// </summary>
    /// <value>The expiration data.</value>
    public virtual DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the security code.
    /// </summary>
    /// <value>The security code.</value>
    public virtual int SecurityCode { get; set; }
  }
}