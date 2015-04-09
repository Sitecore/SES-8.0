// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopContextDropdown.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ShopContextDropdown type.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls
{
  using System;
  using System.Linq;
  using System.Web.UI;
  using Diagnostics;
  using OrderManagement.DataSources;
  using Security;
  using Sitecore.Data;
  using Sitecore.Ecommerce.Apps.OrderManagement;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.WebControls.Extensions;
  using Speak.Web.UI.WebControls;
  using ContextSwitcher = Sitecore.Web.UI.WebControls.ContextSwitcher;

  /// <summary>
  /// The web store dropdown.
  /// </summary>
  public class ShopContextDropdown : ContextSwitcher
  {
    /// <summary>
    /// The single web store.
    /// </summary>
    private const int SingleWebStore = 1;

    /// <summary>
    /// The provider for the inner context items.
    /// </summary>
    private readonly ContextSwitcherDataSourceBase dataSource;

    /// <summary>
    /// The context items.
    /// </summary>
    private ContextItemCollection contextItems;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShopContextDropdown" /> class.
    /// </summary>
    public ShopContextDropdown()
    {
      this.dataSource = Ecommerce.Context.Entity.Resolve<ContextSwitcherDataSourceBase>();
    }

    /// <summary>
    /// Gets or sets the shop contexts root.
    /// </summary>
    /// <value>The shop contexts root.</value>
    public string ShopContextsRoot { get; set; }

    /// <summary>
    /// Gets the context items.
    /// </summary>
    /// <value>
    /// The context items.
    /// </value>
    private ContextItemCollection ContextItems
    {
      get
      {
        if (this.contextItems == null)
        {
          if (Sitecore.Context.Database != null && this.ShopContextsRoot != null)
          {
            this.dataSource.Source = new ItemUri(this.ShopContextsRoot, Sitecore.Context.Database).ToString();
          }

          this.contextItems = this.dataSource.Select();
        }

        Assert.IsNotNull(this.contextItems, "The context items must be specified");
        return this.contextItems;
      }
    }

    /// <summary>
    /// Binds the data.
    /// </summary>
    public override void DataBind()
    {
      var selectedShop = Sitecore.Context.User.Profile.GetSelectedShopContext();

      if (this.ContextItems.Count > SingleWebStore)
      {
        foreach (var contextItem in this.ContextItems)
        {
          contextItem.Selected = string.Compare(contextItem.Name, selectedShop, StringComparison.OrdinalIgnoreCase) == 0;

          this.Items.Add(contextItem);
        }
      }

      bool profileChanged = false;

      if (this.ContextItems.Any() && !this.ContextItems.Any(ci => ci.Selected))
      {
        this.ContextItems.First().Selected = true;
        Sitecore.Context.User.Profile.SetSelectedShopContext(this.ContextItems.First().Name);
        profileChanged = true;
      }

      if (this.ContextItems.Count == SingleWebStore)
      {
        Sitecore.Context.User.Profile.SetSelectedShopContext(this.ContextItems.First().Name);
        profileChanged = true;
      }

      if (!this.ContextItems.Any())
      {
        Sitecore.Context.User.Profile.SetSelectedShopContext(string.Empty);
        profileChanged = true;
      }

      if (profileChanged)
      {
        Sitecore.Context.User.Profile.Save();
      }

      base.DataBind();
    }

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    public void Refresh()
    {
      if (this.Page == null)
      {
        return;
      }

      foreach (Popup popup in this.Page.Controls.Flatten<Popup>().Where(p => p.Type == PopupType.SmartPanel))
      {
        popup.Close();
      }

      foreach (ObjectDetailList control in this.Page.Controls.Flatten<ObjectDetailList>())
      {
        control.DataBind();
        ScriptManager.GetCurrent(this.Page).UpdateControl(control);
      }
    }

    /// <summary>
    /// Loads the switcher.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected internal virtual void LoadSwitcher(object sender, EventArgs e)
    {
      var defaultShop = Sitecore.Context.User.Profile.GetDefaultShopContext();
      if (!this.ContextItems.Any() && string.IsNullOrEmpty(defaultShop))
      {
        this.Page.ShowStickyWarningMessage(Texts.ThereAreNoAvailableWebShops);
      }
    }

    /// <summary>
    /// Handles item clicked event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected internal virtual void ItemClicked(object sender, EventArgs args)
    {
      ItemClickedEventArgs<ContextItem> clickArgs = args as ItemClickedEventArgs<ContextItem>;
      if (clickArgs == null)
      {
        return;
      }

      if (clickArgs.Item == null)
      {
        return;
      }

      if (string.IsNullOrEmpty(clickArgs.Item.Name))
      {
        return;
      }

      Sitecore.Context.User.Profile.SetSelectedShopContext(clickArgs.Item.Name);
      Sitecore.Context.User.Profile.Save();

      this.Refresh();
    }

    /// <summary>
    /// Handles the ASP.NET lifecycle initialize event.
    /// </summary>
    /// <param name="e">The event argument.</param>
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      this.Click += this.ItemClicked;
      this.Load += this.LoadSwitcher;
    }
  }
}