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

namespace Sitecore.Ecommerce.Configurations
{
  using Data;
  using DomainModel.Data;

  /// <summary>
  /// The design settings container class.
  /// </summary>
  public class DesignSettings : DomainModel.Configurations.DesignSettings, IEntity
  {
    /// <summary>
    /// Gets or sets the width of the one column.
    /// </summary>
    /// <value>The width of the one column.</value>
    [Entity(FieldName = "One Column Width")]
    public override int OneColumnWidth { get; set; }

    /// <summary>
    /// Gets or sets the width of the two column.
    /// </summary>
    /// <value>The width of the two column.</value>
    [Entity(FieldName = "Two Column Width")]
    public override int TwoColumnWidth { get; set; }

    /// <summary>
    /// Gets or sets the width of the three column.
    /// </summary>
    /// <value>The width of the three column.</value>
    [Entity(FieldName = "Three Column Width")]
    public override int ThreeColumnWidth { get; set; }

    /// <summary>
    /// Gets or sets the width of the four column.
    /// </summary>
    /// <value>The width of the four column.</value>
    [Entity(FieldName = "Four Column Width")]
    public override int FourColumnWidth { get; set; }

    /// <summary>
    /// Gets or sets the height of the two column.
    /// </summary>
    /// <value>The height of the two column.</value>
    [Entity(FieldName = "Two Column Height")]
    public override int TwoColumnHeight { get; set; }

    /// <summary>
    /// Gets or sets the height of the three column.
    /// </summary>
    /// <value>The height of the three column.</value>
    [Entity(FieldName = "Three Column Height")]
    public override int ThreeColumnHeight { get; set; }

    /// <summary>
    /// Gets or sets the height of the four column.
    /// </summary>
    /// <value>The height of the four column.</value>
    [Entity(FieldName = "Four Column Height")]
    public override int FourColumnHeight { get; set; }

    /// <summary>
    /// Gets or sets the product image max heigth.
    /// </summary>
    /// <value>The product image max heigth.</value>
    [Entity(FieldName = "Product Image Max Heigth")]
    public override int ProductImageMaxHeigth { get; set; }

    /// <summary>
    /// Gets or sets the width of the product image max.
    /// </summary>
    /// <value>The width of the product image max.</value>
    [Entity(FieldName = "Product Image Max Width")]
    public override int ProductImageMaxWidth { get; set; }

    /// <summary>
    /// Gets or sets the product image thumbnail heigth.
    /// </summary>
    /// <value>The product image thumbnail heigth.</value>
    [Entity(FieldName = "Product Image Thumbnail Heigth")]
    public override int ProductImageThumbnailHeigth { get; set; }

    /// <summary>
    /// Gets or sets the width of the product image thumbnail.
    /// </summary>
    /// <value>The width of the product image thumbnail.</value>
    [Entity(FieldName = "Product Image Thumbnail Width")]
    public override int ProductImageThumbnailWidth { get; set; }

    /// <summary>
    /// Gets or sets the color of the product image background.
    /// </summary>
    /// <value>The color of the product image background.</value>
    [Entity(FieldName = "Product Image Background Color")]
    public override string ProductImageBackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the width of the product info image.
    /// </summary>
    /// <value>The width of the product info image.</value>
    [Entity(FieldName = "Product Info Image Width")]
    public override int ProductInfoImageWidth { get; set; }

    /// <summary>
    /// Gets or sets the width of the product info thumbnail image.
    /// </summary>
    /// <value>The width of the product info thumbnail image.</value>
    [Entity(FieldName = "Product Info Thumbnail Image Width")]
    public override int ProductInfoThumbnailImageWidth { get; set; }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public string Alias { get; set; }

    #endregion
  }
}