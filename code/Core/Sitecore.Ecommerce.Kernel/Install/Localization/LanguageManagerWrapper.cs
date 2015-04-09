// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageManagerWrapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the language manager wrapper class.
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
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Managers;

  /// <summary>
  /// Defines the language manager wrapper class.
  /// </summary>
  public class LanguageManagerWrapper
  {
    /// <summary>
    /// Database instance.
    /// </summary>
    private readonly Database database;

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageManagerWrapper"/> class.
    /// </summary>
    /// <param name="database">The database.</param>
    public LanguageManagerWrapper(Database database)
    {
      this.database = database;
    }

    /// <summary>
    /// Gets or sets the languages.
    /// </summary>
    /// <value>
    /// The languages.
    /// </value>
    public virtual List<string> Languages { get; set; }

    /// <summary>
    /// Gets the languages.
    /// </summary>
    /// <returns>
    /// The languages.
    /// </returns>
    public virtual List<string> GetLanguages()
    {
      if (this.Languages != null)
      {
        return this.Languages;
      }

      return LanguageManager.GetLanguages(this.database).Select(l => l.Name).ToList();
    }
  }
}