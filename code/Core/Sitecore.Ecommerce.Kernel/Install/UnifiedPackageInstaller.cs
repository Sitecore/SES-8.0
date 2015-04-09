// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnifiedPackageInstaller.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the unified package installer class.
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
  using System.Linq;
  using IO;
  using Sitecore.Install;
  using Text;

  /// <summary>
  /// Defines the unified package installer class.
  /// </summary>
  public class UnifiedPackageInstaller
  {
    /// <summary>
    /// The installer wrapper.
    /// </summary>
    private readonly InstallerWrapper installer;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnifiedPackageInstaller"/> class.
    /// </summary>
    /// <param name="installer">The installer.</param>
    /// <param name="packagesFolderFolder">The packages folder folder.</param>
    public UnifiedPackageInstaller([NotNull] InstallerWrapper installer, [NotNull] string packagesFolderFolder)
    {
      Diagnostics.Assert.ArgumentNotNull(installer, "installer");
      Diagnostics.Assert.ArgumentNotNull(packagesFolderFolder, "packagesFolderFolder");

      this.installer = installer;
      this.PackagesFolder = packagesFolderFolder;
    }

    /// <summary>
    /// Gets or sets the packages folder.
    /// </summary>
    /// <value>
    /// The packages folder.
    /// </value>
    public string PackagesFolder { get; set; }

    /// <summary>
    /// Installs the packages.
    /// </summary>
    /// <param name="packages">The packages.</param>
    public virtual void InstallPackages([NotNull] string packages)
    {
      Diagnostics.Assert.ArgumentNotNull(packages, "packages");

      foreach (string path in new ListString(packages, ';').Select(packageName => FileUtil.MakePath(this.PackagesFolder, packageName)))
      {
        var pathWithExtension = path;
        if (!path.EndsWith(".zip"))
        {
          pathWithExtension += ".zip";
        }

        this.installer.InstallPackage(pathWithExtension);
      }
    }
  }
}