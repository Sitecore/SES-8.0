// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartLineConvertor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the ShoppingCartLineConvertor type.
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

namespace Sitecore.Ecommerce.Data.Convertors
{
  using System.Collections.Generic;
  using System.Data;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.DomainModel.Carts;
  using Sitecore.Ecommerce.DomainModel.Data;

  /// <summary>
  /// Converts ShoppingCartLine to DataRow and vice-versa
  /// </summary>
  public class ShoppingCartLineConvertor : Convertor<ShoppingCartLine>
  {
    /// <summary>
    /// The totals convertor
    /// </summary>
    private static readonly KeyValuePairConvertor<string, decimal> TotalsConvertor = new KeyValuePairConvertor<string, decimal>();

    /// <summary>
    /// Stores the information from ShoppingCartLine in DataRow
    /// </summary>
    /// <param name="model">
    /// The ShoppingCartLine which information to store
    /// </param>
    /// <param name="row">
    /// The row where to store the information
    /// </param>
    public override void DomainModelToDTO(ShoppingCartLine model, ref DataRow row)
    {
      Assert.IsNotNull(model, "model must not be null");
      Assert.IsNotNull(row, "row must not be null");

      row["FriendlyUrl"] = model.FriendlyUrl;
      row["ImageUrl"] = model.ImageUrl;

      if (model.Product is IEntity)
      {
        row["Product.Alias"] = ((IEntity)model.Product).Alias;
      }

      row["Product.Code"] = model.Product.Code;
      row["Product.EAN"] = model.Product.EAN;
      row["Product.Name"] = model.Product.Name;
      row["Product.SKU"] = model.Product.SKU;
      row["Product.Title"] = model.Product.Title;

      if (model.Product is Products.Product)
      {
        row["Product.Brand"] = ((Products.Product)model.Product).Brand;
        row["Product.Description"] = ((Products.Product)model.Product).Description;
        row["Product.ImageUrl"] = ((Products.Product)model.Product).ImageUrl;
        row["Product.ShortDescription"] = ((Products.Product)model.Product).ShortDescription;
        row["Product.VatType"] = ((Products.Product)model.Product).VatType;
      }

      row["Quantity"] = model.Quantity;
      row["Totals"] = TotalsConvertor.DomainModelToDTO(model.Totals);
    }

    /// <summary>
    /// Restores the ShoppingCartLine from the DataRow
    /// </summary>
    /// <param name="row">
    /// The DataRow where the information is stored.
    /// </param>
    /// <param name="model">
    /// The ShoppingCartLine object to restore.
    /// </param>
    public override void DTOToDomainModel(DataRow row, ref ShoppingCartLine model)
    {
      Assert.IsNotNull(model, "model must not be null");
      Assert.IsNotNull(row, "row must not be null");

      model.FriendlyUrl = this.ConvertDataRowValue<string>(row["FriendlyUrl"]);
      model.ImageUrl = this.ConvertDataRowValue<string>(row["ImageUrl"]);

      if (model.Product is IEntity)
      {
        ((IEntity)model.Product).Alias = this.ConvertDataRowValue<string>(row["Product.Alias"]);
      }

      model.Product.Code = this.ConvertDataRowValue<string>(row["Product.Code"]);
      model.Product.EAN = this.ConvertDataRowValue<string>(row["Product.EAN"]);
      model.Product.Name = this.ConvertDataRowValue<string>(row["Product.Name"]);
      model.Product.SKU = this.ConvertDataRowValue<string>(row["Product.SKU"]);
      model.Product.Title = this.ConvertDataRowValue<string>(row["Product.Title"]);

      if (model.Product is Products.Product)
      {
        ((Products.Product)model.Product).Brand = this.ConvertDataRowValue<string>(row["Product.Brand"]);
        ((Products.Product)model.Product).Description = this.ConvertDataRowValue<string>(row["Product.Description"]);
        ((Products.Product)model.Product).ImageUrl = this.ConvertDataRowValue<string>(row["Product.ImageUrl"]);
        ((Products.Product)model.Product).ShortDescription = this.ConvertDataRowValue<string>(row["Product.ShortDescription"]);
        ((Products.Product)model.Product).VatType = this.ConvertDataRowValue<string>(row["Product.VatType"]);
      }

      model.Quantity = this.ConvertDataRowValue<uint>(row["Quantity"]);

      DataTable pairsDataTable = this.ConvertDataRowValue<DataTable>(row["Totals"]);
      if (pairsDataTable != null)
      {
        foreach (KeyValuePair<string, decimal> pair in TotalsConvertor.DTOToDomainModel(pairsDataTable))
        {
          if (model.Totals.ContainsKey(pair.Key))
          {
            model.Totals[pair.Key] = pair.Value;
          }
          else
          {
            model.Totals.Add(pair);
          }
        }
      }
    }

    /// <summary>
    /// Gets an instance of the ShoppingCartLine class
    /// </summary>
    /// <returns>
    /// New instance of the ShoppingCartLine class
    /// </returns>
    public override ShoppingCartLine GetInstance()
    {
      return Context.Entity.Resolve<ShoppingCartLine>();
    }
  }
}