// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResetSampleUserPaswordPostStep.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the reset sample user paswords class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Install
{
  using System.Collections.Specialized;
  using System.Web.Security;
  using Diagnostics;
  using Sitecore.Install.Framework;

  /// <summary>
  /// Defines the reset sample user paswords class.
  /// </summary>
  public class ResetSampleUserPaswordPostStep : IPostStep
  {
    /// <summary>
    /// Runs this post step
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="metaData">The meta data.</param>
    public virtual void Run([CanBeNull] ITaskOutput output, [CanBeNull] NameValueCollection metaData)
    {
      this.ResetPassword(@"sitecore\andrea", "a");
      this.ResetPassword(@"sitecore\cassandra", "c");
      this.ResetPassword(@"sitecore\sophia", "s");
    }

    /// <summary>
    /// Resets the password.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="password">The password.</param>
    private void ResetPassword([NotNull] string name, [NotNull] string password)
    {
      Debug.ArgumentNotNull(name, "name");
      Debug.ArgumentNotNull(password, "password");

      MembershipUser user = Membership.GetUser(name);
      if (user == null)
      {
        return;
      }

      if (!user.IsApproved)
      {
        user.IsApproved = true;
        Membership.UpdateUser(user);
      }

      string oldPassword = user.ResetPassword();
      user.ChangePassword(oldPassword, password);

      user.IsApproved = false;
      Membership.UpdateUser(user);
    }
  }
}