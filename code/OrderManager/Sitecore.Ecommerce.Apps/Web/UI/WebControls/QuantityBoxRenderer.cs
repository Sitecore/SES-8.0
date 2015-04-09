// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuantityBoxRenderer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the quantity box renderer class.
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
  using System.Web.UI.WebControls;
  using Globalization;
  using Speak.Web.UI.WebControls;

  /// <summary>
  /// Defines the quantity box renderer class.
  /// </summary>
  public class QuantityBoxRenderer : CompositeWebControl
  {
    /// <summary>
    /// Quantity label.
    /// </summary>
    private readonly Label quantityLabel;

    /// <summary>
    /// Quantity input text box.
    /// </summary>
    private readonly TextBox quantityInput;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuantityBoxRenderer"/> class.
    /// </summary>
    public QuantityBoxRenderer()
    {
      this.quantityInput = new TextBox
      {
        ID = "QuantityBoxRendererInput",
        Text = "1"
      };

      this.quantityLabel = new Label
      {
        ID = "QuantityBoxRendererLabel"
      };
    }

    /// <summary>
    /// Gets the text.
    /// </summary>
    public virtual string Text
    {
      get
      {
        return this.quantityInput.Text;
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      this.InitializeQuantityLabel();
      this.InitializeQuantityInput();
    }

    /// <summary>
    /// Initializes the quantity label.
    /// </summary>
    protected virtual void InitializeQuantityLabel()
    {
      this.quantityLabel.Text = Translate.Text(Texts.Quantity);
      this.quantityLabel.CssClass = "quantityLabel"; 
    }

    /// <summary>
    /// Initializes the quantity input.
    /// </summary>
    protected virtual void InitializeQuantityInput()
    {
      this.quantityLabel.CssClass = "quantityInput"; 
    }

    /// <summary>
    /// Creates the child controls.
    /// </summary>
    protected override void CreateChildControls()
    {
      base.CreateChildControls();
      this.Controls.Add(this.quantityLabel);
      this.Controls.Add(this.quantityInput);
    }
  }
}