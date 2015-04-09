// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchVariant.cs" company="Sitecore Corporation">
//  Copyright (c) Sitecore Corporation 1999-2015 
// </copyright>
// <summary>
//   Match variants.
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
  /// <summary>
  /// Match variants.
  /// </summary>
  public enum MatchVariant
  {
    /// <summary>
    /// When values must be equal.
    /// </summary>
    Exactly,

    /// <summary>
    /// When values should be equal.
    /// </summary>
    Like,

    /// <summary>
    /// When value must be greater 
    /// </summary>
    GreaterThan,

    /// <summary>
    ///  When value must be greater or equal
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// When value must be less 
    /// </summary>
    LessThan,

    /// <summary>
    ///  When value must be less or equal
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// When value must not be equals
    /// </summary>
    NotEquals
  }
}