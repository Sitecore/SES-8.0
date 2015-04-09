// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartLine.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the shopping cart line class.
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

namespace Sitecore.Ecommerce.Carts
{
  using System;
  using Catalogs;
  using DomainModel.Prices;
  using DomainModel.Products;
  using Products;
  using Resources.Media;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Text;
  using Utils;
  using Validators.Interception;

  /// <summary>
  /// Defines the shopping cart line class.
  /// </summary>
  [Serializable]
  public class ShoppingCartLine : DomainModel.Carts.ShoppingCartLine
  {
    /// <summary>
    /// The frient url.
    /// </summary>
    private string friendlyUrl;

    /// <summary>
    /// The image url.
    /// </summary>
    private string imageUrl;

    /// <summary>
    /// The product.
    /// </summary>
    private ProductBaseData product;

    /// <summary>
    /// The product repository.
    /// </summary>
    [NonSerialized]
    private IProductRepository productRepository;

    /// <summary>
    /// Gets or sets the product.
    /// </summary>
    /// <value>The product.</value>
    [CanBeNull]
    public override ProductBaseData Product
    {
      get
      {
        if (this.product != null && !string.IsNullOrEmpty(this.product.Code) && (this.ProductRepository != null))
        {
          this.product = this.ProductRepository.Get<ProductBaseData>(this.product.Code);
        }

        return this.product;
      }

      [NotNullValue]
      set
      {
        this.product = value;
      }
    }

    /// <summary>
    /// Gets or sets the product repository.
    /// </summary>
    /// <value>
    /// The product repository.
    /// </value>
    [NotNull]
    public IProductRepository ProductRepository
    {
      get
      {
        return this.productRepository;
      }

      set
      {
        this.productRepository = value;
      }
    }

    /// <summary>
    /// Gets or sets the totals.
    /// </summary>
    /// <value>The totals.</value>
    [NotNull]
    public override Totals Totals { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the friendly URL.
    /// </summary>
    /// <value>The friendly URL.</value>
    [NotNull]
    public override string FriendlyUrl
    {
      get
      {
        if (this.Product == null)
        {
          return string.Empty;
        }

        Item productItem = ProductRepositoryUtil.GetRepositoryItem(this.Product);
        Item contextItem = Context.Entity.Resolve<VirtualProductResolver>().ProductCatalogItem ?? Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);

        if (productItem == null || contextItem == null)
        {
          return string.Empty;
        }

        this.friendlyUrl = Context.Entity.Resolve<VirtualProductResolver>().GetVirtualProductUrl(contextItem, productItem);

        return this.friendlyUrl;
      }

      [NotNullValue]
      set
      {
        this.friendlyUrl = value;
      }
    }

    /// <summary>
    /// Gets or sets the image URL.
    /// </summary>
    /// <value>The image URL.</value>
    [NotNull]
    public override string ImageUrl
    {
      get
      {
        if (!(this.Product is Product))
        {
          return this.imageUrl;
        }

        Product product = (Product)this.Product;

        Item productItem = ProductRepositoryUtil.GetRepositoryItem(this.Product);

        if (productItem != null && !string.IsNullOrEmpty(product.ImageUrl))
        {
          ListString imagesIds = new ListString(product.ImageUrl);
          if (imagesIds.Count == 0 || !ID.IsID(imagesIds[0]))
          {
            return string.Empty;
          }

          // Get first image. Why exactly the first one?   
          MediaUrlOptions options = new MediaUrlOptions { AbsolutePath = true };
          MediaItem mediaItem = productItem.Database.GetItem(imagesIds[0]);

          var cleanUrl = MediaManager.GetMediaUrl(mediaItem, options);
          var hashedUrl = HashingUtils.ProtectAssetUrl(cleanUrl);
          this.imageUrl = hashedUrl; 

          return this.imageUrl;
        }

        return string.Empty;
      }

      [NotNullValue]
      set
      {
        this.imageUrl = value;
      }
    }
  }
}