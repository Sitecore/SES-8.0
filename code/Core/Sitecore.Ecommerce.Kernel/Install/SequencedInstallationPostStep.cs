// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SequencedInstallationPostStep.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the sequenced installation post step class.
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

namespace Sitecore.Ecommerce.Install
{
  using System;
  using System.Collections.Specialized;
  using System.IO;
  using Diagnostics;
  using Localization;
  using SecurityModel;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Proxies;
  using Sitecore.Install.Framework;

  /// <summary>
  /// Defines the sequenced installation post step class.
  /// </summary>
  public class SequencedInstallationPostStep : IPostStep
  {
    /// <summary>
    /// Packages directory name.
    /// </summary>
    private const string PackagesDirName = "temp";

    /// <summary>
    /// Packages names.
    /// </summary>
    private readonly string[] packages = new[]
    {
      "Speak.zip", 
      "Sitecore E-Commerce Order Manager Standalone.zip" 
    };

    /// <summary>
    /// SequencedInstaller instance.
    /// </summary>
    private SequencedInstaller sequencedInstaller;

    /// <summary>
    /// Site root for current instance.
    /// </summary>
    private string siteRoot;

    /// <summary>
    ///  OmLocalizationPostStep instance.
    /// </summary>
    private LocalizationPostStep localizationPostStep;

    /// <summary>
    /// Gets or sets the sequenced installer.
    /// </summary>
    /// <value>
    /// The sequenced installer.
    /// </value>
    [NotNull]
    public virtual SequencedInstaller SequencedInstaller
    {
      get
      {
        return this.sequencedInstaller ?? (this.sequencedInstaller = new SequencedInstaller());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.sequencedInstaller = value;
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
    /// Gets or sets the localization post step.
    /// </summary>
    /// <value>
    /// The localization post step.
    /// </value>
    [NotNull]
    public virtual LocalizationPostStep LocalizationPostStep
    {
      get
      {
        return this.localizationPostStep ?? (this.localizationPostStep = new LocalizationPostStep());
      }

      set
      {
        this.localizationPostStep = value;
      }
    }

    /// <summary>
    /// Runs this post step
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="metaData">The meta data.</param>
    public virtual void Run(ITaskOutput output, NameValueCollection metaData)
    {
      using (new SecurityDisabler())
      {
        using (new ProxyDisabler())
        {
          using (new SyncOperationContext())
          {
            foreach (string package in this.packages)
            {
              string dir = Path.Combine(this.SiteRoot, PackagesDirName);
              string path = Path.Combine(dir, package);
              this.SequencedInstaller.Install(path);
              Log.Info(string.Format("The package {0} has been installed", package), this);
            }
          }
        }
      }

      this.LocalizationPostStep.Run(output, metaData);
    }
  }
}