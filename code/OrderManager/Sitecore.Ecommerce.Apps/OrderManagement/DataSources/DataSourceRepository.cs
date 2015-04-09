// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourceRepository.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the data source repository class which is used to
//   bind data for <see cref="Sitecore.Marketing.Client.Web.UI.Controls.SelectDataSource" /> control.
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
  using System.Collections.Generic;
  using System.Data;
  using Diagnostics;
  using Sitecore.Utils;

  /// <summary>
  /// Defines the data source repository class which is used to 
  /// bind data for <see cref="Sitecore.Marketing.Client.Web.UI.Controls.SelectDataSource"/> control.
  /// </summary>
  /// <typeparam name="T">The repository item.</typeparam>
  public abstract class DataSourceRepository<T>
  {
    /// <summary>
    /// Selects the specified raw query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>The data table.</returns>
    public virtual DataTable Select(string rawQuery)
    {
      return this.ConvertToDataTable(this.SelectEntities(rawQuery));
    }

    /// <summary>
    /// Selects the specified raw query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <returns>The result.</returns>
    public abstract IEnumerable<T> SelectEntities(string rawQuery);

    /// <summary>
    /// Converts to data table.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns>The to data table.</returns>
    protected virtual DataTable ConvertToDataTable([NotNull] IEnumerable<T> entities)
    {
      Assert.ArgumentNotNull(entities, "entities");
      return entities.ConvertToDataTable();
    }
  }
}