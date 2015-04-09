// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaymentUrls.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines payment URLs
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
  /// <summary>
  ///  Defines payment URLs
  /// </summary>
  public class PaymentUrls
  {
    /// <summary>
    /// Gets or sets the payment success page URL.
    /// </summary>
    /// <value>The payment failure page URL.</value>
    public string SuccessPageUrl { get; set; }

    /// <summary>
    /// Gets or sets the payment failure page URL.
    /// </summary>
    /// <value>The payment failure page URL.</value>
    public string FailurePageUrl { get; set; }

    /// <summary>
    /// Gets or sets the payment return page URL.
    /// </summary>
    /// <value>The payment return page URL.</value>
    public string ReturnPageUrl { get; set; }

    /// <summary>
    /// Gets or sets the payment cancel page URL.
    /// </summary>
    /// <value>The payment cancel page URL.</value>
    public string CancelPageUrl { get; set; }
  }
}
