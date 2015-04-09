// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateRolesPostStep.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The create roles post step.
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

namespace Sitecore.Ecommerce.Install.Upgrade
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Web.Security;
  using Diagnostics;
  using OrderManagement;
  using Sitecore.Install.Framework;
  using Sitecore.Security.Accounts;

  /// <summary>
  /// The create roles post step.
  /// </summary>
  public class CreateRolesPostStep : IPostStep
  {
    /// <summary>
    /// The run.
    /// </summary>
    /// <param name="output">
    /// The output. 
    /// </param>
    /// <param name="metaData">
    /// The meta data. 
    /// </param>
    public void Run(ITaskOutput output, NameValueCollection metaData)
    {
      Log.Info("Installing Order Manager roles.", this);

      this.CreateRoles();
      this.AddRolesToRoles();
    }

    /// <summary>
    /// Creates the roles.
    /// </summary>
    private void CreateRoles()
    {
      this.CreateIfNotExists(OrderManagerRole.OrderManagerAccounting);
      this.CreateIfNotExists(OrderManagerRole.OrderManagerAdministrators);
      this.CreateIfNotExists(OrderManagerRole.OrderManagerClientUsers);
      this.CreateIfNotExists(OrderManagerRole.OrderManagerCustomerService);
      this.CreateIfNotExists(OrderManagerRole.OrderManagerEveryone);
      this.CreateIfNotExists(OrderManagerRole.OrderManagerLogViewers);
      this.CreateIfNotExists(OrderManagerRole.OrderManagerPackaging);
      this.CreateIfNotExists(OrderManagerRole.OrderManagerProcessing);
      this.CreateIfNotExists(OrderManagerRole.OrderManagerExampleProcessing);
      this.CreateIfNotExists(OrderManagerRole.OrderManagerSales);
      this.CreateIfNotExists(OrderManagerRole.OrderManagerShipping);
    }

    /// <summary>
    /// Creates if not exists.
    /// </summary>
    /// <param name="roleName">Name of the role.</param>
    private void CreateIfNotExists(string roleName)
    {
      if (Role.Exists(roleName))
      {
        return;
      }

      Roles.CreateRole(roleName);
    }

    /// <summary>
    /// Adds the roles to roles.
    /// </summary>
    private void AddRolesToRoles()
    {
      var accounting = Role.FromName(OrderManagerRole.OrderManagerAccounting);
      var administrators = Role.FromName(OrderManagerRole.OrderManagerAdministrators);
      var clientUsers = Role.FromName(OrderManagerRole.OrderManagerClientUsers);
      var customerService = Role.FromName(OrderManagerRole.OrderManagerCustomerService);
      var everyone = Role.FromName(OrderManagerRole.OrderManagerEveryone);
      var logViewers = Role.FromName(OrderManagerRole.OrderManagerLogViewers);
      var packaging = Role.FromName(OrderManagerRole.OrderManagerPackaging);
      var processing = Role.FromName(OrderManagerRole.OrderManagerProcessing);
      var exampleProcessing = Role.FromName(OrderManagerRole.OrderManagerExampleProcessing);
      var sales = Role.FromName(OrderManagerRole.OrderManagerSales);
      var shipping = Role.FromName(OrderManagerRole.OrderManagerShipping);

      var everyoneChildren = new List<Role>
      {
        accounting,
        administrators,
        customerService,
        packaging,
        processing,
        sales,
        shipping
      };

      var clientUsersChildren = new List<Role>
      {
        accounting,
        administrators,
        customerService,
        packaging,
        processing,
        sales,
        shipping
      };

      var logViewersChildren = new List<Role>
      {
        administrators   
      };

      var processingChildren = new List<Role>
      {
        exampleProcessing
      };

      RolesInRolesManager.AddRolesToRole(everyoneChildren, everyone);
      RolesInRolesManager.AddRolesToRole(clientUsersChildren, clientUsers);
      RolesInRolesManager.AddRolesToRole(logViewersChildren, logViewers);
      RolesInRolesManager.AddRolesToRole(processingChildren, processing);
    }
  }
}