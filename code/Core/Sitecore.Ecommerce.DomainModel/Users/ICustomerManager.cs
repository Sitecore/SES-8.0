// -------------------------------------------------------------------------------------------
// <copyright file="ICustomerManager.cs" company="Sitecore Corporation">
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
  /// <summary>
  /// The customer provider interface.
  /// </summary>
  /// <typeparam name="TCustomerInfo">The type of the customer info.</typeparam>
  public interface ICustomerManager<TCustomerInfo> where TCustomerInfo : CustomerInfo
  {
    /// <summary>
    /// Gets or sets the current user.
    /// </summary>
    /// <value>The current user.</value>
    TCustomerInfo CurrentUser { get; set; }

    /// <summary>
    /// Resets the current user.
    /// </summary>
    void ResetCurrentUser();

    /// <summary>
    /// Gets the customer info.
    /// </summary>
    /// <param name="nickName">Name of the nick.</param>
    /// <returns>
    /// If user account exists, returns the user info, else returns null.
    /// </returns>
    TCustomerInfo GetCustomerInfo(string nickName);

    /// <summary>
    /// Creates the customer with customer info.
    /// </summary>
    /// <param name="nickName">Name of the nick.</param>
    /// <param name="password">The password.</param>
    /// <param name="email">The email.</param>
    /// <returns>
    /// If customer is created, returns customer, else return null.
    /// </returns>
    TCustomerInfo CreateCustomerAccount(string nickName, string password, string email);

    /// <summary>
    /// Updates the customer info.
    /// </summary>
    /// <param name="customerInfo">The customer info.</param>
    void UpdateCustomerProfile(TCustomerInfo customerInfo);

    /// <summary>
    /// Log in the customer.
    /// </summary>
    /// <param name="nickName">Name of the nick.</param>
    /// <param name="password">The customer password.</param>
    /// <returns>The login result.</returns>
    bool LogInCustomer(string nickName, string password);

    /// <summary>
    /// Logins the customer.
    /// </summary>
    /// <param name="customerInfo">The customer info.</param>
    /// <param name="password">The password.</param>
    /// <returns>The login result.</returns>
    bool LogInCustomer(TCustomerInfo customerInfo, string password);

    /// <summary>
    /// Notifies the user.
    /// </summary>
    /// <param name="mailTemplate">The mail template.</param>
    /// <param name="customerInfo">The customer info.</param>
    void NotifyUser(string mailTemplate, TCustomerInfo customerInfo);

    /// <summary>
    /// Notifies the user.
    /// </summary>
    /// <param name="mailTemplate">The mail template.</param>
    /// <param name="mailParams">The mail params.</param>
    void NotifyUser(string mailTemplate, object mailParams);

    /// <summary>
    /// Deletes the customer.
    /// </summary>
    /// <param name="nickName">Name of the nick.</param>
    void DeleteCustomer(string nickName);

    /// <summary>
    /// Deletes the customer.
    /// </summary>
    /// <param name="customerInfo">The customer info.</param>
    void DeleteCustomer(TCustomerInfo customerInfo);

    /// <summary>
    /// Deletes all customers.
    /// </summary>
    void DeleteAllCustomers();
  }
}