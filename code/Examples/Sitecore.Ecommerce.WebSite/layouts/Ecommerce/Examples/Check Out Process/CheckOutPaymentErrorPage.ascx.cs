// -------------------------------------------------------------------------------------------
// <copyright file="CheckOutPaymentErrorPage.ascx.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess
{
  using System;
  using System.Web.UI;

  /// <summary>
  /// The error page.
  /// </summary>
  public partial class CheckOutPaymentErrorPage : UserControl
  {
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      if (this.IsPostBack)
      {
        this.paymentError.Visible = false;
        this.lblErrorText.Visible = false;
      }

      string message = this.Session["paymentErrorMessage"] as string;

      if (!string.IsNullOrEmpty(message))
      {
        this.sctErrorText.Visible = false;
        this.lblErrorText.Visible = true;

        this.lblErrorText.Text = message;

        this.Session.Remove("paymentErrorMessage");
      }
    }
  }
}