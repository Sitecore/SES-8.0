// -------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartAndOrderView.ascx.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess
{
  using System;
  using System.Collections.Generic;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using Diagnostics;
  using DomainModel.Orders;
  using Examples.CheckOutProcess;
  using Globalization;
  using Sections;
  using Sitecore.Data.Items;
  using Sitecore.Ecommerce.Payments;
  using Sitecore.Exceptions;

  /// <summary>
  /// ShoppingCart and order view user control.
  /// </summary>
  public partial class ShoppingCartAndOrderView : UserControl
  {
    #region Constants

    /// <summary> The template name order section row query. </summary>
    private const string TemplateNameOrderSectionRowQuery = "./*[@@templatename='Order Section Row']";

    /// <summary> The template name order sections and row query. </summary>
    private const string TemplateNameOrderSectionsAndRowQuery = "./*[@@templatename='Order Sections']/*[@@templatename='Order Section Row']";

    /// <summary> The template name order sections query. </summary>
    private const string TemplateNameOrderSectionsQuery = "./*[@@templatename='Order Sections']";

    #endregion

    #region Public properties

    /// <summary> Gets or sets DisplayMode. </summary>
    public OrderDisplayMode CurrentOrderDisplayMode { get; set; }

    /// <summary>
    /// Gets or sets the data entity.
    /// </summary>
    /// <value>The data entity.</value>
    public object DataEntity { get; set; }

    #endregion

    #region Protected methods

    /// <summary> Handles the Load event of the Page control. </summary>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e"> The <see cref="System.EventArgs"/> instance containing the event data. </param>
    protected void Page_Load(object sender, EventArgs e)
    {
      string useOtherOrderSection = "Use Other Order Section";

      // Initializes the OrderSections
      this.ProductsListView.DisplayMode = this.CurrentOrderDisplayMode;
      if (this.DataEntity != null)
      {
        if (this.DataEntity is Order)
        {
          Order order = this.DataEntity as Order;

          this.ProductsListView.ProductLines = order.OrderLines;
          this.ProductsListView.Currency = order.Currency;
          this.summary.Totals = order.Totals;
          this.summary.Currency = order.Currency;
          this.delivery.ShippingProvider = order.ShippingProvider;
          this.delivery.Currency = order.Currency;

          this.lblOrderNumber.InnerText = string.Format("{0}: {1}", Translate.Text(Sitecore.Ecommerce.Examples.Texts.OrderNumber), order.OrderNumber);

          string cardType = order.CustomerInfo.CustomProperties[TransactionConstants.CardType];

          if (!string.IsNullOrEmpty(cardType))
          {
            this.lblCardType.InnerText = string.Format("{0}: {1}", Translate.Text(Sitecore.Ecommerce.Examples.Texts.CardType), cardType); 
          }
        }
      }
      else
      {
        return;
      }

      Item[] rowItems = Sitecore.Context.Item.Axes.SelectItems(TemplateNameOrderSectionsAndRowQuery);
      if (rowItems == null)
      {
        Item orderSectionItem = Sitecore.Context.Item.Axes.SelectSingleItem(TemplateNameOrderSectionsQuery);
        string id = orderSectionItem[useOtherOrderSection];
        Assert.IsTrue(Sitecore.Data.ID.IsID(id), string.Format("The '{0}' is not a valid id.", useOtherOrderSection));
        Item useOtherOrderSectionItem = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(id));
        rowItems = useOtherOrderSectionItem.Axes.SelectItems(TemplateNameOrderSectionRowQuery);
      }

      if (rowItems == null)
      {
        return;
      }

      for (int i = 0; i < rowItems.Length; i++)
      {
        string cssClass = "content2";
        Item rowItem = rowItems[i];

        // Set the CSS class on the row div.
        string cssClassID = rowItem["Css Class"];

        if (Sitecore.Data.ID.IsID(cssClassID))
        {
          Item cssClassItem = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(cssClassID));
          if (cssClassItem != null)
          {
            cssClass = cssClassItem.Name;
          }
        }

        string attributeClass = "class";
        if (i == 0)
        {
          this.divRow1.Attributes.Add(attributeClass, cssClass);
        }
        else
        {
          this.divRow2.Attributes.Add(attributeClass, cssClass);
        }

        // Find max height
        string maxHeightForSections = "Max height for sections";
        string maxHeight = rowItem[maxHeightForSections];
        int height = -1;
        if (!string.IsNullOrEmpty(maxHeight) && !int.TryParse(maxHeight, out height))
        {
#if DEBUG
          throw new ConfigurationException(string.Format("The field '{0}' wasn't a valid integer to be used as height", maxHeightForSections));
#else
                            height = 188;
#endif
        }

        int addedSectionItems = 0;
        switch (i)
        {
          case 0:
            addedSectionItems = 0;
            break;
          case 1:
            addedSectionItems = 3;
            break;
        }

        // Set each OrderSections
        foreach (Item orderSectionItem in rowItem.Children)
        {
          List<SectionTableRow> tableRows = SectionFactory.GetSection(orderSectionItem, this.DataEntity);
          string title = Utils.ItemUtil.GetTitleOrDictionaryEntry(orderSectionItem, true);

          // try
          // {
          // title = ItemUtil.GetTitleOrDictionaryEntry(orderSectionItem, true);
          // }
          // catch (Exception)
          // {
          // // Do nothing since it's okay with a empty title of a section.
          // }
          bool hideSection = orderSectionItem["Hide Section"] == "1";
          bool showLabelColumn = orderSectionItem["Show Label Column"] == "1";

          if (!hideSection)
          {
            addedSectionItems += 1;
            string heightFormat = "height: {0}px;";
            string classLabels = "labels";
            string classNoLabels = "noLabels";
            string attributeStyle = "style";
            switch (addedSectionItems)
            {
              case 1:
                this.litTitleSection1.Text = title;
                this.repeaterOrderSection1.DataSource = tableRows;
                this.repeaterOrderSection1.DataBind();
                this.divSection1.Visible = true;
                if (height != -1)
                {
                  this.divSection1.Attributes.Add(attributeStyle, string.Format(heightFormat, height));
                }

                if (showLabelColumn)
                {
                  this.ddSection1.Attributes.Add(attributeClass, classLabels);
                }
                else
                {
                  this.ddSection1.Attributes.Add(attributeClass, classNoLabels);
                }

                break;
              case 2:
                this.litTitleSection2.Text = title;
                this.repeaterOrderSection2.DataSource = tableRows;
                this.repeaterOrderSection2.DataBind();
                this.divSection2.Visible = true;
                if (height != -1)
                {
                  this.divSection2.Attributes.Add(attributeStyle, string.Format(heightFormat, height));
                }

                if (showLabelColumn)
                {
                  this.ddSection2.Attributes.Add(attributeClass, classLabels);
                }
                else
                {
                  this.ddSection2.Attributes.Add(attributeClass, classNoLabels);
                }

                break;
              case 3:
                this.litTitleSection3.Text = title;
                this.repeaterOrderSection3.DataSource = tableRows;
                this.repeaterOrderSection3.DataBind();
                this.divSection3.Visible = true;
                if (height != -1)
                {
                  this.divSection3.Attributes.Add(attributeStyle, string.Format(heightFormat, height));
                }

                if (showLabelColumn)
                {
                  this.ddSection3.Attributes.Add(attributeClass, classLabels);
                }
                else
                {
                  this.ddSection3.Attributes.Add(attributeClass, classNoLabels);
                }

                break;
              case 4:
                this.litTitleSection4.Text = title;
                this.repeaterOrderSection4.DataSource = tableRows;
                this.repeaterOrderSection4.DataBind();
                this.divSection4.Visible = true;
                if (height != -1)
                {
                  this.divSection4.Attributes.Add(attributeStyle, string.Format(heightFormat, height));
                }

                if (showLabelColumn)
                {
                  this.ddSection4.Attributes.Add(attributeClass, classLabels);
                }
                else
                {
                  this.ddSection4.Attributes.Add(attributeClass, classNoLabels);
                }

                break;
              case 5:
                this.litTitleSetion5.Text = title;
                this.repeaterOrderSection5.DataSource = tableRows;
                this.repeaterOrderSection5.DataBind();
                this.divSection5.Visible = true;
                if (height != -1)
                {
                  this.divSection5.Attributes.Add(attributeStyle, string.Format(heightFormat, height));
                }

                if (showLabelColumn)
                {
                  this.ddSection5.Attributes.Add(attributeClass, classLabels);
                }
                else
                {
                  this.ddSection5.Attributes.Add(attributeClass, classNoLabels);
                }

                break;
              case 6:
                this.litTitleSection6.Text = title;
                this.repeaterOrderSection6.DataSource = tableRows;
                this.repeaterOrderSection6.DataBind();
                this.divSection6.Visible = true;
                if (height != -1)
                {
                  this.divSection6.Attributes.Add(attributeStyle, string.Format(heightFormat, height));
                }

                if (showLabelColumn)
                {
                  this.ddSection6.Attributes.Add(attributeClass, classLabels);
                }
                else
                {
                  this.ddSection6.Attributes.Add(attributeClass, classNoLabels);
                }

                break;
              default:
                break;
            }
          }
        }
      }
    }

    /// <summary>
    /// Handles the ItemDataBound event of the RepeaterOrderSection control.
    /// </summary>
    /// <param name="sender">
    /// The source of the event.
    /// </param>
    /// <param name="e">
    /// The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.
    /// </param>
    protected void RepeaterOrderSection_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
      Control label = e.Item.FindControl("tdLabel");
      if (e.Item.DataItem is SectionTableRow)
      {
        SectionTableRow tableRow = e.Item.DataItem as SectionTableRow;
        if (!tableRow.ShowLabelColumn)
        {
          if (label != null)
          {
            label.Visible = false;
          }
        }
      }
    }

    #endregion
  }
}