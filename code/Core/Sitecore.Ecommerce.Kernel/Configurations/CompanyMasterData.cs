// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyMasterData.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the CompanyMasterData type.
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

namespace Sitecore.Ecommerce.Configurations
{
  using Data.Mapping;
  using Diagnostics;
  using DomainModel.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// Defines the company master data class.
  /// </summary>
  public class CompanyMasterData : IEntity
  {
    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public string Alias { get; set; }

    /// <summary>
    /// Gets or sets the name of the company.
    /// </summary>
    /// <value>The name of the company.</value>
    public virtual string CompanyName { get; set; }

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    /// <value>The address.</value>
    public virtual string Address { get; set; }

    /// <summary>
    /// Gets or sets the address2.
    /// </summary>
    /// <value>The address2.</value>
    public virtual string Address2 { get; set; }

    /// <summary>
    /// Gets or sets the zip.
    /// </summary>
    /// <value>The zip.</value>
    public virtual string Zip { get; set; }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    /// <value>The state.</value>
    public virtual string State { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    public virtual string City { get; set; }

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    /// <value>The country.</value>
    public virtual string Country { get; set; }

    /// <summary>
    /// Gets or sets the company number.
    /// </summary>
    /// <value>The company number.</value>
    public virtual string CompanyNumber { get; set; }

    /// <summary>
    /// Gets or sets the E mail.
    /// </summary>
    /// <value>The E mail.</value>
    public virtual string Email { get; set; }

    /// <summary>
    /// Gets or sets the post box.
    /// </summary>
    /// <value>The post box.</value>
    public virtual string PostBox { get; set; }

    /// <summary>
    /// Gets or sets the fax.
    /// </summary>
    /// <value>The fax.</value>
    public virtual string Fax { get; set; }

    /// <summary>
    /// Gets or sets the website.
    /// </summary>
    /// <value>The website.</value>
    public virtual string Website { get; set; }

    /// <summary>
    /// Gets or sets the logo URL.
    /// </summary>
    /// <value>The logo URL.</value>
    public virtual string LogoUrl { get; set; }

    /// <summary>
    /// Reads the data.
    /// </summary>
    public virtual void ReadData()
    {
      ShopContext context = Context.Entity.GetConfiguration<ShopContext>();
      Item configurationItem = context.Database.GetItem(context.BusinessCatalogSettings.CompanyMasterDataLink);

      ItemToEntityMapper entityMapper = Context.Entity.Resolve<ItemToEntityMapper>();

      if (configurationItem != null)
      {
        entityMapper.Map(configurationItem, this);
      }
    }
  }
}