// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICatalogView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Catalog view interface.
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
  using Models;
  using Sitecore.Data;
  using Sitecore.Ecommerce.Search;
  using Sitecore.Ecommerce.Shell.Applications.Catalogs.Models.Search;
  using Sitecore.Web.UI.Grids;

  /// <summary>
  /// Represents catalog view.
  /// </summary>
  public interface ICatalogView
  {
    /// <summary>
    /// Occurs when search is started.
    /// </summary>
    event Action Search;

    /// <summary>
    /// Occurs when text box creating.
    /// </summary>
    event Action<TextBoxEventArgs> TextBoxCreating;

    /// <summary>
    /// Occurs when checklist creating.
    /// </summary>
    event Action<ChecklistEventArgs> ChecklistCreating;

    /// <summary>
    /// Occurs when grid row clicked twice.
    /// </summary>
    event Action<GridCommandEventArgs> GridRowDoubleClick;

    /// <summary>
    /// Occurs when [grid row selected].
    /// </summary>
    event Action<GridCommandEventArgs> GridRowSelect;

    /// <summary>
    /// Gets a value indicating whether the view is being rendered for the first time or is being loaded in response to a callback.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the page is being loaded in response to a client callback; otherwise, <c>false</c>.
    /// </value>
    bool IsCallBack { get; }

    /// <summary>
    /// Gets the catalog item URI. The URI is used to find all the catalog settings.
    /// By default catalogs stored in '/sitecore/system/Modules/Ecommerce/Catalogs' folder.
    /// </summary>
    /// <value>The catalog ID.</value>
    ItemUri CatalogUri { get; }

    /// <summary>
    /// Gets the current item URI.
    /// </summary>
    /// <value>The current item URI.</value>
    ItemUri CurrentItemUri { get; }

    /// <summary>
    /// Gets or sets the selected rows id.
    /// </summary>
    /// <value>The selected rows id.</value>
    StringCollection SelectedRowsId { get; set; }

    /// <summary>
    /// Gets or sets the editor fields.
    /// </summary>
    /// <value>The editor fields.</value>
    StringCollection EditorFields { get; set; }

    /// <summary>
    /// Gets the current item path.
    /// </summary>
    /// <value>The current item path.</value>
    string CurrentItemPath { get; }

    /// <summary>
    /// Gets or sets the ribbon source URI.
    /// </summary>
    /// <value>The ribbon source URI.</value>
    ItemUri RibbonSourceUri { get; set; }

    /// <summary>
    /// Gets the search textboxes NameValueCollection.
    /// </summary>
    /// <value>The search query.</value>
    NameValueCollection TextBoxes { get; }

    /// <summary>
    /// Gets the seleced search checklists in a uri format.
    /// </summary>
    /// <value>The search checklists.</value>
    NameValueCollection Checklists { get; }

    /// <summary>
    /// Initializes the search text boxes.
    /// </summary>
    /// <param name="textBoxDefinitions">The list of text box definitions. Used to build the text box controls.</param>
    void InitializeTextBoxes(IEnumerable<TextBoxDefinition> textBoxDefinitions);

    /// <summary>
    /// Initializes the checklists. Checklists provides additional search result filtering functionlity.
    /// It's stored in the catalog settings folder.
    /// </summary>
    /// <param name="checklists">The checklists.</param>
    void InitializeChecklists(IEnumerable<ChecklistDefinition> checklists);

    /// <summary>
    /// Initializes the grid.
    /// </summary>
    /// <param name="columns">The columns list.</param>
    void InitializeGrid(IEnumerable<Models.GridColumn> columns);

    /// <summary>
    /// Fills the grid.
    /// </summary>
    /// <param name="data">The grid data.</param>
    void FillGrid(GridData data);

    /// <summary>
    /// Handles the grid row double click.
    /// </summary>
    /// <param name="rowId">The row ID.</param>
    void ShowDetails(string rowId);

    /// <summary>
    /// Updates the ribbon.
    /// </summary>
    void UpdateRibbon();

    /// <summary>
    /// Refreshes the grid.
    /// </summary>
    void RefreshGrid();

    /// <summary>
    /// Fills the grid.
    /// </summary>
    /// <param name="source">The source.</param>
    void FillGrid(IGridSource<List<string>> source);
  }
}