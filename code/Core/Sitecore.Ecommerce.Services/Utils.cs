// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utils.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The service utilites.
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

namespace Sitecore.Ecommerce.Services
{
  using Diagnostics;
  using Sitecore.Pipelines;
  using Sites;
  using Visitor.OrderManagement;
  using Visitor.Pipelines.HttpRequest;

  /// <summary>
  /// The service utilites.
  /// </summary>
  public static class Utils
  {
    /// <summary>
    /// Gets the existing site context.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The site context.</returns>
    [NotNull]
    public static SiteContext GetExistingSiteContext([NotNull] ServiceClientArgs args)
    {
      Debug.ArgumentNotNull(args, "args");

      Assert.IsNotNullOrEmpty(args.SiteName, "Site name reqired.");

      SiteContext site = SiteContextFactory.GetSiteContext(args.SiteName);
      Assert.IsNotNull(site, "Unable to resolve site.");

      VisitorShopResolvingProcessor shopReolver = new VisitorShopResolvingProcessor();
      using (new SiteContextSwitcher(site))
      {
        shopReolver.Process(new PipelineArgs());
      }

      return site;
    }

    /// <summary>
    /// Sets the customer id.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <param name="orderManager">The order manager.</param>
    internal static void SetCustomerId(ServiceClientArgs args, object orderManager)
    {
      if (orderManager is IUserAware)
      {
        ((IUserAware)orderManager).CustomerId = args.CustomerId;
      }
    }
  }
}