// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mail.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the mail class.
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

namespace Sitecore.Ecommerce.Mails
{
  using System;
  using System.ComponentModel;
  using System.Configuration;
  using System.IO;
  using System.Net.Mail;
  using System.Web;
  using Diagnostics;
  using DomainModel.Configurations;
  using DomainModel.Mails;
  using Links;
  using SecurityModel;
  using Sitecore.Data.Items;
  using Sitecore.Web;
  using Text;
  using Utils;

  /// <summary>
  /// Defines the mail class.
  /// </summary>
  public class Mail : IMail
  {
    #region Consts

    /// <summary>
    /// The encrypt key.
    /// </summary>
    private const string EncryptKey = "5dfkjek5";

    /// <summary> Could not load mailtemplate message. </summary>
    private const string CouldNotLoadTemplateMessage = "Could not load mailtemplate:  from {0}";

    /// <summary> The Invalid Email address message. </summary>
    private const string InvalidEmailAddressMessage = "'{0}' is not a valid email address. Check '{2}' field in mail template '{1}'";

    /// <summary> The mail sent message. </summary>
    private const string MailSentToMessage = "Mail sent to {0} with subject: {1}";

    /// <summary> The could not send mail message. </summary>
    private const string CouldNotSendMailMessage = "Could not send Mail: '{0}' To:{1}";

    #endregion

    #region Mail Data fields

    /// <summary>
    /// The mail from address.
    /// </summary>
    private string mailFrom = string.Empty;

    /// <summary>
    /// The mail to adrress.
    /// </summary>
    private string mailTo = string.Empty;

    /// <summary>
    /// The mail body.
    /// </summary>
    private string mailBody = string.Empty;

    /// <summary>
    /// The mail subject.
    /// </summary>
    private string mailSubject = string.Empty;

    /// <summary>
    /// The mail attachment file name.
    /// </summary>
    private string mailAttachmentFileName = string.Empty;

    #endregion

    /// <summary>
    /// Sends the mail.
    /// </summary>
    /// <param name="to">The mail recipient.</param>
    /// <param name="from">The mail sender.</param>
    /// <param name="subject">The mail subject.</param>
    /// <param name="body">The mail body.</param>
    /// <param name="attachmentFileName">Name of the attachment file.</param>
    public virtual void SendMail([NotNull] string to, [NotNull] string from, [NotNull] string subject, [NotNull] string body, [NotNull] string attachmentFileName)
    {
      Assert.ArgumentNotNull(to, "to");
      Assert.ArgumentNotNull(from, "from");
      Assert.ArgumentNotNull(subject, "subject");
      Assert.ArgumentNotNull(body, "body");
      Assert.ArgumentNotNull(attachmentFileName, "attachmentFileName");

      this.mailTo = to;
      this.mailFrom = from;
      this.mailBody = body;
      this.mailAttachmentFileName = attachmentFileName;
      this.mailSubject = subject;

      System.Web.UI.Page page = HttpContext.Current.CurrentHandler as System.Web.UI.Page;
      if (page != null && page.IsAsync)
      {
        BackgroundWorker backgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
        backgroundWorker.DoWork += this.BackgroundWorker_DoWork;
        backgroundWorker.RunWorkerAsync();
      }
      else
      {
        this.SendMail();
      }
    }

