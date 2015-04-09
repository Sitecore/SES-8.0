// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatalogPresenter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The base catalog presenter.
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
  using System.Collections.Generic;
  using Diagnostics;
  using Models;
  using Models.Search;
  using Sitecore.Data;
  using Views;

  /// <summary>
  /// The base catalog presenter.
  /// </summary>
  public class CatalogPresenter
  {
    /// <summary>
    /// Provides access to the catalog configuration.
    /// </summary>
    private Catalog catalog;

    /// <summary>
    /// The list of textbox definitions.
    /// </summary>
    private List<TextBoxDefinition> textBoxDefinitions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CatalogPresenter"/> class.
    /// </summary>
    /// <param name="view">The order catalog view.</param>
    public CatalogPresenter(ICatalogView view)
    {
      this.View = view;
      this.catalog = new Catalog
      {
        ItemUri = this.View.CatalogUri,
        Database = Database.GetDatabase(this.View.CurrentItemUri.DatabaseName),
        Language = this.View.CurrentItemUri.Language
      };

      this.View.Search += this.Search;
      this.View.TextBoxCreating += this.TextBoxCreating;
      this.View.ChecklistCreating += this.ChecklistCreating;
      this.View.GridRowDoubleClick += this.GridRowDoubleClick;
      this.View.GridRowSelect += this.GridRowSelect;
    }

    /// <summary>
    /// Gets or sets the catalog view.
    /// </summary>
    /// <value>The catalog view.</value>
    public ICatalogView View { get; protected set; }

    /// <summary>
    /// Gets or sets the catalog definition.
    /// </summary>
    /// <value>The catalog definition.</value>
    public virtual Catalog Catalog
    {
      get { return this.catalog; }
      set { this.catalog = value; }
    }

    /// <summary>
    /// Gets the textbox definitions.
    /// </summary>
    /// <value>The textbox definitions.</value>
    private List<TextBoxDefinition> TextBoxDefinitions
    {
      get
      {
        return this.textBoxDefinitions ?? (this.textBoxDefinitions = this.Catalog.GetTextBoxDefinitions());
      }
    }

    /// <summary>
    /// Initializes the view.
    /// </summary>
    public virtual void InitializeView()
    {
      var ribbounSourceUri = this.Catalog.GetRibbonSourceUri();
      if (ribbounSourceUri != null)
      {
        this.View.RibbonSourceUri = ribbounSourceUri;
      }

      if (this.Catalog.EditorFields != null)
      {
        this.View.EditorFields = this.Catalog.EditorFields;
      }

      if (!this.View.IsCallBack)
      {
        this.View.InitializeTextBoxes(this.TextBoxDefinitions);
        this.View.InitializeChecklists(this.Catalog.GetChecklistDefinitions());
      }

      IEnumerable<GridColumn> columns = this.Catalog.GetGridColumns();
      this.View.InitializeGrid(columns);

      if (!this.View.IsCallBack)
      {
        this.Search();
      }
    }

    /// <summary>
    /// Searches this instance.
    /// </summary>
    public virtual void Search()
    {
      var options = this.CreateSearchOptions();
      this.Search(options);
    }

    /// <summary>
    /// Search click event handler.
    /// </summary>
    /// <param name="options">The options.</param>
    public virtual void Search(SearchOptions options)
    {
      var source = this.Catalog.CatalogSource;
      if (source == null)
      {
        return;
      }

      source.SearchOptions = options;
      this.View.FillGrid(source);
    }

    /// <summary>
    /// Gets the search options.
    /// </summary>
    /// <returns>The search options.</returns>
    public virtual SearchOptions CreateSearchOptions()
    {
      var options = new SearchOptions
      {
        SearchRoot = this.View.CurrentItemPath,
        SearchFields = this.View.TextBoxes,
        Checklists = this.View.Checklists,
        GridColumns = this.Catalog.GetGridColumns(),
        Templates = this.Catalog.GetSearchTemplates()
      };

      return options;
    }

    /// <summary>
    /// Handles textbox creating event.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void TextBoxCreating(TextBoxEventArgs args)
    {
      if (this.TextBoxDefinitions.IndexOf(args.TextBoxDefinition) == 0)
      {
        args.IsWide = true;
      }
    }

    /// <summary>
    /// Handles checklist creating event.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void ChecklistCreating(ChecklistEventArgs args)
    {
    }

    /// <summary>
    /// Handles the grid row double click event.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void GridRowDoubleClick(GridCommandEventArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (string.IsNullOrEmpty(args.RowID) || !ID.IsID(args.RowID))
      {
        return;
      }

      this.View.ShowDetails(args.RowID);
    }

    /// <summary>
    /// Grids the row select.
    /// </summary>
    /// <param name="args">The <see cref="GridCommandEventArgs"/> instance containing the event data.</param>
    public virtual void GridRowSelect(GridCommandEventArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (args.RowsID == null)
      {
        return;
      }

      this.View.SelectedRowsId = args.RowsID;
      this.View.UpdateRibbon();
    }
  }
}