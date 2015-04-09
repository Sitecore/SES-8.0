// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionLogDataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ActionLogDataSourceRepository type.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.DataSources
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;
  using Diagnostics;
  using Logging;

  /// <summary>
  /// Defines the action log data source repository class.
  /// </summary>
  public class ActionLogDataSourceRepository : DataSourceRepository<LogEntry>
  {
    /// <summary>
    /// Stores reference to predefined filter.
    /// </summary>
    private PredefinedFilter predefinedFilter = new PredefinedFilter();

    /// <summary>
    /// Stores reference to expression parser.
    /// </summary>
    private SpeakExpressionParser expressionParser = new SpeakExpressionParser();

    /// <summary>
    /// The logger.
    /// </summary>
    private Logger logger;

    /// <summary>
    /// Gets or sets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    [NotNull]
    public Logger Logger
    {
      get
      {
        return this.logger ?? (this.logger = Context.Entity.Resolve<Logger>());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        this.logger = value;
      }
    }

    /// <summary>
    /// Gets or sets the predefined filter.
    /// </summary>
    /// <value>The predefined filter.</value>
    [NotNull]
    public PredefinedFilter PredefinedFilter 
    {
      get
      {
        return this.predefinedFilter;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.predefinedFilter = value;
      }
    }

    /// <summary>
    /// Gets or sets the expression parser.
    /// </summary>
    /// <value>The expression parser.</value>
    [NotNull]
    public SpeakExpressionParser ExpressionParser 
    {
      get
      {
        return this.expressionParser;
      }
      
      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.expressionParser = value;
      }
    }

    /// <summary>
    /// Selects the specified order id.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="expression">The expression.</param>
    /// <returns>The data table.</returns>
    public virtual DataTable Select([NotNull] string orderId, [CanBeNull] string expression)
    {
      Assert.ArgumentNotNull(orderId, "orderId");

      return this.ConvertToDataTable(this.SelectEntities(orderId, expression));
    }

    /// <summary>
    /// Selects the entities.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <returns>The entities.</returns>
    [NotNull]
    public override IEnumerable<LogEntry> SelectEntities([NotNull] string orderId)
    {
      Assert.ArgumentNotNull(orderId, "orderId");
      return this.Logger.GetEntries(orderId);
    }

    /// <summary>
    /// Selects the entities.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="expression">The expression.</param>
    /// <returns>The entities.</returns>
    [NotNull]
    public virtual IEnumerable<LogEntry> SelectEntities([NotNull] string orderId, [CanBeNull] string expression)
    {
      Assert.ArgumentNotNull(orderId, "orderId");

      IEnumerable<LogEntry> result = this.SelectEntities(orderId);
      
      if (!string.IsNullOrEmpty(expression))
      {
        string parsedExpression = this.ExpressionParser.Parse("o", null, expression);

        if (!string.IsNullOrEmpty(parsedExpression))
        {
          result = this.PredefinedFilter.ApplyFilter(result, parsedExpression);
        }
      }

      return result;
    }

    /// <summary>
    /// Gets the specified raw query.
    /// </summary>
    /// <param name="entryId">The raw query.</param>
    /// <returns>
    /// The logging entry.
    /// </returns>
    [CanBeNull]
    public virtual LogEntry Get([NotNull] string entryId)
    {
      Assert.ArgumentNotNull(entryId, "entryId");
      long logEntryId = long.Parse(entryId);

      return this.Logger.GetEntries().FirstOrDefault(le => le.LogEntryID == logEntryId);
    }

    /// <summary>
    /// Converts to data table.
    /// </summary>
    /// <param name="entries">The entries.</param>
    /// <returns>The to data table.</returns>
    protected override DataTable ConvertToDataTable([NotNull] IEnumerable<LogEntry> entries)
    {
      Assert.IsNotNull(entries, "entries");
      
      DataTable dataTable = new DataTable();

      dataTable.Columns.Add("TimeStamp", typeof(DateTime));
      dataTable.Columns.Add("TransactionID", typeof(long));
      dataTable.Columns.Add("EntityID", typeof(long));
      dataTable.Columns.Add("EntityType", typeof(string));
      dataTable.Columns.Add("Action", typeof(string));
      dataTable.Columns.Add("Severity", typeof(Severity));
      dataTable.Columns.Add("LevelCode", typeof(string));
      dataTable.Columns.Add("Result", typeof(string));
      dataTable.Columns.Add("Details.Message", typeof(string));
      dataTable.Columns.Add("User", typeof(string));
      dataTable.Columns.Add("LogEntryId", typeof(long));

      foreach (LogEntry entry in entries)
      {
        DataRow dataRow = dataTable.NewRow();

        dataRow["TimeStamp"] = entry.TimeStamp;
        dataRow["TransactionID"] = entry.TransactionID;
        dataRow["EntityID"] = entry.EntityID;
        dataRow["EntityType"] = entry.EntityType;
        dataRow["Action"] = entry.Action;
        dataRow["Severity"] = entry.Severity;
        dataRow["LevelCode"] = entry.LevelCode;
        dataRow["Result"] = entry.Result;
        if (entry.Details != null)
        {
          dataRow["Details.Message"] = entry.Details.Message;
        }

        dataRow["User"] = entry.User;
        dataRow["LogEntryId"] = entry.LogEntryID;

        dataTable.Rows.Add(dataRow);
      }

      return dataTable;
    }
  }
}