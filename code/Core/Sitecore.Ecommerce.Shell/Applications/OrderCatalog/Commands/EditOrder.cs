// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditOrder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the edit order commend class.
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

namespace Sitecore.Ecommerce.Shell.Applications.OrderCatalog.Commands
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;
  using System.Web;
  using System.Web.UI;
  using Catalogs.Views;
  using Diagnostics;
  using Globalization;
  using SecurityModel;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Shell.Applications.WebEdit;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Web.UI.Sheer;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.XamlSharp.Continuations;
  using Text;

  /// <summary>
  /// Edit order command.
  /// </summary>
  public class EditOrder : Command, ISupportsContinuation
  {
    /// <summary>
    /// The setting name.
    /// </summary>
    private const string SettingName = "Catalog.OpenInNewWindow";

    /// <summary>
    /// Executes the command in the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public override void Execute(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      string id = context.Parameters["id"];
      if (!ID.IsID(id))
      {
        return;
      }

      Item orderItem = Sitecore.Context.ContentDatabase.GetItem(new ID(id));
      if (orderItem == null)
      {
        return;
      }

      ICatalogView view = context.CustomData as ICatalogView;
      if (view == null)
      {
        return;
      }

      ClientPipelineArgs args = new ClientPipelineArgs(new NameValueCollection(context.Parameters));
      args.Parameters.Add("uri", orderItem.Uri.ToString());
      args.Parameters["fields"] = this.GetFields(view.EditorFields);
      args.CustomData.Add("catalogView", view);

      ContinuationManager.Current.Start(this, "Run", args);
    }

    /// <summary>
    /// Queries the state of the command.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The state of the command.</returns>
    public override CommandState QueryState(CommandContext context)
    {
      Item item = this.GetEditItem(context);

      if (item == null)
      {
        return CommandState.Hidden;
      }

      if (item.Access.CanWrite() && (item.Locking.HasLock() || item.State.CanEdit()))
      {
        return CommandState.Enabled;
      }

      return CommandState.Disabled;
    }

    /// <summary>
    /// Gets the click event.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="click">The default click event.</param>
    /// <returns>The click event.</returns>
    public override string GetClick(CommandContext context, string click)
    {
      Item item = this.GetEditItem(context);
      if (item != null)
      {
        click = string.Format("{0}(id={1})", click, item.ID);
      }

      return base.GetClick(context, click);
    }

    /// <summary>
    /// Sheer UI processor methods that orchestrates starting the Field Editor and processing the returned value
    /// </summary>
    /// <param name="args">The arguments.</param>
    public void OpenFieldEditor(ClientPipelineArgs args)
    {
      HttpContext current = HttpContext.Current;
      if (current != null)
      {
        Page handler = current.Handler as Page;
        if (handler != null)
        {
          NameValueCollection form = handler.Request.Form;
          if (form != null)
          {
            if (!args.IsPostBack)
            {
              SheerResponse.ShowModalDialog(this.GetOptions(args, form).ToUrlString().ToString(), "720", "480", string.Empty, true);
              args.WaitForPostBack();
            }
          }
        }
      }
    }

    /// <summary>
    /// Opens the content editor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public void OpenContentEditor(ClientPipelineArgs args)
    {
      Item item = this.GetItemFromPipelineArgs(args);
      var parameters = string.Format("fo={0}", HttpContext.Current.Server.UrlEncode(item.ID.ToString()));
      Sitecore.Shell.Framework.Windows.RunApplication("Content Editor", parameters);
    }

    /// <summary>
    /// Gets the fields.
    /// </summary>
    /// <param name="fields">The fields.</param>
    /// <returns>Returns a collection of field for edit.</returns>
    protected virtual string GetFields(StringCollection fields)
    {
      ListString ls = new ListString();
      foreach (string fieldId in fields)
      {
        ls.Add(fieldId);
      }

      return ls.ToString();
    }

    /// <summary>
    /// Runs the specified args.
    /// </summary>
    /// <param name="args">The pipeline args.</param>
    protected void Run(ClientPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (!args.IsPostBack)
      {
        if (Configuration.Settings.GetBoolSetting(SettingName, true))
        {
          this.OpenFieldEditor(args);
        }
        else
        {
          this.OpenContentEditor(args);
        }
      }
      else
      {
        if (args.HasResult)
        {
          this.SaveFieldEditorValues(args);
          ICatalogView view = args.CustomData["catalogView"] as ICatalogView;
          if (view != null)
          {
            view.RefreshGrid();
          }
        }
      }
    }

    /// <summary>
    /// The save field editor values.
    /// </summary>
    /// <param name="args">
    /// The pipeline args.
    /// </param>
    protected void SaveFieldEditorValues(ClientPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      PageEditFieldEditorOptions options = PageEditFieldEditorOptions.Parse(args.Result);
      this.SaveFieldValues(options);
    }

    /// <summary>
    /// Saves the page editor field values.
    /// </summary>
    /// <param name="options">The options.</param>
    protected virtual void SaveFieldValues(PageEditFieldEditorOptions options)
    {
      Item item = Database.GetItem(options.Fields.First().ItemUri);
      if (item == null)
      {
        AjaxScriptManager.Current.ShowError(Translate.Text(Texts.TheOrderCannotBeSaved), Translate.Text(Texts.ThisOrderDoesNotExist));
        return;
      }

      using (new EditContext(item, SecurityCheck.Enable))
      {
        foreach (FieldDescriptor descr in options.Fields)
        {
          item[descr.FieldID] = descr.Value;
        }
      }
    }

    /// <summary>
    /// Gets the order item.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>Returns the order item.</returns>
    protected virtual Item GetEditItem(CommandContext context)
    {
      ICatalogView catalogView = context.CustomData as ICatalogView;

      if (catalogView == null || catalogView.SelectedRowsId == null || catalogView.SelectedRowsId.Count > 1)
      {
        return null;
      }

      return Sitecore.Context.ContentDatabase.SelectSingleItem(catalogView.SelectedRowsId[0]);
    }

    /// <summary>
    /// Gets the options.
    /// </summary>
    /// <param name="args">The pipeline args.</param>
    /// <param name="form">The page form.</param>
    /// <returns>Returns edit field options for</returns>
    protected PageEditFieldEditorOptions GetOptions(ClientPipelineArgs args, NameValueCollection form)
    {
      List<FieldDescriptor> fieldDescriptors = new List<FieldDescriptor>();
      var item = this.GetItemFromPipelineArgs(args);
      string fields = args.Parameters["fields"];
      Assert.IsNotNullOrEmpty(fields, "Field Editor command expects 'fields' parameter");
      foreach (string field in new ListString(fields))
      {
        if (item.Fields[field] != null)
        {
          fieldDescriptors.Add(new FieldDescriptor(item, field));
        }
      }

      return new PageEditFieldEditorOptions(form, fieldDescriptors) { Title = item["Title"], Icon = item["Icon"] };
    }

    /// <summary>
    /// Gets the item from parameters.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The item.</returns>
    private Item GetItemFromPipelineArgs(ClientPipelineArgs args)
    {
      var item = Database.GetItem(ItemUri.Parse(args.Parameters["uri"]));
      Assert.IsNotNull(item, "item");
      return item;
    }
  }
}