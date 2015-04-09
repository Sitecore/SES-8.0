// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyShoppingCartDtoConvertor.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the MyShoppingCartDtoConvertor class.
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

namespace Sitecore.Ecommerce.Examples.Orders
{
  using System.Data;
  using Data.Convertors;
  using DomainModel.Carts;

  public class MyShoppingCartDtoConvertor : ShoppingCartConvertor
  {
    public override void DomainModelToDTO(ShoppingCart model, ref DataRow row)
    {
      base.DomainModelToDTO(model, ref row);

      row["MyCustomerInfo.Hobby"] = ((MyShoppingCart)model).MyCustomerInfo.Hobby;
    }

    public override void DTOToDomainModel(DataRow row, ref ShoppingCart model)
    {
      base.DTOToDomainModel(row, ref model);

      ((MyShoppingCart)model).MyCustomerInfo.Hobby = row["MyCustomerInfo.Hobby"] as string ?? string.Empty;
    }
  }
}