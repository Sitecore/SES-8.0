// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPrintOrderView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The print order view.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Views
{
  using Report;

  using Stimulsoft.Report;

  /// <summary>
  /// The print order view.
  /// </summary>
  public interface IPrintOrderView
  {
    /// <summary>
    /// Gets the order ID.
    /// </summary>
    /// <value>
    /// The order ID.
    /// </value>
    string OrderID { get; }

    /// <summary>
    /// Gets or sets the report.
    /// </summary>
    /// <value>
    /// The report.
    /// </value>
    StiReport Report { get; set; }

    /// <summary>
    /// Gets or sets the model.
    /// </summary>
    /// <value>
    /// The model.
    /// </value>
    OrderReportModel Model { get; set; }

    /// <summary>
    /// Shows the order not found error.
    /// </summary>
    void ShowOrderNotFoundError();
  }
}