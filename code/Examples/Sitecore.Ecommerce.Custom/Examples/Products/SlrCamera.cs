﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SlrCamera.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SLR camera class.
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

namespace Sitecore.Ecommerce.Examples.Products
{
  using System;
  using Ecommerce.Products;

  /// <summary>
  /// Defines the SLR camera class.
  /// </summary>
  [Serializable]
  public class SlrCamera : Product
  {
    /// <summary>
    /// Gets or sets the effective pixels.
    /// </summary>
    /// <value>
    /// The effective pixels.
    /// </value>
    public string EffectivePixels { get; set; }

    /// <summary>
    /// Gets or sets the image sensor.
    /// </summary>
    /// <value>
    /// The image sensor.
    /// </value>
    public string ImageSensor { get; set; }
  }
}