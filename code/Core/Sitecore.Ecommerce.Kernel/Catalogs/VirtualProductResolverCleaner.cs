// -------------------------------------------------------------------------------------------
// <copyright file="VirtualProductResolverCleaner.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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

namespace Sitecore.Ecommerce.Catalogs
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using Diagnostics;
  using Events;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// The virtual product resolver cleaner.
  /// </summary>
  public class VirtualProductResolverCleaner
  {
    /// <summary>
    /// Cleans the VirtualProductResolver caches.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public void OnItemSaved(object sender, EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      Item item = Event.ExtractParameter(args, 0) as Item;

      if (item == null)
      {
        return;
      }

      var virtualProductResolver = Context.Entity.Resolve<VirtualProductResolver>();
      Assert.IsNotNull(virtualProductResolver, "Virtual product resolver is null");

      lock (virtualProductResolver.ProductsUrlsCollection.SyncRoot)
      {
        if (virtualProductResolver.ProductsUrlsCollection == null || virtualProductResolver.ProductsUrlsCollection.Count <= 0)
        {
          return;
        }

        var keysToRemove = new List<string>();

        foreach (DictionaryEntry urlsCollection in virtualProductResolver.ProductsUrlsCollection)
        {
          if (!(urlsCollection.Value is ProductUriLine))
          {
            continue;
          }

          var productUriLine = (ProductUriLine)urlsCollection.Value;

          var productUriValues = new[]
                                 {
                                   productUriLine.ProductCatalogItemUri, 
                                   productUriLine.ProductItemUri, 
                                   productUriLine.ProductPresentationItemUri
                                 };

          foreach (var productUriValue in productUriValues)
          {
            if (string.IsNullOrEmpty(productUriValue) || !ItemUri.IsItemUri(productUriValue))
            {
              continue;
            }

            var productUri = ItemUri.Parse(productUriValue);
            if (productUri.ItemID.Equals(item.ID))
            {
              keysToRemove.Add(urlsCollection.Key as string);
              break;
            }
          }
        }

        keysToRemove.ForEach(k => virtualProductResolver.ProductsUrlsCollection.Remove(k));
      }
    }
  }
}