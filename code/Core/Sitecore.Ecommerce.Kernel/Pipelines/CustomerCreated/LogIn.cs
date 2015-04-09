// -------------------------------------------------------------------------------------------
// <copyright file="LogIn.cs" company="Sitecore Corporation">
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
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;
  using Sitecore.Security.Authentication;

  /// <summary>
  /// Log In customer processor
  /// </summary>
  public class LogIn
  {
    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void Process(PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      string fullNickName = args.CustomData["userName"] as string;

      if (!AuthenticationManager.Login(fullNickName))
      {
        Log.Warn(string.Format("User: '{0}' cannot be logged in", fullNickName), this);
      }
    }
  }
}