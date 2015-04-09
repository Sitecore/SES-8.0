// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoreExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the CoreExtensions type.
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

namespace Sitecore.Ecommerce.OrderManagement.Extensions
{
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Order extensions.
  /// </summary>
  public static class CoreExtensions
  {
    /// <summary>
    /// Copies data partially from one LineItem to another.
    /// </summary>
    /// <param name="acceptor">The acceptor.</param>
    /// <param name="donor">The donor.</param>
    public static void CopyLineItemFrom([NotNull] this LineItem acceptor, [NotNull] LineItem donor)
    {
      Assert.ArgumentNotNull(acceptor, "acceptor");
      Assert.ArgumentNotNull(donor, "donor");

      acceptor.Quantity = donor.Quantity;
      acceptor.Price = donor.Price;
      acceptor.Item = donor.Item;
    }
  }
}
