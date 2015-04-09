// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintOrder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the print order class.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls
{
  using System;
  using System.Web.UI.WebControls;
  using Diagnostics;
  using Merchant.OrderManagement;
  using OrderManagement.Presenters;
  using OrderManagement.Views;
  using Report;
  using Stimulsoft.Report;
  using Stimulsoft.Report.Web;

  /// <summary>
  /// Defines the print order class.
  /// </summary>
  public class PrintOrder : CompositeControl, IPrintOrderView
  {
    /// <summary>
    /// The report viewer.
    /// </summary>
    private StiWebViewer reportViewer;

    /// <summary>
    /// The print order presenter.
    /// </summary>
    private PrintOrderPresenter presenter;

    /// <summary>
    /// Gets the order ID.
    /// </summary>
    /// <value>
    /// The order ID.
    /// </value>
    public string OrderID
    {
      get { return Context.Request.QueryString["orderid"]; }
    }

    /// <summary>
    /// Gets or sets the report.
    /// </summary>
    /// <value>
    /// The report.
    /// </value>
    public StiReport Report
    {
      get { return this.reportViewer.Report; }
      set { this.reportViewer.Report = value; }
    }

    /// <summary>
    /// Gets or sets the model.
    /// </summary>
    /// <value>
    /// The model.
    /// </value>
    public OrderReportModel Model { get; set; }

    /// <summary>
    /// Shows the order not found error.
    /// </summary>
    public void ShowOrderNotFoundError()
    {
    }

    /// <summary>
    /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
    /// </summary>
    protected override void CreateChildControls()
    {
      base.CreateChildControls();

      this.reportViewer = new StiWebViewer { ID = "ReportViewer" };
      this.Controls.Add(this.reportViewer);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      Debug.ArgumentNotNull(e, "e");

      base.OnLoad(e);

      var repository = Ecommerce.Context.Entity.Resolve<MerchantOrderManager>();
      var reportFactory = Ecommerce.Context.Entity.Resolve<StiReportFactory>();
      this.Model = Ecommerce.Context.Entity.Resolve<OrderReportModel>();

      this.presenter = new PrintOrderPresenter(this, repository, reportFactory);
      this.presenter.Initialize();
    }
  }
}