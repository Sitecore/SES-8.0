// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfLoggingProvider.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the EfLoggingProvider type.
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

namespace Sitecore.Ecommerce.Data.Logging
{
  using System.Collections.Concurrent;
  using System.Linq;
  using System.Threading;

  using Diagnostics;

  using Ecommerce.Logging;

  /// <summary>
  /// Entity Framework Logging provider.
  /// </summary>
  public class EfLoggingProvider : LoggingProvider
  {
    /// <summary>
    /// Order Management Log Entry model.
    /// </summary>
    private readonly LoggingModel logEntryModel;

    /// <summary>
    /// Stores logging entries that must be added.
    /// </summary>
    private readonly ConcurrentBag<LogEntry> loggingEntriesToAdd;

    /// <summary>
    /// Primitive that is used to synchronize operations.
    /// </summary>
    private readonly ReaderWriterLockSlim synchronizationPrimitive;

    /// <summary>
    /// Is set to true when instance is disposed.
    /// </summary>
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="EfLoggingProvider" /> class.
    /// </summary>
    /// <param name="shopContext">The shop context.</param>
    public EfLoggingProvider([NotNull] ShopContext shopContext)
    {
      Assert.ArgumentNotNull(shopContext, "shopContext");
      Assert.IsNotNullOrEmpty(shopContext.LoggingDatabaseName, "shopContext.LoggingDatabaseName");

      this.logEntryModel = new LoggingModel(shopContext.LoggingDatabaseName);
      this.loggingEntriesToAdd = new ConcurrentBag<LogEntry>();
      this.synchronizationPrimitive = new ReaderWriterLockSlim();
    }

    /// <summary>
    /// Writes data to log.
    /// </summary>
    /// <param name="logEntry">Order Management Log Entry.</param>
    public override void AppendEntry([NotNull] LogEntry logEntry)
    {
      Assert.ArgumentNotNull(logEntry, "logEntry");

      this.loggingEntriesToAdd.Add(logEntry);
    }

    /// <summary>
    /// Flushes this instance.
    /// </summary>
    public override void Flush()
    {
      var transactionId = this.GetTransactionId();
      this.synchronizationPrimitive.EnterWriteLock();

      foreach (var logEntry in this.loggingEntriesToAdd)
      {
        logEntry.TransactionID = transactionId;
        this.logEntryModel.LogEntries.Add(logEntry);
      }

      this.logEntryModel.SaveChanges();

      this.ClearLoggingEntries();

      this.synchronizationPrimitive.ExitWriteLock();
    }

    /// <summary>
    /// Reads the specified args.
    /// </summary>
    /// <returns>
    /// The collection of the 
    /// <see cref="LogEntry"> 
    /// log entries.</see>
    /// </returns>
    [NotNull]
    public override IQueryable<LogEntry> Read()
    {
      this.synchronizationPrimitive.EnterReadLock();
      var result = this.logEntryModel.LogEntries;
      this.synchronizationPrimitive.ExitReadLock();
      return result;
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
        if (this.logEntryModel != null)
        {
          this.logEntryModel.Dispose();
        }

        if (null != this.synchronizationPrimitive)
        {
          this.synchronizationPrimitive.Dispose();
        }
      }

      // Free any unmanaged objects here. 
      this.isDisposed = true;

      // Call base class implementation. 
      base.Dispose(disposing);
    }

    /// <summary>
    /// Gets the transaction ID.
    /// </summary>
    /// <returns>The transaction ID.</returns>
    protected virtual long GetTransactionId()
    {
      long result = 0;
      this.synchronizationPrimitive.EnterReadLock(); 
      if (this.logEntryModel.LogEntries.Any())
      {
        result = this.logEntryModel.LogEntries.Max(logEntry => logEntry.TransactionID) + 1;
      }

      this.synchronizationPrimitive.ExitReadLock();
      return result;
    }

    /// <summary>
    /// Clears the logging entries.
    /// </summary>
    protected virtual void ClearLoggingEntries()
    {
      while (!this.loggingEntriesToAdd.IsEmpty)
      {
        LogEntry someItem;
        this.loggingEntriesToAdd.TryTake(out someItem);
      }
    }
  }
}
