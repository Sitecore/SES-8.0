// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductCatalogView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the IProductCatalogView interface. This interface determines the base view for all product catalogs.
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
  using Models;
  using Text;

  /// <summary>
  /// Defines the IProductCatalogView interface. This interface determines the base view for all product catalogs.
  /// </summary>
  public interface IProductCatalogView : ICatalogView
  {
    /// <summary>
    /// Occurs when Save button clicked.
    /// </summary>
    event Action Save;

    /// <summary>
    /// Occurs when selected products grid row clicked twice.
    /// </summary>
    event Action<GridCommandEventArgs> SelectedProductsGridRowDoubleClick;

    /// <summary>
    /// Occurs when Add to selected products button clicked.
    /// </summary>
    event Action<GridCommandEventArgs> AddButtonClick;

    /// <summary>
    /// Gets or sets the selected search method.
    /// </summary>
    /// <value>The selected search method.</value>
    string SelectedSearchMethod { get; set; }

    /// <summary>
    /// Gets or sets the selected products.
    /// </summary>
    /// <value>The selected products.</value>
    ListString SelectedProducts { get; set; }

    /// <summary>
    /// Initializes the search method.
    /// </summary>
    /// <param name="id">The search method id.</param>
    /// <param name="title">The search method title.</param>
    void InitializeSearchMethod(string id, string title);

    /// <summary>
    /// Initializes the selected products grid.
    /// </summary>
    /// <param name="columns">The columns.</param>
    void InitializeSelectedProductsGrid(IEnumerable<GridColumn> columns);

    /// <summary>
    /// Fills the selected products grid.
    /// </summary>
    /// <param name="data">The data.</param>
    void FillSelectedProductsGrid(GridData data);

    /// <summary>
    /// Adds the row to selected products grid.
    /// </summary>
    /// <param name="rowId">The row ID.</param>
    void AddRowToSelectedProductsGrid(string rowId);

    /// <summary>
    /// Shows the item dublicated alert.
    /// </summary>
    void ShowProductGridItemDublicatedAlert();

    /// <summary>
    /// Removes the product from selected products grid.
    /// </summary>
    /// <param name="rowId">The row ID.</param>
    void RemoveProductFromSelectedProductsGrid(string rowId);
  }
}