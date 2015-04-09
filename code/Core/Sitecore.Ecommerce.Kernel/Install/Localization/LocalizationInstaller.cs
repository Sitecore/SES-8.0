// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizationInstaller.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the localization installer wrapper class.
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
  using System.Threading;
  using Jobs;
  using SecurityModel;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Proxies;

  /// <summary>
  /// Defines the localization installer class.
  /// </summary>
  public class LocalizationInstaller
  {
    /// <summary>
    /// LanguageManagerWrapper instance.
    /// </summary>
    private readonly LanguageManagerWrapper languageManagerWrapper;

    /// <summary>
    /// Job manager wrapper instance.
    /// </summary>
    private readonly JobManagerWrapper jobManagerWrapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizationInstaller"/> class.
    /// </summary>
    /// <param name="languageManagerWrapper">The language manager wrapper.</param>
    /// <param name="jobManagerWrapper">The job manager wrapper.</param>
    public LocalizationInstaller(LanguageManagerWrapper languageManagerWrapper, JobManagerWrapper jobManagerWrapper)
    {
      this.languageManagerWrapper = languageManagerWrapper;
      this.jobManagerWrapper = jobManagerWrapper;
    }

    /// <summary>
    /// Installs the specified database name.
    /// </summary>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="filePath">The file path.</param>
    public virtual void Install(string databaseName, string filePath)
    {
      using (new SecurityDisabler())
      {
        using (new ProxyDisabler())
        {
          using (new SyncOperationContext())
          {
            Job job = this.jobManagerWrapper.Start(databaseName, filePath, this.languageManagerWrapper.GetLanguages());
            if (job != null)
            {
              while (!job.IsDone)
              {
                Thread.Sleep(500);
              }
            }
          }
        }
      }
    }
  }
}