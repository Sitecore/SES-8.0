// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StiReportTranslator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the StiReportTranslator type.
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
  using Diagnostics;
  using Globalization;
  using Stimulsoft.Report;
  using Stimulsoft.Report.Dictionary;

  /// <summary>
  /// Defines the report translator class.
  /// </summary>
  public class StiReportTranslator
  {
    /// <summary>
    /// Translates the specified report.
    /// </summary>
    /// <param name="report">The report.</param>
    /// <param name="languageCode">The language code.</param>
    public virtual void Translate([NotNull] StiReport report, string languageCode)
    {
      Assert.ArgumentNotNull(report, "report");

      foreach (StiVariable variable in report.Dictionary.Variables)
      {
        if (this.MustBeTranslated(variable.Name))
        {
          variable.Value = this.TranslatePhraseByLanguage(variable.Value, languageCode);
        }
      }
    }

    /// <summary>
    /// Musts the be translated.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The be translated.</returns>
    protected virtual bool MustBeTranslated([NotNull] string key)
    {
      Assert.ArgumentNotNull(key, "key");

      return key.StartsWith("TEXT_");
    }

    /// <summary>
    /// Translates the phrase.
    /// </summary>
    /// <param name="phrase">The phrase to translate.</param>
    /// <returns>The translated phrase.</returns>
    protected virtual string TranslatePhrase([NotNull] string phrase)
    {
      Assert.ArgumentNotNull(phrase, "phrase");

      return Globalization.Translate.Text(phrase);
    }

    /// <summary>
    /// Translates the phrase.
    /// </summary>
    /// <param name="phrase">The phrase to translate.</param>
    /// <param name="languageCode">The language code.</param>
    /// <returns>
    /// The translated phrase.
    /// </returns>
    protected virtual string TranslatePhraseByLanguage([NotNull] string phrase, [NotNull] string languageCode)
    {
      Assert.ArgumentNotNull(phrase, "phrase");
      Assert.ArgumentNotNull(languageCode, "languageCode");

      return Globalization.Translate.TextByLanguage(phrase, Language.Parse(languageCode));
    }
  }
}