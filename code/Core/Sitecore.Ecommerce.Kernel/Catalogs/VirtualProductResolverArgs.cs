// -------------------------------------------------------------------------------------------
// <copyright file="VirtualProductResolverArgs.cs" company="Sitecore Corporation">
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
  /// <summary>
  /// Virtual Products resplver arguments
  /// </summary>
  public class VirtualProductResolverArgs
  {

    private string displayProductModeField = "Display Products Mode";
    private string productDetailPresentationStorageField = "Product Detail Presentation Storage";
    private string displayProductsModeKeyField = "Key";

    /// <summary>
    /// Gets the display products mode field.
    /// </summary>
    /// <value>The display products mode field id.</value>
    public virtual string DisplayProductsModeField
    {
      get
      {
        return this.displayProductModeField;
      }

      set
      {
        this.displayProductModeField = value;
      }
    }

    /// <summary>
    /// Gets the Product Detail Presentation Storage field.
    /// </summary>
    public virtual string ProductDetailPresentationStorageField
    {
      get
      {
        return this.productDetailPresentationStorageField;
      }
      set
      {
        this.productDetailPresentationStorageField = value;
      }
    }

    /// <summary>
    /// Gets the display products mode key field.
    /// </summary>
    public virtual string DisplayProductsModeKeyField
    {
      get
      {
        return this.displayProductsModeKeyField;
      }
      set
      {
        this.displayProductsModeKeyField = value;
      }
    }
  }
}