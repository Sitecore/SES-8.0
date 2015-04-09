// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizationPostStep.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the LocalizationPostStep class.
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
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.IO;
  using System.Linq;
  using Configuration;
  using Diagnostics;
  using Sitecore.Install.Framework;
  using Text;

  /// <summary>
  /// Defines the localization post step class.
  /// </summary>
  public class LocalizationPostStep : IPostStep
  {
    /// <summary>
    /// Core database name.
    /// </summary>
    private const string CoreDatabaseName = "core";

    /// <summary>
    /// Database names.
    /// </summary>
    private readonly string[] databaseNames = new[]
    {
      CoreDatabaseName, "master"
    };

    /// <summary>
    /// Localization installer instance.
    /// </summary>
    private LocalizationInstaller localizationInstaller;

    /// <summary>
    /// Site root.
    /// </summary>
    private string siteRoot;

    /// <summary>
    /// Gets or sets Installer.
    /// </summary>
    /// <value>
    /// The installer.
    /// </value>
    [NotNull]
    public virtual LocalizationInstaller Installer
    {
      get
      {
        if (this.localizationInstaller == null)
        {
          LanguageManagerWrapper languageManagerWrapper = new LanguageManagerWrapper(Factory.GetDatabase(CoreDatabaseName));
          JobManagerWrapper jobManagerWrapper = new JobManagerWrapper();
          this.localizationInstaller = new LocalizationInstaller(languageManagerWrapper, jobManagerWrapper);
        }

        return this.localizationInstaller;
      }

      set
      {
        this.localizationInstaller = value;
      }
    }

    /// <summary>
    /// Gets or sets the site root.
    /// </summary>
    /// <value>
    /// The site root.
    /// </value>
    [NotNull]
    public virtual string SiteRoot
    {
      get
      {
        return this.siteRoot ?? (this.SiteRoot = AppDomain.CurrentDomain.BaseDirectory);
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.siteRoot = value;
      }
    }

    /// <summary>
    /// Runs this post step
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="metaData">The meta data.</param>
    public virtual void Run(ITaskOutput output, NameValueCollection metaData)
    {
      Assert.ArgumentNotNull(output, "output");
      Assert.ArgumentNotNull(metaData, "metaData");

      foreach (string path in this.GetFilesPathes(metaData))
      {
        foreach (string databaseName in this.databaseNames)
        {
          this.Installer.Install(databaseName, path);
        }
      }
    }

    /// <summary>
    /// Gets the files pathes.
    /// </summary>
    /// <param name="metaData">The meta data.</param>
    /// <returns>
    /// The files pathes.
    /// </returns>
    private IEnumerable<string> GetFilesPathes(NameValueCollection metaData)
    {
      const string MetadataAttributesKey = "Attributes";
      const string BaseFolderKey = "folder";
      const string DictionaryFilesKey = "files";
      const char KeyValueDelimeter = '=';
      const char FileNamesDelimeter = ';';
      const int SingleAttributeConvertedToArrayLength = 2;
      const int SingeAttributeConvertedToArrayValueIndex = 1;

      string attributesString = metaData[MetadataAttributesKey];
      if (string.IsNullOrEmpty(attributesString))
      {
        return Enumerable.Empty<string>();
      }

      ListString attributes = new ListString(attributesString);
      string folderString = attributes.FirstOrDefault(a => a.StartsWith(BaseFolderKey));
      string filesString = attributes.FirstOrDefault(a => a.StartsWith(DictionaryFilesKey));

      if (string.IsNullOrEmpty(folderString) || string.IsNullOrEmpty(filesString))
      {
        return Enumerable.Empty<string>();
      }

      string[] folderInfo = folderString.Split(KeyValueDelimeter);
      if (folderInfo.Length != SingleAttributeConvertedToArrayLength)
      {
        return Enumerable.Empty<string>();
      }

      string folder = folderInfo[SingeAttributeConvertedToArrayValueIndex];
      if (string.IsNullOrEmpty(folder))
      {
        return Enumerable.Empty<string>();
      }

      string[] filesInfo = filesString.Split(KeyValueDelimeter);
      if (filesInfo.Length != SingleAttributeConvertedToArrayLength)
      {
        return Enumerable.Empty<string>();
      }

      string files = filesInfo[SingeAttributeConvertedToArrayValueIndex];
      if (string.IsNullOrEmpty(files))
      {
        return Enumerable.Empty<string>();
      }

      string dir = Path.Combine(this.SiteRoot, folder);
      return files.Split(FileNamesDelimeter).Select(fileName => Path.Combine(dir, fileName));
    }
  }
}