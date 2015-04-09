// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIoCContainer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The IoC container.
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

namespace Sitecore.Ecommerce
{
  using Microsoft.Practices.Unity;

  /// <summary>
  /// The IoC container.
  /// </summary>
  public interface IIoCContainer : IUnityContainer
  {
    /// <summary>
    /// Gets the entity instance.
    /// </summary>
    /// <typeparam name="T">The entity contract.</typeparam>
    /// <returns>The entity instance.</returns>
    T GetConfiguration<T>() where T : class;

    /// <summary>
    /// Sets the entity instance.
    /// </summary>
    /// <param name="entity">The entity instance.</param>
    /// <typeparam name="T">The entity contract.</typeparam>
    void SetInstance<T>(T entity);

    /// <summary>
    /// Gets the entity instance.
    /// </summary>
    /// <typeparam name="T">The entity contract.</typeparam>
    /// <returns>The entity instance.</returns>
    T GetInstance<T>();
  }
}