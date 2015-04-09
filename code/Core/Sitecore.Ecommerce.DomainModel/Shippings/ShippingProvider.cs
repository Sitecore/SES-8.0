// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShippingProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ShippingProvider type.
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

namespace Sitecore.Ecommerce.DomainModel.Shippings
{
  using System;

  /// <summary>
  /// Defines the ShippingProvider type.
  /// </summary>
  [Serializable]
  public class ShippingProvider
  {
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The shipping method code.</value>
    public virtual string Code { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The shipping method name.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>The shipping method price.</value>
    public virtual decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The shipping method title.</value>
    public virtual string Title { get; set; }

    /// <summary>
    /// Gets or sets the short description.
    /// </summary>
    /// <value>The short description.</value>
    public virtual string Description { get; set; }

    /// <summary>
    /// Gets or sets the icon URL.
    /// </summary>
    /// <value>The icon URL.</value>
    public virtual string IconUrl { get; set; }

    /// <summary>
    /// Gets or sets the available notification options.
    /// </summary>
    /// <value>The available notification options.</value>
    public virtual string AvailableNotificationOptions { get; set; }
  }
}