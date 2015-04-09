// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisitorShopResolvingProcessor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the VisitorShopResolvingProcessor type.
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

namespace Sitecore.Ecommerce.Visitor.Pipelines.HttpRequest
{
  using Diagnostics;
  using Microsoft.Practices.Unity;
  using Sitecore.Pipelines;
  using Sitecore.Sites;
  using Sites;

  /// <summary>
  /// Defines the visitor shop resolver class.
  /// </summary>
  public class VisitorShopResolvingProcessor
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
    /// Runs the processor.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void Process([NotNull] PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      SiteContext siteContext = Sitecore.Context.Site;
      Assert.IsNotNull(siteContext, "Unable to resolve shop context. Sitecore context site not set.");

      ShopContext shopContext = this.Resolver.GetShopContext(siteContext);
      if (shopContext == null)
      {
        return;
      }

      Context.Entity.RegisterInstance(typeof(ShopContext), null, shopContext, new HierarchicalLifetimeManager());

      if (Context.ShopIoCContainers.ContainsKey(siteContext.Name))
      {
        // Dispose current container to prevent memory leaks.
        if (Sitecore.Context.Items[Context.RequestContainerKey] is IIoCContainer)
        {
          var currentContainer = Sitecore.Context.Items[Context.RequestContainerKey] as IIoCContainer;
          currentContainer.Dispose();
        }

        // Set new container according to current shop context.
        Sitecore.Context.Items[Context.RequestContainerKey] = new IoCContainer(Context.ShopIoCContainers[siteContext.Name].CreateChildContainer());
      }
    }
  }
}