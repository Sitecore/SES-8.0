// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntry.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the LogEntry type.
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

namespace Sitecore.Ecommerce.Logging
{
  using System;

  /// <summary>
  /// Log Entry.
  /// </summary>
  public class LogEntry
  {
    /// <summary>
    /// Empty logging entry.
    /// </summary>
    [UsedImplicitly]
    private static LogEntry emptyLogEntry;

    /// <summary>
    /// Gets empty logging entry.
    /// </summary>
    /// <returns>Empty logging entry.</returns>
    [NotNull]
    public static LogEntry Empty
    {
      get
      {
        return emptyLogEntry ?? (emptyLogEntry = new LogEntry());
      }
    }

    /// <summary>
    /// Gets or sets the log entry ID.
    /// </summary>
    /// <value>
    /// The log entry ID.
    /// </value>
    public virtual long LogEntryID { get; protected set; }

    /// <summary>
    /// Gets or sets the time stamp.
    /// </summary>
    /// <value>
    /// The time stamp.
    /// </value>
    public virtual DateTime TimeStamp { get; protected internal set; }

    /// <summary>
    /// Gets or sets the transaction ID.
    /// </summary>
    /// <value>
    /// The transaction ID.
    /// </value>
    public virtual long TransactionID { get; set; }

    /// <summary>
    /// Gets or sets the entity ID.
    /// </summary>
    /// <value>
    /// The entity ID.
    /// </value>
    public virtual string EntityID { get; set; }

    /// <summary>
    /// Gets or sets the type of the entity.
    /// </summary>
    /// <value>
    /// The type of the entity.
    /// </value>
    public virtual string EntityType { get; set; }

    /// <summary>
    /// Gets or sets the action.
    /// </summary>
    /// <value>
    /// The action.
    /// </value>
    public virtual string Action { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    public virtual string LevelCode { get; set; }

    /// <summary>
    /// Gets or sets the severity.
    /// </summary>
    /// <value>
    /// The severity.
    /// </value>
    public virtual Severity Severity { get; set; }

    /// <summary>
    /// Gets or sets the result.
    /// </summary>
    /// <value>
    /// The result.
    /// </value>
    public virtual string Result { get; set; }

    /// <summary>
    /// Gets or sets the details.
    /// </summary>
    /// <value>
    /// The details.
    /// </value>
    public virtual LogEntryDetails Details { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    /// <value>
    /// The user.
    /// </value>
    public virtual string User { get; set; }

    /// <summary>
    /// Gets or sets the shop context id.
    /// </summary>
    /// <value>
    /// The shop context id.
    /// </value>
    public virtual string ShopContextId { get; set; }
  }
}
