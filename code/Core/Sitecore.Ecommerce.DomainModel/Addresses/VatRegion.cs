// -------------------------------------------------------------------------------------------
// <copyright file="VatRegion.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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

namespace Sitecore.Ecommerce.DomainModel.Addresses
{
  using System;

  /// <summary>
  /// The Vat Region base class .
  /// </summary>
  [Serializable]
  public class VatRegion
  {
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The VAT Region code.</value>
    public virtual string Code { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The VAT Region name.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The VAT Region title.</value>
    public virtual string Title { get; set; }
  }
}