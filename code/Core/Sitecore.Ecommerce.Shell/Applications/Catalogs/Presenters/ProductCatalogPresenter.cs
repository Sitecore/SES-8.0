// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductCatalogPresenter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The product catalog presenter.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Presenters
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using DomainModel.Configurations;
  using Globalization;
  using Models;
  using Models.Search;
  using Sitecore.Data;
  using Text;
  using Views;

  /// <summary>
  /// The product catalog presenter.
  /// </summary>
  public class ProductCatalogPresenter : CatalogPresenter
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCatalogPresenter"/> class.
    /// </summary>
    /// <param name="view">The product catalog view.</param>
    public ProductCatalogPresenter(IProductCatalogView view)
      : base(view)
    {
      this.View = view;
      this.View.Save += this.Save;
      this.View.Search += this.Search;
      this.View.SelectedProductsGridRowDoubleClick += this.SelectedProductsGridRowDoubleClick;
      this.View.AddButtonClick += this.AddButtonClick;

      this.ProductCatalog = new ProductCatalog
      {
        ItemUri = view.CatalogUri,
        Database = Database.GetDatabase(this.View.CurrentItemUri.DatabaseName),
        Language = this.View.CurrentItemUri.Language
      };

      this.CatalogSettings = new ProductCatalogSettings { ItemUri = this.View.CurrentItemUri };
    }

    /// <summary>
    /// Gets or sets the catalog settings.
    /// </summary>
    /// <value>The catalog settings.</value>
    public ProductCatalogSettings CatalogSettings { get; set; }

    /// <summary>
    /// Gets or sets the product catalog view.
    /// </summary>
    /// <value>The product catalog view.</value>
    public new IProductCatalogView View { get; protected set; }

    /// <summary>
    /// Gets or sets the product catalog definition.
    /// </summary>
    /// <value>The product catalog definition.</value>
    public ProductCatalog ProductCatalog
    {
      get { return (ProductCatalog)this.Catalog; }
      set { this.Catalog = value; }
    }

    /// <summary>
    /// Initializes the view.
    /// </summary>
    public override void InitializeView()
    {
      var searchMethods = this.ProductCatalog.GetSearchMethods();
      foreach (var searchMethod in searchMethods)
      {
        this.View.InitializeSearchMethod(searchMethod.ID.ToString(), searchMethod.Title);
      }

      if (searchMethods.Count > 0 && !ID.IsNullOrEmpty(this.CatalogSettings.SelectionMethod))
      {
        this.View.SelectedSearchMethod = this.CatalogSettings.SelectionMethod.ToString();
      }

      base.InitializeView();

      IEnumerable<GridColumn> columns = this.ProductCatalog.GetGridColumns();
      this.View.InitializeSelectedProductsGrid(columns);

      if (!this.View.IsCallBack)
      {
        this.View.SelectedProducts = this.CatalogSettings.ProductIDs;
      }
    }

    /// <summary>
    /// Called when the view save button clicked.
    /// </summary>
    public void Save()
    {
      this.CatalogSettings.TextBoxes = this.View.TextBoxes;
      this.CatalogSettings.Checklists = this.View.Checklists;

      if (!string.IsNullOrEmpty(this.View.SelectedSearchMethod))
      {
        this.CatalogSettings.SelectionMethod = new ID(this.View.SelectedSearchMethod);
      }

      this.CatalogSettings.ProductIDs = this.View.SelectedProducts;
    }

    /// <summary>
    /// Search click event handler.
    /// </summary>
    public override void Search()
    {
      var options = this.CreateSearchOptions();

      var settingsResolver = Context.Entity.Resolve<SiteSettingsResolver>();
      BusinessCatalogSettings settings;

      var provider = Context.Entity.Resolve<Ecommerce.Search.ISearchProvider>();

      try
      {
        settings = settingsResolver.GetSiteSettings(this.View.CurrentItemUri);
      }
      catch (InvalidOperationException ex)
      {
        Log.Error("Unable to resolve Business Catalog settings.", ex, this);
        return;
      }

      options.SearchRoot = settings.ProductsLink;

      base.Search(options);

      if (this.CatalogSettings.ProductIDs.Count <= 0)
      {
        return;
      }

      var builder = new ItemIDListQueryBuilder(this.CatalogSettings.ProductIDs);

      var converter = new ItemResultDataConverter();
      using (new LanguageSwitcher(this.View.CurrentItemUri.Language))
      {
        var arrangedProducts = this.CatalogSettings.ProductIDs.Join(provider.Search(builder.GetResultQuery()), productId => productId, productItem => productItem.ID.ToString(), (productId, productItem) => productItem);
        var data = converter.Convert(arrangedProducts, this.ProductCatalog.GetGridColumns());
        this.View.FillSelectedProductsGrid(data);
      }
    }

    /// <summary>
    /// Handles textbox creating event.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public override void TextBoxCreating(TextBoxEventArgs args)
    {
      base.TextBoxCreating(args);

      var text = this.CatalogSettings.TextBoxes[args.TextBoxDefinition.Field];
      if (!string.IsNullOrEmpty(text))
      {
        args.Text = text;
      }
    }

    /// <summary>
    /// Handles checklist creating event.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public override void ChecklistCreating(ChecklistEventArgs args)
    {
      base.ChecklistCreating(args);

      var values = this.CatalogSettings.Checklists[args.ChecklistDefinition.Field];
      if (!string.IsNullOrEmpty(values))
      {
        args.CheckedValues = new ListString(values);
      }
    }

    /// <summary>
    /// Search method click event handler.
    /// </summary>
    /// <param name="shortId">The search method short ID.</param>
    public void SearchMethodClicked(string shortId)
    {
      if (this.View.SelectedSearchMethod != shortId)
      {
        this.View.SelectedSearchMethod = shortId;
      }
    }

    /// <summary>
    /// Handles the grid row double click event.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public override void GridRowDoubleClick(GridCommandEventArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (!ID.IsID(args.RowID))
      {
        return;
      }

      if (this.View.SelectedProducts.Contains(args.RowID))
      {
        this.View.ShowProductGridItemDublicatedAlert();
        return;
      }

      this.View.AddRowToSelectedProductsGrid(args.RowID);
    }

    /// <summary>
    /// Handles the grid row double click event.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void SelectedProductsGridRowDoubleClick(GridCommandEventArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (!ID.IsID(args.RowID))
      {
        return;
      }

      this.View.RemoveProductFromSelectedProductsGrid(args.RowID);
    }

    /// <summary>
    /// Handles the grid row double click event.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void AddButtonClick(GridCommandEventArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (args.RowsID == null)
      {
        return;
      }

      var rowIDs = new ListString();
      var showProductGridItemDublicatedAlert = false;

      foreach (var id in args.RowsID.Cast<string>().Where(ID.IsID))
      {
        if (this.View.SelectedProducts.Contains(id))
        {
          showProductGridItemDublicatedAlert = true;
        }
        else
        {
          rowIDs.Add(id);
        }
      }

      if (rowIDs.Count > 0)
      {
        this.View.AddRowToSelectedProductsGrid(rowIDs.ToString());
      }

      if (showProductGridItemDublicatedAlert)
      {
        this.View.ShowProductGridItemDublicatedAlert();
      }
    }
  }
}