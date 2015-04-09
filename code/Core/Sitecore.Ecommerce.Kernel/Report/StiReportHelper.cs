// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StiReportHelper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the StiReportHelper class.
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

namespace Sitecore.Ecommerce.Report
{
  using System.IO;
  using Stimulsoft.Report;

  /// <summary>
  /// Defines the StiReportHelper class.
  /// </summary>
  public class StiReportHelper
  {
    /// <summary>
    /// Checks if compiled report exists.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>
    /// Boolean value that shows if compiled report exists.
    /// </returns>
    public virtual bool CheckIfCompiledReportExists(string path)
    {
      return File.Exists(path);
    }

    /// <summary>
    /// Gets the localized compiled report.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>
    /// The compiled report.
    /// </returns>
    public virtual StiReport GetCompiledReport(string path)
    {
      return StiReport.GetReportFromAssembly(path, false);
    }

    /// <summary>
    /// Compiles the report.
    /// </summary>
    public virtual void CompileReport(StiReport report, string file)
    {
      report.Compile(file);
    }
  }
}