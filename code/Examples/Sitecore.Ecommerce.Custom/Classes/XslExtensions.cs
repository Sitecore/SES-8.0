// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XslExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   XslMehtodes for accesing eCommerce API from xslt.
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

namespace Sitecore.Ecommerce.Classes
{
  using System;
  using System.Text;
  using System.Xml;
  using System.Xml.Linq;
  using System.Xml.XPath;
  using Configuration;
  using DomainModel.Configurations;
  using Links;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// XslMehtodes for accesing eCommerce API from xslt.
  /// </summary>
  public class XslExtensions : Xsl.XslExtensions
  {
    #region Fields

    /// <summary>
    /// The option item.
    /// </summary>
    private string optionItem = @"<li><table width=""100%""><tr><td>{0}</td><td width=""1px"">{1}</td></tr></table></li>";

    #endregion

    #region Public properties

    /// <summary>
    /// Gets or sets option item.
    /// </summary>
    public string OptionItem
    {
      get
      {
        return this.optionItem;
      }

      set
      {
        this.optionItem = value;
      }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Gets the avalible variants XML.
    /// </summary>
    /// <param name="fieldList">
    /// The field list.
    /// </param>
    /// <param name="ni">
    /// The node iterator.
    /// </param>
    /// <returns>
    /// returns avalible
    /// </returns>
    public XmlDocument GetAvalibleVariantsXml(string fieldList, XPathNodeIterator ni)
    {
      Item itm = this.GetItem(ni);
      string[] fields = fieldList.Split(',');
      XElement doc = new XElement("root");

      foreach (Item child in itm.Parent.Children)
      {
        string value;
        string fieldName;
        if (IsValidVariant(child, itm, fields, out value, out fieldName))
        {
          XElement elem = new XElement("variant");
          elem.SetAttributeValue("value", LinkManager.GetItemUrl(child));
          elem.SetAttributeValue("text", value);
          elem.SetAttributeValue("field", fieldName);
          doc.Add(elem);
        }
        else if (child.ID == itm.ID)
        {
          foreach (string field in fields)
          {
            XElement elem = new XElement("variant");
            elem.SetAttributeValue("value", LinkManager.GetItemUrl(child));
            elem.SetAttributeValue("text", child[field]);
            elem.SetAttributeValue("field", field);
            elem.SetAttributeValue("selected", true);
            doc.Add(elem);
          }
        }
      }

      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(doc.ToString());
      return xmlDoc;
    }

    /// <summary>
    /// The get children str.
    /// </summary>
    /// <param name="rootItemId">
    /// Root item ID
    /// </param>
    /// <returns>
    /// returns children str.
    /// </returns>
    public string GetChildrenStr(string rootItemId)
    {
      return NicamHelper.GetChildrenStr(rootItemId);

    }

    /// <summary>
    /// The get random guid.
    /// </summary>
    /// <returns>
    /// returns random guid.
    /// </returns>
    public string GetRandomGUID()
    {
      return Guid.NewGuid().ToString();
    }

    // public string GetVisibleSpotsStr(string rootItemId)
    // {
    // return NicamHelper.GetImageSpotStr(rootItemId, true, false);
    // }

    // public string GetIndividualSpotsStr(string rootItemId)
    // {
    // return NicamHelper.GetImageSpotStr(rootItemId, true, true);
    // }

    /// <summary>
    /// The get spot links root.
    /// </summary>
    /// <returns>
    /// returns spot links root.
    /// </returns>
    public string GetSpotLinksRoot()
    {
      return Consts.SpotLinksRoot;
    }

    /// <summary>
    /// Gets the variant or product item
    /// </summary>
    /// <param name="field">
    /// The field.
    /// </param>
    /// <param name="ni">
    /// The node iterator.
    /// </param>
    /// <returns>
    /// returns variant or product
    /// </returns>
    public XPathNavigator GetVariantOrProduct(string field, XPathNodeIterator ni)
    {
      Item itm = this.GetItem(ni);
      string value = itm[field];
      if (string.IsNullOrEmpty(value)
          && itm.TemplateName.ToLower().Contains("variant"))
      {
        itm = itm.Parent;
      }

      return Factory.CreateItemNavigator(itm);
    }

    /// <summary>
    /// The poll options.
    /// </summary>
    /// <param name="pollId">
    /// id of poll
    /// </param>
    /// <returns>
    /// returns poll options.
    /// </returns>
    public string PollOptions(string pollId)
    {
      StringBuilder str = new StringBuilder();

      ID pollItmId = new ID(pollId);
      Item pollItm = Sitecore.Context.Database.GetItem(pollItmId);

      if (pollItm != null)
      {
        int i = 1;
        foreach (Item itm in pollItm.Children)
        {
          str.Append("<li>");
          str.Append(string.Concat("<input id=\"poll_option_", i.ToString(), "\" type=\"radio\" value=\"", itm.ID.ToString(), "\" name=\"poll_option\"/ onclick=\"document.forms[0].submit();return false;\">"));
          str.Append(string.Concat("<label for=\"poll_option_", i.ToString(), "\">", itm.Fields["Text"].Value, "</label>"));
          str.Append("</li>");
          i++;
        }
      }

      return str.ToString();
    }

    /// <summary>
    /// The poll options result.
    /// </summary>
    /// <param name="pollId">
    /// id of poll
    /// </param>
    /// <returns>
    /// returns options result.
    /// </returns>
    public string PollOptionsResult(string pollId)
    {
      StringBuilder str = new StringBuilder();

      ID pollItmId = new ID(pollId);
      Item pollItm = Sitecore.Context.Database.GetItem(pollItmId);

      if (pollItm != null)
      {
        double totalCount = 0;
        int optionValue;

        foreach (Item itm in pollItm.Children)
        {
          if (!int.TryParse(itm.Fields["count"].Value, out optionValue))
          {
            optionValue = 0;
          }

          totalCount += optionValue;
        }

        foreach (Item itm in pollItm.Children)
        {
          if (!int.TryParse(itm.Fields["count"].Value, out optionValue))
          {
            optionValue = 0;
          }

          double d = (optionValue / totalCount) * 100;
          string value = string.Concat(Math.Round(d).ToString(), "%");
          str.Append(string.Format(this.optionItem, itm.Fields["Text"].Value, value));
        }

        str.Append("<hr/>");
        str.Append(string.Format(this.optionItem, "Total votes", totalCount));
      }

      return str.ToString();
    }

    /// <summary>
    /// The rate page.
    /// </summary>
    /// <param name="id">
    /// id of page
    /// </param>
    /// <returns>
    /// returns page rate.
    /// </returns>
    public string RatePage(string id)
    {
      string rateString = NicamHelper.RatePage(id).ToString();
      return string.Format("<span class=\"rating score{0}\"></span>", rateString);
    }

    /// <summary>
    /// The rss news feed item path.
    /// </summary>
    /// <returns>
    /// returns path to rss news item.
    /// </returns>
    public string RssNewsFeedItemPath()
    {
      return Utils.ItemUtil.GetItemUrl(Context.Entity.GetConfiguration<GeneralSettings>().RSSNewsFeedLink, true);
    }

    /// <summary>
    /// The specifications.
    /// </summary>
    /// <param name="id">
    /// id of item
    /// </param>
    /// <returns>
    /// returns specifications.
    /// </returns>
    public string Specifications(string id)
    {
      StringBuilder str = new StringBuilder();
      TemplateFieldItem[] templateFieldsItem = NicamHelper.GetSectionFields(id, "Specification");

      if (templateFieldsItem != null)
      {
        foreach (TemplateFieldItem fld in templateFieldsItem)
        {
          str.Append(string.Concat(fld.Name, "|"));
        }
      }

      return str.ToString();
    }

    /// <summary>
    /// Get items from an id list from variant or product
    /// </summary>
    /// <param name="field">
    /// The field.
    /// </param>
    /// <param name="ni">
    /// The node iterator.
    /// </param>
    /// <returns>
    /// returns product items
    /// </returns>
    public XPathNodeIterator VariantOrProductItems(string field, XPathNodeIterator ni)
    {
      Item itm = this.GetItem(ni);
      string idlist = itm[field];

      if (string.IsNullOrEmpty(idlist)
          && itm.TemplateName.ToLower().Contains("variant"))
      {
        idlist = itm.Parent[field];
      }

      return this.Items(idlist);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Determines whether [is valid variant] [the specified child].
    /// </summary>
    /// <param name="child">
    /// The child.
    /// </param>
    /// <param name="item">
    /// The item to proceed.
    /// </param>
    /// <param name="fields">
    /// The strings.
    /// </param>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <param name="fieldName">
    /// field name
    /// </param>
    /// <returns>
    /// <c>true</c> if [is valid variant] [the specified child]; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsValidVariant(Item child, Item item, string[] fields, out string value, out string fieldName)
    {
      int fieldCount = fields.Length;
      int fieldMatch = 0;
      string fldName = string.Empty;
      string fldValue = string.Empty;

      foreach (string field in fields)
      {
        if (child[field] == item[field])
        {
          fieldMatch += 1;
        }
        else
        {
          fldName = field;
          fldValue = child[field];
        }
      }

      value = fldValue;
      fieldName = fldName;
      return fieldCount == fieldMatch + 1;
    }

    #endregion
  }
}