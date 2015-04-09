// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBuilder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the message builder class.
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

namespace Sitecore.Ecommerce.Mail
{
  using System.Net.Mail;

  /// <summary>
  /// Defines the message builder class.
  /// </summary>
  public abstract class MessageBuilder
  {
    /// <summary>
    /// Gets the mail message.
    /// </summary>
    /// <param name="templateName">Name of the template.</param>
    /// <returns>
    /// The mail message
    /// </returns>
    [NotNull]
    public abstract MailMessage GetMessage(string templateName);

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    /// <returns>
    /// The parameters.
    /// </returns>
    [NotNull]
    protected abstract object GetParameters();
  }
}
