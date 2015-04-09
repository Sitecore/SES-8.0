// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLineFieldEditor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLineFieldEditor type.
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
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using OrderManagement.ContextStrategies;
  using OrderManagement.Models;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the order line field editor class.
  /// </summary>
  public class OrderLineFieldEditor : BaseFieldEditor
  {
    /// <summary>
    /// The Quantity field name
    /// </summary>
    private const string FieldName = "Quantity";

    /// <summary>
    /// UblEntityResolvingStrategy instance.
    /// </summary>
    private readonly UblEntityResolvingStrategy ublEntityResolvingStrategy = new OnSmartPanelOrderLineStrategy();

    /// <summary>
    /// Allowance of the fields.
    /// </summary>
    private bool? allowance;

    /// <summary>
    /// Gets a value indicating whether this <see cref="OrderLineFieldEditor"/> is allowance.
    /// </summary>
    /// <value>
    ///   <c>true</c> if allowance; otherwise, <c>false</c>.
    /// </value>
    protected virtual bool Allowance
    {
      get
      {
        if (this.allowance == null)
        {
          OrderLineModel orderLineModel = this.DataItem as OrderLineModel;
          Assert.IsNotNull(orderLineModel, "OrderLineModel cannot be null.");
          OrderLine orderLine = this.ublEntityResolvingStrategy.GetEntity(orderLineModel.Alias) as OrderLine;
          if (orderLine != null)
          {
            this.allowance = this.ublEntityResolvingStrategy.GetSecurityChecker()(orderLine.Order);
          }
        }

        return this.allowance.GetValueOrDefault(true);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      bool needsDataBind = false;

      foreach (Field field in this.fieldEditorLeft.Fields.Union(this.fieldEditorRight.Fields).Where(f => f.Name == FieldName))
      {
        needsDataBind = needsDataBind || (field.ReadOnly ^ (!this.Allowance));
        field.ReadOnly = !this.Allowance;
      }

      if (needsDataBind)
      {
        this.fieldEditorLeft.DataBind();
        this.fieldEditorRight.DataBind();
      }
    }
  }
}