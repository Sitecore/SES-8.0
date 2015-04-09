// -------------------------------------------------------------------------------------------
// <copyright file="OrderStatusPanelPresenter.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Shell.Applications.OrderCatalog.Presenters
{
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using DomainModel.Configurations;
  using DomainModel.Data;
  using DomainModel.Orders;
  using Ecommerce.Search;
  using Models;
  using Sitecore.Data.Items;
  using Text;
  using Views;

  /// <summary>
  /// Order Statuses panel presenter.
  /// </summary>
  public class OrderStatusPanelPresenter
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStatusPanelPresenter"/> class.
    /// </summary>
    /// <param name="view">The panel view.</param>
    public OrderStatusPanelPresenter(IOrderStatusPanelView view)
    {
      this.View = view;
    }

    /// <summary>
    /// Gets or sets the view.
    /// </summary>
    /// <value>The panel view.</value>
    public IOrderStatusPanelView View { get; set; }

    /// <summary>
    /// Gets the order statuses commands.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns>Returns the order statuses commands.</returns>
    public virtual IEnumerable<OrderStatusCommand> GetOrderStatusesCommands(OrderStatus status)
    {
      Assert.ArgumentNotNull(status, "status");

      return this.GetAvailableFollowingStatuses(status).Select<OrderStatus, OrderStatusCommand>(this.GetOrderStatusCommand);
    }

    public virtual OrderStatusCommand GetOrderStatusCommand(OrderStatus status)
    {
      Assert.ArgumentNotNull(status, "status");
      IEntity entity = status as IEntity;

      if (entity != null && !string.IsNullOrEmpty(entity.Alias))
      {
        Item statusItem = Sitecore.Context.ContentDatabase.GetItem(entity.Alias);
        if (statusItem != null)
        {
          return new OrderStatusCommand(statusItem["Code"], statusItem.Appearance.Icon, statusItem["Title"]);
        }
      }

      return null;
    }

    protected virtual IEnumerable<OrderStatus> GetAvailableFollowingStatuses(OrderStatus status)
    {
      Assert.ArgumentNotNull(status, "status");
      IEntity entity = status as IEntity;

      if (entity != null && !string.IsNullOrEmpty(entity.Alias))
      {
        Item orderStatusItem = Sitecore.Context.ContentDatabase.GetItem(entity.Alias);
        Assert.IsNotNull(orderStatusItem, string.Concat("Order status item is null. ID: '", entity.Alias, "'"));
        ListString avalibleStatusesIds = new ListString(orderStatusItem["Available List"]);

        foreach (string statusId in avalibleStatusesIds)
        {
          Item statusItem = Sitecore.Context.ContentDatabase.SelectSingleItem(statusId);
          if (statusItem == null)
          {
            yield return default(OrderStatus);
          }

          OrderStatus orderStatus = Context.Entity.Resolve<OrderStatus>(statusItem["Code"]);
          ((IEntity)orderStatus).Alias = statusItem.ID.ToString();
          yield return orderStatus;
        }
      }
      else
      {
        BusinessCatalogSettings settings = Context.Entity.GetConfiguration<BusinessCatalogSettings>();
        if (settings != null && !string.IsNullOrEmpty(settings.OrderStatusesLink))
        {
          Item orderStatusesRoot = Sitecore.Context.ContentDatabase.SelectSingleItem(settings.OrderStatusesLink);
          if (orderStatusesRoot != null)
          {
            foreach (Item statusItem in orderStatusesRoot.Children)
            {
              OrderStatus orderStatus = Context.Entity.Resolve<OrderStatus>(statusItem["Code"]);
              ((IEntity)orderStatus).Alias = statusItem.ID.ToString();
              yield return orderStatus;
            }
          }
        }
      }
    }

    /// <summary>
    /// Currents the order.
    /// </summary>
    /// <returns>returns the current selected order</returns>
    public virtual Order CurrentOrder()
    {
      IOrderManager<Order> orderProvider = Context.Entity.Resolve<IOrderManager<Order>>();
      if (this.View.CatalogView.SelectedRowsId != null && this.View.CatalogView.SelectedRowsId.Count == 1)
      {
        Query query = new Query();
        query.AppendAttribute("id", this.View.CatalogView.SelectedRowsId[0], MatchVariant.Exactly);
        return orderProvider.GetOrders(query).FirstOrDefault();
      }

      return null;
    }
  }
}