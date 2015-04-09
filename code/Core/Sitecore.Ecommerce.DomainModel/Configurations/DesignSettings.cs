// -------------------------------------------------------------------------------------------
// <copyright file="DesignSettings.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.DomainModel.Configurations
{
  /// <summary>
  /// The business catalog settings abstract class
  /// </summary>
  public class DesignSettings
  {
    /// <summary>
    /// Gets or sets the width of the one col.
    /// </summary>
    /// <value>The width of the one col.</value>
    public virtual int OneColumnWidth { get; set; }

    /// <summary>
    /// Gets or sets the width of the two col.
    /// </summary>
    /// <value>The width of the two col.</value>
    public virtual int TwoColumnWidth { get; set; }

    /// <summary>
    /// Gets or sets the width of the three col.
    /// </summary>
    /// <value>The width of the three col.</value>
    public virtual int ThreeColumnWidth { get; set; }

    /// <summary>
    /// Gets or sets the width of the four col.
    /// </summary>
    /// <value>The width of the four col.</value>
    public virtual int FourColumnWidth { get; set; }

    /// <summary>
    /// Gets or sets the height of the two col.
    /// </summary>
    /// <value>The height of the two col.</value>
    public virtual int TwoColumnHeight { get; set; }

    /// <summary>
    /// Gets or sets the height of the three col.
    /// </summary>
    /// <value>The height of the three col.</value>
    public virtual int ThreeColumnHeight { get; set; }

    /// <summary>
    /// Gets or sets the height of the four col.
    /// </summary>
    /// <value>The height of the four col.</value>
    public virtual int FourColumnHeight { get; set; }

    /// <summary>
    /// Gets or sets the product image max heigth.
    /// </summary>
    /// <value>The product image max heigth.</value>
    public virtual int ProductImageMaxHeigth { get; set; }

    /// <summary>
    /// Gets or sets the width of the product image max.
    /// </summary>
    /// <value>The width of the product image max.</value>
    public virtual int ProductImageMaxWidth { get; set; }

    /// <summary>
    /// Gets or sets the product image thumbnail heigth.
    /// </summary>
    /// <value>The product image thumbnail heigth.</value>
    public virtual int ProductImageThumbnailHeigth { get; set; }

    /// <summary>
    /// Gets or sets the width of the product image thumbnail.
    /// </summary>
    /// <value>The width of the product image thumbnail.</value>
    public virtual int ProductImageThumbnailWidth { get; set; }

    /// <summary>
    /// Gets or sets the color of the product image background.
    /// </summary>
    /// <value>The color of the product image background.</value>
    public virtual string ProductImageBackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the width of the product info image.
    /// </summary>
    /// <value>The width of the product info image.</value>
    public virtual int ProductInfoImageWidth { get; set; }

    /// <summary>
    /// Gets or sets the width of the product info thumbnail image.
    /// </summary>
    /// <value>The width of the product info thumbnail image.</value>
    public virtual int ProductInfoThumbnailImageWidth { get; set; }
  }
}