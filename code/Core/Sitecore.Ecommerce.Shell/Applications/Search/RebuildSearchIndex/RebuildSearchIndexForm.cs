// -------------------------------------------------------------------------------------------
// <copyright file="RebuildSearchIndexForm.cs" company="Sitecore Corporation">
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
  using System.Collections.Generic;
  using System.Web.UI;
  using Diagnostics;
  using Globalization;
  using Jobs;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Search;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.Web.UI.Pages;
  using Text;

  /// <summary>
  /// Rebuid Search Indexes Form
  /// </summary>
  public class RebuildSearchIndexForm : WizardForm
  {
    /// <summary>
    /// Error text
    /// </summary>
    protected Memo ErrorText;

    /// <summary>
    /// Indexes Container
    /// </summary>
    protected Border Indexes;

    /// <summary>
    /// The result text
    /// </summary>
    protected Memo ResultText;

    /// <summary>
    /// Gets or sets the index map.
    /// </summary>
    /// <value>The index map.</value>
    public string IndexMap
    {
      get
      {
        return StringUtil.GetString(ServerProperties["IndexMap"]);
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        ServerProperties["IndexMap"] = value;
      }
    }

    /// <summary>
    /// Raises the load event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
      base.OnLoad(e);
      if (!Sitecore.Context.ClientPage.IsEvent)
      {
        this.BuildIndexes();
      }
    }

    /// <summary>
    /// Builds the indexes.
    /// </summary>
    protected virtual void BuildIndexes()
    {
      ListString selected = new ListString(Registry.GetString("/Current_User/Rebuild Search Index/Selected"));
      ListString indexMap = new ListString();
      foreach (string databaseName in Factory.GetDatabaseNames())
      {
        Database database = Factory.GetDatabase(databaseName);
        Assert.IsNotNull(database, "database");
        if (database.Indexes.Count > 0)
        {
          this.BuildIndexCheckbox(databaseName, databaseName, selected, indexMap);
        }
      }

      foreach (string indexName in this.GetIndexes())
      {
        string name = string.Format("${0}", indexName);
        this.BuildIndexCheckbox(name, indexName, selected, indexMap);
      }

      this.IndexMap = indexMap.ToString();
    }

    /// <summary>
    /// Builds the index checkbox.
    /// </summary>
    /// <param name="name">The index name.</param>
    /// <param name="header">The checkbox header.</param>
    /// <param name="selected">The selected checkboxes.</param>
    /// <param name="indexMap">The index map.</param>
    protected virtual void BuildIndexCheckbox(string name, string header, ListString selected, ListString indexMap)
    {
      Assert.ArgumentNotNull(name, "name");
      Assert.ArgumentNotNull(header, "header");
      Assert.ArgumentNotNull(selected, "selected");
      Assert.ArgumentNotNull(indexMap, "indexMap");

      Checkbox child = new Checkbox();
      this.Indexes.Controls.Add(child);
      child.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
      child.Header = header;
      child.Value = name;
      child.Checked = selected.Contains(name);
      indexMap.Add(child.ID);
      indexMap.Add(name);
      this.Indexes.Controls.Add(new LiteralControl("<br />"));
    }

    /// <summary>
    /// Gets the indexes.
    /// </summary>
    /// <returns>returns a collection of available indexes names</returns>
    protected virtual IEnumerable<string> GetIndexes()
    {
      SearchConfiguration configuration = Factory.CreateObject("search/configuration", true) as SearchConfiguration;
      if (configuration != null)
      {
        return configuration.Indexes.Keys;
      }

      return new List<string>();
    }

    /// <summary>
    /// Called when the active page has been changed.
    /// </summary>
    /// <param name="page">The page that has been entered.</param>
    /// <param name="oldPage">The page that was left.</param>
    /// <contract>
    /// <requires name="page" condition="not null"/>
    /// <requires name="oldPage" condition="not null"/>
    /// </contract>
    protected override void ActivePageChanged(string page, string oldPage)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(oldPage, "oldPage");
      base.ActivePageChanged(page, oldPage);
      NextButton.Header = "Next >";

      if (page == "Database")
      {
        NextButton.Header = "Rebuild >";
      }

      if (page == "Rebuilding")
      {
        NextButton.Disabled = true;
        BackButton.Disabled = true;
        CancelButton.Disabled = true;
        Sitecore.Context.ClientPage.ClientResponse.Timer("StartRebuilding", 10);
      }
    }

    /// <summary>
    /// Called when the active page is changing.
    /// </summary>
    /// <param name="page">The page that is being left.</param>
    /// <param name="newpage">The new page that is being entered.</param>
    /// <returns>
    /// True, if the change is allowed, otherwise false.
    /// </returns>
    /// <remarks>Set the newpage parameter to another page ID to control the
    /// path through the wizard pages.</remarks>
    /// <contract>
    /// <requires name="page" condition="not null"/>
    /// <requires name="newpage" condition="not null"/>
    /// </contract>
    protected override bool ActivePageChanging(string page, ref string newpage)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(newpage, "newpage");
      if ((page == "Retry") && (newpage == "Rebuilding"))
      {
        newpage = "Database";
        NextButton.Disabled = false;
        BackButton.Disabled = false;
        CancelButton.Disabled = false;
      }

      return base.ActivePageChanging(page, ref newpage);
    }

    /// <summary>
    /// Starts the rebuilding.
    /// </summary>
    protected virtual void StartRebuilding()
    {
      ListString databases = new ListString();
      ListString indexMap = new ListString(this.IndexMap);
      foreach (string formKey in Sitecore.Context.ClientPage.ClientRequest.Form.Keys)
      {
        if (!string.IsNullOrEmpty(formKey) && formKey.StartsWith("dk_"))
        {
          int index = indexMap.IndexOf(formKey);
          if (index >= 0)
          {
            databases.Add(indexMap[index + 1]);
          }
        }
      }

      Registry.SetString("/Current_User/Rebuild Search Index/Selected", databases.ToString());
      JobOptions options = new JobOptions("RebuildSearchIndex", "index", Client.Site.Name, new Builder(databases.ToString()), "Build")
      {
        AfterLife = TimeSpan.FromMinutes(1.0),
        ContextUser = Sitecore.Context.User
      };
      Job job = JobManager.Start(options);
      Sitecore.Context.ClientPage.ServerProperties["handle"] = job.Handle.ToString();
      Sitecore.Context.ClientPage.ClientResponse.Timer("CheckStatus", 500);
    }

    /// <summary>
    /// Checks the status.
    /// </summary>
    protected virtual void CheckStatus()
    {
      string handle = Sitecore.Context.ClientPage.ServerProperties["handle"] as string;
      Assert.IsNotNullOrEmpty(handle, "handle");
      Job job = JobManager.GetJob(Handle.Parse(handle));
      if (job == null)
      {
        Active = "Retry";
        NextButton.Disabled = true;
        BackButton.Disabled = false;
        CancelButton.Disabled = false;
        this.ErrorText.Value = "Job has finished unexpectedly";
      }
      else if (job.Status.Failed)
      {
        Active = "Retry";
        NextButton.Disabled = true;
        BackButton.Disabled = false;
        CancelButton.Disabled = false;
        this.ErrorText.Value = StringUtil.StringCollectionToString(job.Status.Messages);
      }
      else
      {
        string status;
        if (job.Status.State == JobState.Running)
        {
          status = Translate.Text("Processed {0} items. ", new object[] { job.Status.Processed, job.Status.Total });
        }
        else
        {
          status = Translate.Text("Queued.");
        }

        if (job.IsDone)
        {
          Active = "LastPage";
          BackButton.Disabled = true;
          this.ResultText.Value = StringUtil.StringCollectionToString(job.Status.Messages);
        }
        else
        {
          Sitecore.Context.ClientPage.ClientResponse.SetInnerHtml("Status", status);
          Sitecore.Context.ClientPage.ClientResponse.Timer("CheckStatus", 500);
        }
      }
    }
  }
}