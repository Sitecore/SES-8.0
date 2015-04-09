// -------------------------------------------------------------------------------------------
// <copyright file="SendMailToAdmin.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Pipelines.OrderCreated
{
  using Diagnostics;
  using DomainModel.Configurations;
  using DomainModel.Mails;
  using DomainModel.Orders;
  using Sitecore.Data.Items;
  using Sitecore.Pipelines;
  using Unity;

  /// <summary>
  /// The send mail to admin processor.
  /// </summary>
  public class SendMailToAdmin
  {
    /// <summary>
    /// Processes the specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void Process(PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      Order order = args.CustomData["order"] as Order;

      if (order == null)
      {
        return;
      }

      BusinessCatalogSettings businnesCatalogSettings = Context.Entity.GetConfiguration<BusinessCatalogSettings>();
      Item companyMasterData = Sitecore.Context.Database.SelectSingleItem(businnesCatalogSettings.CompanyMasterDataLink);
      if (companyMasterData == null)
      {
        return;
      }

      string queryString = string.Format("orderid={0}&mode=mail", order.OrderNumber);
      Item saleDepartment = companyMasterData.Children["Sale"];
      var saleParams = new { Recipient = saleDepartment["Email"] };

      IMail mailProvider = Context.Entity.Resolve<IMail>();
      mailProvider.SendMail("Order Mail To Admin", saleParams, queryString);
    }
  }
}