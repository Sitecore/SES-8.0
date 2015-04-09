// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpeakDateTimeExtractor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the speak date time extractor class.
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
  using System;
  using System.Globalization;
  using System.Text.RegularExpressions;

  /// <summary>
  /// Defines the speak date time extractor class.
  /// </summary>
  public class SpeakDateTimeExtractor
  {
    /// <summary>
    /// Extracts the values.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>
    /// The values.
    /// </returns>
    public string Extract([CanBeNull]string expression)
    {
      if (!string.IsNullOrEmpty(expression))
      {
        Regex regex = new Regex(@"\d{8}T\d{6}");
        return regex.Replace(expression, m => this.Parse(m.ToString()));
      }

      return string.Empty;
    }

    /// <summary>
    /// Parses the specified initial value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The string.
    /// </returns>
    [NotNull]
    public string Parse([NotNull] string value)
    {
      var parsedValue = DateTime.ParseExact(value, "yyyyMMdd'T'HHmmss", DateTimeFormatInfo.InvariantInfo);

      return string.Format("(new System.DateTime({0}))", parsedValue.Ticks);
    }
  }
}
