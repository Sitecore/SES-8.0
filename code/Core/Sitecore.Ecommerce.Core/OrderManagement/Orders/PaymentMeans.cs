// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaymentMeans.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the PaymentMeans class.
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
  using Common;

  public class PaymentMeans : IEntity
  {
    /// <summary>
    /// Identifier
    /// </summary>
    public virtual string ID { get; set; }

    /// <summary>
    /// Identifier for the payment means
    /// </summary>
    public virtual string PaymentMeansCode { get; set; }

    /// <summary>
    /// Date the payments is due
    /// </summary>
    public virtual DateTime PaymentDueDate { get; set; }

    /// <summary>
    /// Paymentchannel ( type
    /// </summary>
    public virtual string PaymentChannelCode { get; set; }

    /// <summary>
    /// Identifier for the payment
    /// </summary>
    public virtual string PaymentID { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
