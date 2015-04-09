// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlainMessageConfirmationBuilder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the plain text confirmation builder class.
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

namespace Sitecore.Ecommerce.OrderManagement
{
  using System.Net.Mail;
  using Diagnostics;

  /// <summary>
  /// Defines the plain text confirmation builder class.
  /// </summary>
  public class PlainMessageConfirmationBuilder : ConfirmationMessageBuilder
  {
    /// <summary>
    /// The message subject
    /// </summary>
    private const string MessageSubject = "Confirmed";

    /// <summary>
    /// The message body
    /// </summary>
    private const string MessageBody = "You order is confirmed";

    /// <summary>
    /// Gets the mail message.
    /// </summary>
    /// <param name="templateName">Name of the template.</param>
    /// <returns>
    /// The mail message
    /// </returns>
    [NotNull]
    public override MailMessage GetMessage([NotNull] string templateName)
    {
      Assert.ArgumentNotNull(templateName, "templateName");

      string from = this.Order.SellerSupplierParty.Party.Contact.ElectronicMail;
      string to = this.Order.BuyerCustomerParty.Party.Contact.ElectronicMail;

      MailMessage message = new MailMessage(from, to)
      {
        Subject = MessageSubject,
        Body = MessageBody,
        IsBodyHtml = true
      };

      return message;
    }
  }
}