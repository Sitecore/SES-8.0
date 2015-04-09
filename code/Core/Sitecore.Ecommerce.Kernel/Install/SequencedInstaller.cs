// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SequencedInstaller.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the sequenced installer class.
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
  using Diagnostics;
  using Sitecore.Install;

  /// <summary>
  /// Defines the sequenced installer class.
  /// </summary>
  public class SequencedInstaller
  {
    /// <summary>
    /// Inner InstallerWrapper instance.
    /// </summary>
    private readonly InstallerWrapper installer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SequencedInstaller"/> class.
    /// </summary>
    /// <param name="installer">The installer.</param>
    public SequencedInstaller([NotNull] InstallerWrapper installer)
    {
      Assert.ArgumentNotNull(installer, "installer");

      this.installer = installer;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SequencedInstaller"/> class.
    /// </summary>
    public SequencedInstaller() : this(new InstallerWrapper())
    {
    }

    /// <summary>
    /// Installs the specified file zip.
    /// </summary>
    /// <param name="path">The path.</param>
    public virtual void Install([NotNull] string path)
    {
      Assert.ArgumentNotNull(path, "path"); 

      this.installer.InstallPackage(path);
      this.installer.InstallSecurity(path);
    }
  }
}