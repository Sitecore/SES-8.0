// -------------------------------------------------------------------------------------------
// <copyright file="SectionFactory.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Sections
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Data;
  using DomainModel.Configurations;
  using DomainModel.Orders;
  using Sitecore.Collections;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Unity;
  using Utils;

  /// <summary>
  /// The section factory class.
  /// </summary>
  public static class SectionFactory
  {
    /// <summary>
    /// Gets the section.
    /// </summary>
    /// <param name="sectionItem">The section item.</param>
    /// <param name="entity">The data entity.</param>
    /// <returns>the section.</returns>
    public static List<SectionTableRow> GetSection(Item sectionItem, object entity)
    {
      if (sectionItem != null)
      {
        ChildList childList = sectionItem.Children;
        var tableRows = new List<SectionTableRow>();

        foreach (Item sectionFieldItem in childList)
        {
          var sectionTableRow = new SectionTableRow();

          string title = Utils.ItemUtil.GetTitleOrDictionaryEntry(sectionFieldItem, false);

          sectionTableRow.Label = title;

          string fieldID = sectionFieldItem["Field"];
          string fieldName = title;
          string value = fieldID;
          object obj = new object();

          if (!sectionFieldItem.TemplateName.Equals("Footer Section Multi-Line Text Field") &&
              !sectionFieldItem.TemplateName.Equals("Footer Section Text Field") &&
              !sectionFieldItem.TemplateName.Equals("Footer Section Link Field"))
          {
            fieldName = GetTemplateFieldName(fieldID);
            obj = GetSourceObject(fieldID, entity);
            value = GetPropertyValue(fieldName, obj);
          }

          sectionTableRow.FieldName = fieldName;
          sectionTableRow.ShowLabelColumn = sectionFieldItem.Parent["Show Label Column"] == "1";
          sectionTableRow.HideField = sectionFieldItem["Hide Field"] == "1";
          sectionTableRow.ShowLabel = sectionFieldItem["Show Label"] == "1";

          if (sectionFieldItem.TemplateName.Equals("Order Section Field") ||
              sectionFieldItem.TemplateName.Equals("Footer Section Field") ||
              sectionFieldItem.TemplateName.Equals("Footer Section Multi-Line Text Field") ||
              sectionFieldItem.TemplateName.Equals("Footer Section Text Field"))
          {
            if (sectionTableRow.ShowLabel && !sectionTableRow.ShowLabelColumn)
            {
              sectionTableRow.Value = title + ": " + value;
            }
            else
            {
              sectionTableRow.Value = value;
            }
          }
          else if (sectionFieldItem.TemplateName.Equals("Order Section Double Field") ||
                   sectionFieldItem.TemplateName.Equals("Footer Section Double Field"))
          {
            string fieldID2 = sectionFieldItem["Field2"];
            string fieldName2 = GetTemplateFieldName(fieldID2);
            string value2 = GetPropertyValue(fieldName2, obj);

            sectionTableRow.FieldName2 = fieldName2;
            if (sectionTableRow.ShowLabel && !sectionTableRow.ShowLabelColumn)
            {
              sectionTableRow.Value = title + ": " + value + ", " + value2;
            }
            else
            {
              sectionTableRow.Value = value + ", " + value2;
            }
          }
          else if (sectionFieldItem.TemplateName.Equals("Footer Section Link Field"))
          {
            if (sectionTableRow.ShowLabel && !sectionTableRow.ShowLabelColumn)
            {
              sectionTableRow.Value = title + " " + "<a href=\"" + value + ".aspx\">" + title + "</a>";
            }
            else
            {
              sectionTableRow.Value = "<a href=\"" + value + ".aspx\">" + title + "</a>";
            }
          }

          // To not add the row if HideField is enabled.
          if (!sectionTableRow.HideField)
          {
            tableRows.Add(sectionTableRow);
          }
        }

        return tableRows;
      }

      return
        null;
    }

    /// <summary>
    /// Gets the property value from the object by the selected fieldName
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="obj">The object.</param>
    /// <returns>The property value from the object by the selected fieldName</returns>
    private static string GetPropertyValue(string fieldName, object obj)
    {
      if (!string.IsNullOrEmpty(fieldName))
      {
        if (obj is Order)
        {
          IMappingRule<Order> rule = Context.Entity.Resolve<IMappingRule<Order>>("OrderMappingRule");
          rule.MappingObject = (Order)obj;
          obj = rule;
        }

        // If obj is of type Item, then retrieve fieldname.
        if (obj is Item)
        {
          var item = (Item)obj;
          return BusinessCatalogUtil.GetTitleFromReferencedItemOrValue(item[fieldName]);
        }

        EntityHelper entityHelper = Context.Entity.Resolve<EntityHelper>();
        IDictionary<string, object> fieldsCollection = entityHelper.GetPropertiesValues(obj);

        if (fieldsCollection.Count > 0)
        {
          string value = fieldsCollection.FirstOrDefault(p => p.Key.EndsWith(fieldName)).Value as string;
          if (DateUtil.IsIsoDate(value))
          {
            DateTime date = DateUtil.IsoDateToDateTime(value);
            return DateUtil.FormatShortDateTime(date, Sitecore.Context.Culture);
          }

          if (!string.IsNullOrEmpty(value) && ID.IsID(value))
          {
            Item valueItem = Sitecore.Context.Database.GetItem(value) ?? Sitecore.Context.ContentDatabase.GetItem(value);
            if (valueItem != null)
            {
              value = valueItem.Name;
            }
          }

          return value;
        }

        // Else retrieve from Property
        if (obj != null)
        {
          PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
          foreach (PropertyInfo info in propertyInfos)
          {
            if (info.Name.Equals(fieldName))
            {
              object o = info.GetValue(obj, null);
              string value = o + string.Empty;

              return BusinessCatalogUtil.GetTitleFromReferencedItemOrValue(value);
            }
          }
        }
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets the name of the template field.
    /// </summary>
    /// <param name="fieldId">The field id.</param>
    /// <returns>The name of the template field.</returns>
    private static string GetTemplateFieldName(string fieldId)
    {
      if (string.IsNullOrEmpty(fieldId))
      {
        return string.Empty;
      }

      if (!ID.IsID(fieldId))
      {
        return fieldId;
      }

      ID id = new ID(fieldId);

      Item item = Sitecore.Context.Database.GetItem(id);
      if (item != null)
      {
        var templateFieldItem = (TemplateFieldItem)item;
        string name = templateFieldItem.Name;
        return name;
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets the source object.
    /// </summary>
    /// <param name="fieldId">The field id.</param>
    /// <param name="orderEntity">The order entity.</param>
    /// <returns>The source object.</returns>
    private static object GetSourceObject(string fieldId, object orderEntity)
    {
      if (!string.IsNullOrEmpty(fieldId))
      {
        if (!ID.IsID(fieldId))
        {
          return orderEntity;
        }

        var id = new ID(fieldId);
        Item item = Sitecore.Context.Database.GetItem(id);
        if (item != null)
        {
          var templateFieldItem = (TemplateFieldItem)item;
          TemplateItem templateItem = templateFieldItem.Template;
          if (templateItem.Name.Equals("Order"))
          {
            return orderEntity;
          }

          if (templateItem.Name.Equals("Company Master Data"))
          {
            BusinessCatalogSettings businessCatalogSettings = Context.Entity.GetConfiguration<BusinessCatalogSettings>();
            return Sitecore.Context.Database.GetItem(businessCatalogSettings.CompanyMasterDataLink);
          }
        }
      }

      return null;
    }
  }
}