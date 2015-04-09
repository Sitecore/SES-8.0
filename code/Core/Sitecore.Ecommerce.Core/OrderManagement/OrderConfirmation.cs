// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderConfirmation.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order confirmation class.
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
  using Mail;

  /// <summary>
  /// Defines the order confirmation class.
  /// </summary>
  public class OrderConfirmation
  {
    /// <summary>
    /// The mail service.
    /// </summary>
    private MailService mailService;

    /// <summary>
    /// The confirmation text builder.
    /// </summary>
    private ConfirmationMessageBuilder messageBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderConfirmation"/> class.
    /// </summary>
    public OrderConfirmation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderConfirmation"/> class.
    /// </summary>
    /// <param name="mailService">The mail service.</param>
    /// <param name="messageBuilder">The message builder.</param>
    public OrderConfirmation([NotNull] MailService mailService, [NotNull] ConfirmationMessageBuilder messageBuilder)
    {
      Assert.ArgumentNotNull(mailService, "mailService");
      Assert.ArgumentNotNull(messageBuilder, "messageBuilder");

      this.mailService = mailService;
      this.messageBuilder = messageBuilder;
    }

    /// <summary>
    /// Gets or sets the mail service.
    /// </summary>
    /// <value>
    /// The mail service.
    /// </value>
    [CanBeNull]
    public virtual MailService MailService
    {
      get { return this.mailService; }
      set { this.mailService = value; }
    }

    /// <summary>
    /// Gets or sets the confirmation text builder.
    /// </summary>
    /// <value>
    /// The confirmation text builder.
    /// </value>
    [CanBeNull]
    public virtual ConfirmationMessageBuilder ConfirmationMessageBuilder
    {
      get { return this.messageBuilder; }
      set { this.messageBuilder = value; }
    }

    /// <summary>
    /// Sends the confirmation.
    /// </summary>
    public virtual void Send()
    {
      MailMessage mailMessage = this.messageBuilder.GetMessage("Order Confirmation");
      this.mailService.Send(mailMessage);
    }
  }
}