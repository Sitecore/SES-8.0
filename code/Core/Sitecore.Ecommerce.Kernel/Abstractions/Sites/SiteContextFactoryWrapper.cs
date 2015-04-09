// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteContextFactoryWrapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SiteContextFactoryWrapper type.
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

namespace Sitecore.Sites
{
  using System.Collections.Generic;
  using Web;

  /// <summary>
  /// Defines the site context factory wrapper class.
  /// </summary>
  public class SiteContextFactoryWrapper
  {
    /// <summary>
    /// Gets the sites.
    /// </summary>
    /// <value>The sites.</value>
    [NotNull]
    public virtual IEnumerable<SiteInfo> Sites
    {
      get { return SiteContextFactory.Sites; }
    }

    /// <summary>
    /// Gets the site context.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The site context.</returns>
    [CanBeNull]
    public virtual SiteContext GetSiteContext([NotNull] string name)
    {
      return SiteContextFactory.GetSiteContext(name);
    }
  }
}