// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpeakExpressionLocalizer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SpeakExpressionLocalizer class.
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

namespace Sitecore.Ecommerce.Apps
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;
  using Ecommerce.OrderManagement;
  using Globalization;

  /// <summary>
  /// Defines the SpeakExpressionLocalizer class.
  /// </summary>
  public class SpeakExpressionLocalizer
  {
    /// <summary>
    /// Format pattern.
    /// </summary>
    private const string FormatPattern = "Sitecore.Globalization.Translate.Text({0})";

    /// <summary>
    /// Max ANSI code.
    /// </summary>
    private const int MaxAnsiCode = 255;

    /// <summary>
    /// Regex patterns.
    /// </summary>
    private readonly string[] regexPropertyPatterns = new[]
    {
      @"o\.State\.Code",
      @"o\.State\.Name"
    };

    /// <summary>
    /// The state code patterns.
    /// </summary>
    private readonly string[] stateCodesPatterns = new[]
    {
      OrderStateCode.New,
      OrderStateCode.Open,
      Texts.InProcess,
      OrderStateCode.Closed,
      OrderStateCode.Cancelled
    };

    /// <summary>
    /// The state transition patterns [crutch].
    /// </summary>
    private readonly IDictionary<string, string> stateKeysCrutchPatterns = new Dictionary<string, string>
    {
      { OrderStateCode.InProcess, Texts.InProcess }
    };

    /// <summary>
    /// The TranslateWrapper instance.
    /// </summary>
    private readonly TranslateWrapper translateWrapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpeakExpressionLocalizer"/> class.
    /// </summary>
    public SpeakExpressionLocalizer() : this(new TranslateWrapper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpeakExpressionLocalizer"/> class.
    /// </summary>
    /// <param name="translateWrapper">The translate wrapper.</param>
    public SpeakExpressionLocalizer(TranslateWrapper translateWrapper)
    {
      this.translateWrapper = translateWrapper;
    }

    /// <summary>
    /// Updates the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    /// The string.
    /// </returns>
    public virtual string Update(string input)
    {
      if (!this.ContainsNonAnsiCharacters(input))
      {
        return input;
      }

      StringBuilder stringBuilder = new StringBuilder(input);
      stringBuilder = this.TransformSpecialKeys(stringBuilder);
      stringBuilder = this.LocalizeKeys(stringBuilder);

      return this.PerformUpdate(stringBuilder);
    }

    /// <summary>
    /// Performs update of the input.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <returns>
    /// The update.
    /// </returns>
    private string PerformUpdate(StringBuilder stringBuilder)
    {
      foreach (string pattern in this.regexPropertyPatterns)
      {
        string match = Regex.Match(stringBuilder.ToString(), pattern).Value;
        if (!string.IsNullOrEmpty(match))
        {
          stringBuilder = stringBuilder.Replace(match, string.Format(FormatPattern, match));
        }
      }

      return stringBuilder.ToString();
    }

    /// <summary>
    /// Determines whether contains unicode character the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    ///   <c>true</c> if contains unicode character the specified input; otherwise, <c>false</c>.
    /// </returns>
    private bool ContainsNonAnsiCharacters(string input)
    {
      return input.ToCharArray().Any(c => c > MaxAnsiCode);
    }

    /// <summary>
    /// Localizes the key.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <returns>
    /// The key.
    /// </returns>
    private StringBuilder LocalizeKeys(StringBuilder stringBuilder)
    {
      foreach (var pattern in this.stateCodesPatterns)
      {
        string match = Regex.Match(stringBuilder.ToString(), pattern).Value;
        if (!string.IsNullOrEmpty(match))
        {
          stringBuilder = stringBuilder.Replace(match, this.translateWrapper.Text(match));
        }
      }

      return stringBuilder;
    }

    /// <summary>
    /// Transforms the special keys.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <returns>
    /// The special keys.
    /// </returns>
    private StringBuilder TransformSpecialKeys(StringBuilder stringBuilder)
    {
      foreach (var key in this.stateKeysCrutchPatterns.Keys)
      {
        string match = Regex.Match(stringBuilder.ToString(), key).Value;
        if (!string.IsNullOrEmpty(match))
        {
          stringBuilder = stringBuilder.Replace(match, this.stateKeysCrutchPatterns[key]);
        }
      }

      return stringBuilder;
    }
  }
}