// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldNamingPolicy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the field naming policy class.
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

namespace Sitecore.Ecommerce.Data.Mapping
{
  using System.Text;
  using Diagnostics;

  /// <summary>
  /// Defines the field naming policy class.
  /// </summary>
  public class FieldNamingPolicy
  {
    /// <summary>
    /// Gets the name of the field.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>The field name.</returns>
    [NotNull]
    public virtual string GetFieldName([NotNull] string source)
    {
      Assert.ArgumentNotNullOrEmpty(source, "source");

      if (source.ToUpper() == source)
      {
        return source;
      }

      StringBuilder nameBuilder = new StringBuilder();
      foreach (char c in source)
      {
        if (char.IsUpper(c) && source.IndexOf(c) > 0)
        {
          nameBuilder.Append(" ");
        }

        nameBuilder.Append(c);
      }

      return nameBuilder.ToString();
    }
  }
}