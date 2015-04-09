// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultTotalsFactory.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the default implementation of totals factory.
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

namespace Sitecore.Ecommerce.Prices
{
  using Diagnostics;
  using DomainModel.Prices;
  using Microsoft.Practices.Unity;

  /// <summary>
  /// Defines the default implementation of totals factory.
  /// </summary>
  public class DefaultTotalsFactory : TotalsFactory
  {
    /// <summary>
    /// Defines referenct to IIoCContainer being used.
    /// </summary>
    private readonly IUnityContainer container;

    /// <summary>
    /// Initializes a new instance of the totals class.
    /// </summary>
    /// <param name="container">The container.</param>
    public DefaultTotalsFactory([NotNull] IUnityContainer container)
    {
      Assert.ArgumentNotNull(container, "container");
      
      this.container = container;
    }

    /// <summary>
    /// Gets the container.
    /// </summary>
    [NotNull]
    protected IUnityContainer Container
    {
      get { return this.container; }
    }

    /// <summary>
    /// Creates instance of Totals type.
    /// </summary>
    /// <returns>
    /// The created instance of Totals type.
    /// </returns>
    public override DomainModel.Prices.Totals Create()
    {
      return this.Container.Resolve<DomainModel.Prices.Totals>();
    }
  }
}
