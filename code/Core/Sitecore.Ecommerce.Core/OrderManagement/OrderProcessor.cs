// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderProcessor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order processor class.
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
  using Diagnostics;
  using Orders;

  /// <summary>
  /// Defines the order processor class.
  /// </summary>
  public class OrderProcessor
  {
    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>
    /// The order.
    /// </value>
    [CanBeNull]
    public virtual Order Order { get; set; }

    /// <summary>
    /// Processes the order.
    /// </summary>
    /// <param name="processingStrategy">The processing strategy.</param>
    public virtual void Process([NotNull] ProcessingStrategy processingStrategy)
    {
      Assert.ArgumentNotNull(processingStrategy, "processingStrategy");

      Assert.IsNotNull(this.Order, "Unable to process the order. Order cannot be null.");

      processingStrategy.Process(this.Order);
    }
  }
}