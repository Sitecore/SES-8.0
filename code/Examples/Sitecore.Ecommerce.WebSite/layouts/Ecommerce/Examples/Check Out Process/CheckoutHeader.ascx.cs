// -------------------------------------------------------------------------------------------
// <copyright file="CheckoutHeader.ascx.cs" company="Sitecore Corporation">
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
  #region

  using Sitecore.Resources.Media;
  using System;
  using System.Web.UI;

  #endregion

  /// <summary>
  /// The checkout header.
  /// </summary>
  public partial class CheckoutHeader : UserControl
  {
    #region Protected methods

    /// <summary>
    /// The page_ load.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The event argument.
    /// </param>
    protected void Page_Load(object sender, EventArgs e)
    {
      if (Sitecore.Context.Item.Name == "Customer Details")
      {
        this.headerImage.ImageUrl = HashingUtils.ProtectAssetUrl("/images/ecommerce/checkout_header_first.gif");
      }
      else
      {
        this.headerImage.ImageUrl = HashingUtils.ProtectAssetUrl("/images/ecommerce/checkout_header_last.gif");
      }
    }

    #endregion
  }
}