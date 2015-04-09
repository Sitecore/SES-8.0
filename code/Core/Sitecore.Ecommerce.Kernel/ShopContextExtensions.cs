// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopContextExtensions.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the shop context extensions class.
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

namespace Sitecore.Ecommerce
{
  using Diagnostics;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// Defines the shop context extensions class.
  /// </summary>
  public static class ShopContextExtensions
  {
    /// <summary>
    /// Gets the master currency.
    /// </summary>
    /// <param name="shopContext">The shop context.</param>
    /// <param name="entityMapper">The entity mapper.</param>
    /// <returns>The master currency.</returns>
    public static Currency GetMasterCurrency(this ShopContext shopContext, EntityMapper<Item, IEntity> entityMapper)
    {
      Assert.ArgumentNotNull(shopContext, "shopContext");
      Assert.ArgumentNotNull(entityMapper, "entityMapper");

      if (!ID.IsID(shopContext.GeneralSettings.MasterCurrency))
      {
        return null;
      }

      Item currencyItem = shopContext.Database != null ? shopContext.Database.GetItem(shopContext.GeneralSettings.MasterCurrency) : null;

      if (currencyItem == null)
      {
        return null;
      }

      IEntity result = Context.Entity.Resolve<Currency>() as IEntity;
      entityMapper.Map(currencyItem, result);

      return result as Currency;
    }
  }
}
