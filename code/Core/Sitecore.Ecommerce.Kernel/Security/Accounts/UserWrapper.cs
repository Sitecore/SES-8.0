// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserWrapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the user wrapper class.
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

namespace Sitecore.Ecommerce.Security.Accounts
{
  using System.Security.Principal;
  using Diagnostics;
  using SecurityModel;
  using Sitecore.Security;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Domains;

  /// <summary>
  /// Defines the user wrapper class.
  /// </summary>
  public class UserWrapper : User
  {
    /// <summary>
    /// The inned user.
    /// </summary>
    [NotNull]
    private readonly User innerUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserWrapper"/> class.
    /// </summary>
    /// <param name="user">The user.</param>
    public UserWrapper([NotNull] User user)
      : base(user.Name, user.IsAuthenticated)
    {
      Assert.ArgumentNotNull(user, "user");

      this.innerUser = user;
    }

    /// <summary>
    /// Gets the type of the account.
    /// </summary>
    /// <value>
    /// The type of the account.
    /// </value>
    public override AccountType AccountType
    {
      get { return this.InnerUser.AccountType; }
    }

    /// <summary>
    /// Gets the delegation.
    /// </summary>
    /// <value>
    /// The delegation.
    /// </value>
    public override UserDelegation Delegation
    {
      get { return this.InnerUser.Delegation; }
    }

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public override string Description
    {
      get { return this.InnerUser.Description; }
    }

    /// <summary>
    /// Gets the display name.
    /// </summary>
    /// <value>
    /// The display name.
    /// </value>
    public override string DisplayName
    {
      get { return this.InnerUser.DisplayName; }
    }

    /// <summary>
    /// Gets the domain to which the user belongs.
    /// </summary>
    /// <value>
    /// The domain.
    /// </value>
    public override Domain Domain
    {
      get { return this.InnerUser.Domain; }
    }

    /// <summary>
    /// Gets the identity of the current principal.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Security.Principal.IIdentity"></see> object associated with the current principal.
    /// </returns>
    [NotNull]
    public override IIdentity Identity
    {
      get { return this.InnerUser.Identity; }
    }

    /// <summary>
    /// Gets a value indicating whether the user is an administrator.
    /// </summary>
    /// <value><c>true</c> if the user is an administrator; otherwise, <c>false</c>.</value>
    public override bool IsAdministrator
    {
      get { return this.InnerUser.IsAdministrator; }
    }

    /// <summary>
    /// Gets a value indicating whether the user has been authenticated.
    /// </summary>
    /// <value><c>true</c> if the user has been authenticated; otherwise, <c>false</c>.</value>
    public override bool IsAuthenticated
    {
      get { return this.InnerUser.IsAuthenticated; }
    }

    /// <summary>
    /// Gets the name of the local.
    /// </summary>
    /// <value>
    /// The name of the local.
    /// </value>
    public override string LocalName
    {
      get { return this.InnerUser.LocalName; }
    }

    /// <summary>
    /// Gets the name of the account.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public override string Name
    {
      get { return this.InnerUser.Name; }
    }

    /// <summary>
    /// Gets the profile.
    /// </summary>
    /// <value>
    /// The profile.
    /// </value>
    public override UserProfile Profile
    {
      get { return this.InnerUser.Profile; }
    }

    /// <summary>
    /// Gets the roles that the user is a member of.
    /// </summary>
    /// <value>
    /// The roles.
    /// </value>
    public override UserRoles Roles
    {
      get { return this.InnerUser.Roles; }
    }

    /// <summary>
    /// Gets the runtime settings.
    /// </summary>
    /// <value>
    /// The runtime settings.
    /// </value>
    public override UserRuntimeSettings RuntimeSettings
    {
      get { return this.InnerUser.RuntimeSettings; }
    }

    /// <summary>
    /// Gets the inner user.
    /// </summary>
    [NotNull]
    protected virtual User InnerUser
    {
      get { return this.innerUser; }
    }

    /// <summary>
    /// Deletes the user from the underlying membership database.
    /// </summary>
    public override void Delete()
    {
      this.InnerUser.Delete();
    }

    /// <summary>
    /// Gets the name of the domain.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.String"/>.
    /// </returns>
    public override string GetDomainName()
    {
      return this.InnerUser.GetDomainName();
    }

    /// <summary>
    /// Gets the local name of the user.
    /// </summary>
    /// <returns>
    /// The name of the user without the domain.
    /// </returns>
    public override string GetLocalName()
    {
      return this.InnerUser.GetLocalName();
    }

    /// <summary>
    /// Determines whether the user belongs to the specified role.
    /// </summary>
    /// <param name="role">The role.</param>
    /// <returns>
    /// true if the user is a member of the specified role; otherwise, false.
    /// </returns>
    public override bool IsInRole([NotNull] Role role)
    {
      Assert.ArgumentNotNull(role, "role");

      return this.InnerUser.IsInRole(role);
    }

    /// <summary>
    /// Determines whether the current principal belongs to the specified role.
    /// </summary>
    /// <param name="roleName">The name of the role for which to check membership.</param>
    /// <returns>
    /// true if the current principal is a member of the specified role; otherwise, false.
    /// </returns>
    public override bool IsInRole([NotNull] string roleName)
    {
      Assert.ArgumentNotNull(roleName, "roleName");

      return this.InnerUser.IsInRole(roleName);
    }
  }
}