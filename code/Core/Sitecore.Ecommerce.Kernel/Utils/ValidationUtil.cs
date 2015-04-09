// -------------------------------------------------------------------------------------------
// <copyright file="ValidationUtil.cs" company="Sitecore Corporation">
//  Copyright (c) Sitecore Corporation 1999-2015 
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

namespace Sitecore.Ecommerce.Utils
{
  using System.Text.RegularExpressions;
  using System.Web.UI.WebControls;
  using Globalization;

  /// <summary>
  /// The Validation Util.
  /// </summary>
  public static class ValidationUtil
  {
    /// <summary>
    /// Validates the emailadress.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="args">
    /// The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.
    /// </param>
    /// <param name="labelNavn">
    /// The label navn.
    /// </param>
    /// <param name="validateEmpty">
    /// if set to <c>true</c> [validate empty].
    /// </param>
    public static void ValidateEmailAddress(object sender, ServerValidateEventArgs args, string labelNavn, bool validateEmpty)
    {
      var validator = (BaseValidator)sender;
      if (validateEmpty && string.IsNullOrEmpty(args.Value))
      {
        validator.ErrorMessage = string.Format(Translate.Text(Texts.TheFieldCannotBeEmpty), CleanLabelName(labelNavn));
        args.IsValid = false;
      }

      // The email adress doesn't conform to a general email pattern
      else if (!IsValidEmailPattern(args.Value))
      {
        validator.ErrorMessage = string.Format(Translate.Text(Texts.TheFieldContainsAnInvalidEmailAddress), CleanLabelName(labelNavn));
        args.IsValid = false;
      }
    }

    /// <summary>
    /// Validates a number field
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="args">
    /// The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.
    /// </param>
    /// <param name="labelName">
    /// The label name.
    /// </param>
    /// <param name="validateEmpty">
    /// if set to <c>true</c> [validate empty].
    /// </param>
    public static void ValidateNumberValue(object sender, ServerValidateEventArgs args, string labelName, bool validateEmpty)
    {
      var validator = (BaseValidator)sender;
      int i;
      if (validateEmpty && string.IsNullOrEmpty(args.Value))
      {
        validator.ErrorMessage = string.Format(Translate.Text(Texts.TheFieldCannotBeEmpty), CleanLabelName(labelName));
        args.IsValid = false;
      }
      else if ((!string.IsNullOrEmpty(args.Value)) && (!int.TryParse(args.Value.Replace("+", "00"), out i)))
      {
        validator.ErrorMessage = string.Format(Translate.Text(Texts.TheFieldContainsInvalidCharacters), CleanLabelName(labelName));
        args.IsValid = false;
      }
    }

    /// <summary>
    /// Determines whether [is valid email pattern] [the specified epostadresse].
    /// </summary>
    /// <param name="epostadresse">
    /// The epostadresse.
    /// </param>
    /// <returns>
    /// <c>true</c> if [is valid email pattern] [the specified epostadresse]; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsValidEmailPattern(string epostadresse)
    {
      string pattern = @"[a-zA-Z0-9_\-\.]+@[a-zA-Z0-9_\-\.]+\.[a-zA-Z]{2,5}";
      return !string.IsNullOrEmpty(epostadresse) && Regex.IsMatch(epostadresse, pattern);
    }

    /// <summary>
    /// Cleans the name of the label.
    /// </summary>
    /// <param name="label">
    /// The label.
    /// </param>
    /// <returns>
    /// </returns>
    private static string CleanLabelName(string label)
    {
      return label.Replace(":", string.Empty);
    }
  }
}