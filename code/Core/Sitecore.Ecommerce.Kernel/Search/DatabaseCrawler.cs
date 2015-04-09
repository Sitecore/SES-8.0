// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseCrawler.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the database crawler class.
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

namespace Sitecore.Ecommerce.Search
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using Configuration;
  using Diagnostics;
  using Globalization;
  using Lucene.Net.Documents;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Templates;
  using Sitecore.Search;
  using Sitecore.Search.Crawlers;
  using Sitecore.Search.Crawlers.FieldCrawlers;

  /// <summary>
  ///  Defines the database crawler class.
  /// </summary>
  public class DatabaseCrawler : Sitecore.Search.Crawlers.DatabaseCrawler, ICrawler
  {
    /// <summary>
    /// Adds an item to the index.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="context">The context.</param>
    protected override void AddItem(Item item, IndexUpdateContext context)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(context, "context");

      if (this.IsMatch(item))
      {
        foreach (Language language in item.Languages)
        {
          Item latestVersion = item.Database.GetItem(item.ID, language, Version.Latest);
          if (latestVersion != null)
          {
            foreach (Item version in latestVersion.Versions.GetVersions(false))
            {
              this.IndexVersion(version, latestVersion, context);
            }
          }
        }
      }
    }

    /// <summary>
    /// Adds all fields.
    /// </summary>
    /// <param name="document">The document.</param>
    /// <param name="item">The item to proceed</param>
    /// <param name="versionSpecific">if set to <c>true</c> this instance is version specific.</param>
    protected override void AddAllFields(Document document, Item item, bool versionSpecific)
    {
      item.Fields.ReadAll();

      Assert.ArgumentNotNull(document, "document");
      Assert.ArgumentNotNull(item, "item");
      foreach (Sitecore.Data.Fields.Field field in item.Fields)
      {
        if (!string.IsNullOrEmpty(field.Key) && !this.IsStandardField(field))
        {
          bool tokenize = this.IsTextField(field);
          FieldCrawlerBase fieldCrawler = FieldCrawlerFactory.GetFieldCrawler(field);
          Assert.IsNotNull(fieldCrawler, "fieldCrawler");
          if (this.IndexAllFields)
          {
            document.Add(CreateField(field.Key, fieldCrawler.GetValue(), tokenize, 1f));
          }

          if (tokenize)
          {
            document.Add(CreateField(Sitecore.Search.BuiltinFields.Content, fieldCrawler.GetValue(), tokenize, 1f));
          }
        }
      }
    }

    /// <summary>
    /// Adds the boosted fields.
    /// </summary>
    /// <param name="document">The document.</param>
    /// <param name="item">The item to proceed.</param>
    protected override void AddSpecialFields(Document document, Item item)
    {
      base.AddSpecialFields(document, item);
      document.Add(CreateTextField(Sitecore.Search.BuiltinFields.Template, ShortID.Encode(item.TemplateID)));
      this.AddBaseTemplateForProducts(document, item);
      StringBuilder result = new StringBuilder();
      this.GetAllBaseTemplates(item.Template, ref result);
      document.Add(CreateTextField(Sitecore.Search.BuiltinFields.AllTemplates, result.ToString()));
    }

    /// <summary>
    /// Determines whether [is standard field] [the specified field].
    /// </summary>
    /// <param name="field">The field.</param>
    /// <returns><c>true</c> if [is standard field] [the specified field]; otherwise, <c>false</c>.</returns>
    protected virtual bool IsStandardField(Sitecore.Data.Fields.Field field)
    {
      Assert.ArgumentNotNull(field, "field");

      TemplateField templateField = field.GetTemplateField();
      return (templateField.Template.ID != TemplateIDs.StandardTemplate) && (templateField.Template.BaseIDs.Length == 0);
    }

    /// <summary>
    /// Adds the base template for products.
    /// </summary>
    /// <param name="document">The document.</param>
    /// <param name="item">The item to proceed.</param>
    protected virtual void AddBaseTemplateForProducts(Document document, Item item)
    {
      ID productBaseTemplateId = new ID(Settings.GetSetting("Ecommerce.Product.BaseTemplateId"));
      if (item.Template.BaseTemplates.Any(bt => bt.ID.Equals(productBaseTemplateId)))
      {
        document.Add(CreateTextField(BuiltinFields.ProductBaseTemplate, ShortID.Encode(productBaseTemplateId)));
      }
    }

    /// <summary>
    /// Gets all base templates.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="result">The result.</param>
    protected virtual void GetAllBaseTemplates(TemplateItem item, ref StringBuilder result)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(result, "result");

      result.Append(ShortID.Encode(item.ID));
      result.Append(" ");

      List<TemplateItem> templates = item.BaseTemplates.Where(template => template.BaseTemplates.Count() > 0 && template.ID != TemplateIDs.StandardTemplate).ToList();

      foreach (TemplateItem template in templates)
      {
        this.GetAllBaseTemplates(template, ref result);
      }
    }

    /// <summary>
    /// Initializes the specified crawler for the index.
    /// </summary>
    /// <param name="index">The index.</param>
    void ICrawler.Initialize(Index index)
    {
      if (this.Root != null)
      {
        this.Initialize(index);
      }
    }
  }
}