// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OmSmartPanelButtons.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OmSmartPanelButtons type.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls.Buttons
{
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Merchant.OrderManagement;
  using OrderManagement.ContextStrategies;
  using Speak.Web.UI.WebControls;

  /// <summary>
  /// Defines the smart panel buttons class.
  /// </summary>
  public abstract class OmSmartPanelButtons : SmartPanelButtons
  {
    /// <summary>
    ///  Merchant order processor.
    /// </summary>
    private MerchantOrderProcessor orderProcessor;

    /// <summary>
    /// Order line resolver.
    /// </summary>
    private UblEntityResolvingStrategy ublEntityResolver;

    /// <summary>
    /// Gets or sets the strategy.
    /// </summary>
    /// <value>
    /// The strategy.
    /// </value>
    [NotNull]
    public virtual OrderProcessingStrategy Strategy { get; set; }

    /// <summary>
    /// Gets or sets the order processor.
    /// </summary>
    /// <value>
    /// The order processor.
    /// </value>
    [NotNull]
    public virtual MerchantOrderProcessor OrderProcessor
    {
      get
      {
        return this.orderProcessor ?? (this.orderProcessor = Ecommerce.Context.Entity.Resolve<MerchantOrderProcessor>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.orderProcessor = value;
      }
    }

    /// <summary>
    /// Gets or sets the order processor.
    /// </summary>
    /// <value>
    /// The order processor.
    /// </value>
    [NotNull]
    public virtual UblEntityResolvingStrategy UblEntityResolver
    {
      get
      {
        return this.ublEntityResolver ?? (this.ublEntityResolver = new OnSmartPanelOrderLineStrategy());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.ublEntityResolver = value;
      }
    }

    /// <summary>
    /// Initializes the leave without saving warning.
    /// </summary>
    protected override void InitializeLeaveWithoutSavingWarning()
    {
    }
  }
}