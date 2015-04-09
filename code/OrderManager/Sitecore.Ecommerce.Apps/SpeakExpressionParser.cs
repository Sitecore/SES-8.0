// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpeakExpressionParser.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the speak expression parser class.
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
  using System.Text.RegularExpressions;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Defines the speak expression parser class.
  /// </summary>
  public class SpeakExpressionParser
  {
    /// <summary>
    /// Parses the specified raw query.
    /// </summary>
    /// <param name="parameter">The start.</param>
    /// <param name="rawQuery">The raw query.</param>
    /// <param name="expression">The expression.</param>
    /// <returns>
    /// The string.
    /// </returns>
    [CanBeNull]
    public virtual string Parse([CanBeNull]string parameter, [CanBeNull]string rawQuery, [CanBeNull]string expression)
    {
      var parsed = this.Parse(rawQuery, expression);

      if (!string.IsNullOrEmpty(parsed) && !string.IsNullOrEmpty(parameter))
      {
        parsed = string.Format("{0} => {1}", parameter, parsed);

        parsed = this.ReplaceModelFields(parameter, parsed);
      }

      return parsed;
    }

    /// <summary>
    /// Parses the specified raw query.
    /// </summary>
    /// <param name="rawQuery">The raw query.</param>
    /// <param name="expression">The expression.</param>
    /// <returns>
    /// The string.
    /// </returns>
    [CanBeNull]
    private string Parse([CanBeNull]string rawQuery, [CanBeNull]string expression)
    {
      if (!string.IsNullOrEmpty(expression))
      {
        expression = Regex.Replace(expression, " and ", " && ", RegexOptions.IgnoreCase);
        expression = Regex.Replace(expression, " or ", " || ", RegexOptions.IgnoreCase);
      }

      if (ID.IsID(rawQuery) || string.IsNullOrEmpty(rawQuery))
      {
        return string.IsNullOrEmpty(expression) ? null : expression;
      }

      if (!string.IsNullOrEmpty(expression))
      {
        return string.Format("{0} && {1}", rawQuery, expression);
      }

      return rawQuery;
    }

    /// <summary>
    /// Replaces the model fields.
    /// </summary>
    /// <param name="paramaeter">The paramaeter.</param>
    /// <param name="expression">The expression.</param>
    /// <returns>
    /// The model fields.
    /// </returns>
    [NotNull]
    private string ReplaceModelFields([NotNull]string paramaeter, [NotNull]string expression)
    {
      Assert.ArgumentNotNull(paramaeter, "paramaeter");
      Assert.ArgumentNotNull(expression, "expression");

      expression = expression.Replace(string.Format("{0}.ID", paramaeter), string.Format("{0}.OrderId", paramaeter));
      expression = expression.Replace(string.Format("{0}.Currency", paramaeter), string.Format("{0}.PricingCurrencyCode", paramaeter));
      expression = expression.Replace(string.Format("{0}.CustomerName", paramaeter), string.Format("{0}.BuyerCustomerParty.Party.PartyName", paramaeter));
      expression = expression.Replace(string.Format("{0}.TotalAmount", paramaeter), string.Format("{0}.AnticipatedMonetaryTotal.PayableAmount.Value", paramaeter));

      return expression;
    }
  }
}