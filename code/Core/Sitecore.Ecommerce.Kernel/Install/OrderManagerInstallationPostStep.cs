// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderManagerInstallationPostStep.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
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
  using System.Linq;
  using Diagnostics;
  using Jobs.AsyncUI;
  using SecurityModel;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Proxies;
  using Sitecore.Install;
  using Sitecore.Install.Framework;
  using Text;

  /// <summary>
  /// Defines the order manager installation post step class.
  /// </summary>
  public class OrderManagerInstallationPostStep : IPostStep
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref = "OrderManagerInstallationPostStep" /> class.
    /// </summary>
    public OrderManagerInstallationPostStep()
    {
      string packages = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
      this.PackageInstaller = new UnifiedPackageInstaller(new InstallerWrapper(), packages);
    }

    /// <summary>
    ///   Gets or sets the package installer.
    /// </summary>
    /// <value>
    ///   The package installer.
    /// </value>
    public UnifiedPackageInstaller PackageInstaller { get; set; }

    /// <summary>
    /// Runs this post step
    /// </summary>
    /// <param name="output">
    /// The output.
    /// </param>
    /// <param name="metaData">
    /// The meta data.
    /// </param>
    public void Run([NotNull] ITaskOutput output, [NotNull] NameValueCollection metaData)
    {
      Assert.ArgumentNotNull(output, "output");
      Assert.ArgumentNotNull(metaData, "metaData");

      string attributes = metaData["Attributes"];
      if (string.IsNullOrEmpty(attributes))
      {
        return;
      }

      string packages = new ListString(attributes).Where(a => a.StartsWith("innerpackages=")).Select(s => s.Substring("innerpackages=".Length)).FirstOrDefault();
      if (string.IsNullOrEmpty(packages))
      {
        return;
      }

      using (new SecurityDisabler())
      {
        using (new ProxyDisabler())
        {
          using (new SyncOperationContext())
          {
            Callback cb = () =>
            {
              this.PackageInstaller.InstallPackages(packages);
              return true;
            };

            output.Execute(cb);
          }
        }
      }
    }
  }
}