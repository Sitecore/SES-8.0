// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderManagerPostStep.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the order manager post step class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Install
{
  using System.Collections.Specialized;
  using Ecommerce.Install.Localization;
  using Sitecore.Install.Framework;

  /// <summary>
  /// Defines the order manager post step class.
  /// </summary>
  public class OrderManagerPostStep : IPostStep
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderManagerPostStep"/> class.
    /// </summary>
    public OrderManagerPostStep()
    {
      this.ResetClonedColumnHeadersPostStep = new ResetClonedDetailListHeadersPostStep();
      this.ResetSampleUserPasswordsPostStep = new ResetSampleUserPaswordPostStep();
      this.LocalizationPostStep = new LocalizationPostStep();
    }

    /// <summary>
    /// Gets or sets the reset clonned headers post step.
    /// </summary>
    /// <value>
    /// The reset clonned headers post step.
    /// </value>
    [NotNull]
    public ResetClonedDetailListHeadersPostStep ResetClonedColumnHeadersPostStep { get; set; }

    /// <summary>
    /// Gets or sets the reset passwords post step.
    /// </summary>
    /// <value>
    /// The reset passwords post step.
    /// </value>
    [NotNull]
    public ResetSampleUserPaswordPostStep ResetSampleUserPasswordsPostStep { get; set; }

    /// <summary>
    /// Gets or sets the om localization post step.
    /// </summary>
    /// <value>
    /// The om localization post step.
    /// </value>
    [NotNull]
    public LocalizationPostStep LocalizationPostStep { get; set; }

    /// <summary>
    /// Runs this post step
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="metaData">The meta data.</param>
    public void Run(ITaskOutput output, NameValueCollection metaData)
    {
      // this.ResetClonedColumnHeadersPostStep.Run(output, metaData);
      this.ResetSampleUserPasswordsPostStep.Run(output, metaData);
      this.LocalizationPostStep.Run(output, metaData);
    }
  }
}