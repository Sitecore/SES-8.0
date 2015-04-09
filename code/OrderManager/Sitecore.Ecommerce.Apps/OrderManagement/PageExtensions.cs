// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the page extensions class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement
{
  using System.Web.UI;
  using Diagnostics;
  using Sitecore.Web.UI;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the page extensions class.
  /// </summary>
  public static class PageExtensions
  {
    /// <summary>
    /// Shows the sticky info message.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="text">The text.</param>
    /// <param name="args">The arguments.</param>
    public static void ShowStickyInfoMessage([NotNull] this Page page, [NotNull] string text, [CanBeNull] params object[] args)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(text, "text");

      page.ShowMessage(MessageType.Info, true, text, args);
    }

    /// <summary>
    /// Shows the non sticky info message.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="text">The text.</param>
    /// <param name="args">The arguments.</param>
    public static void ShowNonStickyInfoMessage([NotNull] this Page page, [NotNull] string text, [CanBeNull] params object[] args)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(text, "text");

      page.ShowMessage(MessageType.Info, false, text, args);
    }

    /// <summary>
    /// Shows the sticky warning message.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="text">The text.</param>
    /// <param name="args">The arguments.</param>
    public static void ShowStickyWarningMessage([NotNull] this Page page, [NotNull] string text, [CanBeNull] params object[] args)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(text, "text");

      page.ShowMessage(MessageType.Warning, true, text, args);
    }

    /// <summary>
    /// Shows the sticky error message.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="text">The text.</param>
    /// <param name="args">The arguments.</param>
    public static void ShowStickyErrorMessage([NotNull] this Page page, [NotNull] string text, [CanBeNull] params object[] args)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(text, "text");

      page.ShowMessage(MessageType.Error, true, text, args);
    }

    /// <summary>
    /// Shows the non sticky error message.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="text">The text.</param>
    /// <param name="args">The arguments.</param>
    public static void ShowNonStickyErrorMessage([NotNull] this Page page, [NotNull] string text, [CanBeNull] params object[] args)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(text, "text");

      page.ShowMessage(MessageType.Error, false, text, args);
    }

    /// <summary>
    /// Shows the message.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="sticky">if set to <c>true</c> [sticky].</param>
    /// <param name="text">The text.</param>
    /// <param name="args">The arguments.</param>
    public static void ShowMessage([NotNull] this Page page, MessageType messageType, bool sticky, [NotNull] string text, [CanBeNull] params object[] args)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(text, "text");
      Assert.ArgumentNotNullOrEmpty(text, "text");

      ScriptManager scriptManager = ScriptManager.GetCurrent(page);
      Assert.IsNotNull(scriptManager, "Script Manager cannot be null.");

      string formattedText = args == null ? text : string.Format(text, args);
      Message message = new Message(formattedText)
      {
        Sticky = sticky,
        Type = messageType
      };

      scriptManager.Message(message);
    }

    /// <summary>
    /// Shows the confirmation dialog.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="message">The message.</param>
    /// <param name="description">The description.</param>
    /// <returns>
    /// The confirmation dialog.
    /// </returns>
    public static bool ShowConfirmationDialog([NotNull] this Page page, [NotNull] string message, [NotNull] string description)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(message, "message");
      Assert.ArgumentNotNull(description, "description");

      return MessageBox.Show(message, description, null, null, page) == DialogResult.Yes;
    }

    /// <summary>
    /// Registers the stylesheet.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="key">The key.</param>
    /// <param name="styleSheetUrl">The style sheet URL.</param>
    public static void RegisterStylesheet([NotNull] this Page page, [NotNull] string key, [NotNull] string styleSheetUrl)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNullOrEmpty(key, "key");
      Assert.ArgumentNotNull(styleSheetUrl, "styleSheetUrl");

      if (page.Header.FindControl(key) == null)
      {
        LiteralControl literalControl = new LiteralControl
        {
          ID = key,
          Text = string.Format("<link rel=\"stylesheet\" type=\"text/css\" media=\"screen\" href=\"{0}\" />", styleSheetUrl)
        };
        page.Header.Controls.Add(literalControl);
      }
    }
  }
}