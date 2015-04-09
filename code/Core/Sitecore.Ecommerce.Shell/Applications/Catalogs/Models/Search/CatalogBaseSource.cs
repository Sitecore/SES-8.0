// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatalogBaseSource.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   CatalogBaseSource class.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models.Search
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Reflection;
  using System.Web;
  using System.Web.UI.WebControls;
  using Data;
  using Diagnostics;
  using Ecommerce.Search;
  using Globalization;
  using Sitecore.Data;
  using Sitecore.Web.UI.Grids;

  /// <summary>
  /// CatalogBaseSource class.
  /// </summary>
  public abstract class CatalogBaseSource : IGridSource<List<string>>
  {
    /// <summary>
    /// Gets or sets the search options.
    /// </summary>
    /// <value>The search options.</value>
    public SearchOptions SearchOptions { get; set; }

    /// <summary>
    /// Gets or sets the database.
    /// </summary>
    /// <value>
    /// The database.
    /// </value>
    public Database Database { get; set; }

    /// <summary>
    /// Gets or sets the language.
    /// </summary>
    /// <value>
    /// The language.
    /// </value>
    public Language Language { get; set; }

    /// <summary>
    /// Filters the specified filters.
    /// </summary>
    /// <param name="filters">The filters.</param>
    public abstract void Filter(IList<GridFilter> filters);

    /// <summary>
    /// Gets the entries.
    /// </summary>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns>Returns entries.</returns>
    [NotNull]
    public virtual IEnumerable<List<string>> GetEntries(int pageIndex, int pageSize)
    {
      using (new LanguageSwitcher(this.Language))
      {
        return this.GetContentLanguageEntries(pageIndex, pageSize);
      }
    }

    /// <summary>
    /// Gets the entry count.
    /// </summary>
    /// <returns>
    /// Returns entry count.
    /// </returns>
    public abstract int GetEntryCount();

    /// <summary>
    /// Sorts the specified sorting.
    /// </summary>
    /// <param name="sorting">The sorting.</param>
    public virtual void Sort([NotNull] IList<SortCriteria> sorting)
    {
      Assert.ArgumentNotNull(sorting, "sorting");

      Type elementType;
      var source = this.GetSource(out elementType);
      if (source == null)
      {
        return;
      }

      foreach (var criterion in sorting)
      {
        // Decode field name
        string[] fieldName = { HttpUtility.UrlDecode(criterion.FieldName) };

        // Update field name if the value of the entity attribute is different from the property name
        foreach (var info in from info in elementType.GetProperties() let entityAttribute = Attribute.GetCustomAttribute(info, typeof(EntityAttribute), true) as EntityAttribute where entityAttribute != null && !string.IsNullOrEmpty(entityAttribute.FieldName) && string.Equals(entityAttribute.FieldName, fieldName[0]) select info)
        {
          fieldName[0] = info.Name;
        }

        // Sort the data
        PropertyInfo propertyInfo;
        if ((propertyInfo = elementType.GetProperty(fieldName[0])) != null)
        {
          // Check if the sorting can be done.
          if (propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IComparable)))
          {
            var parameter = Expression.Parameter(elementType, string.Empty);
            var property = Expression.Property(parameter, fieldName[0]);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = (criterion.Direction == SortDirection.Ascending) ? "OrderBy" : "OrderByDescending";
            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName, new[] { elementType, property.Type }, source.Expression, Expression.Quote(lambda));
            source = source.Provider.CreateQuery(methodCallExpression);
          }
        }

        this.UpdateSourceData(source);
      }
    }

    /// <summary>
    /// Gets data ro sort.
    /// </summary>
    /// <param name="elementType">The element type.</param>
    /// <returns>list of data.</returns>
    [CanBeNull]
    protected abstract IQueryable GetSource(out Type elementType);

    /// <summary>
    /// Updates the data.
    /// </summary>
    /// <param name="source">The source.</param>
    protected abstract void UpdateSourceData(IQueryable source);

    /// <summary>
    /// Casts the source.
    /// </summary>
    /// <param name="elementType">The element type.</param>
    /// <param name="tempSource">The temp source.</param>
    /// <returns>
    /// query able source
    /// </returns>
    [NotNull]
    protected virtual IQueryable CastSource([NotNull] Type elementType, [NotNull] IQueryable tempSource)
    {
      Assert.ArgumentNotNull(elementType, "elementType");
      Assert.ArgumentNotNull(tempSource, "tempSource");

      var methodInfo = typeof(Queryable).GetMethod("OfType", BindingFlags.Static | BindingFlags.Public);
      var genericMethodInfo = methodInfo.MakeGenericMethod(elementType);

      return (IQueryable)genericMethodInfo.Invoke(null, new object[] { tempSource });
    }

    /// <summary>
    /// Gets a query.
    /// </summary>
    /// <returns>A Query.</returns>
    [NotNull]
    protected virtual Query GetQuery()
    {
      var builder = new CatalogQueryBuilder();

      return builder.BuildQuery(this.SearchOptions);
    }

    /// <summary>
    /// Gets the entries in content language.
    /// </summary>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns>
    /// The entries in content language.
    /// </returns>
    [NotNull]
    protected virtual IEnumerable<List<string>> GetContentLanguageEntries(int pageIndex, int pageSize)
    {
      return new List<List<string>>();
    }
  }
}