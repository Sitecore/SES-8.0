// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Logger type.
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
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using SecurityModel;
  using Sitecore.Data;

  /// <summary>
  /// Defines the Logger type.
  /// </summary>
  public class Logger
  {
    /// <summary>
    /// Provider not set constant.
    /// </summary>
    private const string ProviderNotSet = "Unable to log the action. LoggingProvider is not set.";

    /// <summary>
    /// Security setting root item ID.
    /// </summary>
    private const string SecuritySettingId = "{E77FD238-D553-436D-A397-CBBD2F8AFC88}";

    /// <summary>
    /// Security level template's id.
    /// </summary>
    private const string SecurityLevelTemplateId = "{311F8DEC-FBD9-4E54-994C-BC1A1FF61425}";

    /// <summary>
    /// Level items.
    /// </summary>
    private IEnumerable<string> availableLevelCodes;

    /// <summary>
    /// Gets or sets the database.
    /// </summary>
    /// <value>
    /// The database.
    /// </value>
    public string DefaultShopContextId { get; set; }

    /// <summary>
    /// Gets or sets the database.
    /// </summary>
    /// <value>
    /// The database.
    /// </value>
    [NotNull]
    public IEnumerable<string> AvailableLevelCodes
    {
      get
      {
        if (this.availableLevelCodes == null)
        {
          using (new SecurityDisabler())
          {
            string query = string.Format("fast://*[@@id='{0}']//*[@@templateid='{1}']", SecuritySettingId, SecurityLevelTemplateId);
            Database database = Configuration.Factory.GetDatabase("master");
            this.availableLevelCodes = database.SelectItems(query).Where(itm => itm.Access.CanRead()).Select(levelItem => levelItem["Level Code"]);
          }
        }

        return this.availableLevelCodes;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.availableLevelCodes = value;
      }
    }

    /// <summary>
    /// Gets or sets the provider.
    /// </summary>
    /// <value>
    /// The provider.
    /// </value>
    [CanBeNull]
    public LoggingProvider Provider { get; set; }

    /// <summary>
    /// Writes the specified action.
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="entityId">The entity id.</param>
    /// <param name="action">The action.</param>
    /// <param name="levelCode">The level code.</param>
    /// <param name="result">The result.</param>
    public virtual void Write([NotNull] string entityType, string entityId, [NotNull] string action, [NotNull] string levelCode, [NotNull] string result)
    {
      Assert.ArgumentNotNull(entityType, "entityType");
      Assert.ArgumentNotNull(action, "action");
      Assert.ArgumentNotNull(levelCode, "levelCode");
      Assert.ArgumentNotNull(result, "result");

      Assert.IsNotNull(this.Provider, ProviderNotSet);

      LogEntry logEntry = new LogEntry
      {
        EntityType = entityType,
        EntityID = entityId,
        Action = action,
        LevelCode = levelCode,
        Result = result,
        Details = new LogEntryDetails(action),
      };

      this.Write(logEntry);
    }

    /// <summary>
    /// Writes the specified logging entry.
    /// </summary>
    /// <param name="logEntry">The logging entry.</param>
    public virtual void Write([NotNull] LogEntry logEntry)
    {
      Assert.ArgumentNotNull(logEntry, "logEntry");

      this.Log(logEntry);
      this.Flush();
    }

    /// <summary>
    /// Logs the specified logging entry.
    /// </summary>
    /// <param name="logEntry">The logging entry.</param>
    public virtual void Log([NotNull] LogEntry logEntry)
    {
      Assert.ArgumentNotNull(logEntry, "logEntry");

      Assert.IsNotNull(this.Provider, ProviderNotSet);

      if (this.AvailableLevelCodes.Contains(logEntry.LevelCode))
      {
        this.Provider.AppendEntry(this.ApplyDefaults(logEntry));
      }
    }

    /// <summary>
    /// Flushes this instance.
    /// </summary>
    public virtual void Flush()
    {
      Assert.IsNotNull(this.Provider, ProviderNotSet);

      this.Provider.Flush();
    }

    /// <summary>
    /// Gets the entries.
    /// </summary>
    /// <returns>Collection of the log entries.</returns>
    [NotNull]
    public virtual IQueryable<LogEntry> GetEntries()
    {
      Assert.IsNotNull(this.Provider, ProviderNotSet);

      return this.Provider.Read().Where(e => this.AvailableLevelCodes.Contains(e.LevelCode)).OrderByDescending(le => le.TimeStamp);
    }

    /// <summary>
    /// Gets the entries.
    /// </summary>
    /// <param name="entityId">The entity id.</param>
    /// <returns>
    /// The entries.
    /// </returns>
    [NotNull]
    public virtual IQueryable<LogEntry> GetEntries(string entityId)
    {
      return this.GetEntries().Where(le => le.EntityID == entityId);
    }

    /// <summary>
    /// Applies the stamps.
    /// </summary>
    /// <param name="logEntry">The logging entry.</param>
    /// <returns>
    /// Logging entry with the applied stamps.
    /// </returns>
    [NotNull]
    private LogEntry ApplyDefaults([NotNull] LogEntry logEntry)
    {
      Assert.ArgumentNotNull(logEntry, "logEntry");

      if (logEntry.TimeStamp == DateTime.MinValue)
      {
        logEntry.TimeStamp = DateTime.Now;
      }

      if (logEntry.Severity == null)
      {
        logEntry.Severity = SeverityLevels.Information;
      }

      if (string.IsNullOrEmpty(logEntry.User))
      {
        logEntry.User = Context.User.Name;
      }

      if (string.IsNullOrEmpty(logEntry.ShopContextId))
      {
        logEntry.ShopContextId = this.DefaultShopContextId;
      }

      return logEntry;
    }
  }
}