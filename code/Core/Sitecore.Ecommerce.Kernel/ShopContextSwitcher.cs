// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopContextSwitcher.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the class which allows to switch the context shop programmatically.
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

namespace Sitecore.Ecommerce
{
  using System;
  using Microsoft.Practices.Unity;

  /// <summary>
  /// Defines the class which allows to switch the context shop programmatically.
  /// </summary>
  
  // INFO: intentionally not deriving from Switcher<T> since it will not provide any benefits.
  public class ShopContextSwitcher : IDisposable
  {
    /// <summary>
    /// The backup.
    /// </summary>
    private readonly ShopContext backup;

    /// <summary>
    /// Is set to true when instance is disposed.
    /// </summary>
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShopContextSwitcher"/> class.
    /// </summary>
    /// <param name="shopContext">The shop context.</param>
    public ShopContextSwitcher(ShopContext shopContext)
    {
      this.backup = Context.Entity.Resolve<ShopContext>();
      SetShopContext(shopContext);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (this.isDisposed)
      {
        return;
      }

      if (disposing)
      {
          SetShopContext(this.backup);
      }

      this.isDisposed = true;
    }

    /// <summary>
    /// Sets the shop context.
    /// </summary>
    /// <param name="shopContext">The shop context.</param>
    private static void SetShopContext(ShopContext shopContext)
    {
      Context.Entity.RegisterInstance(typeof(ShopContext), null, shopContext, new HierarchicalLifetimeManager());
    }
  }
}
