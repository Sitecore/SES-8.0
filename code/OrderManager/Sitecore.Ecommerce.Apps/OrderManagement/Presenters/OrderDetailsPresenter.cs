// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderDetailsPresenter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order details presenter class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Presenters
{
  using Diagnostics;
  using Merchant.OrderManagement;
  using Views;

  /// <summary>
  /// Defines the order details presenter class.
  /// </summary>
  public class OrderDetailsPresenter
  {
    /// <summary>
    /// The view.
    /// </summary>
    private readonly IOrderDetailsView view;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderDetailsPresenter"/> class.
    /// </summary>
    /// <param name="view">The view.</param>
    public OrderDetailsPresenter([NotNull] IOrderDetailsView view)
    {
      Assert.ArgumentNotNull(view, "view");

      this.view = view;
    }

    /// <summary>
    /// Gets or sets the order security.
    /// </summary>
    /// <value>The order security.</value>
    public MerchantOrderSecurity OrderSecurity { get; set; }

    /// <summary>
    /// Gets the view.
    /// </summary>
    /// <value>
    /// The view.
    /// </value>
    public IOrderDetailsView View
    {
      get
      {
        return this.view;
      }
    }

    /// <summary>
    /// Gets or sets the state substate checker.
    /// </summary>
    /// <value>
    /// The state substate checker.
    /// </value>
    [NotNull]
    public virtual OrderStateListValidator OrderStateListValidator { get; set; }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    internal void Initialize()
    {
      this.view.IsReadOnly = (this.OrderSecurity == null) || (this.view.Order == null) || !this.OrderSecurity.CanProcess(this.view.Order);
      this.view.CanReopenOrder = (this.OrderSecurity != null) && (this.view.Order != null) && this.OrderSecurity.CanReopen(this.view.Order);

      if (this.view.Order != null)
      {
        this.OrderStateListValidator.ShowWarningIfCapturedInFullNotAvailable(this.view.Order);
      }
    }
  }
}