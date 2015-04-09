// -------------------------------------------------------------------------------------------
// <copyright file="ConfigureSecurity.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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

namespace Sitecore.Ecommerce.Pipelines.CustomerCreated
{
  using System.Web.Security;
  using Diagnostics;
  using DomainModel.Configurations;
  using Sitecore.Collections;
  using Sitecore.Pipelines;
  using Sitecore.Security.Accounts;

  /// <summary>
  /// Configure user security
  /// </summary>
  public class ConfigureSecurity
  {
    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void Process(PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      User user = args.CustomData["user"] as User;
      if (user == null)
      {
        Log.Error("The user is null", this);
        return;
      }

      GeneralSettings generalSettings = Context.Entity.GetConfiguration<GeneralSettings>();

      StringList roles = new StringList(generalSettings.DefaultCustomerRoles);
      foreach (string role in roles)
      {
        if (Roles.RoleExists(role))
        {
          user.Roles.Add(Role.FromName(role));
        }
        else
        {
          Log.Warn(string.Format("Role: '{0}' does not exist", role), this);
        }
      }
    }
  }
}