    /// <summary>
    /// Sends the mail.
    /// </summary>
    /// <param name="templateName">Name of the template.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="queryString">The query string.</param>
    /// <exception cref="ConfigurationErrorsException"><c>The Configuration errors exception</c>.</exception>
    public virtual void SendMail([NotNull] string templateName, [NotNull] object parameters, [NotNull] string queryString)
    {
      Assert.ArgumentNotNull(templateName, "templateName");
      Assert.ArgumentNotNull(parameters, "parameters");
      Assert.ArgumentNotNull(queryString, "queryString");

      GeneralSettings generalSettings = Context.Entity.GetConfiguration<GeneralSettings>();

      Item templateItem = Sitecore.Context.Database.GetItem(generalSettings.MailTemplatesLink);

      if (templateItem == null)
      {
        return;
      }

      string templatePath = string.Format("{0}/{1}", templateItem.Paths.FullPath, templateName);
      Item mailTemplate = Sitecore.Context.Database.GetItem(templatePath);

      if (mailTemplate == null)
      {
        Log.Warn(string.Format(CouldNotLoadTemplateMessage, templatePath), this);
        return;
      }

      string body = mailTemplate["Body"].FormatWith(parameters);
      string subject = mailTemplate["Subject"].FormatWith(parameters);

      // Get body from external source
      string bodySourceId = mailTemplate["BodySource"];
      Item bodySourceItem = Sitecore.Context.Database.GetItem(bodySourceId);

      if (bodySourceItem != null)
      {
        UrlString qs = new UrlString(queryString);
        string orderId = qs["orderid"];
        Assert.IsNotNullOrEmpty(orderId, "Unable to send mail. Order number is null or empty.");

        string encryptKey = Crypto.EncryptTripleDES(orderId, EncryptKey);
        encryptKey = Uri.EscapeDataString(encryptKey);
        
        UrlString url = new UrlString(LinkManager.GetItemUrl(bodySourceItem));
        url.Add("key", encryptKey);
        url.Append(new UrlString(queryString));
        
        using (new SecurityDisabler())
        {
          body += WebUtil.ExecuteWebPage(url.ToString());
        }
      }

      // Replace relative url with absolute url
      string urlPrefix = "http://" + HttpContext.Current.Request.Url.Host + "/";
      body = body.Replace("href=\"/", "href=\"" + urlPrefix);
      body = body.Replace("href='/", "href='" + urlPrefix);
      body = body.Replace("HREF=\"/", "href=\"" + urlPrefix);
      body = body.Replace("HREF='/", "href='" + urlPrefix);
      body = body.Replace("src=\"/", "src=\"" + urlPrefix);
      body = body.Replace("src='/", "src='" + urlPrefix);
      body = body.Replace("SRC=\"/", "src=\"" + urlPrefix);
      body = body.Replace("SRC='/", "src='" + urlPrefix);

      // Configuration.OrderGrid
      string from = mailTemplate["From"].FormatWith(parameters);
      if (!MainUtil.IsValidEmailAddress(from))
      {
        string info = string.Format(InvalidEmailAddressMessage, from, templateName, "From");
        ConfigurationErrorsException configurationErrorsException = new ConfigurationErrorsException(info);
        Log.Warn(configurationErrorsException.Message, configurationErrorsException, this);
        return;
      }

      string to = mailTemplate["To"].FormatWith(parameters);
      if (!MainUtil.IsValidEmailAddress(to))
      {
        string info = string.Format(InvalidEmailAddressMessage, to, templateName, "To");
        ConfigurationErrorsException configurationErrorsException = new ConfigurationErrorsException(info);
        Log.Warn(configurationErrorsException.Message, configurationErrorsException, this);
        return;
      }

      this.SendMail(to, from, subject, body, string.Empty);
    }

    /// <summary>
    /// Handles the DoWork event of the imageEncoder control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
    protected virtual void BackgroundWorker_DoWork([NotNull] object sender, [NotNull] DoWorkEventArgs e)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(e, "e");

      this.SendMail();
    }

    /// <summary>
    /// Sends the mail.
    /// </summary>
    protected virtual void SendMail()
    {
      MailMessage message = new MailMessage
                              {
                                From = new MailAddress(this.mailFrom),
                                Body = this.mailBody,
                                Subject = this.mailSubject,
                                IsBodyHtml = true
                              };
      message.To.Add(this.mailTo);

      if (this.mailAttachmentFileName != null && File.Exists(this.mailAttachmentFileName))
      {
        Attachment attachment = new Attachment(this.mailAttachmentFileName);
        message.Attachments.Add(attachment);
      }

      try
      {
        Sitecore.MainUtil.SendMail(message);
        Log.Info(string.Format(MailSentToMessage, message.To, message.Subject), "SendMailFromTemplate");
      }
      catch (Exception err)
      {
        Log.Error(string.Format(CouldNotSendMailMessage, message.Subject, message.To), err, "SendMailFromTemplate");
        HttpContext.Current.Request.Params.Add("MailException", string.Empty);
      }
    }
  }
}