// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobManagerWrapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the jobManager wrapper class.
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

namespace Sitecore.Ecommerce.Install.Localization
{
  using System.Collections.Generic;
  using Jobs;
  using Shell.Applications.Globalization.ImportLanguage;

  /// <summary>
  /// Defines the job manager wrapper class.
  /// </summary>
  public class JobManagerWrapper
  {
    /// <summary>
    /// Starts the specified database name.
    /// </summary>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="filePath">The file path.</param>
    /// <param name="languages">The languages.</param>
    /// <returns>
    /// The job.
    /// </returns>
    public virtual Job Start(string databaseName, string filePath, List<string> languages)
    {
      JobOptions jobOptions = new JobOptions("ImportLanguage", "ImportLanguage", "shell", new ImportLanguageForm.Importer(databaseName, filePath, languages), "Import");
      return JobManager.Start(jobOptions);
    }
  }
}