// -------------------------------------------------------------------------------------------
// <copyright file="PriceMatrix.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.PriceMatrix
{
  using System;
  using System.IO;
  using System.Xml.Serialization;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// IPriceMatrixItem contains sites.
  /// </summary>
  [Serializable]
  public class PriceMatrix
  {
    /// <summary>
    /// </summary>
    [XmlElement("structure", typeof(Category))]
    public Category MainCategory;

    /// <summary>
    /// </summary>
    /// <param name="mainCategory">
    /// </param>
    public PriceMatrix(Category mainCategory)
    {
      this.MainCategory = mainCategory;
    }

    /// <summary>
    /// </summary>
    public PriceMatrix()
    {
      this.MainCategory = new Category();
    }

    /// <summary>
    /// </summary>
    /// <param name="category">
    /// </param>
    public void AddCategory(Category category)
    {
      category.Parent = this.MainCategory;
      this.MainCategory.AddCategory(category);
    }

    /// <summary>
    /// </summary>
    /// <param name="categoryItem">
    /// </param>
    public void AddItem(CategoryItem categoryItem)
    {
      categoryItem.Parent = this.MainCategory;
      this.MainCategory.AddItem(categoryItem);
    }

    /// <summary>
    /// </summary>
    /// <param name="query">
    /// </param>
    /// <returns>
    /// </returns>
    public CategoryItem GetDefaultPriceFromCategory(string query)
    {
      IPriceMatrixItem priceMatrixItem = this.SelectSingleItem(query);
      var category = (Category)priceMatrixItem;

      Item priceMatrix = Sitecore.Context.Database.SelectSingleItem("/*/system/Modules/*[@@templatekey='configuration']").Children["PriceMatrix"];
      if (priceMatrix != null)
      {
        Item currentPriceItem = priceMatrix.Axes.SelectSingleItem(query);
        if (currentPriceItem != null)
        {
          string id = currentPriceItem["defaultPrice"];
          Item priceItem = null;
          if (!string.IsNullOrEmpty(id))
          {
            priceItem = priceMatrix.Database.GetItem(id);
          }
          else
          {
            if (currentPriceItem.Children.Count > 0)
            {
              priceItem = currentPriceItem.Children[0];
            }
          }

          if (priceItem != null)
          {
            string priceName = priceItem.Name;
            return category.GetItem(priceName);
          }
        }
      }

      return null;
    }

    /// <summary>
    /// Query language:
    /// "./name/name"
    /// </summary>
    /// <param name="query">
    /// </param>
    /// <returns>
    /// </returns>
    public IPriceMatrixItem SelectSingleItem(string query)
    {
      string[] axes = query.Split('/');
      IPriceMatrixItem current = null;
      for (int i = 0; i < axes.Length; i++)
      {
        string name = axes[i];
        if (!(i == 0 && name.Contains(".")))
        {
          if (!string.IsNullOrEmpty(name))
          {
            current = this.GetElement(current, axes[i]);
            if (current == null)
            {
              return null;
            }
          }
          else
          {
            return null;
          }
        }
      }

      return current;
    }

    /// <summary>
    /// </summary>
    /// <param name="priceMatrixItem">
    /// </param>
    /// <param name="id">
    /// </param>
    /// <returns>
    /// </returns>
    public IPriceMatrixItem GetElement(IPriceMatrixItem priceMatrixItem, string id)
    {
      if (priceMatrixItem == null)
      {
        priceMatrixItem = this.MainCategory;
      }

      if (priceMatrixItem is Category)
      {
        Category category = this.GetCategory(priceMatrixItem, id);
        if (category != null)
        {
          return category;
        }

        return this.GetItem(priceMatrixItem, id);
      }

      return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="id">
    /// </param>
    /// <returns>
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
    /// </summary>
    /// <param name="item">
    /// </param>
    /// <param name="id">
    /// </param>
    /// <returns>
    /// </returns>
    private Category GetCategory(IPriceMatrixItem item, string id)
    {
      if (item is Category)
      {
        return ((Category)item).GetCategory(id);
      }

      return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="id">
    /// </param>
    /// <returns>
    /// </returns>
    public Category GetCategory(string id)
    {
      return this.MainCategory.GetCategory(id);
    }

    /// <summary>
    /// </summary>
    /// <param name="item">
    /// </param>
    /// <param name="id">
    /// </param>
    /// <returns>
    /// </returns>
    private CategoryItem GetItem(IPriceMatrixItem item, string id)
    {
      if (item is Category)
      {
        return ((Category)item).GetItem(id);
      }

      return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="id">
    /// </param>
    /// <returns>
    /// </returns>
    public CategoryItem GetItem(string id)
    {
      return this.MainCategory.GetItem(id);
    }

    /// <summary>
    /// </summary>
    /// <param name="xml">
    /// </param>
    /// <returns>
    /// </returns>
    public static PriceMatrix Load(string xml)
    {
      if (string.IsNullOrEmpty(xml))
      {
        return new PriceMatrix();
      }

      var serializer = new XmlSerializer(typeof(PriceMatrix));
      TextReader tr = new StringReader(xml);
      PriceMatrix priceMatrixItem = null;
      try
      {
        priceMatrixItem = (PriceMatrix)serializer.Deserialize(tr);
      }
      catch (InvalidOperationException)
      {
      }
      finally
      {
        tr.Close();
      }

      return priceMatrixItem;
    }

    /*public int Remove(string idsToKeep)
        {
            string[] ids = idsToKeep.Split('|');
            string removeIds = "";
            for (int i = 0; i < SiteItems.Count; i++)
            {
                Category site = SiteItems[i];
                if (!idsToKeep.Contains(site.Id))
                {
                    removeIds += site.Id + "|";
                }
            }
            ids = removeIds.Split('|');
            foreach (string id in ids)
            {
                SiteItems.Remove(GetSite(id));
            }
            return ids.Length;
        }*/
  }
}