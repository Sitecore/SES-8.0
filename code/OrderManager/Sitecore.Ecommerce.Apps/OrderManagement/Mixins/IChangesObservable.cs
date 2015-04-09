// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChangesObservable.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the IChangesObservable generic type.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement.Mixins
{
  using System.Collections;
  using System.Collections.Generic;

  /// <summary>
  /// Represents the intreface that all OrderTaskFlowButtonsViews must implement.
  /// </summary>
  /// <typeparam name="T">The Ubl entity.</typeparam>
  public interface IChangesObservable<T>
  {
    /// <summary>
    /// Gets the tracked entities.
    /// </summary>
    /// <value>The tracked entities.</value>
    IEnumerable<T> TrackedEntities { get; }

    /// <summary>
    /// Gets the order changes.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>The entity changes.</returns>
    [NotNull]
    IDictionary GetEntityChanges([NotNull] T entity);
  }
}
