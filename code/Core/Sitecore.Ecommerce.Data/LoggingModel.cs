// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingModel.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the LoggingModel type.
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
  using System.Data.Entity;
  using Ecommerce.Logging;

  /// <summary>
  /// Order Management Log Entry model.
  /// </summary>
  public class LoggingModel : DbContext
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingModel"/> class.
    /// </summary>
    /// <param name="nameOrConnectionString">The name or connection string.</param>
    public LoggingModel(string nameOrConnectionString)
      : base(nameOrConnectionString)
    {
      this.Configuration.LazyLoadingEnabled = true;
    }

    /// <summary>Gets or sets Orders.</summary>
    public virtual DbSet<LogEntry> LogEntries { get; set; }

    /// <summary>
    /// Is called when model is created.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<LogEntry>().ToTable("LogEntries");
      modelBuilder.Entity<LogEntry>().HasKey(k => k.LogEntryID);
      modelBuilder.ComplexType<LogEntryDetails>().Property(p => p.MessageKey).HasColumnName("Details");
      modelBuilder.ComplexType<LogEntryDetails>().Property(p => p.FormattedParameters).HasColumnName("Parameters");
      modelBuilder.ComplexType<LogEntryDetails>().Ignore(p => p.Message);
      modelBuilder.ComplexType<Severity>().Property(p => p.SeverityLevel).HasColumnName("Severity");
    }
  }
}