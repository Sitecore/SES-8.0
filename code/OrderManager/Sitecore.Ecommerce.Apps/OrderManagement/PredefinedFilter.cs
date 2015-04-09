// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredefinedFilter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the PredefinedFilter type.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using CodeDom.Scripts;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Defines the predefined filter class.
  /// </summary>
  public class PredefinedFilter
  {
    /// <summary>
    /// Represents code for filter.
    /// </summary>
    private const string CodeTemplate = @"
      namespace Sitecore.Ecommerce.Apps.OrderManagement.OrderManager.Utils.DynamicallyGenerated
      {{
        using System;
        using System.Collections.Generic;
        using System.Linq;
          
        public static class PredefinedFilter
        {{
          public static {0}<{1}> ApplyFilter({0}<{1}> source)
          {{
            return source.Where({2});
          }}
        }}
      }}";

    /// <summary>
    /// Applies the filter.
    /// </summary>
    /// <typeparam name="T">Represents sequence element type.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="filterExpression">The filter expression.</param>
    /// <returns>The filtered source.</returns>
    [NotNull]
    public virtual IEnumerable<T> ApplyFilter<T>([NotNull] IEnumerable<T> source, [NotNull] string filterExpression)
    {
      Assert.ArgumentNotNull(source, "source");
      Assert.ArgumentNotNull(filterExpression, "filterExpression");

      SpeakDateTimeExtractor extractor = new SpeakDateTimeExtractor();
      filterExpression = extractor.Extract(filterExpression);

      SpeakFreeTextSearchExtractor<T> freeTextSearchExtractor = new SpeakFreeTextSearchExtractor<T>();
      filterExpression = freeTextSearchExtractor.Extract(filterExpression);

      SpeakExpressionLocalizer localizer = new SpeakExpressionLocalizer();
      filterExpression = localizer.Update(filterExpression);

      IEnumerable<string> references = new[] { Assembly.GetAssembly(typeof(Order)).Location, Assembly.GetAssembly(typeof(Queryable)).Location };
      string code = string.Format(CodeTemplate, typeof(IQueryable<T>).IsAssignableFrom(source.GetType()) ? "System.Linq.IQueryable" : "System.Collections.Generic.IEnumerable", typeof(T).FullName, filterExpression);

      Script script = ScriptFactory.Compile(filterExpression, references, code, "C#");
      Assert.IsTrue(script.IsValid, "Unable to apply the filter. Expression compilation failed.");

      return script.InvokeDefaultMethod<IEnumerable<T>>(source);
    }
  }
}