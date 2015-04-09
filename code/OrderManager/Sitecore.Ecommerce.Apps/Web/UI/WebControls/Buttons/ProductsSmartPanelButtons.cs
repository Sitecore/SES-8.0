// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductsSmartPanelButtons.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ProductsSmartPanelButtons type.
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
  using Actions;
  using Diagnostics;
  using DomainModel.Products;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using OrderManagement;
  using OrderManagement.ContextStrategies;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.WebControls.Extensions;
  using Speak.Web.UI.WebControls;
  using Speak.WebSite;
  using WebControls;

  /// <summary>
  /// Defines the products task flow buttons class.
  /// </summary>
  public class ProductsSmartPanelButtons : OmSmartPanelButtons
  {
    /// <summary>
    /// ActionKey constant.
    /// </summary>
    private const string ActionKey = "orderlineaction";

    /// <summary>
    /// Strategy for adding of the new order line.
    /// </summary>
    private readonly UblEntityResolvingStrategy strategyForAdding = new OnSmartPanelStrategy();

    /// <summary>
    ///  List of validation messages.
    /// </summary>
    private readonly IList<string> validationMessages = new List<string>();

    /// <summary>
    /// Product price service.
    /// </summary>
    private ProductPriceService priceService;

    /// <summary>
    /// The product repository.
    /// </summary>
    private IProductRepository productRepository;

    /// <summary>
    /// Gets the product code.
    /// </summary>
    [CanBeNull]
    public string ProductCode
    {
      get
      {
        ObjectDetailList products = this.Page.Controls.Flatten<ObjectDetailList>().FirstOrDefault();
        Assert.IsNotNull(products, "Products grid cannot be null.");
        Assert.IsNotNull(products.List, "products.List cannot be null.");
        return products.List.SelectedRows.FirstOrDefault();
      }
    }

    /// <summary>
    /// Gets the quantity.
    /// </summary>
    [CanBeNull]
    public string Quantity
    {
      get
      {
        QuantityBoxRenderer quantityBox = this.Page.Controls.Flatten<QuantityBoxRenderer>().FirstOrDefault();
        Assert.IsNotNull(quantityBox, "Quantity box cannot be null.");
        return quantityBox.Text;
      }
    }

    /// <summary>
    /// Gets the price service.
    /// </summary>
    [NotNull]
    public ProductPriceService PriceService
    {
      get { return this.priceService ?? (this.priceService = new ProductPriceService()); }
    }

    /// <summary>
    /// Gets or sets the product repository.
    /// </summary>
    /// <value>
    /// The product repository.
    /// </value>
    [NotNull]
    public IProductRepository ProductRepository
    {
      get
      {
        return this.productRepository ?? (this.productRepository = Ecommerce.Context.Entity.Resolve<IProductRepository>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.productRepository = value;
      }
    }

    /// <summary>
    /// Validates the controls.
    /// </summary>
    /// <returns>
    /// The controls.
    /// </returns>
    protected bool ValidateControls()
    {
      bool isValid = true;
      this.validationMessages.Clear();

      if (string.IsNullOrEmpty(this.ProductCode))
      {
        this.validationMessages.Add(Texts.SelectAProduct);
        isValid = false;
      }

      if (string.IsNullOrEmpty(this.Quantity))
      {
        this.validationMessages.Add(Texts.EnterAQuantity);
        isValid = false;
      }
      else
      {
        long quantity;
        long.TryParse(this.Quantity, out quantity);

        if (quantity <= 0)
        {
          this.validationMessages.Add(Texts.TheQuantityShouldBeANumberGreaterThan0);
          isValid = false;
        }
      }

      if (!isValid)
      {
        foreach (string message in this.validationMessages)
        {
          ScriptManager.GetCurrent(this.Page).Message(new Message(message) { Sticky = false, Type = MessageType.Error });
        }
      }

      return isValid;
    }

    /// <summary>
    /// Called when the save has click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <exception cref="NotImplementedException">Such action is not supported.</exception>
    /// <exception cref="NotSupportedException">Such action is not supported.</exception>
    protected override void OnSaveClick(object sender, EventArgs e)
    {
      Assert.IsNotNull(this.Page, "Page cannot be null.");
      SmartPanel smartPanel = this.Page as SmartPanel;
      Assert.IsNotNull(smartPanel, "SmartPanel cannot be null.");

      string orderLineAction = smartPanel.Parameters[ActionKey];
      Assert.IsNotNullOrEmpty(orderLineAction, "Order line action is not provided.");
      OrderLineActions action = (OrderLineActions)Enum.Parse(typeof(OrderLineActions), orderLineAction);

      if (!this.ValidateControls())
      {
        base.OnSaveClick(sender, e);
        return;
      }

      ActionContext context = new ActionContext { Owner = this.Page };
      IDictionary<string, object> parameters = new Dictionary<string, object>();
      string message;
      string result;

      switch (action)
      {
        case OrderLineActions.Add:
          {
            this.Strategy = Ecommerce.Context.Entity.Resolve<OrderProcessingStrategy>("AddOrderLine");
            Assert.IsNotNull(this.Strategy, "Cannot resolve the strategy.");

            this.OrderProcessor.OrderProcessingStrategy = this.Strategy;
            this.UblEntityResolver = this.strategyForAdding;
            Order order = this.UblEntityResolver.GetEntity(context) as Order;
            Assert.IsNotNull(order, "Order should be provided.");

            if (!this.CheckPrice(order))
            {
              return;
            }

            parameters.Add("productcode", this.ProductCode);
            parameters.Add("quantity", this.Quantity);
            message = Texts.TheOrderLineHasBeenAddedToTheOrder;

            result = this.OrderProcessor.ProcessOrder(order, parameters);
            break;
          }

        case OrderLineActions.Edit:
          {
            this.Strategy = Ecommerce.Context.Entity.Resolve<OrderProcessingStrategy>("EditOrderLine");
            Assert.IsNotNull(this.Strategy, "Cannot resolve the strategy.");

            this.OrderProcessor.OrderProcessingStrategy = this.Strategy;
            OrderLine orderLine = this.UblEntityResolver.GetEntity(context) as OrderLine;
            Assert.IsNotNull(orderLine, "OrderLine should be provided.");

            if (!this.CheckPrice(orderLine.Order))
            {
              return;
            }

            parameters.Add("orderlineid", orderLine.Alias);
            parameters.Add("productcode", this.ProductCode);
            parameters.Add("quantity", this.Quantity);
            message = Texts.TheOrderLineHasBeenChanged;

            result = this.OrderProcessor.ProcessOrder(orderLine.Order, parameters);
            break;
          }

        default:
          {
            throw new NotSupportedException("Such action is not supported.");
          }
      }

      base.OnSaveClick(sender, e);

      if (result == OrderProcessingStrategy.SuccessfulResult)
      {
        ScriptManager.GetCurrent(this.Page).Message(new Message(message) { Sticky = false, Type = MessageType.Info });
      }
      else if (result == OrderProcessingStrategy.CustomResults.OutOfStock.ToString())
      {
        ScriptManager.GetCurrent(this.Page).Message(new Message(Texts.TheProductIsOutOfStock) { Sticky = false, Type = MessageType.Error });
      }
    }

    /// <summary>
    /// Checks the price.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>
    /// The price.
    /// </returns>
    private bool CheckPrice([NotNull] Order order)
    {
      Assert.ArgumentNotNull(order, "order");

      var product = this.ProductRepository.Get<ProductBaseData>(this.ProductCode);

      var price = this.PriceService.GetPrice(product, order.PricingCurrencyCode);

      if (price == null)
      {
        ScriptManager.GetCurrent(this.Page).Message(new Message(Texts.UnableToPerformTheOperationPriceIsNotAvailable) { Sticky = false, Type = MessageType.Error });
        return false;
      }

      return true;
    }
  }
}