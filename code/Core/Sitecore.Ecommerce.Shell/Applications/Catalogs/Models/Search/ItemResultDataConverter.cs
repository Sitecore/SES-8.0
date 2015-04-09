// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemResultDataConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Converts item collection to the result data grid.
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

namespace Sitecore.Ecommerce.Shell.Applications.Catalogs.Models.Search
{
  using System;
  using System.Collections.Generic;
  using Diagnostics;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using DomainModel.Prices;
  using DomainModel.Products;
  using Sitecore.Data.Items;

  /// <summary>
  /// Converts item collection to the result data grid.
  /// </summary>
  public class ItemResultDataConverter : IResultDataConverter<IEnumerable<Item>>
  {
    /// <summary>
    /// Converts the specified initial result.
    /// </summary>
    /// <param name="initialResult">The initial result.</param>
    /// <param name="columns">The columns.</param>
    /// <returns>
    /// The data converted to the GridData format.
    /// </returns>
    public GridData Convert(IEnumerable<Item> initialResult, List<GridColumn> columns)
    {
      var data = new GridData();
      foreach (var item in initialResult)
      {
        var row = new List<string>(columns.Count);
        foreach (var column in columns)
        {
          if (column.RowID)
          {
            row.Add(item.ID.ToString());
            continue;
          }

          if (string.IsNullOrEmpty(column.FieldName))
          {
            row.Add(string.Empty);
            continue;
          }

          var field = item.Fields[column.FieldName];

          var key = field != null ? field.TypeKey : string.Empty;
          var value = field != null ? field.Value : string.Empty;

          switch (key)
          {
            case "datetime":
              {
                row.Add(DateUtil.FormatIsoDate(value, column.FormatString ?? string.Empty));
                break;
              }

            case "droplink":
            case "droptree":
              {
                var link = item.Database.GetItem(value);
                row.Add(link != null ? link.DisplayName : string.Empty);
                break;
              }

            case "price-matrix":
              {
                var price = string.Empty;

                var productProvider = Context.Entity.Resolve<IProductRepository>();
                var product = productProvider.Get<ProductBaseData>(item["Product Code"]);
                if (product != null)
                {
                  var provider = Context.Entity.Resolve<IEntityProvider<Currency>>();
                  var productPriceManager = Context.Entity.Resolve<IProductPriceManager>();
                  var productPricesList = productPriceManager.GetProductTotals<Totals, ProductBaseData, Currency>(product, provider.GetDefault());

                  try
                  {
                    price = Utils.MainUtil.FormatPrice(productPricesList.PriceExVat, true);
                  }
                  catch (Exception exception)
                  {
                    Log.Error(exception.Message, exception);
                    price = "-";
                  }
                }
                
                row.Add(price);
                break;
              }

            default:
              {
                row.Add(value);
                break;
              }
          }
        }

        data.Rows.Add(row);
      }

      return data;
    }
  }
}