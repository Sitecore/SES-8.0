// -------------------------------------------------------------------------------------------
// <copyright file="CheckoutEvents.cs" company="Sitecore Corporation">
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
    /// The Checkouts analitics extensions.
    /// </summary>
    public class CheckoutEvents
    {
        /// <summary>
        /// Go to check out.
        /// </summary>
        public virtual void GoToCheckOut()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventGoToCheckout);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventGoToCheckout, description);
            }
        }

        /// <summary>
        /// Checkouts the delivery next.
        /// </summary>
        /// <param name="deliveryAlternativeOption">The delivery alternative option.</param>
        /// <param name="notificationOption">The notification option.</param>
        /// <param name="notificationText">The notification text.</param>
        public virtual void DeliveryNext(string deliveryAlternativeOption, string notificationOption, string notificationText)
        {
            Assert.ArgumentNotNull(deliveryAlternativeOption, "deliveryAlternativeOption");
            Assert.ArgumentNotNull(notificationOption, "notificationOption");
            Assert.ArgumentNotNull(notificationText, "notificationText");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventCheckoutDeliveryNext);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventCheckoutDeliveryNext;
            }

            string text = description.FormatWith(new { DeliveryAlternativeOption = deliveryAlternativeOption, NotificationOption = notificationOption, NotificationText = notificationText });
            ListString data = new ListString { deliveryAlternativeOption, notificationOption, notificationText };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventCheckoutDeliveryNext) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Checkout delivery alternative changed.
        /// </summary>
        /// <param name="deliveryAlternativeOption">The delivery alternative option.</param>
        public virtual void DeliveryOptionSelected(string deliveryAlternativeOption)
        {
            Assert.ArgumentNotNull(deliveryAlternativeOption, "deliveryAlternativeOption");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventCheckoutDeliveryOptionSelected);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventCheckoutDeliveryOptionSelected;
            }

            string text = description.FormatWith(new { Option = deliveryAlternativeOption, });
            ListString data = new ListString { deliveryAlternativeOption };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventCheckoutDeliveryOptionSelected) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Checkout delivery alternative changed.
        /// </summary>
        /// <param name="optionTitle">The option title.</param>
        /// <param name="optionCode">The option code.</param>
        public virtual void PaymentMethodSelected(string optionTitle, string optionCode)
        {
            Assert.ArgumentNotNull(optionTitle, "optionTitle");
            Assert.ArgumentNotNull(optionCode, "optionCode");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventCheckoutPaymentMethodSelected);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventCheckoutPaymentMethodSelected;
            }

            string text = description.FormatWith(new { Option = optionTitle, });
            ListString data = new ListString { optionCode, optionTitle };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventCheckoutPaymentMethodSelected) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Next to the next page.
        /// </summary>
        public virtual void Next()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventCheckoutNext);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventCheckoutNext, description);
            }
        }

        /// <summary>
        /// Next to the next page.
        /// </summary>
        public virtual void PaymentNext()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventCheckoutPaymentNext);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventCheckoutPaymentNext, description);
            }
        }

        /// <summary>
        /// Notification option selected.
        /// </summary>
        /// <param name="notificationOption">The notification option.</param>
        public virtual void NotificationOptionSelected(string notificationOption)
        {
            Assert.ArgumentNotNull(notificationOption, "notificationOption");

            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventCheckoutNotificationOptionSelected);
            if (string.IsNullOrEmpty(description))
            {
                description = EventConstants.EventCheckoutNotificationOptionSelected;
            }

            string text = description.FormatWith(new { Option = notificationOption, });
            ListString data = new ListString { notificationOption };

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage == null)
            {
                return;
            }

            var pageEventData = new PageEventData(EventConstants.EventCheckoutNotificationOptionSelected) { Text = text, Data = data.ToString() };
            currentPage.Register(pageEventData);
        }

        /// <summary>
        /// Previous page.
        /// </summary>
        public virtual void Previous()
        {
            if (!Tracker.IsActive)
            {
                return;
            }

            AnalyticsHelper analyticsHelper = Context.Entity.Resolve<AnalyticsHelper>();
            Assert.IsNotNull(analyticsHelper, "analyticsHelper");

            string description = analyticsHelper.GetPageEventDescription(EventConstants.EventCheckoutPrevious);

            var currentPage = Tracker.Current.CurrentPage;
            if (currentPage != null)
            {
                currentPage.Register(EventConstants.EventCheckoutPrevious, description);
            }
        }
    }
}