// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderReportModelExtended.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The custom order report model.
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

namespace Sitecore.Ecommerce.Examples.Reports
{
  using Report;

  /// <summary>
  /// The custom order report model.
  /// </summary>
  public class OrderReportModelExtended : OrderReportModel
  {
    /// <summary>
    /// Gets the buyer party supplier assigned account id.
    /// </summary>
    /// <value>The buyer party supplier assigned account id.</value>
    [NotNull]
    public virtual string FreightForwarderPartyIdentification
    {
      get
      {
        if ((this.Order != null) && (this.Order.DefaultFreightForwarderParty != null))
        {
          return this.Order.DefaultFreightForwarderParty.PartyIdentification;
        }

        return string.Empty;
      }
    } 
  }
}