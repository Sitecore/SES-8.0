// -------------------------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Sitecore Corporation">
//  Copyright (c) Sitecore Corporation 1999-2015 
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

namespace Sitecore.Ecommerce.Users
{
  using System;
  using Data;
  using Diagnostics;
  using DomainModel.Addresses;
  using DomainModel.Data;
  using Validators.Interception;

  /// <summary>
  /// Customer info container.
  /// </summary>
  [Serializable]
  public class CustomerInfo : DomainModel.Users.CustomerInfo, IEntity
  {
    /// <summary>
    /// Gets or sets the billing address.
    /// </summary>
    /// <value>The billing address.</value>
    [Entity(FieldName = "Billing")]
    public override AddressInfo BillingAddress { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the shipping address.
    /// </summary>
    /// <value>The shipping address.</value>
    [Entity(FieldName = "Shipping")]
    public override AddressInfo ShippingAddress { get; [NotNullValue] set; }

    /// <summary>
    /// Gets or sets the customer id.
    /// </summary>
    /// <value>
    /// The customer id.
    /// </value>
    [NotNull, Entity(FieldName = "Customer Id")]
    public override string CustomerId
    {
      get
      {
        return base.CustomerId;
      }

      [NotNullValue]
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.CustomerId = value;
      }
    }

    /// <summary>
    /// Gets or sets the account id.
    /// </summary>
    /// <value>
    /// The account id.
    /// </value>
    [NotNull, Entity(FieldName = "Account Id")]
    public override string AccountId
    {
      get
      {
        return base.AccountId;
      }

      [NotNullValue]
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.AccountId = value;
      }
    }


    /// <summary>
    /// Gets or sets the type of the account.
    /// </summary>
    /// <value>
    /// The type of the account.
    /// </value>
    [NotNull, Entity(FieldName = "Account Type")]
    public override string AccountType
    {
      get
      {
        return base.AccountType;
      }

      [NotNullValue]
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.AccountType = value;
      }
    }

    /// <summary>
    /// Gets or sets the primary email.
    /// </summary>
    /// <value>The primary email.</value>
    [NotNull, Entity(FieldName = "Email")]
    public override string Email
    {
      get
      {
        return base.Email;
      }

      [EmailValue]
      [NotNullValue]
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.Email = value;
      }
    }

    /// <summary>
    /// Gets or sets the seconary email.
    /// </summary>
    /// <value>The seconary email.</value>
    [NotNull, Entity(FieldName = "Email 2")]   
    public override string Email2
    {
      get
      {
        return base.Email2;
      }

      [EmailValue]
      [NotNullValue]
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.Email2 = value;
      }
    }

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    /// <value>The mobile number.</value>
    [NotNull, Entity(FieldName = "Mobile")]
    public override string Mobile
    {
      get
      {
        return base.Mobile;
      }

      [NotNullValue]
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.Mobile = value;
      }
    }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    /// <value>The phone number.</value>
    [NotNull, Entity(FieldName = "Phone")]
    public override string Phone
    {
      get
      {
        return base.Phone;
      }

      [NotNullValue]
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.Phone = value;
      }
    }

    /// <summary>
    /// Gets or sets the fax number.
    /// </summary>
    /// <value>The fax number.</value>
    [NotNull, Entity(FieldName = "Fax")]
    public override string Fax
    {
      get
      {
        return base.Fax;
      }

      [NotNullValue]
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.Fax = value;
      }
    }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    [NotNull, Entity(FieldName = "Nick Name")]
    public override string NickName
    {
      get
      {
        return base.NickName;
      }

      [NotNullValue]
      set
      {
        Assert.ArgumentNotNull(value, "value");
        base.NickName = value;
      }
    }

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    public virtual string Alias { get; [NotNullValue] set; }

    #endregion
  }
}