// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyLoggingProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the EmptyLoggingProvider type.
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

namespace Sitecore.Ecommerce.Merchant.OrderManagement
{
  using System.Linq;
  using Logging;

  /// <summary>
  /// The empty logging provider.
  /// </summary>
  public class EmptyLoggingProvider : LoggingProvider
  {

    /// <summary>
    /// Is set to true when instance is disposed.
    /// </summary>
    private bool isDisposed;

    /// <summary>
    /// Writes data to log.
    /// </summary>
    /// <param name="logEntry">Order Management Log Entry.</param>
    public override void AppendEntry(LogEntry logEntry)
    {
    }

    /// <summary>
    /// Flushes this instance.
    /// </summary>
    public override void Flush()
    {
    }

    /// <summary>
    /// Reads the specified args.
    /// </summary>
    /// <returns>
    /// The collection of the
    /// <see cref="LogEntry">
    /// log entries.</see>
    /// </returns>
    public override IQueryable<LogEntry> Read()
    {
      return Enumerable.Empty<LogEntry>().AsQueryable();
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (this.isDisposed)
      {
        return;
      }

      if (disposing)
      {
        // Free any other managed objects here. 
      }

      // Free any unmanaged objects here. 
      this.isDisposed = true;
      
      // Call base class implementation. 
      base.Dispose(disposing);
    }
  }
}