// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticsHelper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The analytics helper.
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

namespace Sitecore.Ecommerce.Analytics
{
  using Diagnostics;
  using Sitecore.Data.Items;

  /// <summary>
  /// The analytics helper.
  /// </summary>
  public class AnalyticsHelper
  {
    /// <summary>
    /// Gets the description from event.
    /// </summary>
    /// <param name="eventName">Name of the event.</param>
    /// <returns>The description from event.</returns>
    public virtual string GetPageEventDescription(string eventName)
    {
      Assert.ArgumentNotNullOrEmpty(eventName, "eventName");

      Item goalRootItem = Sitecore.Context.ContentDatabase.GetItem("/sitecore/system/Marketing Control Panel/Goals");
      Assert.IsNotNull(goalRootItem, "Goal root item is null");

      Item pageEventRootItem = Sitecore.Context.ContentDatabase.GetItem("/sitecore/system/Settings/Analytics/Page Events");
      Assert.IsNotNull(pageEventRootItem, "Page event root item is null");

      string @event = string.Format(".//*[@Name='{0}']", eventName);
      Item eventItem = goalRootItem.Axes.SelectSingleItem(@event) ?? pageEventRootItem.Axes.SelectSingleItem(@event);
      Assert.IsNotNull(eventItem, string.Format("The event item {0} cannot be found", eventName));

      string typeOfEvent = string.Empty;

      if (eventItem["IsGoal"] == "1")
      {
        typeOfEvent = " [Goal]";
      }
      else if (eventItem["IsFailure"] == "1")
      {
        typeOfEvent = " [Failure]";
      }

      return string.Format("{0}{1}", eventItem["Description"], typeOfEvent);
    }
  }
}