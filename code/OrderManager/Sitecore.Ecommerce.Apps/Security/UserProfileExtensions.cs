// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfileExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the UserProfileExtensions type.
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

namespace Sitecore.Ecommerce.Apps.Security
{
  using Sitecore.Security;

  /// <summary>
  /// The user profile extensions.
  /// </summary>
  public static class UserProfileExtensions
  {
    /// <summary>
    /// The selected context key.
    /// </summary>
    private const string SelectedContextKey = "SelectedShopContext";

    /// <summary>
    /// The default context key.
    /// </summary>
    private const string DefaultContextKey = "DefaultShopContext";

    /// <summary>
    /// Gets the selected shop context.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <returns>The selected shop context.</returns>
    public static string GetSelectedShopContext(this UserProfile profile)
    {
      return profile.GetCustomProperty(SelectedContextKey);
    }

    /// <summary>
    /// Sets the selected shop context.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <param name="siteName">Name of the site.</param>
    public static void SetSelectedShopContext(this UserProfile profile, string siteName)
    {
      profile.SetCustomProperty(SelectedContextKey, siteName);
    }

    /// <summary>
    /// Gets the default shop context.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <returns>Name of the site.</returns>
    public static string GetDefaultShopContext(this UserProfile profile)
    {
      return profile.GetCustomProperty(DefaultContextKey);
    }

    /// <summary>
    /// Sets the default shop context.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <param name="siteName">Name of the site.</param>
    public static void SetDefaultShopContext(this UserProfile profile, string siteName)
    {
      profile.SetCustomProperty(DefaultContextKey, siteName);
    }
  }
}