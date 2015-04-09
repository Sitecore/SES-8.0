// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransactionStatus.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the TransactionStatus type.
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

namespace Sitecore.Ecommerce.Payments.PayEx
{
  /// <summary>
  /// Enumeration of the transaction statuses from the PayEx response.
  /// </summary>
  public enum TransactionStatus
  {
    /// <summary>
    /// Sale transaction was performed.
    /// </summary>
    Sale = 0,

    /// <summary>
    /// Initialize transaction was performed.
    /// </summary>
    Initialize = 1,

    /// <summary>
    /// Credit transaction was performed.
    /// </summary>
    Credit = 2,

    /// <summary>
    /// Authorization transaction was performed.
    /// </summary>
    Authorize = 3,

    /// <summary>
    /// Transaction was canceled.
    /// </summary>
    Cancel = 4,

    /// <summary>
    /// Transaction was failed.
    /// </summary>
    Failure = 5,

    /// <summary>
    /// Amount of the money was captured.
    /// </summary>
    Capture = 6
  }
}