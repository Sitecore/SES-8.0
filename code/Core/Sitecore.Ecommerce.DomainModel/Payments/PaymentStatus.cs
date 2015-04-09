// -------------------------------------------------------------------------------------------
// <copyright file="PaymentStatus.cs" company="Sitecore Corporation">
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
  /// <summary>
  /// This enum represents the status of the online payment.
  /// </summary>
  public enum PaymentStatus
  {
    /// <summary>
    /// The payment status is unknown.
    /// </summary>
    Unknown = -1,

    /// <summary>
    /// The payment failed.
    /// </summary>
    Failure = 0,

    /// <summary>
    /// The payment is canceld by the user.
    /// </summary>
    Canceled = 1,

    /// <summary>
    /// The payment is reserved.
    /// </summary>
    Reserved = 2,

    /// <summary>
    /// The payment is captured.
    /// </summary>
    Captured = 3,

    /// <summary>
    /// The payment is authorised.
    /// </summary>
    Succeeded = 5,
  }
}