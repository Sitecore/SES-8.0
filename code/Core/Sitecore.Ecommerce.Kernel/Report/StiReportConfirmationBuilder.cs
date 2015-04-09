// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StiReportConfirmationBuilder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the StimulsoftReportConfirmationBuilder type.
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

namespace Sitecore.Ecommerce.Report
{
  using System.IO;
  using System.Net.Mail;
  using Data.Mapping;
  using Diagnostics;
  using DomainModel.Configurations;
  using Globalization;
  using Mails;
  using OrderManagement;
  using Sitecore.Data.Items;
  using Stimulsoft.Report;
  using Stimulsoft.Report.Export;
  using Utils;

  /// <summary>
  /// Defines the Stimulsoft report confirmation builder class.
  /// </summary>
  public class StiReportConfirmationBuilder : ConfirmationMessageBuilder
  {
    /// <summary>
    /// The report factory.
    /// </summary>
    private readonly StiReportFactory reportFactory;

    /// <summary>
    /// The report model.
    /// </summary>
    private readonly OrderReportModel reportModel;

    /// <summary>
    /// The shop context.
    /// </summary>
    private readonly ShopContext shopContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="StiReportConfirmationBuilder" /> class.
    /// </summary>
    /// <param name="reportFactory">The report factory.</param>
    /// <param name="reportModel">The report model.</param>
    /// <param name="shopContext">The shop context.</param>
    public StiReportConfirmationBuilder([NotNull] StiReportFactory reportFactory, OrderReportModel reportModel, ShopContext shopContext)
    {
      Assert.ArgumentNotNull(reportFactory, "reportFactory");
      Assert.ArgumentNotNull(reportModel, "reportModel");

      this.reportFactory = reportFactory;
      this.reportModel = reportModel;
      this.shopContext = shopContext;
    }

    /// <summary>
    /// Gets the mail message.
    /// </summary>
    /// <param name="templateName">Name of the template.</param>
    /// <returns>
    /// The mail message
    /// </returns>
    public override MailMessage GetMessage([NotNull] string templateName)
    {
      Assert.ArgumentNotNull(templateName, "templateName");
      Assert.IsNotNull(this.Order, "Order cannot be null.");

      string language;
      if (this.Order.BuyerCustomerParty != null && (this.Order.BuyerCustomerParty.Party != null) && !string.IsNullOrEmpty(this.Order.BuyerCustomerParty.Party.LanguageCode))
      {
        language = this.Order.BuyerCustomerParty.Party.LanguageCode;
      }
      else
      {
        language = Sitecore.Context.Language.Name;
      }

      MailTemplate mailTemplate = this.GetMailTemplate(templateName, language);

      Assert.IsNotNull(mailTemplate, "Mail Template cannot be null.");

      object parameters = this.GetParameters();

      string from = mailTemplate.From.FormatWith(parameters);
      string to = mailTemplate.To.FormatWith(parameters);

      MailMessage message = new MailMessage(from, to)
      {
        Subject = mailTemplate.Subject.FormatWith(parameters),
        Body = mailTemplate.Body.FormatWith(parameters),
        IsBodyHtml = true
      };

      message.Attachments.Add(this.GetAttachment());

      return message;
    }

    /// <summary>
    /// Gets the attachment.
    /// </summary>
    /// <returns>
    /// The attachment.
    /// </returns>
    [NotNull]
    protected virtual Attachment GetAttachment()
    {
      Assert.IsNotNull(this.reportModel, "Unable to build the report. Report model is not set.");

      this.reportModel.Order = this.Order;

      StiReport report = this.reportFactory.CreateReport(this.reportModel);
      report.Render();

      StiPdfExportService exportService = new StiPdfExportService();
      MemoryStream stream = new MemoryStream();
      exportService.ExportPdf(report, stream);

      stream.Seek(0, SeekOrigin.Begin);

      return new Attachment(stream, "Report.pdf", "application/pdf");
    }

    /// <summary>
    /// Gets the mail template.
    /// </summary>
    /// <param name="templateName">Name of the template.</param>
    /// <param name="language">The language.</param>
    /// <returns>
    /// The mail template.
    /// </returns>
    [CanBeNull]
    protected virtual MailTemplate GetMailTemplate([NotNull] string templateName, string language)
    {
      Assert.ArgumentNotNull(templateName, "templateName");
      Assert.IsNotNull(this.shopContext, "Unable to build report. ShopContext is not set.");
      Assert.IsNotNull(this.shopContext.Database, "Database must be set.");

      GeneralSettings generalSettings = this.shopContext.GeneralSettings;
      if (!string.IsNullOrEmpty(generalSettings.MailTemplatesLink))
      {
        Item templateItemContainer = this.shopContext.Database.GetItem(generalSettings.MailTemplatesLink, Language.Parse(language));

        if (templateItemContainer != null)
        {
          Item templateItem = templateItemContainer.Children[templateName];

          if (templateItem != null)
          {
            ItemToEntityMapper entityMapper = Context.Entity.Resolve<ItemToEntityMapper>();
            MailTemplate mailTemplate = new MailTemplate();
            entityMapper.Map(templateItem, mailTemplate);
            return mailTemplate;
          }
        }
      }

      return null;
    }
  }
}