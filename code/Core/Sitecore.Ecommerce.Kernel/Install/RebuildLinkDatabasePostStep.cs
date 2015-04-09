// -------------------------------------------------------------------------------------------
// <copyright file="RebuildLinkDatabasePostStep.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Install
{
  using System.Collections.Specialized;
  using Configuration;
  using SecurityModel;
  using Sitecore.Data;
  using Sitecore.Install.Framework;

  /// <summary>
  /// The rebuild link database post action.
  /// </summary>
  public class RebuildLinkDatabasePostStep : IPostStep
  {
    /// <summary>
    /// Runs this post step
    /// </summary>
    /// <param name="output">The output.</param><param name="metaData">The meta data.</param>
    public void Run(ITaskOutput output, NameValueCollection metaData)
    {
      using (new SecurityDisabler())
      {
        this.RebuildLinkDatabase("master");
        this.RebuildLinkDatabase(Constants.CoreDatabaseName);
      }
    }

    /// <summary>
    /// Rebuilds the link database.
    /// </summary>
    /// <param name="databaseName">Name of the database.</param>
    protected virtual void RebuildLinkDatabase(string databaseName)
    {
      Database database = Factory.GetDatabase(databaseName);
      if (database == null)
      {
        return;
      }

      Globals.LinkDatabase.Rebuild(database);
    }
  }
}