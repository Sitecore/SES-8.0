// -------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartAndOrderViewMail.ascx.cs" company="Sitecore Corporation">
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
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using Diagnostics;
  using Examples.CheckOutProcess;
  using Sections;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Exceptions;

  public partial class ShoppingCartAndOrderViewMail : UserControl
  {
    private const string TemplateNameOrderSectionsAndRowQuery = "./*[@@templatename='Order Sections']/*[@@templatename='Order Section Row']";
    private const string TemplateNameOrderSectionsQuery = "./*[@@templatename='Order Sections']";
    private const string TemplateNameOrderSectionRowQuery = "./*[@@templatename='Order Section Row']";

    /// <summary>
    /// Gets or sets the current order display mode.
    /// </summary>
    /// <value>The current order display mode.</value>
    public OrderDisplayMode CurrentOrderDisplayMode { set; get; }

    /// <summary>
    /// Gets or sets the data entity.
    /// </summary>
    /// <value>The data entity.</value>
    public object DataEntity { get; set; }

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      const string useOtherOrderSection = "Use Other Order Section";
      if (!IsPostBack)
      {
        // Initializes the OrderSections
        this.ProductsListView.DisplayMode = CurrentOrderDisplayMode;

        var rowItems =
            Sitecore.Context.Item.Axes.SelectItems(TemplateNameOrderSectionsAndRowQuery);


        if (rowItems == null)
        {
          var orderSectionItem = Sitecore.Context.Item.Axes.SelectSingleItem(
              TemplateNameOrderSectionsQuery);
          var id = orderSectionItem[useOtherOrderSection];
          Assert.IsTrue(Sitecore.Data.ID.IsID(id), string.Format("The '{0}' is not a valid id.", useOtherOrderSection));
          var useOtherOrderSectionItem = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(id));
          rowItems = useOtherOrderSectionItem.Axes.SelectItems(TemplateNameOrderSectionRowQuery);
        }

        if (rowItems != null)
        {
          for (var i = 0; i < rowItems.Length; i++)
          {
            const string classContent = "content";
            const string classContent2 = "content2";
            const string classContent3 = "content3";
            const string attributeClass = "class";

            var cssClass = classContent2;
            var rowItem = rowItems[i];
            // Set the CSS class on the row div.
            var cssClassID = rowItem["Css Class"];
            var id = new ID(cssClassID);
            var cssClassItem = Sitecore.Context.Database.GetItem(id);
            if (cssClassItem != null)
            {
              cssClass = cssClassItem.Name;
            }
            //try
            //{
            //    ID id = new ID(cssClassID);
            //    Item cssClassItem = Sitecore.Context.Database.GetItem(id);
            //    if (cssClassItem != null)
            //    {
            //        cssClass = cssClassItem.Name;
            //    }
            //}
            //catch (Exception)
            //{

            //}

            if (i == 0)
            {
              divRow1.Attributes.Add(attributeClass, cssClass);
            }
            else
            {
              divRow2.Attributes.Add(attributeClass, cssClass);
            }

            // Find max height
            const string maxHeightForSections = "Max height for sections";
            var maxHeight = rowItem[maxHeightForSections];
            var height = -1;
            if (!string.IsNullOrEmpty(maxHeight) && !int.TryParse(maxHeight, out height))
            {
              throw new ConfigurationException(string.Format("The field '{0}' wasn't a valid integer to be used as height", maxHeightForSections));
            }

            var addedSectionItems = 0;
            if (i == 0)
            {
              // The first row
              addedSectionItems = 0;
            }
            else if (i == 1)
            {
              // The second row
              addedSectionItems = 3;
            }
            //else
            //{
            //    // Not implemented in ascx file.
            //}


            // Set each OrderSections
            foreach (Item orderSectionItem in rowItem.Children)
            {
              var tableRows = SectionFactory.GetSection(orderSectionItem, this.DataEntity);
              var title = Utils.ItemUtil.GetTitleOrDictionaryEntry(orderSectionItem, true);
              //try
              //{
              //    title = ItemUtil.GetTitleOrDictionaryEntry(orderSectionItem, true);
              //}
              //catch (Exception)
              //{
              //    // Do nothing since it's okay with a empty title of a section.
              //}
              var hideSection = orderSectionItem["Hide Section"] == "1";
              var showLabelColumn = orderSectionItem["Show Label Column"] == "1";

              if (!hideSection)
              {
                addedSectionItems += 1;
                const string classLabels = "labels";
                const string classNoLabels = "noLabels";
                const string widthFormat = "width: {0}%;";
                const string heightFormat = "height: {0}px;";
                const string attributeStyle = "style";
                const string emptyTr = "</tr><tr>";
                const int widthA = 50;
                const int widthB = 33;
                switch (addedSectionItems)
                {
                  case 1:
                    if (cssClass == classContent2)
                    {
                      divSection1.Attributes.Add(attributeStyle, string.Format(widthFormat, widthA));
                    }
                    else if (cssClass == classContent3)
                    {
                      divSection1.Attributes.Add(attributeStyle, string.Format(widthFormat, widthB));
                    }
                    litTitleSection1.Text = title;
                    repeaterOrderSection1.DataSource = tableRows;
                    repeaterOrderSection1.DataBind();
                    divSection1.Visible = true;
                    //if(height!=-1)
                    //{
                    //    divSection1.Attributes.Add("style", string.Format("height: {0}px;", height));
                    //}
                    if (showLabelColumn)
                      ddSection1.Attributes.Add(attributeClass, classLabels);
                    else
                    {
                      ddSection1.Attributes.Add(attributeClass, classNoLabels);
                    }
                    break;
                  case 2:
                    if (cssClass == classContent)
                    {
                      phSection1.Controls.Add(new LiteralControl(emptyTr));
                    }
                    if (cssClass == classContent2)
                      divSection2.Attributes.Add(attributeStyle, string.Format(widthFormat, widthA));
                    else if (cssClass == classContent3)
                      divSection2.Attributes.Add(attributeStyle, string.Format(widthFormat, widthB));
                    litTitleSection2.Text = title;
                    repeaterOrderSection2.DataSource = tableRows;
                    repeaterOrderSection2.DataBind();
                    divSection2.Visible = true;
                    //if (height != -1)
                    //{
                    //    divSection2.Attributes.Add("style", string.Format("height: {0}px;", height));
                    //}
                    if (showLabelColumn)
                      ddSection2.Attributes.Add(attributeClass, classLabels);
                    else
                    {
                      ddSection2.Attributes.Add(attributeClass, classNoLabels);
                    }
                    break;
                  case 3:
                    if (cssClass == classContent)
                    {
                      phSection2.Controls.Add(new LiteralControl(emptyTr));
                    }
                    if (cssClass == classContent2)
                      divSection3.Attributes.Add(attributeStyle, string.Format(widthFormat, widthA));
                    else if (cssClass == classContent3)
                      divSection3.Attributes.Add(attributeStyle, string.Format(widthFormat, widthB));
                    litTitleSection3.Text = title;
                    repeaterOrderSection3.DataSource = tableRows;
                    repeaterOrderSection3.DataBind();
                    divSection3.Visible = true;
                    //if (height != -1)
                    //{
                    //    divSection3.Attributes.Add("style", string.Format("height: {0}px;", height));
                    //}
                    if (showLabelColumn)
                      ddSection3.Attributes.Add(attributeClass, classLabels);
                    else
                    {
                      ddSection3.Attributes.Add(attributeClass, classNoLabels);
                    }
                    break;
                  case 4:
                    if (cssClass == classContent2)
                      divSection4.Attributes.Add(attributeStyle, string.Format(widthFormat, widthA));
                    else if (cssClass == classContent3)
                      divSection4.Attributes.Add(attributeStyle, string.Format(widthFormat, widthB));
                    litTitleSection4.Text = title;
                    repeaterOrderSection4.DataSource = tableRows;
                    repeaterOrderSection4.DataBind();
                    divSection4.Visible = true;
                    //if (height != -1)
                    //{
                    //    divSection4.Attributes.Add("style", string.Format("height: {0}px;", height));
                    //}
                    if (showLabelColumn)
                      ddSection4.Attributes.Add(attributeClass, classLabels);
                    else
                    {
                      ddSection4.Attributes.Add(attributeClass, classNoLabels);
                    }
                    break;
                  case 5:
                    if (cssClass == classContent)
                    {
                      phSection4.Controls.Add(new LiteralControl(emptyTr));
                    }
                    if (cssClass == classContent2)
                      divSection5.Attributes.Add(attributeStyle, string.Format(widthFormat, widthA));
                    else if (cssClass == classContent3)
                      divSection5.Attributes.Add(attributeStyle, string.Format(widthFormat, widthB));
                    litTitleSetion5.Text = title;
                    repeaterOrderSection5.DataSource = tableRows;
                    repeaterOrderSection5.DataBind();
                    divSection5.Visible = true;
                    //if (height != -1)
                    //{
                    //    divSection5.Attributes.Add("style", string.Format("height: {0}px;", height));
                    //}
                    if (showLabelColumn)
                      ddSection5.Attributes.Add(attributeClass, classLabels);
                    else
                    {
                      ddSection5.Attributes.Add(attributeClass, classNoLabels);
                    }
                    break;
                  case 6:
                    if (cssClass == classContent)
                    {
                      phSection5.Controls.Add(new LiteralControl(emptyTr));
                    }
                    if (cssClass == classContent2)
                      divSection6.Attributes.Add(attributeStyle, string.Format(widthFormat, widthA));
                    else if (cssClass == classContent3)
                      divSection6.Attributes.Add(attributeStyle, string.Format(widthFormat, widthB));
                    litTitleSection6.Text = title;
                    repeaterOrderSection6.DataSource = tableRows;
                    repeaterOrderSection6.DataBind();
                    divSection6.Visible = true;
                    if (height != -1)
                    {
                      divSection6.Attributes.Add(attributeStyle, string.Format(heightFormat, height));
                    }
                    if (showLabelColumn)
                      ddSection6.Attributes.Add(attributeClass, classLabels);
                    else
                    {
                      ddSection6.Attributes.Add(attributeClass, classNoLabels);
                    }
                    break;
                  default:
                    break;
                }
              }
            }
          }
        }
      }
    }

    protected void RepeaterOrderSection_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
      var tdLabel = e.Item.FindControl("tdLabel");
      if (e.Item.DataItem is SectionTableRow)
      {
        var tableRow = e.Item.DataItem as SectionTableRow;
        if (!tableRow.ShowLabelColumn)
        {
          if (tdLabel != null)
          {
            tdLabel.Visible = false;
          }
        }
      }
    }
  }
}