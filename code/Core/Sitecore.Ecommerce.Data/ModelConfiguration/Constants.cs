// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Constants class.
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

namespace Sitecore.Ecommerce.Data.ModelConfiguration
{
  /// <summary>
  /// Defines the constants class.
  /// </summary>
  public class Constants
  {
    /// <summary>
    /// Max code fields length.
    /// </summary>
    public const int BigCodeFieldLength = 50;

    /// <summary>
    /// Short code field length.
    /// </summary>
    public const int ShortCodeFieldLength = 3;

    /// <summary>
    /// The language code field length.
    /// See the definition of the macro-languages.
    /// They could be in the 'aaa–ezz' format.
    /// </summary>
    public const int LanguageCodeFieldLength = 7;

    /// <summary>
    /// Name field length.
    /// </summary>
    public const int NameFieldLength = 500;

    /// <summary>
    /// URI field length.
    /// </summary>
    public const int UriFieldLenght = 260;

    /// <summary>
    /// The decimal numeric scale.
    /// </summary>
    public const byte DecimalNumericScale = 6;

    /// <summary>
    /// The decimal numeric precision.
    /// </summary>
    public const byte DecimalNumericPrecision = 18;
  }
}