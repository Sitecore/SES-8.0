// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerMembership.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the customer membership class.
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

namespace Sitecore.Ecommerce.Security
{
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using Sitecore.Security.Accounts;
  using Text;

  /// <summary>
  /// Defines the customer membership class.
  /// </summary>
  public class CustomerMembership
  {
    /// <summary>
    /// The existent roles.
    /// </summary>
    private IEnumerable<string> defaultCustomerRoles;


    /// <summary>
    /// Gets or sets the shop context.
    /// </summary>
    /// <value>
    /// The shop context.
    /// </value>
    [NotNull]
    public ShopContext ShopContext { get; set; }

    /// <summary>
    /// Gets or sets the existent roles.
    /// </summary>
    /// <value>
    /// The existent roles.
    /// </value>
    [NotNull]
    public IEnumerable<string> DefaultCustomerRoles
    {
      get
      {
        this.EnsureCustomerRolesIsResolved();
        return this.defaultCustomerRoles;
      }

      protected set
      {
        Assert.ArgumentNotNull(value, "value");
        this.defaultCustomerRoles = value;
      }
    }

    /// <summary>
    /// Determines whether the specified user is customer.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>
    ///   <c>true</c> if the specified user is customer; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool IsCustomer([NotNull] User user)
    {
      Assert.ArgumentNotNull(user, "user");

      if (user.Domain.IsAnonymousUser(user.Name))
      {
        return false;
      }

      if (!this.DefaultCustomerRoles.Any())
      {
        return false;
      }

      return this.DefaultCustomerRoles.All(user.IsInRole);
    }

    /// <summary>
    /// Ensures the customer roles is resolved.
    /// </summary>
    private void EnsureCustomerRolesIsResolved()
    {
      if (this.defaultCustomerRoles == null)
      {
        this.DefaultCustomerRoles = new ListString(this.ShopContext.GeneralSettings.DefaultCustomerRoles);
      }
    }
  }
}