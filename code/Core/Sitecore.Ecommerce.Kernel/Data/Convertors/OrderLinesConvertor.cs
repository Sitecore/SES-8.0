// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderLinesConvertor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderLinesConvertor type.
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
  using Sitecore.Ecommerce.DomainModel.Data;
  using Sitecore.Ecommerce.DomainModel.Orders;

  /// <summary>
  /// Converts an OrderLine object to DataRow and vice-versa
  /// </summary>
  public class OrderLinesConvertor : Convertor<OrderLine>
  {
    /// <summary>
    /// The totals convertor
    /// </summary>
    private static readonly KeyValuePairConvertor<string, decimal> TotalsConvertor = new KeyValuePairConvertor<string, decimal>();

    /// <summary>
    /// Converts OrderLine to DataRow
    /// </summary>
    /// <param name="model">
    /// The OrderLine to convert.
    /// </param>
    /// <param name="row">
    /// The row to store the information.
    /// </param>
    public override void DomainModelToDTO(OrderLine model, ref DataRow row)
    {
      Assert.IsNotNull(model, "model must not be null");
      Assert.IsNotNull(row, "row must not be null");

      row["FriendlyUrl"] = model.FriendlyUrl;
      row["Id"] = model.Id;
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
      row["Type"] = model.Type;
    }

    /// <summary>
    /// Restores the content of the OrderLine from the DataRow
    /// </summary>
    /// <param name="row">
    /// The row to retrieve the information from
    /// </param>
    /// <param name="model">
    /// The model to restore
    /// </param>
    public override void DTOToDomainModel(DataRow row, ref OrderLine model)
    {
      Assert.IsNotNull(model, "model must not be null");
      Assert.IsNotNull(row, "row must not be null");

      model.FriendlyUrl = this.ConvertDataRowValue<string>(row["FriendlyUrl"]);
      model.Id = this.ConvertDataRowValue<string>(row["Id"]);
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

      model.Type = this.ConvertDataRowValue<string>(row["Type"]);
    }

    /// <summary>
    /// Creates an instance of the OrderLine
    /// </summary>
    /// <returns>
    /// An instance of the class
    /// </returns>
    public override OrderLine GetInstance()
    {
      return Context.Entity.Resolve<OrderLine>();
    }
  }
}
