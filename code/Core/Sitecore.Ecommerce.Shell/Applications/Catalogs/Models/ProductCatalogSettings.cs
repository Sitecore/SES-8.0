// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductCatalogSettings.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ProductCatalogSettings type.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models
{
  using System.Collections.Specialized;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Text;

  /// <summary>
  /// Defines the ProductCatalogSettings type.
  /// </summary>
  public class ProductCatalogSettings
  {
    /// <summary>
    /// Current Item
    /// </summary>
    private Item currentItem;

    /// <summary>
    /// Gets or sets the item URI.
    /// </summary>
    /// <value>The item URI.</value>
    public virtual ItemUri ItemUri { get; set; }

    /// <summary>
    /// Gets or sets the selected search method.
    /// </summary>
    /// <value>The selected search method.</value>
    public virtual ID SelectionMethod
    {
      get
      {
        if (this.CurrentItem != null && ID.IsID(this.CurrentItem["Product Selection Method"]))
        {
          return new ID(this.CurrentItem["Product Selection Method"]);
        }

        return null;
      }

      set
      {
        using (new EditContext(this.CurrentItem))
        {
          this.CurrentItem["Product Selection Method"] = value.ToString();
        }
      }
    }

    /// <summary>
    /// Gets or sets the search text boxes.
    /// </summary>
    /// <value>The search text boxes.</value>
    public virtual NameValueCollection TextBoxes
    {
      get
      {
        return new NameValueCollection(new UrlString(this.CurrentItem["Search Text Boxes"]).Parameters);
      }

      set
      {
        if (value != null)
        {
          using (new EditContext(this.CurrentItem))
          {
            this.CurrentItem["Search Text Boxes"] = new UrlString(value).ToString();
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets the search checklists.
    /// </summary>
    /// <value>The search checklists.</value>
    public virtual NameValueCollection Checklists
    {
      get
      {
        return new NameValueCollection(new UrlString(this.CurrentItem["Search Checklists"]).Parameters);
      }

      set
      {
        if (value != null)
        {
          using (new EditContext(this.CurrentItem))
          {
            this.CurrentItem["Search Checklists"] = new UrlString(value).ToString();
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets the product identifiers.
    /// </summary>
    /// <value>The product identifiers.</value>
    public virtual ListString ProductIDs
    {
      get
      {
        return new ListString(this.CurrentItem["Products"]);
      }

      set
      {
        using (new EditContext(this.CurrentItem))
        {
          this.CurrentItem["Products"] = value.ToString();
        }
      }
    }

    /// <summary>
    /// Gets the current item.
    /// </summary>
    /// <value>The current item.</value>
    [CanBeNull]
    public Item CurrentItem
    {
      get
      {
        if (this.currentItem == null && this.ItemUri != null)
        {
          this.currentItem = Database.GetItem(this.ItemUri);
          if (this.currentItem == null)
          {
            return null;
          }

          this.currentItem.RuntimeSettings.ReadOnlyStatistics = true;
        }

        return this.currentItem;
      }
    }
  }
}