// -------------------------------------------------------------------------------------------
// -------------------------------------------------------------------------------------------
// <copyright file="SearchEvents.cs" company="Sitecore Corporation">
//  Copyright (c) Sitecore Corporation 1999-2015 
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

namespace Sitecore.Ecommerce.Analytics.Components.PageEvents
{
  using Diagnostics;
  using Sitecore.Analytics;
  using Sitecore.Analytics.Data;

  using Utils;

  /// <summary>
  /// Searches extension.
  /// </summary>
  public class SearchEvents
  {
    /// <summary>
    /// Searches the specified page.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="hits">The number of hits.</param>
    public virtual void Search(string query, int hits)
    {
      Assert.ArgumentNotNull(query, "query");

      if (!Tracker.IsActive)
      {
        return;
      }

      AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
      Assert.IsNotNull(analyticsHelper, "analyticsHelper");

      if (hits == 0)
      {
        string description = analyticsHelper.GetPageEventDescription(EventConstants.EventNoSearchHitsFound);
        if (string.IsNullOrEmpty(description))
        {
          description = EventConstants.EventNoSearchHitsFound;
        }

        string text = description.FormatWith(new { Query = query });

        if (Tracker.Current.CurrentPage != null)
        {
            var pageEventData = new PageEventData(EventConstants.EventNoSearchHitsFound) { Text = text, Data = query };

            Tracker.Current.CurrentPage.Register(pageEventData);
        }
      }
      else
      {
        string description = analyticsHelper.GetPageEventDescription(EventConstants.EventSearch);
        if (string.IsNullOrEmpty(description))
        {
          description = EventConstants.EventSearch;
        }

        string text = description.FormatWith(new { Query = query });

        if (Tracker.Current.CurrentPage != null)
        {
            var pageEventData = new PageEventData(EventConstants.EventSearch) { Text = text, Data = query };
            Tracker.Current.CurrentPage.Register(pageEventData);
        }
      }
    }
  }
}