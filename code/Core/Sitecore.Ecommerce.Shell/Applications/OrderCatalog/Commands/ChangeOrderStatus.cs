// -------------------------------------------------------------------------------------------
// <copyright file="ChangeOrderStatus.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Shell.Applications.OrderCatalog.Commands
{
  using System.Collections.Specialized;
  using Catalogs.Views;
  using DomainModel.Data;
  using DomainModel.Orders;  
  using Sitecore.Shell.Framework.Commands;  

  /// <summary>
  /// Change Order status command
  /// </summary>
  public class ChangeOrderStatus : Command
  {
    /// <summary>
    /// Executes the command in the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public override void Execute(CommandContext context)
    {
      string orderNumber = context.Parameters["orderNumber"];
      string statusCode = context.Parameters["statusCode"];
      
      IOrderManager<Order> orderProvider = Context.Entity.Resolve<IOrderManager<Order>>();
      Order order = orderProvider.GetOrder(orderNumber);

      order.Status = Context.Entity.Resolve<OrderStatus>(statusCode);
      order.ProcessStatus();
      
      orderProvider.SaveOrder(order);
      
      ICatalogView catalogView = context.CustomData as ICatalogView;
      if (catalogView != null)
      {
        IEntity orderEntity = order as IEntity;
        if (orderEntity != null)
        {
          catalogView.SelectedRowsId = new StringCollection { orderEntity.Alias };
          catalogView.RefreshGrid();
          catalogView.UpdateRibbon();          
        }
      }      
    }
  }
}