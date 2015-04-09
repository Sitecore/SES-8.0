// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Catalog.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Represents catalog configuration. By default stored in /system/modules/Ecommerce/Catalogs/[Catalog Name] item.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using Data;
  using Globalization;
  using Search;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Text;

  /// <summary>
  /// Represents catalog configuration. By default stored in /system/modules/Ecommerce/Catalogs/[Catalog Name] item.
  /// </summary>
  public class Catalog
  {
    /// <summary>
    /// The catalog data source field name.
    /// </summary>
    public const string CatalogDataSourceFieldName = "Catalog Data Source";

    /// <summary>
    /// The data mapper.
    /// </summary>
    private readonly IDataMapper dataMapper;

    /// <summary>
    /// The catalog item.
    /// </summary>
    private Item catalogItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="Catalog"/> class.
    /// </summary>
    public Catalog()
      : this(Context.Entity.Resolve<DataMapper>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Catalog"/> class.
    /// </summary>
    /// <param name="dataMapper">The data mapper.</param>
    public Catalog(IDataMapper dataMapper)
    {
      this.dataMapper = dataMapper;
    }

    /// <summary>
    /// Gets or sets the item URI.
    /// </summary>
    /// <value>The item URI.</value>
    public ItemUri ItemUri { get; set; }

    /// <summary>
    /// Gets or sets the database.
    /// </summary>
    /// <value>
    /// The database.
    /// </value>
    public Database Database { get; set; }

    /// <summary>
    /// Gets or sets Language.
    /// </summary>
    public Language Language { get; set; }

    /// <summary>
    /// Gets the search provider.
    /// </summary>
    /// <value>The search provider.</value>
    public virtual CatalogBaseSource CatalogSource
    {
      get
      {
        if (this.CatalogItem != null && !string.IsNullOrEmpty(this.CatalogItem[CatalogDataSourceFieldName]))
        {
          CatalogBaseSource source = this.CreateCatalogSource();
          if (source != null)
          {
            source.Database = this.Database;
            source.Language = this.Language;

            return source;
          }
        }

        return null;
      }
    }

    /// <summary>
    /// Gets the row editor fields.
    /// </summary>
    /// <value>The row editor fields.</value>
    public virtual StringCollection EditorFields
    {
      get
      {
        return this.GetFieldValueList("Editor Fields").ToStringCollection();
      }
    }

    /// <summary>
    /// Gets the data mapper.
    /// </summary>
    [NotNull]
    protected IDataMapper DataMapper
    {
      get { return this.dataMapper; }
    }

    /// <summary>
    /// Gets the catalog item.
    /// </summary>
    /// <value>The catalog item.</value>
    protected Item CatalogItem
    {
      get
      {
        if (this.catalogItem != null)
        {
          return this.catalogItem;
        }

        if (this.ItemUri == null)
        {
          return null;
        }

        this.catalogItem = Database.GetItem(this.ItemUri);
        return this.catalogItem;
      }
    }

    /// <summary>
    /// Gets the ribbon URI.
    /// </summary>
    /// <returns>The ribbon URI.</returns>
    public virtual ItemUri GetRibbonSourceUri()
    {
      var uri = this.CatalogItem["Ribbon Source Uri"];

      return string.IsNullOrEmpty(uri) ? null : new ItemUri(uri);
    }

    /// <summary>
    /// Gets the textbox definitions.
    /// </summary>
    /// <returns>The textbox definitions.</returns>
    public virtual List<TextBoxDefinition> GetTextBoxDefinitions()
    {
      var list = new List<TextBoxDefinition>();
      if (this.CatalogItem == null)
      {
        return list;
      }

      var searchFieldsRoot = this.CatalogItem.Children["Search Text Fields"];
      if (searchFieldsRoot == null || searchFieldsRoot.Children.Count <= 0)
      {
        return list;
      }

      foreach (Item searchTextField in searchFieldsRoot.Children)
      {
        list.Add(this.dataMapper.GetEntity<TextBoxDefinition>(searchTextField));
      }

      return list;
    }

    /// <summary>
    /// Gets the search templates.
    /// </summary>
    /// <returns>The search templates</returns>
    public virtual ListString GetSearchTemplates()
    {
      return this.GetFieldValueList("Templates");
    }

    /// <summary>
    /// Gets the checklists.
    /// </summary>
    /// <returns>Gets the checklist definitions.</returns>
    public virtual List<ChecklistDefinition> GetChecklistDefinitions()
    {
      var checklists = new ChecklistCollection();

      if (this.CatalogItem == null)
      {
        return checklists;
      }

      var checklistsRoot = this.CatalogItem.Children["Checklists"];
      if (checklistsRoot == null || checklistsRoot.Children.Count <= 0)
      {
        return checklists;
      }

      foreach (Item checklistItem in checklistsRoot.Children)
      {
        var checkboxes = new Collection<ChecklistItem>();
        var checkboxesRoot = this.Database.GetItem(checklistItem["Root"]);
        if (checkboxesRoot != null)
        {
          foreach (Item item in checkboxesRoot.Children)
          {
            checkboxes.Add(new ChecklistItem { Text = item["Title"], Value = item.ID.ToString() });
          }
        }

        var checklist = new ChecklistDefinition { Header = checklistItem["Title"], Field = checklistItem["Field"], Checkboxes = checkboxes };
        checklists.Add(checklist);
      }

      return checklists;
    }

    /// <summary>
    /// Gets the catalog grid column definitions. Used to construct a grid in catalog view.
    /// </summary>
    /// <returns>The catalog grid column definitions.</returns>
    [NotNull]
    public virtual List<GridColumn> GetGridColumns()
    {
      var columns = new List<GridColumn>();

      if (this.CatalogItem == null)
      {
        return columns;
      }

      var row = this.CatalogItem.Children["Grid"];
      if (row == null || row.Children.Count <= 0)
      {
        return columns;
      }

      foreach (Item columnItem in row.Children)
      {
        columns.Add(this.dataMapper.GetEntity<GridColumn>(columnItem));
      }

      return columns;
    }

    /// <summary>
    /// Creates the catalog source.
    /// </summary>
    /// <returns>The catalog source.</returns>
    [CanBeNull]
    protected virtual CatalogBaseSource CreateCatalogSource()
    {
      return Reflection.ReflectionUtil.CreateObject(this.CatalogItem[CatalogDataSourceFieldName]) as CatalogBaseSource;
    }

    /// <summary>
    /// Gets the field value list.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns>The field value list</returns>
    [NotNull]
    private ListString GetFieldValueList([NotNull] string fieldName)
    {
      return this.CatalogItem == null ? new ListString() : new ListString(this.CatalogItem[fieldName]);
    }
  }
}