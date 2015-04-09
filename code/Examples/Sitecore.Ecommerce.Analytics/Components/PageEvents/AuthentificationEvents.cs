// -------------------------------------------------------------------------------------------
// <copyright file="AuthentificationEvents.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Analytics.Components.PageEvents
{
    using Diagnostics;
    using Sitecore.Analytics;
    using Sitecore.Analytics.Data;

    using Text;
    using Utils;

    /// <summary>
    /// The authentification enevts.
    /// </summary>
    public class AuthentificationEvents
    {
        /// <summary>
        /// Clickeds the login button.
        /// </summary>
        public virtual void ClickedLoginButton()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventClickedLoginButton);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventClickedLoginButton, description);
            }
        }

        /// <summary>
        /// Click the login link.
        /// </summary>
        public virtual void ClickedLoginLink()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventClickedLoginLink);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventClickedLoginLink, description);
            }
        }

        /// <summary>
        /// User logged out.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public virtual void UserLoggedOut(string userName)
        {
            Assert.ArgumentNotNull(userName, "userName");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventUserLoggedOut);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventUserLoggedOut;
            }

            string text = description.FormatWith(new { Username = userName, });
            ListString data = new ListString { userName };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventUserLoggedOut) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// User login failed.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public virtual void UserLoginFailed(string userName)
        {
            Assert.ArgumentNotNull(userName, "userName");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventUserLoginFailed);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventUserLoginFailed;
            }

            string text = description.FormatWith(new { Username = userName, });
            ListString data = new ListString { userName };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventUserLoginFailed) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// User login succeded.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public virtual void UserLoginSucceded(string userName)
        {
            Assert.ArgumentNotNull(userName, "userName");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventUserLoginSucceded);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventUserLoginSucceded;
            }

            string text = description.FormatWith(new
            {
                Username = userName,
            });
            ListString data = new ListString { userName };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventUserLoginSucceded) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Account creation failed.
        /// </summary>
        public virtual void AccountCreationFailed()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventAccountCreationFailed);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventAccountCreationFailed, description);
            }
        }

        /// <summary>
        /// Account creation succeded.
        /// </summary>
        public virtual void AccountCreated()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventAccountCreated);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventAccountCreated, description);
            }
        }
    }
}