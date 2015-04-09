// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderItemEventHandler.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderItemEventHandler class.
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

namespace Sitecore.Ecommerce.Orders
{
  using System.Collections.Generic;
  using System.Linq;
  using Configuration;
  using Diagnostics;
  using DomainModel.Orders;
  using Shell.Framework.Pipelines;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Proxies;
  using Sitecore.Pipelines.Save;
  using Sitecore.Web.UI.Sheer;
  using Sites;
  using Text;
  using Utils;

  public class OrderItemEventHandler
  {
    /// <summary>
    /// Order template ID.
    /// </summary>
    private readonly ID orderTemplateId = new ID(Settings.GetSetting("Ecommerce.Order.OrderItemTempalteId"));

    /// <summary>
    ///    Order Line template ID.
    /// </summary>
    private readonly ID orderLineTemplateId = new ID(Settings.GetSetting("Ecommerce.Order.OrderLineItemTempalteId"));

    /// <summary>
    /// Order manager.
    /// </summary>
    private IOrderManager<DomainModel.Orders.Order> orderManager;

    /// <summary>
    /// Gets the order manager.
    /// </summary>
    /// <value>The order manager.</value>
    protected virtual IOrderManager<DomainModel.Orders.Order> OrderManager
    {
      get { return this.orderManager ?? (this.orderManager = Context.Entity.Resolve<IOrderManager<DomainModel.Orders.Order>>()); }
    }

    /// <summary>
    /// Called when the item has saved.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public void OnItemSaved([NotNull] SaveArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      foreach (Item item in args.SavedItems)
      {
        this.RecalculateOrder(item);
      }
    }

    /// <summary>
    /// Called when the item has duplicated.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public void OnItemDuplicated([NotNull] ClientPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      Item item = Sitecore.Context.Database.GetItem(args.Parameters["id"]);
      if (item != null)
      {
        this.RecalculateOrder(item);
      }
    }

    /// <summary>
    /// Called when the item has copied.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public void OnItemCopied([NotNull] CopyItemsArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      foreach (Item item in args.Copies)
      {
        this.RecalculateOrder(item);
      }
    }

    /// <summary>
    /// Called when the item has deleted.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void OnItemDeleted([NotNull] ClientPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      foreach (Item item in this.GetItems(args))
      {
        if (item.TemplateID == this.orderTemplateId)
        {
          return;
        }

        if (item.TemplateID != this.orderLineTemplateId)
        {
          continue;
        }

        string site = SiteUtils.GetSiteByItem(item.Parent);
        if (string.IsNullOrEmpty(site))
        {
          return;
        }

        SiteContext siteContext = SiteContextFactory.GetSiteContext(site);
        using (new SiteContextSwitcher(siteContext))
        {
          DomainModel.Orders.Order order = this.GetOrder(item.Parent);
          order.OrderLines.Remove(order.OrderLines.Where(ol => ol.Id == item.ID.ToString()).FirstOrDefault());

          this.RecalculateOrder(order);
        }
      }
    }

    /// <summary>
    /// Recalculates the order.
    /// </summary>
    /// <param name="item">The item.</param>
    protected virtual void RecalculateOrder([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      if (item.TemplateID.ToString().ToLower() == Settings.GetSetting("Ecommerce.Order.OrderItemTempalteId").ToLower() || item.TemplateID.ToString().ToLower() == Settings.GetSetting("Ecommerce.Order.OrderLineItemTempalteId").ToLower())
      {
        string site = SiteUtils.GetSiteByItem(item);
        SiteContext siteContext = SiteContextFactory.GetSiteContext(site);
        using (new SiteContextSwitcher(siteContext))
        {
          DomainModel.Orders.Order order = this.GetOrder(item);
          this.RecalculateOrder(order);
        }
      }
    }

    /// <summary>
    /// Recalculates the order.
    /// </summary>
    /// <param name="order">The order.</param>
    protected virtual void RecalculateOrder(DomainModel.Orders.Order order)
    {
      if (order == null)
      {
        return;
      }

      OrderPriceCalculator manager = Context.Entity.Resolve<OrderPriceCalculator>();

      manager.Recalculate(ref order);

      this.OrderManager.SaveOrder(order);
    }

    /// <summary>
    /// Gets the order item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>Returns Order.</returns>
    protected virtual DomainModel.Orders.Order GetOrder([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      Item orderItem = null;

      if (item.TemplateID == this.orderTemplateId || item.TemplateID == this.orderLineTemplateId)
      {
        orderItem = item.TemplateID == this.orderLineTemplateId ? item.Parent : item;
      }

      if (orderItem == null)
      {
        return null;
      }

      return OrderManager.GetOrder(orderItem["OrderNumber"]);
    }

    /// <summary>
    /// Gets the items.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>Retuns list of items to be deleted.</returns>
    protected virtual List<Item> GetItems([NotNull] ClientPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      Database database = GetDatabase(args);
      List<Item> result = (from itemId in new ListString(args.Parameters["items"], '|')
                           select database.GetItem(itemId)
                             into item
                             where item != null
                             select MapToRealItem(item)).ToList();

      return Assert.ResultNotNull(result);
    }

    /// <summary>
    /// Gets the database.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>Returns Items database.</returns>
    protected virtual Database GetDatabase([NotNull] ClientPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      Database database = Factory.GetDatabase(args.Parameters["database"]);
      Assert.IsNotNull(database, typeof(Database), "Name: {0}", new object[] { args.Parameters["database"] });
      return Assert.ResultNotNull(database);
    }

    /// <summary>
    /// Maps to real item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>Returns real item.</returns>
    private static Item MapToRealItem([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      return ProxyManager.GetRealItem(item, false);
    }
  }
}