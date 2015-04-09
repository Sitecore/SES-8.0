// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditOrderLines.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the edit order and shopping cart lines command class.
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
  using Catalogs.Views;
  using Sitecore.Data.Items;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Web.UI.Sheer;

  /// <summary>
  /// Definition of the edit order and shopping cart lines command class
  /// </summary>
  public class EditOrderLines : Command
  {
    /// <summary>
    /// The Orders.OpenInNewWindow setting name
    /// </summary>
    private const string OrdersOpenInNewWindowSettingName = "Orders.OpenInNewWindow";

    /// <summary>
    /// Executes the command in the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public override void Execute(CommandContext context)
    {
      Item itemToEdit = this.GetEditItem(context);

      if (itemToEdit != null)
      {
        if (Configuration.Settings.GetBoolSetting(OrdersOpenInNewWindowSettingName, false))
        {
          Sitecore.Shell.Framework.Windows.RunApplication("Content Editor", string.Format("fo={0}", System.Web.HttpContext.Current.Server.UrlEncode(itemToEdit.ID.ToString())));
        }
        else
        {
          string result = string.Format("window.parent.scForm.browser.getControl('Ribbon_ContextualToolbar').style.display = 'none';window.parent.scForm.invoke(\"item:load(id={0})\");", itemToEdit.ID);
          SheerResponse.Eval(result);
        }
      }
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
  }
}
