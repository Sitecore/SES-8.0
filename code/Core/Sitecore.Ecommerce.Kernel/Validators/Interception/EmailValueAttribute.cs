// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailValueAttribute.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The email validation attribute. Checks whether the value set to a property marked with the attribute is the valid email address. 
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

namespace Sitecore.Ecommerce.Validators.Interception
{
  using System;
  using System.Text.RegularExpressions;
  using Diagnostics;
  using Microsoft.Practices.Unity;
  using Microsoft.Practices.Unity.InterceptionExtension;

  /// <summary>
  /// The email validation attribute. Checks whether the value set to a property marked with the attribute is the valid email address. 
  /// </summary>
  public class EmailValueAttribute : HandlerAttribute, ICallHandler
  {
    #region Overrides of HandlerAttribute

    /// <summary>
    /// Creates the handler.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <returns>The handler.</returns>
    public override ICallHandler CreateHandler(IUnityContainer container)
    {
      return new EmailValueAttribute();
    }

    #endregion

    #region Implementation of ICallHandler

    /// <summary>
    /// Invokes the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="getNext">The get next.</param>
    /// <returns>The method return result.</returns>
    public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
    {
      foreach (var argument in input.Arguments)
      {
        string target = argument as string;

        if (string.IsNullOrEmpty(target))
        {
          continue;
        }

        if (Regex.Match(target, @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?").Success)
        {
          continue;
        }

        ArgumentException argumentException = new ArgumentException("Invalid e-mail format", input.MethodBase.Name);

        Log.Error("Argument exception", argumentException, this);

        return input.CreateExceptionMethodReturn(argumentException);
      }

      return getNext()(input, getNext);
    }

    #endregion
  }
}