// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxDefinitionCollection.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Represents collection of the textbox definitions.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models
{
  using System.Collections.ObjectModel;
  using Text;

  /// <summary>
  /// Represents collection of the textbox definitions.
  /// </summary>
  public class TextBoxDefinitionCollection : Collection<TextBoxDefinition>
  {
    /// <summary>
    /// The checklist parameter prefix;
    /// </summary>
    private const string ParameterPrefix = "tb_";

    /// <summary>
    /// Parses the specified value.
    /// </summary>
    /// <param name="s">The value.</param>
    /// <returns>The checklist collection.</returns>
    public static TextBoxDefinitionCollection Parse(string s)
    {
      TextBoxDefinitionCollection collection = new TextBoxDefinitionCollection();

      UrlString url = new UrlString(s);

      foreach (string parameter in url.Parameters)
      {
        if (!parameter.StartsWith(ParameterPrefix))
        {
          continue;
        }

        TextBoxDefinition tbd = new TextBoxDefinition { Field = parameter.Replace(ParameterPrefix, string.Empty), Title = url.Parameters[parameter] };
        collection.Add(tbd);
      }

      return collection;
    }
  }
}