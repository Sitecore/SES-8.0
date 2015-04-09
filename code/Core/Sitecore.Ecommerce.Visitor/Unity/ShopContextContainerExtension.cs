// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopContextContainerExtension.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
// Defines the site context container extension class.   
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

namespace Sitecore.Ecommerce.Visitor.Unity
{
  using Diagnostics;
  using Microsoft.Practices.ObjectBuilder2;
  using Microsoft.Practices.Unity;
  using Microsoft.Practices.Unity.ObjectBuilder;
  using Sites;

  /// <summary>
  /// Defines the site context container extension class.
  /// </summary>
  public class ShopContextContainerExtension : UnityContainerExtension
  {
    /// <summary>
    ///  VisitorShopResolver instance.
    /// </summary>
    private VisitorShopResolver resolver;

    /// <summary>
    /// Gets or sets the resolver.
    /// </summary>
    /// <value>
    /// The resolver.
    /// </value>
    [NotNull]
    public virtual VisitorShopResolver Resolver
    {
      get
      {
        return this.resolver ?? (this.resolver = new VisitorShopResolver());
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.resolver = value;
      }
    }

    /// <summary>
    /// Initial the container with this extension's functionality.
    /// </summary>
    protected override void Initialize()
    {
      var strategy = new ContextShopBuilderStrategy(this.Resolver);

      Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
    }

    /// <summary>
    /// Defines the context shop builder strategy class.
    /// </summary>
    protected class ContextShopBuilderStrategy : BuilderStrategy
    {
      /// <summary>
      /// VisitorShopResolver instance.
      /// </summary>
      private readonly VisitorShopResolver resolver;

      /// <summary>
      /// Initializes a new instance of the <see cref="ContextShopBuilderStrategy"/> class.
      /// </summary>
      /// <param name="resolver">The resolver.</param>
      public ContextShopBuilderStrategy([NotNull] VisitorShopResolver resolver)
      {
        Assert.ArgumentNotNull(resolver, "resolver");

        this.resolver = resolver;
      }

      /// <summary>
      /// Called during the chain of responsibility for a build operation. The
      /// PreBuildUp method is called when the chain is being executed in the
      /// forward direction.
      /// </summary>
      /// <param name="context">Context of the build operation.</param>
      public override void PreBuildUp([NotNull] IBuilderContext context)
      {
        Assert.ArgumentNotNull(context, "context");

        var key = context.OriginalBuildKey;

        if (key.Type == typeof(ShopContext))
        {
          context.Existing = this.resolver.GetShopContext(Sitecore.Context.Site);
        }
      }
    }
  }
}