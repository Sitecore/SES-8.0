// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopContext.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Contains information about the context of the webshop. Extends Sitecore <see cref="SiteContext"/> class
//   with E-Commerce specific information such as webshop settings or E-Commerce database configuration.
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
  using Diagnostics;
  using DomainModel.Configurations;
  using Sitecore.Data;
  using Sites;

  /// <summary>
  /// Contains information about the context of the webshop. Extends Sitecore <see cref="SiteContext"/> class
  /// with E-Commerce specific information such as webshop settings or E-Commerce database configuration.
  /// </summary>
  public class ShopContext
  {
    /// <summary>
    /// The orders database key.
    /// </summary>
    public const string OrdersDatabaseKey = "ordersDatabase";

    /// <summary>
    /// The logging database key.
    /// </summary>
    public const string LoggingDatabaseKey = "actionLogDatabase";

    /// <summary>
    /// The orders database name.
    /// </summary>
    private readonly string ordersDatabaseName;

    /// <summary>
    /// The logging database name.
    /// </summary>
    private readonly string loggingDatabaseName;

    /// <summary>
    /// The inner site.
    /// </summary>
    private readonly SiteContext innerSite;

    /// <summary>
    ///  The inner site database.
    /// </summary>
    private Database database;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShopContext"/> class.
    /// </summary>
    /// <param name="innerSite">The inner site.</param>
    public ShopContext([NotNull] SiteContext innerSite)
    {
      Assert.ArgumentNotNull(innerSite, "innerSite");

      this.innerSite = innerSite;
      this.database = innerSite.Database;
      this.ordersDatabaseName = innerSite.Properties[OrdersDatabaseKey];
      this.loggingDatabaseName = innerSite.Properties[LoggingDatabaseKey];
    }

    /// <summary>
    /// Gets the inner site context.
    /// </summary>
    [NotNull]
    public SiteContext InnerSite
    {
      get { return this.innerSite; }
    }

    /// <summary>
    /// Gets or sets the database.
    /// </summary>
    /// <value>The database.</value>
    [CanBeNull]
    public virtual Database Database
    {
      get { return this.database; }
      set { this.database = value; }
    }

    /// <summary>
    /// Gets or sets the Business Catalog settings.
    /// </summary>
    /// <value>The Business Catalog settings.
    /// </value>
    [CanBeNull]
    public virtual BusinessCatalogSettings BusinessCatalogSettings { get; set; }

    /// <summary>
    /// Gets or sets the General settings.
    /// </summary>
    /// <value>The General settings.</value>
    [NotNull]
    public virtual GeneralSettings GeneralSettings { get; set; }

    /// <summary>
    /// Gets the name of the orders database.
    /// The value is taken from the "ordersDatabase" custom property of the <see cref="SiteContext"/>.
    /// </summary>
    /// <value>The name of the orders database.</value>
    [NotNull]
    public virtual string OrdersDatabaseName
    {
      get { return this.ordersDatabaseName; }
    }

    /// <summary>
    /// Gets the name of the logging database.
    /// The value is taken from the "actionLogDatabase" custom property of the <see cref="SiteContext"/>.
    /// </summary>
    /// <value>The name of the orders database.</value>
    [NotNull]
    public virtual string LoggingDatabaseName
    {
      get { return this.loggingDatabaseName; }
    }
  }
}