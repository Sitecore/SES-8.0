// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaymentUrlResolver.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   PaymentUrlResolver class
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

namespace Sitecore.Ecommerce.Payments
{
  using System.Web;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.DomainModel.Configurations;
  using Sitecore.Ecommerce.DomainModel.Payments;
  using Sitecore.Links;
  using Sitecore.Text;

  /// <summary>
  /// Resolves neccessary URLs for the payment providers
  /// </summary>
  public class PaymentUrlResolver
  {
    #region Fields

    /// <summary>
    /// Payment succeded url.
    /// </summary>
    private const string PaymentSucceededUrl = "Payment Succeeded Url";

    /// <summary>
    /// The Payment failed url.
    /// </summary>
    private const string PaymentFailedUrl = "Payment Failed Url";

    /// <summary>
    /// The payment return page url.
    /// </summary>
    private const string PaymentReturnPageUrl = "Payment Return Page Url";

    /// <summary>
    /// The payment cancel url.
    /// </summary>
    private const string PaymentCancelUrl = "Payment Cancel Url";

    #endregion

    /// <summary>
    ///  Resolves payment URLs
    /// </summary>
    /// <returns>
    /// Payment URLs
    /// </returns>
    public virtual PaymentUrls Resolve()
    {
      return new PaymentUrls
      {
        CancelPageUrl = this.ExtractPaymentLinkUrl(PaymentCancelUrl),
        FailurePageUrl = this.ExtractPaymentLinkUrl(PaymentFailedUrl),
        ReturnPageUrl = this.ExtractPaymentLinkUrl(PaymentReturnPageUrl),
        SuccessPageUrl = this.ExtractPaymentLinkUrl(PaymentSucceededUrl)
      };
    }

    /// <summary>
    /// Gets the payment link URL.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns>The payment link URL.</returns>
    protected string ExtractPaymentLinkUrl(string fieldName)
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
        SiteResolving = true,
        ShortenUrls = true,
        AddAspxExtension = true,
        EncodeNames = true,
        LanguageEmbedding = LanguageEmbedding.AsNeeded
      };
      string relativeTargetPageUrl = LinkManager.GetItemUrl(targetPageItem, urlOptions);

      Assert.IsNotNull(HttpContext.Current.Request, "Http request is null");

      // If non regular http ports are used with instances on different ports like in Azure local emulation, HttpContext.Current.Request.Url.Port contains incorrect port.
      // This can be removed if/when issue is fixed by Microsoft.
      string host;
      var port = HttpContext.Current.Request.Url.Port;
      if (port == 80 || port == 443)
      {
        host = HttpContext.Current.Request.Url.Host;
      }
      else
      {
        host = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
      }

      UrlString url = new UrlString
      {
        HostName = host,
        Path = relativeTargetPageUrl
      };

      return url.ToString();
    }
  }
}
