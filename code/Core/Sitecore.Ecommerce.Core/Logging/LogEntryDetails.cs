// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntryDetails.cs" company="Sitecore Corporation">
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
  using System.Collections;
  using System.Linq;
  using Diagnostics;
  using Globalization;
  using Text;

  /// <summary>
  /// Defines the details class.
  /// </summary>
  public class LogEntryDetails
  {
    /// <summary>
    /// Details parameters.
    /// </summary>
    private object[] parameters;

    /// <summary>
    /// The formatted parameters.
    /// </summary>
    private string formattedParameters;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LogEntryDetails"/> class.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="parameters">The parameters.</param>
    public LogEntryDetails([NotNull] string key, [NotNull] params object[] parameters)
    {
      Assert.ArgumentNotNull(key, "key");
      Assert.ArgumentNotNull(parameters, "parameters");
      Assert.ArgumentNotNullOrEmpty(key, "key");

      this.MessageKey = key;
      this.FormatParameters(parameters);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogEntryDetails"/> class.
    /// </summary>
    protected LogEntryDetails()
    {
    }

    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    /// <value>
    /// The key.
    /// </value>
    public string MessageKey { get; protected set; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public string Message
    {
      get
      {
        return this.Parameters != null ? Translate.Text(this.MessageKey, this.Parameters) : Translate.Text(this.MessageKey);
      }
    }

    /// <summary>
    /// Gets or sets the formatted parameters.
    /// </summary>
    /// <value>
    /// The formatted parameters.
    /// </value>
    public virtual string FormattedParameters
    {
      get
      {
        return this.formattedParameters;
      }

      protected set
      {
        this.formattedParameters = value;
      }
    }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    [CanBeNull]
    private object[] Parameters
    {
      get
      {
        if (this.parameters == null && !string.IsNullOrEmpty(this.FormattedParameters))
        {
          // ReSharper disable CoVariantArrayConversion
          this.parameters = new ListString(this.FormattedParameters).ToArray();
          // ReSharper restore CoVariantArrayConversion
        }

        return this.parameters;
      }
    }

    /// <summary>
    /// Formats the parameters.
    /// </summary>
    /// <param name="keyParams">The parameters.</param>
    private void FormatParameters([NotNull] IEnumerable keyParams)
    {
      Debug.ArgumentNotNull(keyParams, "keyParams");

      var list = new ListString();

      foreach (var param in keyParams)
      {
        if ((param == null) || string.IsNullOrEmpty(param.ToString()))
        {
          list.Add("Not defined");
          continue;
        }

        list.Add(param.ToString());
      }

      this.formattedParameters = list.ToString();
    }
  }
}