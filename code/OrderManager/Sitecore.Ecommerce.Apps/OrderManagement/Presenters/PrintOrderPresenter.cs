// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintOrderPresenter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the print order presenter class.
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
  using Ecommerce.OrderManagement.Orders;
  using Merchant.OrderManagement;
  using Report;
  using Stimulsoft.Report;
  using Views;

  /// <summary>
  /// Defines the print order presenter class.
  /// </summary>
  public class PrintOrderPresenter
  {
    /// <summary>
    /// The print order view.
    /// </summary>
    private readonly IPrintOrderView view;

    /// <summary>
    /// The order repository.
    /// </summary>
    private readonly MerchantOrderManager orderRepository;

    /// <summary>
    /// The report factory.
    /// </summary>
    private readonly StiReportFactory reportFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrintOrderPresenter"/> class.
    /// </summary>
    /// <param name="view">The view.</param>
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="reportFactory">The report factory.</param>
    public PrintOrderPresenter([NotNull] IPrintOrderView view, [NotNull] MerchantOrderManager orderRepository, [NotNull] StiReportFactory reportFactory)
    {
      Assert.ArgumentNotNull(view, "view");
      Assert.ArgumentNotNull(orderRepository, "orderRepository");
      Assert.ArgumentNotNull(reportFactory, "reportFactory");

      this.view = view;
      this.orderRepository = orderRepository;
      this.reportFactory = reportFactory;
    }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    public void Initialize()
    {
      Order order = this.orderRepository.GetOrder(this.view.OrderID);
      if (order != null)
      {
        this.view.Model.Order = order;
        StiReport report = this.reportFactory.CreateReport(this.view.Model);

        this.view.Report = report;
      }
      else
      {
        this.view.ShowOrderNotFoundError();
      }
    }
  }
}