// -------------------------------------------------------------------------------------------
// <copyright file="IMail.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
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

namespace Sitecore.Ecommerce.DomainModel.Mails
{
  /// <summary>
  /// The mail provider interface.
  /// </summary>
  public interface IMail
  {
    /// <summary>
    /// Sends the mail.
    /// </summary>
    /// <param name="to">The mail recipient.</param>
    /// <param name="from">The mail sender.</param>
    /// <param name="subject">The mail subject.</param>
    /// <param name="body">The mail body.</param>
    /// <param name="attachmentFileName">Name of the attachment file.</param>
    void SendMail(string to, string from, string subject, string body, string attachmentFileName);

    /// <summary>
    /// Sends the mail.
    /// </summary>
    /// <param name="templateName">Name of the template.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="queryString">The query string.</param>
    void SendMail(string templateName, object parameters, string queryString);
  }
}