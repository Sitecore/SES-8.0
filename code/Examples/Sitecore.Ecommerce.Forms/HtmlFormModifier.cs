// -------------------------------------------------------------------------------------------
// <copyright file="HtmlFormModifier.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Forms
{
  using System;
  using System.IO;
  using System.Linq;
  using System.Xml.Linq;
  using HtmlAgilityPack;
  using Sitecore.Data.Items;
  using Sitecore.Form.Core.Pipelines.RenderForm;

  /// <summary>
  /// Html form modifier.
  /// </summary>
  public class HtmlFormModifier
  {
    /// <summary>
    /// The arguments.
    /// </summary>
    private readonly RenderFormArgs args;

    /// <summary>
    /// Xml document first part.
    /// </summary>
    private readonly XDocument firstPart;
    
    /// <summary>
    /// The form item
    /// </summary>
    private readonly Item formItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlFormModifier"/> class.
    /// </summary>
    /// <param name="args">The render form arguments.</param>
    public HtmlFormModifier(RenderFormArgs args)
    {
      this.formItem = args.Item;
      this.args = args;

      var firstPartDoc = new HtmlDocument();
      firstPartDoc.LoadHtml(args.Result.FirstPart);
      this.firstPart = ToXDocument(firstPartDoc);
    }

    /// <summary>
    /// Sets the text area value.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool SetTextAreaValue(string fieldName, string value)
    {
      var e = this.GetInputElement(fieldName);
      if (e == null || value == null)
      {
        return false;
      }

      e.Value = value;
      this.args.Result.FirstPart = this.GetResultFirstPart();
      return true;
    }

    /// <summary>
    /// Sets the input value.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool SetInputValue(string fieldName, string value)
    {
      var input = this.GetInputElement(fieldName);
      if (input == null)
      {
        return false;
      }

      input.SetAttributeValue("value", value);
      this.args.Result.FirstPart = this.GetResultFirstPart();
      return true;
    }

    /// <summary>
    /// Sets the checkbox selected.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">if set to <c>true</c> [value].</param>
    /// <returns></returns>
    public bool SetCheckboxSelected(string fieldName, bool value)
    {
      var input = this.GetInputElement(fieldName);
      if (input == null)
      {
        return false;
      }

      SetChecked(input, value);
      this.args.Result.FirstPart = this.GetResultFirstPart();
      return true;
    }

    /// <summary>
    /// Sets the selected drop list value.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool SetSelectedDropListValue(string fieldName, string value)
    {
      var selectList = this.GetInputElement(fieldName);
      if (selectList == null)
      {
        return false;
      }

      this.SetSelectedDropListValue(selectList, value);
      return true;
    }

    /// <summary>
    /// Sets the selected list box values.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public bool SetSelectedListBoxValues(string fieldName, params string[] values)
    {
      var selectList = this.GetInputElement(fieldName);
      if (selectList == null)
      {
        return false;
      }

      foreach (var option in selectList.Descendants())
      {
        var optValue = GetValueAttribute(option);
        if (optValue != null && values.Contains(optValue))
        {
          SetSelected(option, true);
        }
        else
        {
          SetSelected(option, false);
        }
      }

      this.args.Result.FirstPart = this.GetResultFirstPart();
      return true;
    }

    /// <summary>
    /// Sets the selected radio value.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool SetSelectedRadioValue(string fieldName, string value)
    {
      var table = this.GetInputElement(fieldName);
      if (table == null)
      {
        return false;
      }

      var radios = from t in table.Descendants()
                   where t.Attribute("type") != null
                         && t.Attribute("type").Value == "radio"
                   select t;

      foreach (var radio in radios)
      {
        var val = GetValueAttribute(radio);
        if (val != null && val == value)
        {
          SetChecked(radio, true);
        }
        else
        {
          SetChecked(radio, false);
        }
      }

      this.args.Result.FirstPart = this.GetResultFirstPart();
      return true;
    }

    /// <summary>
    /// Sets the selected check box list values.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public bool SetSelectedCheckBoxListValues(string fieldName, params string[] values)
    {
      var table = this.GetInputElement(fieldName);
      if (table == null)
      {
        return false;
      }

      var checkboxes = from t in table.Descendants()
                       where t.Attribute("type") != null
                             && t.Attribute("type").Value == "checkbox"
                       select t;

      foreach (var checkbox in checkboxes)
      {
        if (checkbox.Parent == null)
        {
          SetChecked(checkbox, false);
          continue;
        }

        var val = (from c in checkbox.Parent.Descendants()
                   where c.Name.LocalName == "label"
                   select c.Value).FirstOrDefault();

        if (val != null && values.Contains(val))
        {
          SetChecked(checkbox, true);
        }
        else
        {
          SetChecked(checkbox, false);
        }
      }

      this.args.Result.FirstPart = this.GetResultFirstPart();
      return true;
    }

    /// <summary>
    /// Hides the section.
    /// </summary>
    /// <param name="sectionName">Name of the section.</param>
    /// <returns></returns>
    public bool HideSection(string sectionName)
    {
      var sectionItem = this.formItem.Axes.GetDescendant(sectionName);
      if (sectionItem == null)
      {
        return false;
      }

      var sectionDiv = (from f in this.firstPart.Descendants()
                        where f.Name.LocalName == "div"
                              && f.Attribute("class") != null
                              && f.Attribute("class").Value == "scfLegendAsDiv"
                              && f.Value.Contains(sectionItem["Title"])
                        let xElement = f.Parent
                        where xElement != null
                        select xElement.Parent).FirstOrDefault();

      if (sectionDiv == null)
      {
        return false;
      }

      sectionDiv.SetAttributeValue("style", "display:none");
      var inputFields = from s in sectionDiv.Descendants()
                        where s.Attribute("id") != null
                              && s.Attribute("id").Value.EndsWith("_ecstate")
                        select s;

      foreach (var inputField in inputFields)
      {
        inputField.SetAttributeValue("value", "disabled");
      }

      this.args.Result.FirstPart = this.GetResultFirstPart();
      return true;
    }

    /// <summary>
    /// Hides the element.
    /// </summary>
    /// <param name="tagName">Name of the tag.</param>
    /// <param name="attribute">The attribute.</param>
    /// <param name="attributeValue">The attribute value.</param>
    public void HideElement(string tagName, string attribute, string attributeValue)
    {
      var elem = this.GetElement(tagName, attribute, attributeValue);
      if (elem == null)
      {
        return;
      }

      elem.SetAttributeValue("style", "display:none");
      this.args.Result.FirstPart = this.GetResultFirstPart();
    }

    /// <summary>
    /// Hides the section by field.
    /// </summary>
    /// <param name="sectionName">Name of the section.</param>
    /// <param name="sectionFieldName">Name of the section field.</param>
    public void HideSectionByField(string sectionName, string sectionFieldName)
    {
      var sectionItem = this.formItem.Axes.GetDescendant(sectionName);
      if (sectionItem == null)
      {
        return;
      }

      var fieldItem = sectionItem.Axes.GetDescendant(sectionFieldName);
      if (fieldItem == null)
      {
        return;
      }

      var fieldId = fieldItem.ID.ToString().Replace("{", string.Empty).Replace("}", string.Empty);
      var sectionDiv = (from f in this.firstPart.Descendants()
                        where f.Name.LocalName == "div"
                              && f.Attribute("class") != null
                              && f.Attribute("class").Value.Contains(fieldId)
                        let xElement = f.Parent
                        where xElement != null
                        let element = xElement.Parent
                        where element != null
                        select element.Parent).FirstOrDefault();
      if (sectionDiv == null)
      {
        return;
      }

      sectionDiv.SetAttributeValue("style", "display:none;");

      DisableFields(sectionDiv);
      this.args.Result.FirstPart = this.GetResultFirstPart();
    }

    /// <summary>
    /// Hides the field.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns></returns>
    public bool HideField(string fieldName)
    {
      var field = this.formItem.Axes.GetDescendant(fieldName);
      if (field == null)
      {
        return false;
      }

      var fieldDiv = (from f in this.firstPart.Descendants()
                      where f.Name.LocalName == "div"
                            && f.Attribute("fieldid") != null
                            && f.Attribute("fieldid").Value == field.ID.ToString()
                      select f).FirstOrDefault();
      if (fieldDiv == null)
      {
        return false;
      }

      fieldDiv.SetAttributeValue("style", "display:none");
      var inputFields = from s in fieldDiv.Descendants()
                        where s.Attribute("id") != null
                              && s.Attribute("id").Value.EndsWith("_ecstate")
                        select s;
      foreach (var inputField in inputFields)
      {
        inputField.SetAttributeValue("value", "disabled");
      }

      this.args.Result.FirstPart = this.GetResultFirstPart();
      return true;
    }

    /// <summary>
    /// Hides the form and leaves the validation summary (removing validation summary will cause an script error)
    /// </summary>
    public void HideForm()
    {
      var rootDiv = (from f in this.firstPart.Descendants()
                          where f.Attribute("class") != null
                                && f.Attribute("class").Value == "scfForm"
                          select f).FirstOrDefault();
      if (rootDiv != null)
      {
        rootDiv.SetAttributeValue("style", "display:none;");
      }

      this.args.Result.FirstPart = this.GetResultFirstPart();
    }

    /// <summary>
    /// Adds a button from navigation link.
    /// </summary>
    /// <param name="navigationLinkItem">The navigation link item.</param>
    /// <param name="addFirst">if set to <c>true</c> [add first].</param>
    /// <returns></returns>
    public bool AddButtonFromNavigationLink(Item navigationLinkItem, bool addFirst)
    {
      var submitDiv = (from f in this.firstPart.Descendants()
                            where f.Attribute("type") != null
                                  && f.Attribute("type").Value == "submit"
                            select f.Parent).FirstOrDefault();
      if (submitDiv == null)
      {
        return false;
      }

      var title = Utils.ItemUtil.GetNavigationLinkTitle(navigationLinkItem);
      var path = Utils.ItemUtil.GetNavigationLinkPath(navigationLinkItem);

      var btn = new XElement("input");
      btn.SetAttributeValue("type", "submit");
      btn.SetAttributeValue("value", title);
      btn.SetAttributeValue("onclick", "javascript:location.href='" + path + "';return false;");

      if (addFirst)
      {
        submitDiv.AddFirst(btn);
      }
      else
      {
        submitDiv.Add(btn);
      }

      this.args.Result.FirstPart = this.GetResultFirstPart();
      return true;
    }

    /// <summary>
    /// Changes the form introduction.
    /// </summary>
    /// <param name="value">The new introduction value.</param>
    public void ChangeFormIntroduction(string value)
    {
      XElement formInfo = this.GetElement("div", "class", "scfIntroBorder");
      if (formInfo != null)
      {
        XElement info = formInfo.Descendants("span").FirstOrDefault();
        if (info != null)
        {
          info.Value = value;
          this.args.Result.FirstPart = this.GetResultFirstPart();
        }
      }
    }

    /// <summary>
    /// Changes the form title.
    /// </summary>
    /// <param name="value">The value.</param>
    public void ChangeFormTitle(string value)
    {
      var formTitle = GetElementByLasIdPart(this.firstPart.Root, "_title");
      if (formTitle == null)
      {
        return;
      }

      formTitle.Value = value;
      this.args.Result.FirstPart = this.GetResultFirstPart();
    }

    /// <summary>
    /// Surround the content with unordered list.
    /// </summary>
    /// <param name="className">Name of the class.</param>
    public void SurroundContentWithUlLi(string className)
    {
      var tag = (from f in this.firstPart.Descendants()
                      where f.Attribute("class") != null &&
                            f.Attribute("class").Value == className
                      select f).FirstOrDefault();
      if (tag == null)
      {
        return;
      }

      var li = new XElement("li");
      var ul = new XElement("ul", li);
      li.Value = tag.Value;
      tag.ReplaceNodes(ul);
      this.args.Result.FirstPart = this.GetResultFirstPart();
    }
    
    /// <summary>
    /// Removes the NBSP.
    /// </summary>
    public void RemoveNbsp()
    {
      this.RemoveFromChild(this.firstPart.Root);
      this.args.Result.FirstPart = this.GetResultFirstPart();
    }

    /// <summary>
    /// Disables the input field.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    public void DisableInputField(string fieldName)
    {
      var input = this.GetInputElement(fieldName);
      if (input == null)
      {
        return;
      }

      input.SetAttributeValue("disabled", "disabled");
      this.args.Result.FirstPart = this.GetResultFirstPart();
    }

    /// <summary>
    /// Sets the selected date.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    internal bool SetSelectedDate(string fieldName, DateTime value)
    {
      var field = this.formItem.Axes.GetDescendant(fieldName);
      var identifier = field.ID.ToString().Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty);

      var div = this.GetInputElement(fieldName);
      if (div == null)
      {
        return false;
      }

      var dayElem = GetElementByLasIdPart(div, identifier + "_day");
      if (dayElem != null)
      {
        this.SetSelectedDropListValue(dayElem, value.Day.ToString());
      }

      var monthElem = GetElementByLasIdPart(div, identifier + "_month");
      if (monthElem != null)
      {
        this.SetSelectedDropListValue(monthElem, value.Month.ToString());
      }

      var yearElem = GetElementByLasIdPart(div, identifier + "_year");
      if (yearElem != null)
      {
        this.SetSelectedDropListValue(yearElem, value.Year.ToString());
      }

      this.args.Result.FirstPart = this.GetResultFirstPart();
      return true;
    }

    /// <summary>
    /// Adds a button from navigation link.
    /// </summary>
    /// <param name="navigationLinkItem">The navigation link item.</param>
    /// <returns></returns>
    internal bool AddButtonFromNavigationLink(Item navigationLinkItem)
    {
      return this.AddButtonFromNavigationLink(navigationLinkItem, false);
    }

    /// <summary>
    /// Replaces all legend with div tag.
    /// </summary>
    internal void ReplaceLegendWithDiv()
    {
      var legends = from f in this.firstPart.Descendants()
                                      where f.Name.LocalName == "legend"
                                      select f;

      var legendsElements = legends as XElement[] ?? legends.ToArray();

      foreach (var legend in legendsElements)
      {
        var div = new XElement("div");
        div.SetAttributeValue("class", "scfLegendAsDiv");
        div.Value = legend.Value;
        legend.AddBeforeSelf(div);
      }

      legendsElements.Remove();

      this.args.Result.FirstPart = this.GetResultFirstPart();
    }

    /// <summary>
    /// Removes the empty tags.
    /// </summary>
    /// <param name="tagName">Name of the tag.</param>
    /// <param name="className">Name of the class.</param>
    internal void RemoveEmptyTags(string tagName, string className)
    {
      (from f in this.firstPart.Descendants()
       where f.Name.LocalName == tagName
             && f.Attribute("class") != null
             && f.Attribute("class").Value == className
             && string.IsNullOrEmpty(f.Value)
       select f).Remove();

      this.args.Result.FirstPart = this.GetResultFirstPart();
    }

    /// <summary>
    /// Gets the attribute.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The value of the attribute</returns>
    private static string GetValueAttribute(XElement element)
    {
      const string AttrName = "value";
      return element.Attribute(AttrName) != null ? element.Attribute(AttrName).Value : null;
    }

    /// <summary>
    /// Toes the X document.
    /// </summary>
    /// <param name="document">The document.</param>
    /// <returns>The X document.</returns>
    private static XDocument ToXDocument(HtmlDocument document)
    {
      using (var sw = new StringWriter())
      {
        document.OptionOutputAsXml = true;
        document.Save(sw);
        return XDocument.Parse(sw.GetStringBuilder().ToString());
      }
    }

    /// <summary>
    /// Sets the checked.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="value">if set to <c>true</c> [value].</param>
    private static void SetChecked(XElement input, bool value)
    {
      if (value)
      {
        input.SetAttributeValue("checked", "checked");
      }
      else
      {
        if (input.Attribute("checked") != null)
        {
          input.Attribute("checked").Remove();
        }
      }
    }

    /// <summary>
    /// Sets the selected.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <param name="value">if set to <c>true</c> [value].</param>
    private static void SetSelected(XElement option, bool value)
    {
      if (value)
      {
        option.SetAttributeValue("selected", "selected");
      }
      else
      {
        if (option.Attribute("selected") != null)
        {
          option.Attribute("selected").Remove();
        }
      }
    }

    /// <summary>
    /// Gets the element by las id part.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <param name="endsWith">The ends with.</param>
    /// <returns>The element</returns>
    private static XElement GetElementByLasIdPart(XElement root, string endsWith)
    {
      return (from el in root.Descendants()
              where el.Attribute("id") != null &&
                    el.Attribute("id").Value.EndsWith(endsWith)
              select el).FirstOrDefault();
    }

    /// <summary>
    /// Disables the fields.
    /// </summary>
    /// <param name="sectionDiv">The section div.</param>
    private static void DisableFields(XContainer sectionDiv)
    {
      var inputFields = from s in sectionDiv.Descendants()
                        where s.Attribute("id") != null
                              && s.Attribute("id").Value.EndsWith("_ecstate")
                        select s;

      foreach (var inputField in inputFields)
      {
        inputField.SetAttributeValue("value", "disabled");
      }
    }

    /// <summary>
    /// Gets the result first part.
    /// </summary>
    /// <returns>The first result part.</returns>
    private string GetResultFirstPart()
    {
      return this.firstPart.ToString();
    }

    /// <summary>
    /// Sets the selected drop list value.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="value">The value.</param>
    private void SetSelectedDropListValue(XContainer element, string value)
    {
      foreach (var option in element.Descendants())
      {
        var optValue = GetValueAttribute(option);
        if (optValue != null && optValue == value)
        {
          SetSelected(option, true);
        }
        else
        {
          SetSelected(option, false);
        }
      }

      this.args.Result.FirstPart = this.GetResultFirstPart();
    }

    /// <summary>
    /// Gets the input control.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns>the input element</returns>
    private XElement GetInputElement(string fieldName)
    {
      var field = this.formItem.Axes.GetDescendant(fieldName);
      if (field == null)
      {
        return null;
      }

      var identifier = field.ID.ToString().Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty);
      var e = (from el in this.firstPart.Descendants()
                    where el.Attribute("id") != null &&
                          el.Attribute("id").Value.EndsWith(identifier)
                    select el).FirstOrDefault();
      return e;
    }

    /// <summary>
    /// Gets the element.
    /// </summary>
    /// <param name="tagName">Name of the tag.</param>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <param name="attributeValue">The attribute value.</param>
    /// <returns>returns first matched element by element tag and attribute</returns>
    private XElement GetElement(string tagName, string attributeName, string attributeValue)
    {
      return (from el in this.firstPart.Descendants()
                    where el.Name.LocalName == tagName &&
                          el.Attribute(attributeName) != null &&
                          el.Attribute(attributeName).Value.Contains(attributeValue)
                    select el).FirstOrDefault();
    }

    /// <summary>
    /// Removes from child.
    /// </summary>
    /// <param name="root">The root.</param>
    private void RemoveFromChild(XElement root)
    {
      foreach (var elem in root.Descendants())
      {
        (from f in elem.Nodes()
         where f.NodeType == System.Xml.XmlNodeType.Text &&
         f.ToString().Contains("&amp;nbsp")
         select f).Remove();

        if (elem.HasElements)
        {
          this.RemoveFromChild(elem);
        }
      }
    }
  }
}