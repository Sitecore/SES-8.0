// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductCatalogView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//  Defines the Product Catalog view.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Views
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Text;
  using System.Web;
  using System.Web.UI.WebControls;
  using ComponentArt.Web.UI;
  using Models;
  using Presenters;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Web.UI.Grids;
  using Sitecore.Web.UI.Sheer;
  using Sitecore.Web.UI.XamlSharp.Ajax;
  using Text;
  using GridColumn = Models.GridColumn;

  /// <summary>
  ///   Defines the Product Catalog form.
  /// </summary>
  public class ProductCatalogView : CatalogView, IProductCatalogView
  {
    /// <summary>
    /// The search method list.
    /// </summary>
    protected DropDownList SearchMethodList;

    /// <summary>
    /// The manually selected products grid.
    /// </summary>
    protected Grid ProductsGrid;

    /// <summary>
    /// Stores the selected products.
    /// </summary>
    protected HiddenField SelectedProductsField;

    /// <summary>
    /// Occurs when Save button clicked.
    /// </summary>
    public event Action Save;

    /// <summary>
    /// Occurs when selected products grid row clicked twice.
    /// </summary>
    public event Action<GridCommandEventArgs> SelectedProductsGridRowDoubleClick;

    /// <summary>
    /// Occurs when Add to selected products button clicked.
    /// </summary>
    public event Action<GridCommandEventArgs> AddButtonClick;

    /// <summary>
    /// Gets or sets the product catalog form presenter.
    /// </summary>
    /// <value></value>
    public new ProductCatalogPresenter Presenter { get; protected set; }

    /// <summary>
    /// Gets or sets the selected search method.
    /// </summary>
    /// <value>The selected search method.</value>
    public string SelectedSearchMethod
    {
      get
      {
        return this.SearchMethodList.SelectedValue;
      }

      set
      {
        foreach (ListItem item in this.SearchMethodList.Items)
        {
          item.Selected = item.Value == value;
        }
      }
    }

    /// <summary>
    /// Gets or sets the selected products.
    /// </summary>
    /// <value>The selected products.</value>
    public ListString SelectedProducts
    {
      get { return new ListString(this.SelectedProductsField.Value); }
      set { this.SelectedProductsField.Value = value.ToString(); }
    }

    /// <summary>
    /// Initializes the search method.
    /// </summary>
    /// <param name="id">The search method id.</param>
    /// <param name="title">The search method title.</param>
    public void InitializeSearchMethod(string id, string title)
    {
      ListItem item = new ListItem { Value = id, Text = title };
      item.Attributes["id"] = id;

      this.SearchMethodList.Items.Add(item);
    }

    /// <summary>
    /// Initializes the selected products grid.
    /// </summary>
    /// <param name="columns">The columns.</param>
    public void InitializeSelectedProductsGrid(IEnumerable<GridColumn> columns)
    {
      this.InitializeGrid(columns, this.ProductsGrid);
    }

    /// <summary>
    /// Fills the selected products grid.
    /// </summary>
    /// <param name="data">The data.</param>
    public void FillSelectedProductsGrid(GridData data)
    {
      ComponentArtGridHandler<List<string>>.Manage(this.ProductsGrid, new GridSource<List<string>>(data.Rows), false);
    }

    /// <summary>
    /// Adds the row to selected products grid.
    /// </summary>
    /// <param name="rowId">The row ID.</param>
    public void AddRowToSelectedProductsGrid(string rowId)
    {
      var selectedProducts = this.SelectedProducts;
      selectedProducts.Add(rowId);
      this.SelectedProducts = selectedProducts;
      SheerResponse.SetAttribute(this.SelectedProductsField.ClientID, "value", selectedProducts.ToString());

      SheerResponse.Eval("scCatalog.moveToSelected('{0}');scCatalog.unSelectAllRows();", rowId);
    }

    /// <summary>
    /// Shows the item duplicated alert.
    /// </summary>
    public void ShowProductGridItemDublicatedAlert()
    {
      SheerResponse.Alert("The product is already selected.");
    }

    /// <summary>
    /// Removes the product from selected products grid.
    /// </summary>
    /// <param name="rowId">The row ID.</param>
    public void RemoveProductFromSelectedProductsGrid(string rowId)
    {
      var selectedProducts = this.SelectedProducts;
      selectedProducts.Remove(rowId);
      this.SelectedProducts = selectedProducts;
      SheerResponse.SetAttribute(this.SelectedProductsField.ClientID, "value", selectedProducts.ToString());

      SheerResponse.Eval("scCatalog.removeSelectedProduct()");
    }

    /// <summary>
    /// Initializes the presenter.
    /// </summary>
    protected override void InitializePresenter()
    {
      this.Presenter = new ProductCatalogPresenter(this);
      this.Presenter.InitializeView();
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      // disabling client event of catalog view application.
      this.Grid.ClientEvents.ItemSelect = null;

      this.ProductsGrid.ItemDataBound += this.OnGridItemDataBound;
    }

    /// <summary>
    /// Handles the OnExecute event of the AjaxScriptManager control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The <see cref="Sitecore.Web.UI.XamlSharp.Ajax.AjaxCommandEventArgs"/> instance containing the event data.</param>
    protected override void OnAjaxScriptManagerExecute(object sender, AjaxCommandEventArgs args)
    {
      base.OnAjaxScriptManagerExecute(sender, args);

      switch (args.Name)
      {
        case "item:save":
        case "notification:itemsaved":
          {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["saveProducts"]))
            {
              this.OnSave();
            }

            break;
          }

        case "productcatalog:spgridrowdoubleclick":
          {
            var gridArgs = new GridCommandEventArgs { RowID = args.Parameters["rowID"] };
            this.OnSelectedProductsGridRowDoubleClick(gridArgs);
            break;
          }

        case "productcatalog:addbuttonclick":
          {
            var gridArgs = new GridCommandEventArgs { RowsID = new StringCollection() };
            var rowIDs = new ListString(args.Parameters["rowIDs"]);

            if (rowIDs.Count > 0)
            {
              gridArgs.RowID = rowIDs[0];
            }

            gridArgs.RowsID.AddRange(rowIDs.Items);

            this.OnAddButtonClick(gridArgs);

            break;
          }
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      var script = new StringBuilder();
      script.AppendFormat("var scCatalog = new ProductCatalog('{0}');", this.SelectedProductsField.ClientID);
      script.AppendFormat("var SearchMethodList = $('{0}');", this.SearchMethodList.ClientID);

      this.Page.ClientScript.RegisterStartupScript(this.GetType(), "productcatalog", script.ToString(), true);
    }

    /// <summary>
    /// Called when save button clicked.
    /// </summary>
    protected virtual void OnSave()
    {
      if (this.Save != null)
      {
        this.Save();
      }
    }

    /// <summary>
    /// Called when the selected products grid row double has click.
    /// </summary>
    /// <param name="args">The arguments.</param>
    protected virtual void OnSelectedProductsGridRowDoubleClick(GridCommandEventArgs args)
    {
      if (this.SelectedProductsGridRowDoubleClick != null)
      {
        this.SelectedProductsGridRowDoubleClick(args);
      }
    }

    /// <summary>
    /// Called when Add to selected products button clicked.
    /// </summary>
    /// <param name="args">The arguments.</param>
    protected virtual void OnAddButtonClick(GridCommandEventArgs args)
    {
      if (this.AddButtonClick != null)
      {
        this.AddButtonClick(args);
      }
    }
  }
}