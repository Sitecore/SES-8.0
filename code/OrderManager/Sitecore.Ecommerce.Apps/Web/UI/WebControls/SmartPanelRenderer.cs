// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmartPanelRenderer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SmartPanelRenderer type.
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
  using Diagnostics;
  using Links;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Web.UI.WebControls;
  using Speak.Web.UI.WebControls;

  /// <summary>
  /// The smart panel renderer.
  /// </summary>
  public class SmartPanelRenderer : CompositeWebControl
  {
    /// <summary>
    /// Smart panel id.
    /// </summary>
    private const string SmartPanelId = "{F49C3E24-AB53-4600-9A27-6E5A0F823BF2}";

    /// <summary>
    /// The smart panel.
    /// </summary>
    private readonly Popup smartPanel;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmartPanelRenderer" /> class.
    /// </summary>
    public SmartPanelRenderer()
    {
      this.smartPanel = new Popup();
    }

    /// <summary>
    /// Gets the smart panel.
    /// </summary>
    /// <value>
    /// The smart panel.
    /// </value>
    public Popup SmartPanel
    {
      get
      {
        return this.smartPanel;
      }
    }

    /// <summary>
    /// Raises the <see>
    /// <cref>E:Init</cref>
    /// </see>
    /// event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
      base.OnInit(e);

      this.InitializeSmartPanel();
    }

    /// <summary>
    /// Creates the child controls.
    /// </summary>
    protected override void CreateChildControls()
    {
      base.CreateChildControls();
      this.Controls.Add(this.smartPanel);
    }

    /// <summary>
    /// Initializes the smart panel.
    /// </summary>
    protected virtual void InitializeSmartPanel()
    {
      this.smartPanel.ID = "smartPanel_" + new ShortID(SmartPanelId);
      this.smartPanel.Type = PopupType.SmartPanel;
      this.smartPanel.Url = this.GetSmartPanelUrl();
    }

    /// <summary>
    /// Gets the URL.
    /// </summary>
    /// <returns>
    /// The URL.
    /// </returns>
    private string GetSmartPanelUrl()
    {
      Item smartPanelItem = Sitecore.Context.Database.GetItem(SmartPanelId);
      return LinkManager.GetItemUrl(smartPanelItem, UrlOptions.DefaultOptions);
    }
  }
}