// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveOrdersException.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrdersSaveException type.
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
  using System;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Defines the orders save exception class.
  /// </summary>
  [Serializable]
  public class SaveOrdersException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveOrdersException"/> class.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public SaveOrdersException([NotNull] Exception innerException)
      : base("There was an error during orders save operation.", innerException)
    {
      Assert.ArgumentNotNull(innerException, "innerException");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveOrdersException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public SaveOrdersException([NotNull] string message, [NotNull] Exception innerException)
      : base(message, innerException)
    {
      Assert.ArgumentNotNull(message, "message");
      Assert.ArgumentNotNull(innerException, "innerException");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveOrdersException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public SaveOrdersException([NotNull] string message)
      : base(message)
    {
      Assert.ArgumentNotNull(message, "message");
    }
  }
}
