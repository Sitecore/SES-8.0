// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateRequestContainer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the create request container class.
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
  using Sitecore.Pipelines;

  /// <summary>
  /// Defines the create request container class.
  /// </summary>
  public class CreateRequestContainer
  {
    /// <summary>
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public void Process([NotNull] PipelineArgs args)
    {
      var requestContainer = Context.AppContainer.CreateChildContainer();

      Sitecore.Context.Items[Context.RequestContainerKey] = new IoCContainer(requestContainer);
    }
  }
}