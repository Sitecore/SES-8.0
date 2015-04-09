// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllowanceChargeFieldEditor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the AllowanceChargeFieldEditor type.
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
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the allowance charge field editor class.
  /// </summary>
  public class AllowanceChargeFieldEditor : BaseFieldEditor
  {
    /// <summary>
    /// UblEntityResolvingStrategy instance.
    /// </summary>
    private readonly UblEntityResolvingStrategy ublEntityResolvingStrategy = new OnSmartPanelAllowanceChargeStrategy();

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      AllowanceCharge allowanceCharge = this.DataItem as AllowanceCharge;
      Assert.IsNotNull(allowanceCharge, "AllowanceCharge cannot be null.");
      Order order = this.ublEntityResolvingStrategy.GetEntity(allowanceCharge.Alias) as Order;

      if (order != null)
      {
        bool needsDataBind = false;
        bool enabled = this.ublEntityResolvingStrategy.GetSecurityChecker()(order);

        foreach (Field field in this.fieldEditorLeft.Fields.Union(this.fieldEditorRight.Fields))
        {
          needsDataBind = needsDataBind || (field.ReadOnly ^ (!enabled));
          field.ReadOnly = !enabled;
        }

        this.fieldEditorLeft.DataBind();
        this.fieldEditorRight.DataBind();
      }
    }
  }
}
