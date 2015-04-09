// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdatePostStep.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the update post step class.
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

namespace Sitecore.Ecommerce.Update
{
  using System;
  using System.Collections.Specialized;
  using System.Globalization;
  using System.IO;
  using System.Text;
  using System.Web;
  using System.Xml;
  using System.Xml.Serialization;
  using Configuration;
  using Data.Fields;
  using Diagnostics;
  using log4net.spi;
  using PriceMatrix;
  using SecurityModel;
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Install.Framework;
  using StringExtensions;
  using Text;

  /// <summary>
  /// Defines the update post step class.
  /// </summary>
  public class UpdatePostStep : IPostStep
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePostStep"/> class.
    /// </summary>
    public UpdatePostStep()
    {
      this.Result = new StringWriter();
    }

    /// <summary>
    /// Gets or sets CurrentCulture.
    /// </summary>
    public string PriceCulture { get; set; }

    /// <summary>
    /// Gets the result.
    /// </summary>
    public StringWriter Result { get; private set; }

    /// <summary>
    /// Runs this post step
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="metaData">The meta data.</param>
    public void Run([NotNull] ITaskOutput output, [NotNull] NameValueCollection metaData)
    {
      Assert.ArgumentNotNull(output, "output");
      Assert.ArgumentNotNull(metaData, "metaData");

      if (string.IsNullOrEmpty(this.PriceCulture))
      {
        this.Write("Numbers converting: The current price culture is not set. There is nothing to convert.", Level.INFO);
        return;
      }

      CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

      try
      {
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(this.PriceCulture);

        ListString lineFields = new ListString("Vat|TotalPriceExVat|TotalPriceIncVat|DiscountExVat|DiscountIncVat|PriceExVat|PriceIncVat|TotalVat|PossibleDiscountExVat|PossibleDiscountIncVat");
        this.FormatPrices(Settings.GetSetting("Ecommerce.Order.OrderLineItemTempalteId"), lineFields);

        ListString orderFields = new ListString("Vat|DiscountExVat|DiscountIncVat|PriceExVat|PriceIncVat|TotalVat|PossibleDiscountExVat|PossibleDiscountIncVat|Shipping Price");
        this.FormatPrices(Settings.GetSetting("Ecommerce.Order.OrderItemTempalteId"), orderFields);

        this.FormatProductPrices(new ListString("{A457165A-E6C0-45AD-91BF-2C7407CB86E5}"));
        this.Write("Conversion was finished.", null, Level.INFO);
      }
      catch (Exception ex)
      {
        this.Write("Numbers converting: convertion failed", ex, Level.ERROR);
      }
      finally
      {
        System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
      }
    }

    /// <summary>
    /// Formats the prices.
    /// </summary>
    /// <param name="templateId">The template id.</param>
    /// <param name="fields">The fields.</param>
    private void FormatPrices([NotNull] string templateId, [NotNull] ListString fields)
    {
      Assert.ArgumentNotNullOrEmpty(templateId, "templateId");
      Assert.ArgumentNotNull(fields, "fields");

      Item[] items = Database.GetDatabase("master").GetRootItem().Axes.SelectItems(".//*[@@templateid='{0}']".FormatWith(templateId));
      if (items == null)
      {
        this.Write("Numbers converting: No items found to convert for \"{0}\" template.".FormatWith(templateId), Level.WARN);
        return;
      }

      PriceToInvariantConverter converter = new PriceToInvariantConverter();

      using (new SecurityDisabler())
      {
        foreach (Item item in items)
        {
          foreach (string field in fields)
          {
            string value = item[field];
            bool converted = converter.Convert(ref value);
            if (!converted && item[field] != value)
            {
              this.Write("Numbers converting: Unable to convert value \"{0}\", item id \"{1}\", item path \"{2}\", field name \"{3}\".".FormatWith(item[field], item.ID, item.Paths.FullPath, field), Level.WARN);
              continue;
            }

            if (item[field] == value)
            {
              continue;
            }

            using (new EditContext(item))
            {
              this.Write("Numbers converting: Updating value from \"{0}\" to \"{1}\", item id \"{2}\", item path \"{3}\", field name \"{4}\".".FormatWith(item[field], value, item.ID, item.Paths.FullPath, field), Level.INFO);
              item[field] = value;
            }
          }
        }
      }
    }

