// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextUser.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the context user class.
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
  using Sitecore.Security.Accounts;

  /// <summary>
  /// Defines the context user class.
  /// </summary>
  public class ContextUser : UserWrapper
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ContextUser"/> class.
    /// </summary>
    public ContextUser()
      : base(Sitecore.Context.User)
    {
    }

    /// <summary>
    /// Gets the inner user.
    /// </summary>
    protected override User InnerUser
    {
      get { return Sitecore.Context.User; }
    }
  }
}