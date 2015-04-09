// -------------------------------------------------------------------------------------------
// <copyright file="NotNullValueAttribute.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Validators.Interception
{
  using System;
  using Diagnostics;
  using Microsoft.Practices.Unity;
  using Microsoft.Practices.Unity.InterceptionExtension;

  /// <summary>
  /// The email validation attribute.
  /// </summary>
  public class NotNullValueAttribute : HandlerAttribute, ICallHandler
  {
    #region Overrides of HandlerAttribute

    /// <summary>
    /// Creates the handler.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <returns>The handler.</returns>
    public override ICallHandler CreateHandler(IUnityContainer container)
    {
      return new NotNullValueAttribute();
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
        object target = argument;

        if (target != null)
        {
          continue;
        }

        ArgumentNullException argumentException = new ArgumentNullException(input.MethodBase.Name);
        
        Log.Error("Argument null exception", argumentException, this);
        
        return input.CreateExceptionMethodReturn(argumentException);
      }

      return getNext()(input, getNext);
    }

    #endregion
  }
}