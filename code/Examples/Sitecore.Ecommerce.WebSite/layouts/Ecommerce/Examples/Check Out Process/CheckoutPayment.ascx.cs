// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckoutPayment.ascx.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The checkout payment page
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.UI;
  using System.Web.UI.HtmlControls;
  using System.Web.UI.WebControls;
  using Analytics.Components;
  using CheckOuts;
  using Diagnostics;
  using DomainModel.CheckOuts;
  using DomainModel.Data;
  using DomainModel.Orders;
  using DomainModel.Payments;
  using Globalization;
  using Sitecore.Pipelines;
  using Utils;
  using Texts = Sitecore.Ecommerce.Examples.Texts;

  /// <summary>
  /// The checkout payment page.
  /// </summary>
  public partial class CheckoutPayment : UserControl
  {
    /// <summary>
    /// Codes of offline payment methods
    /// </summary>
    private readonly string[] offlinePaymentCodes = new[]
    {
      "MoneyTransfer", "PayByCheck"
    };

    /// <summary>
    /// The name of the default payment method field
    /// </summary>
    private const string defaultPaymentMethodFieldName = "Default Payment Method";

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <exception cref="ArgumentException">List of payment methods is empty.</exception>
    protected void Page_Load(object sender, EventArgs e)
    {
      if (this.IsPostBack)
      {
        return;
      }

      // Checks if the user has appropiate access.      
      ICheckOut checkOut = Sitecore.Ecommerce.Context.Entity.GetInstance<ICheckOut>();

      if ((checkOut == null) || !(checkOut is CheckOut) || !((CheckOut)checkOut).BeginCheckOut || !((CheckOut)checkOut).NameAndAddressDone || !((CheckOut)checkOut).DeliveryDone)
      {
        this.Response.Redirect("/");
      }

      this.lblFormTitle.Text = Translate.Text(Texts.Payment);
      this.lblFormDescription.Text = Translate.Text(Texts.PleaseSelectAPaymentMethod);
      this.lblpaymentMethods.Text = string.Concat(Translate.Text(Texts.PaymentMethod), ": ");

      ListItem item = new ListItem { Text = "      ", Value = "nonSelected", Selected = true };
      this.ddlPaymentMethods.Items.Insert(0, item);

      this.btnConfirm.Text = Translate.Text(Texts.ConfirmPayment);

      IOrderManager<Order> orderProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IOrderManager<Order>>();
      DomainModel.Carts.ShoppingCart shoppingCart = Sitecore.Ecommerce.Context.Entity.GetInstance<DomainModel.Carts.ShoppingCart>();

      // Sets the ordernumber.
      if (string.IsNullOrEmpty(shoppingCart.OrderNumber))
      {
        string orderno = orderProvider.GenerateOrderNumber();
        if (string.IsNullOrEmpty(orderno))
        {
          orderno = DateTime.Now.ToString("yyyyMMdd HHmmss");
        }

        shoppingCart.OrderNumber = orderno;

        Sitecore.Ecommerce.Context.Entity.SetInstance(shoppingCart);
      }

      IEntityProvider<PaymentSystem> paymentMethodProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IEntityProvider<PaymentSystem>>();
      Assert.IsNotNull(paymentMethodProvider, "Payment methods provider is null");

      IEnumerable<PaymentSystem> paymentMethods = paymentMethodProvider.GetAll();
      if (paymentMethods.IsNullOrEmpty())
      {
        throw new ArgumentException("List of payment methods is empty.");
      }

      this.repeaterPaymentMethods.DataSource = paymentMethods;
      this.repeaterPaymentMethods.DataBind();

      this.AddOnlinePayMethods();
      this.AddOfflinePayMethods();
      this.SetDefaultPaymentMethod();
    }

    /// <summary>
    /// Handles the Click event of the ConfirmButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void ConfirmButton_Click(object sender, EventArgs e)
    {
      AnalyticsUtil.CheckoutPaymentNext();

      PipelineArgs pipelineArgs = new PipelineArgs();
      CorePipeline.Run("paymentStarted", pipelineArgs);

      this.btnConfirm.Enabled = false;
    }

    /// <summary>
    /// Handles the ItemDataBound event of the repeaterPaymentMethods control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
    protected void repeaterPaymentMethods_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
      HtmlInputControl input = e.Item.FindControl("paymentMethod") as HtmlInputControl;

      PaymentSystem method = e.Item.DataItem as PaymentSystem;
      string code = string.Empty;
      if (method != null)
      {
        code = method.Code;
      }

      if (input != null)
      {
        input.Value = code;
      }

      if (this.ddlPaymentMethods.SelectedItem != null)
      {
        if (string.Compare(code, this.ddlPaymentMethods.SelectedItem.Value, true) != 0)
        {
          HtmlControl container = e.Item.FindControl("paymentMethodContainer") as HtmlControl;
          if (container != null)
          {
            container.Attributes["style"] = "display:none;";
          }
        }
      }
    }

    /// <summary>
    /// Adds the offline pay methods.
    /// </summary>
    private void AddOfflinePayMethods()
    {
      this.PopulatePaymentMethods(this.offlinePaymentCodes, Translate.Text(Texts.OfflinePayments));
    }

    /// <summary>
    /// Adds the online pay methods.
    /// </summary>
    private void AddOnlinePayMethods()
    {
      IEnumerable<string> onlinePaymentMethodCodes = ((IEnumerable<PaymentSystem>)this.repeaterPaymentMethods.DataSource)
        .Select(d => d.Code)
        .Except(this.offlinePaymentCodes);

      this.PopulatePaymentMethods(onlinePaymentMethodCodes, Translate.Text(Texts.PaymentProviders));
    }

    /// <summary>
    /// Gets the code of the default payment method
    /// </summary>
    /// <returns>The code of the default payment method</returns>
    private string GetDefaultPaymentMethodCode()
    {
      Sitecore.Data.Fields.Field field = Sitecore.Context.Item.Fields[defaultPaymentMethodFieldName];

      if (field != null)
      {
        IEntityProvider<PaymentSystem> paymentMethodProvider = Sitecore.Ecommerce.Context.Entity.Resolve<IEntityProvider<PaymentSystem>>();
        PaymentSystem payment = paymentMethodProvider.GetAll().OfType<Payments.PaymentSystem>().Where(paymentSystem => paymentSystem.Alias == field.Value).SingleOrDefault();

        if (payment != null)
        {
          return payment.Code;
        }
      }

      return string.Empty;
    }

    /// <summary>
    /// Sets the default payment method
    /// </summary>
    private void SetDefaultPaymentMethod()
    {
      string code = this.GetDefaultPaymentMethodCode();

      if (!string.IsNullOrEmpty(code))
      {
        this.ddlPaymentMethods.SelectedValue = code;
      }
    }

    /// <summary>
    /// Populates the payment methods.
    /// </summary>
    /// <param name="methodCodes">The method codes.</param>
    /// <param name="methodGroupName">Name of the method group.</param>
    private void PopulatePaymentMethods(IEnumerable<string> methodCodes, string methodGroupName)
    {
      Assert.ArgumentNotNull(methodCodes, "methodCodes");
      Assert.ArgumentNotNullOrEmpty(methodGroupName, "methodGroupName");

      foreach (string code in methodCodes)
      {
        PaymentSystem method = (from s in (IEnumerable<PaymentSystem>)this.repeaterPaymentMethods.DataSource
                                where s.Code == code
                                select s).FirstOrDefault();

        if (method == null)
        {
          continue;
        }

        ListItem item = new ListItem { Text = method.Title, Value = method.Code };
        item.Attributes["OptionGroup"] = methodGroupName;
        this.ddlPaymentMethods.Items.Add(item);
      }
    }
  }
}