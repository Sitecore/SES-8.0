// -------------------------------------------------------------------------------------------
// <copyright file="Builder.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Shell.Applications.Search.RebuildSearchIndex
{
  using System;
  using Diagnostics;
  using Jobs;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Search;
  using Text;

  /// <summary>
  /// Indexes Builder
  /// </summary>
  public class Builder
  {
    /// <summary>
    /// Database Names
    /// </summary>
    private readonly ListString databaseNames;

    /// <summary>
    /// Initializes a new instance of the <see cref="Builder"/> class.
    /// </summary>
    /// <param name="databaseNames">The database names.</param>
    public Builder(string databaseNames)
    {
      Assert.ArgumentNotNull(databaseNames, "databaseNames");

      this.databaseNames = new ListString(databaseNames);
    }

    /// <summary>
    /// Builds this instance.
    /// </summary>
    protected void Build()
    {
      Job job = Sitecore.Context.Job;
      if (job != null)
      {
        try
        {        
          foreach (string databaseName in this.databaseNames)
          {
            if (databaseName.StartsWith("$"))
            {              
              Index index = SearchManager.GetIndex(databaseName.Substring(1));
              Assert.IsNotNull(index, databaseName.Substring(1));
              index.Rebuild();
            }
            else
            {
              Database database = Factory.GetDatabase(databaseName, false);
              if (database == null)
              {
                continue;
              }

              for (int i = 0; i < database.Indexes.Count; i++)
              {
                database.Indexes[i].Rebuild(database);
                Log.Audit(this, "Rebuild search index: {0}", new[] { database.Name });
              }
            }

            JobStatus status = job.Status;
            status.Processed += 1L;
          }
        }
        catch (Exception exception)
        {
          job.Status.Failed = true;
          job.Status.Messages.Add(exception.ToString());
        }

        job.Status.State = JobState.Finished;
      }
    }
  }
}