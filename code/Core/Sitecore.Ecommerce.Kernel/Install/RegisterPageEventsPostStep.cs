// -------------------------------------------------------------------------------------------
// <copyright file="RegisterPageEventsPostStep.cs" company="Sitecore Corporation">
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
  using Diagnostics;
  using SecurityModel;
  using Sitecore.Data.Items;
  using Sitecore.Install.Framework;

  /// <summary>
  /// The UTP Post Action. It is used to save E-Commerce OMS page events to the customers OMS Database. 
  /// It is necessary to make events working.
  /// </summary>
  public class RegisterPageEventsPostStep : IPostStep
  {
    /// <summary>
    /// Runs this post step
    /// </summary>
    /// <param name="output">The output.</param><param name="metaData">The meta data.</param>
    public void Run(ITaskOutput output, NameValueCollection metaData)
    {
      using (new SecurityDisabler())
      {
        Item pageEventsRootItem = Sitecore.Context.ContentDatabase.GetItem("{633273C1-02A5-4EBC-9B82-BD1A7C684FEA}");
        Assert.IsNotNull(pageEventsRootItem, "Page events root item is null.");

        foreach (Item pageEventItem in pageEventsRootItem.Children)
        {
          using (new EditContext(pageEventItem))
          {
            pageEventItem.Name = pageEventItem.Name;
          }
        }
      }
    }
  }
}