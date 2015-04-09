// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaxScheme.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the TaxScheme class.
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

namespace Sitecore.Ecommerce.OrderManagement.Orders
{
  public class TaxScheme
  {
    /// <summary>
    /// Identifier
    /// </summary>
    public virtual string ID { get; set; }

    /// <summary>
    /// Name of the tax area
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// Identification for the tax type
    /// </summary>
    public virtual string TaxTypeCode { get; set; }

    /// <summary>
    /// The currency the tax is in
    /// </summary>
    public virtual string CurrencyCode { get; set; }
  }
}
