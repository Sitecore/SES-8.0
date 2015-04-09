// -------------------------------------------------------------------------------------------
// <copyright file="PaymentSettingsReader.cs" company="Sitecore Corporation">
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
  using System.Collections.Specialized;
  using System.Linq;
  using System.Xml.Linq;
  using Diagnostics;

  /// <summary>
  /// The configuration class.
  /// </summary>
  public class PaymentSettingsReader
  {
    /// <summary>
    /// The settings name.
    /// </summary>
    private static readonly string settingsName = "settings";

    /// <summary>
    /// The settings name.
    /// </summary>
    private static readonly string settingName = "setting";

    /// <summary>
    /// The currency name.
    /// </summary>
    private static readonly string currencyName = "currency";

    /// <summary>
    /// The transaction type name.
    /// </summary>
    private static readonly string transactionTypeName = "transactionType";

    /// <summary>
    /// The xnl document.
    /// </summary>
    private readonly XDocument xmlDocument;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentSettingsReader"/> class.
    /// </summary>
    /// <param name="paymentMethod">The payment method.</param>
    public PaymentSettingsReader(DomainModel.Payments.PaymentSystem paymentMethod) : this(paymentMethod.PaymentSettings)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentSettingsReader"/> class.
    /// </summary>
    /// <param name="xmlBody">The XML body.</param>
    public PaymentSettingsReader(string xmlBody)
    {
      Assert.ArgumentNotNull(xmlBody, "xmlBody");

      this.xmlDocument = XDocument.Parse(string.Format("<{0}>{1}</{0}>", settingsName, xmlBody));
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="elementName">Name of the element.</param>
    /// <returns>The value.</returns>
    public virtual string GetSetting(string elementName)
    {
      Assert.ArgumentNotNull(elementName, "elementName");

      return this.GetValue(settingName, elementName);
    }

    /// <summary>
    /// Gets the currency.
    /// </summary>
    /// <param name="elementName">Name of the element.</param>
    /// <returns>The currency</returns>
    public virtual string GetCurrency(string elementName)
    {
      Assert.ArgumentNotNull(elementName, "elementName");

      return this.GetValue(currencyName, elementName);
    }

    /// <summary>
    /// Returns the settings from the provider.
    /// </summary>
    /// <returns>
    /// NameValue collection of the settings.
    /// </returns>
    public virtual NameValueCollection GetProviderSettings()
    {
      return this.GetProviderSettings(new string[0]);
    }

    /// <summary>
    /// Returns the settings from the provider.
    /// </summary>
    /// <param name="ignoredValues">The ignored Values.</param>
    /// <returns>NameValue collection of the settings without custom values.</returns>
    public virtual NameValueCollection GetProviderSettings(string[] ignoredValues)
    {
      Assert.IsNotNull(this.xmlDocument, "XML document is null");
      Assert.IsNotNull(this.xmlDocument.Root, "XML document root is null");
      Assert.ArgumentNotNull(ignoredValues, "ignoredValues");

      NameValueCollection result = new NameValueCollection();
      XAttribute idAttribute = null;
      foreach (var entry in this.xmlDocument.Descendants()
        .Where(e => e.Name == settingName)
        .Where(entry => (idAttribute = entry.Attribute("id")) != null)
        .Where(entry => idAttribute.Value != transactionTypeName && !ignoredValues.Contains(idAttribute.Value)))
      {
        result.Add(idAttribute.Value, entry.Value);
      }

      return result;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="tagName">Name of the tag.</param>
    /// <param name="elementName">Name of the element.</param>
    /// <returns>The value.</returns>
    protected virtual string GetValue(string tagName, string elementName)
    {
      Assert.ArgumentNotNull(tagName, "tagName");
      Assert.ArgumentNotNull(elementName, "elementName");
      Assert.IsNotNull(this.xmlDocument, "XML document is null");
      Assert.IsNotNull(this.xmlDocument.Root, "XML document root is null");

      return (from e in this.xmlDocument.Descendants()
              where e.Name == tagName && e.Attributes().Any(a => a.Value == elementName)
              select e.Value).FirstOrDefault();
    }
  }
}