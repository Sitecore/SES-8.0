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

namespace Sitecore.Ecommerce.DomainModel.Users
{
  using System;
  using System.Collections.Specialized;
  using System.Linq;
  using Addresses;

  /// <summary>
  /// The customer info virtual class.
  /// </summary>
  [Serializable]
  public class CustomerInfo
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerInfo"/> class.
    /// </summary>
    public CustomerInfo()
    {
      this.BasicProperties = new NameValueCollection();
      this.CustomProperties = new NameValueCollection();
    }

    /// <summary>
    /// Gets or sets the billing address.
    /// </summary>
    /// <value>The billing address.</value>
    public virtual AddressInfo BillingAddress { get; set; }

    /// <summary>
    /// Gets or sets the shipping address.
    /// </summary>
    /// <value>The shipping address.</value>
    public virtual AddressInfo ShippingAddress { get; set; }

    /// <summary>
    /// Gets or sets the name of the nick.
    /// </summary>
    /// <value>The name of the nick.</value>
    public virtual string NickName
    {
      get { return this["Nick Name"]; }
      set { this["Nick Name"] = value; }
    }

    /// <summary>
    /// Gets or sets the email.s
    /// </summary>
    /// <value>The emails.</value>
    public virtual string Email
    {
      get { return this["E-mail"]; }
      set { this["E-mail"] = value; }
    }

    /// <summary>
    /// Gets or sets the email2.
    /// </summary>
    /// <value>The email2.</value>
    public virtual string Email2
    {
      get { return this["E-mail 2"]; }
      set { this["E-mail 2"] = value; }
    }

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    /// <value>The mobile number.</value>
    public virtual string Mobile
    {
      get { return this["Mobile"]; }
      set { this["Mobile"] = value; }
    }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    /// <value>The phone number.</value>
    public virtual string Phone
    {
      get { return this["Phone"]; }
      set { this["Phone"] = value; }
    }

    /// <summary>
    /// Gets or sets the fax number.
    /// </summary>
    /// <value>The fax number.</value>
    public virtual string Fax
    {
      get { return this["Fax"]; }
      set { this["Fax"] = value; }
    }

    #region Identifies the uniq identifier for user

    /// <summary>
    /// Gets or sets the customer id.
    /// </summary>
    /// <value>The customer id.</value>
    public virtual string CustomerId
    {
      get { return this["Customer ID"]; }
      set { this["Customer ID"] = value; }
    }

    /// <summary>
    /// Gets or sets the account id.
    /// </summary>
    /// <value>The account id.</value
    /// >
    public virtual string AccountId
    {
      get { return this["Account ID"]; }
      set { this["Account ID"] = value; }
    }

    /// <summary>
    /// Gets or sets the type of the account.
    /// </summary>
    /// <value>The type of the account.</value>
    public virtual string AccountType
    {
      get { return this["Account Type"]; }
      set { this["Account Type"] = value; }
    }

    #region Custom Properties

    /// <summary>
    /// Gets or sets the collection of the custom properties.
    /// </summary>
    public NameValueCollection CustomProperties { get; set; }

    /// <summary>
    /// Gets or sets the collection of the custom properties.
    /// </summary>
    protected NameValueCollection BasicProperties { get; set; }

    #endregion

    /// <summary>
    /// Allows to access the CustomerInfo properties using string indexes.
    /// </summary>
    /// <param name="key">Index value.</param>
    public virtual string this[string key]
    {
      get
      {
        return (this.BasicProperties.AllKeys.Contains(key) ? this.BasicProperties[key] : this.CustomProperties[key]) ?? string.Empty;
      }

      set
      {
        switch (key)
        {
          case "Account ID":
          case "Account Type":
          case "Customer ID":
          case "Nick Name":
          case "E-mail":
          case "E-mail 2":
          case "Mobile":
          case "Phone":
          case "Fax":
            this.BasicProperties[key] = value;
            break;
          default:
            this.CustomProperties[key] = value;
            break;
        }
      }
    }

    #endregion
  }
}