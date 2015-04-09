// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StiReportFactory.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the stimulsoft report factory class.
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
  using System.Web;
  using Diagnostics;
  using IO;
  using Stimulsoft.Report;
  using Stimulsoft.Report.Components;

  /// <summary>
  /// Defines the stimulsoft report factory class.
  /// </summary>
  public class StiReportFactory
  {
    /// <summary>
    /// The URL key.
    /// </summary>
    private const string UrlKey = "BaseUrl";

    /// <summary>
    /// The logo image key.
    /// </summary>
    private const string LogoImageKey = "LogoImage";

    /// <summary>
    /// The report helper
    /// </summary>
    private readonly StiReportHelper reportHelper;

    /// <summary>
    /// The translator
    /// </summary>
    private readonly StiReportTranslator reportTranslator;

    /// <summary>
    /// The report file.
    /// </summary>
    private string reportFile;

    /// <summary>
    /// The logo file.
    /// </summary>
    private string logoFile;

    /// <summary>
    /// The http context.
    /// </summary>
    private HttpContextBase httpContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="StiReportFactory" /> class.
    /// </summary>
    /// <param name="reportHelper">The report helper.</param>
    /// <param name="reportTranslator">The translator.</param>
    public StiReportFactory(StiReportHelper reportHelper, StiReportTranslator reportTranslator)
    {
      this.reportHelper = reportHelper;
      this.reportTranslator = reportTranslator;
    }

    /// <summary>
    /// Gets or sets the HTTP context.
    /// </summary>
    /// <value>The HTTP context.</value>
    [CanBeNull]
    public HttpContextBase HttpContext
    {
      get
      {
        if (this.httpContext == null && System.Web.HttpContext.Current != null)
        {
          this.httpContext = new HttpContextWrapper(System.Web.HttpContext.Current);
        }

        return this.httpContext;
      }

      set
      {
        this.httpContext = value;
      }
    }

    /// <summary>
    /// Gets or sets the report file.
    /// </summary>
    /// <value>
    /// The report file.
    /// </value>
    public string ReportFile
    {
      get
      {
        if (string.IsNullOrEmpty(this.reportFile))
        {
          this.reportFile = "/sitecore modules/shell/Ecommerce/Reports/OrderDetails.mrt";
        }

        return this.reportFile;
      }

      set
      {
        this.reportFile = value;
      }
    }

    /// <summary>
    /// Gets or sets the logo file.
    /// </summary>
    /// <value>
    /// The logo file.
    /// </value>
    public string LogoFile
    {
      get
      {
        if (string.IsNullOrEmpty(this.logoFile))
        {
          this.logoFile = "/sitecore modules/shell/Ecommerce/Reports/logo.jpg";
        }

        return this.logoFile;
      }

      set
      {
        this.logoFile = value;
      }
    }

    /// <summary>
    /// Creates the report.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>
    /// The report.
    /// </returns>
    [NotNull]
    public virtual StiReport CreateReport([NotNull] OrderReportModel model)
    {
      Assert.ArgumentNotNull(model, "model");

      StiReport report = new StiReport();
      string language = string.IsNullOrEmpty(model.LanguageCode) ? Sitecore.Context.Language.Name : model.LanguageCode;

      string reportFilePath = FileUtil.MapPath(this.ReportFile);

      StiConfig.ApplicationDirectory = FileUtil.GetParentPath(reportFilePath);
      string reportFileName = Path.GetFileNameWithoutExtension(reportFilePath);
      reportFileName = string.Concat(reportFileName, "-", language, ".dll");
      string compiledReportFile = FileUtil.MakePath(StiConfig.ApplicationDirectory, reportFileName);

      if (!this.reportHelper.CheckIfCompiledReportExists(compiledReportFile))
      {
        report.Load(reportFilePath);
        report.Dictionary.Variables[UrlKey].Value = this.GetLogoUrl();

        report.AutoLocalizeReportOnRun = true;
        this.reportTranslator.Translate(report, language);

        StiImage image = (StiImage)report.GetComponentByName(LogoImageKey);
        string reportLogoFilePath = FileUtil.MapPath(this.LogoFile);
        image.Image = System.Drawing.Image.FromFile(reportLogoFilePath);

        this.reportHelper.CompileReport(report, compiledReportFile);
      }
      else
      {
        report = this.reportHelper.GetCompiledReport(compiledReportFile);
      }

      if (model.CompanyMasterData != null)
      {
        model.CompanyMasterData.ReadData();
      }

      if (model.Order != null)
      {
        report.RegData("Order", model);
      }

      return report;
    }

    /// <summary>
    /// Gets the logo URL.
    /// </summary>
    /// <returns></returns>
    private string GetLogoUrl()
    {
      Assert.IsNotNull(this.HttpContext, "Unable to set up host. Http context required.");

      var url = this.HttpContext.Request.Url;
      Assert.IsNotNull(url, "Unable to set up host. Url required.");

      var port = url.Port;
      string host;

      if (port == 80 || port == 443)
      {
        host = url.Host;
      }
      else
      {
        host = this.HttpContext.Request.ServerVariables["HTTP_HOST"];
      }

      return string.Format("{0}://{1}", url.Scheme, host);
    }
  }
}