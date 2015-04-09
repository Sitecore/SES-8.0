// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderModelInitializer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderModelInitializer type.
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

namespace Sitecore.Ecommerce.Data.ModelConfiguration
{
  using System.Data.Entity;
  using Diagnostics;

  /// <summary>
  /// Initializes "Orders" database.
  /// </summary>
  public class OrderModelInitializer : CreateDatabaseIfNotExists<OrderModel>
  {
    /// <summary>
    /// Sql command to add unique constraint to the "Orders" database.
    /// </summary>
    private const string SqlCommand = "ALTER TABLE [Order] ADD CONSTRAINT OrderId_ShopContext UNIQUE (OrderId, ShopContext)";

    /// <summary>
    /// Seeds the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Seed([NotNull] OrderModel context)
    {
      Assert.ArgumentNotNull(context, "context");

      context.Database.ExecuteSqlCommand(SqlCommand);

      base.Seed(context);
    }
  }
}