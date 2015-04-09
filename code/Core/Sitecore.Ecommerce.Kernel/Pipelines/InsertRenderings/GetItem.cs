// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetItem.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the GetItem type.
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

namespace Sitecore.Ecommerce.Pipelines.InsertRenderings
{
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.Catalogs;
  using Sitecore.Pipelines.InsertRenderings;

  /// <summary>
  /// The get item processor.
  /// </summary>
  public class GetItem : InsertRenderingsProcessor
  {
    /// <summary>
    /// Processes the specified args.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public override void Process(InsertRenderingsArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      var resolver = Context.Entity.Resolve<VirtualProductResolver>();
      var presentationItem = resolver.ProductPresentationItem;
      args.ContextItem = presentationItem ?? Sitecore.Context.Item;
    }
  }
}