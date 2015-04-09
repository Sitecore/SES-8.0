// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStateCode.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order state code class.
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

namespace Sitecore.Ecommerce.OrderManagement
{
  /// <summary>
  /// Defines the order state code class.
  /// </summary>
  public static class OrderStateCode
  {
    /// <summary>
    /// The "New" order state code.
    /// </summary>
    public const string New = "New";

    /// <summary>
    /// The "Open" order state code.
    /// </summary>
    public const string Open = "Open";

    /// <summary>
    /// The "In Process" order state code.
    /// </summary>
    public const string InProcess = "In Process";

    /// <summary>
    /// The "Captured in full" order sub-state code.
    /// </summary>
    public const string InProcessCapturedInFull = "Captured In Full";

    /// <summary>
    /// The "Packed in full" order sub-state code.
    /// </summary>
    public const string InProcessPackedInFull = "Packed In Full";

    /// <summary>
    /// The "Shipped in full" order sub-state code.
    /// </summary>
    public const string InProcessShippedInFull = "Shipped In Full";

    /// <summary>
    /// The "Closed" order state code.
    /// </summary>
    public const string Closed = "Closed";

    /// <summary>
    /// The "Cancelled" order state code.
    /// </summary>
    public const string Cancelled = "Cancelled";

    /// <summary>
    /// The "Customer" order sub-state code.
    /// </summary>
    public const string CancelledCustomer = "Customer";

    /// <summary>
    /// The "Fraud" order sub-state code.
    /// </summary>
    public const string CancelledFraud = "Fraud";

    /// <summary>
    /// The "Shop owner" order sub-state code.
    /// </summary>
    public const string CancelledShopOwner = "Shop Owner";

    /// <summary>
    /// The "Suspicious" order state.
    /// </summary>
    public const string Suspicious = "Suspicious";

    /// <summary>
    /// The "Product Quantity" order sub-state code.
    /// </summary>
    public const string SuspiciousProductQuantity = "Product Quantity";
  }
}