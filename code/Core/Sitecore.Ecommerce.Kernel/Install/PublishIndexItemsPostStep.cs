// -------------------------------------------------------------------------------------------
// <copyright file="PublishIndexItemsPostStep.cs" company="Sitecore Corporation">
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
  using Diagnostics;
  using Publishing;
  using SecurityModel;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Install.Framework;

  /// <summary>
  /// The Publish Index Items Post Action.
  /// </summary>
  public class PublishIndexItemsPostStep : IPostStep
  {

    /// <summary>
    /// Runs this post step
    /// </summary>
    /// <param name="output">The output.</param><param name="metaData">The meta data.</param>
    public void Run(ITaskOutput output, NameValueCollection metaData)
    {
      using (new SecurityDisabler())
      {
        this.PublishItem("{FB814ADD-C821-4326-A024-851AADEC3284}");
        this.PublishItem("{0A702337-81CD-45B9-8A72-EC15D2BE1635}");
      }
    }

    /// <summary>
    /// Publishes the item.
    /// </summary>
    /// <param name="id">The item id.</param>
    protected virtual void PublishItem(string id)
    {
      Assert.ArgumentNotNullOrEmpty("id", id);

      Database masterDatabase = Factory.GetDatabase("master");
      Assert.IsNotNull(masterDatabase, "Master database is null");
      
      Item examplesRootItem = masterDatabase.GetItem(id);
      Assert.IsNotNull(examplesRootItem, string.Concat("Can not find item with Id: ", id));

      Database webDatabase = Factory.GetDatabase("web");
      Assert.IsNotNull(webDatabase, "Web database is null");

      PublishManager.PublishItem(examplesRootItem, new[] { webDatabase }, examplesRootItem.Languages, false, true);
    }
  }
}