    /// <summary>
    /// Formats the product prices.
    /// </summary>
    /// <param name="fieldIDs">The field Ids.</param>
    protected virtual void FormatProductPrices([NotNull] System.Collections.Generic.IEnumerable<string> fieldIDs)
    {
      Assert.ArgumentNotNull(fieldIDs, "fieldIDs");

      using (new SecurityDisabler())
      {
        Item contentItem = Database.GetDatabase("master").GetItem(ItemIDs.ContentRoot);
        Assert.IsNotNull(contentItem, "Cannot get content root item");

        Item[] productItems = contentItem.Axes.SelectItems(".//*");
        foreach (Item productItem in productItems)
        {
          productItem.Fields.ReadAll();
          foreach (string fieldId in fieldIDs)
          {
            PriceField priceField;
            if (productItem.Fields.Contains(ID.Parse(fieldId)))
            {
              priceField = productItem.Fields[ID.Parse(fieldId)];

              PriceMatrix matrix = new PriceMatrix(priceField.PriceMatrix.MainCategory);

              this.UpdatePriceMatrix(matrix.MainCategory, productItem, priceField.InnerField.Name);

              using (new EditContext(productItem, false, true))
              {
                productItem[ID.Parse(fieldId)] = SavePriceMatrix(matrix);
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Saves the price matrix.
    /// </summary>
    /// <param name="priceMatrix">The price matrix.</param>
    /// <returns>Returns the serialized price matrix</returns>
    [NotNull]
    private static string SavePriceMatrix([NotNull] PriceMatrix priceMatrix)
    {
      Assert.ArgumentNotNull(priceMatrix, "priceMatrix");

      var x = new XmlSerializer(priceMatrix.GetType());
      var sb = new StringBuilder();
      var settings = new XmlWriterSettings
      {
        OmitXmlDeclaration = true
      };
      XmlWriter xw = XmlWriter.Create(sb, settings);
      var ns = new XmlSerializerNamespaces();
      ns.Add(string.Empty, string.Empty);
      x.Serialize(xw, priceMatrix, ns);

      return sb.ToString();
    }

    /// <summary>
    /// Updates the price matrix.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="sitecoreItem">The sitecore item.</param>
    /// <param name="fieldName">Name of the field.</param>
    private void UpdatePriceMatrix([NotNull] Category category, [NotNull] Item sitecoreItem, [NotNull] string fieldName)
    {
      Assert.ArgumentNotNull(category, "category");
      Assert.ArgumentNotNull(sitecoreItem, "sitecoreItem");
      Assert.ArgumentNotNull(fieldName, "fieldName");

      foreach (Category subCategory in category.Categories)
      {
        foreach (CategoryItem item in subCategory.Items)
        {
          PriceToInvariantConverter converter = new PriceToInvariantConverter();
          string value = item.Amount;
          bool converted = converter.Convert(ref value);
          if (!converted && item.Amount != value)
          {
            this.Write("Numbers converting: Unable to convert value \"{0}\", item id \"{1}\", item path \"{2}\", field name \"{3}\".".FormatWith(item.Amount, sitecoreItem.ID, sitecoreItem.Paths.FullPath, fieldName), Level.WARN);
            continue;
          }

          this.Write("Numbers converting: Updating number value from \"{0}\" to \"{1}\", item id \"{2}\", item path \"{3}\", field name \"{4}\".".FormatWith(item.Amount, value, sitecoreItem.ID, sitecoreItem.Paths.FullPath, fieldName), Level.INFO);
          item.Amount = value;
        }

        if (subCategory.Categories.Count > 0)
        {
          this.UpdatePriceMatrix(subCategory, sitecoreItem, fieldName);
        }
      }
    }

    /// <summary>
    ///  Writes conversion log
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="level">The level.</param>
    private void Write([NotNull] string message, [NotNull] Level level)
    {
      Assert.ArgumentNotNull(message, "message");
      Assert.ArgumentNotNull(level, "level");

      this.Write(message, null, level);
    }

    /// <summary>Writes conversion log</summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="level">The level.</param>
    private void Write([NotNull] string message, Exception exception, [NotNull] Level level)
    {
      Assert.ArgumentNotNull(message, "message");
      Assert.ArgumentNotNull(level, "level");

      switch (level.Name)
      {
        case "INFO":
          {
            Log.Info(message, this);
            WriteToTarget(string.Format("{0} <br />", message));
            break;
          }

        case "WARN":
          {
            Log.Warn(message, this);
            WriteToTarget(string.Format("{0} <br />", message));
            break;
          }

        case "ERROR":
          {
            Log.Error(message, exception, this);
            WriteToTarget(string.Format("{0} <br />", message));
            break;
          }

        default:
          {
            break;
          }
      }
    }

    /// <summary>
    /// Additional writer
    /// </summary>
    /// <param name="message">The message.</param>
    private void WriteToTarget([NotNull] string message)
    {
      Assert.ArgumentNotNullOrEmpty(message, "message");

      this.Result.Write(string.Format("{0} <br />", message));
    }
  }
}