// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisposeRequestContainer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the dispose request container class.
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

namespace Sitecore.Ecommerce.Pipelines.HttpRequest
{
  using System;
  using Sitecore.Pipelines;

  /// <summary>
  /// Defines the dispose request container class.
  /// </summary>
  public class DisposeRequestContainer
  {
    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public void Process(PipelineArgs args)
    {
      var container = Sitecore.Context.Items[Context.RequestContainerKey] as IDisposable;
      if (container == null)
      {
        return;
      }

      container.Dispose();
      Sitecore.Context.Items.Remove(Context.RequestContainerKey);
    }
  }
}