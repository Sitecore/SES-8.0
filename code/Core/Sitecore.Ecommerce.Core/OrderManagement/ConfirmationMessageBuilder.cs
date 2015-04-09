// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfirmationMessageBuilder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the confirmation text builder class.
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

namespace Sitecore.Ecommerce.OrderManagement
{
  using Mail;
  using Orders;

  /// <summary>
  /// Defines the confirmation text builder class.
  /// </summary>
  public abstract class ConfirmationMessageBuilder : MessageBuilder
  {
    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>
    /// The order.
    /// </value>
    public virtual Order Order { get; set; }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    /// <returns>
    /// The parameters.
    /// </returns>
    [NotNull]
    protected override object GetParameters()
    {
      return this.Order;
    }
  }
}