// -------------------------------------------------------------------------------------------
// <copyright file="OrderStatusPanelView.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Shell.Applications.OrderCatalog.Views
{
  using System.Collections.Generic;
  using System.Web.UI;
  using Catalogs.Views;
  using DomainModel.Orders;
  using Globalization;
  using Models;
  using Presenters;
  using Sitecore.Data.Items;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Shell.Web.UI.WebControls;
  using Sitecore.Web.UI.WebControls.Ribbons;

  /// <summary>
  /// Order Statuses Panel View
  /// </summary>
  public class OrderStatusPanelView : RibbonPanel, IOrderStatusPanelView
  {
    /// <summary>
    /// Gets or sets the presenter.
    /// </summary>
    /// <value>The presenter.</value>
    public virtual OrderStatusPanelPresenter Presenter { get; protected set; }

    /// <summary>
    /// Gets or sets the catalog view.
    /// </summary>
    /// <value>The catalog view.</value>
    public ICatalogView CatalogView { get; set; }

    /// <summary>
    /// Renders the panel.
    /// </summary>
    /// <param name="output">The output writer.</param>
    /// <param name="ribbon">The ribbon.</param>
    /// <param name="button">The button.</param>
    /// <param name="context">The context.</param>
    public override void Render(HtmlTextWriter output, Ribbon ribbon, Item button, CommandContext context)
    {
      this.CatalogView = context.CustomData as ICatalogView;
      this.Initilize();

      Order currentOrder = this.Presenter.CurrentOrder();
      if (currentOrder != null)
      {
        RenderText(output, this.GetText(currentOrder));
        IEnumerable<OrderStatusCommand> commands = this.Presenter.GetOrderStatusesCommands(currentOrder.Status);
        Sitecore.Context.ClientPage.ClientResponse.DisableOutput();
        foreach (OrderStatusCommand command in commands)
        {
          this.RenderSmallButton(output, ribbon, string.Empty, command.Title, command.Icon, command.Title, new OrderStatusCommandBuilder(currentOrder, command).ToString(), this.Enabled, false);
        }

        Sitecore.Context.ClientPage.ClientResponse.EnableOutput();
      }
    }

    /// <summary>
    /// Gets the text.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>Returns texts for orders panel.</returns>
    protected virtual string GetText(Order order)
    {
      OrderStatusCommand statusCommand = this.Presenter.GetOrderStatusCommand(order.Status);

      if (statusCommand != null)
      {
        return Translate.Text(Texts.TheOrderIsInTheXState, new object[] { string.Format("<br/><b> {0} </b>", statusCommand.Title) });
      }

      return string.Format("<b>{0}</b>", Translate.Text(Texts.TheOrderHasNoState));
    }

    /// <summary>
    /// Initilizes this instance.
    /// </summary>
    private void Initilize()
    {
      this.Presenter = new OrderStatusPanelPresenter(this);
    }
  }
}