// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineSmartPanelButtons.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineSmartPanelButtons type.
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
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.UI;
  using ComponentModel;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using OrderManagement.Models;
  using Sitecore.Speak.WebSite;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.WebControls.Extensions;

  /// <summary>
  /// Defines the order line smart panel buttons class.
  /// </summary>
  public class OrderLineSmartPanelButtons : ObservableOmSmartPanelButtons<OrderLineModel>
  {
    /// <summary>
    /// Name of the default processing strategy.
    /// </summary>
    private const string DefaultStrategyName = "ChangeOrderLineQuantity";

    /// <summary>
    /// OrderProcessingStrategy instance.
    /// </summary>
    private OrderProcessingStrategy strategy;

    /// <summary>
    /// Gets or sets the strategy.
    /// </summary>
    /// <value>
    /// The strategy.
    /// </value>
    public override OrderProcessingStrategy Strategy
    {
      get
      {
        return this.strategy ?? (this.strategy = Ecommerce.Context.Entity.Resolve<OrderProcessingStrategy>(DefaultStrategyName));
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.strategy = value;
      }
    }

    /// <summary>
    /// Gets the smart panel renderer.
    /// </summary>
    public SmartPanelRenderer SmartPanelRenderer 
    {
      get
      {
        return this.Page.Controls.Flatten<SmartPanelRenderer>().FirstOrDefault();
      }
    }

    /// <summary>
    /// Raises the <see cref="E:Init"/> event.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      if (this.SmartPanelRenderer != null)
      {
        this.SmartPanelRenderer.SmartPanel.PopupClose += this.PopupClosed;
      }
    }

    /// <summary>
    /// Called when the save has click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The arguments.</param>
    protected override void OnSaveClick(object sender, EventArgs args)
    {
      base.OnSaveClick(sender, args);

      this.PerformUpdate();
    }

    /// <summary>
    /// Performs the update.
    /// </summary>
    protected virtual void PerformUpdate()
    {
      OrderLineModel orderLineModel = this.TrackedEntities.FirstOrDefault();
      if (orderLineModel != null)
      {
        decimal quantityBefore = orderLineModel.Quantity;
        Binder.Current.GetModelBinderFactory().GetBinder(typeof(OrderLineModel)).BindToModel(orderLineModel, this.GetEntityChanges(orderLineModel));
        decimal quantityAfter = orderLineModel.Quantity;

        if (quantityAfter <= 0)
        {
          ScriptManager.GetCurrent(this.Page).Message(new Message(Texts.TheQuantityShouldBeANumberGreaterThan0) { Sticky = false, Type = MessageType.Error });
          return;
        }

        if (quantityAfter != quantityBefore)
        {
          OrderLine orderLine = this.UblEntityResolver.GetEntity(orderLineModel.Alias) as OrderLine;
          Assert.IsNotNull(orderLine, "OrderLine cannot be null.");
          IDictionary<string, object> parameters = new Dictionary<string, object>
          {
            { "orderlineid", orderLineModel.Alias },
            { "quantity", orderLineModel.Quantity }
          };

          this.OrderProcessor.OrderProcessingStrategy = this.Strategy;
          string result = this.OrderProcessor.ProcessOrder(orderLine.Order, parameters);

          if (result == OrderProcessingStrategy.SuccessfulResult)
          {
            ScriptManager.GetCurrent(this.Page).Message(new Message(Texts.TheOrderLineQuantityHasBeenChanged) { Sticky = false, Type = MessageType.Info });
          }
          else if (result == OrderProcessingStrategy.CustomResults.OutOfStock.ToString())
          {
            ScriptManager.GetCurrent(this.Page).Message(new Message(Texts.TheProductIsOutOfStock) { Sticky = false, Type = MessageType.Error });
          }
        }
      }
    }

    /// <summary>
    /// Popups the closed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The arguments.</param>
    private void PopupClosed(object sender, EventArgs args)
    {
      var panel = this.Page as SmartPanel;
      if (panel != null)
      {
        panel.Close();
      }
    }
  }
}