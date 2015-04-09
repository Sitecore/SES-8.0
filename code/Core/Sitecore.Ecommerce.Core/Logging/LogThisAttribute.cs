// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogThisAttribute.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the LogThisAttribute type.
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
  using System;
  using Diagnostics;
  using Microsoft.Practices.Unity;
  using Microsoft.Practices.Unity.InterceptionExtension;

  /// <summary>
  /// Defines the log this attribute class.
  /// </summary>
  public class LogThisAttribute : HandlerAttribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LogThisAttribute"/> class.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="levelCode">The level code.</param>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">action</exception>
    public LogThisAttribute([NotNull] string action, [NotNull] string levelCode)
    {
      Assert.ArgumentNotNull(action, "action");
      if (string.IsNullOrEmpty(action))
      {
        throw new ArgumentException("Value cannot be empty.", "action");
      }

      Assert.ArgumentNotNull(levelCode, "levelCode");
      if (string.IsNullOrEmpty(levelCode))
      {
        throw new ArgumentException("Value cannot be empty.", "levelCode");
      }

      this.Action = action;
      this.LevelCode = levelCode;
    }

    /// <summary>
    /// Gets the action.
    /// </summary>
    [NotNull]
    public string Action { get; [NotNull] private set; }

    /// <summary>
    /// Gets the action.
    /// </summary>
    [NotNull]
    public string LevelCode { get; [NotNull] private set; }

    /// <summary>
    /// Derived classes implement this method. When called, it
    /// creates a new call handler as specified in the attribute
    /// configuration.
    /// </summary>
    /// <param name="container">The <see cref="T:Microsoft.Practices.Unity.IUnityContainer"/> to use when creating handlers,
    /// if necessary.</param>
    /// <returns>
    /// A new call handler object.
    /// </returns>
    [NotNull]
    public override ICallHandler CreateHandler([NotNull] IUnityContainer container)
    {
      Assert.ArgumentNotNull(container, "container");

      return container.Resolve<LoggingHandler>();
    }
  }
}