// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryResult.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The query result.
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

namespace Sitecore.Ecommerce.StructuredData
{
  using Lucene.Net.QueryParsers;

  /// <summary>
  /// The query result.
  /// </summary>
  public class QueryResult
  {
    /// <summary>
    /// The error message.
    /// </summary>
    public string ErrorMessage;

    /// <summary>
    /// The parse exception.
    /// </summary>
    public ParseException ParseException;

    /// <summary>
    /// The result.
    /// </summary>
    public string[] Result;

    /// <summary>
    /// The success.
    /// </summary>
    public bool Success;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryResult"/> class.
    /// </summary>
    public QueryResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryResult"/> class.
    /// </summary>
    /// <param name="success">if set to <c>true</c> success.</param>
    /// <param name="result">The result.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="parseException">The parse exception.</param>
    public QueryResult(bool success, string[] result, string errorMessage, ParseException parseException)
    {
      this.Success = success;
      this.Result = result;
      this.ErrorMessage = errorMessage;
      this.ParseException = parseException;
    }
  }
}