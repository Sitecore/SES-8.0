// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PipelineWrapper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the pipeline wrapper class.
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

namespace Sitecore.Pipelines
{
  /// <summary>
  /// Defines the pipeline wrapper class.
  /// </summary>
  public class PipelineWrapper
  {
    /// <summary>
    /// Starts the Sitecore pipeline.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="pipelineArgs">The pipeline args.</param>
    public virtual void Start(string name, PipelineArgs pipelineArgs)
    {
      Pipeline.Start(name, pipelineArgs);
    }
  }
}