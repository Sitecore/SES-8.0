// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteIndependentDatabaseSwitcher.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SiteIndependentDatabaseSwitcher type.
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

namespace Sitecore.Ecommerce.Data
{
  using System;
  using System.Threading;
  using Sitecore.Data;

  /// <summary>
  /// Defines the site independent database switcher class.
  /// </summary>
  public class SiteIndependentDatabaseSwitcher : IDisposable
  {
    /// <summary>
    /// Stores reference to previous context database.
    /// </summary>
    private readonly Database previousContextDatabase;

    /// <summary>
    /// Is set to true when instance is disposed.
    /// </summary>
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="SiteIndependentDatabaseSwitcher"/> class.
    /// </summary>
    /// <param name="database">The database.</param>
    public SiteIndependentDatabaseSwitcher(Database database)
    {
      this.previousContextDatabase = Sitecore.Context.Data.Database;
      Sitecore.Context.Data.Database = database;
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
          Sitecore.Context.Data.Database = this.previousContextDatabase;
      }

      this.isDisposed = true;
    }
  }
}
