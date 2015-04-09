// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryableContainerExtension.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Queryable container extension. Hooks into the Registering Event of the
//   Context Class that fires each time a type is registered with the container.
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

namespace Sitecore.Ecommerce.Unity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.Practices.Unity;

  /// <summary>
  /// Queryable container extension. Hooks into the Registering Event of the 
  /// Context Class that fires each time a type is registered with the container.
  /// </summary>
  public class QueryableContainerExtension : UnityContainerExtension
  {
    /// <summary>
    /// Initializes static members of the <see cref="QueryableContainerExtension"/> class.
    /// </summary>
    static QueryableContainerExtension()
    {
      Instances = new List<RegisterInstanceEventArgs>();
    }

    /// <summary>
    /// Gets or sets the instances.
    /// </summary>
    /// <value>The instances.</value>
    public static List<RegisterInstanceEventArgs> Instances { get; set; }

    /// <summary>
    /// Determines whether is type registered.
    /// </summary>
    /// <param name="name">The instance name.</param>
    /// <typeparam name="T">The interface.</typeparam>
    /// <returns><c>true</c> if is type registere]; otherwise, <c>false</c>.</returns>
    public virtual bool IsInstanceRegistered<T>(string name)
    {
      return Instances.FirstOrDefault(e => e.RegisteredType == typeof(T)
                                        && string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase)) != null;
    }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    protected override void Initialize()
    {
      this.Context.RegisteringInstance += this.ContextRegisteringInstance;
    }

    /// <summary>
    /// Handles the RegisteringInstance event of the Context control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="Microsoft.Practices.Unity.RegisterInstanceEventArgs"/> instance containing the event data.</param>
    protected virtual void ContextRegisteringInstance(object sender, RegisterInstanceEventArgs e)
    {
      Instances.Add(e);
    }
  }
}