// -------------------------------------------------------------------------------------------
// <copyright file="NavigationEvents.cs" company="Sitecore Corporation">
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
    using Configuration;
    using Diagnostics;

    using Sitecore.Analytics;
    using Sitecore.Web;
    using Text;
    using Utils;

    using PageEventData = Sitecore.Analytics.Data.PageEventData;

    /// <summary>
    /// The Navigation extension.
    /// </summary>
    public class NavigationEvents
    {
        /// <summary>
        /// Tabs the selected.
        /// </summary>
        /// <param name="tabName">Name of the tab.</param>
        public virtual void TabSelected(string tabName)
        {
            Assert.ArgumentNotNull(tabName, "tabName");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventTabSelected);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventTabSelected;
            }

            string text = description.FormatWith(new { TabName = tabName, });
            ListString data = new ListString { tabName };

            var currentPage = Tracker.Current.CurrentPage;

            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventTabSelected) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Products the reviewed.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="rate">The product rate.</param>
        public virtual void ProductReviewed(string productCode, string productName, string title, string description, string rate)
        {
            Assert.ArgumentNotNull(title, "title");
            Assert.ArgumentNotNull(description, "description");
            Assert.ArgumentNotNull(rate, "rate");
            Assert.ArgumentNotNull(productCode, "productCode");
            Assert.ArgumentNotNull(productName, "productName");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string eventDescription = analyticsHelper.GetPageEventDescription(EventConstants.EventProductReviewed);
            if (string.IsNullOrEmpty(eventDescription))
            {
                eventDescription = EventConstants.EventProductReviewed;
            }

            string text = eventDescription.FormatWith(new { Title = title, Text = description, Rate = rate, ProductName = productName });
            ListString data = new ListString { productCode, title, description, rate, productName };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventProductReviewed) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Used to read the querystring parameter from a productlink in list and register event in analytics database.
        /// </summary>
        public virtual void FollowListHit()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            string setting = Settings.GetSetting("Ecommerce.Analytics.EventQueryStringKey");

            UrlString urlString = new UrlString(WebUtil.GetRawUrl());

            string listName = urlString[setting + "_data"] ?? string.Empty;

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string eventDescription = analyticsHelper.GetPageEventDescription(EventConstants.EventUserClickedItemInList);
            if (string.IsNullOrEmpty(eventDescription))
            {
                eventDescription = EventConstants.EventUserClickedItemInList;
            }

            string text = eventDescription.FormatWith(new { List = listName, ProductName = Sitecore.Context.Item["Title"] });
            ListString data = new ListString { listName, Sitecore.Context.Item["Title"] };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventUserClickedItemInList) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Adds the trigger event string to query string.
        /// </summary>
        /// <param name="url">The request URL.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <returns>The trigger event string to query string.</returns>
        public virtual string AddTriggerEventStringToQueryString(string url, string eventName)
        {
            Assert.ArgumentNotNull(url, "url");
            Assert.ArgumentNotNull(eventName, "eventName");

            string setting = Settings.GetSetting("Ecommerce.Analytics.EventQueryStringKey");

            UrlString urlString = new UrlString(url);
            urlString[setting] = eventName;

            return urlString.ToString();
        }

        /// <summary>
        /// Adds the follow list to query string.
        /// </summary>
        /// <param name="url">The request URL.</param>
        /// <param name="listName">Name of the list.</param>
        /// <returns>The follow list to query string.</returns>
        public virtual string AddFollowListToQueryString(string url, string listName)
        {
            Assert.ArgumentNotNull(url, "url");
            Assert.ArgumentNotNull(listName, "listName");

            string setting = Settings.GetSetting("Ecommerce.Analytics.EventQueryStringKey");

            UrlString urlString = new UrlString(url);
            urlString[setting] = "followlist";
            urlString[setting + "_data"] = listName;

            return urlString.ToString();
        }
    }
}