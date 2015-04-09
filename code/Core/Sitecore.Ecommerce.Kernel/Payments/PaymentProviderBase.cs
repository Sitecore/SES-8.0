// -------------------------------------------------------------------------------------------
// <copyright file="PaymentProviderBase.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Payments
{
  using System;
  using System.Web;
  using Diagnostics;
  using DomainModel.Configurations;
  using DomainModel.Payments;
  using Links;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Text;

  /// <summary>
  /// The payment provider base class.
  /// </summary>
  /// <typeparam name="T">The payment method type.</typeparam>
  [Obsolete("Use the PaymentProvider class")]
  public abstract class PaymentProviderBase<T> : OnlinePaymentProvider, IPaymentProvider<T> where T : DomainModel.Payments.PaymentSystem
  {
    #region Fields

    /// <summary>
    /// Payment succeded url.
    /// </summary>
    private string paymentSucceededUrl = "Payment Succeeded Url";

    /// <summary>
    /// The Payment failed url.
    /// </summary>
    private string paymentFailedUrl = "Payment Failed Url";

    /// <summary>
    /// The selected payment method.
    /// </summary>
    private T selectedPaymentSytem;

    #endregion

    /// <summary>
    /// Gets or sets the payment success page URL.
    /// </summary>
    /// <value>The payment success page URL.</value>
    [Obsolete("Use the SuccessPageUrl property of the PaymentArgs instance")]
    public virtual string SuccessPageUrl
    {
      get
      {
        return this.ExtractPaymentLinkUrl(this.paymentSucceededUrl);
      }

      set
      {
        this.paymentSucceededUrl = value;
      }
    }

    /// <summary>
    /// Gets or sets the payment failure page URL.
    /// </summary>
    /// <value>The payment failure page URL.</value>
    [Obsolete("Use the FailurePageUrl property of the PaymentArgs instance")]
    public virtual string FailurePageUrl
    {
      get
      {
        return this.ExtractPaymentLinkUrl(this.paymentFailedUrl);
      }

      set
      {
        this.paymentFailedUrl = value;
      }
    }

    /// <summary>
    /// Gets or sets the selected payment system.
    /// </summary>
    /// <value>The payment system.</value>
    [Obsolete("Use the additional paymentSystem parameter")]
    public virtual T PaymentSystem
    {
      get
      {
        return this.selectedPaymentSytem;
      }

      set
      {
        if (value != default(T))
        {
          this.selectedPaymentSytem = value;
        }
      }
    }

    /// <summary>
    /// Initializes the payment.
    /// </summary>
    [Obsolete("Use the InvokePayment(PaymentSystem, PaymentArgs) method")]
    public virtual void InvokePayment()
    {
    }

    /// <summary>
    /// Gets the payment link URL.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns>The payment link URL.</returns>
    [Obsolete("This functionality was moved to the PaymentUrlResolver class")]
    protected virtual string ExtractPaymentLinkUrl(string fieldName)
    {
      Assert.IsNotNullOrEmpty(fieldName, "fieldName");

      if (Sitecore.Web.WebUtil.IsExternalUrl(fieldName))
      {
        return fieldName;
      }

      BusinessCatalogSettings businessCatalogSettings = Context.Entity.GetConfiguration<BusinessCatalogSettings>();
      Item paymentsItem = Sitecore.Context.Database.GetItem(businessCatalogSettings.PaymentSystemsLink);
      Assert.IsNotNull(paymentsItem, "Payments item is null");
      Assert.IsNotNull(paymentsItem.Fields[fieldName], string.Concat("Payments item does not contains field: '", fieldName, "'"));

      LinkField linkField = paymentsItem.Fields[fieldName];
      Assert.IsNotNull(linkField, string.Concat("'", fieldName, "' is not a link field"));

      // If link is not internal just return url value.
      string externalUrl = linkField.Url;
      if (!linkField.LinkType.Equals("internal"))
      {
        return externalUrl;
      }

      // If the site doesn't contains Business Catalog/Payments item under itself, Payment pages urls are wrong.
      // E.g.: Site path is: '/sitecore/content/Examples/Home';
      // Business Catalog path is: '/sitecore/content/Examples/Business Catalog';
      // Payment item path is: '/sitecore/content/Examples/Business Catalog/Payments';
      // In this case return page url is: '/Examples/Home/checkout/paymentreturnpage.aspx';
      // But should be: '/checkout/paymentreturnpage.aspx';
      Item targetPageItem = Sitecore.Context.Database.GetItem(linkField.TargetID);
      Assert.IsNotNull(targetPageItem, string.Concat("'", fieldName, "' contains invalid target item"));

      UrlOptions urlOptions = new UrlOptions
                                {
                                  SiteResolving = true, ShortenUrls = true,
                                  AddAspxExtension = true, EncodeNames = true,
                                  LanguageEmbedding = LanguageEmbedding.AsNeeded
                                };
      string relativeTargetPageUrl = LinkManager.GetItemUrl(targetPageItem, urlOptions);

      Assert.IsNotNull(HttpContext.Current.Request, "Http request is null");
      UrlString url = new UrlString
      {
        HostName = HttpContext.Current.Request.Url.Host,
        Path = relativeTargetPageUrl
      };

      return url.ToString();
    }
  }
}