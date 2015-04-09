// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatalogView.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//  The base catalog view. The form contains base search functionality such as SearchQuery and Results Grid.
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
  using System.Diagnostics.CodeAnalysis;
  using System.IO;
  using System.Linq;
  using System.Web;
  using System.Web.UI;
  using System.Web.UI.HtmlControls;
  using ComponentArt.Web.UI;
  using Data;
  using Globalization;
  using Models;
  using Presenters;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Web;
  using Sitecore.Web.UI.Grids;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.Web.UI.WebControls.Ribbons;
  using Sitecore.Web.UI.XamlSharp.Ajax;
  using Sitecore.Web.UI.XamlSharp.Xaml;
  using Text;
  using Web.UI.WebControls;
  using Action = System.Action;
  using Checklist = Models.ChecklistDefinition;
  using Control = Sitecore.Web.UI.HtmlControls.Control;
  using GridColumn = ComponentArt.Web.UI.GridColumn;

  /// <summary>
  /// The base catalog form. The form contains base search functionality such as SearchQuery and Results Grid.
  /// </summary>
  public class CatalogView : XamlMainControl, ICatalogView, IHasCommandContext
  {
    /// <summary>
    /// The text box id prefix.
    /// </summary>
    private const string TextBoxIdPrefix = "tb_";

    /// <summary>
    /// The checklist id prefix.
    /// </summary>
    private const string CheckListIdPrefix = "cl_";

    /// <summary>
    /// The current item database.
    /// </summary>
    private readonly Database database = Database.GetDatabase(WebUtil.GetQueryString("database"));

    /// <summary>
    /// The container control for all the textboxes.
    /// </summary>
    protected Border TextBoxContainer;

    /// <summary>
    /// The container control for all the checklists.
    /// </summary>
    protected Border ChecklistContainer;

    /// <summary>
    /// The search button.
    /// </summary>
    protected HtmlButton SearchButton;

    /// <summary>
    /// The search result grid.
    /// </summary>
    protected Grid Grid;

    /// <summary>
    /// Checks if the second encoding is required.
    /// </summary>
    private bool doEncode = true;

    /// <summary>
    /// Occurs when search is started.
    /// </summary>
    public event Action Search;

    /// <summary>
    /// Occurs when text box creating.
    /// </summary>
    public event Action<TextBoxEventArgs> TextBoxCreating;

    /// <summary>
    /// Occurs when checklist creating.
    /// </summary>
    public event Action<ChecklistEventArgs> ChecklistCreating;

    /// <summary>
    /// Occurs when grid row clicked twice.
    /// </summary>
    public event Action<GridCommandEventArgs> GridRowDoubleClick;

    /// <summary>
    /// Occurs when [grid row selected].
    /// </summary>
    public event Action<GridCommandEventArgs> GridRowSelect;

    /// <summary>
    /// Gets or sets the catalog form presenter.
    /// </summary>
    public CatalogPresenter Presenter { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether the view is being rendered for the first time or is being loaded in response to a callback.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the page is being loaded in response to a client callback; otherwise, <c>false</c>.
    /// </value>
    public bool IsCallBack
    {
      get { return this.Page.IsPostBack || this.Grid.IsCallback; }
    }

    /// <summary>
    /// Gets the catalog item URI. The URI is used to find all the catalog settings.
    /// By default catalogs stored in '/sitecore/system/Modules/Ecommerce/Catalogs' folder.
    /// The current value is specified by an URL of the correspond CE Editor.
    /// </summary>
    /// <value>The catalog ID.</value>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public ItemUri CatalogUri
    {
      get { return new ItemUri(WebUtil.GetQueryString("catalogId"), this.database); }
    }

    /// <summary>
    /// Gets or sets the selected rows id.
    /// </summary>
    /// <value>The selected rows id.</value>
    public StringCollection SelectedRowsId
    {
      get
      {
        return this.ViewState["SelectedRowsID"] as StringCollection;
      }

      set
      {
        this.ViewState["SelectedRowsID"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the editor fields.
    /// </summary>
    /// <value>The editor fields.</value>
    public StringCollection EditorFields { get; set; }

    /// <summary>
    /// Gets the current item path.
    /// </summary>
    /// <value>The current item path.</value>
    public string CurrentItemPath
    {
      get
      {
        return WebUtil.GetQueryString("id");
      }
    }

    /// <summary>
    /// Gets or sets the ribbon source URI.
    /// </summary>
    /// <value>The ribbon source URI.</value>
    public virtual ItemUri RibbonSourceUri { get; set; }

    /// <summary>
    /// Gets the current item URI.
    /// </summary>
    /// <value>The current item URI.</value>
    [NotNull]
    public virtual ItemUri CurrentItemUri
    {
      get
      {
        var id = WebUtil.GetQueryString("id");
        var language = WebUtil.GetQueryString("language");

        return new ItemUri(id, Language.Parse(language), this.database);
      }
    }

    /// <summary>
    /// Gets or sets the search text boxes NameValueCollection.
    /// </summary>
    /// <value>The search query.</value>
    public virtual NameValueCollection TextBoxes { get; protected set; }

    /// <summary>
    /// Gets or sets the selected search checklists in a uri format.
    /// </summary>
    /// <value>The search checklists.</value>
    public virtual NameValueCollection Checklists { get; protected set; }

    /// <summary>
    /// Gets or sets the grid columns.
    /// </summary>
    /// <value>The grid columns.</value>
    protected IList<Models.GridColumn> GridColumns { get; set; }

    #region IHasCommandContext Members

    /// <summary>
    /// Gets the command context.
    /// </summary>
    /// <returns>The command context.</returns>
    public virtual CommandContext GetCommandContext()
    {
      var uri = ItemUri.ParseQueryString();
      var context = uri != null ? new CommandContext(Database.GetItem(uri)) : new CommandContext();

      context.CustomData = this;

      if (this.RibbonSourceUri == null)
      {
        return context;
      }

      context.RibbonSourceUri = this.RibbonSourceUri;
      context.Parameters["Ribbon.RenderTabs"] = "true";
      context.Parameters["Ribbon.RenderAsContextual"] = "true";
      context.Parameters["Ribbon.RenderContextualStripTitles"] = "true";

      if (!this.Page.ClientScript.IsClientScriptBlockRegistered("load"))
      {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "load", "scCatalog.showRibbon();", true);
      }

      return context;
    }

    /// <summary>
    /// Updates the ribbon.
    /// </summary>
    public virtual void UpdateRibbon()
    {
      if (this.RibbonSourceUri == null)
      {
        return;
      }

      var ribbon = new Ribbon();
      IHasCommandContext context = this;
      ribbon.CommandContext = context.GetCommandContext();
      ribbon.ID = "Ribbon";
      HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());
      ribbon.RenderControl(writer);
      AjaxScriptManager.SetOuterHtml("Ribbon", writer.InnerWriter.ToString());

      AjaxScriptManager.Eval("scCatalog.showRibbon()");
    }

    /// <summary>
    /// Refreshes the grid.
    /// </summary>
    public virtual void RefreshGrid()
    {
      AjaxScriptManager.Eval("scCatalog.refreshGrid()");
    }

    /// <summary>
    /// Fills the grid.
    /// </summary>
    /// <param name="source">The source.</param>
    public void FillGrid(IGridSource<List<string>> source)
    {
      if (this.doEncode)
      {
        foreach (GridColumn column in this.Grid.Levels[0].Columns)
        {
          column.DataField = HttpUtility.UrlEncode(column.DataField);
        }

        this.doEncode = false;
      }

      ComponentArtGridHandler<List<string>>.Manage(this.Grid, source, false);
    }

    #endregion

    /// <summary>
    /// Fills the grid.
    /// </summary>
    /// <param name="data">The grid data.</param>
    public void FillGrid(GridData data)
    {
      ComponentArtGridHandler<List<string>>.Manage(this.Grid, new GridSource<List<string>>(data.Rows), false);
    }

    /// <summary>
    /// Handles the grid row double click.
    /// </summary>
    /// <param name="rowId">The row ID.</param>
    public virtual void ShowDetails(string rowId)
    {
      var context = new CommandContext { CustomData = this };
      context.Parameters["id"] = rowId;
      CommandManager.GetCommand("ordercatalog:editorder").Execute(context);
    }

    /// <summary>
    /// Initializes the search text boxes.
    /// </summary>
    /// <param name="textBoxDefinitions">The list of text box definitions. Used to build the text box controls.</param>
    public void InitializeTextBoxes(IEnumerable<TextBoxDefinition> textBoxDefinitions)
    {
      this.TextBoxes = new NameValueCollection();

      foreach (var args in textBoxDefinitions.Select(tbd => new TextBoxEventArgs { TextBoxDefinition = tbd }))
      {
        this.OnTextBoxCreating(args);

        var textBox = new MaskedTextBox { ID = TextBoxIdPrefix + args.TextBoxDefinition.Field, CssClass = args.IsWide ? "scSearch Wide" : "scSearch", Text = args.Text, MaskedText = args.TextBoxDefinition.Title, MaskedCssStyle = "scSearchLabel" };
        textBox.Attributes["onkeydown"] = "javascript:if (event.keyCode == 13) { Grid.scHandler.refresh(); return false; }";

        this.TextBoxContainer.Controls.Add(textBox);

        if (!string.IsNullOrEmpty(args.Text))
        {
          this.TextBoxes.Add(args.TextBoxDefinition.Field, args.Text);
        }
      }
    }

    /// <summary>
    /// Initializes the checklists. Checklists provides additional search result filtering functionality.
    /// It's stored in the catalog settings folder.
    /// </summary>
    /// <param name="checklists">The checklists.</param>
    public void InitializeChecklists(IEnumerable<ChecklistDefinition> checklists)
    {
      this.Checklists = new NameValueCollection();

      foreach (var checklist in checklists)
      {
        var args = new ChecklistEventArgs { ChecklistDefinition = checklist };
        this.OnChecklistCreating(args);

        var border = new Border();
        border.Attributes["class"] = "scChecklist";
        this.ChecklistContainer.Controls.Add(border);

        var header = new Border { ID = Control.GetUniqueID("ChecklistHeader"), InnerHtml = checklist.Header };
        header.Attributes["class"] = "scChecklistHeader";
        border.Controls.Add(header);

        var checklistBorder = new Border { ID = Control.GetUniqueID("ChecklistBody") };
        checklistBorder.Attributes["name"] = CheckListIdPrefix + checklist.Field;
        checklistBorder.Attributes["class"] = "scChecklistItems";
        border.Controls.Add(checklistBorder);

        foreach (var checklistItem in checklist.Checkboxes)
        {
          var cb = new HtmlInputCheckBox { ID = string.Concat(Control.GetUniqueID("checkbox_"), CheckListIdPrefix, checklist.Field), Checked = args.CheckedValues != null && args.CheckedValues.Contains(checklistItem.Value), Value = checklistItem.Value };
          checklistBorder.Controls.Add(cb);

          var label = new LiteralControl(string.Format("<label for=\"{0}\">{1}</label><br />", cb.ClientID, checklistItem.Text));
          checklistBorder.Controls.Add(label);

          if (cb.Checked)
          {
            AddListStringValue(this.Checklists, checklist.Field, checklistItem.Value);
          }
        }
      }
    }

    /// <summary>
    /// Initializes the grid.
    /// </summary>
    /// <param name="columns">The columns list.</param>
    public virtual void InitializeGrid(IEnumerable<Models.GridColumn> columns)
    {
      this.InitializeGrid(columns, this.Grid);
    }

    /// <summary>
    /// Initializes the grid.
    /// </summary>
    /// <param name="columns">The columns.</param>
    /// <param name="grid">The grid.</param>
    protected virtual void InitializeGrid(IEnumerable<Models.GridColumn> columns, Grid grid)
    {
      if (grid.Levels == null || grid.Levels.Count == 0)
      {
        return;
      }

      grid.GroupingNotificationText = string.Empty;

      var level = grid.Levels[0];

      var gridColumns = columns as Models.GridColumn[] ?? columns.ToArray();
      this.GridColumns = new List<Models.GridColumn>(gridColumns);

      foreach (var gridColumn in gridColumns)
      {
        var col = new GridColumn();
        level.Columns.Add(col);
        col.Visible = !gridColumn.Hidden;

        col.AllowGrouping = InheritBool.False;

        col.DataField = gridColumn.FieldName;
        col.HeadingText = gridColumn.Header;
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      using (new SiteIndependentDatabaseSwitcher(this.database))
      {
        this.Grid.ClientEvents.ItemSelect = new ClientEvent("itemSelect");

        AjaxScriptManager.OnExecute += this.OnAjaxScriptManagerExecute;

        this.Grid.ItemDataBound += this.OnGridItemDataBound;
        this.Grid.NeedDataSource += this.OnGridNeedDataSource;

        this.InitializePresenter();

        this.SearchButton.InnerText = Translate.Text(Texts.Search);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      if (this.TextBoxes == null)
      {
        this.TextBoxes = this.ExtractFields(TextBoxIdPrefix);
      }

      if (this.Checklists == null)
      {
        this.Checklists = this.ExtractFields(CheckListIdPrefix);
      }

      this.EmulateSearchButtonClickEvent();
    }

    /// <summary>
    /// Handles the OnExecute event of the AjaxScriptManager control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The <see cref="Sitecore.Web.UI.XamlSharp.Ajax.AjaxCommandEventArgs"/> instance containing the event data.</param>
    protected virtual void OnAjaxScriptManagerExecute(object sender, AjaxCommandEventArgs args)
    {
      switch (args.Name)
      {
        case "catalog:gridrowdoubleclick":
          var gridArgs = new GridCommandEventArgs { RowID = args.Parameters["rowID"] };
          this.OnGridRowDoubleClick(gridArgs);
          break;
        case "catalog:gridrowselect":
          var gridArgs2 = new GridCommandEventArgs { RowsID = new ListString(args.Parameters["rowIDs"]).ToStringCollection() };
          this.OnGridRowSelect(gridArgs2);
          break;
      }
    }

    /// <summary>
    /// Initializes the presenter.
    /// </summary>
    protected virtual void InitializePresenter()
    {
      this.Presenter = new CatalogPresenter(this);
      this.Presenter.InitializeView();
    }

    /// <summary>
    /// Called when search started.
    /// </summary>
    protected virtual void OnSearch()
    {
      if (this.Search != null)
      {
        this.Search();
      }
    }

    /// <summary>
    /// Called before the textbox creating.
    /// </summary>
    /// <param name="args">The arguments.</param>
    protected virtual void OnTextBoxCreating(TextBoxEventArgs args)
    {
      if (this.TextBoxCreating != null)
      {
        this.TextBoxCreating(args);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:GridRowSelect"/> event.
    /// </summary>
    /// <param name="gridArgs">The <see cref="Sitecore.Ecommerce.Shell.Applications.Catalogs.Models.GridCommandEventArgs"/> instance containing the event data.</param>
    protected virtual void OnGridRowSelect(GridCommandEventArgs gridArgs)
    {
      if (this.GridRowSelect != null)
      {
        this.GridRowSelect(gridArgs);
      }
    }

    /// <summary>
    /// Called before the checklist creating.
    /// </summary>
    /// <param name="args">The arguments.</param>
    protected virtual void OnChecklistCreating(ChecklistEventArgs args)
    {
      if (this.ChecklistCreating != null)
      {
        this.ChecklistCreating(args);
      }
    }

    /// <summary>
    /// Called when the grid row clicked twice.
    /// </summary>
    /// <param name="args">The arguments.</param>
    protected virtual void OnGridRowDoubleClick(GridCommandEventArgs args)
    {
      if (this.GridRowDoubleClick != null)
      {
        this.GridRowDoubleClick(args);
      }
    }

    /// <summary>
    /// Handles the ItemDataBound event of the Grid control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="ComponentArt.Web.UI.GridItemDataBoundEventArgs"/> instance containing the event data.</param>
    protected virtual void OnGridItemDataBound(object sender, GridItemDataBoundEventArgs e)
    {
      var rowValues = e.DataItem as List<string>;
      if (rowValues == null)
      {
        return;
      }

      for (var i = 0; i < this.GridColumns.Count; i++)
      {
        e.Item[i] = rowValues[i];
      }
    }

    /// <summary>
    /// Handles the NeedDataSource event of the Grid control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnGridNeedDataSource(object sender, EventArgs e)
    {
      this.OnSearch();
    }

    /// <summary>
    /// Adds string value to the pipe-separated value field.
    /// By default NameValueCollection uses comma.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    private static void AddListStringValue(NameValueCollection list, string key, string value)
    {
      if (string.IsNullOrEmpty(list[key]))
      {
        list.Add(key, value);
      }
      else
      {
        var values = new ListString(list[key]) { value };
        list[key] = values.ToString();
      }
    }

    /// <summary>
    /// Search button performs ComponentArt grid call back so
    /// standard ASP.NET event handler cannot be used to handle the button click.
    /// The method emulates Search button click event handler.
    /// </summary>
    private void EmulateSearchButtonClickEvent()
    {
      if (WebUtil.GetQueryString("action") == "search")
      {
        this.OnSearch();
      }
    }

    /// <summary>
    /// Extracts the fields.
    /// </summary>
    /// <param name="prefix">The prefix.</param>
    /// <returns>The fields.</returns>
    private NameValueCollection ExtractFields(string prefix)
    {
      var fields = new NameValueCollection();
      var source = this.Grid.IsCallback ? this.Page.Request.QueryString : this.Page.Request.Form;

      foreach (string rawKey in source)
      {
        if (string.IsNullOrEmpty(rawKey))
        {
          continue;
        }

        var i = rawKey.LastIndexOf(prefix, StringComparison.Ordinal);
        if (i < 0)
        {
          continue;
        }

        if (string.IsNullOrEmpty(source[rawKey]))
        {
          continue;
        }

        var key = rawKey.Substring(i + prefix.Length);
        AddListStringValue(fields, key, source[rawKey]);
      }

      return fields;
    }
  }
}