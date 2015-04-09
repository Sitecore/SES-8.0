// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Severity.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the LogEntry type.
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

namespace Sitecore.Ecommerce.Logging
{
  using Globalization;

  /// <summary>
  /// Entry type.
  /// </summary>
  public enum SeverityLevels
  {
     /// <summary>
    /// Information type.
    /// </summary>
    Information = 0,

    /// <summary>
    /// Warning type
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Error ype.
    /// </summary>
    Error = 2
  }

  /// <summary>
  /// Defines the severity class.
  /// </summary>
  public class Severity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Severity"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public Severity(int value)
    {
      this.SeverityLevel = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Severity"/> class.
    /// </summary>
    /// <param name="type">The type.</param>
    public Severity(SeverityLevels type)
    {
      this.SeverityLevel = (int)type;
    }

    /// <summary>
    /// Prevents a default instance of the <see cref="Severity"/> class from being created.
    /// </summary>
    private Severity()
      : this(default(SeverityLevels))
    {
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    public int SeverityLevel { get; private set; }

    /// <summary>
    /// Performs an implicit conversion from <see cref="int"/> to <see cref="Sitecore.Ecommerce.Core"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator int(Severity type)
    {
      return type.SeverityLevel;
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="Severity"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator Severity(int value)
    {
      return new Severity(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="Severity"/> to <see cref="SeverityLevels"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator SeverityLevels(Severity type)
    {
      return (SeverityLevels)type.SeverityLevel;
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="SeverityLevels"/> to <see cref="Severity"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator Severity(SeverityLevels type)
    {
      return new Severity(type);
    }

    /// <summary>
    /// Returns localized string representation of the Severity.
    /// </summary>
    /// <returns>Localized severity</returns>
    public override string ToString()
    {
      return Translate.Text(((SeverityLevels)this.SeverityLevel).ToString());
    }
  }
}