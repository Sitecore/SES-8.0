// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldQuery.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Field query class.
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

namespace Sitecore.Ecommerce.Search
{
  using System;

  /// <summary>
  /// Field query class.
  /// </summary>
  [Serializable]
  public class FieldQuery : SubQuery
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldQuery"/> class.
    /// </summary>
    public FieldQuery()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FieldQuery"/> class.
    /// </summary>
    /// <param name="key">The field key.</param>
    /// <param name="value">The value.</param>
    /// <param name="matchVariant">The match variant.</param>
    public FieldQuery(string key, string value, MatchVariant matchVariant)
      : base(key, value, matchVariant)
    {
    }
  }
}