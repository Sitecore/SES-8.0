// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupplierParty.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the SupplierParty class.
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

namespace Sitecore.Ecommerce.OrderManagement.Orders
{
  using Common;

  public class SupplierParty : IEntity
  {
    /// <summary>
    /// ID of the account for the customer
    /// </summary>
    public virtual string CustomerAssignedAccountID { get; set; }

    /// <summary>
    /// ID of the supplier account from third party
    /// </summary>
    public virtual string AddtitionalAccountID { get; set; }

    /// <summary>
    /// Information about the party
    /// </summary>
    public virtual Party Party { get; set; }

    /// <summary>
    /// Information about the Shipping contact
    /// </summary>
    public virtual Contact ShippingContact { get; set; }

    /// <summary>
    /// Information about the accounting contact of the Supplier
    /// </summary>
    public virtual Contact AccountingContact { get; set; }

    /// <summary>
    /// Information about the seller
    /// </summary>
    public virtual Contact OrderContact { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
