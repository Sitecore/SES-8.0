// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Category.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Category class.
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

namespace Sitecore.Ecommerce.PriceMatrix
{
  using System;
  using System.Collections.Generic;
  using System.Xml.Serialization;

  /// <summary>
  /// Site
  /// </summary>
  [Serializable]
  public class Category : IPriceMatrixItem
  {
    // Id
    /// <summary>
    /// </summary>
    [XmlArray("categories")]
    [XmlArrayItem("category", typeof(Category))]
    public List<Category> Categories = new List<Category>();

    /// <summary>
    /// </summary>
    [XmlAttribute("id")]
    public string Id;

    /// <summary>
    /// </summary>
    [XmlArray("items")]
    [XmlArrayItem("item", typeof(CategoryItem))]
    public List<CategoryItem> Items =
      new List<CategoryItem>();

    /// <summary>
    /// </summary>
    [XmlIgnore]
    private Category parent;

    /// <summary>
    /// </summary>
    public Category()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="id">
    /// </param>
    public Category(string id)
    {
      this.Id = id;
    }

    /// <summary>
    /// </summary>
    [XmlIgnore]
    public Category Parent
    {
      get
      {
        return this.parent;
      }

      set
      {
        this.parent = value;
      }
    }

    #region IPriceMatrixItem Members

    /// <summary>
    /// </summary>
    [XmlIgnore]
    public string ID
    {
      get
      {
        return this.Id;
      }

      set
      {
        this.Id = value;
      }
    }

    #endregion

    /// <summary>
    /// Adds <c>category</c> to list
    /// </summary>
    /// <param name="category">
    /// </param>
    public void AddCategory(Category category)
    {
      category.Parent = this;
      this.Categories.Add(category);
    }

    /// <summary>
    /// Adds <c>categoryItem</c> to category items list
    /// </summary>
    /// <param name="categoryItem">
    /// </param>
    public void AddItem(CategoryItem categoryItem)
    {
      categoryItem.Parent = this;
      this.Items.Add(categoryItem);
    }

    /// <summary>
    /// Get's the Sitecore ItemId for category or item
    /// </summary>
    /// <param name="id">
    /// </param>
    /// <returns>
    /// Sitecore ItemId
    /// </returns>
    public IPriceMatrixItem GetElement(string id)
    {
      Category category = this.GetCategory(id);
      if (category != null)
      {
        return category;
      }

      return this.GetItem(id);
    }

    /// <summary>
    /// Get a specific category by it's <c>id</c>
    /// </summary>
    /// <param name="id">
    /// </param>
    /// <returns>
    /// Category item
    /// </returns>
    public Category GetCategory(string id)
    {
      foreach (Category category in this.Categories)
      {
        if (category.Id.ToLower().Equals(id.ToLower()))
        {
          return category;
        }
      }

      return null;
    }

    /// <summary>
    /// Get a specific item in category by it's <c>id</c>
    /// </summary>
    /// <param name="id">
    /// </param>
    /// <returns>
    /// Item in category
    /// </returns>
    public CategoryItem GetItem(string id)
    {
      foreach (CategoryItem categoryItem in this.Items)
      {
        if (categoryItem.Id.ToLower().Equals(id.ToLower()))
        {
          return categoryItem;
        }
      }

      return null;
    }
  }
}